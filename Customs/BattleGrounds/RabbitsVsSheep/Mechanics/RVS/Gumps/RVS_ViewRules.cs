using System;
using Server;
using Server.Gumps;
using Server.Items;
using Server.Network;
using Server.Commands;
using Server.Mobiles;
using System.Collections;
using System.Collections.Generic;

namespace Server.RabbitsVsSheep
{
    public class RVSSetup_Rules_View : Gump
    {
        public RVS Handeling;
        public int TeamID;
        public int Index;

        public RVSSetup_Rules_View(RVS d, int team, int id)
            : base(0, 0)
        {
            Handeling = d;
            TeamID = team;
            Index = id;

            Closable = false;
            Dragable = true;
            Resizable = false;

            int bh = 309 + (20 * Handeling.Teams.Count);
            AddBackground(221, 179, 194, bh, 9300);
            AddLabel(266, 186, 70, @"View RVS Rules");

            AddButton(225, 209, 1209, 1210, 11, GumpButtonType.Reply, 0);
            AddLabel(245, 206, 57, @"I accept these rules");
            AddButton(225, 230, 1209, 1210, 0, GumpButtonType.Reply, 0);
            AddLabel(245, 227, 38, @"I do not accept these rules");

			AddButton(225, 256, 1209, 1210, 1, GumpButtonType.Reply, 0);
      		AddLabel(245, 253, 54, @"Monster Types");
            //AddButton(225, 256, 1209, 1210, 1, GumpButtonType.Reply, 0);
            //AddLabel(245, 253, 54, @"Spells");
       //AddCheck(225, 256, 1209, 1210, Handeling.Weapons, 1);
       //AddLabel(245, 253, 54, @"Rabbits Vs Sheep");
            //AddButton(225, 276, 1209, 1210, 2, GumpButtonType.Reply, 0);
            //AddLabel(245, 273, 54, @"Combat Abilities");
       //AddCheck(225, 276, 1209, 1210, Handeling.Weapons, 1);
       //AddLabel(245, 273, 54, @"Orc Massacre");
            //AddButton(225, 296, 1209, 1210, 3, GumpButtonType.Reply, 0);
            //AddLabel(245, 293, 54, @"Skills");
       //AddCheck(225, 296, 1209, 1210, Handeling.Weapons, 1);
       //AddLabel(245, 293, 54, @"Lizardmen Lair");
            //AddButton(225, 316, 1209, 1210, 4, GumpButtonType.Reply, 0);
            //AddLabel(245, 313, 54, @"Weapons");
       //AddCheck(225, 316, 1209, 1210, Handeling.Weapons, 1);
       //AddLabel(245, 313, 54, @"Ratmen Rampage");
            //AddButton(225, 336, 1209, 1210, 5, GumpButtonType.Reply, 0);
            //AddLabel(245, 333, 54, @"Armor");
       //AddCheck(225, 336, 1209, 1210, Handeling.Weapons, 1);
       //AddLabel(245, 333, 54, @"Dark Ritual");
            //AddButton(225, 356, 1209, 1210, 6, GumpButtonType.Reply, 0);
            //AddLabel(245, 353, 54, @"Items");
            //AddLabel(325, 376, 1263, Handeling.AllowTrees.ToString() );
            //AddLabel(245, 373, 54, @"Allow Trees=");
            //AddLabel(245, 393, 54, Handeling.NumberOfTrees.ToString() );
            //AddLabel(325, 410, 1263, Handeling.AllowTraps.ToString() );
            //AddLabel(245, 407, 54, @"Allow Traps=");
            //AddLabel(245, 427, 54, Handeling.NumberOfTraps.ToString() );
            //AddButton(225, 376, 1209, 1210, 7, GumpButtonType.Reply, 0);
            //AddLabel(245, 373, 54, @"Samurai Spells");
            //AddButton(225, 396, 1209, 1210, 8, GumpButtonType.Reply, 0);
            //AddLabel(245, 393, 54, @"Chivalry");
            //AddButton(225, 416, 1209, 1210, 9, GumpButtonType.Reply, 0);
            //AddLabel(245, 413, 54, @"Necromancy");
            //AddButton(225, 436, 1209, 1210, 10, GumpButtonType.Reply, 0);
            //AddLabel(245, 433, 54, @"Ninja Spells");

            AddLabel(256, 463, 70, @"View Participants");
            int y = 486, bid = 12;
            IEnumerator key = Handeling.Teams.Keys.GetEnumerator();
            for (int i = 0; i < Handeling.Teams.Count; ++i)
            {
                key.MoveNext();
                RVS_Team d_team = (RVS_Team)Handeling.Teams[(int)key.Current];
                AddButton(225, y, 1209, 1210, bid, GumpButtonType.Reply, 0);
                AddLabel(245, (y - 3), 12, String.Format(@"Team {0}: {1} out of {2} players", d_team.TeamID.ToString(), TeamPlayers(d_team).ToString(), d_team.Size.ToString()));

                y += 20;
                bid += 1;
            }
        }

        public int TeamPlayers(RVS_Team team)
        {
            int toreturn = 0;

            for (int i = 0; i < team.Players.Count; ++i)
            {
                if (team.Players[i] != "@null")
                    toreturn += 1;
            }

            return toreturn;
        }

