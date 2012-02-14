using System;
using Server;
using Server.Gumps;
using Server.Items;
using Server.Network;
using Server.Commands;
using Server.Mobiles;
using System.Collections;
using System.Collections.Generic;

namespace Server.Dueling
{
	public class DuelSetup_Main : Gump
	{
        public Duel Handling;

		public DuelSetup_Main(Duel d) : base(0, 0)
		{
            Handling = d;

			Closable = true;
			Dragable = true;
			Resizable = false;

            int bh = 165 + (Handling.Teams.Count * 20);
            AddBackground(281, 144, 279, bh, 9300);
            AddLabel(381, 151, 70, @"Duel Setup");
            AddLabel(315, 172, 40, @"Rules");
            AddLabel(315, 192, 70, @"Start Duel");
            AddLabel(315, 212, 53, @"Set Buy In");
            AddLabel(315, 232, 104, @"Add A Team");
            AddLabel(315, 252, 142, @"Remove A Team");
            AddLabel(376, 278, 104, @"Participants");
            AddButton(290, 175, 1209, 1210, 2, GumpButtonType.Reply, 0); // Rules
            AddButton(290, 195, 1209, 1210, 1, GumpButtonType.Reply, 0); // Start Duel
            AddButton(290, 215, 1209, 1210, 5, GumpButtonType.Reply, 0); // Set Buy In
            AddButton(290, 235, 1209, 1210, 3, GumpButtonType.Reply, 0); // Add A Team
            AddButton(290, 255, 1209, 1210, 4, GumpButtonType.Reply, 0); // Remove A Team

            int y = 298;
            int id = 6;
            IEnumerator key = Handling.Teams.Keys.GetEnumerator();
            for (int i = 0; i < Handling.Teams.Count; ++i)
            {
                key.MoveNext();
                Duel_Team team = (Duel_Team)Handling.Teams[(int)key.Current];
                AddLabel(315, (y - 3), 104, String.Format(@"Team {0}: {1} out of {2} players", team.TeamID.ToString(), TeamPlayers(team).ToString(), team.Size.ToString()));
                AddButton(290, y, 1209, 1210, id, GumpButtonType.Reply, 0); // View A Team

                y += 20;
                id += 1;
            }
		}

        public int TeamPlayers(Duel_Team team)
        {
            int toreturn = 0;

            for (int i = 0; i < team.Players.Count; ++i)
            {
                if (team.Players[i] != "@null")
                    toreturn += 1;
            }

            return toreturn;
        }

		public override void OnResponse( NetState sender, RelayInfo info )
		{
			Mobile from = sender.Mobile;

            switch (info.ButtonID)
            {
                case 0: // Close
                    {
                        Handling.EndSelf();
                        return;
                    }
                case 1: // Start
                    {
                        if (FilledTeams(Handling) < 2)
                        {
                            Handling.EchoMessage("You must have at least two participating teams to start a duel.");
                            from.SendGump(new DuelSetup_Main(Handling));
                            return;
                        }
                        else if (!CheckLoggedIn())
                        {
                            Handling.EchoMessage("One or more players participating in this duel have logged out, duel start delayed.");
                            from.SendGump(new DuelSetup_Main(Handling));
                            return;
                        }
                        else if (!AllAlive())
                        {
                            Handling.EchoMessage("Not all players participating in the duel are alive, duel start delayed.");
                            from.SendGump(new DuelSetup_Main(Handling));
                            return;
                        }
                        else // Start Duel
                        {
                            Handling.UpdateAllPending();
                            return;
                        }
                    }
                case 2: // Rules
                    {
                        from.SendGump(new DuelSetup_Rules(Handling));
                        from.SendMessage("If the box next to something is checked, it is allowed.");
                        return;
                    }
                case 3: // Add A Team
                    {
                        if (Handling.Teams.Count >= 4)
                        {
                            from.SendMessage("There is a maximum of 4 teams");
                            from.SendGump(new DuelSetup_Main(Handling));
                            return;
                        }

                        Duel_Team toadd = new Duel_Team((Handling.Teams.Count + 1));
                        toadd.Players.Add("@null");
                        Handling.Teams.Add((Handling.Teams.Count + 1), toadd);
                        from.SendGump(new DuelSetup_Main(Handling));
                        return;
                    }
                case 4: // Remove A Team
                    {
                        if (Handling.Teams.Count <= 2)
                        {
                            from.SendMessage("Cannot have less then 2 teams.");
                            from.SendGump(new DuelSetup_Main(Handling));
                            return;
                        }

                        Duel_Team toremove = (Duel_Team)Handling.Teams[Handling.Teams.Count];

                        if (toremove.Players.Count <= 1 && toremove.Players[0] == "@null")
                            Handling.Teams.Remove(Handling.Teams.Count);
                        else
                            from.SendMessage("You can not remove a team that contains players, move the players to a higher team.");

                        from.SendGump(new DuelSetup_Main(Handling));
                        return;
                    }
                case 5: // Set Buy In
                    {
                        if (FilledTeams(Handling) != 1)
                        {
                            from.SendMessage("You cannot set the duel buy in after you have already added players.");
                            from.SendGump(new DuelSetup_Main(Handling));
                            return;
                        }

                        from.SendGump(new DuelSetup_SetBuyIn(Handling));
                        return;
                    }
            }

            if (info.ButtonID >= 6)
            {
                int id = (info.ButtonID - 5);
                from.SendGump(new DuelSetup_ParticipantSetup(Handling, id));
            }
		}

        public bool AllAlive()
        {
            IEnumerator key = Handling.Teams.Keys.GetEnumerator();
            for (int i = 0; i < Handling.Teams.Count; ++i)
            {
                key.MoveNext();
                Duel_Team team = (Duel_Team)Handling.Teams[(int)key.Current];

                for (int i2 = 0; i2 < team.Players.Count; ++i2)
                {
                    object o = (object)team.Players[i2];

                    if (o is PlayerMobile)
                    {
                        PlayerMobile pm = (PlayerMobile)o;

                        if (!pm.Alive)
                            return false;
                    }
                }
            }

            return true;
        }

        public bool CheckLoggedIn()
        {
            IEnumerator key = Handling.Teams.Keys.GetEnumerator();
            for (int i = 0; i < Handling.Teams.Count; ++i)
            {
                key.MoveNext();
                Duel_Team team = (Duel_Team)Handling.Teams[(int)key.Current];
                
                bool alllogged = true;
                for (int i2 = 0; i2 < team.Players.Count; ++i2)
                {
                    object o = (object)team.Players[i2];

                    if (o is PlayerMobile)
                    {
                        PlayerMobile pm = (PlayerMobile)o;

                        if (pm.NetState == null)
                        {
                            alllogged = false;
                            Handling.Teams[(int)key.Current].Players[i2] = "@null";
                            Handling.Teams[(int)key.Current].Accepted.Remove(pm);
                        }
                    }
                }

                if (!alllogged)
                    return false;
            }

            return true;
        }

        public int FilledTeams(Duel d)
        {
            int filled = 0;
            IEnumerator key = Handling.Teams.Keys.GetEnumerator();
            for (int i = 0; i < Handling.Teams.Count; ++i)
            {
                key.MoveNext();
                Duel_Team d_team = (Duel_Team)Handling.Teams[(int)key.Current];

                bool hasplayers = false;
                for (int i2 = 0; i2 < d_team.Players.Count; ++i2)
                {
                    object o = (object)d_team.Players[i2];

                    if (o is PlayerMobile)
                        hasplayers = true;
                }

                if (hasplayers)
                    filled += 1;
            }

            return filled;
        }
    }
}
