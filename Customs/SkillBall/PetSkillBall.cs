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

namespace Server.Items
{
	public class PetSkillBall : Item
	{
		[Constructable]
		public PetSkillBall() : base( 0x1870 )
		{
			base.Weight = 0;
			base.Name = "A Pet Skill Ball";
			base.Hue = 26;
		}		

		public override void OnDoubleClick( Mobile from )
		{
			if ( IsChildOf( from.Backpack ) )
			{
				from.Target = new PetBallTarget(from, this);
			}
			else
			{
				from.SendMessage("The Pet Skill Ball must be in your backpack.");
			}
		}
		
		public PetSkillBall( Serial serial ) : base( serial )
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
	
		public class PetBallTarget : Target
		{
			private Mobile m_From;
			private PetSkillBall m_Ball;
			
			public PetBallTarget( Mobile from, PetSkillBall ball ) :  base ( 3, false, TargetFlags.None )
			{
				m_Ball = ball;
				m_From = from;
				m_From.CloseGump( typeof( PetBallGump ) );
				from.SendMessage("Select a pet to modify their skills.");
			}
			
			protected override void OnTarget( Mobile from, object targeted )
			{
				
				if (m_Ball.IsChildOf( m_From.Backpack ) )
				{					
					if ( targeted is Mobile )
					{
						if ( targeted is BaseCreature )
						{
							BaseCreature creature = (BaseCreature)targeted;
							if( !creature.Tamable ){
								from.SendMessage("That creature is not tameable.");
							}
							else if(  !creature.Controlled || creature.ControlMaster != from ){
								from.SendMessage("That creature is not controlled by you.");
							}
							else if ( creature.Summoned ){
								from.SendMessage("Cannot modify summoned creatures skills.");
							}
							else if ( creature.Body.IsHuman ){
								from.SendMessage("Cannot bond humanoids.");
							}
							else
							{	
									from.SendGump( new PetBallGump( (PlayerMobile)from, (BaseCreature)creature, m_Ball ) );
							}							
						}
						else{
							from.SendMessage("Target must be an creature.");
						}
					}
					else{
							from.SendMessage("Target must not be an item.");
						}
				}
				else{
					from.SendMessage("The Pet Skill Ball must be in your backpack.");
				}			
			}
	}
}

namespace Server.Items
{
	public class PetBallGump : Gump
	{
		private PlayerMobile m_From;
		private BaseCreature m_Pet;
		private PetSkillBall m_Ball;
		
		private double Wrestling;
		private double Tactics;
		private double ResistingSpells;
		private double Anatomy;
		private double Poisoning;
		private double Magery;
		private double EvaluatingIntelligence;
		private double Meditation;
		
//	Uncomment the line below to use a defined number instead of the standard Stat Cap
//		private int Cap;
				