        public override void OnResponse(NetState sender, RelayInfo info)
        {
            Mobile from = sender.Mobile;

            switch (info.ButtonID)
            {
                case 0: // Close
                    {
                        PlayerMobile pm = (PlayerMobile)Handeling.Teams[TeamID].Players[Index];
                        Handeling.Teams[TeamID].Players[Index] = "@null";
                        Handeling.Teams[TeamID].Accepted.Remove(pm);
                        Handeling.EchoMessage(String.Format("{0} has declined the duel, and has been removed.", pm.Name));
                        Handeling.RefundBuyIn(pm);

                        Handeling.SendControllerSetup();
                        break;
                    }
                case 1: //Monsters, Used to be Spells
                    {
                    	from.SendGump(new RVSSetup_Rules_Monsters_View(Handeling, TeamID, Index));
                        //from.SendGump(new RVSSetup_Rules_Spells_View(Handeling, TeamID, Index));
                        break;
                    }
                case 2: // Combat Abilities
                    {
                        from.SendGump(new RVSSetup_Rules_Combat_View(Handeling, TeamID, Index));
                        break;
                    }
                case 3: // Skills
                    {
                        from.SendGump(new RVSSetup_Rules_Skills_View(Handeling, TeamID, Index));
                        break;
                    }
                case 4:  // Weapons
                    {
                        from.SendGump(new RVSSetup_Rules_Weapons_View(Handeling, TeamID, Index));
                        break;
                    }
                case 5: // Armor
                    {
                        from.SendGump(new RVSSetup_Rules_Armor_View(Handeling, TeamID, Index));
                        break;
                    }
                case 6: // Items
                    {
                        from.SendGump(new RVSSetup_Rules_Items_View(Handeling, TeamID, Index));
                        break;
                    }
                case 7: // Samurai
                    {
                        from.SendGump(new RVSSetup_Rules_Samurai_View(Handeling, TeamID, Index));
                        break;
                    }
                case 8: // Chivalry
                    {
                        from.SendGump(new RVSSetup_Rules_Chivalry_View(Handeling, TeamID, Index));
                        break;
                    }
                case 9: // Necromancy
                    {
                        from.SendGump(new RVSSetup_Rules_Necromancy_View(Handeling, TeamID, Index));
                        break;
                    }
                case 10: // Ninjitsu
                    {
                        from.SendGump(new RVSSetup_Rules_Ninjitsu_View(Handeling, TeamID, Index));
                        break;
                    }
                case 11: //Accepted
                    {
                        PlayerMobile pm = (PlayerMobile)Handeling.Teams[TeamID].Players[Index];
                        Handeling.Teams[TeamID].Accepted[pm] = true;
                        Handeling.UpdateAllPending();
                        Handeling.EchoMessage(String.Format("{0} has accepted the duel.", pm.Name));
                        break;
                    }
            }

            if (info.ButtonID > 11)
            {
                int teamid2 = (info.ButtonID - 11);
                from.SendGump(new RVSSetup_ViewParticipants(Handeling, TeamID, Index, teamid2));
            }
        }
    }

    public class RVSSetup_ViewParticipants : Gump
    {
        public RVS Handeling;
        public int TeamID;
        public int Index;

        public RVSSetup_ViewParticipants(RVS d, int teamid, int id, int teamid2)
            : base(0, 0)
        {
            Handeling = d;
            TeamID = teamid;
            Index = id;

            Closable = true;
            Dragable = true;
            Resizable = false;

            int bh = 34 + (Handeling.Teams[teamid2].Players.Count * 20);
            AddBackground(182, 269, 205, bh, 9300);
            AddLabel(204, 276, 70, String.Format(@"View Team {0} Participants", teamid2.ToString()));

            int y = 47;
            RVS_Team d_team = (RVS_Team)Handeling.Teams[teamid2];
            for (int i = 0; i < d_team.Players.Count; ++i)
            {
                object o = (object)d_team.Players[i];
                if (o is PlayerMobile)
                {
                    PlayerMobile pm = (PlayerMobile)o;
                    AddLabel(186, 297, y, String.Format(@"{0}: {1}", (i + 1).ToString(), pm.Name));
                }
                else
                    AddLabel(186, 297, y, String.Format(@"{0}: Empty Slot", (i + 1).ToString()));

                y += 20;
            }
        }

        public int PlayerCount(RVS_Team d_team)
        {
            int count = 0;
            for (int i = 0; i < d_team.Players.Count; ++i)
            {
                if (d_team.Players[i] != "@null")
                    count += 1;
            }

            return count;
        }

        public override void OnResponse(NetState sender, RelayInfo info)
        {
            Mobile from = sender.Mobile;
            from.SendGump(new RVSSetup_Rules_View(Handeling, TeamID, Index));
        }
    }

    public class RVSSetup_Rules_Armor_View : Gump
    {
        public RVS Handeling;
        public int TeamID;
        public int Index;

        public RVSSetup_Rules_Armor_View(RVS d, int team, int id)
            : base(0, 0)
        {
            Handeling = d;
            TeamID = team;
            Index = id;

            Closable = true;
            Dragable = true;
            Resizable = false;

            AddBackground(219, 238, 172, 129, 9300);
            AddLabel(263, 244, 70, @"Armor Rules");

            AddImage(225, 265, ImageID(Handeling.Armor));
            AddLabel(255, 265, 54, @"Armor Allowed");
            AddImage(225, 290, ImageID(Handeling.MagicalArmor));
            AddLabel(255, 290, 54, @"Magic Armor");
            AddImage(225, 315, ImageID(Handeling.Shields));
            AddLabel(255, 315, 54, @"Shields");
            AddImage(225, 340, ImageID(Handeling.Colored));
            AddLabel(255, 340, 54, @"Colored Armor");
        }

        public int ImageID(bool b)
        {
            if (b)
                return 211;
            else
                return 210;
        }

        public override void OnResponse(NetState sender, RelayInfo info)
        {
            Mobile from = sender.Mobile;
            from.SendGump(new RVSSetup_Rules_View(Handeling, TeamID, Index));
        }
    }

    public class RVSSetup_Rules_Combat_View : Gump
    {
        public RVS Handeling;
        public int TeamID;
        public int Index;

