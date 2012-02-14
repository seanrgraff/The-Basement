using System;
using Server.Network;
using Server.Items;
using Server.Spells;

namespace Server.RabbitsVsSheep
{
	[FlipableAttribute( 0xF62, 0xF63 )]
	public class SearingSpear : BaseSpear
	{
		public override WeaponAbility PrimaryAbility{ get{ return WeaponAbility.ArmorIgnore; } }
		public override WeaponAbility SecondaryAbility{ get{ return WeaponAbility.ParalyzingBlow; } }

		public override int AosStrengthReq{ get{ return 50; } }
		public override int AosMinDamage{ get{ return 13; } }
		public override int AosMaxDamage{ get{ return 15; } }
		public override int AosSpeed{ get{ return 42; } }

		public override int OldStrengthReq{ get{ return 30; } }
		public override int OldMinDamage{ get{ return 2; } }
		public override int OldMaxDamage{ get{ return 36; } }
		public override int OldSpeed{ get{ return 46; } }

		public override int InitMinHits{ get{ return 31; } }
		public override int InitMaxHits{ get{ return 80; } }

		public int flamestrikeChance = 5;
		public int fireChance = 10;
		public int fireballChance = 50;

		[Constructable]
		public SearingSpear() : base( 0xF62 )
		{
			Name = "Searing Spear";
			Hue = 1260;
			Weight = 7.0;
		}
		
		public override void OnHit( Mobile attacker, Mobile defender, double damagebonus )
		{
			int ChanceWheel = Utility.Random( 100 );
			
			if ( flamestrikeChance != 0 && flamestrikeChance > ChanceWheel )
				DoFlamestrike( attacker, defender );
			
			if ( fireballChance != 0 && fireballChance > Utility.Random( 100 ) )
				DoFireball( attacker, defender );
				
			if ( fireChance != 0 && fireChance > Utility.Random( 100 ) )
				DoAreaAttack( attacker, defender, 0x11D, 1160, 0, 100, 0, 0, 0 );
				
			base.OnHit(attacker,defender,damagebonus);
		}
		
		public void DoFlamestrike( Mobile attacker, Mobile defender )
		{
			if ( !attacker.CanBeHarmful( defender, false ) )
				return;

			attacker.DoHarmful( defender );

			double damage = GetAosDamage( attacker, 48, 1, 5 );
			
			defender.FixedParticles( 0x3709, 10, 30, 5052, EffectLayer.LeftFoot );
			defender.PlaySound( 0x208 );

			SpellHelper.Damage( TimeSpan.FromSeconds( 1.0 ), defender, attacker, damage, 0, 100, 0, 0, 0 );
		}

		public SearingSpear( Serial serial ) : base( serial )
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