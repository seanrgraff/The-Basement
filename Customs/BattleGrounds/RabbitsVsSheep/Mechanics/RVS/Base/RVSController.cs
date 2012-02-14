using System;
using System.Collections;
using System.Collections.Generic;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Commands;
using Server.Gumps;
using Server.Misc;
using Server.Factions;
using Solaris.BoardGames;

namespace Server.RabbitsVsSheep
{
    public class RVSController : Item
    {
    	public virtual string GameName{ get{ return "RVSing"; } }
    	
    	//FOR TEAM ONE
        protected Map SheepRegionMap;
        protected Rectangle2D SheepRegionPoint;
        [CommandProperty(AccessLevel.GameMaster)]
        public Map sheepRegionMap
        {
            get { return SheepRegionMap; }
            set { SheepRegionMap = value; }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public Rectangle2D sheepRegionPoint
        {
            get { return SheepRegionPoint; }
            set { SheepRegionPoint = value; }
        }
        
        //FOR TEAM TWO
        protected Map RabbitRegionMap;
        protected Rectangle2D RabbitRegionPoint;
        [CommandProperty(AccessLevel.GameMaster)]
        public Map rabbitRegionMap
        {
            get { return RabbitRegionMap; }
            set { RabbitRegionMap = value; }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public Rectangle2D rabbitRegionPoint
        {
            get { return RabbitRegionPoint; }
            set { RabbitRegionPoint = value; }
        }

        protected Map SheepSpawnMap;
        protected Point3D SheepSpawnPoint;

        [CommandProperty(AccessLevel.GameMaster)]
        public Map sheepSpawnMap
        {
            get { return SheepSpawnMap; }
            set { SheepSpawnMap = value; }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public Point3D sheepSpawnPoint
        {
            get { return SheepSpawnPoint; }
            set { SheepSpawnPoint = value; }
        }
        
        protected Map RabbitSpawnMap;
        protected Point3D RabbitSpawnPoint;

        [CommandProperty(AccessLevel.GameMaster)]
        public Map rabbitSpawnMap
        {
            get { return RabbitSpawnMap; }
            set { RabbitSpawnMap = value; }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public Point3D rabbitSpawnPoint
        {
            get { return RabbitSpawnPoint; }
            set { RabbitSpawnPoint = value; }
        }

        protected bool abled;
        [CommandProperty(AccessLevel.GameMaster)]
        public bool Enabled
        {
            get { return abled; }
            set { abled = value; }
        }
        
        public int SheepPoints = 0;
        public int RabbitPoints = 0;

        public RVS Handeling;
        public RVSRegion SheepRegion;
        public RVSRegion RabbitRegion;
        public bool InUse;
        public bool HasStarted;
        public bool AllKilled = false;

        public RVS_EndTimer EndTimer;

        public static Dictionary<Serial, Point3D> PlayerLocations;
        public static Dictionary<Serial, Map> PlayerMaps;
        public static Dictionary<Serial, ArrayList> PlayerItems;
        public static Dictionary<Serial, Point3D> ItemLocations;
        public static ArrayList Trees;
        public static ArrayList Traps;
        public static ArrayList Enemies;
        public static ArrayList InsuredItems;
        public static ArrayList FrozenItems;
        public static ArrayList Fountains;
        public static ArrayList EVsAndBSs;

        [Constructable]
        public RVSController()
            : base(0xEDE)
        {
            Name = "RVS Controller";
            Visible = false;
            Movable = false;
        }

        public static NotorietyHandler NotoHandler = null;

        public static void Initialize()
        {
            NotoHandler = new NotorietyHandler(Handle_Notoriety);
        }

        public virtual void On_ShutDown(ShutdownEventArgs args)
        {
            for (int i = 0; i < RVS_Config.Controllers.Count; ++i)
            {
                RVSController d = (RVSController)RVS_Config.Controllers[i];

                if (d.HasStarted && d.Handeling != null)
                    d.EndRVS(0);
            }
        }

        public static int Handle_Notoriety(Mobile from, Mobile target)
        {
        	//BaseCreature targetPet = null;
        	//BaseCreature fromPet = null;
            if (from is PlayerMobile && target is PlayerMobile)
            {
                if (RVS_Config.InARVS((PlayerMobile)from) && RVS_Config.InARVS((PlayerMobile)target))
                {
                    if (!RVS_Config.RVSStarted((PlayerMobile)from))
                        return NotorietyHandlers.MobileNotoriety2(from, target);

                    if (RVS_Config.IsAlly((PlayerMobile)from, (PlayerMobile)target))
                        return Notoriety.Ally;
                    if (RVS_Config.IsEnemy((PlayerMobile)from, (PlayerMobile)target))
                        return Notoriety.Enemy;
                }
                else if (RVS_Config.InARVS((PlayerMobile)from) || RVS_Config.InARVS((PlayerMobile)target))
                    return NotorietyHandlers.MobileNotoriety2(from, target);
            }
            else if (from is PlayerMobile && target is BaseCreature)
            {
            	BaseCreature targetPet = (BaseCreature)target;
            	if (RVS_Config.InARVS((PlayerMobile) from) && RVS_Config.InARVS((PlayerMobile)targetPet.ControlMaster) && targetPet.Controlled)
            	{
            		if (!RVS_Config.RVSStarted((PlayerMobile)from))
                        return NotorietyHandlers.MobileNotoriety2(from, target);

                    if (RVS_Config.IsAlly((PlayerMobile)from, (PlayerMobile)targetPet.ControlMaster))
                        return Notoriety.Ally;
                    if (RVS_Config.IsEnemy((PlayerMobile)from, (PlayerMobile)targetPet.ControlMaster))
                        return Notoriety.Enemy;
            	}
            }
            else if (from is BaseCreature && target is PlayerMobile)
            {
            	BaseCreature fromPet = (BaseCreature)from;
            	if (RVS_Config.InARVS((PlayerMobile)fromPet.ControlMaster) && RVS_Config.InARVS((PlayerMobile)target) && fromPet.Controlled)
            	{
            		if (!RVS_Config.RVSStarted((PlayerMobile)fromPet.ControlMaster))
                        return NotorietyHandlers.MobileNotoriety2(from, target);

                    if (RVS_Config.IsAlly((PlayerMobile)fromPet.ControlMaster, (PlayerMobile)target))
                        return Notoriety.Ally;
                    if (RVS_Config.IsEnemy((PlayerMobile)fromPet.ControlMaster, (PlayerMobile)target))
                        return Notoriety.Enemy;
            	}
            }
           	else if (from is BaseCreature && target is BaseCreature)
            {
            	BaseCreature fromPet = (BaseCreature)from;
            	BaseCreature targetPet = (BaseCreature)target;
            	if (RVS_Config.InARVS((PlayerMobile)fromPet.ControlMaster) && RVS_Config.InARVS((PlayerMobile)targetPet.ControlMaster) && fromPet.Controlled && targetPet.Controlled)
            	{
            		if (!RVS_Config.RVSStarted((PlayerMobile)fromPet.ControlMaster))
                        return NotorietyHandlers.MobileNotoriety2(from, target);

                    if (RVS_Config.IsAlly((PlayerMobile)fromPet.ControlMaster, (PlayerMobile)targetPet.ControlMaster))
                        return Notoriety.Ally;
                    if (RVS_Config.IsEnemy((PlayerMobile)fromPet.ControlMaster, (PlayerMobile)targetPet.ControlMaster))
                        return Notoriety.Enemy;
            	}
            }

            return NotorietyHandlers.MobileNotoriety2(from, target);
        }

        public void StartRVS(RVS d)
        {
            Handeling = d;

            if (Handeling.Caller.NetState == null)
            {
                Handeling.EndSelf();
                return;
            }

            HasStarted = true;
            InUse = true;
            Handeling.InProgress = true;
            Handeling.Controller = this;

            if (PlayerLocations == null)
                PlayerLocations = new Dictionary<Serial, Point3D>();
            if (PlayerMaps == null)
                PlayerMaps = new Dictionary<Serial, Map>();
            if (PlayerItems == null)
                PlayerItems = new Dictionary<Serial, ArrayList>();
            if (ItemLocations == null)
                ItemLocations = new Dictionary<Serial, Point3D>();
            if (InsuredItems == null)
                InsuredItems = new ArrayList();
            if (FrozenItems == null)
                FrozenItems = new ArrayList();
            if (Trees == null)
                Trees = new ArrayList();
            if (Traps == null)
            	Traps = new ArrayList();
            if (Enemies == null)
            	Enemies = new ArrayList();
            if (Fountains == null)
            	Fountains = new ArrayList();
           	if (EVsAndBSs == null)
            	EVsAndBSs = new ArrayList();
                
            SpawnTrees(Handeling, SheepSpawnPoint, SheepSpawnMap);
            SpawnTraps(Handeling, SheepSpawnPoint, SheepSpawnMap);
            SpawnFountainsSheep( Handeling, "Sheep", SheepSpawnPoint, SheepSpawnMap );
            
            SpawnTrees(Handeling, RabbitSpawnPoint, RabbitSpawnMap);
            SpawnTraps(Handeling, RabbitSpawnPoint, RabbitSpawnMap);
            SpawnFountainsRabbits( Handeling, "Rabbit", RabbitSpawnPoint, RabbitSpawnMap );
            
            SpawnEnemies( Handeling, "Sheep", 1);
            SheepPoints++;
            SpawnEnemies( Handeling, "Rabbit", 1);
            RabbitPoints++;

            IEnumerator key = Handeling.Teams.Keys.GetEnumerator();
            for (int i = 0; i < Handeling.Teams.Count; ++i)
            {
                key.MoveNext();
                RVS_Team d_team = (RVS_Team)Handeling.Teams[(int)key.Current];

                for (int i2 = 0; i2 < d_team.Players.Count; ++i2)
                {
                    object o = (object)d_team.Players[i2];

                    if (o is PlayerMobile)
                    {
                        PlayerMobile pm = (PlayerMobile)o;
                        StorePlayer(pm);
                        SpawnPlayer(pm, (int)key.Current);
                        PlayerNoto(pm);
                        PetNoto(pm);
                        pm.Blessed = true;
                        pm.Frozen = true;
                        pm.Spell = null;
                        ResurrectPlayer(pm);
                        ResurrectPets(pm);
                    }
                }
            }

            Timer.DelayCall(TimeSpan.FromSeconds(3.0), new TimerCallback(UnInvul));
            EndTimer = new RVS_EndTimer(Handeling);
        }

        public void UnInvul()
        {
            if (Handeling == null)
                return;
            if (Handeling.Ended)
                EndRVS(0);

            IEnumerator key = Handeling.Teams.Keys.GetEnumerator();
            for (int i = 0; i < Handeling.Teams.Count; ++i)
            {
                key.MoveNext();
                RVS_Team d_team = (RVS_Team)Handeling.Teams[(int)key.Current];

                for (int i2 = 0; i2 < d_team.Players.Count; ++i2)
                {
                    object o = (object)d_team.Players[i2];

                    if (o is PlayerMobile)
                    {
                        PlayerMobile pm = (PlayerMobile)o;
                        pm.Blessed = false;
                        pm.Frozen = false;
                    }
                }
            }

            if(HasStarted)
                Handeling.EchoMessage("You can now fight!");
        }

        public void EndRVS(int teamid)
        {
            if (Handeling == null)
                return;

            if (EndTimer != null)
                EndTimer.Stop();

            Handeling.EchoMessage(String.Format("The RVS duel has ended, team {0} has won!", teamid.ToString()));

            HasStarted = false;
            InUse = false;
            Handeling.InProgress = false;

            if (!Handeling.Ended)
            {
                IEnumerator key = Handeling.Teams.Keys.GetEnumerator();
                for (int i = 0; i < Handeling.Teams.Count; ++i)
                {
                    key.MoveNext();
                    RVS_Team d_team = (RVS_Team)Handeling.Teams[(int)key.Current];

                    for (int i2 = 0; i2 < d_team.Players.Count; ++i2)
                    {
						if ( d_team.Players[i2] != "@null" )
						{
							object o = (object)d_team.Players[i2];

							if (o is PlayerMobile && o != "@null")
							{
								PlayerMobile pm = (PlayerMobile)o;
								if (teamid != 0)
								{
									if ((int)key.Current == teamid)
									{
										//GiveHeads(pm, teamid);
										//BoardGameData.ChangeScore( GameName, pm, 1 );
										BoardGameData.AddWin( GameName, pm );
									}
									else
									{
										//BoardGameData.ChangeScore( GameName, pm, -1 );
										BoardGameData.AddLose( GameName, pm );
									}
									int score = BoardGameData.getWins(GameName, pm) - BoardGameData.getLosses(GameName, pm);
									BoardGameData.SetScore( GameName, pm, score );

									if ((int)key.Current == teamid && AllKilled && !Handeling.IsRematch)
										PayBuyIn(pm, teamid);
								}
								else
									if (!AllKilled && !Handeling.IsRematch)
										Handeling.RefundBuyIn(pm);

								Handeling.Teams[(int)key.Current].Accepted[pm] = false;
								ResurrectPlayer(pm);
								ResurrectPets(pm);
								LoadPlayer(pm);
								PlayerNoto(pm);
								PetNoto(pm);
							}
						}
                    }
                }
                if ( InsuredItems != null )
				{
					for (int i = 0; i < InsuredItems.Count; ++i)
					{
						Item item = (Item)InsuredItems[i];
						item.Insured = true;
					}
                }
				if ( FrozenItems != null )
				{
					for (int i = 0; i < FrozenItems.Count; ++i)
					{
						Item item = (Item)FrozenItems[i];
						item.Movable = true;
					}
				}
                UnInvul();
            }

            //PlayerLocations = null;
            //PlayerMaps = null;
            //PlayerItems = null;
            //ItemLocations = null;
            InsuredItems = null;
            FrozenItems = null;
            
            DeleteTrees();
            Trees = null;
            DeleteTraps();
            Traps = null;
            DeleteEnemies();
            Enemies = null;
            DeleteFountains();
            Fountains = null;
            
            //remove any residual BS or EV
            DeleteEVsAndBSs();
            EVsAndBSs = null;
            
            //reset points
            RabbitPoints = 0;
            SheepPoints = 0;

            Handeling.UpdateAllPending();
            Handeling.BuyIn = 0;
            Handeling.IsRematch = true;
            //Handeling = null;
        }

        public void PayBuyIn(PlayerMobile pm, int teamid)
        {
            if (Handeling.BuyIn <= 0)
                return;

            int playercount = 0;
            int sharecount = 0;
            IEnumerator key = Handeling.Teams.Keys.GetEnumerator();
            for (int i = 0; i < Handeling.Teams.Count; ++i)
            {
                key.MoveNext();
                RVS_Team d_team = (RVS_Team)Handeling.Teams[(int)key.Current];

                for (int i2 = 0; i2 < d_team.Players.Count; ++i2)
                {
                    object o = (object)d_team.Players[i2];

                    if (o != "@null")
                        playercount += 1;

                    if ((int)key.Current == teamid && o != "@null")
                        sharecount += 1;
                }
            }

            int goldshare = (playercount * Handeling.BuyIn) / sharecount;

            BankBox box = (BankBox)pm.BankBox;
            if (goldshare >= 5000)
            {
                BankCheck check = new BankCheck(goldshare);
                box.DropItem(check);
            }
            else
            {
                Gold gold = new Gold(goldshare);
                box.DropItem(gold);
            }

            pm.SendMessage(String.Format("Congragulations! you have won {0}gp.", goldshare.ToString()));
        }

        public void HandleDeath(object o)
        {
            PlayerMobile died = (PlayerMobile)o;
            if (Handeling == null)
                return;

            if (!Handeling.InRVS(died))
                return;

            if (!HasStarted)
                return;

            Handeling.EchoMessage(String.Format("{0} has been killed!", died.Name));

            int teamid = 0;
            int alive = 0;

            IEnumerator key = Handeling.Teams.Keys.GetEnumerator();
            for(int i = 0; i < Handeling.Teams.Count; ++i)
            {
                key.MoveNext();
                RVS_Team d_team = (RVS_Team)Handeling.Teams[(int)key.Current];

                if(!d_team.AllDead())
                {
                    alive += 1;
                    teamid = (int)key.Current;
                }
            }

            if (alive == 1 || alive == 0)
            {
                AllKilled = true;
                EndRVS(teamid);
            }
        }

        public void GiveHeads(PlayerMobile pm, int teamid)
        {
         
			if (Handeling == null)
                return;

            Backpack pack = (Backpack)pm.Backpack;
            ArrayList heads = new ArrayList();

            IEnumerator key = Handeling.Teams.Keys.GetEnumerator();
            for (int i = 0; i < Handeling.Teams.Count; ++i)
            {
                key.MoveNext();
                RVS_Team d_team = (RVS_Team)Handeling.Teams[(int)key.Current];

                for (int i2 = 0; i2 < d_team.Players.Count; ++i2)
                {
                    object o = (object)d_team.Players[i2];

                    if (o is PlayerMobile)
                    {
                        PlayerMobile pm2 = (PlayerMobile)o;

                        if ((int)key.Current != teamid)
                            heads.Add(new RVSHead(pm2, pm));
                    }
                }
            }

            for (int i = 0; i < heads.Count; ++i)
            {
                RVSHead head = (RVSHead)heads[i];
                pack.DropItem(head);
            }
			
	    }
        
	    public void LaunchFireworks( Mobile from )
		{
			Map map = from.Map;

			if ( map == null )
				return;

			Point3D ourLoc = from.Location;

			Point3D startLoc = new Point3D( ourLoc.X, ourLoc.Y, ourLoc.Z );
			Point3D endLoc = new Point3D( startLoc.X + Utility.RandomMinMax( -2, 2 ), startLoc.Y + Utility.RandomMinMax( -2, 2 ), startLoc.Z + 5 );

			Effects.SendMovingEffect( new Entity( Serial.Zero, startLoc, map ), new Entity( Serial.Zero, endLoc, map ),
				0x36E4, 5, 0, false, false );

			Timer.DelayCall( TimeSpan.FromSeconds( 1.0 ), new TimerStateCallback( FinishLaunch ), new object[]{ from, endLoc, map } );
		}
		
		private void FinishLaunch( object state )
		{
			object[] states = (object[])state;

			Mobile from = (Mobile)states[0];
			Point3D endLoc = (Point3D)states[1];
			Map map = (Map)states[2];

			int hue = Utility.Random( 40 );

			if ( hue < 8 )
				hue = 0x66D;
			else if ( hue < 10 )
				hue = 0x482;
			else if ( hue < 12 )
				hue = 0x47E;
			else if ( hue < 16 )
				hue = 0x480;
			else if ( hue < 20 )
				hue = 0x47F;
			else
				hue = 0;

			if ( Utility.RandomBool() )
				hue = Utility.RandomList( 0x47E, 0x47F, 0x480, 0x482, 0x66D );

			int renderMode = Utility.RandomList( 0, 2, 3, 4, 5, 7 );

			Effects.PlaySound( endLoc, map, Utility.Random( 0x11B, 4 ) );
			Effects.SendLocationEffect( endLoc, map, 0x373A + (0x10 * Utility.Random( 4 )), 16, 10, hue, renderMode );
		}
		

        public void StorePlayer(PlayerMobile pm)
        {
			//pm.SendMessage(String.Format("Storing Char"));
            PlayerLocations.Add(pm.Serial, pm.Location);
			//pm.SendMessage(String.Format("PlayerLocationAdded"));
            PlayerMaps.Add(pm.Serial, pm.Map);
			//pm.SendMessage(String.Format("PlayerMapAdded"));
            ArrayList items = new ArrayList();
            foreach (Item item in pm.Items)
            {
                if (item is Backpack || item is BankBox || !item.Movable) { }
                else
                {
                    items.Add(item);
                    ItemLocations.Add(item.Serial, item.Location);
                }

                if (item.Insured)
                {
                    item.Insured = false;
                    InsuredItems.Add(item);
                }
            }
			//pm.SendMessage(String.Format("PaperdollItemsAdded"));

            foreach (Item item in pm.Backpack.Items)
            {
                if (item is Backpack || item is BankBox || !item.Movable) { }
                else
                {
                    items.Add(item);
                    ItemLocations.Add(item.Serial, item.Location);
                }

                if (item.Insured)
                {
                    item.Insured = false;
                    InsuredItems.Add(item);
                }
            }
			//pm.SendMessage(String.Format("BackpackItemsAdded"));

            for (int i = 0; i < items.Count; ++i)
            {
                CheckAllowed(pm, (Item)items[i]);
            }

            PlayerItems.Add(pm.Serial, items);
			//pm.SendMessage(String.Format("ALLItemsAdded"));
        }

        public void CheckAllowed(Mobile owner, Item item)
        {
            bool isallowed = true;

            if (item is BaseWeapon)
            {
                BaseWeapon weap = (BaseWeapon)item;

                if (!Handeling.Weapons)
                    isallowed = false;
                if (!Handeling.Poisoned && weap.PoisonCharges > 0)
                    isallowed = false;
                if (!Handeling.Magical && !weap.Attributes.IsEmpty || !Handeling.Magical && weap.AccuracyLevel != WeaponAccuracyLevel.Regular || !Handeling.Magical && weap.DamageLevel != WeaponDamageLevel.Regular || !Handeling.Magical && weap.DurabilityLevel != WeaponDurabilityLevel.Regular)
                    isallowed = false;
                if (!Handeling.RunicWeapons && weap.Quality != WeaponQuality.Regular)
                    isallowed = false;
            }
            else if (item is BaseArmor)
            {
                BaseArmor armor = (BaseArmor)item;

                if (!Handeling.Armor)
                    isallowed = false;
                if (!Handeling.MagicalArmor && !armor.Attributes.IsEmpty)
                    isallowed = false;
                if (!Handeling.MagicalArmor && armor.ProtectionLevel != ArmorProtectionLevel.Regular)
                    isallowed = false;
            }
            else if (item is BaseShield && !Handeling.Shields)
                isallowed = false;

            if (!isallowed)
            {
                Backpack pack = (Backpack)owner.Backpack;
                pack.DropItem(item);
                item.Movable = false;
                FrozenItems.Add(item);
            }
        }

        public void LoadPlayer(PlayerMobile pm)
        {
            Backpack pack = (Backpack)pm.Backpack;
			//pm.SendMessage(String.Format("SettingBackpack"));
			
			TeleportAllPets( pm, (Point3D)PlayerLocations[pm.Serial], (Map)PlayerMaps[pm.Serial] );
			
            pm.Location = (Point3D)PlayerLocations[pm.Serial];
			PlayerLocations.Remove(pm.Serial);
			//pm.SendMessage(String.Format("SettingLocation"));
			
            pm.Map = (Map)PlayerMaps[pm.Serial];
			PlayerMaps.Remove(pm.Serial);
			//pm.SendMessage(String.Format("SettingMap"));

            ArrayList items = (ArrayList)PlayerItems[pm.Serial];
			PlayerItems.Remove(pm.Serial);
			//pm.SendMessage(String.Format("Setting Arraylist of Items"));
            for (int i = 0; i < items.Count; ++i)
            {
				//pm.SendMessage(String.Format("LoadPlayer() {0} / {1}.", i , items.Count));
                Item item = (Item)items[i];
                if (item.Layer == Layer.Invalid)
                {
					//pm.SendMessage(String.Format("Layer Invalid!"));
                    item.Location = (Point3D)ItemLocations[item.Serial];
					ItemLocations.Remove(item.Serial);
                    pack.AddItem(item);
                }
                else
                {
                    Item test = (Item)pm.FindItemOnLayer(item.Layer);

                    if (test == null || test == item)
                    {
						ItemLocations.Remove(item.Serial);
                        pm.EquipItem(item);
                        test = (Item)pm.FindItemOnLayer(item.Layer);
                        if ( test == null )
                        	pack.AddItem(item);
                    }
                    else
                    {
                        item.Location = (Point3D)ItemLocations[item.Serial];
						ItemLocations.Remove(item.Serial);
                        if (item != pm.Backpack && item != pm.BankBox)
                            pack.AddItem(item);
                    }
                }
            }
	    
        }

        public void ResurrectPlayer(PlayerMobile pm)
        {
            if (!pm.Alive)
                pm.Resurrect();

            pm.Hits = pm.HitsMax;
            pm.Stam = pm.StamMax;
            pm.Mana = pm.ManaMax;

            pm.Combatant = null;
            pm.Aggressed.Clear();
            pm.Aggressors.Clear();
            pm.Criminal = false;
        }
        
        public void ResurrectPets(PlayerMobile master)
        {
            if ( master.Mounted )
        	{
        		if ( master.Mount is BaseCreature )
        		{
        			BaseCreature mount = (BaseCreature)master.Mount;
        			
        			mount.Hits = mount.HitsMax;
        			mount.Stam = mount.StamMax;
        			mount.Mana = mount.ManaMax;
        			mount.Combatant = null;
            		mount.Aggressed.Clear();
            		mount.Aggressors.Clear();
           	 		mount.Criminal = false;
        		}
        	}
        
        	foreach ( Mobile m in master.GetMobilesInRange( 50 ) )
			{
				if ( m is BaseCreature )
				{
					BaseCreature pet = (BaseCreature)m;

					if ( pet.Controlled && pet.ControlMaster == master )
					{
           			 	if (pet.IsDeadPet)
                			pet.ResurrectPet();

            			pet.Hits = pet.HitsMax;
            			pet.Stam = pet.StamMax;
            			pet.Mana = pet.ManaMax;

            			pet.Combatant = null;
            			pet.Aggressed.Clear();
            			pet.Aggressors.Clear();
            			pet.Criminal = false;
					}
				}
			}
        }

        public void PlayerNoto(PlayerMobile pm)
        {
            pm.Delta(MobileDelta.Noto);
            pm.InvalidateProperties();
        }
        
        public void PetNoto(PlayerMobile master)
        {
        	if ( master.Mounted )
        	{
        		if ( master.Mount is BaseCreature )
        		{
        			BaseCreature mount = (BaseCreature)master.Mount;
        			mount.Delta(MobileDelta.Noto);
        			mount.InvalidateProperties();
        		}
        	}
        
        	foreach ( Mobile m in master.GetMobilesInRange( 50 ) )
			{
				if ( m is BaseCreature )
				{
					BaseCreature pet = (BaseCreature)m;

					if ( pet.Controlled && pet.ControlMaster == master )
					{
						pet.Delta(MobileDelta.Noto);
        				pet.InvalidateProperties();
					}
				}
			}
        }

		public virtual void SpawnTrees( RVS Handeling, Point3D spawn, Map s_map )
		{
			if ( Handeling.AllowTrees == false )
				return;
		
			int numberOfTrees = Handeling.NumberOfTrees;
			int kindOfTree = 0;
			Item tree = null;
			
			int randomValueX = 0;
			int randomValueY = 0;
			int negative = 0;
			
			for ( int i = 0; i < numberOfTrees; i++ )
			{
				kindOfTree = Utility.Random( 4 );
				
				randomValueX = Utility.Random( 20 );
				negative = Utility.Random( 2 );
				if ( negative > 0 )
					randomValueX *= -1;
					
				negative = Utility.Random( 2 );
				randomValueY = Utility.Random( 20 );
				if ( negative > 0 )
					randomValueY *= -1;
				
				switch ( kindOfTree )
				{
					case 0:
						{
							tree = new RVSTree1();
							break;
						}
					case 1:
						{
							tree = new RVSTree2();
							break;
						}
					case 2:
						{
							tree = new RVSTree3();
							break;
						}
					case 3:
						{
							tree = new RVSTree4();
							break;
						}
				}
				
				tree.Location = new Point3D((spawn.X + randomValueX), (spawn.Y + randomValueY), (spawn.Z));
				tree.Map = s_map;
				Trees.Add(tree);
			}
		}
		
		public virtual void DeleteTrees()
		{
			if( Trees == null )
				return;

			foreach (Item tree in Trees)
			{
				tree.Delete();
			}
			
			Trees.Clear();
		}

		public virtual void SpawnTraps( RVS Handeling, Point3D spawn, Map s_map )
		{
			if ( Handeling.AllowTraps == false )
				return;
		
			int numberOfTraps = Handeling.NumberOfTraps;
			int kindOfTrap = 0;
			Item trap = null;
			
			int randomValueX = 0;
			int randomValueY = 0;
			int negative = 0;
			
			for ( int i = 0; i < numberOfTraps; i++ )
			{
				kindOfTrap = Utility.Random( 5 );
				
				randomValueX = Utility.Random( 20 );
				negative = Utility.Random( 2 );
				if ( negative > 0 )
					randomValueX *= -1;
					
				negative = Utility.Random( 2 );
				randomValueY = Utility.Random( 20 );
				if ( negative > 0 )
					randomValueY *= -1;
				
				switch ( kindOfTrap )
				{
					case 0:
						{
							trap = new FireColumnTrap();
							break;
						}
					case 1:
						{
							trap = new GasTrap();
							break;
						}
					case 2:
						{
							trap = new GiantSpikeTrap();
							break;
						}
					case 3:
						{
							trap = new SawTrap();
							break;
						}
					case 4:
						{
							trap = new SpikeTrap();
							break;
						}
				}
				trap.Movable = false;
				trap.Location = new Point3D((spawn.X + randomValueX), (spawn.Y + randomValueY), (spawn.Z));
				trap.Map = s_map;
				Traps.Add(trap);
			}
		}

		public virtual void DeleteTraps()
		{
			if( Traps == null )
				return;

			foreach (Item trap in Traps)
			{
				trap.Delete();
			}
			
			Traps.Clear();
		}
		
		public virtual void SpawnEnemies( RVS Handeling, String RegionName, int amount)
		{
			int numberOfEnemies = amount;
			int kindOfEnemy = 0;
			int randomEnemy = 0;
			Mobile Enemy = null;
			//Mobile Enemy = new Sheep();
			
			int randomValueX = 0;
			int randomValueY = 0;
			int negative = 0;
			
			Point3D spawn;
			Map spawnmap;
			
			if ( RegionName == "Sheep" )
			{
				spawn = SheepSpawnPoint;
				spawnmap = SheepSpawnMap;
				kindOfEnemy = 0;
			}
			else if ( RegionName == "Rabbit" )
			{
				spawn = RabbitSpawnPoint;
				spawnmap = RabbitSpawnMap;
				kindOfEnemy = 1;
			}
			else
			{
				spawn = SheepSpawnPoint;
				spawnmap = SheepSpawnMap;
				kindOfEnemy = 0;
			}
			
			for ( int i = 0; i < numberOfEnemies; i++ )
			{
				//kindOfEnemy = Utility.Random( 5 );
				
				randomValueX = Utility.Random( 11 );
				negative = Utility.Random( 2 );
				if ( negative > 0 )
					randomValueX *= -1;
					
				negative = Utility.Random( 2 );
				randomValueY = Utility.Random( 11 );
				if ( negative > 0 )
					randomValueY *= -1;
				
				if ( Handeling.RvS )
				{ 
					switch ( kindOfEnemy )
					{
						case 0:
							{
								Enemy = new Sheep();
								break;
							}
						case 1:
							{
								Enemy = new Rabbit();
								break;
							}
					}
				}
				else if ( Handeling.Orcs )
				{ 
					kindOfEnemy = 0;
					randomEnemy = Utility.Random( 100 );
					if ( randomEnemy <= 20 )
						kindOfEnemy++;
					if ( randomEnemy <= 10 )
						kindOfEnemy++;
					if ( randomEnemy <= 5 )
						kindOfEnemy++;
					
					switch ( kindOfEnemy )
					{
						case 0:
							{
								Enemy = new Orc();
								break;
							}
						case 1:
							{
								Enemy = new OrcishLord();
								break;
							}
						case 2:
							{
								Enemy = new Ettin();
								break;
							}
						case 3:
							{
								Enemy = new OrcishMage();
								break;
							}
					}
				}
				else if ( Handeling.Lizardmen )
				{ 
					kindOfEnemy = 0;
					randomEnemy = Utility.Random( 100 );
					if ( randomEnemy <= 20 )
						kindOfEnemy++;
					if ( randomEnemy <= 10 )
						kindOfEnemy++;
					if ( randomEnemy <= 5 )
						kindOfEnemy++;
					switch ( kindOfEnemy )
					{
						case 0:
							{
								Enemy = new Snake();
								break;
							}
						case 1:
							{
								Enemy = new GiantSerpent();
								break;
							}
						case 2:
							{
								Enemy = new Lizardman();
								break;
							}
						case 3:
							{
								Enemy = new Drake();
								break;
							}
					}
				}
				else if ( Handeling.Ratmen )
				{ 
					kindOfEnemy = 0;
					randomEnemy = Utility.Random( 100 );
					if ( randomEnemy <= 20 )
						kindOfEnemy++;
					if ( randomEnemy <= 15 )
						kindOfEnemy++;
					if ( randomEnemy <= 10 )
						kindOfEnemy++;
					if ( randomEnemy <= 5 )
						kindOfEnemy++;
					switch ( kindOfEnemy )
					{
						case 0:
							{
								Enemy = new Sewerrat();
								break;
							}
						case 1:
							{
								Enemy = new GiantRat();
								break;
							}
						case 2:
							{
								Enemy = new Ratman();
								break;
							}
						case 3:
							{
								Enemy = new RatmanMage();
								break;
							}	
						case 4:
							{
								Enemy = new RatmanArcher();
								break;
							}
					}
				}
				else if ( Handeling.Undead )
				{ 
					kindOfEnemy = 0;
					randomEnemy = Utility.Random( 100 );
					if ( randomEnemy <= 50 )
						kindOfEnemy++;
					if ( randomEnemy <= 40 )
						kindOfEnemy++;
					if ( randomEnemy <= 30 )
						kindOfEnemy++;
					if ( randomEnemy <= 20 )
						kindOfEnemy++;
					if ( randomEnemy <= 10 )
						kindOfEnemy++;
					if ( randomEnemy <= 5 )
						kindOfEnemy++;
						
					switch ( kindOfEnemy )
					{
						case 0:
							{
								Enemy = new Skeleton();
								break;
							}
						case 1:
							{
								Enemy = new Zombie();
								break;
							}
						case 2:
							{
								Enemy = new RestlessSoul();
								break;
							}
						case 3:
							{
								Enemy = new SkeletalMage();
								break;
							}
						case 4:
							{
								Enemy = new SkeletalKnight();
								break;
							}
						case 5:
							{
								Enemy = new Mummy();
								break;
							}
						case 6:
							{
								Enemy = new Lich();
								break;
							}
					}
				}
				
				Enemy.Map = spawnmap;
				Enemy.Location = new Point3D((spawn.X + randomValueX), (spawn.Y + randomValueY), (spawn.Z));
				
				Enemies.Add(Enemy);
			}
		}

		public virtual void DeleteEnemies()
		{
			if( Enemies == null )
				return;

			foreach (Mobile enemy in Enemies)
			{
				enemy.Delete();
			}
			
			Enemies.Clear();
		}
		
		public virtual void SpawnFountainsRabbits( RVS Handeling, String RegionName, Point3D spawn, Map s_map )
		{
			Item HealthFountain = new StoneFountainAddon();
			Item ManaFountain = new BrownStoneFountainAddon();
			HealthFlies HealthFlies = new HealthFlies();
			ManaFlies ManaFlies = new ManaFlies();
			
			HealthFountain.Map = s_map;
			HealthFountain.Location = new Point3D( (spawn.X - 13),(spawn.Y + 13), spawn.Z );
			ManaFountain.Map = s_map;
			ManaFountain.Location = new Point3D( (spawn.X + 12),(spawn.Y - 14), spawn.Z );
			
			HealthFlies.Name = RegionName;
			HealthFlies.rvsc = this;
			HealthFlies.Enabled = true;
			HealthFlies.Map = s_map;
			HealthFlies.Location = new Point3D( (spawn.X - 13),(spawn.Y + 13), (spawn.Z + 15) );
			
			ManaFlies.Name = RegionName;
			ManaFlies.rvsc = this;
			ManaFlies.Enabled = true;
			ManaFlies.Map = s_map;
			ManaFlies.Location  = new Point3D( (spawn.X + 13),(spawn.Y - 13), (spawn.Z + 15) );
			
			Fountains.Add( HealthFountain );
			Fountains.Add( ManaFountain );
			Fountains.Add( HealthFlies );
			Fountains.Add( ManaFlies );
		}
		
		public virtual void SpawnFountainsSheep( RVS Handeling, String RegionName, Point3D spawn, Map s_map )
		{
			Item HealthFountain = new StoneFountainAddon();
			Item ManaFountain = new BrownStoneFountainAddon();
			HealthFlies2 HealthFlies = new HealthFlies2();
			ManaFlies2 ManaFlies = new ManaFlies2();
			
			HealthFountain.Map = s_map;
			HealthFountain.Location = new Point3D( (spawn.X - 13),(spawn.Y + 13), spawn.Z );
			ManaFountain.Map = s_map;
			ManaFountain.Location = new Point3D( (spawn.X + 12),(spawn.Y - 14), spawn.Z );
			
			HealthFlies.Name = RegionName;
			HealthFlies.rvsc = this;
			HealthFlies.Enabled = true;
			HealthFlies.Map = s_map;
			HealthFlies.Location = new Point3D( (spawn.X - 13),(spawn.Y + 13), (spawn.Z + 15) );
			
			ManaFlies.Name = RegionName;
			ManaFlies.rvsc = this;
			ManaFlies.Enabled = true;
			ManaFlies.Map = s_map;
			ManaFlies.Location  = new Point3D( (spawn.X + 13),(spawn.Y - 13), (spawn.Z + 15) );
			
			Fountains.Add( HealthFountain );
			Fountains.Add( ManaFountain );
			Fountains.Add( HealthFlies );
			Fountains.Add( ManaFlies );
		}
		
		public virtual void DeleteFountains()
		{
			if( Fountains == null )
				return;

			foreach (Item fountain in Fountains)
			{
				fountain.Delete();
			}
			
			Fountains.Clear();
		}
		
		public virtual void EVsAndBSsAdd(Mobile m)
		{
			EVsAndBSs.Add(m);
		}
		
		public virtual void DeleteEVsAndBSs()
        {
        	if ( EVsAndBSs == null )
        		return;
        		
        	foreach (Mobile bs in EVsAndBSs)
			{
				bs.Delete();
			}
			
			EVsAndBSs.Clear();
        }

        public virtual void SpawnPlayer(PlayerMobile pm, int team)
        {
            switch (team)
            {
                case 1:
                    {
                    	TeleportAllPets( pm, new Point3D((SheepSpawnPoint.X), (SheepSpawnPoint.Y), (SheepSpawnPoint.Z)), SheepSpawnMap );
                        pm.Location = new Point3D((SheepSpawnPoint.X), (SheepSpawnPoint.Y), (SheepSpawnPoint.Z));
                        pm.Map = SheepSpawnMap;
                       
                        break;
                    }
                case 2:
                    {
                    	TeleportAllPets( pm, new Point3D((RabbitSpawnPoint.X), (RabbitSpawnPoint.Y), (RabbitSpawnPoint.Z)), RabbitSpawnMap );
                        pm.Location = new Point3D((RabbitSpawnPoint.X), (RabbitSpawnPoint.Y), (RabbitSpawnPoint.Z));
                        pm.Map = RabbitSpawnMap;
                        
                        break;
                    }
            }
        }
        
        public static void TeleportAllPets( Mobile master, Point3D loc, Map map )
		{
			List<Mobile> move = new List<Mobile>();

			foreach ( Mobile m in master.GetMobilesInRange( 50 ) )
			{
				if ( m is BaseCreature )
				{
					BaseCreature pet = (BaseCreature)m;

					if ( pet.Controlled && pet.ControlMaster == master )
					{
						move.Add( pet );
					}
				}
			}

			foreach ( Mobile m in move )
			{
				m.MoveToWorld( loc, map );
			}
		}
        
        public RVSController(Serial serial)
            : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
            writer.Write((Map)SheepRegionMap);
            writer.Write((Rectangle2D)SheepRegionPoint);
            writer.Write((Map)RabbitRegionMap);
            writer.Write((Rectangle2D)RabbitRegionPoint);
            writer.Write((bool)abled);
            writer.Write((Point3D)SheepSpawnPoint);
            writer.Write((Point3D)RabbitSpawnPoint);
            writer.Write((Map)SheepSpawnMap);
            writer.Write((Map)RabbitSpawnMap);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
            SheepRegionMap = reader.ReadMap();
            SheepRegionPoint = reader.ReadRect2D();
            RabbitRegionMap = reader.ReadMap();
            RabbitRegionPoint = reader.ReadRect2D();
            abled = reader.ReadBool();
            SheepSpawnPoint = reader.ReadPoint3D();
            RabbitSpawnPoint = reader.ReadPoint3D();
            SheepSpawnMap = reader.ReadMap();
            RabbitSpawnMap = reader.ReadMap();
        }
    }

    public class RVS_EndTimer : Timer
    {
        public RVS Handeling;

        public RVS_EndTimer(RVS d)
            : base(TimeSpan.FromMinutes(45.0))
        {
            Handeling = d;
            this.Start();
        }

        protected override void OnTick()
        {
            if (Handeling.InProgress && !Handeling.Ended)
                Handeling.EndSelf();

            this.Stop();
        }
    }
}