        public RVSSetup_Rules_Combat_View(RVS d, int team, int id)
            : base(0, 0)
        {
            Handeling = d;
            TeamID = team;
            Index = id;

            Closable = true;
            Dragable = true;
            Resizable = false;

            AddBackground(219, 238, 172, 179, 9300);
            AddLabel(245, 244, 70, @"Combat Ability Rules");

            AddImage(225, 265, ImageID(Handeling.CombatAbilities));
            AddLabel(255, 265, 54, @"Combat Abilities");
            AddImage(225, 290, ImageID(Handeling.Stun));
            AddLabel(255, 290, 54, @"Stun");
            AddImage(225, 315, ImageID(Handeling.Disarm));
            AddLabel(255, 315, 54, @"Disarm");
            AddImage(225, 340, ImageID(Handeling.ConcussionBlow));
            AddLabel(255, 340, 54, @"Concussion Blow");
            AddImage(225, 365, ImageID(Handeling.CrushingBlow));
            AddLabel(255, 365, 54, @"Crushing Blow");
            AddImage(225, 390, ImageID(Handeling.ParalyzingBlow));
            AddLabel(255, 390, 54, @"Paralyzing Blow");
        }

        public int ImageID(bool b)
        {
            if (b)
                return 211;
            else
                return 210;
        }

        public override void OnResponse(NetState sender, RelayInfo info)
        {
            Mobile from = sender.Mobile;
            from.SendGump(new RVSSetup_Rules_View(Handeling, TeamID, Index));
        }
    }

    public class RVSSetup_Rules_Items_View : Gump
    {
        public RVS Handeling;
        public int TeamID;
        public int Index;

        public RVSSetup_Rules_Items_View(RVS d, int team, int id)
            : base(0, 0)
        {
            Handeling = d;
            TeamID = team;
            Index = id;

            Closable = true;
            Dragable = true;
            Resizable = false;

            AddBackground(219, 238, 172, 154, 9300);
            AddLabel(255, 244, 70, @"Item Use Rules");

            AddImage(225, 265, ImageID(Handeling.Potions));
            AddLabel(255, 265, 54, @"Potions");
            AddImage(225, 290, ImageID(Handeling.Bandages));
            AddLabel(255, 290, 54, @"Bandages");
            AddImage(225, 315, ImageID(Handeling.TrappedContainers));
            AddLabel(255, 315, 54, @"Trapped Containers");
            AddImage(225, 340, ImageID(Handeling.Bolas));
            AddLabel(255, 340, 54, @"Bolas");
            AddImage(225, 365, ImageID(Handeling.Mounts));
            AddLabel(255, 365, 54, @"Mounts");
        }

        public int ImageID(bool b)
        {
            if (b)
                return 211;
            else
                return 210;
        }

        public override void OnResponse(NetState sender, RelayInfo info)
        {
            Mobile from = sender.Mobile;
            from.SendGump(new RVSSetup_Rules_View(Handeling, TeamID, Index));
        }
    }

    public class RVSSetup_Rules_Skills_View : Gump
    {
        public RVS Handeling;
        public int TeamID;
        public int Index;

        public RVSSetup_Rules_Skills_View(RVS d, int team, int id)
            : base(0, 0)
        {
            Handeling = d;
            TeamID = team;
            Index = id;

            Closable = true;
            Dragable = true;
            Resizable = false;

            AddBackground(219, 238, 172, 254, 9300);
            AddLabel(255, 244, 70, @"Skill Use Rules");

            AddImage(225, 265, ImageID(Handeling.Anatomy));
            AddLabel(255, 265, 54, @"Anatomy");
            AddImage(225, 290, ImageID(Handeling.DetectHidden));
            AddLabel(255, 290, 54, @"Detect Hidden");
            AddImage(225, 315, ImageID(Handeling.EvaluatingIntelligence));
            AddLabel(255, 315, 54, @"Evaluating Intelligence");
            AddImage(225, 340, ImageID(Handeling.Hiding));
            AddLabel(255, 340, 54, @"Hiding");
            AddImage(225, 365, ImageID(Handeling.Poisoning));
            AddLabel(255, 365, 54, @"Poisoning");
            AddImage(225, 390, ImageID(Handeling.Snooping));
            AddLabel(255, 390, 54, @"Snooping");
            AddImage(225, 415, ImageID(Handeling.Stealing));
            AddLabel(255, 415, 54, @"Stealing");
            AddImage(225, 440, ImageID(Handeling.SpiritSpeak));
            AddLabel(255, 440, 54, @"Spirit Speak");
            AddImage(225, 465, ImageID(Handeling.Stealth));
            AddLabel(255, 465, 54, @"Stealth");
        }

        public int ImageID(bool b)
        {
            if (b)
                return 211;
            else
                return 210;
        }

        public override void OnResponse(NetState sender, RelayInfo info)
        {
            Mobile from = sender.Mobile;
            from.SendGump(new RVSSetup_Rules_View(Handeling, TeamID, Index));
        }
    }

    public class RVSSetup_Rules_Weapons_View : Gump
    {
        public RVS Handeling;
        public int TeamID;
        public int Index;

        public RVSSetup_Rules_Weapons_View(RVS d, int team, int id)
            : base(0, 0)
        {
            Handeling = d;
            TeamID = team;
            Index = id;

            Closable = true;
            Dragable = true;
            Resizable = false;

            AddBackground(219, 238, 172, 129, 9300);
            AddLabel(263, 244, 70, @"Weapon Rules");

            AddImage(225, 265, ImageID(Handeling.Weapons));
            AddLabel(255, 265, 54, @"Weapons Allowed");
            AddImage(225, 290, ImageID(Handeling.Magical));
            AddLabel(255, 290, 54, @"Magic Weapons");
            AddImage(225, 315, ImageID(Handeling.Poisoned));
            AddLabel(255, 315, 54, @"Poisoned Weapons");
            AddImage(225, 340, ImageID(Handeling.RunicWeapons));
            AddLabel(255, 340, 54, @"Runic Weapons");
        }

        public int ImageID(bool b)
        {
            if (b)
                return 211;
            else
                return 210;
        }

        public override void OnResponse(NetState sender, RelayInfo info)
        {
            Mobile from = sender.Mobile;
            from.SendGump(new RVSSetup_Rules_View(Handeling, TeamID, Index));
        }
    }

    public class RVSSetup_Rules_Monsters_View : Gump
    {
    	public RVS Handeling;
        public int TeamID;
        public int Index;

