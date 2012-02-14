// Include namespaces
using System;
using Server;
using Server.Gumps;
using Server.Items;
using Server.Network;
using Server.Commands;
using Server.Mobiles;
using System.Collections;
using System.Collections.Generic;

namespace Server.RabbitsVsSheep // Declare the classes to be in the Server.Dueling namespace
{
    public class RVS // Declare the public class duel
    {
        public Mobile Caller; // Declare the mobile which called the duel

        public bool InProgress; // declare the boolean to be toggled whilst the duel may/may not be in progress
        public RVSController Controller;

        public RVS_StartTimer StartTimer;

        public bool IsRematch;
        public bool Ended;

		//determines if the RVS has trees in it or not
		public bool AllowTrees = false;
		public int NumberOfTrees = 10;
		
		//determines if the RVS has traps in it or not
		public bool AllowTraps = false;
		public int NumberOfTraps = 10;

		//determines the amount of sheep required to win
		public int MonstersOverwhelm = 50;

        public int BuyIn = 0;

        public RVS_Add_Timer AddTimer; // Declares AddTimer, so that the AddTimer cannot have multiple instances per duel

        public bool Spells = true; // Declares all boolean's for allowed/disallowed spells
        public bool ReactiveArmor = true, Clumsy = true, CreateFood = true, Feeblemind = true, Heal = true, MagicArrow = true, NightSight = true, Weaken = true;
        public bool Agility = true, Cunning = true, Cure = true, Harm = true, MagicTrap = true, Untrap = true, Protection = true, Strength = true;
        public bool Bless = true, Fireball = true, MagicLock = true, Poison = true, Telekinisis = true, Teleport = false, Unlock = true, WallOfStone = true;
        public bool ArchCure = true, ArchProtection = true, Curse = true, FireField = true, GreaterHeal = true, Lightning = true, ManaDrain = true, Recall = false;
        public bool BladeSpirits = true, DispelField = true, Incognito = true, MagicReflection = true, MindBlast = true, Paralyze = true, PoisonField = true, SummonCreature = true;
        public bool Dispel = true, EnergyBolt = true, Explosion = true, Invisibility = true, Mark = false, MassCurse = true, ParalyzeField = false, Reveal = true;
        public bool ChainLightning = true, EnergyField = true, FlameStrike = true, GateTravel = false, ManaVampire = true, MassDispel = true, MeteorSwarm = true, Polymorph = true;
        public bool EarthQuake = true, EnergyVotex = true, Resurrection = true, SummonAirElemental = true, SummonDaemon = true, SummonEarthElemental = true, SummonFireElemental = true, SummonWaterElemental = true;
        // Lesser Magical Skills
        public bool AllowSamuraiSpells = false;
        public bool Confidence = true, CounterAttack = true, Evasion = false, HonorableExecution = false, LightningStrike = true, MomentumStrike = true;
        public bool AllowChivalry = false;
        public bool ClenseByFire = false, CloseWounds = true, ConsecrateWeapon = true, DispellEvil = true, DivineFury = false, EnemyOfOne = false, HolyLight = false, NobleSacrafice = false, RemoveCurse = true, SacredJourny = false;
        public bool AllowNecromancy = false;
        public bool AnimateDead = false, BloodOath = false, CorpseSkin = true, CurseWeapon = true, EvilOmen = true, Exorcisim = false, HorrificBeast = true, LichForm = false, MindRot = true, PainSpike = true, PoisonStrike = true, Strangle = false, SummonFamiliar = false, VampiricEmbrace = false, VengefulSpirit = false, Wither = false, WraithForm = false;
        public bool AllowNinjaSpells = false;
        public bool AnimalForm = false, Backstab = false, DeathStrike = false, FocusAttack = true, KiAttack = true, MirrorImage = false, ShadowJump = false, SurpriseAttack = false;

