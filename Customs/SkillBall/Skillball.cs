
using System;
using Server;
using Server.Network;
using Server.Mobiles;
using Server.Items;
using Server.Gumps;
using Server.Misc;

namespace Server
{
	public class TemplatePickGump : Gump
	{
        private SkillBall m_SkillBall;
        
        public TemplatePickGump( SkillBall ball ) : base( 0,0 )
		{
			m_SkillBall = ball;
			
			this.Closable=true;
			this.Disposable=true;
			this.Dragable=true;
			this.Resizable=false;
			this.AddPage(0);
			this.AddBackground(35, 39, 146, 156, 9200);
			this.AddBackground(49, 48, 114, 132, 9350);
			this.AddLabel(99, 57, 0, @"Tanks");
			this.AddLabel(99, 77, 0, @"Mages");
			this.AddLabel(99, 97, 0, @"Dexxers");
			this.AddLabel(99, 117, 0, @"Tamers");
			this.AddButton(71, 59, 2117, 2118, 1, GumpButtonType.Reply, 0);
			this.AddButton(71, 79, 2117, 2118, 2, GumpButtonType.Reply, 0);
			this.AddButton(71, 99, 2117, 2118, 3, GumpButtonType.Reply, 0);
			this.AddButton(71, 119, 2117, 2118, 4, GumpButtonType.Reply, 0);
			this.AddButton(91, 146, 4014, 4015, 5, GumpButtonType.Reply, 0);
		}
		
		public override void OnResponse( NetState state, RelayInfo info )
		{
			Mobile m = state.Mobile;
			
			switch( info.ButtonID )
			{
				case 0: {break;}
				case 1: //Tanks
				{
                	m.CloseGump(typeof(TemplatePickGump));
                	if ( m_SkillBall.IsChildOf( m.Backpack ))
                    {
                    	m.SendGump( new TankPickGump(m_SkillBall) );
                   		
                    }
                    break;
				}
                case 2: //Mages
                {
                	m.CloseGump(typeof(TemplatePickGump));
                	if ( m_SkillBall.IsChildOf( m.Backpack ))
                    {
                    	m.SendGump( new MagePickGump(m_SkillBall) );
                   		
                    } 
                    break;
                }
                case 3: //Dexxers
                {
                	m.CloseGump(typeof(TemplatePickGump));
                	if ( m_SkillBall.IsChildOf( m.Backpack ))
                    {
                    	m.SendGump( new DexPickGump(m_SkillBall) );
                   		
                    }
                    break;
                }
                case 4: //Tamers
                {
                	m.CloseGump(typeof(TemplatePickGump));
                	if ( m_SkillBall.IsChildOf( m.Backpack ))
                    {
                    	m.SendGump( new TamerPickGump(m_SkillBall) );
                   		
                    }
                    break;
                }
                case 5: //close
                {
                	m.CloseGump(typeof(TemplatePickGump));
                	if ( m_SkillBall.IsChildOf( m.Backpack ))
                    {
                    	m.SendGump( new SkillPickGump(m_SkillBall) );
                   		
                    }
                    break;
                }
			}  
		}
	}
	
	public class TankPickGump : Gump
	{
		private SkillBall  m_SkillBall;
		private double val = 100;
        
        public TankPickGump( SkillBall ball ) : base( 0,0 )
		{
			m_SkillBall = ball;
		
			this.Closable=true;
			this.Disposable=true;
			this.Dragable=true;
			this.Resizable=false;
			this.AddPage(0);
			this.AddBackground(35, 39, 181, 156, 9200);
			this.AddBackground(49, 48, 152, 132, 9350);
			this.AddLabel(99, 57, 0, @"Sword Mage");
			this.AddLabel(99, 77, 0, @"Macer Mage");
			this.AddLabel(99, 97, 0, @"Fencer Mage");
			this.AddLabel(99, 117, 0, @"Archer Mage");
			this.AddButton(71, 59, 2117, 2118, 1, GumpButtonType.Reply, 0);
			this.AddButton(71, 79, 2117, 2118, 2, GumpButtonType.Reply, 0);
			this.AddButton(71, 99, 2117, 2118, 3, GumpButtonType.Reply, 0);
			this.AddButton(71, 119, 2117, 2118, 4, GumpButtonType.Reply, 0);
			this.AddButton(107, 146, 4014, 4015, 5, GumpButtonType.Reply, 0);
		}
		
		public override void OnResponse( NetState state, RelayInfo info )
		{
			Mobile m = state.Mobile;
			
			Server.Skills skills = m.Skills;
			
			switch( info.ButtonID )
			{
				case 0: {break;}
				case 1: //Sword Mage
				{
                	m.CloseGump(typeof(TankPickGump));
                	if ( m_SkillBall.IsChildOf( m.Backpack ))
                    {
                        for (int i = 0; i < skills.Length; ++i)
                            skills[i].Base = 0;
                            
                    	m.Skills[SkillName.Swords].Base = val;
                   		m.Skills[SkillName.Tactics].Base = val;
                   		m.Skills[SkillName.Anatomy].Base = val;
                   		m.Skills[SkillName.Magery].Base = val;
                   		m.Skills[SkillName.MagicResist].Base = val;
                   		m.Skills[SkillName.Meditation].Base = val;
                   		m.Skills[SkillName.EvalInt].Base = val;
                   		
                   		m_SkillBall.Delete();
                   		
                    }
                    break;
				}
                case 2: //Macer Mage
                {
                	m.CloseGump(typeof(TankPickGump));
                	if ( m_SkillBall.IsChildOf( m.Backpack ))
                    {
                        for (int i = 0; i < skills.Length; ++i)
                            skills[i].Base = 0;
                            
                    	m.Skills[SkillName.Macing].Base = val;
                   		m.Skills[SkillName.Tactics].Base = val;
                   		m.Skills[SkillName.Anatomy].Base = val;
                   		m.Skills[SkillName.Magery].Base = val;
                   		m.Skills[SkillName.MagicResist].Base = val;
                   		m.Skills[SkillName.Meditation].Base = val;
                   		m.Skills[SkillName.EvalInt].Base = val;
                   		
                   		m_SkillBall.Delete();
                   		
                    } 
                    break;
                }
                case 3: //Fencer Mage
                {
                	m.CloseGump(typeof(TankPickGump));
                	if ( m_SkillBall.IsChildOf( m.Backpack ))
                    {
                        for (int i = 0; i < skills.Length; ++i)
                            skills[i].Base = 0;
                            
                    	m.Skills[SkillName.Fencing].Base = val;
                   		m.Skills[SkillName.Tactics].Base = val;
                   		m.Skills[SkillName.Anatomy].Base = val;
                   		m.Skills[SkillName.Magery].Base = val;
                   		m.Skills[SkillName.MagicResist].Base = val;
                   		m.Skills[SkillName.Meditation].Base = val;
                   		m.Skills[SkillName.EvalInt].Base = val;
                   		
                   		m_SkillBall.Delete();
                   		
                    }
                    break;
                }
                case 4: //Archer Mage
                {
                	m.CloseGump(typeof(TankPickGump));
                	if ( m_SkillBall.IsChildOf( m.Backpack ))
                    {
                    	for (int i = 0; i < skills.Length; ++i)
                            skills[i].Base = 0;
                            
                    	m.Skills[SkillName.Archery].Base = val;
                   		m.Skills[SkillName.Tactics].Base = val;
                   		m.Skills[SkillName.Anatomy].Base = val;
                   		m.Skills[SkillName.Magery].Base = val;
                   		m.Skills[SkillName.MagicResist].Base = val;
                   		m.Skills[SkillName.Meditation].Base = val;
                   		m.Skills[SkillName.EvalInt].Base = val;
                   		
                   		m_SkillBall.Delete();
                   		
                    }
                    break;
                }
                case 5: //close
                {
                	m.CloseGump(typeof(TankPickGump));
                	if ( m_SkillBall.IsChildOf( m.Backpack ))
                    {
                    	m.SendGump( new TemplatePickGump(m_SkillBall) );
                   		
                    }
                    break;
                }
			}  
		}
		
	}
	
