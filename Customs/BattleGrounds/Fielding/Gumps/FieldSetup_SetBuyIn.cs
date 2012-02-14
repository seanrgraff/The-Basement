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

namespace Server.Fielding
{
    public class FieldSetup_SetBuyIn : Gump
    {
        public Field Handeling;
        public string Mode;

        public FieldSetup_SetBuyIn(Field d)
            : base(0, 0)
        {
            Handeling = d;

            AddBackground(135, 134, 240, 114, 9300);
            AddLabel(201, 142, 70, @"Set Field Buy In");
            AddBackground(140, 165, 230, 26, 9350);

            if (Handeling.BuyIn > 0)
            {
                Mode = "remove";
                AddButton(145, 220, 1209, 1210, 1, GumpButtonType.Reply, 0); // Set/Remove Buy In
                AddLabel(165, 217, 40, @"Remove Buy In");
            }
            else
            {
                Mode = "set";
                AddButton(145, 220, 1209, 1210, 1, GumpButtonType.Reply, 0); // Set/Remove Buy In
                AddLabel(165, 217, 40, @"Set Buy In");
            }

            AddTextEntry(142, 169, 224, 19, 52, 3, Handeling.BuyIn.ToString());
        }

        public override void OnResponse(NetState sender, RelayInfo info)
        {
            Mobile from = sender.Mobile;

            switch (info.ButtonID)
            {
                case 0: // Close
                    {
                        from.SendGump(new FieldSetup_Main(Handeling));
                        break;
                    }
                case 1:
                    {
                        TextRelay text = (TextRelay)info.GetTextEntry(3);
                        int amount = 0;
                        try
                        {
                            amount = Convert.ToInt32(text.Text);
                        }
                        catch(Exception)
                        {
                            from.SendMessage("Invalid amount entered.");
                            from.SendGump(new FieldSetup_SetBuyIn(Handeling));
                            return;
                        }

                        switch (Mode)
                        {
                            case "remove":
                                {
                                    RefundBuyIn((PlayerMobile)from, amount);
                                    break;
                                }
                            case "set":
                                {
                                    if (amount <= 0)
                                    {
                                        from.SendMessage("You must enter more then 0.");
                                        from.SendGump(new FieldSetup_SetBuyIn(Handeling));
                                        return;
                                    }

                                    if (!HasGold((PlayerMobile)from, amount))
                                    {
                                        from.SendMessage("You do not have that much gold.");
                                        from.SendGump(new FieldSetup_SetBuyIn(Handeling));
                                        return;
                                    }

                                    SetBuyIn((PlayerMobile)from, amount);
                                    break;
                                }
                        }

                        from.SendGump(new FieldSetup_SetBuyIn(Handeling));
                        break;
                    }
            }
        }

        public void SetBuyIn(PlayerMobile pm, int amount)
        {
            Item i = CheckForGoldSources(pm, amount);

            if (i is Gold)
            {
                i.Amount -= amount;
                Handeling.BuyIn += amount;
            }
            else
            {
                BankCheck check = (BankCheck)i;

                check.Worth -= amount;
                Handeling.BuyIn += amount;
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

            pm.SendMessage("The funds have been gathered.");
        }

        public void RefundBuyIn(PlayerMobile pm, int amount)
        {
            BankBox box = (BankBox)pm.BankBox;

            if (amount >= 5000)
            {
                BankCheck check = new BankCheck(amount);
                box.DropItem(check);
            }
            else
            {
                Gold gold = new Gold(amount);
                box.DropItem(gold);
            }

            Handeling.BuyIn = 0;
            pm.SendMessage("The buy in has been refunded.");
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

            foreach(Item i in pack.Items)
                if(i is Gold)
                    gold_piles.Add((Gold)i);
                else if(i is BankCheck)
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
    }
}