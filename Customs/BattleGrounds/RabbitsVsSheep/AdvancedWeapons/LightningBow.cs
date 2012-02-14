using System;
using Server.Network;
using Server.Items;
using Server.Targeting;
using Server.Regions;


namespace Server.RabbitsVsSheep
{
	[FlipableAttribute( 0x13B2, 0x13B1 )]
	public class LightningBow : BaseRanged
	{
		public override int EffectID{ get{ return 0xF42; } }
		public override Type AmmoType{ get{ return typeof( Arrow ); } }
		public override Item Ammo{ get{ return new Arrow(); } }

		public override WeaponAbility PrimaryAbility{ get{ return WeaponAbility.ParalyzingBlow; } }
		public override WeaponAbility SecondaryAbility{ get{ return WeaponAbility.MortalStrike; } }

		public int bowSpeed = 25;

		public override int AosStrengthReq{ get{ return 30; } }
		public override int AosMinDamage{ get{ return 16; } }
		public override int AosMaxDamage{ get{ return 18; } }
		public override int AosSpeed{ get{ return bowSpeed; } }

		public override int OldStrengthReq{ get{ return 20; } }
		public override int OldMinDamage{ get{ return 9; } }
		public override int OldMaxDamage{ get{ return 41; } }
		public override int OldSpeed{ get{ return bowSpeed; } }

		public override int DefMaxRange{ get{ return 10; } }

		public override int InitMinHits{ get{ return 31; } }
		public override int InitMaxHits{ get{ return 60; } }

		public override WeaponAnimation DefAnimation{ get{ return WeaponAnimation.ShootBow; } }

		public int lightningChance = 70;
		public int doubleShotChance = 20;
		
		public int Charges = 1;

		[Constructable]
		public LightningBow() : base( 0x13B2 )
		{
			Name = "Lightning Bow";
			Hue = 2038;
			Weight = 6.0;
			Layer = Layer.TwoHanded;
		}
		
		public override void OnHit( Mobile attacker, Mobile defender, double damagebonus )
		{
			if (bowSpeed > 25)
				bowSpeed = 25;
		
			int ChanceWheel = Utility.Random( 100 );
			
			if ( lightningChance != 0 && lightningChance > ChanceWheel )
				DoLightning( attacker, defender );
				
			if ( doubleShotChance != 0 && doubleShotChance > ChanceWheel )
				DoDoubleShot( attacker, defender );
				
			base.OnHit(attacker,defender,damagebonus);
		}
		
		public virtual void DoDoubleShot( Mobile attacker, Mobile defender )
		{
			if ( !attacker.CanBeHarmful( defender, false ) )
				return;

			bowSpeed = 350;
			attacker.SendMessage( "The bow strikes again with lightning speed!" );
		}
		
		public override void OnDoubleClick( Mobile from )
		{
			if ( !from.CanBeginAction( typeof( BaseWand ) ) )
				return;
		
			if ( Parent == from )
			{
				if ( Charges > 0 )
				{
					from.Target = new LightningBowTarget( from, this );
					from.SendMessage( "Where do you wish to teleport?" );
				}
				else
					from.SendLocalizedMessage( 1019073 ); // This item is out of charges.
			}
			else
			{
				from.SendLocalizedMessage( 502641 ); // You must equip this item to use it.
			}
		}
		
		public void Teleport( IPoint3D p, Mobile Caster )
		{
			IPoint3D orig = p;
			Map map = Caster.Map;

			if ( Factions.Sigil.ExistsOn( Caster ) )
			{
				Caster.SendLocalizedMessage( 1061632 ); // You can't do that while carrying the sigil.
			}
			else if ( Server.Misc.WeightOverloading.IsOverloaded( Caster ) )
			{
				Caster.SendLocalizedMessage( 502359, "", 0x22 ); // Thou art too encumbered to move.
			}
			else if ( map == null || !map.CanSpawnMobile( p.X, p.Y, p.Z ) )
			{
				Caster.SendLocalizedMessage( 501942 ); // That location is blocked.
			}
			else if ( !Caster.Alive )
			{
				Caster.SendMessage( "You are dead and cannot do that!" );
			}
			else if ( !Caster.CanSee( orig ) )
			{
				Caster.SendMessage( "You cannot see there!" );
			}
			else
			{
				Mobile m = Caster;

				Point3D from = m.Location;
				Point3D to = new Point3D( p );

				m.Location = to;
				m.ProcessDelta();

				if ( m.Player )
				{
					Effects.SendLocationParticles( EffectItem.Create( from, m.Map, EffectItem.DefaultDuration ), 0x3728, 10, 10, 2023 );
					Effects.SendLocationParticles( EffectItem.Create(   to, m.Map, EffectItem.DefaultDuration ), 0x3728, 10, 10, 5023 );
				}
				else
				{
					m.FixedParticles( 0x376A, 9, 32, 0x13AF, EffectLayer.Waist );
				}

				m.PlaySound( 0x1FE );
			}
		}

		public LightningBow( Serial serial ) : base( serial )
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

			if ( Weight == 7.0 )
				Weight = 6.0;
		}
	}
	
	public class LightningBowTarget : Target
    {
           	private Mobile t_attacker;
           	private LightningBow t_bow;

          	public LightningBowTarget( Mobile attacker, LightningBow bow ) : base( 12, false, TargetFlags.None )
           	{
           		t_attacker = attacker;
           		t_bow = bow;
            }

            protected override void OnTarget( Mobile from, object targeted )
            {
            	IPoint3D p = targeted as IPoint3D;

            	if ( t_bow.Charges > 0 )
            	{
						from.Hidden = false;
						t_bow.Charges--;
						t_bow.Hue = 2028;
						t_bow.Teleport( p, t_attacker );
						TeleportRechargeTimer t = new TeleportRechargeTimer( from, t_bow );
						t.Start();
				}
				else
				{
					from.SendMessage( "You do not have any more charges!" );
				}
            }
    }
	
	public class TeleportRechargeTimer : Timer
    {
        public Mobile m_Attacker;
        public int chargeWaitLength = 15;
        public int count = 0;
        public LightningBow t_bow;
        
        public TeleportRechargeTimer( Mobile Attacker, LightningBow bow )
                : base(TimeSpan.FromSeconds(1.0), TimeSpan.FromSeconds(1.0))
        {
            Priority = TimerPriority.FiftyMS;
            m_Attacker = Attacker;
            t_bow = bow;
        }

        protected override void OnTick()
        {
        	count++;

            if ( count == chargeWaitLength )
            {
            	t_bow.Charges++;
            	t_bow.Hue = 2038;
            	m_Attacker.SendMessage( "Your bow is recharged!" );
            	this.Stop();
            }
            else if ( t_bow.Charges >= 1 )
            {
            	this.Stop();
            }
        }
    }
}