	public class MagePickGump : Gump
	{
		private SkillBall  m_SkillBall;
		private double val = 100;
		
		public MagePickGump( SkillBall ball ) : base(0,0)
		{
			m_SkillBall = ball;
		
			this.Closable=true;
			this.Disposable=true;
			this.Dragable=true;
			this.Resizable=false;
			this.AddPage(0);
			this.AddBackground(35, 39, 210, 257, 9200);
			this.AddBackground(49, 48, 183, 237, 9350);
			this.AddLabel(99, 56, 0, @"Duelist");
			this.AddLabel(99, 76, 0, @"Heal-Mage");
			this.AddLabel(99, 96, 0, @"Scribe-Mage");
			this.AddLabel(99, 116, 0, @"Scribe-Alchy-Mage");
			this.AddLabel(99, 136, 0, @"Alchy-Stun-Mage");
			this.AddLabel(99, 156, 0, @"Hiding-Mage");
			this.AddLabel(99, 176, 0, @"Stealth-Mage");
			this.AddLabel(99, 196, 0, @"Poisonner");
			this.AddLabel(99, 216, 0, @"Parry Mage");
			this.AddButton(71, 59, 2117, 2118, 1, GumpButtonType.Reply, 0);
			this.AddButton(71, 79, 2117, 2118, 2, GumpButtonType.Reply, 0);
			this.AddButton(71, 99, 2117, 2118, 3, GumpButtonType.Reply, 0);
			this.AddButton(71, 119, 2117, 2118, 4, GumpButtonType.Reply, 0);
			this.AddButton(71, 139, 2117, 2118, 5, GumpButtonType.Reply, 0);
			this.AddButton(71, 159, 2117, 2118, 6, GumpButtonType.Reply, 0);
			this.AddButton(71, 179, 2117, 2118, 7, GumpButtonType.Reply, 0);
			this.AddButton(71, 199, 2117, 2118, 8, GumpButtonType.Reply, 0);
			this.AddButton(71, 219, 2117, 2118, 9, GumpButtonType.Reply, 0);
			this.AddButton(124, 249, 4014, 4015, 10, GumpButtonType.Reply, 0);
		}
		
		public override void OnResponse( NetState state, RelayInfo info )
		{
			Mobile m = state.Mobile;
			
			Server.Skills skills = m.Skills;
			
			switch( info.ButtonID )
			{
				case 0: {break;}
				case 1: //Duelist
				{
                	m.CloseGump(typeof(MagePickGump));
                	if ( m_SkillBall.IsChildOf( m.Backpack ))
                    {
                        for (int i = 0; i < skills.Length; ++i)
                            skills[i].Base = 0;
                            
                    	m.Skills[SkillName.Wrestling].Base = val;
                   		m.Skills[SkillName.Tactics].Base = val;
                   		m.Skills[SkillName.Anatomy].Base = val;
                   		m.Skills[SkillName.Magery].Base = val;
                   		m.Skills[SkillName.MagicResist].Base = val;
                   		m.Skills[SkillName.Meditation].Base = val;
                   		m.Skills[SkillName.EvalInt].Base = val;
                   		
                   		m_SkillBall.Delete();
                   		
                    }
                    break;
				}
                case 2: //Heal-Mage
                {
                	m.CloseGump(typeof(MagePickGump));
                	if ( m_SkillBall.IsChildOf( m.Backpack ))
                    {
                        for (int i = 0; i < skills.Length; ++i)
                            skills[i].Base = 0;
                            
                    	m.Skills[SkillName.Wrestling].Base = val;
                   		m.Skills[SkillName.Healing].Base = val;
                   		m.Skills[SkillName.Anatomy].Base = val;
                   		m.Skills[SkillName.Magery].Base = val;
                   		m.Skills[SkillName.MagicResist].Base = val;
                   		m.Skills[SkillName.Meditation].Base = val;
                   		m.Skills[SkillName.EvalInt].Base = val;
                   		
                   		m_SkillBall.Delete();
                   		
                    } 
                    break;
                }
                case 3: //Scribe-Mage
                {
                	m.CloseGump(typeof(MagePickGump));
                	if ( m_SkillBall.IsChildOf( m.Backpack ))
                    {
                        for (int i = 0; i < skills.Length; ++i)
                            skills[i].Base = 0;
                            
                    	m.Skills[SkillName.Wrestling].Base = val;
                   		m.Skills[SkillName.Inscribe].Base = val;
                   		m.Skills[SkillName.Anatomy].Base = val;
                   		m.Skills[SkillName.Magery].Base = val;
                   		m.Skills[SkillName.MagicResist].Base = val;
                   		m.Skills[SkillName.Meditation].Base = val;
                   		m.Skills[SkillName.EvalInt].Base = val;
                   		
                   		m_SkillBall.Delete();
                   		
                    }
                    break;
                }
                case 4: //Scribe-Alchy-Mage
                {
                	m.CloseGump(typeof(MagePickGump));
                	if ( m_SkillBall.IsChildOf( m.Backpack ))
                    {
                    	for (int i = 0; i < skills.Length; ++i)
                            skills[i].Base = 0;
                            
                    	m.Skills[SkillName.Archery].Base = val;
                   		m.Skills[SkillName.Alchemy].Base = val;
                   		m.Skills[SkillName.Inscribe].Base = val;
                   		m.Skills[SkillName.Magery].Base = val;
                   		m.Skills[SkillName.MagicResist].Base = val;
                   		m.Skills[SkillName.Meditation].Base = val;
                   		m.Skills[SkillName.EvalInt].Base = val;
                   		
                   		m_SkillBall.Delete();
                   		
                    }
                    break;
                }
                case 5: //Alchy-Stun-Mage
                {
                	m.CloseGump(typeof(MagePickGump));
                	if ( m_SkillBall.IsChildOf( m.Backpack ))
                    {
                    	for (int i = 0; i < skills.Length; ++i)
                            skills[i].Base = 0;
                            
                    	m.Skills[SkillName.Wrestling].Base = val;
                   		m.Skills[SkillName.Alchemy].Base = val;
                   		m.Skills[SkillName.Anatomy].Base = val;
                   		m.Skills[SkillName.Magery].Base = val;
                   		m.Skills[SkillName.MagicResist].Base = val;
                   		m.Skills[SkillName.Meditation].Base = val;
                   		m.Skills[SkillName.EvalInt].Base = val;
                   		
                   		m_SkillBall.Delete();
                   		
                    }
                    break;
                }
                case 6: //Hiding-Mage
                {
                	m.CloseGump(typeof(MagePickGump));
                	if ( m_SkillBall.IsChildOf( m.Backpack ))
                    {
                    	for (int i = 0; i < skills.Length; ++i)
                            skills[i].Base = 0;
                            
                    	m.Skills[SkillName.Wrestling].Base = val;
                   		m.Skills[SkillName.Hiding].Base = val;
                   		m.Skills[SkillName.Anatomy].Base = val;
                   		m.Skills[SkillName.Magery].Base = val;
                   		m.Skills[SkillName.MagicResist].Base = val;
                   		m.Skills[SkillName.Meditation].Base = val;
                   		m.Skills[SkillName.EvalInt].Base = val;
                   		
                   		m_SkillBall.Delete();
                   		
                    }
                    break;
                }
                case 7: //Stealth-Mage
                {
                	m.CloseGump(typeof(MagePickGump));
                	if ( m_SkillBall.IsChildOf( m.Backpack ))
                    {
                    	for (int i = 0; i < skills.Length; ++i)
                            skills[i].Base = 0;
                            
                    	m.Skills[SkillName.Wrestling].Base = val;
                   		m.Skills[SkillName.Hiding].Base = val;
                   		m.Skills[SkillName.Stealth].Base = val;
                   		m.Skills[SkillName.Magery].Base = val;
                   		m.Skills[SkillName.MagicResist].Base = val;
                   		m.Skills[SkillName.Meditation].Base = val;
                   		m.Skills[SkillName.EvalInt].Base = val;
                   		
                   		m_SkillBall.Delete();
                   		
                    }
                    break;
                }
                case 8: //Poisonner
                {
                	m.CloseGump(typeof(MagePickGump));
                	if ( m_SkillBall.IsChildOf( m.Backpack ))
                    {
                    	for (int i = 0; i < skills.Length; ++i)
                            skills[i].Base = 0;
                            
                    	m.Skills[SkillName.Wrestling].Base = val;
                   		m.Skills[SkillName.Poisoning].Base = val;
                   		m.Skills[SkillName.Anatomy].Base = val;
                   		m.Skills[SkillName.Magery].Base = val;
                   		m.Skills[SkillName.MagicResist].Base = val;
                   		m.Skills[SkillName.Meditation].Base = val;
                   		m.Skills[SkillName.EvalInt].Base = val;
                   		
                   		m_SkillBall.Delete();
                   		
                    }
                    break;
                }
                case 9: //Parry Mage
                {
                	m.CloseGump(typeof(MagePickGump));
                	if ( m_SkillBall.IsChildOf( m.Backpack ))
                    {
                    	for (int i = 0; i < skills.Length; ++i)
                            skills[i].Base = 0;
                            
                    	m.Skills[SkillName.Wrestling].Base = val;
                   		m.Skills[SkillName.Parry].Base = val;
                   		m.Skills[SkillName.Anatomy].Base = val;
                   		m.Skills[SkillName.Magery].Base = val;
                   		m.Skills[SkillName.MagicResist].Base = val;
                   		m.Skills[SkillName.Meditation].Base = val;
                   		m.Skills[SkillName.EvalInt].Base = val;
                   		
                   		m_SkillBall.Delete();
                   		
                    }
                    break;
                }
                case 10: //close
                {
                	m.CloseGump(typeof(MagePickGump));
                	if ( m_SkillBall.IsChildOf( m.Backpack ))
                    {
                    	m.SendGump( new TemplatePickGump(m_SkillBall) );
                   		
                    }
                    break;
                }
			}  
		}
	}
	
