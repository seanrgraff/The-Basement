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
    public class FieldSetup_Rules_Armor : Gump
    {
        public Field Handeling;

        public FieldSetup_Rules_Armor(Field d)
            : base(0, 0)
        {
            Handeling = d;

            Closable = true;
            Dragable = true;
            Resizable = false;

            AddBackground(219, 238, 172, 129, 9300);
            AddLabel(263, 244, 70, @"Armor Rules");

            AddCheck(225, 265, 210, 211, Handeling.Armor, 1);
            AddLabel(255, 265, 54, @"Armor Allowed");
            AddCheck(225, 290, 210, 211, Handeling.MagicalArmor, 2);
            AddLabel(255, 290, 54, @"Magic Armor");
            AddCheck(225, 315, 210, 211, Handeling.Shields, 3);
            AddLabel(255, 315, 54, @"Shields");
            AddCheck(225, 340, 210, 211, Handeling.Colored, 4);
            AddLabel(255, 340, 54, @"Colored Armor");
        }

        public override void OnResponse(NetState sender, RelayInfo info)
        {
            Mobile from = sender.Mobile;

            Handeling.Armor = info.IsSwitched(1);
            Handeling.MagicalArmor = info.IsSwitched(2);
            Handeling.Shields = info.IsSwitched(3);
            Handeling.Colored = info.IsSwitched(4);

            from.SendGump(new FieldSetup_Rules(Handeling));
        }
    }
}
