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
    public class RVSSetup_Rules_Monsters : Gump
    {
        public RVS Handeling;

        public RVSSetup_Rules_Monsters(RVS d)
            : base(0, 0)
        {
            Handeling = d;

            Closable = true;
            Dragable = true;
            Resizable = false;

            //AddBackground(219, 238, 172, 154, 9300);
            AddBackground(219, 238, 172, 178, 9300);
            AddLabel(255, 244, 70, @"Monster Type Rules");

            AddRadio(225, 265, 210, 211, Handeling.RvS, 1);
            AddLabel(255, 265, 54, @"Rabbits Vs Sheep");
            AddRadio(225, 290, 210, 211, Handeling.Orcs, 2);
            AddLabel(255, 290, 54, @"Orcs");
            AddRadio(225, 315, 210, 211, Handeling.Lizardmen, 3);
            AddLabel(255, 315, 54, @"Lizardmen");
            AddRadio(225, 340, 210, 211, Handeling.Ratmen, 4);
            AddLabel(255, 340, 54, @"Ratmen");
            AddRadio(225, 365, 210, 211, Handeling.Undead, 5);
            AddLabel(255, 365, 54, @"Undead");
        }

        public override void OnResponse(NetState sender, RelayInfo info)
        {
            Mobile from = sender.Mobile;
			
            Handeling.RvS = info.IsSwitched(1);
            Handeling.Orcs = info.IsSwitched(2);
            Handeling.Lizardmen = info.IsSwitched(3);
            Handeling.Ratmen = info.IsSwitched(4);
            Handeling.Undead = info.IsSwitched(5);

            from.SendGump(new RVSSetup_Rules(Handeling));
        }
    }
}