	public class DexPickGump : Gump 
	{
		private SkillBall  m_SkillBall;
		private double val = 100;
		
		public DexPickGump( SkillBall ball ) : base(0,0)
		{
			m_SkillBall = ball;
			
			this.Closable=true;
			this.Disposable=true;
			this.Dragable=true;
			this.Resizable=false;
			this.AddPage(0);
			this.AddBackground(35, 39, 239, 412, 9200);
			this.AddBackground(49, 48, 211, 386, 9350);
			this.AddLabel(99, 56, 0, @"Lumberjacker");
			this.AddLabel(99, 76, 0, @"Alchy-Dex-Swordsman");
			this.AddLabel(99, 96, 0, @"Alchy-Dex-Fencer");
			this.AddLabel(99, 116, 0, @"Alchy-Dex-Macer");
			this.AddLabel(99, 136, 0, @"Alchy-Dex-Archer");
			this.AddLabel(99, 156, 0, @"Axer with hiding");
			this.AddLabel(99, 176, 0, @"Fencer with hiding");
			this.AddLabel(99, 196, 0, @"Macer with hiding");
			this.AddLabel(99, 216, 0, @"Archer with hiding");
			this.AddLabel(99, 236, 0, @"Fencer with parrying");
			this.AddLabel(99, 256, 0, @"Swordsman with parrying");
			this.AddLabel(99, 276, 0, @"Macer with parrying");
			this.AddLabel(99, 296, 0, @"Scribe Fencer");
			this.AddLabel(99, 316, 0, @"Scribe Swordsman");
			this.AddLabel(99, 336, 0, @"Scribe Macer");
			this.AddLabel(99, 356, 0, @"Scribe Archer");
			this.AddButton(71, 59, 2117, 2118, 1, GumpButtonType.Reply, 0);
			this.AddButton(71, 79, 2117, 2118, 2, GumpButtonType.Reply, 0);
			this.AddButton(71, 99, 2117, 2118, 3, GumpButtonType.Reply, 0);
			this.AddButton(71, 119, 2117, 2118, 4, GumpButtonType.Reply, 0);
			this.AddButton(71, 139, 2117, 2118, 5, GumpButtonType.Reply, 0);
			this.AddButton(71, 159, 2117, 2118, 6, GumpButtonType.Reply, 0);
			this.AddButton(71, 179, 2117, 2118, 7, GumpButtonType.Reply, 0);
			this.AddButton(71, 199, 2117, 2118, 8, GumpButtonType.Reply, 0);
			this.AddButton(71, 219, 2117, 2118, 9, GumpButtonType.Reply, 0);
			this.AddButton(71, 239, 2117, 2118, 10, GumpButtonType.Reply, 0);
			this.AddButton(71, 259, 2117, 2118, 11, GumpButtonType.Reply, 0);
			this.AddButton(71, 279, 2117, 2118, 12, GumpButtonType.Reply, 0);
			this.AddButton(71, 299, 2117, 2118, 13, GumpButtonType.Reply, 0);
			this.AddButton(71, 319, 2117, 2118, 14, GumpButtonType.Reply, 0);
			this.AddButton(71, 339, 2117, 2118, 15, GumpButtonType.Reply, 0);
			this.AddButton(71, 358, 2117, 2118, 16, GumpButtonType.Reply, 0);
			this.AddButton(141, 390, 4014, 4015, 17, GumpButtonType.Reply, 0);
		}
	