        public bool CombatAbilities = false; // Declares the bool which allows/disallows combat abilities
        public bool Stun = false, Disarm = false, ConcussionBlow = false, CrushingBlow = false, ParalyzingBlow = false;
        // Declares the bools which allow/disallow useable skills
        public bool Anatomy = true, DetectHidden = true, EvaluatingIntelligence = true, Hiding = true, Poisoning = true, Snooping = true, Stealing = true, SpiritSpeak = true, Stealth = true;

        public bool Weapons = true; // allows/disallows weapons/weapon types & attributes
        public bool Magical = true, Poisoned = true, RunicWeapons = true;

        public bool Armor = true; // Allows/disallows armor/armor types & attributes
        public bool MagicalArmor = true, Shields = true, Colored = true;

        public bool Potions = true; // allows/disallows the use of items such as potions
        public bool Bandages = true, TrappedContainers = true, Bolas = true, Mounts = true, Wands = true;
		
		// Allows different monsters in Rabbits Vs Sheep
		public bool RvS = true, Orcs = false, Lizardmen = false, Ratmen = false, Undead = false;

        public Dictionary<int, RVS_Team> Teams; // Declares the dictionary to keep track of all the teams involved in the duel

        public RVS(PlayerMobile starter) // The constructor for RVS, initializes required variables
        {
            Caller = starter; // sets the caller
            Teams = new Dictionary<int, RVS_Team>(); // initializes the Teams dictionary so it does not = null
            Teams.Add(1, new RVS_Team(1)); // creates & adds two default teams 1 & 2 to the Teams dictionary
            Teams.Add(2, new RVS_Team(2));
            RVS_Team TeamOne = (RVS_Team)Teams[1]; // creates a variable reference to the default team 1
            TeamOne.AddPlayer(starter); // adds the callers as the first player to the default team 1
            RVS_Team TeamTwo= (RVS_Team)Teams[2]; // creates a variable reference to the defualt team 2
            TeamTwo.Players.Add("@null"); // installs one empty slot into the default team 2

            InProgress = false;
            IsRematch = false;
            Ended = false;
        }

        public void UpdateAllPending()
        {
            // This sub is used to send all players the required gumps to view the rules of the duel and
            // eventually accept/decline the duel, after a player has not already accepted the duel they
            // are sent the gump to accept/decline the duel, if they have accepted the duel, they are 
            // sent the gump to view all the players that have/have not accepted the duel
            // if all players have accepted the duel this sub will then begin the process of starting the
            // duel

            bool start = false; // bool to determine weather to start the duel or not
            IEnumerator key = Teams.Keys.GetEnumerator(); // creates the key variable so we can iterate through the Teams dictionary
            for (int i = 0; i < Teams.Count; ++i) // Iterates through the Teams dictionary using a for loop
            {
                key.MoveNext(); // Moves the enumerator to its next item
                RVS_Team d_team = (RVS_Team)Teams[(int)key.Current]; // creates a variable referencing the currently iterated team

                for (int i2 = 0; i2 < d_team.Players.Count; ++i2) // creates a second for loop to iterate through all the players in the current teams
                {
                    object o = (object)d_team.Players[i2]; // creates the currently iterated object in the teams Players variable as an object to be type safe

                    if (o is PlayerMobile) // checks if o is infact a Player, if it isn't it is just an empty slot
                    {
                        PlayerMobile pm = (PlayerMobile)o; // o is a playermobile so we create a variable to reference it as a playermobile

                        if (AllAccepted())
                        {
                            start = true;
                            pm.CloseGump(typeof(RVSSetup_Pending));
                        }
                        else if ((bool)d_team.Accepted[pm])
                        {
                            pm.CloseGump(typeof(RVSSetup_Pending));
                            pm.SendGump(new RVSSetup_Pending(this));
                        }
                        else
                        {
                            pm.CloseGump(typeof(RVSSetup_Rules_View));
                            pm.SendGump(new RVSSetup_Rules_View(this, (int)key.Current, i2));
                        }
                    }
                }
            }

            if (start)
            {
                EchoMessage("RVS Duel Starting....");
                StartTimer = new RVS_StartTimer(this, 1, 1);
            }
        }

