using System;
using Server.Network;
using Server.Items;
using Server.Mobiles;

namespace Server.RabbitsVsSheep
{
	[FlipableAttribute( 0x13FD, 0x13FC )]
	public class CrossbowOfDeath : BaseRanged
	{
		public override int EffectID{ get{ return 0x1BFE; } }
		public override Type AmmoType{ get{ return typeof( Bolt ); } }
		public override Item Ammo{ get{ return new Bolt(); } }

		public override WeaponAbility PrimaryAbility{ get{ return WeaponAbility.MovingShot; } }
		public override WeaponAbility SecondaryAbility{ get{ return WeaponAbility.Dismount; } }

		public override int AosStrengthReq{ get{ return 80; } }
		public override int AosMinDamage{ get{ return 1; } }
		public override int AosMaxDamage{ get{ return 1; } }
		public override int AosSpeed{ get{ return 22; } }

		public override int OldStrengthReq{ get{ return 40; } }
		public override int OldMinDamage{ get{ return 1; } }
		public override int OldMaxDamage{ get{ return 1; } }
		public override int OldSpeed{ get{ return 10; } }

		public override int DefMaxRange{ get{ return 8; } }

		public override int InitMinHits{ get{ return 31; } }
		public override int InitMaxHits{ get{ return 100; } }

		[Constructable]
		public CrossbowOfDeath() : base( 0x13FD )
		{
			Name = "Crossbow of Death";
			Hue = 196;
			Weight = 9.0;
			Layer = Layer.TwoHanded;
			AccuracyLevel = WeaponAccuracyLevel.Supremely;
		}
		
		public override void OnHit( Mobile attacker, Mobile defender, double damagebonus )
		{
			if ( defender is PlayerMobile )
			{
				if ( defender.Hits > 6 )
				{
					defender.Hits = 6;
					defender.SendMessage( "The horrable clutches of death overtake you!" );
				}
			}
			else
			{
				defender.Hits = -1;
			}
			
			base.OnHit(attacker,defender,damagebonus);
		}

		public CrossbowOfDeath( Serial serial ) : base( serial )
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
}