		public override void OnResponse( NetState state, RelayInfo info )
		{
			Mobile m = state.Mobile;
			
			Server.Skills skills = m.Skills;
			
			switch( info.ButtonID )
			{
				case 0: {break;}
				case 1: //Lumberjacker
				{
                	m.CloseGump(typeof(DexPickGump));
                	if ( m_SkillBall.IsChildOf( m.Backpack ))
                    {
                        for (int i = 0; i < skills.Length; ++i)
                            skills[i].Base = 0;
                            
                    	m.Skills[SkillName.Swords].Base = val;
                   		m.Skills[SkillName.Tactics].Base = val;
                   		m.Skills[SkillName.Anatomy].Base = val;
                   		m.Skills[SkillName.Magery].Base = val;
                   		m.Skills[SkillName.Healing].Base = val;
                   		m.Skills[SkillName.MagicResist].Base = val;
                   		m.Skills[SkillName.Lumberjacking].Base = val;
                   		
                   		m_SkillBall.Delete();
                   		
                    }
                    break;
				}
                case 2: //Alchy-Dex-Swordsman
                {
                	m.CloseGump(typeof(DexPickGump));
                	if ( m_SkillBall.IsChildOf( m.Backpack ))
                    {
                        for (int i = 0; i < skills.Length; ++i)
                            skills[i].Base = 0;
                            
                    	m.Skills[SkillName.Swords].Base = val;
                   		m.Skills[SkillName.Tactics].Base = val;
                   		m.Skills[SkillName.Anatomy].Base = val;
                   		m.Skills[SkillName.Magery].Base = val;
                   		m.Skills[SkillName.Healing].Base = val;
                   		m.Skills[SkillName.MagicResist].Base = val;
                   		m.Skills[SkillName.Alchemy].Base = val;
                   		
                   		m_SkillBall.Delete();
                   		
                    } 
                    break;
                }
                case 3: //Alchy-Dex-Fencer
                {
                	m.CloseGump(typeof(DexPickGump));
                	if ( m_SkillBall.IsChildOf( m.Backpack ))
                    {
                        for (int i = 0; i < skills.Length; ++i)
                            skills[i].Base = 0;
                            
                    	m.Skills[SkillName.Fencing].Base = val;
                   		m.Skills[SkillName.Tactics].Base = val;
                   		m.Skills[SkillName.Anatomy].Base = val;
                   		m.Skills[SkillName.Magery].Base = val;
                   		m.Skills[SkillName.Healing].Base = val;
                   		m.Skills[SkillName.MagicResist].Base = val;
                   		m.Skills[SkillName.Alchemy].Base = val;
                   		
                   		m_SkillBall.Delete();
                   		
                    }
                    break;
                }
                case 4: //Alchy-Dex-Macer
                {
                	m.CloseGump(typeof(DexPickGump));
                	if ( m_SkillBall.IsChildOf( m.Backpack ))
                    {
                    	for (int i = 0; i < skills.Length; ++i)
                            skills[i].Base = 0;
                            
                    	m.Skills[SkillName.Macing].Base = val;
                   		m.Skills[SkillName.Tactics].Base = val;
                   		m.Skills[SkillName.Anatomy].Base = val;
                   		m.Skills[SkillName.Magery].Base = val;
                   		m.Skills[SkillName.Healing].Base = val;
                   		m.Skills[SkillName.MagicResist].Base = val;
                   		m.Skills[SkillName.Alchemy].Base = val;
                   		
                   		m_SkillBall.Delete();
                   		
                    }
                    break;
                }
                case 5: //Alchy-Dex-Archer
                {
                	m.CloseGump(typeof(DexPickGump));
                	if ( m_SkillBall.IsChildOf( m.Backpack ))
                    {
                    	for (int i = 0; i < skills.Length; ++i)
                            skills[i].Base = 0;
                            
                    	m.Skills[SkillName.Archery].Base = val;
                   		m.Skills[SkillName.Tactics].Base = val;
                   		m.Skills[SkillName.Anatomy].Base = val;
                   		m.Skills[SkillName.Magery].Base = val;
                   		m.Skills[SkillName.Healing].Base = val;
                   		m.Skills[SkillName.MagicResist].Base = val;
                   		m.Skills[SkillName.Alchemy].Base = val;
                   		
                   		m_SkillBall.Delete();
                   		
                    }
                    break;
                }
                case 6: //Axer with hiding
                {
                	m.CloseGump(typeof(DexPickGump));
                	if ( m_SkillBall.IsChildOf( m.Backpack ))
                    {
                    	for (int i = 0; i < skills.Length; ++i)
                            skills[i].Base = 0;
                            
                    	m.Skills[SkillName.Swords].Base = val;
                   		m.Skills[SkillName.Tactics].Base = val;
                   		m.Skills[SkillName.Anatomy].Base = val;
                   		m.Skills[SkillName.Magery].Base = val;
                   		m.Skills[SkillName.Healing].Base = val;
                   		m.Skills[SkillName.MagicResist].Base = val;
                   		m.Skills[SkillName.Hiding].Base = val;
                   		
                   		m_SkillBall.Delete();
                   		
                    }
                    break;
                }
                case 7: //Fencer with hiding
                {
                	m.CloseGump(typeof(DexPickGump));
                	if ( m_SkillBall.IsChildOf( m.Backpack ))
                    {
                    	for (int i = 0; i < skills.Length; ++i)
                            skills[i].Base = 0;
                            
                    	m.Skills[SkillName.Fencing].Base = val;
                   		m.Skills[SkillName.Tactics].Base = val;
                   		m.Skills[SkillName.Anatomy].Base = val;
                   		m.Skills[SkillName.Magery].Base = val;
                   		m.Skills[SkillName.Healing].Base = val;
                   		m.Skills[SkillName.MagicResist].Base = val;
                   		m.Skills[SkillName.Hiding].Base = val;
                   		
                   		m_SkillBall.Delete();
                   		
                    }
                    break;
                }
                case 8: //Macer with hiding
                {
                	m.CloseGump(typeof(DexPickGump));
                	if ( m_SkillBall.IsChildOf( m.Backpack ))
                    {
                    	for (int i = 0; i < skills.Length; ++i)
                            skills[i].Base = 0;
                            
                    	m.Skills[SkillName.Macing].Base = val;
                   		m.Skills[SkillName.Tactics].Base = val;
                   		m.Skills[SkillName.Anatomy].Base = val;
                   		m.Skills[SkillName.Magery].Base = val;
                   		m.Skills[SkillName.Healing].Base = val;
                   		m.Skills[SkillName.MagicResist].Base = val;
                   		m.Skills[SkillName.Hiding].Base = val;
                   		
                   		m_SkillBall.Delete();
                   		
                    }
                    break;
                }
                case 9: //Archer with hiding
                {
                	m.CloseGump(typeof(DexPickGump));
                	if ( m_SkillBall.IsChildOf( m.Backpack ))
                    {
                    	for (int i = 0; i < skills.Length; ++i)
                            skills[i].Base = 0;
                            
                    	m.Skills[SkillName.Archery].Base = val;
                   		m.Skills[SkillName.Tactics].Base = val;
                   		m.Skills[SkillName.Anatomy].Base = val;
                   		m.Skills[SkillName.Magery].Base = val;
                   		m.Skills[SkillName.Healing].Base = val;
                   		m.Skills[SkillName.MagicResist].Base = val;
                   		m.Skills[SkillName.Hiding].Base = val;
                   		
                   		m_SkillBall.Delete();
                   		
                    }
                    break;
                }
                case 10: //Fencer with parrying
                {
                	m.CloseGump(typeof(DexPickGump));
                	if ( m_SkillBall.IsChildOf( m.Backpack ))
                    {
                    	for (int i = 0; i < skills.Length; ++i)
                            skills[i].Base = 0;
                            
                    	m.Skills[SkillName.Fencing].Base = val;
                   		m.Skills[SkillName.Tactics].Base = val;
                   		m.Skills[SkillName.Anatomy].Base = val;
                   		m.Skills[SkillName.Magery].Base = val;
                   		m.Skills[SkillName.Healing].Base = val;
                   		m.Skills[SkillName.MagicResist].Base = val;
                   		m.Skills[SkillName.Parry].Base = val;
                   		
                   		m_SkillBall.Delete();
                   		
                    }
                    break;                		
                }
                case 11: //Swordsman with parrying
                {
                	m.CloseGump(typeof(DexPickGump));
                	if ( m_SkillBall.IsChildOf( m.Backpack ))
                    {
                    	for (int i = 0; i < skills.Length; ++i)
                            skills[i].Base = 0;
                            
                    	m.Skills[SkillName.Swords].Base = val;
                   		m.Skills[SkillName.Tactics].Base = val;
                   		m.Skills[SkillName.Anatomy].Base = val;
                   		m.Skills[SkillName.Magery].Base = val;
                   		m.Skills[SkillName.Healing].Base = val;
                   		m.Skills[SkillName.MagicResist].Base = val;
                   		m.Skills[SkillName.Parry].Base = val;
                   		
                   		m_SkillBall.Delete();
                   		
                    }
                    break;      
                }
                case 12: //Macer with parrying
                {
                	m.CloseGump(typeof(DexPickGump));
                	if ( m_SkillBall.IsChildOf( m.Backpack ))
                    {
                    	for (int i = 0; i < skills.Length; ++i)
                            skills[i].Base = 0;
                            
                    	m.Skills[SkillName.Macing].Base = val;
                   		m.Skills[SkillName.Tactics].Base = val;
                   		m.Skills[SkillName.Anatomy].Base = val;
                   		m.Skills[SkillName.Magery].Base = val;
                   		m.Skills[SkillName.Healing].Base = val;
                   		m.Skills[SkillName.MagicResist].Base = val;
                   		m.Skills[SkillName.Parry].Base = val;
                   		
                   		m_SkillBall.Delete();
                   		
                    }
                    break;  
                }
                case 13: //Scribe Fencer
                {
                	m.CloseGump(typeof(DexPickGump));
                	if ( m_SkillBall.IsChildOf( m.Backpack ))
                    {
                    	for (int i = 0; i < skills.Length; ++i)
                            skills[i].Base = 0;
                            
                    	m.Skills[SkillName.Fencing].Base = val;
                   		m.Skills[SkillName.Tactics].Base = val;
                   		m.Skills[SkillName.Anatomy].Base = val;
                   		m.Skills[SkillName.Magery].Base = val;
                   		m.Skills[SkillName.Healing].Base = val;
                   		m.Skills[SkillName.MagicResist].Base = val;
                   		m.Skills[SkillName.Inscribe].Base = val;
                   		
                   		m_SkillBall.Delete();
                   		
                    }
                    break;  
                }
                case 14: //Scribe Swordsman
                {
                	m.CloseGump(typeof(DexPickGump));
                	if ( m_SkillBall.IsChildOf( m.Backpack ))
                    {
                    	for (int i = 0; i < skills.Length; ++i)
                            skills[i].Base = 0;
                            
                    	m.Skills[SkillName.Swords].Base = val;
                   		m.Skills[SkillName.Tactics].Base = val;
                   		m.Skills[SkillName.Anatomy].Base = val;
                   		m.Skills[SkillName.Magery].Base = val;
                   		m.Skills[SkillName.Healing].Base = val;
                   		m.Skills[SkillName.MagicResist].Base = val;
                   		m.Skills[SkillName.Inscribe].Base = val;
                   		
                   		m_SkillBall.Delete();
                   		
                    }
                    break;  
                }
                case 15: //Scribe Macer
                {
                	m.CloseGump(typeof(DexPickGump));
                	if ( m_SkillBall.IsChildOf( m.Backpack ))
                    {
                    	for (int i = 0; i < skills.Length; ++i)
                            skills[i].Base = 0;
                            
                    	m.Skills[SkillName.Macing].Base = val;
                   		m.Skills[SkillName.Tactics].Base = val;
                   		m.Skills[SkillName.Anatomy].Base = val;
                   		m.Skills[SkillName.Magery].Base = val;
                   		m.Skills[SkillName.Healing].Base = val;
                   		m.Skills[SkillName.MagicResist].Base = val;
                   		m.Skills[SkillName.Inscribe].Base = val;
                   		
                   		m_SkillBall.Delete();
                   		
                    }
                    break;  
                }
                case 16: //Scribe Archer
                {
                	m.CloseGump(typeof(DexPickGump));
                	if ( m_SkillBall.IsChildOf( m.Backpack ))
                    {
                    	for (int i = 0; i < skills.Length; ++i)
                            skills[i].Base = 0;
                            
                    	m.Skills[SkillName.Archery].Base = val;
                   		m.Skills[SkillName.Tactics].Base = val;
                   		m.Skills[SkillName.Anatomy].Base = val;
                   		m.Skills[SkillName.Magery].Base = val;
                   		m.Skills[SkillName.Healing].Base = val;
                   		m.Skills[SkillName.MagicResist].Base = val;
                   		m.Skills[SkillName.Inscribe].Base = val;
                   		
                   		m_SkillBall.Delete();
                   		
                    }
                    break;  
                }
                case 17: //close
                {
                	m.CloseGump(typeof(DexPickGump));
                	if ( m_SkillBall.IsChildOf( m.Backpack ))
                    {
                    	m.SendGump( new TemplatePickGump(m_SkillBall) );
                   		
                    }
                    break;
                }
			}  
		}
	
	}
	
