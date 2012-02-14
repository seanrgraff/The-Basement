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
    public class FieldSetup_Rules_Combat : Gump
    {
        public Field Handeling;

        public FieldSetup_Rules_Combat(Field d)
            : base(0, 0)
        {
            Handeling = d;

            Closable = true;
            Dragable = true;
            Resizable = false;

            AddBackground(219, 238, 172, 179, 9300);
            AddLabel(245, 244, 70, @"Combat Ability Rules");

            AddCheck(225, 265, 210, 211, Handeling.CombatAbilities, 1);
            AddLabel(255, 265, 54, @"Combat Abilities");
            AddCheck(225, 290, 210, 211, Handeling.Stun, 2);
            AddLabel(255, 290, 54, @"Stun");
            AddCheck(225, 315, 210, 211, Handeling.Disarm, 3);
            AddLabel(255, 315, 54, @"Disarm");
            AddCheck(225, 340, 210, 211, Handeling.ConcussionBlow, 4);
            AddLabel(255, 340, 54, @"Concussion Blow");
            AddCheck(225, 365, 210, 211, Handeling.CrushingBlow, 5);
            AddLabel(255, 365, 54, @"Crushing Blow");
            AddCheck(225, 390, 210, 211, Handeling.ParalyzingBlow, 6);
            AddLabel(255, 390, 54, @"Paralyzing Blow");
        }

        public override void OnResponse(NetState sender, RelayInfo info)
        {
            Mobile from = sender.Mobile;

            Handeling.CombatAbilities = info.IsSwitched(1);
            Handeling.Stun = info.IsSwitched(2);
            Handeling.Disarm = info.IsSwitched(3);
            Handeling.ConcussionBlow = info.IsSwitched(4);
            Handeling.CrushingBlow = info.IsSwitched(5);
            Handeling.ParalyzingBlow = info.IsSwitched(6);

            from.SendGump(new FieldSetup_Rules(Handeling));
        }
    }
}
