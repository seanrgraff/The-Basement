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
    public class FieldSetup_Rules_Weapons : Gump
    {
        public Field Handeling;

        public FieldSetup_Rules_Weapons(Field d)
            : base(0, 0)
        {
            Handeling = d;

            Closable = true;
            Dragable = true;
            Resizable = false;

            AddBackground(219, 238, 172, 129, 9300);
            AddLabel(263, 244, 70, @"Weapon Rules");

            AddCheck(225, 265, 210, 211, Handeling.Weapons, 1);
            AddLabel(255, 265, 54, @"Weapons Allowed");
            AddCheck(225, 290, 210, 211, Handeling.Magical, 2);
            AddLabel(255, 290, 54, @"Magic Weapons");
            AddCheck(225, 315, 210, 211, Handeling.Poisoned, 3);
            AddLabel(255, 315, 54, @"Poisoned Weapons");
            AddCheck(225, 340, 210, 211, Handeling.RunicWeapons, 4);
            AddLabel(255, 340, 54, @"Runic Weapons");
        }

        public override void OnResponse(NetState sender, RelayInfo info)
        {
            Mobile from = sender.Mobile;

            Handeling.Weapons = info.IsSwitched(1);
            Handeling.Magical = info.IsSwitched(2);
            Handeling.Poisoned = info.IsSwitched(3);
            Handeling.RunicWeapons = info.IsSwitched(4);

            from.SendGump(new FieldSetup_Rules(Handeling));
        }
    }
}