        public RVSSetup_Rules_Monsters_View(RVS d, int team, int id)
            : base(0, 0)
        {
            Handeling = d;
            TeamID = team;
            Index = id;

            Closable = true;
            Dragable = true;
            Resizable = false;

            AddBackground(219, 238, 172, 164, 9300);
            AddLabel(263, 244, 70, @"Monster Type Rules");

            AddImage(225, 265, ImageID(Handeling.RvS));
            AddLabel(255, 265, 54, @"Rabbits Vs Sheep");
            AddImage(225, 290, ImageID(Handeling.Orcs));
            AddLabel(255, 290, 54, @"Orcs");
            AddImage(225, 315, ImageID(Handeling.Lizardmen));
            AddLabel(255, 315, 54, @"Lizardmen");
            AddImage(225, 340, ImageID(Handeling.Ratmen));
            AddLabel(255, 340, 54, @"Ratmen");
            AddImage(225, 365, ImageID(Handeling.Undead));
            AddLabel(255, 365, 54, @"Undead");
        }

        public int ImageID(bool b)
        {
            if (b)
                return 211;
            else
                return 210;
        }

        public override void OnResponse(NetState sender, RelayInfo info)
        {
            Mobile from = sender.Mobile;
            from.SendGump(new RVSSetup_Rules_View(Handeling, TeamID, Index));
        }
    }
    
    
    //Spells Start Here ///////////////////////////////////////////////////////////////////////////////
    public class RVSSetup_Rules_Spells_View : Gump
    {
        public RVS Handeling;
        public int TeamID;
        public int Index;

        public RVSSetup_Rules_Spells_View(RVS d, int team, int id)
            : base(0, 0)
        {
            Handeling = d;
            TeamID = team;
            Index = id;

            Closable = true;
            Dragable = true;
            Resizable = false;

            AddBackground(219, 238, 172, 254, 9300);
            AddLabel(255, 244, 70, @"Spell Use Rules");

            AddImage(225, 265, ImageID(Handeling.Spells));
            AddLabel(255, 265, 54, @"Allow Magic");
            AddButton(225, 290, 1209, 1210, 1, GumpButtonType.Reply, 0);
            AddLabel(255, 288, 54, @"1st Circle");
            AddButton(225, 315, 1209, 1210, 2, GumpButtonType.Reply, 0);
            AddLabel(255, 312, 54, @"2nd Circle");
            AddButton(225, 340, 1209, 1210, 3, GumpButtonType.Reply, 0);
            AddLabel(255, 338, 54, @"3rd Circle");
            AddButton(225, 365, 1209, 1210, 4, GumpButtonType.Reply, 0);
            AddLabel(255, 362, 54, @"4th Circle");
            AddButton(225, 390, 1209, 1210, 5, GumpButtonType.Reply, 0);
            AddLabel(255, 388, 54, @"5th Circle");
            AddButton(225, 415, 1209, 1210, 6, GumpButtonType.Reply, 0);
            AddLabel(255, 412, 54, @"6th Circle");
            AddButton(225, 440, 1209, 1210, 7, GumpButtonType.Reply, 0);
            AddLabel(255, 438, 54, @"7th Circle");
            AddButton(225, 465, 1209, 1210, 8, GumpButtonType.Reply, 0);
            AddLabel(255, 462, 54, @"8th Circle");
        }

        public int ImageID(bool b)
        {
            if (b)
                return 211;
            else
                return 210;
        }

        public override void OnResponse(NetState sender, RelayInfo info)
        {
            Mobile from = sender.Mobile;

            switch (info.ButtonID)
            {
                case 0: // Closeing
                    {
                        from.SendGump(new RVSSetup_Rules_View(Handeling, TeamID, Index));
                        break;
                    }
                case 1: // First Circle
                    {
                        from.SendGump(new RVSSetup_Rules_Spells_1st_View(Handeling, TeamID, Index));
                        break;
                    }
                case 2: // Second Circle
                    {
                        from.SendGump(new RVSSetup_Rules_Spells_2nd_View(Handeling, TeamID, Index));
                        break;
                    }
                case 3: // Third Circle
                    {
                        from.SendGump(new RVSSetup_Rules_Spells_3rd_View(Handeling, TeamID, Index));
                        break;
                    }
                case 4: // Fourth Circle
                    {
                        from.SendGump(new RVSSetup_Rules_Spells_4th_View(Handeling, TeamID, Index));
                        break;
                    }
                case 5: // Fifth Circle
                    {
                        from.SendGump(new RVSSetup_Rules_Spells_5th_View(Handeling, TeamID, Index));
                        break;
                    }
                case 6: // Sixth Circle
                    {
                        from.SendGump(new RVSSetup_Rules_Spells_6th_View(Handeling, TeamID, Index));
                        break;
                    }
                case 7: // Seventh Circle
                    {
                        from.SendGump(new RVSSetup_Rules_Spells_7th_View(Handeling, TeamID, Index));
                        break;
                    }
                case 8: // Eigth Circle
                    {
                        from.SendGump(new RVSSetup_Rules_Spells_8th_View(Handeling, TeamID, Index));
                        break;
                    }
            }
        }
    }

    public class RVSSetup_Rules_Spells_1st_View : Gump
    {
        public RVS Handeling;
        public int TeamID;
        public int Index;

