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
    public class FieldSetup_Rules_Items : Gump
    {
        public Field Handeling;

        public FieldSetup_Rules_Items(Field d)
            : base(0, 0)
        {
            Handeling = d;

            Closable = true;
            Dragable = true;
            Resizable = false;

            //AddBackground(219, 238, 172, 154, 9300);
            AddBackground(219, 238, 172, 178, 9300);
            AddLabel(255, 244, 70, @"Item Use Rules");

            AddCheck(225, 265, 210, 211, Handeling.Potions, 1);
            AddLabel(255, 265, 54, @"Potions");
            AddCheck(225, 290, 210, 211, Handeling.Bandages, 2);
            AddLabel(255, 290, 54, @"Bandages");
            AddCheck(225, 315, 210, 211, Handeling.TrappedContainers, 3);
            AddLabel(255, 315, 54, @"Trapped Containers");
            AddCheck(225, 340, 210, 211, Handeling.Bolas, 4);
            AddLabel(255, 340, 54, @"Bolas");
            AddCheck(225, 365, 210, 211, Handeling.Mounts, 5);
            AddLabel(255, 365, 54, @"Mounts");
            AddCheck(225, 390, 210, 211, Handeling.Wands, 6);
            AddLabel(255, 390, 54, @"Wands");
        }

        public override void OnResponse(NetState sender, RelayInfo info)
        {
            Mobile from = sender.Mobile;

            Handeling.Potions = info.IsSwitched(1);
            Handeling.Bandages = info.IsSwitched(2);
            Handeling.TrappedContainers = info.IsSwitched(3);
            Handeling.Bolas = info.IsSwitched(4);
            Handeling.Mounts = info.IsSwitched(5);
            Handeling.Wands = info.IsSwitched(6);

            from.SendGump(new FieldSetup_Rules(Handeling));
        }
    }
}
