using System;
using Server;
using Server.Network;
using Server.Mobiles;

namespace Server.Items
{
	public enum AmbushTrapImage
	{
		WoodEast,
		WoodWest,
		HayOne,
		HayTwo,
		HayThree,
		Trash,
		FishingLine,
		Rose,
		LambOne,
		LambTwo,
		LambThree,
		LambFour,
		CrystalsOne,
		CrystalsTwo,
		CrystalsThree,
		CrystalsFour
	}
	
	public enum AmbushTrapType
	{
		Poison,
		Explosion,
		Cage
	}

	public class AmbushTrap : BaseTrap
	{
		private AmbushTrapImage m_Image;
		private AmbushTrapType m_Type;

		[CommandProperty( AccessLevel.GameMaster )]
		public AmbushTrapImage Image
		{
			get
			{
				switch ( ItemID )
				{
					case 3119: return AmbushTrapImage.WoodEast;
					case 6922: return AmbushTrapImage.WoodWest;
					case 3892: return AmbushTrapImage.HayOne;
					case 4586: return AmbushTrapImage.HayTwo;
					case 4587: return AmbushTrapImage.HayThree;
					case 4334: return AmbushTrapImage.Trash;
					case 8195: return AmbushTrapImage.FishingLine;
					case 6378: return AmbushTrapImage.Rose;
					case 6925: return AmbushTrapImage.LambOne;
					case 6928: return AmbushTrapImage.LambTwo;
					case 6926: return AmbushTrapImage.LambThree;
					case 12684: return AmbushTrapImage.LambFour;
					case 8762: return AmbushTrapImage.CrystalsOne;
					case 8763: return AmbushTrapImage.CrystalsTwo;
					case 8764: return AmbushTrapImage.CrystalsThree;
					case 8765: return AmbushTrapImage.CrystalsFour;
				}

				return AmbushTrapImage.WoodEast;
			}
			set
			{
				ItemID = GetBaseID( value );
			}
		}

		public static int GetBaseID( AmbushTrapImage type )
		{
			switch ( type )
			{
				case AmbushTrapImage.WoodEast: return 3119;
				case AmbushTrapImage.WoodWest: return 6922;
				case AmbushTrapImage.HayOne: return 3892;
				case AmbushTrapImage.HayTwo: return 4586;
				case AmbushTrapImage.HayThree: return 4587;
				case AmbushTrapImage.Trash: return 4334;
				case AmbushTrapImage.FishingLine: return 8195;
				case AmbushTrapImage.Rose: return 6378;
				case AmbushTrapImage.LambOne: return 6925;
				case AmbushTrapImage.LambTwo: return 6928;
				case AmbushTrapImage.LambThree: return 6926;
				case AmbushTrapImage.LambFour: return 12684;
				case AmbushTrapImage.CrystalsOne: return 8762;
				case AmbushTrapImage.CrystalsTwo: return 8763;
				case AmbushTrapImage.CrystalsThree: return 8764;
				case AmbushTrapImage.CrystalsFour: return 8765;
			}

			return 0;
		}
		
		public static AmbushTrapImage RandomImage()
		{
			int Chance = Utility.Random( 100 );
			if ( Chance <= 7 )
			{
				return AmbushTrapImage.WoodEast;
			}
			else if ( Chance <= 14 )
			{
				return AmbushTrapImage.WoodWest;
			}
			else if ( Chance <= 21 )
			{
				return AmbushTrapImage.HayOne;
			}
			else if ( Chance <= 28 )
			{
				return AmbushTrapImage.HayTwo;
			}
			else if ( Chance <= 35 )
			{
				return AmbushTrapImage.HayThree;
			}
			else if ( Chance <= 42 )
			{
				return AmbushTrapImage.Trash;
			}
			else if ( Chance <= 49 )
			{
				return AmbushTrapImage.FishingLine;
			}
			else if ( Chance <= 56 )
			{
				return AmbushTrapImage.Rose;
			}
			else if ( Chance <= 63 )
			{
				return AmbushTrapImage.LambOne;
			}
			else if ( Chance <= 70 )
			{
				return AmbushTrapImage.LambTwo;
			}
			else if ( Chance <= 77 )
			{
				return AmbushTrapImage.LambThree;
			}
			else if ( Chance <= 84 )
			{
				return AmbushTrapImage.LambFour;
			}
			else if ( Chance <= 91 )
			{
				return AmbushTrapImage.CrystalsOne;
			}
			else if ( Chance <= 93 )
			{
				return AmbushTrapImage.CrystalsTwo;
			}
			else if ( Chance <= 97 )
			{	
				return AmbushTrapImage.CrystalsThree;
			}
			else
			{
				return AmbushTrapImage.CrystalsFour;
			}
		}
		
