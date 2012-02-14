using System;
using Server;
using Server.Targeting;
using Server.Gumps;
using Server.Items;
using Server.Network;
using Server.Commands;
using Server.Mobiles;
using System.Collections;
using System.Collections.Generic;
using Server.Accounting;
using Server.Factions;
using Server.Fielding;

namespace Server.Dueling
{
	public class DuelSetup_ParticipantSetup : Gump
	{
        public Duel Handeling;
        public Duel_Team D_Team;
        public int TeamID;

		public DuelSetup_ParticipantSetup(Duel d, int id) : base(0, 0)
		{
            Handeling = d;
            TeamID = id;
            D_Team = (Duel_Team)Handeling.Teams[TeamID];

			Closable = true;
			Dragable = true;
			Resizable = false;

            int bh = 121 + (D_Team.Size * 20);
            AddBackground(115, 85, 215, bh, 9300);
            AddLabel(164, 91, 70, @"Participant Setup");
            AddLabel(123, 113, 113, String.Format(@"Team Size: {0}", D_Team.Size.ToString()));

            AddButton(145, 138, 1209, 1210, 1, GumpButtonType.Reply, 0);
            AddLabel(166, 135, 144, @"Increase Size");
            AddButton(145, 158, 1209, 1210, 2, GumpButtonType.Reply, 0);
            AddLabel(166, 155, 144, @"Decrease Size");

            AddLabel(177, 180, 70, @"Team Players");
            int y = 204, bi = 3;
            for (int i = 0; i < D_Team.Size; ++i)
            {
                if (Handeling.Teams[TeamID].Players[i] == "@null")
                    AddLabel(144, (y - 3), 254, String.Format(@"{0}: Empty", (i +1).ToString()));
                else
                {
                    PlayerMobile pm = (PlayerMobile)D_Team.Players[i];
                    AddLabel(144, (y - 3), 254, String.Format(@"{0}: {1}", (i + 1).ToString(), pm.Name));
                }
                AddButton(123, y, 1209, 1210, bi, GumpButtonType.Reply, 0);

                y += 20;
                bi += 1;
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
                case 0:
                    {
                        from.CloseGump(typeof(DuelSetup_Main));
                        from.SendGump(new DuelSetup_Main(Handeling));
                        return;
                    }
                case 1:
                    {
                        if (D_Team.Size >= 6)
                        {
                            from.SendMessage("There is a maximum of 6 players per team.");
                            from.SendGump(new DuelSetup_ParticipantSetup(Handeling, TeamID));
                            return;
                        }

                        Handeling.Teams[TeamID].Size += 1;
                        Handeling.Teams[TeamID].Players.Add("@null");
                        from.SendGump(new DuelSetup_ParticipantSetup(Handeling, TeamID));
                        break;
                    }
                case 2:
                    {
                        if (D_Team.Size <= 1)
                        {
                            from.SendMessage("A team may not have less then 1 player.");
                            from.SendGump(new DuelSetup_ParticipantSetup(Handeling, TeamID));
                            return;
                        }

                        int at = ((int)Handeling.Teams[TeamID].Players.Count - 1);
                        if (Handeling.Teams[TeamID].Players[at] != "@null")
                        {
                            from.SendMessage("A player slot must be empty to be removed.");
                            from.SendGump(new DuelSetup_ParticipantSetup(Handeling, TeamID));
                            return;
                        }

                        Handeling.Teams[TeamID].Players.RemoveAt(at);
                        Handeling.Teams[TeamID].Size -= 1;
                        from.SendMessage("The player slot has been removed.");
                        from.SendGump(new DuelSetup_ParticipantSetup(Handeling, TeamID));
                        break;
                    }
            }

            if (info.ButtonID >= 3)
            {
                int index = (info.ButtonID - 3);

                if (D_Team.Players[index] == Handeling.Caller)
                {
                    from.SendMessage("You cannot remove your self from the duel.");
                    from.SendGump(new DuelSetup_ParticipantSetup(Handeling, TeamID));
                    return;
                }

                if (D_Team.Players[index] != "@null")
                {
                    PlayerMobile pm = (PlayerMobile)D_Team.Players[index];
                    Handeling.Teams[TeamID].Players[index] = "@null";
                    Handeling.Teams[TeamID].Accepted.Remove(pm);
                    pm.SendMessage("You have been removed from the duel.");
                    Handeling.RefundBuyIn(pm);

                    from.SendMessage("The player has been removed.");
                    from.SendGump(new DuelSetup_ParticipantSetup(Handeling, TeamID));
                    return;
                }
                else
                {
                    from.Target = new Duel_AddTarget(Handeling, TeamID, index);
                    return;
                }
            }
		}
	}

    public class DuelSetup_AddParticipant : Gump
    {
        public Duel Handeling;
        public int TeamID;
        public Duel_Team D_Team;
        public int Index;

