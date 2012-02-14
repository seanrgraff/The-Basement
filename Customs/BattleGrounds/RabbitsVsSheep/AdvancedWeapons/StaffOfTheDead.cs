using System;
using Server.Network;
using Server.Items;
using Server.Mobiles;
using System.Collections;
using System.Collections.Generic;

namespace Server.RabbitsVsSheep
{
	[FlipableAttribute( 0xDF1, 0xDF0 )]
	public class StaffOfTheDead : BaseStaff
	{
		public override WeaponAbility PrimaryAbility{ get{ return WeaponAbility.WhirlwindAttack; } }
		public override WeaponAbility SecondaryAbility{ get{ return WeaponAbility.ParalyzingBlow; } }

		public override int AosStrengthReq{ get{ return 35; } }
		public override int AosMinDamage{ get{ return 13; } }
		public override int AosMaxDamage{ get{ return 16; } }
		public override int AosSpeed{ get{ return 39; } }

		public override int OldStrengthReq{ get{ return 35; } }
		public override int OldMinDamage{ get{ return 8; } }
		public override int OldMaxDamage{ get{ return 33; } }
		public override int OldSpeed{ get{ return 35; } }

		public override int InitMinHits{ get{ return 31; } }
		public override int InitMaxHits{ get{ return 70; } }
		
		public static ArrayList PlayerTimers = new ArrayList();

		[Constructable]
		public StaffOfTheDead() : base( 0xDF0 )
		{
			Name = "Staff of the Dead";
			Weight = 6.0;
		}
		
		public override void OnHit( Mobile attacker, Mobile defender, double damagebonus )
		{	
			bool found = false;
           	if ( PlayerTimers.Count != 0 )
           	{
           		int numberOfTimers = 0;
				foreach ( AnimateDeadTimer t in PlayerTimers ) 
           		{
           			if ( t != null && t.m_Defender == defender )
           			{
						found = true;
           			}
           			if ( t.count == 30 )
           			{
           				numberOfTimers++;
           			}
           		}
           		
           		if ( numberOfTimers == PlayerTimers.Count )
           		{
           			PlayerTimers.Clear();
           			found = false;
           		}
			}
			if ( found == false )
			{
				AnimateDeadTimer timer = new AnimateDeadTimer( defender, attacker );
				PlayerTimers.Add(timer);
				timer.Start();
			}

			base.OnHit(attacker,defender,damagebonus);
		}

		public StaffOfTheDead( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();
		}
	}
	    
	public class AnimateDeadTimer : Timer
    {
        public Mobile m_Defender;
        public Mobile m_Attacker;
        public int count = 0;
        
        public int AnimateChance = 20;

        public AnimateDeadTimer(Mobile Defender, Mobile Attacker)
                : base(TimeSpan.FromSeconds(1.0), TimeSpan.FromSeconds(1.0))
        {
            Priority = TimerPriority.FiftyMS;
            m_Defender = Defender;
            m_Attacker = Attacker;
        }

        protected override void OnTick()
        {
        	count++;
            // heals the player
            if ( m_Defender is BaseCreature && m_Defender.Hits <= 0 && m_Defender.Alive == false )
            {
				AnimateDefender( m_Defender, m_Attacker );
				this.Stop();
            }
            else if ( m_Defender.Deleted )
            {
            	this.Stop();
            }
            else if ( count == 30 )
            {
            	this.Stop();
            }
        }
        
        public void AnimateDefender( Mobile defender, Mobile attacker )
		{
			int ChanceWheel = Utility.Random( 100 );

			if ( defender is BaseCreature && defender.Corpse != null && ChanceWheel <= AnimateChance)
			{
				attacker.SendMessage("Your staff glows with dark power!");
			
				Corpse c = (Corpse)defender.Corpse;
				
				if ( c == null || c.Deleted )
				{
					attacker.SendLocalizedMessage( 1061084 ); // You cannot animate that.
				}
				
				Point3D p = c.GetWorldLocation();
				Map map = c.Map;

				if ( map != null )
				{
					Effects.PlaySound( p, map, 0x1FB );
					Effects.SendLocationParticles( EffectItem.Create( p, map, EffectItem.DefaultDuration ), 0x3789, 1, 40, 0x3F, 3, 9907, 0 );

					Timer.DelayCall( TimeSpan.FromSeconds( 2.0 ), new TimerStateCallback( SummonDelay_Callback ), new object[]{ attacker, c, p, map } );
				}
			}
			else
			{
				this.Stop();
			}
		}
		
		private static void SummonDelay_Callback( object state )
		{
			object[] states = (object[])state;

			Mobile caster = (Mobile)states[0];
			Corpse corpse = (Corpse)states[1];
			Point3D loc = (Point3D)states[2];
			Map map = (Map)states[3];

			if ( corpse.ItemID != 0x2006 )
				return;

			Mobile owner = corpse.Owner;

			if ( owner == null )
				return;

			//Make toSummon some random undead monster!
			Type toSummon = null;
			
			int ChanceWheel = Utility.Random( 100 );
			
			if ( ChanceWheel > 80 )
			{
				toSummon = typeof( Lich );
			}
			else if ( ChanceWheel > 60 )
			{
				toSummon = typeof( SkeletalKnight );
			}
			else if ( ChanceWheel > 40 )
			{
				toSummon = typeof( SkeletalMage );
			}
			else if ( ChanceWheel > 20 )
			{
				toSummon = typeof( Skeleton );
			}
			else
			{
				toSummon = typeof( Zombie );
			}

			if ( toSummon == null )
				return;

			Mobile summoned = null;

			try{ summoned = Activator.CreateInstance( toSummon ) as Mobile; }
			catch{}

			if ( summoned == null )
				return;

			if ( summoned is BaseCreature )
			{
				BaseCreature bc = (BaseCreature)summoned;

				// to be sure
				bc.Tamable = false;

				if ( bc is BaseMount )
					bc.ControlSlots = 1;
				else
					bc.ControlSlots = 0;

				Effects.PlaySound( loc, map, bc.GetAngerSound() );

				BaseCreature.Summon( (BaseCreature)summoned, false, caster, loc, 0x28, TimeSpan.FromDays( 1.0 ) );
			}

			summoned.Fame = 0;
			summoned.Karma = -1500;

			summoned.MoveToWorld( loc, map );
			
			Timer.DelayCall( TimeSpan.FromSeconds( 2.0 ), TimeSpan.FromSeconds( 2.0 ), new TimerStateCallback( Summoned_Damage ), summoned );

			corpse.ProcessDelta();
			corpse.SendRemovePacket();
			corpse.ItemID = Utility.Random( 0xECA, 9 ); // bone graphic
			corpse.Hue = 132;
			corpse.ProcessDelta();
		}
		
		private static void Summoned_Damage( object state )
		{
			Mobile mob = (Mobile)state;

			if ( mob.Hits > 0 )
				--mob.Hits;
			else
				mob.Kill();
		}
     }
}