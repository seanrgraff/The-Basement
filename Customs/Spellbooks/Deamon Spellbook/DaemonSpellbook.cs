using System;
using Server.Network;
using Server.Prompts;
using Server.Items;
using System.Collections;
using Server.Gumps;
using Server.Targeting;
using Server.Misc;
using Server.Accounting;
using System.Xml;
using Server.Mobiles;
using Server.Spells;
using Server.Spells.Fifth;
using Server.Spells.Seventh; 

namespace Server.Items
{
	public class DaemonSpellbook : Item
	{
	
		public int Page = 1;
	
		[Constructable]
		public DaemonSpellbook() : base( 8787 )
		{
			base.Weight = 1;
			base.Name = "A Daemon Spellbook";
			base.Hue = 2213;
		}		

		public override void OnDoubleClick( Mobile from )
		{
			from.CloseGump(typeof(DaemonSpellGump));
			if ( IsChildOf( from.Backpack ) )
			{
				from.SendGump( new DaemonSpellGump( (PlayerMobile)from, this ) );
			}
			else
			{
				from.SendMessage("The Spellbook must be in your backpack.");
			}
		}
		
		public DaemonSpellbook( Serial serial ) : base( serial )
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

namespace Server.Items
{
	public class DaemonSpellGump : Gump
	{
		private PlayerMobile m_From;
		private DaemonSpellbook m_Book;

		private double Magery;
		
		private int m_Page;
		
//	Uncomment the line below to use a defined number instead of the standard Stat Cap
//		private int Cap;
				
		public DaemonSpellGump( PlayerMobile from, DaemonSpellbook book ): base( 50, 50 )
		{
			m_From = from;
			m_Book = book;
			m_Page = book.Page;

			Magery = from.Skills[SkillName.Magery].Base;
			
			this.Closable=true;
			this.Disposable=true;
			this.Dragable=true;
			this.Resizable=false;
			this.AddPage(0);
			this.AddImage(2, 1, 11058);
			this.AddBackground(62, 105, 119, 90, 9200);
			this.AddBackground(225, 105, 119, 90, 9200);
			this.AddImage(63, 144, 3974);
			this.AddImage(92, 145, 3980);
			this.AddImage(130, 140, 3976);
			this.AddImage(241, 146, 3962);
			this.AddImage(279, 142, 3981);
			
			if ( m_Page != 1 )
				this.AddButton(52, 10, 2205, 2205, 1, GumpButtonType.Reply, 0);
			
			if ( m_Page != 5 )	
				this.AddButton(323, 9, 2206, 2206, 2, GumpButtonType.Reply, 0);
			
			if ( m_Page == 1 )
			{
				this.AddButton(90, 23, 1037, 1033, 3, GumpButtonType.Reply, 0);
				this.AddButton(254, 23, 1004, 1033, 4, GumpButtonType.Reply, 0);
				this.AddLabel(84, 116, 55, @"Unholy Gate"); //creates red gate, spawns friendly imps
				this.AddLabel(246, 116, 55, @"Daemon Form"); //polymorph into Daemon, when dead just Daemon dies human form crawls out
				this.AddLabel(93, 168, 5, @"Mana: 60");
				this.AddLabel(254, 168, 5, @"Mana: 40");
			}
			else if ( m_Page == 2 )
			{
				this.AddButton(90, 23, 1034, 1033, 5, GumpButtonType.Reply, 0);
				this.AddButton(254, 23, 1036, 1033, 6, GumpButtonType.Reply, 0);
				this.AddLabel(84, 116, 55, @"Psi Explode"); //removes percentage of mana from player and does that amount of damage
				this.AddLabel(246, 116, 55, @"Soul Bleed"); //adds blood all around player draining health but adding mana
				this.AddLabel(93, 168, 5, @"Mana: 35");
				this.AddLabel(254, 168, 5, @"Mana: 10");
			}
			else if ( m_Page == 3 )
			{
				this.AddButton(90, 23, 1032, 1033, 7, GumpButtonType.Reply, 0);
				this.AddButton(254, 23, 1005, 1033, 8, GumpButtonType.Reply, 0);
				this.AddLabel(84, 116, 55, @"Possession"); //makes creature under the players control for limited time
				this.AddLabel(246, 116, 55, @"Mana Shield"); //attacks go to mana rather than hitpoints
				this.AddLabel(93, 168, 5, @"Mana: 50");
				this.AddLabel(254, 168, 5, @"Mana: 20");
			}
			else if ( m_Page == 4 )
			{
				this.AddButton(90, 23, 1002, 1033, 9, GumpButtonType.Reply, 0);
				this.AddButton(254, 23, 1006, 1033, 10, GumpButtonType.Reply, 0);
				this.AddLabel(84, 116, 55, @"Shadow Walk"); //hides and stealths player (teleports?)
				this.AddLabel(246, 116, 55, @"Lay Ambush"); //places a hidden trap that decays
				this.AddLabel(93, 168, 5, @"Mana: 5");
				this.AddLabel(254, 168, 5, @"Mana: 10");
			}
			else if ( m_Page == 5 )
			{
				this.AddButton(90, 23, 1003, 1033, 11, GumpButtonType.Reply, 0);
				this.AddButton(254, 23, 1035, 1033, 12, GumpButtonType.Reply, 0);
				this.AddLabel(84, 116, 55, @"Bone Wall"); //Adds wall of bones imbedded with summoned skeletons that attack
				this.AddLabel(246, 116, 55, @"Impersonate"); //Makes the name and appearance the same as another player/creature
				this.AddLabel(93, 168, 5, @"Mana: 10");
				this.AddLabel(254, 168, 5, @"Mana: 20");
			}
		}