		public PetBallGump( PlayerMobile from, BaseCreature pet, PetSkillBall ball ): base( 50, 50 )
		{
			m_From = from;
			m_Ball = ball;
			m_Pet = pet;
			
			Wrestling = pet.Skills[SkillName.Wrestling].Base;
			Tactics = pet.Skills[SkillName.Tactics].Base;
			ResistingSpells = pet.Skills[SkillName.MagicResist].Base;
			Anatomy = pet.Skills[SkillName.Anatomy].Base;
			Poisoning = pet.Skills[SkillName.Poisoning].Base;
			Magery = pet.Skills[SkillName.Magery].Base;
			EvaluatingIntelligence = pet.Skills[SkillName.EvalInt].Base;
			Meditation = pet.Skills[SkillName.Meditation].Base;
			
			String petName = pet.Name + "'s";
			
			this.Closable=true;
			this.Disposable=true;
			this.Dragable=true;
			this.Resizable=false;
			this.AddPage(0);
			this.AddImage(4, 23, 8000);
			this.AddImage(24, 60, 8001);
			this.AddImage(24, 126, 8001);
			this.AddImage(24, 196, 8001);
			this.AddImage(23, 250, 8001);
			this.AddImage(24, 313, 8003);
			this.AddImage(202, 33, 2100);
			//this.AddLabel(77, 29, 5, petName );
			this.AddHtml( 64, 29, 123, 18, "<basefont size=5>" + petName + "</basefont>", false, false );
			this.AddImage(51, 88, 2086);
			this.AddImage(51, 212, 2086);
			this.AddLabel(73, 84, 5, @"Combat Ratings");
			this.AddLabel(73, 208, 5, @"Lore & Knowledge");
			this.AddLabel(80, 105, 545, @"Wrestling");
			this.AddLabel(80, 125, 545, @"Tactics");
			this.AddLabel(80, 145, 545, @"Resisting Spells");
			this.AddLabel(80, 165, 545, @"Anatomy");
			this.AddLabel(80, 185, 545, @"Poisoning");
			this.AddLabel(80, 225, 545, @"Magery");
			this.AddLabel(80, 245, 545, @"Evaluating Intelligence");
			this.AddLabel(80, 265, 545, @"Meditation");
			this.AddImage(71, 61, 2091);
			this.AddImage(69, 298, 2091);
			this.AddLabel(76, 320, 1672, @"Created by Doobs.");
			
			if ( pet.Skills[SkillName.Wrestling].Base > 0 )
			{
				this.AddTextEntry(225, 105, 35, 17, 0, 1, pet.Skills[SkillName.Wrestling].Base.ToString() );
			}
			else
			{
				this.AddLabel(225, 105, 0, @"---" );
			}
			if ( pet.Skills[SkillName.Tactics].Base > 0 )
			{
				this.AddTextEntry(225, 125, 35, 17, 0, 2, pet.Skills[SkillName.Tactics].Base.ToString() );
			}
			else
			{
				this.AddLabel(225, 125, 0, @"---" );
			}
			if ( pet.Skills[SkillName.MagicResist].Base > 0 )
			{
				this.AddTextEntry(225, 145, 35, 17, 0, 3, pet.Skills[SkillName.MagicResist].Base.ToString() );
			}
			else
			{
				this.AddLabel(225, 145, 0, @"---" );
			}
			if ( pet.Skills[SkillName.Anatomy].Base > 0 )
			{
				this.AddTextEntry(225, 165, 35, 17, 0, 4, pet.Skills[SkillName.Anatomy].Base.ToString() );
			}
			else
			{
				this.AddLabel(225, 165, 0, @"---" );
			}
			if ( pet.Skills[SkillName.Poisoning].Base > 0 )
			{
				this.AddTextEntry(225, 185, 35, 17, 0, 5, pet.Skills[SkillName.Poisoning].Base.ToString() );
			}
			else
			{
				this.AddLabel(225, 185, 0, @"---" );
			}
			if ( pet.Skills[SkillName.Magery].Base > 0 )
			{
				this.AddTextEntry(225, 225, 35, 17, 0, 6, pet.Skills[SkillName.Magery].Base.ToString() );
			}
			else
			{
				this.AddLabel(225, 225, 0, @"---" );
			}
			if ( pet.Skills[SkillName.EvalInt].Base > 0 )
			{
				this.AddTextEntry(225, 245, 35, 17, 0, 7, pet.Skills[SkillName.EvalInt].Base.ToString() );
			}
			else
			{
				this.AddLabel(225, 245, 0, @"---" );
			}
			if ( pet.Skills[SkillName.Meditation].Base > 0 )
			{
				this.AddTextEntry(225, 265, 35, 17, 0, 8, pet.Skills[SkillName.Meditation].Base.ToString() );
			}
			else
			{
				this.AddLabel(225, 265, 0, @"---" );
			}
			
			this.AddButton(283, 290, 4023, 4024, 9, GumpButtonType.Reply, 0);
		}