		public static AmbushTrapType RandomType()
		{
			int Chance = Utility.Random( 100 );
			if ( Chance <= 35 )
			{
				return AmbushTrapType.Poison;
			}
			else if ( Chance <= 70 )
			{
				return AmbushTrapType.Explosion;
			}
			else
			{
				return AmbushTrapType.Cage;
			}
		}

		[Constructable]
		public AmbushTrap() : this( RandomImage() )
		{
		}

		[Constructable]
		public AmbushTrap( AmbushTrapImage image ) : this( image, RandomType() )
		{
		}

		[Constructable]
		public AmbushTrap( AmbushTrapType type ) : this( RandomImage(), type )
		{
		}

		[Constructable]
		public AmbushTrap( AmbushTrapImage image, AmbushTrapType type ) : base( GetBaseID( image ) )
		{
			m_Image = image;
			m_Type = type;
		}

		public override bool PassivelyTriggered{ get{ return false; } }
		public override TimeSpan PassiveTriggerDelay{ get{ return TimeSpan.Zero; } }
		public override int PassiveTriggerRange{ get{ return 0; } }
		public override TimeSpan ResetDelay{ get{ return TimeSpan.FromSeconds( 0.0 ); } }

		public override void OnTrigger( Mobile from )
		{
			if ( m_Image == null || m_Type == null || !from.Player || !from.Alive || from.AccessLevel > AccessLevel.Player )
				return;
				
			if ( m_Type != AmbushTrapType.Cage )
			{
				Point3D loc = new Point3D(this.X,this.Y,this.Z);
				
				DoLaunch( this, from, loc, this.Map );
			}
			else
			{
				//Make Cage Around the Player
				BoneWallAddon BoneCage;
				Point3D Target = new Point3D(this.X, this.Y, this.Z);
				for ( int i=0; i < 4; i++ )
				{
					BoneCage = new BoneWallAddon();
					if ( i == 0 )
					{
						Target.X++;
					}
					else if ( i == 1 )
					{
						Target.X--;
						Target.X--;
					}
					else if ( i == 2 )
					{
						Target.X++;
						Target.Y++;
					}
					else if ( i == 3 )
					{
						Target.Y--;
						Target.Y--;
					}
					
					bool canfit = AdjustField( ref Target, this.Map, 22, true );
				
					if ( canfit )
					{
						BoneCage.Map = this.Map;
						BoneCage.Location = Target;
						//Add Sparkles
						Effects.SendLocationParticles( BoneCage, 0x376A, 9, 10, 5025 );
					}
					else
					{
						BoneCage.Delete();
					}
				}
			}
			
			this.Delete();
		}
		
