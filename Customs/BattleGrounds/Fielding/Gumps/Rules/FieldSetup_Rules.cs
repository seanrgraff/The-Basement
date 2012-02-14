using System;
using Server;
using Server.Gumps;
using Server.Items;
using Server.Network;
using Server.Commands;
using Server.Mobiles;
using System.Collections;
using System.Collections.Generic;

namespace Server.Fielding
{
	public class FieldSetup_Rules : Gump
	{
        public Field Handeling;

		public FieldSetup_Rules(Field d) : base(0, 0)
		{
            Handeling = d;

			Closable = true;
			Dragable = true;
			Resizable = false;

            AddBackground(219, 238, 172, 229, 9300);
            AddLabel(283, 244, 70, @"Rules");

            AddButton(225, 265, 1209, 1210, 1, GumpButtonType.Reply, 0);
            AddLabel(245, 262, 54, @"Spells");
            AddButton(225, 285, 1209, 1210, 2, GumpButtonType.Reply, 0);
            AddLabel(245, 282, 54, @"Combat Abilities");
            AddButton(225, 305, 1209, 1210, 3, GumpButtonType.Reply, 0);
            AddLabel(245, 302, 54, @"Skills");
            AddButton(225, 325, 1209, 1210, 4, GumpButtonType.Reply, 0);
            AddLabel(245, 322, 54, @"Weapons");
            AddButton(225, 345, 1209, 1210, 5, GumpButtonType.Reply, 0);
            AddLabel(245, 342, 54, @"Armor");
            AddButton(225, 365, 1209, 1210, 6, GumpButtonType.Reply, 0);
            AddLabel(245, 362, 54, @"Items");
            
            AddCheck(225, 385, 210, 211, Handeling.AllowTrees, 7);
            AddLabel(245, 382, 54, @"Allow Trees");
            AddTextEntry(245, 402, 150, 20, 1359, 8, Handeling.NumberOfTrees.ToString() );
            AddButton(285, 402, 238, 240, 9, GumpButtonType.Reply, 0);
            
            AddCheck(225, 422, 210, 211, Handeling.AllowTraps, 10);
            AddLabel(245, 419, 54, @"Allow Traps");
            AddTextEntry(245, 439, 150, 20, 1359, 11, Handeling.NumberOfTraps.ToString() );
            AddButton(285, 439, 238, 240, 12, GumpButtonType.Reply, 0);
            //AddButton(225, 385, 1209, 1210, 7, GumpButtonType.Reply, 0);
            //AddLabel(245, 382, 54, @"Samurai Spells");
            //AddButton(225, 405, 1209, 1210, 8, GumpButtonType.Reply, 0);
            //AddLabel(245, 402, 54, @"Chivalry");
            //AddButton(225, 425, 1209, 1210, 9, GumpButtonType.Reply, 0);
            //AddLabel(245, 422, 54, @"Necromancy");
            //AddButton(225, 445, 1209, 1210, 10, GumpButtonType.Reply, 0);
            //AddLabel(245, 442, 54, @"Ninja Spells");
		}

		public override void OnResponse( NetState sender, RelayInfo info )
		{
			Mobile from = sender.Mobile;
			
			Handeling.AllowTrees = info.IsSwitched(7);
			Handeling.AllowTraps = info.IsSwitched(10);

            switch (info.ButtonID)
            {
                case 0: // Close
                    {
                        from.SendGump(new FieldSetup_Main(Handeling));
                        break;
                    }
                case 1: // Spells
                    {
                        from.SendGump(new FieldSetup_Rules_Spells(Handeling));
                        break;
                    }
                case 2: // Combat Abilities
                    {
                        from.SendGump(new FieldSetup_Rules_Combat(Handeling));
                        break;
                    }
                case 3: // Skills
                    {
                        from.SendGump(new FieldSetup_Rules_Skills(Handeling));
                        break;
                    }
                case 4:  // Weapons
                    {
                        from.SendGump(new FieldSetup_Rules_Weapons(Handeling));
                        break;
                    }
                case 5: // Armor
                    {
                        from.SendGump(new FieldSetup_Rules_Armor(Handeling));
                        break;
                    }
                case 6: // Items
                    {
                        from.SendGump(new FieldSetup_Rules_Items(Handeling));
                        break;
                    }
                //case 7: // Samurai
                //    {
                //        from.SendGump(new FieldSetup_Rules_Samurai(Handeling));
                //        break;
                //    }
                //case 8: // Chivalry
                //    {
                //        from.SendGump(new FieldSetup_Rules_Chivalry(Handeling));
                //        break;
                //    }
                case 9: // Tree adding
                    {
                    	int number = 0;
                    	TextRelay h = info.GetTextEntry( 8 );
            			try
            			{
           		 			number = Convert.ToInt32(h.Text);
            			}
            			catch
            			{
                 			from.SendMessage("Bad entry. A number was expected.");
            			}
			
						if ( number > -1 )
						{
							if ( number > 200 ) 
							{
								from.SendMessage( "Your choice must be between 0 and 200. Please try again!" );
							}	
							else
							{
								Handeling.NumberOfTrees = number;
							}
						}
						else
						{
							from.SendMessage( "Your choice must not be negative. Please try again!" );
						}
						from.SendGump(new FieldSetup_Rules(Handeling));
						break;
					}
				case 12: // Tree adding
                    {
                    	int number = 0;
                    	TextRelay h = info.GetTextEntry( 11 );
            			try
            			{
           		 			number = Convert.ToInt32(h.Text);
            			}
            			catch
            			{
                 			from.SendMessage("Bad entry. A number was expected.");
            			}
			
						if ( number > -1 )
						{
							if ( number > 200 ) 
							{
								from.SendMessage( "Your choice must be between 0 and 200. Please try again!" );
							}	
							else
							{
								Handeling.NumberOfTraps = number;
							}
						}
						else
						{
							from.SendMessage( "Your choice must not be negative. Please try again!" );
						}
						from.SendGump(new FieldSetup_Rules(Handeling));
						break;
					}
                //case 9: // Necromancy
                //    {
                //        from.SendGump(new FieldSetup_Rules_Necromancy(Handeling));
                //        break;
                //    }
                //case 10: // Ninjitsu
                //    {
                //        from.SendGump(new FieldSetup_Rules_Ninjitsu(Handeling));
                //        break;
                //    }
            }
		}
	}
}