	public class TamerPickGump : Gump
	{
		private SkillBall  m_SkillBall;
		private double val = 100;
		
		public TamerPickGump( SkillBall ball ) : base(0,0)
		{
			m_SkillBall = ball;
			
			this.Closable=true;
			this.Disposable=true;
			this.Dragable=true;
			this.Resizable=false;
			this.AddPage(0);
			this.AddBackground(35, 39, 183, 188, 9200);
			this.AddBackground(49, 48, 156, 168, 9350);
			this.AddLabel(99, 56, 0, @"Peacer-Vet");
			this.AddLabel(99, 76, 0, @"Peacer-Med");
			this.AddLabel(99, 96, 0, @"PvP Tamer-Vet");
			this.AddLabel(99, 116, 0, @"Bola Tamer");
			this.AddLabel(99, 136, 0, @"Scribe Tamer");
			this.AddLabel(99, 156, 0, @"Alchy Tamer");
			this.AddButton(71, 59, 2117, 2118, 1, GumpButtonType.Reply, 0);
			this.AddButton(71, 79, 2117, 2118, 2, GumpButtonType.Reply, 0);
			this.AddButton(71, 99, 2117, 2118, 3, GumpButtonType.Reply, 0);
			this.AddButton(71, 119, 2117, 2118, 4, GumpButtonType.Reply, 0);
			this.AddButton(71, 139, 2117, 2118, 5, GumpButtonType.Reply, 0);
			this.AddButton(71, 159, 2117, 2118, 6, GumpButtonType.Reply, 0);
			this.AddButton(112, 183, 4014, 4015, 7, GumpButtonType.Reply, 0);
			
		}
	
