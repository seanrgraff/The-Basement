using System;
using Server.Network;
using Server.Items;

namespace Server.RabbitsVsSheep
{
	[FlipableAttribute( 0x13FF, 0x13FE )]
	public class SwiftKatana : BaseSword
	{
		public override WeaponAbility PrimaryAbility{ get{ return WeaponAbility.DoubleStrike; } }
		public override WeaponAbility SecondaryAbility{ get{ return WeaponAbility.ArmorIgnore; } }

		public int SwiftSpeed = 58;

		public override int AosStrengthReq{ get{ return 25; } }
		public override int AosMinDamage{ get{ return 11; } }
		public override int AosMaxDamage{ get{ return 13; } }
		public override int AosSpeed{ get{ return SwiftSpeed; } }

		public override int OldStrengthReq{ get{ return 10; } }
		public override int OldMinDamage{ get{ return 5; } }
		public override int OldMaxDamage{ get{ return 26; } }
		public override int OldSpeed{ get{ return SwiftSpeed; } }

		public override int DefHitSound{ get{ return 0x23B; } }
		public override int DefMissSound{ get{ return 0x23A; } }

		public override int InitMinHits{ get{ return 31; } }
		public override int InitMaxHits{ get{ return 90; } }

		[Constructable]
		public SwiftKatana() : base( 0x13FF )
		{
			Name = "Swift Katana";
			Hue = 1301;
			Weight = 6.0;
		}

		public SwiftKatana( Serial serial ) : base( serial )
		{
		}
		
		public override void OnHit( Mobile attacker, Mobile defender, double damagebonus )
		{
			IncreaseStrikeSpeed();
			base.OnHit(attacker,defender,damagebonus);
		}
		
		public override void OnMiss( Mobile attacker, Mobile defender )
		{
			count = 0;
			SwiftSpeed = 58;
			base.OnMiss(attacker, defender);
		}
		
		public int count = 0;
		
		public void IncreaseStrikeSpeed()
		{
			count++;
			if(count >= 5)
			{
				count = 0;
				SwiftSpeed = 58;
			}
			else if (count == 1)
			{
				SwiftSpeed = 250;
			}
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