        public void PauseRVS()
        {
            if (StartTimer != null)
                StartTimer.Stop();
        }

        public void SendControllerSetup()
        {
            if (IsRematch)
            {
                EndSelf();
                return;
            }
            PauseRVS();

            IEnumerator key = Teams.Keys.GetEnumerator(); // creates the key variable so we can iterate through the Teams dictionary
            for (int i = 0; i < Teams.Count; ++i) // Iterates through the Teams dictionary using a for loop
            {
                key.MoveNext(); // Moves the enumerator to its next item
                RVS_Team d_team = (RVS_Team)Teams[(int)key.Current]; // creates a variable referencing the currently iterated team

                for (int i2 = 0; i2 < d_team.Players.Count; ++i2)
                {
                    object o = (object)d_team.Players[i2];

                    if (o is PlayerMobile)
                    {
                        PlayerMobile pm = (PlayerMobile)d_team.Players[i2];

                        if (pm == Caller)
                        {
                            pm.CloseGump(typeof(RVSSetup_Pending));
                            pm.SendGump(new RVSSetup_Main(this));
                        }
                        else
                        {
                            pm.CloseGump(typeof(RVSSetup_Pending));
                            pm.CloseGump(typeof(RVSSetup_Rules_View));
                        }
                    }
                }
            }

            EchoMessage("A player has left the RVS duel, start has been delayed.");
        }

        public void EndSelf()
        {
            if (Ended)
                return;

            if (InProgress)
                Controller.EndRVS(0);

            Ended = true;

            IEnumerator key = Teams.Keys.GetEnumerator(); // creates the key variable so we can iterate through the Teams dictionary
            for (int i = 0; i < Teams.Count; ++i) // Iterates through the Teams dictionary using a for loop
            {
                key.MoveNext(); // Moves the enumerator to its next item
                RVS_Team d_team = (RVS_Team)Teams[(int)key.Current]; // creates a variable referencing the currently iterated team

                for (int i2 = 0; i2 < d_team.Players.Count; ++i2)
                {
                    object o = (object)d_team.Players[i2];

                    if (o is PlayerMobile)
                    {
                        PlayerMobile pm = (PlayerMobile)d_team.Players[i2];

                        RefundBuyIn(pm);

                        pm.CloseGump(typeof(RVSSetup_Main));
                        pm.CloseGump(typeof(RVSSetup_Pending));
                        pm.CloseGump(typeof(RVSSetup_ParticipantSetup));
                        pm.CloseGump(typeof(RVSSetup_AddParticipant));
                        pm.CloseGump(typeof(RVSSetup_Rules_View));
                        pm.CloseGump(typeof(RVSSetup_ViewParticipants));
                        pm.CloseGump(typeof(RVSSetup_Rules_Armor_View));
                        pm.CloseGump(typeof(RVSSetup_Rules_Combat_View));
                        pm.CloseGump(typeof(RVSSetup_Rules_Items_View));
                        pm.CloseGump(typeof(RVSSetup_Rules_Skills_View));
                        pm.CloseGump(typeof(RVSSetup_Rules_Weapons_View));
                        pm.CloseGump(typeof(RVSSetup_Rules_Spells_View));
                        pm.CloseGump(typeof(RVSSetup_Rules_Spells_1st_View));
                        pm.CloseGump(typeof(RVSSetup_Rules_Spells_2nd_View));
                        pm.CloseGump(typeof(RVSSetup_Rules_Spells_3rd_View));
                        pm.CloseGump(typeof(RVSSetup_Rules_Spells_4th_View));
                        pm.CloseGump(typeof(RVSSetup_Rules_Spells_5th_View));
                        pm.CloseGump(typeof(RVSSetup_Rules_Spells_6th_View));
                        pm.CloseGump(typeof(RVSSetup_Rules_Spells_7th_View));
                        pm.CloseGump(typeof(RVSSetup_Rules_Spells_8th_View));
                    }
                }
            }

            EchoMessage(String.Format("The RVS duel has been cancelled."));
            if(RVS_Config.Registry.Contains(this))
                RVS_Config.Registry.Remove(this);
        }