		public override void OnResponse( NetState sender, RelayInfo info )
		{
			if ( m_Book.Deleted )
				return;
			
			if ( m_Book.IsChildOf( m_From.Backpack ) )
			{
				if (info.ButtonID == 1)
				{		
					//m_From.SendMessage( "You are turning to the previous page!" );
					m_Book.Page--;
						
					m_From.SendGump( new DaemonSpellGump( (PlayerMobile)m_From, m_Book ) );
				}
				else if (info.ButtonID == 2)
				{
					//m_From.SendMessage( "You are turning to the next page!" );
					m_Book.Page++;
						
					m_From.SendGump( new DaemonSpellGump( (PlayerMobile)m_From, m_Book ) );
				}
				else if (info.ButtonID == 3)
				{
					m_From.SendMessage( "You are casting Unholy Gate!" );
				}
				else if (info.ButtonID == 4) //DEAMON FORM
				{
					if ( ( m_From.BodyMod == 183 || m_From.BodyMod == 184 )
						 || ( !m_From.CanBeginAction( typeof( IncognitoSpell ) ) || m_From.IsBodyMod )
						 || ( DisguiseTimers.IsDisguised( m_From ) )
						 || ( !m_From.CanBeginAction( typeof( PolymorphSpell ) ) )
						 || ( Factions.Sigil.ExistsOn( m_From ) )
					   )
					{
						m_From.SendMessage( "You cannot obtain demonic power in that form." );
					}
					else
					{
						m_From.SendMessage( "You are casting Daemon Form!" );
						new DaemonFormSpell( m_From );
					}
				}
				else if (info.ButtonID == 5)
				{
					m_From.SendMessage( "You are casting Psi Explode!" );
				}
				else if (info.ButtonID == 6)
				{
					m_From.SendMessage( "You are casting Soul Bleed!" );
				}
				else if (info.ButtonID == 7)
				{
					m_From.SendMessage( "You are casting Possession!" );
				}
				else if (info.ButtonID == 8)
				{
					m_From.SendMessage( "You are casting Mana Sheild!" );
				}
				else if (info.ButtonID == 9) //SHADOW WALK
				{
					if ( ( m_From.BodyMod == 183 || m_From.BodyMod == 184 )
						 || ( !m_From.CanBeginAction( typeof( IncognitoSpell ) ) || m_From.IsBodyMod )
						 || ( DisguiseTimers.IsDisguised( m_From ) )
						 || ( !m_From.CanBeginAction( typeof( PolymorphSpell ) ) )
						 || ( Factions.Sigil.ExistsOn( m_From ) )
					   )
					{
						m_From.SendMessage( "You cannot cast this spell in your current form!" );
					}
					else
					{
						m_From.SendMessage( "You are casting Shadow Walk!" );
						
						new ShadowWalkSpell(m_From);
					}
				}
				else if (info.ButtonID == 10) //LAY AMBUSH
				{
					m_From.SendMessage( "You are casting Lay Ambush!" );
					
					m_From.Target = new LayAmbushTarget(m_From, m_Book);
				}
				else if (info.ButtonID == 11) //BONE WALL
				{
					m_From.SendMessage( "You are casting Bone Wall!" );
					
					m_From.Target = new BoneWallTarget(m_From, m_Book);
				}
				else if (info.ButtonID == 12) //IMPERSONATE
				{
					if ( ( m_From.BodyMod == 183 || m_From.BodyMod == 184 )
						 || ( !m_From.CanBeginAction( typeof( IncognitoSpell ) ) || m_From.IsBodyMod )
						 || ( DisguiseTimers.IsDisguised( m_From ) )
						 || ( !m_From.CanBeginAction( typeof( PolymorphSpell ) ) )
						 || ( Factions.Sigil.ExistsOn( m_From ) )
					   )
					{
						m_From.SendMessage( "You cannot impersonate in that form." );
					}
					else
					{
						m_From.SendMessage( "You are casting Impersonate!" );
						m_From.Target = new ImpersonateTarget(m_From, m_Book);
					}
				}
			}
			else
			{
				m_From.SendMessage("The Spellbook must be in your backpack.");
			}
		}				
	}
	