        public DuelSetup_AddParticipant(Duel d, int team, int i)
            : base(0, 0)
        {
            Handeling = d;
            TeamID = team;
            Index = i;
            D_Team = (Duel_Team)Handeling.Teams[TeamID];

            Closable = true;
            Dragable = true;
            Resizable = false;

            AddBackground(187, 187, 260, 216, 9300);
            AddLabel(239, 193, 40, @"Incoming Duel Challenge");

            AddHtml(192, 217, 248, 85, @"You have been challenged to a duel by another player, you can choose to either accept or decline this challenge. If you choose to accept the challenge  you will be added to the duel, if you choose to decline the challenge you will not be added to the duel. To accept some duel's you must pay a lump sum, this is known as dueling for cash, the gold is given to the winning team of the duel.", (bool)true, (bool)true);
            AddLabel(200, 310, 33, String.Format(@"Challenger: {0}", Handeling.Caller.Name));
            AddLabel(200, 330, 54, String.Format(@"Buy In: {0}gp", Handeling.BuyIn.ToString()));

            AddButton(200, 360, 1209, 1210, 1, GumpButtonType.Reply, 0);
            AddLabel(225, 357, 42, @"I will accept the duel");
            AddButton(200, 380, 1209, 1210, 2, GumpButtonType.Reply, 0);
            AddLabel(225, 377, 67, @"I will decline the duel");
        }

        public bool HasGold(PlayerMobile pm, int amount)
        {
            Item i = CheckForGoldSources(pm, amount);

            if (i == null)
                return false;

            return true;
        }

        public Item CheckForGoldSources(PlayerMobile pm, int amount)
        {
            List<Gold> gold_piles = new List<Gold>();
            List<BankCheck> checks = new List<BankCheck>();
            Backpack pack = (Backpack)pm.Backpack;
            BankBox bank = (BankBox)pm.BankBox;

            foreach (Item i in pack.Items)
                if (i is Gold)
                    gold_piles.Add((Gold)i);
                else if (i is BankCheck)
                    checks.Add((BankCheck)i);

            foreach (Item i in bank.Items)
                if (i is Gold)
                    gold_piles.Add((Gold)i);
                else if (i is BankCheck)
                    checks.Add((BankCheck)i);

            for (int i = 0; i < gold_piles.Count; ++i)
            {
                Gold gold = (Gold)gold_piles[i];

                if (gold.Amount >= amount)
                    return gold;
            }

            for (int i = 0; i < checks.Count; ++i)
            {
                BankCheck check = (BankCheck)checks[i];

                if (check.Worth >= amount)
                    return check;
            }

            return null;
        }

        public override void OnResponse(NetState sender, RelayInfo info)
        {
            PlayerMobile from = (PlayerMobile)sender.Mobile;

            Handeling.AddTimer.Stop();

            switch (info.ButtonID)
            {
                case 0:
                    {
                        Handeling.Caller.SendMessage(String.Format("{0} has declined the duel.", from.Name));
                        Handeling.Caller.SendGump(new DuelSetup_ParticipantSetup(Handeling, TeamID));
                        break;
                    }
                case 1:
                    {
                        if (Handeling.BuyIn > 0)
                        {
                            if (!HasGold(from, Handeling.BuyIn))
                            {
                                from.SendMessage("You do not have enough money to buy into this duel.");
                                from.SendGump(new DuelSetup_AddParticipant(Handeling, TeamID, Index));
                                return;
                            }
                            else
                            {
                                Item i = CheckForGoldSources(from, Handeling.BuyIn);

                                if (i is Gold)
                                    i.Amount -= Handeling.BuyIn;
                                else
                                {
                                    BankCheck check = (BankCheck)i;
                                    check.Worth -= Handeling.BuyIn;
                                }

                                if (i.Amount <= 0)
                                {
                                    i.Delete();
                                    return;
                                }

                                if (i is BankCheck)
                                {
                                    BankCheck check = (BankCheck)i;

                                    if (check.Worth > 0)
                                    {
                                        if (check.Worth < 5000)
                                        {
                                            if (check.Parent is BankBox)
                                            {
                                                BankBox box = (BankBox)check.Parent;

                                                Gold g = new Gold(check.Worth);
                                                box.DropItem(g);

                                                check.Delete();
                                            }

                                            if (check.Parent is Backpack)
                                            {
                                                Backpack pack = (Backpack)check.Parent;

                                                Gold g = new Gold(check.Worth);
                                                pack.DropItem(g);

                                                check.Delete();
                                            }
                                        }
                                    }
                                    else
                                        check.Delete();
                                }
                                from.SendMessage("The buy in funds for the duel have been paid.");
                            }
                        }

                        Handeling.Caller.SendMessage(String.Format("{0} has accepted the duel.", from.Name));
                        Handeling.Teams[TeamID].Players[Index] = from;
                        Handeling.Teams[TeamID].Accepted.Add((PlayerMobile)from, false);

                        Handeling.Caller.SendGump(new DuelSetup_ParticipantSetup(Handeling, TeamID));
                        break;
                    }
                case 2:
                    {
                        Handeling.Caller.SendMessage(String.Format("{0} has declined the duel.", from.Name));
                        Handeling.Caller.SendGump(new DuelSetup_ParticipantSetup(Handeling, TeamID));
                        break;
                    }
            }
        }
    }