        public RVSSetup_Rules_Spells_1st_View(RVS d, int team, int id)
            : base(0, 0)
        {
            Handeling = d;
            TeamID = team;
            Index = id;

            Closable = true;
            Dragable = true;
            Resizable = false;

            AddBackground(219, 238, 172, 229, 9300);
            AddLabel(255, 244, 70, @"1st Circle Rules");

            AddImage(225, 265, ImageID(Handeling.ReactiveArmor));
            AddLabel(255, 265, 54, @"Reactive Armor");
            AddImage(225, 290, ImageID(Handeling.Clumsy));
            AddLabel(255, 290, 54, @"Clumsy");
            AddImage(225, 315, ImageID(Handeling.CreateFood));
            AddLabel(255, 315, 54, @"Create Food");
            AddImage(225, 340, ImageID(Handeling.Feeblemind));
            AddLabel(255, 340, 54, @"Feeblemind");
            AddImage(225, 365, ImageID(Handeling.Heal));
            AddLabel(255, 365, 54, @"Heal");
            AddImage(225, 390, ImageID(Handeling.MagicArrow));
            AddLabel(255, 390, 54, @"Magic Arrow");
            AddImage(225, 415, ImageID(Handeling.NightSight));
            AddLabel(255, 415, 54, @"Night Sight");
            AddImage(225, 440, ImageID(Handeling.Weaken));
            AddLabel(255, 440, 54, @"Weaken");
        }

        public int ImageID(bool b)
        {
            if (b)
                return 211;
            else
                return 210;
        }

        public override void OnResponse(NetState sender, RelayInfo info)
        {
            Mobile from = sender.Mobile;
            from.SendGump(new RVSSetup_Rules_Spells_View(Handeling, TeamID, Index));
        }
    }

    public class RVSSetup_Rules_Spells_2nd_View : Gump
    {
        public RVS Handeling;
        public int TeamID;
        public int Index;

        public RVSSetup_Rules_Spells_2nd_View(RVS d, int team, int id)
            : base(0, 0)
        {
            Handeling = d;
            TeamID = team;
            Index = id;

            Closable = true;
            Dragable = true;
            Resizable = false;

            AddBackground(219, 238, 172, 229, 9300);
            AddLabel(255, 244, 70, @"2nd Circle Rules");

            AddImage(225, 265, ImageID(Handeling.Agility));
            AddLabel(255, 265, 54, @"Agility");
            AddImage(225, 290, ImageID(Handeling.Cunning));
            AddLabel(255, 290, 54, @"Cunning");
            AddImage(225, 315, ImageID(Handeling.Cure));
            AddLabel(255, 315, 54, @"Cure");
            AddImage(225, 340, ImageID(Handeling.Harm));
            AddLabel(255, 340, 54, @"Harm");
            AddImage(225, 365, ImageID(Handeling.MagicTrap));
            AddLabel(255, 365, 54, @"Magic Trap");
            AddImage(225, 390, ImageID(Handeling.Untrap));
            AddLabel(255, 390, 54, @"Untrap");
            AddImage(225, 415, ImageID(Handeling.Protection));
            AddLabel(255, 415, 54, @"Protection");
            AddImage(225, 440, ImageID(Handeling.Strength));
            AddLabel(255, 440, 54, @"Strength");
        }

        public int ImageID(bool b)
        {
            if (b)
                return 211;
            else
                return 210;
        }

        public override void OnResponse(NetState sender, RelayInfo info)
        {
            Mobile from = sender.Mobile;
            from.SendGump(new RVSSetup_Rules_Spells_View(Handeling, TeamID, Index));
        }
    }

    public class RVSSetup_Rules_Spells_3rd_View : Gump
    {
        public RVS Handeling;
        public int TeamID;
        public int Index;

        public RVSSetup_Rules_Spells_3rd_View(RVS d, int team, int id)
            : base(0, 0)
        {
            Handeling = d;
            TeamID = team;
            Index = id;

            Closable = true;
            Dragable = true;
            Resizable = false;

            AddBackground(219, 238, 172, 229, 9300);
            AddLabel(255, 244, 70, @"3rd Circle Rules");

            AddImage(225, 265, ImageID(Handeling.Bless));
            AddLabel(255, 265, 54, @"Bless");
            AddImage(225, 290, ImageID(Handeling.Fireball));
            AddLabel(255, 290, 54, @"Fireball");
            AddImage(225, 315, ImageID(Handeling.MagicLock));
            AddLabel(255, 315, 54, @"Magic Lock");
            AddImage(225, 340, ImageID(Handeling.Poison));
            AddLabel(255, 340, 54, @"Poison");
            AddImage(225, 365, ImageID(Handeling.Telekinisis));
            AddLabel(255, 365, 54, @"Telekinesis");
            AddImage(225, 390, ImageID(Handeling.Teleport));
            AddLabel(255, 390, 54, @"Teleport");
            AddImage(225, 415, ImageID(Handeling.Unlock));
            AddLabel(255, 415, 54, @"Unlock Spell");
            AddImage(225, 440, ImageID(Handeling.WallOfStone));
            AddLabel(255, 440, 54, @"Wall of Stone");
        }

        public int ImageID(bool b)
        {
            if (b)
                return 211;
            else
                return 210;
        }

        public override void OnResponse(NetState sender, RelayInfo info)
        {
            Mobile from = sender.Mobile;
            from.SendGump(new RVSSetup_Rules_Spells_View(Handeling, TeamID, Index));
        }
    }

    public class RVSSetup_Rules_Spells_4th_View : Gump
    {
        public RVS Handeling;
        public int TeamID;
        public int Index;

        public RVSSetup_Rules_Spells_4th_View(RVS d, int team, int id)
            : base(0, 0)
        {
            Handeling = d;
            TeamID = team;
            Index = id;

            Closable = true;
            Dragable = true;
            Resizable = false;

            AddBackground(219, 238, 172, 229, 9300);
            AddLabel(255, 244, 70, @"4th Circle Rules");

            AddImage(225, 265, ImageID(Handeling.ArchCure));
            AddLabel(255, 265, 54, @"Arch Cure");
            AddImage(225, 290, ImageID(Handeling.Protection));
            AddLabel(255, 290, 54, @"Arch Protection");
            AddImage(225, 315, ImageID(Handeling.Curse));
            AddLabel(255, 315, 54, @"Curse");
            AddImage(225, 340, ImageID(Handeling.FireField));
            AddLabel(255, 340, 54, @"Firefield");
            AddImage(225, 365, ImageID(Handeling.GreaterHeal));
            AddLabel(255, 365, 54, @"Greater Heal");
            AddImage(225, 390, ImageID(Handeling.Lightning));
            AddLabel(255, 390, 54, @"Lightning");
            AddImage(225, 415, ImageID(Handeling.ManaDrain));
            AddLabel(255, 415, 54, @"Mana Drain");
            AddImage(225, 440, ImageID(Handeling.Recall));
            AddLabel(255, 440, 54, @"Recall");
        }

