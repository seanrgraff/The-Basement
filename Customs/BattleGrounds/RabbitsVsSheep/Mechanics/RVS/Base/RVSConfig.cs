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
using Server.Fielding;

namespace Server.RabbitsVsSheep
{
    public class RVS_Config
    {
        // Suggested User Configured Variables
        public static bool Enabled = true;
        public static bool AllowSameIPRVSs = true;
        public static int RVSStartDelay = 3;
        public static bool AllowFactionersToRVS = false;
        //End of Suggested User Configured Variables

        public static List<RVS> Registry;
        public static List<RVSController> Controllers;
        public static Dictionary<PlayerMobile, int> RVSScores;

        public static void Initialize()
        {
            Registry = new List<RVS>();
            UpdateControllers();
            InitializeRegions();
            RVSCleanUp_DelayLoop();

            CommandSystem.Register("duelregions", AccessLevel.GameMaster, new CommandEventHandler(On_RVSRegions));
            EventSink.Speech += new SpeechEventHandler(On_Speech);
            EventSink.Logout += new LogoutEventHandler(On_Logout);
            EventSink.Shutdown += new ShutdownEventHandler(On_ShutDown);
        }

        public static void On_RVSRegions(CommandEventArgs args)
        {
            InitializeRegions();
        }

        public static void InitializeRegions()
        {
            for (int i = 0; i < Controllers.Count; ++i)
            {
                RVSController controller = (RVSController)Controllers[i];

                if (controller.SheepRegion != null)
                    controller.SheepRegion.Unregister();

                controller.SheepRegion = new RVSRegion(controller, controller.sheepRegionMap, controller.sheepRegionPoint, "Sheep");
                
                if (controller.RabbitRegion != null)
                    controller.RabbitRegion.Unregister();

                controller.RabbitRegion = new RVSRegion(controller, controller.rabbitRegionMap, controller.rabbitRegionPoint, "Rabbit");
            }
            Console.WriteLine("Rabbit VS Sheep regions have been initialized.");
        }

        public static void On_ShutDown(ShutdownEventArgs args)
        {
            for (int i = 0; i < Registry.Count; ++i)
            {
                RVS d = (RVS)Registry[i];
                d.EndSelf();
            }
        }

        public static void RVSCleanUp_DelayLoop()
        {
            for (int i = 0; i < Registry.Count; ++i)
            {
                RVS d = (RVS)Registry[i];

                if (d.Caller.NetState == null || d.Ended)
                {
                    Registry.Remove(d);
                    d.EndSelf();
                }
            }

            Timer.DelayCall(TimeSpan.FromSeconds(1.0), new TimerCallback(RVSCleanUp_DelayLoop));
        }

        public static void UpdateControllers()
        {
            Controllers = new List<RVSController>();

            foreach (Item item in World.Items.Values)
            {
                if (item is RVSController)
                    Controllers.Add((RVSController)item);
            }
        }

        public static void On_Logout(LogoutEventArgs args)
        {
            PlayerMobile pm = (PlayerMobile)args.Mobile;

            for (int i = 0; i < Registry.Count; ++i)
            {
                RVS d = (RVS)Registry[i];

                if (d.Caller == pm)
                {
                    Registry.Remove(d);
                    d.EndSelf();
                }
                else if (d.InRVS(pm))
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
            
            if (args.Speech.ToLower().Contains("i wish to rvs"))
            {
                if (m is PlayerMobile)
                {
                    if (Duel_Config.InADuel((PlayerMobile)m) || Field_Config.InAField((PlayerMobile)m) || InARVS((PlayerMobile)m))
                    {
                        m.SendMessage("You are already in a RVS duel.");
                        return;
                    }

                    if (!RVS_Config.AllowFactionersToRVS)
                    {
                        if ((Faction.Find(m) != null))
                        {
                            m.SendMessage("Factioners cannot RVS duel.");
                            return;
                        }
                    }

                    if (m.Combatant != null || m.Aggressed.Count >= 1)
                    {
                        m.SendMessage("You are in combat an cannot RVS duel.");
                        return;
                    }

                    RVS d = new RVS((PlayerMobile)m);
                    Registry.Add(d);
                    m.CloseGump(typeof(RVSSetup_Main));
                    m.SendGump(new RVSSetup_Main(d));
                }
            }

            if (args.Speech.ToLower().Contains("i yield"))
            {
                if (m is PlayerMobile)
                {
                    if (InARVS((PlayerMobile)m))
                    {
						m.Kill();
                        return;
                    }
                }
            }
        }

        public static bool InARVS(PlayerMobile m)
        {
            for (int i = 0; i < Registry.Count; ++i)
            {
                RVS d = (RVS)Registry[i];

                if (d.InRVS(m) || d.Caller == m)
                    return true;
            }

            return false;
        }

        public static bool IsAlly(PlayerMobile from, PlayerMobile target)
        {
            for (int i = 0; i < Registry.Count; ++i)
            {
                RVS d = (RVS)Registry[i];

                IEnumerator key = d.Teams.Keys.GetEnumerator();
                for (int i2 = 0; i2 < d.Teams.Count; ++i2)
                {
                    key.MoveNext();
                    RVS_Team d_team = (RVS_Team)d.Teams[(int)key.Current];

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
                RVS d = (RVS)Registry[i];

                IEnumerator key = d.Teams.Keys.GetEnumerator();
                for (int i2 = 0; i2 < d.Teams.Count; ++i2)
                {
                    key.MoveNext();
                    RVS_Team d_team = (RVS_Team)d.Teams[(int)key.Current];

                    if (d_team.Players.Contains(from) && !d_team.Players.Contains(target) || !d_team.Players.Contains(from) && d_team.Players.Contains(target))
                        return true;
                }
            }

            return false;
        }

        public static bool RVSStarted(PlayerMobile pm)
        {
            for (int i = 0; i < Registry.Count; ++i)
            {
                RVS d = (RVS)Registry[i];

                IEnumerator key = d.Teams.Keys.GetEnumerator();
                for (int i2 = 0; i2 < d.Teams.Count; ++i2)
                {
                    key.MoveNext();
                    RVS_Team d_team = (RVS_Team)d.Teams[(int)key.Current];

                    if (d_team.Players.Contains(pm) && d.InProgress)
                        return true;
                }
            }

            return false;
        }
    }
}