		public override void OnResponse( NetState state, RelayInfo info )
		{
			Mobile m = state.Mobile;
			
			Server.Skills skills = m.Skills;
			
			switch( info.ButtonID )
			{
				case 0: {break;}
				case 1: //Peacer-Vet
				{
                	m.CloseGump(typeof(TamerPickGump));
                	if ( m_SkillBall.IsChildOf( m.Backpack ))
                    {
                        for (int i = 0; i < skills.Length; ++i)
                            skills[i].Base = 0;
                            
                    	m.Skills[SkillName.AnimalTaming].Base = val;
                   		m.Skills[SkillName.AnimalLore].Base = val;
                   		m.Skills[SkillName.Musicianship].Base = val;
                   		m.Skills[SkillName.Peacemaking].Base = val;
                   		m.Skills[SkillName.Veterinary].Base = val;
                   		m.Skills[SkillName.Magery].Base = val;
                   		m.Skills[SkillName.MagicResist].Base = val;
                   		
                   		m_SkillBall.Delete();
                   		
                    }
                    break;
				}
                case 2: //Peacer-Med
                {
                	m.CloseGump(typeof(TamerPickGump));
                	if ( m_SkillBall.IsChildOf( m.Backpack ))
                    {
                        for (int i = 0; i < skills.Length; ++i)
                            skills[i].Base = 0;
                            
                    	m.Skills[SkillName.AnimalTaming].Base = val;
                   		m.Skills[SkillName.AnimalLore].Base = val;
                   		m.Skills[SkillName.Musicianship].Base = val;
                   		m.Skills[SkillName.Peacemaking].Base = val;
                   		m.Skills[SkillName.Meditation].Base = val;
                   		m.Skills[SkillName.Magery].Base = val;
                   		m.Skills[SkillName.MagicResist].Base = val;
                   		
                   		m_SkillBall.Delete();
                   		
                    }
                    break;
                }
                case 3: //PvP Tamer-Vet
                {
                	m.CloseGump(typeof(TamerPickGump));
                	if ( m_SkillBall.IsChildOf( m.Backpack ))
                    {
                        for (int i = 0; i < skills.Length; ++i)
                            skills[i].Base = 0;
                            
                    	m.Skills[SkillName.AnimalTaming].Base = val;
                   		m.Skills[SkillName.AnimalLore].Base = val;
                   		m.Skills[SkillName.Veterinary].Base = val;
                   		m.Skills[SkillName.Magery].Base = val;
                   		m.Skills[SkillName.MagicResist].Base = val;
                   		m.Skills[SkillName.Meditation].Base = val;
                   		m.Skills[SkillName.EvalInt].Base = val;
                   		
                   		m_SkillBall.Delete();
                   		
                    }
                    break;
                }
                case 4: //Bola Tamer
                {
                	m.CloseGump(typeof(TamerPickGump));
                	if ( m_SkillBall.IsChildOf( m.Backpack ))
                    {
                        for (int i = 0; i < skills.Length; ++i)
                            skills[i].Base = 0;
                            
                    	m.Skills[SkillName.AnimalTaming].Base = val;
                   		m.Skills[SkillName.AnimalLore].Base = val;
                   		m.Skills[SkillName.Tactics].Base = val;
                   		m.Skills[SkillName.Magery].Base = val;
                   		m.Skills[SkillName.MagicResist].Base = val;
                   		m.Skills[SkillName.Meditation].Base = val;
                   		m.Skills[SkillName.EvalInt].Base = val;
                   		
                   		m_SkillBall.Delete();
                   		
                    }
                    break;
                }
                case 5: //Scribe Tamer
                {
                	m.CloseGump(typeof(TamerPickGump));
                	if ( m_SkillBall.IsChildOf( m.Backpack ))
                    {
                        for (int i = 0; i < skills.Length; ++i)
                            skills[i].Base = 0;
                            
                    	m.Skills[SkillName.AnimalTaming].Base = val;
                   		m.Skills[SkillName.AnimalLore].Base = val;
                   		m.Skills[SkillName.Inscribe].Base = val;
                   		m.Skills[SkillName.Magery].Base = val;
                   		m.Skills[SkillName.MagicResist].Base = val;
                   		m.Skills[SkillName.Meditation].Base = val;
                   		m.Skills[SkillName.EvalInt].Base = val;
                   		
                   		m_SkillBall.Delete();
                   		
                    }
                    break;
                }
                case 6: //Alchy Tamer
               	{
                	m.CloseGump(typeof(TamerPickGump));
                	if ( m_SkillBall.IsChildOf( m.Backpack ))
                    {
                        for (int i = 0; i < skills.Length; ++i)
                            skills[i].Base = 0;
                            
                    	m.Skills[SkillName.AnimalTaming].Base = val;
                   		m.Skills[SkillName.AnimalLore].Base = val;
                   		m.Skills[SkillName.Alchemy].Base = val;
                   		m.Skills[SkillName.Magery].Base = val;
                   		m.Skills[SkillName.MagicResist].Base = val;
                   		m.Skills[SkillName.Meditation].Base = val;
                   		m.Skills[SkillName.EvalInt].Base = val;
                   		
                   		m_SkillBall.Delete();
                   		
                    }
                    break;
               	}
                case 7: //close
                {
                	m.CloseGump(typeof(TamerPickGump));
                	if ( m_SkillBall.IsChildOf( m.Backpack ))
                    {
                    	m.SendGump( new TemplatePickGump(m_SkillBall) );
                   		
                    }
                    break;
                }
			}  
		}
	}

	public class SkillPickGump : Gump
	{


		private int switches = 7;
        private SkillBall  m_SkillBall;
        private double val = 100;