        public int ImageID(bool b)
        {
            if (b)
                return 211;
            else
                return 210;
        }

        public override void OnResponse(NetState sender, RelayInfo info)
        {
            Mobile from = sender.Mobile;
            from.SendGump(new RVSSetup_Rules_Spells_View(Handeling, TeamID, Index));
        }
    }

    public class RVSSetup_Rules_Spells_5th_View : Gump
    {
        public RVS Handeling;
        public int TeamID;
        public int Index;

        public RVSSetup_Rules_Spells_5th_View(RVS d, int team, int id)
            : base(0, 0)
        {
            Handeling = d;
            TeamID = team;
            Index = id;

            Closable = true;
            Dragable = true;
            Resizable = false;

            AddBackground(219, 238, 172, 229, 9300);
            AddLabel(255, 244, 70, @"5th Circle Rules");

            AddImage(225, 265, ImageID(Handeling.BladeSpirits));
            AddLabel(255, 265, 54, @"Blade Spirits");
            AddImage(225, 290, ImageID(Handeling.DispelField));
            AddLabel(255, 290, 54, @"Dispel Field");
            AddImage(225, 315, ImageID(Handeling.Incognito));
            AddLabel(255, 315, 54, @"Incognito");
            AddImage(225, 340, ImageID(Handeling.MagicReflection));
            AddLabel(255, 340, 54, @"Magic Reflection");
            AddImage(225, 365, ImageID(Handeling.MindBlast));
            AddLabel(255, 365, 54, @"Mind Blast");
            AddImage(225, 390, ImageID(Handeling.Paralyze));
            AddLabel(255, 390, 54, @"Paralyze");
            AddImage(225, 415, ImageID(Handeling.PoisonField));
            AddLabel(255, 415, 54, @"Poison Field");
            AddImage(225, 440, ImageID(Handeling.SummonCreature));
            AddLabel(255, 440, 54, @"Summon Creature");
        }

        public int ImageID(bool b)
        {
            if (b)
                return 211;
            else
                return 210;
        }

        public override void OnResponse(NetState sender, RelayInfo info)
        {
            Mobile from = sender.Mobile;
            from.SendGump(new RVSSetup_Rules_Spells_View(Handeling, TeamID, Index));
        }
    }

    public class RVSSetup_Rules_Spells_6th_View : Gump
    {
        public RVS Handeling;
        public int TeamID;
        public int Index;

        public RVSSetup_Rules_Spells_6th_View(RVS d, int team, int id)
            : base(0, 0)
        {
            Handeling = d;
            TeamID = team;
            Index = id;

            Closable = true;
            Dragable = true;
            Resizable = false;

            AddBackground(219, 238, 172, 229, 9300);
            AddLabel(255, 244, 70, @"6th Circle Rules");

            AddImage(225, 265, ImageID(Handeling.Dispel));
            AddLabel(255, 265, 54, @"Dispel");
            AddImage(225, 290, ImageID(Handeling.EnergyBolt));
            AddLabel(255, 290, 54, @"Energy Bolt");
            AddImage(225, 315, ImageID(Handeling.Explosion));
            AddLabel(255, 315, 54, @"Explosion");
            AddImage(225, 340, ImageID(Handeling.Invisibility));
            AddLabel(255, 340, 54, @"Invisibility");
            AddImage(225, 365, ImageID(Handeling.Mark));
            AddLabel(255, 365, 54, @"Mark");
            AddImage(225, 390, ImageID(Handeling.MassCurse));
            AddLabel(255, 390, 54, @"Mass Curse");
            AddImage(225, 415, ImageID(Handeling.ParalyzeField));
            AddLabel(255, 415, 54, @"Paralyze Field");
            AddImage(225, 440, ImageID(Handeling.Reveal));
            AddLabel(255, 440, 54, @"Reveal");
        }

        public int ImageID(bool b)
        {
            if (b)
                return 211;
            else
                return 210;
        }

        public override void OnResponse(NetState sender, RelayInfo info)
        {
            Mobile from = sender.Mobile;
            from.SendGump(new RVSSetup_Rules_Spells_View(Handeling, TeamID, Index));
        }
    }

    public class RVSSetup_Rules_Spells_7th_View : Gump
    {
        public RVS Handeling;
        public int TeamID;
        public int Index;

        public RVSSetup_Rules_Spells_7th_View(RVS d, int team, int id)
            : base(0, 0)
        {
            Handeling = d;
            TeamID = team;
            Index = id;

            Closable = true;
            Dragable = true;
            Resizable = false;

            AddBackground(219, 238, 172, 229, 9300);
            AddLabel(255, 244, 70, @"7th Circle Rules");

            AddImage(225, 265, ImageID(Handeling.ChainLightning));
            AddLabel(255, 265, 54, @"Chain Lightning");
            AddImage(225, 290, ImageID(Handeling.EnergyField));
            AddLabel(255, 290, 54, @"Energy Field");
            AddImage(225, 315, ImageID(Handeling.FlameStrike));
            AddLabel(255, 315, 54, @"Flame Strike");
            AddImage(225, 340, ImageID(Handeling.GateTravel));
            AddLabel(255, 340, 54, @"Gate Travel");
            AddImage(225, 365, ImageID(Handeling.ManaVampire));
            AddLabel(255, 365, 54, @"Mana Vampire");
            AddImage(225, 390, ImageID(Handeling.MassDispel));
            AddLabel(255, 390, 54, @"Mass Dispel");
            AddImage(225, 415, ImageID(Handeling.MeteorSwarm));
            AddLabel(255, 415, 54, @"Meteor Swarm");
            AddImage(225, 440, ImageID(Handeling.Polymorph));
            AddLabel(255, 440, 54, @"Polymorph");
        }