        public void RefundBuyIn(PlayerMobile pm)
        {
            if (BuyIn <= 0)
                return;

            BankBox box = (BankBox)pm.BankBox;

            if (BuyIn >= 5000)
            {
                BankCheck check = new BankCheck(BuyIn);
                box.DropItem(check);
            }
            else
            {
                Gold gold = new Gold(BuyIn);
                box.DropItem(gold);
            }

            pm.SendMessage("Your buy in has been refunded, check your bank.");
        }

        public bool AllAccepted()
        {
            IEnumerator key = Teams.Keys.GetEnumerator();
            for (int i = 0; i < Teams.Count; ++i)
            {
                key.MoveNext();
                RVS_Team team = (RVS_Team)Teams[(int)key.Current];

                for (int i2 = 0; i2 < team.Players.Count; ++i2)
                {
                    object o = (object)team.Players[i2];
                    if (o is PlayerMobile)
                    {
                        if (!(bool)team.Accepted[(PlayerMobile)o])
                            return false;
                    }
                }
            }

            return true;
        }

        public void EchoMessage(string msg)
        {
            IEnumerator key = Teams.Keys.GetEnumerator();
            for (int i = 0; i < Teams.Count; ++i)
            {
                key.MoveNext();
                RVS_Team team = (RVS_Team)Teams[(int)key.Current];

                for (int i2 = 0; i2 < team.Players.Count; ++i2)
                {
                    if (team.Players[i2] != "@null")
                    {
                        PlayerMobile player = (PlayerMobile)team.Players[i2];
                        player.SendMessage(msg);
                    }
                }
            }
        }

        public bool InRVS(PlayerMobile m)
        {
            IEnumerator key = (IEnumerator)Teams.Keys.GetEnumerator();
            for (int i = 0; i < Teams.Count; ++i)
            {
                key.MoveNext();
                RVS_Team team = (RVS_Team)Teams[(int)key.Current];

                if (team.Players.Contains(m))
                    return true;
            }

            return false;
        }

        public bool IsAlly(PlayerMobile from, PlayerMobile to)
        {
            bool isaally = false;
            IEnumerator key = (IEnumerator)Teams.Keys.GetEnumerator();
            for (int i = 0; i < Teams.Count; ++i)
            {
                key.MoveNext();
                RVS_Team team = (RVS_Team)Teams[(int)key.Current];

                if (team.Players.Contains(from) && team.Players.Contains(to))
                    isaally = true;
            }

            if (isaally)
                return true;

            return false;
        }

        public bool IsEnemy(PlayerMobile from, PlayerMobile to)
        {
            bool isaenemy = false;
            IEnumerator key = (IEnumerator)Teams.Keys.GetEnumerator();
            for (int i = 0; i < Teams.Count; ++i)
            {
                key.MoveNext();
                RVS_Team team = (RVS_Team)Teams[(int)key.Current];

                if (team.Players.Contains(from) && !team.Players.Contains(to))
                    isaenemy = true;
            }

            if (isaenemy)
                return true;

            return false;
        }
    }

    public class RVS_Team
    {
        public int TeamID;
        public int Size;
        public ArrayList Players;
        public Dictionary<PlayerMobile, bool> Accepted;

        public RVS_Team(int team)
        {
            TeamID = team;
            Size = 1;
            Players = new ArrayList();
            Accepted = new Dictionary<PlayerMobile, bool>();
        }

        public bool AllDead()
        {
            for (int i = 0; i < Players.Count; ++i)
            {
                object o = (object)Players[i];

                if (o is PlayerMobile)
                {
                    PlayerMobile pm = (PlayerMobile)o;

                    if (pm.Alive)
                        return false;
                }
            }

            return true;
        }

        public bool HasPlayers()
        {
            for (int i = 0; i < Players.Count; ++i)
            {
                object o = (object)Players[i];

                if (o is PlayerMobile)
                    return true;
            }

            return false;
        }

