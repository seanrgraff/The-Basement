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
using Server.Fielding;
using Server.RabbitsVsSheep;

namespace Server.Dueling
{
    public class Duel_Config
    {
        // Suggested User Configured Variables
        public static bool Enabled = true;
        public static bool AllowSameIPDuels = true;
        public static int DuelStartDelay = 3;
        public static bool AllowFactionersToDuel = false;
        //End of Suggested User Configured Variables

        public static List<Duel> Registry;
        public static List<DuelController> Controllers;
        public static Dictionary<PlayerMobile, int> DuelScores;

        public static void Initialize()
        {
            Registry = new List<Duel>();
            UpdateControllers();
            InitializeRegions();
            DuelCleanUp_DelayLoop();

            CommandSystem.Register("duelregions", AccessLevel.GameMaster, new CommandEventHandler(On_DuelRegions));
            EventSink.Speech += new SpeechEventHandler(On_Speech);
            EventSink.Logout += new LogoutEventHandler(On_Logout);
            EventSink.Shutdown += new ShutdownEventHandler(On_ShutDown);
        }

        public static void On_DuelRegions(CommandEventArgs args)
        {
            InitializeRegions();
        }

        public static void InitializeRegions()
        {
            for (int i = 0; i < Controllers.Count; ++i)
            {
                DuelController controller = (DuelController)Controllers[i];

                if (controller.ThisRegion != null)
                    controller.ThisRegion.Unregister();

                controller.ThisRegion = new DuelRegion(controller);
            }
            Console.WriteLine("Duel regions have been initialized.");
        }

        public static void On_ShutDown(ShutdownEventArgs args)
        {
            for (int i = 0; i < Registry.Count; ++i)
            {
                Duel d = (Duel)Registry[i];
                d.EndSelf();
            }
        }

        public static void DuelCleanUp_DelayLoop()
        {
            for (int i = 0; i < Registry.Count; ++i)
            {
                Duel d = (Duel)Registry[i];

                if (d.Caller.NetState == null || d.Ended)
                {
                    Registry.Remove(d);
                    d.EndSelf();
                }
            }

            Timer.DelayCall(TimeSpan.FromSeconds(1.0), new TimerCallback(DuelCleanUp_DelayLoop));
        }

        public static void UpdateControllers()
        {
            Controllers = new List<DuelController>();

            foreach (Item item in World.Items.Values)
            {
                if (item is DuelController)
                    Controllers.Add((DuelController)item);
            }
        }

        public static void On_Logout(LogoutEventArgs args)
        {
            PlayerMobile pm = (PlayerMobile)args.Mobile;

            for (int i = 0; i < Registry.Count; ++i)
            {
                Duel d = (Duel)Registry[i];

                if (d.Caller == pm)
                {
                    Registry.Remove(d);
                    d.EndSelf();
                }
                else if (d.InDuel(pm))
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
            
            if (args.Speech.ToLower().Contains("i wish to duel"))
            {
                if (m is PlayerMobile)
                {
                    if (InADuel((PlayerMobile)m) || RVS_Config.InARVS((PlayerMobile)m) || Field_Config.InAField((PlayerMobile)m) )
                    {
                        m.SendMessage("You are already in a duel.");
                        return;
                    }

                    if (!Duel_Config.AllowFactionersToDuel)
                    {
                        if ((Faction.Find(m) != null))
                        {
                            m.SendMessage("Factioners cannot duel.");
                            return;
                        }
                    }

                    if (m.Combatant != null || m.Aggressed.Count >= 1)
                    {
                        m.SendMessage("You are in combat an cannot duel.");
                        return;
                    }

                    Duel d = new Duel((PlayerMobile)m);
                    Registry.Add(d);
                    m.CloseGump(typeof(DuelSetup_Main));
                    m.SendGump(new DuelSetup_Main(d));
                }
            }

            if (args.Speech.ToLower().Contains("i yield"))
            {
                if (m is PlayerMobile)
                {
                    if (InADuel((PlayerMobile)m))
                    {
			m.Kill();
                        return;
                    }
                }
            }
        }

        public static bool InADuel(PlayerMobile m)
        {
            for (int i = 0; i < Registry.Count; ++i)
            {
                Duel d = (Duel)Registry[i];

                if (d.InDuel(m) || d.Caller == m)
                    return true;
            }

            return false;
        }

        public static bool IsAlly(PlayerMobile from, PlayerMobile target)
        {
            for (int i = 0; i < Registry.Count; ++i)
            {
                Duel d = (Duel)Registry[i];

                IEnumerator key = d.Teams.Keys.GetEnumerator();
                for (int i2 = 0; i2 < d.Teams.Count; ++i2)
                {
                    key.MoveNext();
                    Duel_Team d_team = (Duel_Team)d.Teams[(int)key.Current];

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
                Duel d = (Duel)Registry[i];

                IEnumerator key = d.Teams.Keys.GetEnumerator();
                for (int i2 = 0; i2 < d.Teams.Count; ++i2)
                {
                    key.MoveNext();
                    Duel_Team d_team = (Duel_Team)d.Teams[(int)key.Current];

                    if (d_team.Players.Contains(from) && !d_team.Players.Contains(target) || !d_team.Players.Contains(from) && d_team.Players.Contains(target))
                        return true;
                }
            }

            return false;
        }

        public static bool DuelStarted(PlayerMobile pm)
        {
            for (int i = 0; i < Registry.Count; ++i)
            {
                Duel d = (Duel)Registry[i];

                IEnumerator key = d.Teams.Keys.GetEnumerator();
                for (int i2 = 0; i2 < d.Teams.Count; ++i2)
                {
                    key.MoveNext();
                    Duel_Team d_team = (Duel_Team)d.Teams[(int)key.Current];

                    if (d_team.Players.Contains(pm) && d.InProgress)
                        return true;
                }
            }

            return false;
        }
    }
}
