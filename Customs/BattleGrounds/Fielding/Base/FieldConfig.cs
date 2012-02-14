using System;
using Server;
using Server.Gumps;
using Server.Items;
using Server.Network;
using Server.Commands;
using Server.Mobiles;
using System.Collections;
using System.Collections.Generic;
using Server.Factions;
using Server.Dueling;
using Server.RabbitsVsSheep;

namespace Server.Fielding
{
    public class Field_Config
    {
        // Suggested User Configured Variables
        public static bool Enabled = true;
        public static bool AllowSameIPFields = true;
        public static int FieldStartDelay = 3;
        public static bool AllowFactionersToField = false;
        //End of Suggested User Configured Variables

        public static List<Field> Registry;
        public static List<FieldController> Controllers;
        public static Dictionary<PlayerMobile, int> FieldScores;

        public static void Initialize()
        {
            Registry = new List<Field>();
            UpdateControllers();
            InitializeRegions();
            FieldCleanUp_DelayLoop();

            CommandSystem.Register("duelregions", AccessLevel.GameMaster, new CommandEventHandler(On_FieldRegions));
            EventSink.Speech += new SpeechEventHandler(On_Speech);
            EventSink.Logout += new LogoutEventHandler(On_Logout);
            EventSink.Shutdown += new ShutdownEventHandler(On_ShutDown);
        }

        public static void On_FieldRegions(CommandEventArgs args)
        {
            InitializeRegions();
        }

        public static void InitializeRegions()
        {
            for (int i = 0; i < Controllers.Count; ++i)
            {
                FieldController controller = (FieldController)Controllers[i];

                if (controller.ThisRegion != null)
                    controller.ThisRegion.Unregister();

                controller.ThisRegion = new FieldRegion(controller);
            }
            Console.WriteLine("Field regions have been initialized.");
        }

        public static void On_ShutDown(ShutdownEventArgs args)
        {
            for (int i = 0; i < Registry.Count; ++i)
            {
                Field d = (Field)Registry[i];
                d.EndSelf();
            }
        }

        public static void FieldCleanUp_DelayLoop()
        {
            for (int i = 0; i < Registry.Count; ++i)
            {
                Field d = (Field)Registry[i];

                if (d.Caller.NetState == null || d.Ended)
                {
                    Registry.Remove(d);
                    d.EndSelf();
                }
            }

            Timer.DelayCall(TimeSpan.FromSeconds(1.0), new TimerCallback(FieldCleanUp_DelayLoop));
        }

        public static void UpdateControllers()
        {
            Controllers = new List<FieldController>();

            foreach (Item item in World.Items.Values)
            {
                if (item is FieldController)
                    Controllers.Add((FieldController)item);
            }
        }

        public static void On_Logout(LogoutEventArgs args)
        {
            PlayerMobile pm = (PlayerMobile)args.Mobile;

            for (int i = 0; i < Registry.Count; ++i)
            {
                Field d = (Field)Registry[i];

                if (d.Caller == pm)
                {
                    Registry.Remove(d);
                    d.EndSelf();
                }
                else if (d.InField(pm))
                {
                    if (!d.InProgress)
                    {
                        d.RefundBuyIn(pm);
                        d.SendControllerSetup();
                    }
                    else
                    {
                        d.EchoMessage("A player disconnected, duel must end.");
                        d.EndSelf();
                    }
                }
            }
        }

        public static void On_Speech(SpeechEventArgs args)
        {
            Mobile m = (Mobile)args.Mobile;

            if (!Enabled)
                return;
            
            if (args.Speech.ToLower().Contains("i wish to field"))
            {
                if (m is PlayerMobile)
                {
                    if (Duel_Config.InADuel((PlayerMobile)m) || RVS_Config.InARVS((PlayerMobile)m) || InAField((PlayerMobile)m))
                    {
                        m.SendMessage("You are already in a field duel.");
                        return;
                    }

                    if (!Field_Config.AllowFactionersToField)
                    {
                        if ((Faction.Find(m) != null))
                        {
                            m.SendMessage("Factioners cannot field duel.");
                            return;
                        }
                    }

                    if (m.Combatant != null || m.Aggressed.Count >= 1)
                    {
                        m.SendMessage("You are in combat an cannot field duel.");
                        return;
                    }

                    Field d = new Field((PlayerMobile)m);
                    Registry.Add(d);
                    m.CloseGump(typeof(FieldSetup_Main));
                    m.SendGump(new FieldSetup_Main(d));
                }
            }

            if (args.Speech.ToLower().Contains("i yield"))
            {
                if (m is PlayerMobile)
                {
                    if (InAField((PlayerMobile)m))
                    {
						m.Kill();
                        return;
                    }
                }
            }
        }

        public static bool InAField(PlayerMobile m)
        {
            for (int i = 0; i < Registry.Count; ++i)
            {
                Field d = (Field)Registry[i];

                if (d.InField(m) || d.Caller == m)
                    return true;
            }

            return false;
        }

        public static bool IsAlly(PlayerMobile from, PlayerMobile target)
        {
            for (int i = 0; i < Registry.Count; ++i)
            {
                Field d = (Field)Registry[i];

                IEnumerator key = d.Teams.Keys.GetEnumerator();
                for (int i2 = 0; i2 < d.Teams.Count; ++i2)
                {
                    key.MoveNext();
                    Field_Team d_team = (Field_Team)d.Teams[(int)key.Current];

                    if (d_team.Players.Contains(from) && d_team.Players.Contains(target))
                        return true;
                }
            }

            return false;
        }

        public static bool IsEnemy(PlayerMobile from, PlayerMobile target)
        {
            for (int i = 0; i < Registry.Count; ++i)
            {
                Field d = (Field)Registry[i];

                IEnumerator key = d.Teams.Keys.GetEnumerator();
                for (int i2 = 0; i2 < d.Teams.Count; ++i2)
                {
                    key.MoveNext();
                    Field_Team d_team = (Field_Team)d.Teams[(int)key.Current];

                    if (d_team.Players.Contains(from) && !d_team.Players.Contains(target) || !d_team.Players.Contains(from) && d_team.Players.Contains(target))
                        return true;
                }
            }

            return false;
        }

        public static bool FieldStarted(PlayerMobile pm)
        {
            for (int i = 0; i < Registry.Count; ++i)
            {
                Field d = (Field)Registry[i];

                IEnumerator key = d.Teams.Keys.GetEnumerator();
                for (int i2 = 0; i2 < d.Teams.Count; ++i2)
                {
                    key.MoveNext();
                    Field_Team d_team = (Field_Team)d.Teams[(int)key.Current];

                    if (d_team.Players.Contains(pm) && d.InProgress)
                        return true;
                }
            }

            return false;
        }
    }
}