        public int ImageID(bool b)
        {
            if (b)
                return 211;
            else
                return 210;
        }

        public override void OnResponse(NetState sender, RelayInfo info)
        {
            Mobile from = sender.Mobile;
            from.SendGump(new RVSSetup_Rules_Spells_View(Handeling, TeamID, Index));
        }
    }

    public class RVSSetup_Rules_Spells_8th_View : Gump
    {
        public RVS Handeling;
        public int TeamID;
        public int Index;

        public RVSSetup_Rules_Spells_8th_View(RVS d, int team, int id)
            : base(0, 0)
        {
            Handeling = d;
            TeamID = team;
            Index = id;

            Closable = true;
            Dragable = true;
            Resizable = false;

            AddBackground(219, 238, 172, 229, 9300);
            AddLabel(255, 244, 70, @"8th Circle Rules");

            AddImage(225, 265, ImageID(Handeling.EarthQuake));
            AddLabel(255, 265, 54, @"Earthquake");
            AddImage(225, 290, ImageID(Handeling.EnergyVotex));
            AddLabel(255, 290, 54, @"Energy Vortex");
            AddImage(225, 315, ImageID(Handeling.Resurrection));
            AddLabel(255, 315, 54, @"Resurrection");
            AddImage(225, 340, ImageID(Handeling.SummonAirElemental));
            AddLabel(255, 340, 54, @"Summon Air");
            AddImage(225, 365, ImageID(Handeling.SummonDaemon));
            AddLabel(255, 365, 54, @"Summon Daemon");
            AddImage(225, 390, ImageID(Handeling.SummonEarthElemental));
            AddLabel(255, 390, 54, @"Summon Earth");
            AddImage(225, 415, ImageID(Handeling.SummonFireElemental));
            AddLabel(255, 415, 54, @"Summon Fire");
            AddImage(225, 440, ImageID(Handeling.SummonWaterElemental));
            AddLabel(255, 440, 54, @"Summon Water");
        }

        public int ImageID(bool b)
        {
            if (b)
                return 211;
            else
                return 210;
        }

        public override void OnResponse(NetState sender, RelayInfo info)
        {
            Mobile from = sender.Mobile;
            from.SendGump(new RVSSetup_Rules_Spells_View(Handeling, TeamID, Index));
        }
    }

    public class RVSSetup_Rules_Samurai_View : Gump
    {
        public RVS Handeling;
        public int TeamID;
        public int Index;

        public RVSSetup_Rules_Samurai_View(RVS d, int team, int id)
            : base(0, 0)
        {
            Handeling = d;
            TeamID = team;
            Index = id;

            Closable = true;
            Dragable = true;
            Resizable = false;

            AddBackground(219, 238, 172, 204, 9300);
            AddLabel(255, 244, 70, @"Samurai Spell Rules");

            AddImage(225, 265, ImageID(Handeling.AllowSamuraiSpells));
            AddLabel(255, 265, 54, @"Allow Samurai Spells");
            AddImage(225, 290, ImageID(Handeling.Confidence));
            AddLabel(255, 290, 54, @"Confidence");
            AddImage(225, 315, ImageID(Handeling.CounterAttack));
            AddLabel(255, 315, 54, @"Counter Attack");
            AddImage(225, 340, ImageID(Handeling.Evasion));
            AddLabel(255, 340, 54, @"Evasion");
            AddImage(225, 365, ImageID(Handeling.HonorableExecution));
            AddLabel(255, 365, 54, @"Honorable Execution");
            AddImage(225, 390, ImageID(Handeling.LightningStrike));
            AddLabel(255, 390, 54, @"Lightning Strike");
            AddImage(225, 415, ImageID(Handeling.MomentumStrike));
            AddLabel(255, 415, 54, @"Momentum Strike");
        }

        public int ImageID(bool b)
        {
            if (b)
                return 211;
            else
                return 210;
        }

        public override void OnResponse(NetState sender, RelayInfo info)
        {
            Mobile from = sender.Mobile;
            from.SendGump(new RVSSetup_Rules_View(Handeling, TeamID, Index));
        }
    }

    public class RVSSetup_Rules_Chivalry_View : Gump
    {
        public RVS Handeling;
        public int TeamID;
        public int Index;

        public RVSSetup_Rules_Chivalry_View(RVS d, int team, int id)
            : base(0, 0)
        {
            Handeling = d;
            TeamID = team;
            Index = id;

            Closable = true;
            Dragable = true;
            Resizable = false;

            AddBackground(219, 238, 172, 304, 9300);
            AddLabel(255, 244, 70, @"Paladin Spell Rules");

            AddImage(225, 265, ImageID(Handeling.AllowChivalry));
            AddLabel(255, 265, 54, @"Allow Chivalry");
            AddImage(225, 290, ImageID(Handeling.ClenseByFire));
            AddLabel(255, 290, 54, @"Cleanse by Fire");
            AddImage(225, 315, ImageID(Handeling.CloseWounds));
            AddLabel(255, 315, 54, @"Close Wounds");
            AddImage(225, 340, ImageID(Handeling.ConsecrateWeapon));
            AddLabel(255, 340, 54, @"Consecrate Weapon");
            AddImage(225, 365, ImageID(Handeling.DispellEvil));
            AddLabel(255, 365, 54, @"Dispell Evil");
            AddImage(225, 390, ImageID(Handeling.DivineFury));
            AddLabel(255, 390, 54, @"Divine Fury");
            AddImage(225, 415, ImageID(Handeling.EnemyOfOne));
            AddLabel(255, 415, 54, @"Enemy of One");
            AddImage(225, 440, ImageID(Handeling.HolyLight));
            AddLabel(255, 440, 54, @"Holy Light");
            AddImage(225, 465, ImageID(Handeling.NobleSacrafice));
            AddLabel(255, 465, 54, @"Noble Sacrafice");
            AddImage(225, 490, ImageID(Handeling.RemoveCurse));
            AddLabel(255, 490, 54, @"Remove Curse");
            AddImage(225, 515, ImageID(Handeling.SacredJourny));
            AddLabel(255, 515, 54, @"Sacred Journey");
        }

