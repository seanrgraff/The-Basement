using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using Server;
using Server.Accounting;
using Server.Mobiles;
using Server.Items;
using Server.Menus;
using Server.Menus.Questions;
using Server.Menus.ItemLists;
using Server.Network;
using Server.Spells;
using Server.Targeting;
using Server.Targets;
using Server.Gumps;
using Server.Commands.Generic;
using Server.Diagnostics;

namespace Server.RabbitsVsSheep 
{

	[FlipableAttribute( 0x143E, 0x143F )]
	public class CleavingHally : BasePoleArm
	{
		public override WeaponAbility PrimaryAbility{ get{ return WeaponAbility.WhirlwindAttack; } }
		public override WeaponAbility SecondaryAbility{ get{ return WeaponAbility.ConcussionBlow; } }

		public int CriticalBonus = 1;

		public override int AosStrengthReq{ get{ return 95; } }
		public override int AosMinDamage{ get{ return 18 * CriticalBonus; } }
		public override int AosMaxDamage{ get{ return 19 * CriticalBonus; } }
		public override int AosSpeed{ get{ return 25; } }

		public override int OldStrengthReq{ get{ return 45; } }
		public override int OldMinDamage{ get{ return 5 * CriticalBonus; } }
		public override int OldMaxDamage{ get{ return 49 * CriticalBonus; } }
		public override int OldSpeed{ get{ return 25; } }

		public override int InitMinHits{ get{ return 31; } }
		public override int InitMaxHits{ get{ return 80; } }
	
		public int CriticalChance = 20;
	
		[Constructable]
		public CleavingHally() : base( 0x143E )
		{
			Name = "A Cleaving Halberd";
			AccuracyLevel = WeaponAccuracyLevel.Supremely;
			DamageLevel = WeaponDamageLevel.Vanq;
			DurabilityLevel = WeaponDurabilityLevel.Indestructible;
			Hue = 1266;
			Weight = 16.0;
			Identified = true;
		}		
		
		public override void OnHit( Mobile attacker, Mobile defender, double damagebonus )
		{
			int ChanceWheel = Utility.Random( 100 );
			
			if ( ChanceWheel <= CriticalChance )
			{
				attacker.SendMessage("You land a Critical Hit!");
				CriticalBonus = 3;
			}
			else
			{
				CriticalBonus = 1;
			}
			
			ArrayList targets = GetMonstersAround( attacker, defender);
			CleaveAllTargets(targets);
			base.OnHit(attacker,defender,damagebonus);
		}

		
		public ArrayList GetMonstersAround( Mobile attacker, Mobile defender )
		{
		
			ArrayList list = new ArrayList();

			foreach ( Mobile m in defender.GetMobilesInRange( 1 ) )
				list.Add( m );

			ArrayList targets = new ArrayList();

			for ( int i = 0; i < list.Count; ++i )
			{
				Mobile m = (Mobile)list[i];

				if ( m != defender && m != attacker && SpellHelper.ValidIndirectTarget( attacker, m ) )
				{
					if ( m == null || m.Deleted || m.Map != attacker.Map || !m.Alive || !attacker.CanSee( m ) || !attacker.CanBeHarmful( m ) )
						continue;

					if ( attacker.InLOS( m ) )
						targets.Add( m );
				}
			}
			
			return targets;
		
		}
		
		public void CleaveAllTargets( ArrayList targets )
		{
			int damage = 0;
			
			if ( DamageLevel == WeaponDamageLevel.Vanq )
			{
				damage = ( MaxDamage + 50 ) / 4;
			}
			else
			{
				damage = ( MaxDamage + 25 ) / 4;
			}
			
			foreach ( Mobile m in targets )
			{
				Effects.SendLocationParticles( EffectItem.Create( m.Location, m.Map, EffectItem.DefaultDuration ), 0x36B0, 1, 14, 1194, 7, 9915, 0 );
				if ( m.Hits >= m.HitsMax )
					m.Hits -= damage; 
			}
		}
		
		public CleavingHally( Serial serial ) : base( serial )
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