    public class Duel_AddTarget : Target
    {
        public Duel Handeling;
        public int TeamID;
        public Duel_Team D_Team;
        public int Index;

        public Duel_AddTarget(Duel d, int team, int i)
            : base(16, false, TargetFlags.None)
        {
            Handeling = d;
            TeamID = team;
            Index = i;
            D_Team = (Duel_Team)Handeling.Teams[TeamID];
        }

        public bool CanTarget(Mobile from, object o)
        {
            PlayerMobile pm = (PlayerMobile)from;
            PlayerMobile target;

            if (o is PlayerMobile) { target = (PlayerMobile)o; }
            else
            {
                pm.SendMessage("You must target another player.");
                return false;
            }

            if (target.Criminal)
            {
                //pm.SendMessage("They are criminal and cannot participate in a duel.");
                //return false;
            }

            if (target.Combatant != null)
            {
                pm.SendMessage("They are in combat and cannot participate.");
                return false;
            }

            if (target.Aggressed.Count > 0 || target.Aggressors.Count > 0)
            {
                //pm.SendMessage("They have been in combat recently and cannot participate.");
                //return false;
            }

            if (!target.Alive)
            {
                pm.SendMessage("Only living players can participate in a duel.");
                return false;
            }

            if (Duel_Config.InADuel(target) || Field_Config.InAField(target))
            {
                pm.SendMessage("They are already in a duel.");
                return false;
            }

            if (!Duel_Config.AllowSameIPDuels && SameIP(pm, (PlayerMobile)target))
            {
                pm.SendMessage("You cannot duel them as there account is sharing the same ip as yours.");
                return false;
            }

            if (!Duel_Config.AllowFactionersToDuel)
            {
                if (Faction.Find(target) != null)
                {
                    pm.SendMessage("Factioners cannot duel.");
                    return false;
                }
            }

            return true;
        }

        public bool SameIP(PlayerMobile from, PlayerMobile target)
        {
            Account acct1 = (Account)from.Account;
            Account acct2 = (Account)target.Account;

            if (acct1.LoginIPs.GetValue(0) == null || acct2.LoginIPs.GetValue(0) == null)
                return false;

            if (acct1.LoginIPs.GetValue(0) == acct2.LoginIPs.GetValue(0))
                return true;

            return false;
        }

        protected override void OnTarget(Mobile from, object targeted)
        {
            if (CanTarget(from, targeted))
            {
                from.SendMessage("You request has been sent, please wait while they reply.");
                PlayerMobile tosend = (PlayerMobile)targeted;
                Duel_Add_Timer tmr = new Duel_Add_Timer((PlayerMobile)from, tosend, Handeling, TeamID);
                Handeling.AddTimer = tmr;
                tosend.SendGump(new DuelSetup_AddParticipant(Handeling, TeamID, Index));
            }
            else
                from.SendGump(new DuelSetup_ParticipantSetup(Handeling, TeamID));
        }

        protected override void OnTargetCancel(Mobile from, TargetCancelType cancelType)
        {
            from.SendGump(new DuelSetup_ParticipantSetup(Handeling, TeamID));
        }

        protected override void OnTargetDeleted(Mobile from, object targeted)
        {
            from.SendGump(new DuelSetup_ParticipantSetup(Handeling, TeamID));
        }

	protected override void OnTargetOutOfLOS(Mobile from, object targeted)
	{
	    from.SendMessage("You cannot see them. Try again.");
	    from.SendGump(new DuelSetup_ParticipantSetup(Handeling, TeamID));
	}
    }

    public class Duel_Add_Timer : Timer
    {
        PlayerMobile Sent;
        PlayerMobile Recieved;
        Duel Handeling;
        int TeamID;

        public Duel_Add_Timer(PlayerMobile caller, PlayerMobile reciever, Duel d, int team): base (TimeSpan.FromSeconds(25))
        {
            Sent = caller;
            Recieved = reciever;
            Handeling = d;
            TeamID = team;
            this.Start();
        }

        protected override void OnTick()
        {
            if (Recieved.HasGump(typeof(DuelSetup_AddParticipant)))
            {
                Sent.SendMessage(String.Format("{0} seems to be unresponsive.", Recieved.Name));
                Recieved.CloseGump(typeof(DuelSetup_AddParticipant));
                Sent.SendGump(new DuelSetup_ParticipantSetup(Handeling, TeamID));
            }

            this.Stop();
        }
    }
}
