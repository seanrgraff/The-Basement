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

namespace Server.Fielding
{
    public class FieldController : Item
    {
    	public virtual string GameName{ get{ return "Fielding"; } }
    
        protected Map regionmap;
        protected Rectangle2D regionpoint;
        [CommandProperty(AccessLevel.GameMaster)]
        public Map ThisRegionMap
        {
            get { return regionmap; }
            set { regionmap = value; }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public Rectangle2D ThisRegionPoint
        {
            get { return regionpoint; }
            set { regionpoint = value; }
        }

        protected Map spawnmap;
        protected Point3D spawnpoint;

        [CommandProperty(AccessLevel.GameMaster)]
        public Map SpawnMap
        {
            get { return spawnmap; }
            set { spawnmap = value; }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public Point3D SpawnPoint
        {
            get { return spawnpoint; }
            set { spawnpoint = value; }
        }

        protected bool abled;
        [CommandProperty(AccessLevel.GameMaster)]
        public bool Enabled
        {
            get { return abled; }
            set { abled = value; }
        }

        public Field Handeling;
        public FieldRegion ThisRegion;
        public bool InUse;
        public bool HasStarted;
        public bool AllKilled = false;

        public Field_EndTimer EndTimer;

        public static Dictionary<Serial, Point3D> PlayerLocations;
        public static Dictionary<Serial, Map> PlayerMaps;
        public static Dictionary<Serial, ArrayList> PlayerItems;
        public static Dictionary<Serial, Point3D> ItemLocations;
        public static ArrayList Trees;
        public static ArrayList Traps;
        public static ArrayList InsuredItems;
        public static ArrayList FrozenItems;

        [Constructable]
        public FieldController()
            : base(0xEDE)
        {
            Name = "Field Controller";
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
            for (int i = 0; i < Field_Config.Controllers.Count; ++i)
            {
                FieldController d = (FieldController)Field_Config.Controllers[i];

                if (d.HasStarted && d.Handeling != null)
                    d.EndField(0);
            }
        }

        public static int Handle_Notoriety(Mobile from, Mobile target)
        {
        	//BaseCreature targetPet = null;
        	//BaseCreature fromPet = null;
            if (from is PlayerMobile && target is PlayerMobile)
            {
                if (Field_Config.InAField((PlayerMobile)from) && Field_Config.InAField((PlayerMobile)target))
                {
                    if (!Field_Config.FieldStarted((PlayerMobile)from))
                        return NotorietyHandlers.MobileNotoriety2(from, target);

                    if (Field_Config.IsAlly((PlayerMobile)from, (PlayerMobile)target))
                        return Notoriety.Ally;
                    if (Field_Config.IsEnemy((PlayerMobile)from, (PlayerMobile)target))
                        return Notoriety.Enemy;
                }
                else if (Field_Config.InAField((PlayerMobile)from) || Field_Config.InAField((PlayerMobile)target))
                    return NotorietyHandlers.MobileNotoriety2(from, target);
            }
            else if (from is PlayerMobile && target is BaseCreature)
            {
            	BaseCreature targetPet = (BaseCreature)target;
            	if (Field_Config.InAField((PlayerMobile) from) && Field_Config.InAField((PlayerMobile)targetPet.ControlMaster) && targetPet.Controlled)
            	{
            		if (!Field_Config.FieldStarted((PlayerMobile)from))
                        return NotorietyHandlers.MobileNotoriety2(from, target);

                    if (Field_Config.IsAlly((PlayerMobile)from, (PlayerMobile)targetPet.ControlMaster))
                        return Notoriety.Ally;
                    if (Field_Config.IsEnemy((PlayerMobile)from, (PlayerMobile)targetPet.ControlMaster))
                        return Notoriety.Enemy;
            	}
            }
            else if (from is BaseCreature && target is PlayerMobile)
            {
            	BaseCreature fromPet = (BaseCreature)from;
            	if (Field_Config.InAField((PlayerMobile)fromPet.ControlMaster) && Field_Config.InAField((PlayerMobile)target) && fromPet.Controlled)
            	{
            		if (!Field_Config.FieldStarted((PlayerMobile)fromPet.ControlMaster))
                        return NotorietyHandlers.MobileNotoriety2(from, target);

                    if (Field_Config.IsAlly((PlayerMobile)fromPet.ControlMaster, (PlayerMobile)target))
                        return Notoriety.Ally;
                    if (Field_Config.IsEnemy((PlayerMobile)fromPet.ControlMaster, (PlayerMobile)target))
                        return Notoriety.Enemy;
            	}
            }
           	else if (from is BaseCreature && target is BaseCreature)
            {
            	BaseCreature fromPet = (BaseCreature)from;
            	BaseCreature targetPet = (BaseCreature)target;
            	if (Field_Config.InAField((PlayerMobile)fromPet.ControlMaster) && Field_Config.InAField((PlayerMobile)targetPet.ControlMaster) && fromPet.Controlled && targetPet.Controlled)
            	{
            		if (!Field_Config.FieldStarted((PlayerMobile)fromPet.ControlMaster))
                        return NotorietyHandlers.MobileNotoriety2(from, target);

                    if (Field_Config.IsAlly((PlayerMobile)fromPet.ControlMaster, (PlayerMobile)targetPet.ControlMaster))
                        return Notoriety.Ally;
                    if (Field_Config.IsEnemy((PlayerMobile)fromPet.ControlMaster, (PlayerMobile)targetPet.ControlMaster))
                        return Notoriety.Enemy;
            	}
            }

            return NotorietyHandlers.MobileNotoriety2(from, target);
        }

        public void StartField(Field d)
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
                
            SpawnTrees(Handeling);
            SpawnTraps(Handeling);

            IEnumerator key = Handeling.Teams.Keys.GetEnumerator();
            for (int i = 0; i < Handeling.Teams.Count; ++i)
            {
                key.MoveNext();
                Field_Team d_team = (Field_Team)Handeling.Teams[(int)key.Current];

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
            EndTimer = new Field_EndTimer(Handeling);
        }

        public void UnInvul()
        {
            if (Handeling == null)
                return;
            if (Handeling.Ended)
                EndField(0);

            IEnumerator key = Handeling.Teams.Keys.GetEnumerator();
            for (int i = 0; i < Handeling.Teams.Count; ++i)
            {
                key.MoveNext();
                Field_Team d_team = (Field_Team)Handeling.Teams[(int)key.Current];

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

        public void EndField(int teamid)
        {
            if (Handeling == null)
                return;

            if (EndTimer != null)
                EndTimer.Stop();

            Handeling.EchoMessage(String.Format("The field duel has ended, team {0} has won!", teamid.ToString()));

            HasStarted = false;
            InUse = false;
            Handeling.InProgress = false;

            if (!Handeling.Ended)
            {
                IEnumerator key = Handeling.Teams.Keys.GetEnumerator();
                for (int i = 0; i < Handeling.Teams.Count; ++i)
                {
                    key.MoveNext();
                    Field_Team d_team = (Field_Team)Handeling.Teams[(int)key.Current];

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
                Field_Team d_team = (Field_Team)Handeling.Teams[(int)key.Current];

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

            if (!Handeling.InField(died))
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
                Field_Team d_team = (Field_Team)Handeling.Teams[(int)key.Current];

                if(!d_team.AllDead())
                {
                    alive += 1;
                    teamid = (int)key.Current;
                }
            }

            if (alive == 1 || alive == 0)
            {
                AllKilled = true;
                EndField(teamid);
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
                Field_Team d_team = (Field_Team)Handeling.Teams[(int)key.Current];

                for (int i2 = 0; i2 < d_team.Players.Count; ++i2)
                {
                    object o = (object)d_team.Players[i2];

                    if (o is PlayerMobile)
                    {
                        PlayerMobile pm2 = (PlayerMobile)o;

                        if ((int)key.Current != teamid)
                            heads.Add(new FieldHead(pm2, pm));
                    }
                }
            }

            for (int i = 0; i < heads.Count; ++i)
            {
                FieldHead head = (FieldHead)heads[i];
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

		public virtual void SpawnTrees( Field Handeling )
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
							tree = new FieldTree1();
							break;
						}
					case 1:
						{
							tree = new FieldTree2();
							break;
						}
					case 2:
						{
							tree = new FieldTree3();
							break;
						}
					case 3:
						{
							tree = new FieldTree4();
							break;
						}
				}
				
				tree.Location = new Point3D((spawnpoint.X + randomValueX), (spawnpoint.Y + randomValueY), (spawnpoint.Z));
				tree.Map = spawnmap;
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

		public virtual void SpawnTraps( Field Handeling )
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
				trap.Location = new Point3D((spawnpoint.X + randomValueX), (spawnpoint.Y + randomValueY), (spawnpoint.Z));
				trap.Map = spawnmap;
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

        public virtual void SpawnPlayer(PlayerMobile pm, int team)
        {
            switch (team)
            {
                case 1:
                    {
                    	TeleportAllPets( pm, new Point3D((spawnpoint.X - 13), (spawnpoint.Y - 13), (spawnpoint.Z)), spawnmap );
                        pm.Location = new Point3D((spawnpoint.X - 13), (spawnpoint.Y - 13), (spawnpoint.Z));
                        pm.Map = spawnmap;
                       
                        break;
                    }
                case 2:
                    {
                    	TeleportAllPets( pm, new Point3D((spawnpoint.X + 13), (spawnpoint.Y + 13), (spawnpoint.Z)), spawnmap );
                        pm.Location = new Point3D((spawnpoint.X + 13), (spawnpoint.Y + 13), (spawnpoint.Z));
                        pm.Map = spawnmap;
                        
                        break;
                    }
                case 3:
                    {
                    	TeleportAllPets( pm, new Point3D((spawnpoint.X + 13), (spawnpoint.Y - 13), (spawnpoint.Z)), spawnmap );
                        pm.Location = new Point3D((spawnpoint.X + 13), (spawnpoint.Y - 13), (spawnpoint.Z));
                        pm.Map = spawnmap;
                        
                        break;
                    }
                case 4:
                    {
                    	TeleportAllPets( pm, new Point3D((spawnpoint.X - 13), (spawnpoint.Y + 13), (spawnpoint.Z)), spawnmap );
                        pm.Location = new Point3D((spawnpoint.X - 13), (spawnpoint.Y + 13), (spawnpoint.Z));
                        pm.Map = spawnmap;
                        
                        break;
                    }
                case 5:
                    {
                    	TeleportAllPets( pm, new Point3D((spawnpoint.X), (spawnpoint.Y + 10), (spawnpoint.Z)), spawnmap );
                        pm.Location = new Point3D((spawnpoint.X), (spawnpoint.Y + 10), (spawnpoint.Z));
                        pm.Map = spawnmap;
                        
                        break;
                    }
                case 6:
                    {
                    	TeleportAllPets( pm, new Point3D((spawnpoint.X - 10), (spawnpoint.Y), (spawnpoint.Z)), spawnmap );
                        pm.Location = new Point3D((spawnpoint.X - 10), (spawnpoint.Y), (spawnpoint.Z));
                        pm.Map = spawnmap;
                        break;
                    }
                case 7:
                    {
                    	TeleportAllPets( pm, new Point3D((spawnpoint.X), (spawnpoint.Y - 10), (spawnpoint.Z)), spawnmap );
                        pm.Location = new Point3D((spawnpoint.X), (spawnpoint.Y - 10), (spawnpoint.Z));
                        pm.Map = spawnmap;
                        break;
                    }
                case 8:
                    {
                    	TeleportAllPets( pm, new Point3D((spawnpoint.X + 10), (spawnpoint.Y), (spawnpoint.Z)), spawnmap );
                        pm.Location = new Point3D((spawnpoint.X + 10), (spawnpoint.Y), (spawnpoint.Z));
                        pm.Map = spawnmap;
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
        
        public FieldController(Serial serial)
            : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
            writer.Write((Map)regionmap);
            writer.Write((Rectangle2D)regionpoint);
            writer.Write((bool)abled);
            writer.Write((Point3D)spawnpoint);
            writer.Write((Map)spawnmap);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
            regionmap = reader.ReadMap();
            regionpoint = reader.ReadRect2D();
            abled = reader.ReadBool();
            spawnpoint = reader.ReadPoint3D();
            spawnmap = reader.ReadMap();
        }
    }

    public class Field_EndTimer : Timer
    {
        public Field Handeling;

        public Field_EndTimer(Field d)
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