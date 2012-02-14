using System;
using System.Collections.Generic;
using Server.Network;
using Server.Items;
using Server.Targeting;
using Server.Spells;
using Server.Spells.Seventh;

namespace Server.RabbitsVsSheep
{
	[FlipableAttribute( 0xF5C, 0xF5D )]
	public class MeteorMace : BaseBashing
	{
		public override WeaponAbility PrimaryAbility{ get{ return WeaponAbility.ConcussionBlow; } }
		public override WeaponAbility SecondaryAbility{ get{ return WeaponAbility.Disarm; } }

		public override int AosStrengthReq{ get{ return 45; } }
		public override int AosMinDamage{ get{ return 12; } }
		public override int AosMaxDamage{ get{ return 14; } }
		public override int AosSpeed{ get{ return 40; } }

		public override int OldStrengthReq{ get{ return 20; } }
		public override int OldMinDamage{ get{ return 8; } }
		public override int OldMaxDamage{ get{ return 32; } }
		public override int OldSpeed{ get{ return 30; } }

		public override int InitMinHits{ get{ return 31; } }
		public override int InitMaxHits{ get{ return 70; } }

		public int meteorChance = 30;
		public int Charges = 1;

		[Constructable]
		public MeteorMace() : base( 0xF5C )
		{
			Name = "Meteor Mace";
			Weight = 14.0;
			Hue = 2117;
		}
		
		public override void OnHit( Mobile attacker, Mobile defender, double damagebonus )
		{
			int ChanceWheel = Utility.Random( 100 );
			
			if ( meteorChance != 0 && meteorChance > ChanceWheel )
				DoMeteorSwarm( attacker, defender );
				
			base.OnHit(attacker,defender,damagebonus);
		}
		
		public void DoMeteorSwarm( Mobile attacker, Mobile defender )
		{
			if ( !attacker.CanBeHarmful( defender, false ) )
				return;

			IPoint3D p = defender.Location;

			if ( !attacker.CanSee( p ) )
			{
				attacker.SendLocalizedMessage( 500237 ); // Target can not be seen.
			}
			else if ( SpellHelper.CheckTown( p, attacker ) )
			{
				SpellHelper.Turn( attacker, p );

				if ( p is Item )
					p = ((Item)p).GetWorldLocation();

				List<Mobile> targets = new List<Mobile>();

				Map map = attacker.Map;

				bool playerVsPlayer = false;

				if ( map != null )
				{
					IPooledEnumerable eable = map.GetMobilesInRange( new Point3D( p ), 2 );

					foreach ( Mobile m in eable )
					{
						if ( attacker != m && SpellHelper.ValidIndirectTarget( attacker, m ) && attacker.CanBeHarmful( m, false ) )
						{
							if ( Core.AOS && !attacker.InLOS( m ) )
								continue;

							targets.Add( m );

							if ( m.Player )
								playerVsPlayer = true;
						}
					}

					eable.Free();
				}

				double damage;

				damage = Utility.Random( 37, 33 );

				if ( targets.Count > 0 )
				{
					Effects.PlaySound( p, attacker.Map, 0x160 );

					if ( targets.Count > 2 )
						damage = (damage * 2) / targets.Count;

					for ( int i = 0; i < targets.Count; ++i )
					{
						Mobile m = targets[i];

						double toDeal = damage;

						attacker.DoHarmful( m );
						SpellHelper.Damage( TimeSpan.FromSeconds( 1.0 ), m, attacker, toDeal, 0, 100, 0, 0, 0 );

						attacker.MovingParticles( m, 0x36D4, 7, 0, false, true, 9501, 1, 0, 0x100 );
					}
				}
				SpellHelper.Damage( TimeSpan.FromSeconds( 1.0 ), defender, attacker, 1, 0, 100, 0, 0, 0 );
			}
		}
		
		public override void OnDoubleClick( Mobile from )
		{
			if ( !from.CanBeginAction( typeof( BaseWand ) ) )
				return;
		
			if ( Parent == from )
			{
				if ( Charges > 0 )
				{
					from.Target = new MeteorMaceTarget( from, this );
					from.SendMessage( "Where do you wish to cast meteor swarm?" );
				}
				else
					from.SendLocalizedMessage( 1019073 ); // This item is out of charges.
			}
			else
			{
				from.SendLocalizedMessage( 502641 ); // You must equip this item to use it.
			}
		}

		public MeteorMace( Serial serial ) : base( serial )
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
	
	 public class MeteorMaceTarget : Target
     {
            private Mobile t_attacker;
            private MeteorMace t_mace;
            private Mobile t_defender;

            public MeteorMaceTarget( Mobile attacker, MeteorMace mace ) : base( 12, false, TargetFlags.None )
            {
            	t_attacker = attacker;
            	t_mace = mace;
            }

            protected override void OnTarget( Mobile from, object targeted )
            {
            	if ( t_mace.Charges > 0 )
            	{
					if ( targeted is Mobile )
					{
						from.Hidden = false;
						t_mace.Charges--;
						t_mace.Hue = 2115;
						t_mace.DoMeteorSwarm( from, (Mobile)targeted );
						MeteorRechargeTimer t = new MeteorRechargeTimer( from, t_mace );
						t.Start();
					}
					else
					{
						from.SendMessage( "That is not a valid target!" );
					}
				}
				else
				{
					from.SendMessage( "You do not have any more charges!" );
				}
            }
     }

	public class MeteorRechargeTimer : Timer
    {
        public Mobile m_Attacker;
        public int count = 0;
        public MeteorMace t_mace;
        
        public MeteorRechargeTimer( Mobile Attacker, MeteorMace mace )
                : base(TimeSpan.FromSeconds(1.0), TimeSpan.FromSeconds(1.0))
        {
            Priority = TimerPriority.FiftyMS;
            m_Attacker = Attacker;
            t_mace = mace;
        }

        protected override void OnTick()
        {
        	count++;

            if ( count == 60 )
            {
            	t_mace.Charges++;
            	t_mace.Hue = 2117;
            	m_Attacker.SendMessage( "Your mace is recharged!" );
            	this.Stop();
            }
            else if ( t_mace.Charges >= 1 )
            {
            	this.Stop();
            }
        }
    }
}