	//**********************************
	//** DEAMON FORM SPELL *************
	//**********************************
	public class DaemonFormSpell
	{		
		public DaemonFormSpell( PlayerMobile m )
		{
			//Important aspects of returning to normal.
			DaemonFormTimer DFT = new DaemonFormTimer( 	m,
														m.BodyMod,
														TimeSpan.FromMinutes( 5.0 ));
														
			//Set all the aspects of the caster to those of the target
			m.Criminal = true;
			m.BodyMod = 40;
		}
	}
	
	class DaemonFormTimer : Timer
	{	
		private PlayerMobile m_player;
		private int body;

		public DaemonFormTimer( 	PlayerMobile player,
									int body,
									TimeSpan delay ) : base( delay )
		{
			m_player = player;
			this.body = body;
			Priority = TimerPriority.OneSecond;
		}

		protected override void OnTick()
		{
			//return to natural form
			m_player.BodyMod = this.body;
		}
	}
	
	//**********************************
	//** SHADOW WALK SPELL *************
	//**********************************
	public class ShadowWalkSpell
	{
		public ShadowWalkSpell(PlayerMobile m)
		{
			Effects.SendLocationEffect( new Point3D( m.X + 1, m.Y, m.Z + 4 ), m.Map, 0x3728, 13 );
			Effects.SendLocationEffect( new Point3D( m.X + 1, m.Y, m.Z ), m.Map, 0x3728, 13 );
			Effects.SendLocationEffect( new Point3D( m.X + 1, m.Y, m.Z - 4 ), m.Map, 0x3728, 13 );
			Effects.SendLocationEffect( new Point3D( m.X, m.Y + 1, m.Z + 4 ), m.Map, 0x3728, 13 );
			Effects.SendLocationEffect( new Point3D( m.X, m.Y + 1, m.Z ), m.Map, 0x3728, 13 );
			Effects.SendLocationEffect( new Point3D( m.X, m.Y + 1, m.Z - 4 ), m.Map, 0x3728, 13 );

			Effects.SendLocationEffect( new Point3D( m.X + 1, m.Y + 1, m.Z + 11 ), m.Map, 0x3728, 13 );
			Effects.SendLocationEffect( new Point3D( m.X + 1, m.Y + 1, m.Z + 7 ), m.Map, 0x3728, 13 );
			Effects.SendLocationEffect( new Point3D( m.X + 1, m.Y + 1, m.Z + 3 ), m.Map, 0x3728, 13 );
			Effects.SendLocationEffect( new Point3D( m.X + 1, m.Y + 1, m.Z - 1 ), m.Map, 0x3728, 13 );

			m.PlaySound( 0x228 );
			m.Hidden = true;
			m.AllowedStealthSteps = 50;
			m.IsStealthing = true;
		}
	}
	
