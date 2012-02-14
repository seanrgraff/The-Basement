using System;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Items
{
	public class BoneWallAddon : BaseAddon
	{
		public override BaseAddonDeed Deed
		{
			get
			{
				return new BoneWallAddonDeed();
			}
		}
		
		private BoneWallSkeleton Skele = null;

		[ Constructable ]
		public BoneWallAddon()
		{
			int BoneDebris = 6925;
			int HangingSkeleton = 7036;
			
			AddonComponent ac = null;
			ac = new AddonComponent( 8708 );
			AddComponent( ac, 0, 0, 0 );
			
			int ChanceWheel = Utility.Random( 100 );
			if ( ChanceWheel < 10 )
			{
				HangingSkeleton = 7036;
			}
			else if ( ChanceWheel < 20 )
			{
				HangingSkeleton = 7039;
			}
			else if ( ChanceWheel < 30 )
			{
				HangingSkeleton = 6666;
			}
			else if ( ChanceWheel < 40 )
			{
				HangingSkeleton = 6665;
			}
			else if ( ChanceWheel < 50 )
			{
				HangingSkeleton = 6660;
			}
			else if ( ChanceWheel < 60 )
			{
				HangingSkeleton = 6659;
			}
			else if ( ChanceWheel < 70 )
			{
				HangingSkeleton = 6662;
			}
			else if ( ChanceWheel < 80 )
			{
				HangingSkeleton = 6661;
			}
			else if ( ChanceWheel < 90 )
			{
				HangingSkeleton = 6941;
			}
			else
			{
				HangingSkeleton = 6942;
			}
			ac = new AddonComponent( HangingSkeleton );
			AddComponent( ac, 0, 0, 0 );
			
			ChanceWheel = Utility.Random( 100 );
			if ( ChanceWheel < 50 )
			{
				ac = new AddonComponent( 6227 );
				AddComponent( ac, 0, 0, 17 );
			}
			
			ChanceWheel = Utility.Random( 100 );
			if ( ChanceWheel < 14 )
			{
				BoneDebris = 6925;
			}
			else if ( ChanceWheel < 28 )
			{
				BoneDebris = 6923;
			}
			else if ( ChanceWheel < 42 )
			{
				BoneDebris = 6927;
			}
			else if ( ChanceWheel < 56 )
			{
				BoneDebris = 6922;
			}
			else if ( ChanceWheel < 70 )
			{
				BoneDebris = 6928;
			}
			else if ( ChanceWheel < 84 )
			{
				BoneDebris = 6924;
			}
			else if ( ChanceWheel < 98 )
			{
				BoneDebris = 6926;
			}
			else
			{
				BoneDebris = 12684;
			}
			
			ac = new AddonComponent( BoneDebris );
			AddComponent( ac, 0, 0, 0 );
			
			Skele = new BoneWallSkeleton(this);
			Skele.Map = this.Map;
			Skele.Location = this.Location;
			
			Timer.DelayCall( TimeSpan.FromSeconds( 2.0 ), TimeSpan.FromSeconds( 2.0 ), new TimerStateCallback( Summoned_Damage ), Skele );
		}
		
		private static void Summoned_Damage( object state )
		{
			Mobile mob = (Mobile)state;

			if ( mob.Hits > 0 )
				--mob.Hits;
			else
				mob.Kill();
		}

		public override void OnLocationChange(Point3D oldLoc)
		{
			Skele.Map = this.Map;
			Skele.Location = this.Location;
	
			base.OnLocationChange(oldLoc);
		}

		public BoneWallAddon( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );
			writer.Write( 0 ); // Version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );
			int version = reader.ReadInt();
		}
	}

	public class BoneWallAddonDeed : BaseAddonDeed
	{
		public override BaseAddon Addon
		{
			get
			{
				return new BoneWallAddon();
			}
		}

		[Constructable]
		public BoneWallAddonDeed()
		{
			Name = "Bone Wall";
		}

		public BoneWallAddonDeed( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );
			writer.Write( 0 ); // Version
		}

		public override void	Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );
			int version = reader.ReadInt();
		}
	}
}

namespace Server.Mobiles
{
	[CorpseName( "a skeletal corpse" )]
	public class BoneWallSkeleton : BaseCreature
	{
		private BoneWallAddon Wall = null;
	
		[Constructable]
		public BoneWallSkeleton(BoneWallAddon wall) : base( AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			CantWalk = true;
			Name = "a skeleton";
			Body = Utility.RandomList( 50, 56 );
			BaseSoundID = 0x48D;

			SetStr( 56, 80 );
			SetDex( 56, 75 );
			SetInt( 16, 40 );

			SetHits( 34, 48 );

			SetDamage( 3, 7 );

			SetDamageType( ResistanceType.Physical, 100 );

			SetResistance( ResistanceType.Physical, 15, 20 );
			SetResistance( ResistanceType.Fire, 5, 10 );
			SetResistance( ResistanceType.Cold, 25, 40 );
			SetResistance( ResistanceType.Poison, 25, 35 );
			SetResistance( ResistanceType.Energy, 5, 15 );

			SetSkill( SkillName.MagicResist, 45.1, 60.0 );
			SetSkill( SkillName.Tactics, 45.1, 60.0 );
			SetSkill( SkillName.Wrestling, 45.1, 55.0 );

			Fame = 450;
			Karma = -450;

			VirtualArmor = 16;

			switch ( Utility.Random( 5 ))
			{
				case 0: PackItem( new BoneArms() ); break;
				case 1: PackItem( new BoneChest() ); break;
				case 2: PackItem( new BoneGloves() ); break;
				case 3: PackItem( new BoneLegs() ); break;
				case 4: PackItem( new BoneHelm() ); break;
			}
			
			this.Wall = wall;
		}
		
		public override void OnDeath(Container c)
		{
			if ( Wall != null )
			{
				Wall.Delete();
			}
			
			base.OnDeath(c);
		}

		public override bool BleedImmune{ get{ return true; } }
		public override Poison PoisonImmune{ get{ return Poison.Lesser; } }

		public BoneWallSkeleton( Serial serial ) : base( serial )
		{
		}

		public override OppositionGroup OppositionGroup
		{
			get{ return OppositionGroup.FeyAndUndead; }
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );
			writer.Write( (int) 0 );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );
			int version = reader.ReadInt();
		}
	}
}