		public SkillPickGump( SkillBall ball )
			: base( 0, 0 )
		{
			this.Closable=true;
			this.Disposable=true;
			this.Dragable=true;
			this.Resizable=false;
            m_SkillBall = ball;
           	this.AddPage(0);
			this.AddBackground(39, 33, 560, 500, 9200);
			this.AddLabel(67, 41, 1153, @"Please select your 7 skills");
			this.AddButton(80, 500, 2071, 2072, (int)Buttons.Close, GumpButtonType.Reply, 0);
			this.AddBackground(52, 60, 530, 430, 9350);
			this.AddPage(1);
            this.AddButton(500, 500, 2311, 2312, (int)Buttons.FinishButton, GumpButtonType.Reply, 0);
			this.AddCheck(55, 65, 210, 211, false, 40);
			this.AddCheck(55, 90, 210, 211, false, 2);
			this.AddCheck(55, 115, 210, 211, false, 39);
			this.AddCheck(55, 140, 210, 211, false, 36);
			this.AddCheck(55, 165, 210, 211, false, 5);
			this.AddCheck(55, 190, 210, 211, false, 37);
			this.AddCheck(55, 215, 210, 211, false, 38);
			this.AddCheck(55, 240, 210, 211, false, 6);
			this.AddCheck(55, 265, 210, 211, false, 41);
			this.AddCheck(55, 290, 210, 211, false, 9);
            this.AddCheck(55, 315, 210, 211, false, 13);
            this.AddCheck(55, 340, 210, 211, false, 34);
            this.AddCheck(55, 365, 210, 211, false, 33);
            this.AddCheck(55, 390, 210, 211, false, 15);
            this.AddCheck(55, 415, 210, 211, false, 14);
            this.AddCheck(55, 440, 210, 211, false, 56);
			this.AddLabel(80, 65, 0, @"Tactics");          //40
			this.AddLabel(80, 90, 0, @"Anatomy");          //2
			this.AddLabel(80, 115, 0, @"Swordsmanship");   //39
			this.AddLabel(80, 140, 0, @"Fencing");         //36
			this.AddLabel(80, 165, 0, @"Archery");         //5
			this.AddLabel(80, 190, 0, @"Macefighting");    //37
			this.AddLabel(80, 215, 0, @"Parry");           //38
			this.AddLabel(80, 240, 0, @"Arms Lore");       //6
			this.AddLabel(80, 265, 0, @"Wrestling");       //41
            this.AddLabel(80, 290, 0, @"Blacksmithing");    //9
            this.AddLabel(80, 315, 0, @"Carpentry");        //13
            this.AddLabel(80, 340, 0, @"Tinkering");       //34
            this.AddLabel(80, 365, 0, @"Tailoring");       //33
            this.AddLabel(80, 390, 0, @"Fishing");         //15
            this.AddLabel(80, 415, 0, @"Cooking");         //14
            this.AddLabel(80, 440, 0, @"Fletching");       //56
            // ********************************************************
            this.AddCheck(240, 65, 210, 211, false, 23);
            this.AddCheck(240, 90, 210, 211, false, 20);
            this.AddCheck(240, 115, 210, 211, false, 1);
            this.AddCheck(240, 140, 210, 211, false, 44);
            this.AddCheck(240, 165, 210, 211, false, 21);
            this.AddCheck(240, 190, 210, 211, false, 48);
            this.AddCheck(240, 215, 210, 211, false, 50);
            this.AddCheck(240, 240, 210, 211, false, 22);
            this.AddCheck(240, 265, 210, 211, false, 55);
            this.AddCheck(240, 290, 210, 211, false, 32);
            this.AddCheck(240, 315, 210, 211, false, 29);
            this.AddCheck(240, 340, 210, 211, false, 31);
            this.AddCheck(240, 365, 210, 211, false, 19);
            this.AddCheck(240, 390, 210, 211, false, 43);
            this.AddCheck(240, 415, 210, 211, false, 27);
            this.AddCheck(240, 440, 210, 211, false, 45);
            this.AddLabel(265, 65, 0, @"Mining");         //23
            this.AddLabel(265, 90, 0, @"Lumberjacking");  //20
            this.AddLabel(265, 115, 0, @"Alchemy");         //1
            this.AddLabel(265, 140, 0, @"Inscription");     //44
            this.AddLabel(265, 165, 0, @"Magery");         //21
            this.AddLabel(265, 190, 0, @"Spirit Speak");   //48
            this.AddLabel(265, 215, 0, @"Evaluating Intelligence"); //50
            this.AddLabel(265, 240, 0, @"Meditation");     //22
            this.AddLabel(265, 265, 0, @"Hiding");          //55
            this.AddLabel(265, 290, 0, @"Stealth");         //32
            this.AddLabel(265, 315, 0, @"Snooping");       //29
            this.AddLabel(265, 340, 0, @"Stealing");       //31
            this.AddLabel(265, 365, 0, @"Lockpicking");    //19
            this.AddLabel(265, 390, 0, @"Detecting Hidden"); //43
            this.AddLabel(265, 415, 0, @"Remove Trap");     //27
            this.AddLabel(265, 440, 0, @"Tracking");        //45
            //**********************************************************
            this.AddCheck(425, 65, 210, 211, false, 46);
            this.AddCheck(425, 90, 210, 211, false, 4);
            this.AddCheck(425, 115, 210, 211, false, 3);
            this.AddCheck(425, 140, 210, 211, false, 11);
            this.AddCheck(425, 165, 210, 211, false, 24);
            this.AddCheck(425, 190, 210, 211, false, 47);
            this.AddCheck(425, 215, 210, 211, false, 45);
            this.AddCheck(425, 240, 210, 211, false, 52);
            this.AddCheck(425, 265, 210, 211, false, 53);
            this.AddCheck(425, 290, 210, 211, false, 51);
            this.AddCheck(425, 315, 210, 211, false, 7);
            this.AddCheck(425, 340, 210, 211, false, 17);
            this.AddCheck(425, 365, 210, 211, false, 18);
            this.AddCheck(425, 390, 210, 211, false, 28);
            this.AddCheck(425, 415, 210, 211, false, 35);
            this.AddCheck(425, 440, 210, 211, false, 42);
            this.AddLabel(450, 65, 0, @"Poisoning");      //46
            this.AddLabel(450, 90, 0, @"Animal Taming");   //4
            this.AddLabel(450, 115, 0, @"Animal Lore");     //3
            this.AddLabel(450, 140, 0, @"Camping");        //11
            this.AddLabel(450, 165, 0, @"Musicianship");   //24
            this.AddLabel(450, 190, 0, @"Provocation");    //47
            this.AddLabel(450, 215, 0, @"Peacemaking");    //45
            this.AddLabel(450, 240, 0, @"Item Identification"); //52
            this.AddLabel(450, 265, 0, @"Taste Identification"); //53
            this.AddLabel(450, 290, 0, @"Foresic Evaluation");  //51
            this.AddLabel(450, 315, 0, @"Begging"); //7
            this.AddLabel(450, 340, 0, @"Healing"); //17
            this.AddLabel(450, 365, 0, @"Herding"); //18
            this.AddLabel(450, 390, 0, @"Resisting Spells"); //28
            this.AddLabel(450, 415, 0, @"Veterinary"); //35
            this.AddLabel(450, 440, 0, @"Cartography");    //42
            //**********************************************************
			
			this.AddButton(244, 498, 4008, 4009, 2, GumpButtonType.Reply, 0);
			this.AddLabel(281, 499, 0, @"Pre-Made Templates");	
			
		}
		