        public bool AllAccepted()
        {
            IEnumerator key = Accepted.Keys.GetEnumerator();
            for (int i = 0; i < Accepted.Count; ++i)
            {
                key.MoveNext();

                if(!(bool)Accepted[(PlayerMobile)key.Current])
                    return false;
            }

            return true;
        }

        public bool TeamFull()
        {
            if (Players.Count >= Size)
                return true;
            
            return false;
        }

        public bool InTeam(PlayerMobile m)
        {
            if (Players.Contains(m))
                return true;

            return false;
        }

        public void AddPlayer(PlayerMobile m)
        {
            Players.Add(m);
            Accepted.Add(m, true);
        }

        public void RemovePlayer(PlayerMobile m)
        {
            Players.Remove(m);
        }
    }

    public class RVS_StartTimer : Timer
    {
        public RVS Handeling;
        public int C, A;

        public RVS_StartTimer(RVS d, int count, int attempts)
            : base(TimeSpan.FromSeconds(1))
        {
            Handeling = d;
            C = count;
            A = attempts;
            this.Start();
        }

        protected override void OnTick()
        {
            if (Handeling.Ended)
            {
                this.Stop();
                return;
            }

            Handeling.EchoMessage(C.ToString());

            if (A >= 4)
            {
                Handeling.Caller.SendGump(new RVSSetup_Main(Handeling));
                Handeling.EchoMessage("RVS starting timed out, please wait.");
                this.Stop();
                return;
            }

            if (C >= RVS_Config.RVSStartDelay)
            {
                C = 0;
                AttemptStart();
            }
            else
            {
                this.Stop();
                RVS_StartTimer tmr = new RVS_StartTimer(Handeling, (C + 1), A);
            }
        }

        public void AttemptStart()
        {
            if (!CheckLoggedIn())
            {
                Handeling.Caller.SendGump(new RVSSetup_Main(Handeling));
                Handeling.EchoMessage("One or more players have logged out, the duel could not start.");
                this.Stop();
                return;
            }
            else if (!AllAlive())
            {
                Handeling.EchoMessage("Not all players are alive, duel start delayed.");
                RVS_StartTimer tmr = new RVS_StartTimer(Handeling, (C + 1), (A + 1));
                this.Stop();
                return;
            }
            else if (!AllDismounted())
            {
                //Handeling.EchoMessage("Not all participants are dismounted, duel start delayed.");
                //RVS_StartTimer tmr = new RVS_StartTimer(Handeling, (C + 1), (A + 1));
               	//this.Stop();
                //return;
            }
            else if (!NoneFlagged())
            {
                //Handeling.EchoMessage("Some participants have been in combat to recently for the duel to start, duel start delayed.");
                //RVS_StartTimer tmr = new RVS_StartTimer(Handeling, (C + 1), (A + 1));
                //this.Stop();
                //return;
            }
	    	else if (!NoneInJail())
	    	{
                Handeling.EchoMessage("Some participants are in areas where duels cannot take place, duel start delayed.");
                RVS_StartTimer tmr = new RVS_StartTimer(Handeling, (C + 1), (A + 1));
                this.Stop();
                return;
            }
	    	RVSController avail = PickRandomArena();
		
	    	if (avail == null)
            	avail = ArenaAvaiable();

            if (avail != null)
            {
                Handeling.EchoMessage("RVS Duel starting!");

                avail.StartRVS(Handeling);

                return;
            }
            else
            {
                Handeling.Caller.SendGump(new RVSSetup_Main(Handeling));
                Handeling.EchoMessage("No arena's avaiable, please wait before trying to restart the duel.");
                this.Stop();
                return;
            }

        }

		public RVSController PickRandomArena()
		{
			if ( RVS_Config.Controllers.Count > 0 )
			{ 
		    	RVSController c = RVS_Config.Controllers[ Utility.Random(RVS_Config.Controllers.Count) ];
		    	int count = 0;
	
	        	while( c.InUse || !c.Enabled && count != 100)
		    	{
					c = RVS_Config.Controllers[ Utility.Random(RVS_Config.Controllers.Count) ];
					count++;
		    	}
	
		    	if ( count == 100 )
		    	{
					return null;
		   	 	}
		    	else
		    	{
					return c;
		    	}
	    	}
	    	else
	    	{
	    		return null;
	    	}
		}