	//**********************************
	//** BONE WALL SPELL ***************
	//**********************************
	public class BoneWallTarget : Target
	{
		private Mobile m_From;
		private DaemonSpellbook m_Book;
		
		public BoneWallTarget( Mobile from, DaemonSpellbook book ) :  base ( Core.ML ? 10 : 12, true, TargetFlags.None )
		{
			m_Book = book;
			m_From = from;
			from.SendMessage("Place the bone wall!");
		}
		
		protected override void OnTarget( Mobile from, object targeted )
		{
			if ( targeted is IPoint3D )
			{
				from.SendMessage("Valid spot for a bone wall!");
				
				IPoint3D p = targeted as IPoint3D;

				Point3D targ3D;
				Point3D Right;
				Point3D Left;
				if ( p is Item )
					targ3D = ((Item)p).GetWorldLocation();
				else
					targ3D = new Point3D( p );
				
				//Determine if the wall should be northtosouth or easttowest
				
				int dx = from.Location.X - targ3D.X;
				int dy = from.Location.Y - targ3D.Y;
				int rx = (dx - dy) * 44;
				int ry = (dx + dy) * 44;

				bool eastToWest;

				if ( rx >= 0 && ry >= 0 )
				{
					eastToWest = false;
				}
				else if ( rx >= 0 )
				{
					eastToWest = true;
				}
				else if ( ry >= 0 )
				{
					eastToWest = true;
				}
				else
				{
					eastToWest = false;
				}
				
				if (!eastToWest)
				{
					//northtosouth
					//only if the target is west or east of the caster
					Right = new Point3D(targ3D.X, targ3D.Y-1, targ3D.Z);
					Left = new Point3D(targ3D.X, targ3D.Y+1, targ3D.Z);
					
				}
				else
				{
					//easttowest
					//only if the target is north or south of the caster
					Right = new Point3D(targ3D.X-1, targ3D.Y, targ3D.Z);
					Left = new Point3D(targ3D.X+1, targ3D.Y, targ3D.Z);
				}
				
				//Create the bone walls!
				bool canFit = AdjustField( ref targ3D, from.Map, 22, true );
				if ( canFit )
				{
					Item BoneWall = new BoneWallAddon();
					BoneWall.Map = from.Map;
					BoneWall.Location = targ3D;
					//Add Sparkles
					Effects.SendLocationParticles( BoneWall, 0x376A, 9, 10, 5025 );
				}
				
				canFit = AdjustField( ref Right, from.Map, 22, true );
				if ( canFit )
				{
					Item BoneWall2 = new BoneWallAddon();
					BoneWall2.Map = from.Map;
					BoneWall2.Location = Right;
					
					Effects.SendLocationParticles( BoneWall2, 0x376A, 9, 10, 5025 );
				}
				
				canFit = AdjustField( ref Left, from.Map, 22, true );
				if ( canFit )
				{
					Item BoneWall3 = new BoneWallAddon();
					BoneWall3.Map = from.Map;
					BoneWall3.Location = Left;
					Effects.SendLocationParticles( BoneWall3, 0x376A, 9, 10, 5025 );
				}
			}
			else
			{
				from.SendMessage("Invalid spot for a bone wall!");
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
		
	}
	
	//**********************************
	//** IMPEROSONATION SPELL **********
	//**********************************
	public class ImpersonateTarget : Target
	{
		private Mobile m_From;
		private DaemonSpellbook m_Book;
		
		public ImpersonateTarget( Mobile from, DaemonSpellbook book ) :  base ( 3, false, TargetFlags.None )
		{
			m_Book = book;
			m_From = from;
			from.SendMessage("Who do you wish to impersonate?");
		}

		protected override void OnTarget( Mobile from, object targeted )
		{
			if ( m_From is PlayerMobile )
			{
				if (m_Book.IsChildOf( m_From.Backpack ) )
				{					
					if ( targeted is Mobile)
					{
						if ( targeted is BaseCreature )
						{
							BaseCreature creature = (BaseCreature) targeted;
							PlayerMobile caster = (PlayerMobile) m_From;
							
							//Important aspects of returning to normal.
							String fromName = caster.Name;
							int fromBodyMod = caster.BodyMod;
							int fromNameHue = caster.NameHue;
							int fromHue = caster.Hue;
							int fromKills = caster.Kills;
							
							//Set all the aspects of the caster to those of the target
							caster.Kills = 0;
							caster.Criminal = true;
							caster.Name = creature.Name;
							caster.NameHue = creature.NameHue;
							caster.Hue = creature.Hue;
							caster.BodyMod = creature.Body;
							
							//Need to start a timer when impersonation starts.
							ImpersonationTimer DispellTimer = new ImpersonationTimer( 	from, 
																						fromName,
																						fromNameHue,
																						fromHue,
																						fromKills,
																						fromBodyMod,
																						TimeSpan.FromMinutes( 5.0 ));
																	
							DispellTimer.Start();
						}
						else
						{
							from.SendMessage("You cannot impersonate that!");
						}	
					}
					else
					{
						from.SendMessage("You can only impersonate creatures.");
					}
				}
				else{
					from.SendMessage("The Spellbook must be in your pack.");
				}			
			}
		}
	}
	
	class ImpersonationTimer : Timer
	{
		private Mobile m_Caster;
		
		private String fromName;
		private int fromNameHue;
		private int fromHue;
		private int fromKills;
		private int fromBodyMod;

		public ImpersonationTimer( 	Mobile caster,
									String fromName,
									int fromNameHue,
									int fromHue,
									int fromKills,
									int fromBodyMod,
									TimeSpan delay ) : base( delay )
		{
			m_Caster = caster;
			this.fromName = fromName;
			this.fromKills = fromKills;
			this.fromNameHue = fromNameHue;
			this.fromHue = fromHue;
			this.fromBodyMod = fromBodyMod;
			Priority = TimerPriority.OneSecond;
		}

		protected override void OnTick()
		{
			m_Caster.SendMessage("You feel the impersonation spell wear off.");
			m_Caster.Name = fromName;
			m_Caster.Criminal = false;
			m_Caster.NameHue = fromNameHue;
			m_Caster.Hue = fromHue;
			m_Caster.Kills = fromKills;
			m_Caster.BodyMod = fromBodyMod;
		}
	}
	
	//*******************************
	//** LAY AMBUSH SPELL ***********
	//*******************************
	public class LayAmbushTarget : Target
	{
		private Mobile m_From;
		private DaemonSpellbook m_Book;
		
		public LayAmbushTarget( Mobile from, DaemonSpellbook book ) :  base ( Core.ML ? 10 : 12, true, TargetFlags.None )
		{
			m_Book = book;
			m_From = from;
			from.SendMessage("Place the ambush trap!");
		}
		
		protected override void OnTarget( Mobile from, object targeted )
		{		
			IPoint3D p = targeted as IPoint3D;

			Point3D targ3D;
			Point3D Right;
			Point3D Left;
			if ( p is Item )
				targ3D = ((Item)p).GetWorldLocation();
			else
				targ3D = new Point3D( p );
				
			AmbushTrap AmbushTrap = new AmbushTrap();
			bool canFit = AmbushTrap.AdjustField( ref targ3D, from.Map, 22, true );
			if ( canFit )
			{
				AmbushTrap.Map = from.Map;
				AmbushTrap.Location = targ3D;
				//Add Sparkles
				Effects.SendLocationParticles( AmbushTrap, 0x376A, 9, 10, 5025 );
				//Add Dispel Timer
				AmbushTimer DispellTimer = new AmbushTimer( AmbushTrap, TimeSpan.FromMinutes( 20.0 ));
				DispellTimer.Start();
			}
			else
			{
				AmbushTrap.Delete();
				from.SendMessage("You could not create a trap there!");
			}
		}
	}
	class AmbushTimer : Timer
	{	
		private Item m_Trap;

		public AmbushTimer( 		AmbushTrap trap,
									TimeSpan delay ) : base( delay )
		{
			m_Trap = trap;
			Priority = TimerPriority.OneSecond;
		}

		protected override void OnTick()
		{
			m_Trap.Delete();
		}
	}
}