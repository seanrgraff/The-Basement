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
    public class DuelSetup_Rules_Skills : Gump
    {
        public Duel Handeling;

        public DuelSetup_Rules_Skills(Duel d)
            : base(0, 0)
        {
            Handeling = d;

            Closable = true;
            Dragable = true;
            Resizable = false;

            AddBackground(219, 238, 172, 254, 9300);
            AddLabel(255, 244, 70, @"Skill Use Rules");

            AddCheck(225, 265, 210, 211, Handeling.Anatomy, 1);
            AddLabel(255, 265, 54, @"Anatomy");
            AddCheck(225, 290, 210, 211, Handeling.DetectHidden, 2);
            AddLabel(255, 290, 54, @"Detect Hidden");
            AddCheck(225, 315, 210, 211, Handeling.EvaluatingIntelligence, 3);
            AddLabel(255, 315, 54, @"Evaluating Intelligence");
            AddCheck(225, 340, 210, 211, Handeling.Hiding, 4);
            AddLabel(255, 340, 54, @"Hiding");
            AddCheck(225, 365, 210, 211, Handeling.Poisoning, 5);
            AddLabel(255, 365, 54, @"Poisoning");
            AddCheck(225, 390, 210, 211, Handeling.Snooping, 6);
            AddLabel(255, 390, 54, @"Snooping");
            AddCheck(225, 415, 210, 211, Handeling.Stealing, 7);
            AddLabel(255, 415, 54, @"Stealing");
            AddCheck(225, 440, 210, 211, Handeling.SpiritSpeak, 8);
            AddLabel(255, 440, 54, @"Spirit Speak");
            AddCheck(225, 465, 210, 211, Handeling.Stealth, 9);
            AddLabel(255, 465, 54, @"Stealth");
        }

        public override void OnResponse(NetState sender, RelayInfo info)
        {
            Mobile from = sender.Mobile;

            Handeling.Anatomy = info.IsSwitched(1);
            Handeling.DetectHidden = info.IsSwitched(2);
            Handeling.EvaluatingIntelligence = info.IsSwitched(3);
            Handeling.Hiding = info.IsSwitched(4);
            Handeling.Poisoning = info.IsSwitched(5);
            Handeling.Snooping = info.IsSwitched(6);
            Handeling.Stealing = info.IsSwitched(7);
            Handeling.SpiritSpeak = info.IsSwitched(8);
            Handeling.Stealth = info.IsSwitched(9);

            from.SendGump(new DuelSetup_Rules(Handeling));
        }
    }
}