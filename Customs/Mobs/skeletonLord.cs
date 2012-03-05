using System;
using System.Collections;
using Server.Items;
using Server.ContextMenus;
using Server.Misc;
using Server.Network;

namespace Server.Mobiles
{
	[CorpseName( "a Skeleton Lord's Corpse" )]
	public class SkeletonLord : BaseCreature
	{
		public override bool ClickTitle{ get{ return false; } }

		[Constructable]
		public SkeletonLord() : base( AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			SpeechHue = Utility.RandomDyedHue();
			Name = "a Skeleton Lord";
			Hue = 0;
			Body = 0x190;
			BaseSoundID = 357;

		
			SetStr( 1050, 1080 );
			SetDex( 210, 225 );
			SetInt( 101, 110 );

			SetDamage( 25, 40 );

			SetSkill( SkillName.Fencing, 66.0, 97.5 );
			SetSkill( SkillName.Macing, 90.0, 97.5 );
			SetSkill( SkillName.MagicResist, 85.0, 100.5 );
			SetSkill( SkillName.Swords, 100.0 );
			SetSkill( SkillName.Tactics, 95.0, 105.5 );
			SetSkill( SkillName.Wrestling, 105.0, 110.5 );

			Fame = 1000;
			Karma = -1000;

			AddItem( new Boots(1109) );
			AddItem( new BodySash(1109) );
			AddItem( new Cloak(1109) );
			AddItem( new Broadsword() );
			AddItem( new BoneHelm() );
			AddItem( new BoneChest() );
			AddItem( new BoneLegs() );
			AddItem( new BoneArms() );
			AddItem( new BoneGloves() );
			AddItem( new StuddedGorget() );
							
			Utility.AssignRandomHair( this );
		}
		
		public override void GenerateLoot()
		{
			AddLoot( LootPack.Average );
			AddLoot( LootPack.Meager );
		}

		public override bool ReacquireOnMovement{ get{ return !Controlled; } }
		public override bool HasBreath{ get{ return false; } } // fire breath
		public override bool AutoDispel{ get{ return !Controlled; } }
		public override bool AlwaysMurderer{ get{ return true; } }

		public static ArrayList Skeletons = null;

		public void SpawnVampireBats( Mobile target )
		{
			Map map = this.Map;

			if ( map == null )
				return;
			
			if ( Skeletons == null )
				Skeletons = new ArrayList();

			int newVampireBats = Utility.RandomMinMax( 4, 8 );

			for ( int i = 0; i < newVampireBats; ++i )
			{
				Skeleton vampirebat = new Skeleton();

				Skeletons.Add(vampirebat);

				vampirebat.Team = this.Team;
				vampirebat.FightMode = FightMode.Closest;

				bool validLocation = false;
				Point3D loc = this.Location;

				for ( int j = 0; !validLocation && j < 10; ++j )
				{
					int x = X + Utility.Random( 3 ) - 1;
					int y = Y + Utility.Random( 3 ) - 1;
					int z = map.GetAverageZ( x, y );

					if ( validLocation = map.CanFit( x, y, this.Z, 16, false, false ) )
						loc = new Point3D( x, y, Z );
					else if ( validLocation = map.CanFit( x, y, z, 16, false, false ) )
						loc = new Point3D( x, y, z );
				}

				Effects.SendLocationParticles( EffectItem.Create( loc, map, EffectItem.DefaultDuration ), 0x3728, 10, 10, 5023 );
				vampirebat.MoveToWorld( loc, map );
				vampirebat.Combatant = target;
			}
		}

		public override void AlterDamageScalarFrom( Mobile caster, ref double scalar )
		{
			if ( 0.1 >= Utility.RandomDouble() && Hits < (HitsMax / 2) )
				SpawnVampireBats( caster );
		}
		
		public override bool OnBeforeDeath()
		{
			if ( Skeletons == null )
				return true;
			
			foreach (Skeleton s in Skeletons)
			{
				s.Kill();
			}
			Skeletons.Clear();
			return true;
		}

		public override void OnGaveMeleeAttack( Mobile defender )
		{
			base.OnGaveMeleeAttack( defender );

			defender.Damage( Utility.Random( 10, 5 ), this );
			defender.Stam -= Utility.Random( 10, 5 );
			defender.Mana -= Utility.Random( 10, 5 );
		}

		public override void OnGotMeleeAttack( Mobile attacker )
		{
			base.OnGotMeleeAttack( attacker );

			if ( 0.01 >= Utility.RandomDouble() && Hits < (HitsMax / 2) )
				SpawnVampireBats( attacker );
		}
		
		public SkeletonLord( Serial serial ) : base( serial )
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