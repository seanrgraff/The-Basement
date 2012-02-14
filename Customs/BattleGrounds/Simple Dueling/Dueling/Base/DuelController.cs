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

namespace Server.Dueling
{
    public class DuelController : Item
    {
    	public virtual string GameName{ get{ return "Dueling"; } }
    
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

        public Duel Handeling;
        public DuelRegion ThisRegion;
        public bool InUse;
        public bool HasStarted;
        public bool AllKilled = false;

        public Duel_EndTimer EndTimer;

        public static Dictionary<Serial, Point3D> PlayerLocations;
        public static Dictionary<Serial, Map> PlayerMaps;
        public static Dictionary<Serial, ArrayList> PlayerItems;
        public static Dictionary<Serial, Point3D> ItemLocations;
        public static ArrayList InsuredItems;
        public static ArrayList FrozenItems;

        [Constructable]
        public DuelController()
            : base(0xEDD)
        {
            Name = "Duel Controller";
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
            for (int i = 0; i < Duel_Config.Controllers.Count; ++i)
            {
                DuelController d = (DuelController)Duel_Config.Controllers[i];

                if (d.HasStarted && d.Handeling != null)
                    d.EndDuel(0);
            }
        }

        public static int Handle_Notoriety(Mobile from, Mobile target)
        {
            if (from is PlayerMobile && target is PlayerMobile)
            {
                if (Duel_Config.InADuel((PlayerMobile)from) && Duel_Config.InADuel((PlayerMobile)target))
                {
                    if (!Duel_Config.DuelStarted((PlayerMobile)from))
                        return NotorietyHandlers.MobileNotoriety2(from, target);

                    if (Duel_Config.IsAlly((PlayerMobile)from, (PlayerMobile)target))
                        return Notoriety.Ally;
                    if (Duel_Config.IsEnemy((PlayerMobile)from, (PlayerMobile)target))
                        return Notoriety.Enemy;
                }
                else if (Duel_Config.InADuel((PlayerMobile)from) || Duel_Config.InADuel((PlayerMobile)target))
                    return NotorietyHandlers.MobileNotoriety2(from, target);
            }

            return NotorietyHandlers.MobileNotoriety2(from, target);
        }

        public void StartDuel(Duel d)
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
                        StorePlayer(pm);
                        SpawnPlayer(pm, (int)key.Current);
                        PlayerNoto(pm);
                        pm.Blessed = true;
                        pm.Frozen = true;
                        pm.Spell = null;
                        ResurrectPlayer(pm);
                    }
                }
            }

            Timer.DelayCall(TimeSpan.FromSeconds(3.0), new TimerCallback(UnInvul));
            EndTimer = new Duel_EndTimer(Handeling);
        }

        public void UnInvul()
        {
            if (Handeling == null)
                return;
            if (Handeling.Ended)
                EndDuel(0);

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
                        pm.Blessed = false;
                        pm.Frozen = false;
                    }
                }
            }

            if(HasStarted)
                Handeling.EchoMessage("You can now fight!");
        }

        public void EndDuel(int teamid)
        {
            if (Handeling == null)
                return;

            if (EndTimer != null)
                EndTimer.Stop();

            Handeling.EchoMessage(String.Format("The duel has ended, team {0} has won!", teamid.ToString()));

            HasStarted = false;
            InUse = false;
            Handeling.InProgress = false;

            if (!Handeling.Ended)
            {
                IEnumerator key = Handeling.Teams.Keys.GetEnumerator();
                for (int i = 0; i < Handeling.Teams.Count; ++i)
                {
                    key.MoveNext();
                    Duel_Team d_team = (Duel_Team)Handeling.Teams[(int)key.Current];

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
								LoadPlayer(pm);
								PlayerNoto(pm);
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

            InsuredItems = null;
            FrozenItems = null;

            Handeling.UpdateAllPending();
            Handeling.BuyIn = 0;
            Handeling.IsRematch = true;
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
                Duel_Team d_team = (Duel_Team)Handeling.Teams[(int)key.Current];

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

            if (!Handeling.InDuel(died))
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
                Duel_Team d_team = (Duel_Team)Handeling.Teams[(int)key.Current];

                if(!d_team.AllDead())
                {
                    alive += 1;
                    teamid = (int)key.Current;
                }
            }

            if (alive == 1 || alive == 0)
            {
                AllKilled = true;
                EndDuel(teamid);
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
                Duel_Team d_team = (Duel_Team)Handeling.Teams[(int)key.Current];

                for (int i2 = 0; i2 < d_team.Players.Count; ++i2)
                {
                    object o = (object)d_team.Players[i2];

                    if (o is PlayerMobile)
                    {
                        PlayerMobile pm2 = (PlayerMobile)o;

                        if ((int)key.Current != teamid)
                            heads.Add(new DuelHead(pm2, pm));
                    }
                }
            }

            for (int i = 0; i < heads.Count; ++i)
            {
                DuelHead head = (DuelHead)heads[i];
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

        public void PlayerNoto(PlayerMobile pm)
        {
            pm.Delta(MobileDelta.Noto);
            pm.InvalidateProperties();
        }

        public virtual void SpawnPlayer(PlayerMobile pm, int team)
        {
            switch (team)
            {
                case 1:
                    {
                        pm.Location = new Point3D((spawnpoint.X - 2), (spawnpoint.Y - 2), (spawnpoint.Z));
                        pm.Map = spawnmap;
                        break;
                    }
                case 2:
                    {
                        pm.Location = new Point3D((spawnpoint.X + 2), (spawnpoint.Y + 2), (spawnpoint.Z));
                        pm.Map = spawnmap;
                        break;
                    }
                case 3:
                    {
                        pm.Location = new Point3D((spawnpoint.X - 2), (spawnpoint.Y + 2), (spawnpoint.Z));
                        pm.Map = spawnmap;
                        break;
                    }
                case 4:
                    {
                        pm.Location = new Point3D((spawnpoint.X + 2), (spawnpoint.Y - 2), (spawnpoint.Z));
                        pm.Map = spawnmap;
                        break;
                    }
            }
        }

        public DuelController(Serial serial)
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

    public class Duel_EndTimer : Timer
    {
        public Duel Handeling;

        public Duel_EndTimer(Duel d)
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