		public override void OnResponse( NetState sender, RelayInfo info )
		{
			if ( m_Ball.Deleted )
				return;
						
			if (info.ButtonID == 9)
			{
            					TextRelay w = null;
            					if ( info.GetTextEntry( 1 ) != null )
            						w = info.GetTextEntry( 1 );
					            if ( w != null )
					            {
	            					try
	           		 				{
	           		 					Wrestling = Convert.ToDouble(w.Text);
	            					}
	            					catch
	            					{
	                 					m_From.SendMessage("Bad wrestling entry. A number was expected.");
	            					}
            					}
            					TextRelay t = null;
            					if ( info.GetTextEntry( 2 ) != null )
            						t = info.GetTextEntry( 2 );
            					if ( t != null )
            					{
	            					try
	           		 				{
	           		 					Tactics = Convert.ToDouble(t.Text);
	            					}
	            					catch
	            					{
	                 					m_From.SendMessage("Bad tactics entry. A number was expected.");
	            					}
	            				}
            					TextRelay r = null;
            					if ( info.GetTextEntry( 3 ) != null )
            						r = info.GetTextEntry( 3 );
            					if ( r != null )
            					{
	            					try
	           		 				{
	           		 					ResistingSpells = Convert.ToDouble(r.Text);
	            					}
	            					catch
	            					{
	                 					m_From.SendMessage("Bad resisting spells entry. A number was expected.");
	            					}
	            				}
            					TextRelay a = null;
            					if ( info.GetTextEntry( 4 ) != null )
            						a = info.GetTextEntry( 4 );
            					if ( a != null )
            					{
	            					try
	           		 				{
	           		 					Anatomy = Convert.ToDouble(a.Text);
	            					}
	            					catch
	            					{
	                 					m_From.SendMessage("Bad anatomy entry. A number was expected.");
	            					}
            					}
            					TextRelay p = null;
            					if ( info.GetTextEntry( 5 ) != null )
            						p = info.GetTextEntry( 5 );
            					if ( p != null )
            					{
	            					try
	           		 				{
	           		 					Poisoning = Convert.ToDouble(p.Text);
	            					}
	            					catch
	            					{
	                 					m_From.SendMessage("Bad poisoning entry. A number was expected.");
	            					}
	            				}
            					TextRelay mag = null;
            					if ( info.GetTextEntry( 6 ) != null )
            						mag = info.GetTextEntry( 6 );
            					if ( mag != null )
            					{
	            					try
	           		 				{
	           		 					Magery = Convert.ToDouble(mag.Text);
	            					}
	            					catch
	            					{
	                 					m_From.SendMessage("Bad magery entry. A number was expected.");
	            					}
	            				}
            					TextRelay e = null;
            					if ( info.GetTextEntry( 7 ) != null )
            						e = info.GetTextEntry( 7 );
            					if ( e != null )
            					{
	            					try
	           		 				{
	           		 					EvaluatingIntelligence = Convert.ToDouble(e.Text);
	            					}
	            					catch
	            					{
	                 					m_From.SendMessage("Bad evaluating intelligence entry. A number was expected.");
	            					}
            					}
            					TextRelay med = null;
            					if ( info.GetTextEntry( 8 ) != null )
            						med = info.GetTextEntry( 8 );
            					if ( med != null )
            					{
	            					try
	           		 				{
	           		 					Meditation = Convert.ToDouble(med.Text);
	            					}
	            					catch
	            					{
	                 					m_From.SendMessage("Bad meditation entry. A number was expected.");
	            					}
	            				}
	            if ( !m_Pet.Deleted || m_Pet.Alive == true )
	            {
					if ( m_Pet.Controlled && m_Pet.ControlMaster == m_From )
					{
						if(m_Ball.IsChildOf( m_From.Backpack ))
						{
							if(( Wrestling >= 0 && Wrestling <= 100 ) &&
							   ( Tactics >= 0 && Tactics <= 100 ) &&
							   ( ResistingSpells >= 0 && ResistingSpells <= 100 ) &&
							   ( Anatomy >= 0 && Anatomy <= 100 ) &&
							   ( Poisoning >= 0 && Poisoning <= 100 ) &&
							   ( Magery >= 0 && Magery <= 100 ) &&
							   ( EvaluatingIntelligence >= 0 && EvaluatingIntelligence <= 100 ) &&
							   ( Meditation >= 0 && Meditation <= 100 ))
							{
								m_Pet.Skills[SkillName.Wrestling].Base = Wrestling;
								m_Pet.Skills[SkillName.Tactics].Base = Tactics;
								m_Pet.Skills[SkillName.MagicResist].Base = ResistingSpells;
								m_Pet.Skills[SkillName.Anatomy].Base = Anatomy;
								m_Pet.Skills[SkillName.Poisoning].Base = Poisoning;
								m_Pet.Skills[SkillName.Magery].Base = Magery;
								m_Pet.Skills[SkillName.EvalInt].Base = EvaluatingIntelligence;
								m_Pet.Skills[SkillName.Meditation].Base = Meditation;
							
								m_Ball.Delete();
								
								m_From.SendMessage( "The skills of {0} are changed!", m_Pet.Name );
							}
							else
							{ 
								m_From.SendMessage( "Your skill values are too high or too low keep it between 0 and 100.  Please try again!" );
							}	
						}
						else
						{
							m_From.SendMessage( "The Skill Ball must remain in your Backpack.  Please try again!" );
						}
					}
					else
					{
						m_From.SendMessage( "You do not control that pet.  Please try again!" );
					}
				}
				else
				{
					m_From.SendMessage( "Your pet has died...  Please try again!" );
				}
			}
		}				
	}
}