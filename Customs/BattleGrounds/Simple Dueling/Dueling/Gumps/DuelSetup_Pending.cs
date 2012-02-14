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

namespace Server.Dueling
{
	public class DuelSetup_Pending : Gump
	{
        public Duel Handeling;

		public DuelSetup_Pending(Duel d) : base(0, 0)
		{
            Handeling = d;

			Closable = false;
			Dragable = true;
			Resizable = false;

            int bh = (PlayerCount() * 38);
			AddBackground( 212, 177, 251, bh, 9250 );
			AddBackground( 219, 185, 235, (bh - 15), 9350 );

            int y = 190;
            IEnumerator key = Handeling.Teams.Keys.GetEnumerator();
            for (int i = 0; i < Handeling.Teams.Count; ++i)
            {
                key.MoveNext();
                Duel_Team d_team = (Duel_Team)Handeling.Teams[(int)key.Current];

                for (int i2 = 0; i2 < d_team.Players.Count; ++i2)
                {
                    object o = (object)d_team.Players[i2];
                    if (o is PlayerMobile)
                    {
                        PlayerMobile pm = (PlayerMobile)o;
                        AddLabel(250, y, 0, String.Format("{0}", pm.Name));
                        AddImage(225, y, ImageID((bool)d_team.Accepted[pm]));
                        y += 30;
                    }
                }
            }
		}

        public int ImageID(bool b)
        {
            if (b)
                return 211;
            else
                return 210;
        }

        public int PlayerCount()
        {
            int count = 0;
            IEnumerator key = Handeling.Teams.Keys.GetEnumerator();
            for (int i = 0; i < Handeling.Teams.Count; ++i)
            {
                key.MoveNext();
                Duel_Team d_team = (Duel_Team)Handeling.Teams[(int)key.Current];

                for (int i2 = 0; i2 < d_team.Players.Count; ++i2)
                {
                    if (d_team.Players[i2] != "@null")
                        count += 1;
                }
            }

            return count;
        }

        public override void OnResponse(NetState sender, RelayInfo info)
		{
			Mobile from = sender.Mobile;
		}
	}
}