		private void DoLaunch( object killed, Mobile from, Point3D StartLoc, Map map )
		{
				//ONE//
				Point3D endHeadLoc = new Point3D( StartLoc.X + Utility.RandomMinMax( -2, 2 ), StartLoc.Y + Utility.RandomMinMax( -2, 2 ), StartLoc.Z );
				Effects.SendMovingEffect( new Entity( Serial.Zero, StartLoc, map ), new Entity( Serial.Zero, endHeadLoc, map ),
							0xF0D, 5, 0, false, false );
				Timer.DelayCall( TimeSpan.FromSeconds( 0.5 ), new TimerStateCallback( FinishLaunch ), new object[]{ from, endHeadLoc, map, killed } );
				//END ONE//
	
				//TWO//
				Point3D endTorsoLoc = new Point3D( StartLoc.X + Utility.RandomMinMax( -2, 2 ), StartLoc.Y + Utility.RandomMinMax( -2, 2 ), StartLoc.Z );
				Effects.SendMovingEffect( new Entity( Serial.Zero, StartLoc, map ), new Entity( Serial.Zero, endTorsoLoc, map ),
							0xF0D, 5, 0, false, false );
				Timer.DelayCall( TimeSpan.FromSeconds( 0.5 ), new TimerStateCallback( FinishLaunch ), new object[]{ from, endTorsoLoc, map, killed } );
				//END TWO//
			
				//THREE//
				Point3D endLLLoc = new Point3D( StartLoc.X + Utility.RandomMinMax( -2, 2 ), StartLoc.Y + Utility.RandomMinMax( -2, 2 ), StartLoc.Z );
				Effects.SendMovingEffect( new Entity( Serial.Zero, StartLoc, map ), new Entity( Serial.Zero, endLLLoc, map ),
							0xF0D, 5, 0, false, false );
				Timer.DelayCall( TimeSpan.FromSeconds( 0.5 ), new TimerStateCallback( FinishLaunch ), new object[]{ from, endLLLoc, map, killed } );
				//END THREE//
	
				//FOUR//
				Point3D endRLLoc = new Point3D( StartLoc.X + Utility.RandomMinMax( -2, 2 ), StartLoc.Y + Utility.RandomMinMax( -2, 2 ), StartLoc.Z );
				Effects.SendMovingEffect( new Entity( Serial.Zero, StartLoc, map ), new Entity( Serial.Zero, endRLLoc, map ),
							0xF0D, 5, 0, false, false );
				Timer.DelayCall( TimeSpan.FromSeconds( 0.5 ), new TimerStateCallback( FinishLaunch ), new object[]{ from, endRLLoc, map, killed } );
				//END FOUR//
	
				//FIVE//
				Point3D endLALoc = new Point3D( StartLoc.X + Utility.RandomMinMax( -2, 2 ), StartLoc.Y + Utility.RandomMinMax( -2, 2 ), StartLoc.Z );
				Effects.SendMovingEffect( new Entity( Serial.Zero, StartLoc, map ), new Entity( Serial.Zero, endLALoc, map ),
							0xF0D, 5, 0, false, false );
				Timer.DelayCall( TimeSpan.FromSeconds( 0.5 ), new TimerStateCallback( FinishLaunch ), new object[]{ from, endLALoc, map, killed } );
				//END FIVE//
	
				//SIX//
				Point3D endRALoc = new Point3D( StartLoc.X + Utility.RandomMinMax( -2, 2 ), StartLoc.Y + Utility.RandomMinMax( -2, 2 ), StartLoc.Z );
				Effects.SendMovingEffect( new Entity( Serial.Zero, StartLoc, map ), new Entity( Serial.Zero, endRALoc, map ),
							0xF0D, 5, 0, false, false );
				Timer.DelayCall( TimeSpan.FromSeconds( 0.5 ), new TimerStateCallback( FinishLaunch ), new object[]{ from, endRALoc, map, killed } );
				//END SIX//
		}
		
		private void FinishLaunch( object state )
		{
			object[] states = (object[])state;

			Mobile from = (Mobile)states[0];
			Point3D endHeadLoc = (Point3D)states[1];
			Map map = (Map)states[2];

			if ( map == null || map == Map.Internal )
					return;

			if ( m_Type != AmbushTrapType.Cage )
			{
				if ( m_Type == AmbushTrapType.Poison )
				{
					PoisonExplosionPotion Epot = new PoisonExplosionPotion();
					Epot.Movable = true;
					Epot.MoveToWorld( endHeadLoc, map );
					Epot.Explode(from, false, Epot.GetWorldLocation(), Epot.Map); 
				}
				else if ( m_Type == AmbushTrapType.Explosion )
				{
					ExplosionPotion Epot = new ExplosionPotion();
					Epot.Movable = true;
					Epot.MoveToWorld( endHeadLoc, map );
					Epot.Explode(from, false, Epot.GetWorldLocation(), Epot.Map); 
				}
				else
				{
					LesserExplosionPotion Epot = new LesserExplosionPotion();
					Epot.Movable = true;
					Epot.MoveToWorld( endHeadLoc, map );
					Epot.Explode(from, false, Epot.GetWorldLocation(), Epot.Map); 
				}
			}
		}
		
		public static bool AdjustField( ref Point3D p, Map map, int height, bool mobsBlock )
		{
			if( map == null )
				return false;

			for( int offset = 0; offset < 10; ++offset )
			{
				Point3D loc = new Point3D( p.X, p.Y, p.Z - offset );

				if( map.CanFit( loc, height, true, mobsBlock ) )
				{
					p = loc;
					return true;
				}
			}

			return false;
		}

		public AmbushTrap( Serial serial ) : base( serial )
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