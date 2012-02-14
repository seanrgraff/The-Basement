/*
 Army System v1.0 Beta
 By: Shadow-Sigma
 
 If you have any questions or concerns, please leave me a private message on the RunUO forums (Username: Shadow-Sigma), or send me an e-mail at intranetworkster@gmail.com
 
 Enjoy!
 
 Please do not remove this comment from these scripts, thank you! :)
 */
using System;
using System.Collections;
using Server;
using Server.Gumps;
using Server.Items;
using Server.Mobiles;

namespace Server.Mobiles
{
    public class GuildTitleReset : Timer
    {
        private Mobile Player;
        private string GTReset;

        public GuildTitleReset(Mobile m, string GTS)
            : base(TimeSpan.FromDays(1))
        {
            Player = m;
            Priority = TimerPriority.OneSecond;
            GTReset = GTS;
        }

        protected override void OnTick()
        {
            if (Player == null || Player.Deleted)
            {
                Stop();
                return;
            }

            Player.GuildTitle = GTReset;
            Player.SendMessage("Your Guild Title has returned to normal!");
        }
    }

    public class RedTeamDyeTub : Item
    {
        public Timer m_Timer;

        [Constructable]
        public RedTeamDyeTub()
            : base(0xFAB)
        {
            Weight = 0.1;
            Name = "A Barrel of Red Dye - Used to Align Yourself to the Red Team";
            Hue = 140;
        }

        public RedTeamDyeTub(Serial serial)
            : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }

        public override void OnDoubleClick(Mobile from)
        {
            if (from.GuildTitle != "Blue Team" && from.GuildTitle != "Red Team")
            {
                string GTSave = from.GuildTitle;
                from.GuildTitle = "Red Team";
                from.SendMessage("You are now on the Red Team!");

                Timer m_timer = new GuildTitleReset(from, GTSave);
                m_timer.Start();
            }
            else if (from.GuildTitle == "Red Team")
            {
                from.SendMessage("You are already on the Red Team!");
            }
            else
            {
                from.SendMessage("You cannot change from the Blue Team to the Red Team!");
            }
        }
    }

    public class BlueTeamDyeTub : Item
    {
        [Constructable]
        public BlueTeamDyeTub()
            : base(0xFAB)
        {
            Weight = 0.1;
            Name = "A Barrel of Blue Dye - Used to Align Yourself to the Blue Team";
            Hue = 220;
        }

        public BlueTeamDyeTub(Serial serial)
            : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }

        public override void OnDoubleClick(Mobile from)
        {
            if (from.GuildTitle != "Red Team" && from.GuildTitle != "Blue Team")
            {
                string GTSave = from.GuildTitle;
                from.GuildTitle = "Blue Team";
                from.SendMessage("You are now on the Blue Team!");

                Timer m_timer = new GuildTitleReset(from, GTSave);
                m_timer.Start();
            }
            else if (from.GuildTitle == "Blue Team")
            {
                from.SendMessage("You are already on the Blue Team!");
            }
            else
            {
                from.SendMessage("You cannot change from the Red Team to the Blue Team!");
            }
        }
    }

    public class ToughGem : Item
    {
        [Constructable]
        public ToughGem()
            : base(0xF21)
        {
            Weight = 0.1;
            Name = "A Tough Gem - Used to spawn a Hammerman at the Army Spawner.";
            Hue = 1744;
        }

        public ToughGem(Serial serial)
            : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }

    public class SharpGem : Item
    {
        [Constructable]
        public SharpGem()
            : base(0xF21)
        {
            Weight = 0.1;
            Name = "A Sharp Gem - Used to spawn a Fencer at the Army Spawner.";
            Hue = 215;
        }

        public SharpGem(Serial serial)
            : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }

    public class CutGem : Item
    {
        [Constructable]
        public CutGem()
            : base(0xF21)
        {
            Weight = 0.1;
            Name = "A Cut Gem - Used to spawn a Swordsman at the Army Spawner.";
            Hue = 4;
        }

        public CutGem(Serial serial)
            : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }

    public class NoxGem : Item
    {
        [Constructable]
        public NoxGem()
            : base(0xF21)
        {
            Weight = 0.1;
            Name = "A Nox Gem - Used to spawn a Poison Mage at the Army Spawner.";
            Hue = 1269;
        }

        public NoxGem(Serial serial)
            : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }

    public class ElectricGem : Item
    {
        [Constructable]
        public ElectricGem()
            : base(0xF21)
        {
            Weight = 0.1;
            Name = "An Electric Gem - Used to spawn a Stunner at the Army Spawner.";
            Hue = 1196;
        }

        public ElectricGem(Serial serial)
            : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }

    public class FlawlessGem : Item
    {
        [Constructable]
        public FlawlessGem()
            : base(0xF21)
        {
            Weight = 0.1;
            Name = "A Flawless Gem - Used to spawn a Mage Lord at the Army Spawner.";
            Hue = 1100;
        }

        public FlawlessGem(Serial serial)
            : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }

    public class BlackGem : Item
    {
        [Constructable]
        public BlackGem()
            : base(0xF21)
        {
            Weight = 0.1;
            Name = "A Black Gem - Used to spawn an Assassin at the Army Spawner.";
            Hue = 952;
        }

        public BlackGem(Serial serial)
            : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }

    public class SwiftGem : Item
    {
        [Constructable]
        public SwiftGem()
            : base(0xF21)
        {
            Weight = 0.1;
            Name = "A Swift Gem - Used to spawn a Thief at the Army Spawner.";
            Hue = 290;
        }

        public SwiftGem(Serial serial)
            : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }

    public class ClearGem : Item
    {
        [Constructable]
        public ClearGem()
            : base(0xF21)
        {
            Weight = 0.1;
            Name = "A Clear Gem - Used to spawn a Healer at the Army Spawner.";
            Hue = 1072;
        }

        public ClearGem(Serial serial)
            : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }

    public class ArmySpawner : Item
    {
        [Constructable]
        public ArmySpawner()
            : base(0x1F1C)
        {
            Weight = 1.0;
            Name = "Army Spawner";
            Movable = false;
            Hue = 91;
        }

        public ArmySpawner(Serial serial)
            : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }

        public override void OnDoubleClick(Mobile from)
        {
            if (from.InRange(this, 2) && from.CanSee(this))
            {

                int Spawned = 0;
                int Team = 0;

                //Guage the amount of army members spawned.
                int RedSpawned = 0;
                int BlueSpawned = 0;

                //Changes the hues used to define the colors for Red and Blue teams.
                int RedTeamHue = 3;
                int BlueTeamHue = 5;

                //Use this to change the spawn locations of the army members.
                int XOffset = Utility.Random(4) - 2;
                int YOffset = Utility.Random(4) - 2;
                int ZOffset = 0;

                Mobile m;

                if (from.GuildTitle == "Red Team" || from.GuildTitle == "Blue Team")
                {
                    switch(from.GuildTitle)
                    {
                        case "Red Team":
                            {
                                Team = RedTeamHue;
                                break;
                            }
                        case "Blue Team":
                            {
                                Team = BlueTeamHue;
                                break;
                            }
                    }

                    Container pack = from.Backpack;
                    while (pack.FindItemByType(typeof(ToughGem)) != null)
                    {
                        m = new ArmyBaseMace(Team);
                        m.MoveToWorld(new Point3D(X + XOffset, Y + YOffset, Z + ZOffset), Map);
                        Item item = pack.FindItemByType(typeof(ToughGem));
                        item.Delete();
                        Spawned++;
                        switch (Team)
                        {
                            case 3:
                                {
                                    RedSpawned++;
                                    break;
                                }
                            case 5:
                                {
                                    BlueSpawned++;
                                    break;
                                }
                        }
                    }

                    while (pack.FindItemByType(typeof(SharpGem)) != null)
                    {
                        m = new ArmyBaseFence(Team);
                        m.MoveToWorld(new Point3D(X + XOffset, Y + YOffset, Z + ZOffset), Map);
                        Item item = pack.FindItemByType(typeof(SharpGem));
                        item.Delete();
                        Spawned++;
                        switch (Team)
                        {
                            case 3:
                                {
                                    RedSpawned++;
                                    break;
                                }
                            case 5:
                                {
                                    BlueSpawned++;
                                    break;
                                }
                        }
                    }

                    while (pack.FindItemByType(typeof(CutGem)) != null)
                    {
                        m = new ArmyBaseSword(Team);
                        m.MoveToWorld(new Point3D(X + XOffset, Y + YOffset, Z + ZOffset), Map);
                        Item item = pack.FindItemByType(typeof(CutGem));
                        item.Delete();
                        Spawned++;
                        switch (Team)
                        {
                            case 3:
                                {
                                    RedSpawned++;
                                    break;
                                }
                            case 5:
                                {
                                    BlueSpawned++;
                                    break;
                                }
                        }
                    }

                    while (pack.FindItemByType(typeof(NoxGem)) != null)
                    {
                        m = new ArmyBaseNox(Team);
                        m.MoveToWorld(new Point3D(X + XOffset, Y + YOffset, Z + ZOffset), Map);
                        Item item = pack.FindItemByType(typeof(NoxGem));
                        item.Delete();
                        Spawned++;
                        switch (Team)
                        {
                            case 3:
                                {
                                    RedSpawned++;
                                    break;
                                }
                            case 5:
                                {
                                    BlueSpawned++;
                                    break;
                                }
                        }
                    }

                    while (pack.FindItemByType(typeof(ElectricGem)) != null)
                    {
                        m = new ArmyBaseStun(Team);
                        m.MoveToWorld(new Point3D(X + XOffset, Y + YOffset, Z + ZOffset), Map);
                        Item item = pack.FindItemByType(typeof(ElectricGem));
                        item.Delete();
                        Spawned++;
                        switch (Team)
                        {
                            case 3:
                                {
                                    RedSpawned++;
                                    break;
                                }
                            case 5:
                                {
                                    BlueSpawned++;
                                    break;
                                }
                        }
                    }

                    while (pack.FindItemByType(typeof(FlawlessGem)) != null)
                    {
                        m = new ArmyBaseSuper(Team);
                        m.MoveToWorld(new Point3D(X + XOffset, Y + YOffset, Z + ZOffset), Map);
                        Item item = pack.FindItemByType(typeof(FlawlessGem));
                        item.Delete();
                        Spawned++;
                        switch (Team)
                        {
                            case 3:
                                {
                                    RedSpawned++;
                                    break;
                                }
                            case 5:
                                {
                                    BlueSpawned++;
                                    break;
                                }
                        }
                    }

                    while (pack.FindItemByType(typeof(BlackGem)) != null)
                    {
                        m = new ArmyBaseAssassin(Team);
                        m.MoveToWorld(new Point3D(X + XOffset, Y + YOffset, Z + ZOffset), Map);
                        Item item = pack.FindItemByType(typeof(BlackGem));
                        item.Delete();
                        Spawned++;
                        switch (Team)
                        {
                            case 3:
                                {
                                    RedSpawned++;
                                    break;
                                }
                            case 5:
                                {
                                    BlueSpawned++;
                                    break;
                                }
                        }
                    }

                    while (pack.FindItemByType(typeof(SwiftGem)) != null)
                    {
                        m = new ArmyBaseThief(Team);
                        m.MoveToWorld(new Point3D(X + XOffset, Y + YOffset, Z + ZOffset), Map);
                        Item item = pack.FindItemByType(typeof(SwiftGem));
                        item.Delete();
                        Spawned++;
                        switch (Team)
                        {
                            case 3:
                                {
                                    RedSpawned++;
                                    break;
                                }
                            case 5:
                                {
                                    BlueSpawned++;
                                    break;
                                }
                        }
                    }

                    while (pack.FindItemByType(typeof(ClearGem)) != null)
                    {
                        m = new ArmyBaseHealer(Team);
                        m.MoveToWorld(new Point3D(X + XOffset, Y + YOffset, Z + ZOffset), Map);
                        Item item = pack.FindItemByType(typeof(ClearGem));
                        item.Delete();
                        Spawned++;
                        switch (Team)
                        {
                            case 3:
                                {
                                    RedSpawned++;
                                    break;
                                }
                            case 5:
                                {
                                    BlueSpawned++;
                                    break;
                                }
                        }
                    }

                    if (Spawned == 0)
                    {
                        from.SendMessage("You do not have any spawn items!");
                    }

                    if (RedSpawned > 0)
                    {
                        string SpawnMessage = string.Concat(RedSpawned, " Red Army members spawned.");
                        from.SendMessage(SpawnMessage);
                    }

                    if (BlueSpawned > 0)
                    {
                        string SpawnMessage = string.Concat(BlueSpawned, " Blue Army members spawned.");
                        from.SendMessage(SpawnMessage);
                    }
                }

                else
                {
                    from.SendMessage("You must be a member of the red or blue team to build an army!");
                }
            }

            else
            {
                from.SendLocalizedMessage(500446); // That is too far away. 
            }
        }
    }
}