        public int ImageID(bool b)
        {
            if (b)
                return 211;
            else
                return 210;
        }

        public override void OnResponse(NetState sender, RelayInfo info)
        {
            Mobile from = sender.Mobile;
            from.SendGump(new RVSSetup_Rules_View(Handeling, TeamID, Index));
        }
    }

    public class RVSSetup_Rules_Ninjitsu_View : Gump
    {
        public RVS Handeling;
        public int TeamID;
        public int Index;

        public RVSSetup_Rules_Ninjitsu_View(RVS d, int team, int id)
            : base(0, 0)
        {
            Handeling = d;
            TeamID = team;
            Index = id;

            Closable = true;
            Dragable = true;
            Resizable = false;

            AddBackground(219, 238, 172, 254, 9300);
            AddLabel(255, 244, 70, @"Ninja Spell Rules");

            AddImage(225, 265, ImageID(Handeling.AllowNinjaSpells));
            AddLabel(255, 265, 54, @"Allow Ninjitsu");
            AddImage(225, 290, ImageID(Handeling.AnimalForm));
            AddLabel(255, 290, 54, @"Animal Form");
            AddImage(225, 315, ImageID(Handeling.Backstab));
            AddLabel(255, 315, 54, @"Backstab");
            AddImage(225, 340, ImageID(Handeling.DeathStrike));
            AddLabel(255, 340, 54, @"Death Strike");
            AddImage(225, 365, ImageID(Handeling.FocusAttack));
            AddLabel(255, 365, 54, @"Focus Attack");
            AddImage(225, 390, ImageID(Handeling.KiAttack));
            AddLabel(255, 390, 54, @"Ki Attack");
            AddImage(225, 415, ImageID(Handeling.MirrorImage));
            AddLabel(255, 415, 54, @"Mirror Image");
            AddImage(225, 440, ImageID(Handeling.ShadowJump));
            AddLabel(255, 440, 54, @"Shadow Jump");
            AddImage(225, 465, ImageID(Handeling.SurpriseAttack));
            AddLabel(255, 465, 54, @"Surprise Attack");
        }

        public int ImageID(bool b)
        {
            if (b)
                return 211;
            else
                return 210;
        }

        public override void OnResponse(NetState sender, RelayInfo info)
        {
            Mobile from = sender.Mobile;
            from.SendGump(new RVSSetup_Rules_View(Handeling, TeamID, Index));
        }
    }

    public class RVSSetup_Rules_Necromancy_View : Gump
    {
        public RVS Handeling;
        public int TeamID;
        public int Index;

        public RVSSetup_Rules_Necromancy_View(RVS d, int team, int id)
            : base(0, 0)
        {
            Handeling = d;
            TeamID = team;
            Index = id;

            Closable = true;
            Dragable = true;
            Resizable = false;

            AddBackground(219, 238, 172, 479, 9300);
            AddLabel(255, 234, 70, @"Necromancer Spell Rules");

            AddImage(225, 265, ImageID(Handeling.AllowNecromancy));
            AddLabel(255, 265, 54, @"Allow Necromancy");
            AddImage(225, 290, ImageID(Handeling.AnimateDead));
            AddLabel(255, 290, 54, @"Animate Dead");
            AddImage(225, 315, ImageID(Handeling.BloodOath));
            AddLabel(255, 315, 54, @"Blood Oath");
            AddImage(225, 340, ImageID(Handeling.CorpseSkin));
            AddLabel(255, 340, 54, @"Corpse Skin");
            AddImage(225, 365, ImageID(Handeling.CurseWeapon));
            AddLabel(255, 365, 54, @"Curse Weapon");
            AddImage(225, 390, ImageID(Handeling.EvilOmen));
            AddLabel(255, 390, 54, @"Evil Omen");
            AddImage(225, 415, ImageID(Handeling.Exorcisim));
            AddLabel(255, 415, 54, @"Exorcism");
            AddImage(225, 440, ImageID(Handeling.HorrificBeast));
            AddLabel(255, 440, 54, @"Horrific Beast");
            AddImage(225, 465, ImageID(Handeling.LichForm));
            AddLabel(255, 465, 54, @"Lich Form");
            AddImage(225, 490, ImageID(Handeling.MindRot));
            AddLabel(255, 490, 54, @"Mind Rot");
            AddImage(225, 515, ImageID(Handeling.PainSpike));
            AddLabel(255, 515, 54, @"Pain Spike");
            AddImage(225, 540, ImageID(Handeling.PoisonStrike));
            AddLabel(255, 540, 54, @"Poison Strike");
            AddImage(225, 565, ImageID(Handeling.Strangle));
            AddLabel(255, 565, 54, @"Strangle");
            AddImage(225, 590, ImageID(Handeling.SummonFamiliar));
            AddLabel(255, 590, 54, @"Summon Familiar");
            AddImage(225, 615, ImageID(Handeling.VampiricEmbrace));
            AddLabel(255, 615, 54, @"Vampiric Embrace");
            AddImage(225, 640, ImageID(Handeling.VengefulSpirit));
            AddLabel(255, 640, 54, @"Vengeful Spirit");
            AddImage(225, 665, ImageID(Handeling.Wither));
            AddLabel(255, 665, 54, @"Wither");
            AddImage(225, 690, ImageID(Handeling.WraithForm));
            AddLabel(255, 690, 54, @"Wraith Form");
        }

        public int ImageID(bool b)
        {
            if (b)
                return 211;
            else
                return 210;
        }

        public override void OnResponse(NetState sender, RelayInfo info)
        {
            Mobile from = sender.Mobile;
            from.SendGump(new RVSSetup_Rules_View(Handeling, TeamID, Index));
        }
    }
}