        public RVSController ArenaAvaiable()
        {
	    foreach (RVSController c in RVS_Config.Controllers)
            {
                if (!c.InUse && c.Enabled)
                    return c;
            }

            return null;
        }

        public bool NoneFlagged()
        {
            IEnumerator key = Handeling.Teams.Keys.GetEnumerator();
            for (int i = 0; i < Handeling.Teams.Count; ++i)
            {
                key.MoveNext();
                RVS_Team team = (RVS_Team)Handeling.Teams[(int)key.Current];

                for (int i2 = 0; i2 < team.Players.Count; ++i2)
                {
                    object o = (object)team.Players[i2];

                    if (o is PlayerMobile)
                    {
                        PlayerMobile pm = (PlayerMobile)o;

                        if (pm.Combatant != null || pm.Aggressed.Count > 0)
                            return false;
                    }
                }
            }

            return true;
        }

	public bool NoneInJail()
        {
            IEnumerator key = Handeling.Teams.Keys.GetEnumerator();
            for (int i = 0; i < Handeling.Teams.Count; ++i)
            {
                key.MoveNext();
                RVS_Team team = (RVS_Team)Handeling.Teams[(int)key.Current];

                for (int i2 = 0; i2 < team.Players.Count; ++i2)
                {
                    object o = (object)team.Players[i2];

                    if (o is PlayerMobile)
                    {
                        PlayerMobile pm = (PlayerMobile)o;

                        if ( pm.Region.IsPartOf( typeof( Server.Regions.Jail ) ) || pm.Map == Map.Malas )
                            return false;
                    }
                }
            }

            return true;
        }

        public bool AllDismounted()
        {
            IEnumerator key = Handeling.Teams.Keys.GetEnumerator();
            for (int i = 0; i < Handeling.Teams.Count; ++i)
            {
                key.MoveNext();
                RVS_Team team = (RVS_Team)Handeling.Teams[(int)key.Current];

                for (int i2 = 0; i2 < team.Players.Count; ++i2)
                {
                    object o = (object)team.Players[i2];

                    if (o is PlayerMobile)
                    {
                        PlayerMobile pm = (PlayerMobile)o;

                        if (pm.Mounted)
                            return false;
                    }
                }
            }

            return true;
        }

        public bool AllAlive()
        {
            IEnumerator key = Handeling.Teams.Keys.GetEnumerator();
            for (int i = 0; i < Handeling.Teams.Count; ++i)
            {
                key.MoveNext();
                RVS_Team team = (RVS_Team)Handeling.Teams[(int)key.Current];

                for (int i2 = 0; i2 < team.Players.Count; ++i2)
                {
                    object o = (object)team.Players[i2];

                    if (o is PlayerMobile)
                    {
                        PlayerMobile pm = (PlayerMobile)o;

                        if (!pm.Alive)
                            return false;
                    }
                }
            }

            return true;
        }

        public bool CheckLoggedIn()
        {
            IEnumerator key = Handeling.Teams.Keys.GetEnumerator();
            for (int i = 0; i < Handeling.Teams.Count; ++i)
            {
                key.MoveNext();
                RVS_Team team = (RVS_Team)Handeling.Teams[(int)key.Current];

                bool alllogged = true;
                for (int i2 = 0; i2 < team.Players.Count; ++i2)
                {
                    object o = (object)team.Players[i2];

                    if (o is PlayerMobile)
                    {
                        PlayerMobile pm = (PlayerMobile)o;

                        if (pm.NetState == null)
                        {
                            alllogged = false;
                            Handeling.Teams[(int)key.Current].Players[i2] = "@null";
                            Handeling.Teams[(int)key.Current].Accepted.Remove(pm);
                        }
                    }
                }

                if (!alllogged)
                    return false;
            }

            return true;
        }
    }
}