		public enum Buttons
		{
			Close,
			FinishButton,
			
		}
		public override void OnResponse( NetState state, RelayInfo info )
		{
			Mobile m = state.Mobile;
			
			switch( info.ButtonID )
			{
				case 0: {break;}
				case 1:
				{
					if ( info.Switches.Length < switches )
					{
						m.SendGump( new SkillPickGump(m_SkillBall) );
						m.SendMessage( 0, "You must pick {0} more skills.", switches - info.Switches.Length );
						break;
					}
					else if ( info.Switches.Length > switches )
					{
						m.SendGump( new SkillPickGump(m_SkillBall) );
						m.SendMessage( 0, "Please get rid of {0} skills, you have exceeded the 7 skills that are allowed.", info.Switches.Length - switches);
						break;
                     
                    }
                    else if ( !m_SkillBall.IsChildOf( m.Backpack ))
                    {
                    	m.SendMessage( 0, "Please keep that skill ball in your backpack {0}.", m.Name );
                   		break;
                    }
                    else
                    {
                        Server.Skills skills = m.Skills;

                        for (int i = 0; i < skills.Length; ++i)
                            skills[i].Base = 0;
                        if (info.IsSwitched(1))
                            m.Skills[SkillName.Alchemy].Base = val;
                        if (info.IsSwitched(2))
                            m.Skills[SkillName.Anatomy].Base = val;
                        if (info.IsSwitched(3))
                            m.Skills[SkillName.AnimalLore].Base = val;
                        if (info.IsSwitched(4))
                            m.Skills[SkillName.AnimalTaming].Base = val;
                        if (info.IsSwitched(5))
                            m.Skills[SkillName.Archery].Base = val;
                        if (info.IsSwitched(6))
                            m.Skills[SkillName.ArmsLore].Base = val;
                        if (info.IsSwitched(7))
                            m.Skills[SkillName.Begging].Base = val;
                        if (info.IsSwitched(9))
                            m.Skills[SkillName.Blacksmith].Base = val;
                        if (info.IsSwitched(11))
                            m.Skills[SkillName.Camping].Base = val;
                        if (info.IsSwitched(13))
                            m.Skills[SkillName.Carpentry].Base = val;
                        if (info.IsSwitched(14))
                            m.Skills[SkillName.Cooking].Base = val;
                        if (info.IsSwitched(15))
                            m.Skills[SkillName.Fishing].Base = val;
                        if (info.IsSwitched(17))
                            m.Skills[SkillName.Healing].Base = val;
                        if (info.IsSwitched(18))
                            m.Skills[SkillName.Herding].Base = val;
                        if (info.IsSwitched(19))
                            m.Skills[SkillName.Lockpicking].Base = val;
                        if (info.IsSwitched(20))
                            m.Skills[SkillName.Lumberjacking].Base = val;
                        if (info.IsSwitched(21))
                            m.Skills[SkillName.Magery].Base = val;
                        if (info.IsSwitched(22))
                            m.Skills[SkillName.Meditation].Base = val;
                        if (info.IsSwitched(23))
                            m.Skills[SkillName.Mining].Base = val;
                        if (info.IsSwitched(24))
                            m.Skills[SkillName.Musicianship].Base = val;
                        if (info.IsSwitched(27))
                            m.Skills[SkillName.RemoveTrap].Base = val;
                        if (info.IsSwitched(28))
                            m.Skills[SkillName.MagicResist].Base = val;
                        if (info.IsSwitched(29))
                            m.Skills[SkillName.Snooping].Base = val;
                        if (info.IsSwitched(31))
                            m.Skills[SkillName.Stealing].Base = val;
                        if (info.IsSwitched(32))
                            m.Skills[SkillName.Stealth].Base = val;
                        if (info.IsSwitched(33))
                            m.Skills[SkillName.Tailoring].Base = val;
                        if (info.IsSwitched(34))
                            m.Skills[SkillName.Tinkering].Base = val;
                        if (info.IsSwitched(35))
                            m.Skills[SkillName.Veterinary].Base = val;
                        if (info.IsSwitched(36))
                            m.Skills[SkillName.Fencing].Base = val;
                        if (info.IsSwitched(37))
                            m.Skills[SkillName.Macing].Base = val;
                        if (info.IsSwitched(38))
                            m.Skills[SkillName.Parry].Base = val;
                        if (info.IsSwitched(39))
                            m.Skills[SkillName.Swords].Base = val;
                        if (info.IsSwitched(40))
                            m.Skills[SkillName.Tactics].Base = val;
                        if (info.IsSwitched(41))
                            m.Skills[SkillName.Wrestling].Base = val;
                        if (info.IsSwitched(42))
                            m.Skills[SkillName.Cartography].Base = val;
                        if (info.IsSwitched(43))
                            m.Skills[SkillName.DetectHidden].Base = val;
                        if (info.IsSwitched(44))
                            m.Skills[SkillName.Inscribe].Base = val;
                        if (info.IsSwitched(45))
                            m.Skills[SkillName.Peacemaking].Base = val;
                        if (info.IsSwitched(46))
                            m.Skills[SkillName.Poisoning].Base = val;
                        if (info.IsSwitched(47))
                            m.Skills[SkillName.Provocation].Base = val;
                        if (info.IsSwitched(48))
                            m.Skills[SkillName.SpiritSpeak].Base = val;
                        if (info.IsSwitched(49))
                            m.Skills[SkillName.Tracking].Base = val;
                        if (info.IsSwitched(50))
                            m.Skills[SkillName.EvalInt].Base = val;
                        if (info.IsSwitched(51))
                            m.Skills[SkillName.Forensics].Base = val;
                        if (info.IsSwitched(52))
                            m.Skills[SkillName.ItemID].Base = val;
                        if (info.IsSwitched(53))
                            m.Skills[SkillName.TasteID].Base = val;
                        if (info.IsSwitched(55))
                            m.Skills[SkillName.Hiding].Base = val;
                        if (info.IsSwitched(56))
                            m.Skills[SkillName.Fletching].Base = val;


                        m_SkillBall.Delete();

                    }
                    
					break;
				}
                case 2:  
                {
                	m.CloseGump(typeof(SkillPickGump));
                	if ( m_SkillBall.IsChildOf( m.Backpack ))
                    {
                    	m.SendGump( new TemplatePickGump(m_SkillBall) );
                    }
                    break;
                }
			}  
		}
        
	}

	public class SkillBall : Item
	{ 
		[Constructable] 
		public SkillBall() :  base( 0x1870 ) 
		{ 
			Weight = 1.0; 
			Hue = 1194; 
			Name = "A Skill Ball"; 
			Movable =  true;
		}

		public override void OnDoubleClick( Mobile m ) 
		{

            if (m.Backpack != null && m.Backpack.GetAmount(typeof(SkillBall)) > 0)
            {
                m.SendMessage("Please choose  your 7 skills to set to 7x GM.");
                m.CloseGump(typeof(SkillPickGump));
                m.SendGump(new SkillPickGump(this));
            }
            else
                m.SendMessage(" You need to have the skill ball in your backpack.");
			
		} 

        public SkillBall( Serial serial ) : base( serial ) 
		{ 
		} 
       
		public override void Serialize( GenericWriter writer ) 
		{ 
			base.Serialize( writer ); 
			writer.Write( (int) 1 ); // version 
		}

		public override void Deserialize( GenericReader reader ) 
		{ 
			base.Deserialize( reader ); 
			int version = reader.ReadInt();
            
        
        } 
       
	}
   
               
}