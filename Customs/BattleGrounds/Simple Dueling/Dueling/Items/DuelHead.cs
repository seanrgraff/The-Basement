using System;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Dueling
{
    public class DuelHead : Item
    {
        public PlayerMobile m_Taken;
        public PlayerMobile m_From;

        [Constructable]
        public DuelHead(PlayerMobile taken, PlayerMobile from)
            : base(0x1DA0)
        {
            m_Taken = taken;
            m_From = from;
            Name = String.Format("the head of {0}, taken in a duel by {1}", taken.Name, from.Name);
            Visible = true;
            Movable = true;
            Weight = 3;
        }

        public DuelHead(Serial serial)
            : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
            writer.Write((PlayerMobile)m_Taken);
            writer.Write((PlayerMobile)m_From);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
            m_Taken = (PlayerMobile)reader.ReadMobile();
            m_From = (PlayerMobile)reader.ReadMobile();
        }
    }
}