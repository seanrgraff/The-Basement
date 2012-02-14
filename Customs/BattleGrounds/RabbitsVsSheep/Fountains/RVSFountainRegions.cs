using System;
using Server;
using System.Collections;
using Server.Targeting;
using Server.Items;
using Server.Mobiles;
using Server.Gumps;
using System.Collections.Generic;
using System.IO;
using Server.Regions;
using Server.Commands;
using Server.Misc;
using Server.Spells;
using Server.Spells.Necromancy;
using Server.Spells.Bushido;
using Server.Spells.Chivalry;
using Server.Spells.Ninjitsu;
using Server.Spells.First;
using Server.Spells.Second;
using Server.Spells.Third;
using Server.Spells.Fourth;
using Server.Spells.Fifth;
using Server.Spells.Sixth;
using Server.Spells.Seventh;
using Server.Spells.Eighth;

namespace Server.RabbitsVsSheep
{
    public class ManaRegenRegion : BaseRegion
    {
    	public String RegionName;
    
		public static ArrayList PlayerTimers = new ArrayList();
        public RVSController Controller;
        public ManaFlies Flies;

        public ManaRegenRegion(ManaFlies mf, RVSController rvsc, String name)
            : base(String.Format("RVSManaRegion{0}", name), mf.ThisRegionMap, 100, mf.ThisRegionPoint)
        {
        	RegionName = name;
            Controller = rvsc;
            Flies = mf;
            this.Register();
        }

        public override void OnEnter(Mobile m)
        {
        	if (!Controller.InUse)
                return;
                
			m.SendMessage("Your mana is being regenerated faster!");
			
            if (m is PlayerMobile) 
            { 
            	HealManaTimer timer = new HealManaTimer( m );
            	PlayerTimers.Add(timer);
          		timer.Start();
            }

            m.Delta(MobileDelta.Noto);
            m.InvalidateProperties();
        }
        
        public override void OnExit(Mobile m)
        {
            m.SendMessage("Your mana regeneration has returned to normal.", this.Name);

			if (m is PlayerMobile) 
            { 
            	for ( int i = 0; i < PlayerTimers.Count; ++i )
            	{
            		if ( PlayerTimers[i] != null && ((HealManaTimer)PlayerTimers[i]).m == m )
            		{
            			HealManaTimer timer = (HealManaTimer)PlayerTimers[i];
          				timer.Stop();
            			PlayerTimers[i] = null;
            		}
            	}
            }
            int count = 0;
            for ( int j = 0; j < PlayerTimers.Count; ++j )
            {
            	if ( PlayerTimers[j] == null )
            	{
					count++;
            	}
            }
            if ( count == PlayerTimers.Count )
            {
            	PlayerTimers.Clear();
            }
        }
        
        public override bool AllowBeneficial(Mobile from, Mobile target)
        {
            if (!Controller.InUse)
                return false;

            if (from is PlayerMobile && target is PlayerMobile) 
            { 
                if (Controller.Handeling.IsAlly((PlayerMobile)from, (PlayerMobile)target))
                return true;

            	if (Controller.Handeling.IsEnemy((PlayerMobile)from, (PlayerMobile)target))
                return false;

            	if (!Controller.Handeling.InRVS((PlayerMobile)from) || !Controller.Handeling.InRVS((PlayerMobile)target))
                return false;
                
                return base.AllowBeneficial(from, target); 
            }
            else if ( from is PlayerMobile && target is BaseCreature) 
            {
            	BaseCreature targetPet = (BaseCreature) target;
                if ( !targetPet.Controlled )
                	return false;
            	
            	if (Controller.Handeling.IsAlly((PlayerMobile)from, (PlayerMobile)targetPet.ControlMaster))
                	return true;

            	if (Controller.Handeling.IsEnemy((PlayerMobile)from, (PlayerMobile)targetPet.ControlMaster))
                	return false;

            	if (!Controller.Handeling.InRVS((PlayerMobile)from) || !Controller.Handeling.InRVS((PlayerMobile)targetPet.ControlMaster))
                	return false;

            	return base.AllowBeneficial(from, target); 
            }
            else if ( from is BaseCreature && target is PlayerMobile) 
            {
            	BaseCreature fromPet = (BaseCreature)from;
                if ( !fromPet.Controlled )
                	return false;

                if (Controller.Handeling.IsAlly((PlayerMobile)fromPet.ControlMaster, (PlayerMobile)target))
                	return true;

            	if (Controller.Handeling.IsEnemy((PlayerMobile)fromPet.ControlMaster, (PlayerMobile)target))
                	return false;

            	if (!Controller.Handeling.InRVS((PlayerMobile)fromPet.ControlMaster) || !Controller.Handeling.InRVS((PlayerMobile)target))
                	return false;

            	return base.AllowBeneficial(from, target);
			}
            else if ( from is BaseCreature && target is BaseCreature) 
            {
                BaseCreature fromPet = (BaseCreature)from;
                BaseCreature targetPet = (BaseCreature)target;
                if ( !fromPet.Controlled && !targetPet.Controlled )
                	return false;
                
                if (Controller.Handeling.IsAlly((PlayerMobile)fromPet.ControlMaster, (PlayerMobile)targetPet.ControlMaster))
                	return true;

            	if (Controller.Handeling.IsEnemy((PlayerMobile)fromPet.ControlMaster, (PlayerMobile)targetPet.ControlMaster))
                	return false;

            	if (!Controller.Handeling.InRVS((PlayerMobile)fromPet.ControlMaster) || !Controller.Handeling.InRVS((PlayerMobile)targetPet.ControlMaster))
                	return false;

            	return base.AllowBeneficial(from, target); 
            }
            else
                return false;
        }

        public override bool AllowHarmful(Mobile from, Mobile target)
        {
            if (!Controller.InUse)
                return false;

            if (from is PlayerMobile && target is PlayerMobile)
            {
            	if (Controller.Handeling.IsAlly((PlayerMobile)from, (PlayerMobile)target))
                	return false;

            	if (Controller.Handeling.IsEnemy((PlayerMobile)from, (PlayerMobile)target))
                	return true;

            	if (!Controller.Handeling.InRVS((PlayerMobile)from) || !Controller.Handeling.InRVS((PlayerMobile)target))
                	return false;

            	return base.AllowHarmful(from, target);
            }
            else if ( from is PlayerMobile && target is BaseCreature) 
            {
            	BaseCreature targetPet = (BaseCreature) target;

            	if (targetPet.Controlled && Controller.Handeling.IsAlly((PlayerMobile)from, (PlayerMobile)targetPet.ControlMaster))
                	return false;

            	if (targetPet.Controlled && Controller.Handeling.IsEnemy((PlayerMobile)from, (PlayerMobile)targetPet.ControlMaster))
                	return true;

            	return base.AllowHarmful(from, target); 
            }
            else if ( from is BaseCreature && target is PlayerMobile) 
            {
            	BaseCreature fromPet = (BaseCreature)from;

                if (fromPet.Controlled && Controller.Handeling.IsAlly((PlayerMobile)fromPet.ControlMaster, (PlayerMobile)target))
                	return false;

            	if (fromPet.Controlled && Controller.Handeling.IsEnemy((PlayerMobile)fromPet.ControlMaster, (PlayerMobile)target))
                	return true;

            	return base.AllowHarmful(from, target);
			}
            else if ( from is BaseCreature && target is BaseCreature) 
            {
                BaseCreature fromPet = (BaseCreature)from;
                BaseCreature targetPet = (BaseCreature)target;
                
                if (fromPet.Controlled && targetPet.Controlled && Controller.Handeling.IsAlly((PlayerMobile)fromPet.ControlMaster, (PlayerMobile)targetPet.ControlMaster))
                	return false;

            	if (fromPet.Controlled && targetPet.Controlled && Controller.Handeling.IsEnemy((PlayerMobile)fromPet.ControlMaster, (PlayerMobile)targetPet.ControlMaster))
                	return true;

            	return base.AllowHarmful(from, target); 
            }
            else
                return false;
        }

        public override bool AllowHousing(Mobile from, Point3D p)
        {
            return false;
        }

        public override void OnDeath(Mobile m)
        {
            if (!Controller.InUse)
                return;

            if (m is PlayerMobile)
            {
	    		m.SendMessage("You have died in the RVS Duel!!");
	    		for ( int x = 0; x < 5; x++ )
	    		{
					Controller.LaunchFireworks(m);
	    		}
            	Timer.DelayCall(TimeSpan.FromSeconds(5.0), new TimerStateCallback(Controller.HandleDeath), (object)m);
	   		}
	   		else if (m is BaseCreature)//CHANGE THIS WHEN APPROPRIATE
	   		{
	   			if ( this.RegionName == "Sheep" )
	   			{
	   				Controller.SpawnEnemies( Controller.Handeling, "Rabbit", 2);
	   			}
	   			else if ( this.RegionName == "Rabbit" )
	   			{
	   				Controller.SpawnEnemies( Controller.Handeling, "Sheep", 2);
	   			}
	   		}
	   		else
	   		{
	   			m.Corpse.Delete();
	   		}
        }
        
        public override bool OnDoubleClick(Mobile m, object o)
        {
            if (!Controller.InUse)
                return false;

            if (o is Item) { }
            else
                return base.OnDoubleClick(m, o);

            Item item = (Item)o;

            if (item is Corpse)
            {
                m.SendMessage("You may not loot here.");
                return false;
            }

            if (item is BasePotion && !Controller.Handeling.Potions)
            {
                m.SendMessage("The use of potions is not allowed.");
                return false;
            }

            if (item is Bandage && !Controller.Handeling.Bandages)
            {
                m.SendMessage("The use of bandages is not allowed.");
                return false;
            }

            if (item is TrapableContainer && !Controller.Handeling.TrappedContainers)
            {
                TrapableContainer cont = (TrapableContainer)item;
                if (cont.TrapType != TrapType.None)
                {
                    m.SendMessage("The use of trapped containers is not allowed.");
                    return false;
                }
            }
            
            if (item is BaseWand && !Controller.Handeling.Wands)
            {
            	m.SendMessage("The use of Magic Wands is not allowed.");
            	return false;
            }

            if (item is Bola && !Controller.Handeling.Bolas)
            {
                m.SendMessage("The use of bolas is not allowed.");
                return false;
            }

            if (item is EtherealMount && !Controller.Handeling.Mounts)
            {
                m.SendMessage("The use of mounts is not allowed.");
                return false;
            }
            
            if (item is SkillBall || item is StatBall)
            {
                m.SendMessage("The use of skill/stat balls are not allowed here.");
                return false;
            }

            return base.OnDoubleClick(m, o);
        }
	}
	
	public class ManaRegenRegion2 : BaseRegion
    {
    	public String RegionName;
    
		public static ArrayList PlayerTimers = new ArrayList();
        public RVSController Controller;
        public ManaFlies2 Flies;

        public ManaRegenRegion2(ManaFlies2 mf, RVSController rvsc, String name)
            : base(String.Format("RVSManaRegion{0}", name), mf.ThisRegionMap, 100, mf.ThisRegionPoint)
        {
        	RegionName = name;
            Controller = rvsc;
            Flies = mf;
            this.Register();
        }

        public override void OnEnter(Mobile m)
        {
        	if (!Controller.InUse)
                return;
                
			m.SendMessage("Your mana is being regenerated faster!");
			
            if (m is PlayerMobile) 
            { 
            	HealManaTimer timer = new HealManaTimer( m );
            	PlayerTimers.Add(timer);
          		timer.Start();
            }

            m.Delta(MobileDelta.Noto);
            m.InvalidateProperties();
        }
        
        public override void OnExit(Mobile m)
        {
            m.SendMessage("Your mana regeneration has returned to normal.", this.Name);

			if (m is PlayerMobile) 
            { 
            	for ( int i = 0; i < PlayerTimers.Count; ++i )
            	{
            		if ( PlayerTimers[i] != null && ((HealManaTimer)PlayerTimers[i]).m == m )
            		{
            			HealManaTimer timer = (HealManaTimer)PlayerTimers[i];
          				timer.Stop();
            			PlayerTimers[i] = null;
            		}
            	}
            }
            int count = 0;
            for ( int j = 0; j < PlayerTimers.Count; ++j )
            {
            	if ( PlayerTimers[j] == null )
            	{
					count++;
            	}
            }
            if ( count == PlayerTimers.Count )
            {
            	PlayerTimers.Clear();
            }
        }
        
        public override bool AllowBeneficial(Mobile from, Mobile target)
        {
            if (!Controller.InUse)
                return false;

            if (from is PlayerMobile && target is PlayerMobile) 
            { 
                if (Controller.Handeling.IsAlly((PlayerMobile)from, (PlayerMobile)target))
                return true;

            	if (Controller.Handeling.IsEnemy((PlayerMobile)from, (PlayerMobile)target))
                return false;

            	if (!Controller.Handeling.InRVS((PlayerMobile)from) || !Controller.Handeling.InRVS((PlayerMobile)target))
                return false;
                
                return base.AllowBeneficial(from, target); 
            }
            else if ( from is PlayerMobile && target is BaseCreature) 
            {
            	BaseCreature targetPet = (BaseCreature) target;
                if ( !targetPet.Controlled )
                	return false;
            	
            	if (Controller.Handeling.IsAlly((PlayerMobile)from, (PlayerMobile)targetPet.ControlMaster))
                	return true;

            	if (Controller.Handeling.IsEnemy((PlayerMobile)from, (PlayerMobile)targetPet.ControlMaster))
                	return false;

            	if (!Controller.Handeling.InRVS((PlayerMobile)from) || !Controller.Handeling.InRVS((PlayerMobile)targetPet.ControlMaster))
                	return false;

            	return base.AllowBeneficial(from, target); 
            }
            else if ( from is BaseCreature && target is PlayerMobile) 
            {
            	BaseCreature fromPet = (BaseCreature)from;
                if ( !fromPet.Controlled )
                	return false;

                if (Controller.Handeling.IsAlly((PlayerMobile)fromPet.ControlMaster, (PlayerMobile)target))
                	return true;

            	if (Controller.Handeling.IsEnemy((PlayerMobile)fromPet.ControlMaster, (PlayerMobile)target))
                	return false;

            	if (!Controller.Handeling.InRVS((PlayerMobile)fromPet.ControlMaster) || !Controller.Handeling.InRVS((PlayerMobile)target))
                	return false;

            	return base.AllowBeneficial(from, target);
			}
            else if ( from is BaseCreature && target is BaseCreature) 
            {
                BaseCreature fromPet = (BaseCreature)from;
                BaseCreature targetPet = (BaseCreature)target;
                if ( !fromPet.Controlled && !targetPet.Controlled )
                	return false;
                
                if (Controller.Handeling.IsAlly((PlayerMobile)fromPet.ControlMaster, (PlayerMobile)targetPet.ControlMaster))
                	return true;

            	if (Controller.Handeling.IsEnemy((PlayerMobile)fromPet.ControlMaster, (PlayerMobile)targetPet.ControlMaster))
                	return false;

            	if (!Controller.Handeling.InRVS((PlayerMobile)fromPet.ControlMaster) || !Controller.Handeling.InRVS((PlayerMobile)targetPet.ControlMaster))
                	return false;

            	return base.AllowBeneficial(from, target); 
            }
            else
                return false;
        }

        public override bool AllowHarmful(Mobile from, Mobile target)
        {
            if (!Controller.InUse)
                return false;

            if (from is PlayerMobile && target is PlayerMobile)
            {
            	if (Controller.Handeling.IsAlly((PlayerMobile)from, (PlayerMobile)target))
                	return false;

            	if (Controller.Handeling.IsEnemy((PlayerMobile)from, (PlayerMobile)target))
                	return true;

            	if (!Controller.Handeling.InRVS((PlayerMobile)from) || !Controller.Handeling.InRVS((PlayerMobile)target))
                	return false;

            	return base.AllowHarmful(from, target);
            }
            else if ( from is PlayerMobile && target is BaseCreature) 
            {
            	BaseCreature targetPet = (BaseCreature) target;

            	if (targetPet.Controlled && Controller.Handeling.IsAlly((PlayerMobile)from, (PlayerMobile)targetPet.ControlMaster))
                	return false;

            	if (targetPet.Controlled && Controller.Handeling.IsEnemy((PlayerMobile)from, (PlayerMobile)targetPet.ControlMaster))
                	return true;

            	return base.AllowHarmful(from, target); 
            }
            else if ( from is BaseCreature && target is PlayerMobile) 
            {
            	BaseCreature fromPet = (BaseCreature)from;

                if (fromPet.Controlled && Controller.Handeling.IsAlly((PlayerMobile)fromPet.ControlMaster, (PlayerMobile)target))
                	return false;

            	if (fromPet.Controlled && Controller.Handeling.IsEnemy((PlayerMobile)fromPet.ControlMaster, (PlayerMobile)target))
                	return true;

            	return base.AllowHarmful(from, target);
			}
            else if ( from is BaseCreature && target is BaseCreature) 
            {
                BaseCreature fromPet = (BaseCreature)from;
                BaseCreature targetPet = (BaseCreature)target;
                
                if (fromPet.Controlled && targetPet.Controlled && Controller.Handeling.IsAlly((PlayerMobile)fromPet.ControlMaster, (PlayerMobile)targetPet.ControlMaster))
                	return false;

            	if (fromPet.Controlled && targetPet.Controlled && Controller.Handeling.IsEnemy((PlayerMobile)fromPet.ControlMaster, (PlayerMobile)targetPet.ControlMaster))
                	return true;

            	return base.AllowHarmful(from, target); 
            }
            else
                return false;
        }

        public override bool AllowHousing(Mobile from, Point3D p)
        {
            return false;
        }

        public override void OnDeath(Mobile m)
        {
            if (!Controller.InUse)
                return;

            if (m is PlayerMobile)
            {
	    		m.SendMessage("You have died in the RVS Duel!!");
	    		for ( int x = 0; x < 5; x++ )
	    		{
					Controller.LaunchFireworks(m);
	    		}
            	Timer.DelayCall(TimeSpan.FromSeconds(5.0), new TimerStateCallback(Controller.HandleDeath), (object)m);
	   		}
	   		else if (m is BaseCreature)//CHANGE THIS WHEN APPROPRIATE
	   		{
	   			if ( this.RegionName == "Sheep" )
	   			{
	   				Controller.SpawnEnemies( Controller.Handeling, "Rabbit", 2);
	   			}
	   			else if ( this.RegionName == "Rabbit" )
	   			{
	   				Controller.SpawnEnemies( Controller.Handeling, "Sheep", 2);
	   			}
	   		}
	   		else
	   		{
	   			m.Corpse.Delete();
	   		}
        }
        
        public override bool OnDoubleClick(Mobile m, object o)
        {
            if (!Controller.InUse)
                return false;

            if (o is Item) { }
            else
                return base.OnDoubleClick(m, o);

            Item item = (Item)o;

            if (item is Corpse)
            {
                m.SendMessage("You may not loot here.");
                return false;
            }

            if (item is BasePotion && !Controller.Handeling.Potions)
            {
                m.SendMessage("The use of potions is not allowed.");
                return false;
            }

            if (item is Bandage && !Controller.Handeling.Bandages)
            {
                m.SendMessage("The use of bandages is not allowed.");
                return false;
            }

            if (item is TrapableContainer && !Controller.Handeling.TrappedContainers)
            {
                TrapableContainer cont = (TrapableContainer)item;
                if (cont.TrapType != TrapType.None)
                {
                    m.SendMessage("The use of trapped containers is not allowed.");
                    return false;
                }
            }
            
            if (item is BaseWand && !Controller.Handeling.Wands)
            {
            	m.SendMessage("The use of Magic Wands is not allowed.");
            	return false;
            }

            if (item is Bola && !Controller.Handeling.Bolas)
            {
                m.SendMessage("The use of bolas is not allowed.");
                return false;
            }

            if (item is EtherealMount && !Controller.Handeling.Mounts)
            {
                m.SendMessage("The use of mounts is not allowed.");
                return false;
            }
            
            if (item is SkillBall || item is StatBall)
            {
                m.SendMessage("The use of skill/stat balls are not allowed here.");
                return false;
            }

            return base.OnDoubleClick(m, o);
        }
	}
	
	public class HealthRegenRegion : BaseRegion
    {
    	public String RegionName;
    
		public static ArrayList PlayerTimers = new ArrayList();
        public RVSController Controller;
        public HealthFlies Flies;

        public HealthRegenRegion(HealthFlies mf, RVSController rvsc, String name)
            : base(String.Format("RVSHealthRegion{0}", name), mf.ThisRegionMap, 100, mf.ThisRegionPoint)
        {
        	RegionName = name;
            Controller = rvsc;
            Flies = mf;
            this.Register();
        }

        public override void OnEnter(Mobile m)
        {
        	if (!Controller.InUse)
                return;
        
			m.SendMessage("Your health is being regenerated faster!");
			
            if (m is PlayerMobile) 
            { 
            	HealTimer timer = new HealTimer( m );
            	PlayerTimers.Add(timer);
          		timer.Start();
            }
            
            m.Delta(MobileDelta.Noto);
            m.InvalidateProperties();
        }
        
        public override void OnExit(Mobile m)
        {
            m.SendMessage("Your health regeneration has returned to normal.", this.Name);

			if (m is PlayerMobile) 
            { 
            	for ( int i = 0; i < PlayerTimers.Count; ++i )
            	{
            		if ( PlayerTimers[i] != null && ((HealTimer)PlayerTimers[i]).m == m )
            		{
            			HealTimer timer = (HealTimer)PlayerTimers[i];
          				timer.Stop();
            			PlayerTimers[i] = null;
            		}
            	}
            }
            int count = 0;
            for ( int j = 0; j < PlayerTimers.Count; ++j )
            {
            	if ( PlayerTimers[j] == null )
            	{
					count++;
            	}
            }
            if ( count == PlayerTimers.Count )
            {
            	PlayerTimers.Clear();
            }
        }
        
        public override bool AllowBeneficial(Mobile from, Mobile target)
        {
            if (!Controller.InUse)
                return false;

            if (from is PlayerMobile && target is PlayerMobile) 
            { 
                if (Controller.Handeling.IsAlly((PlayerMobile)from, (PlayerMobile)target))
                return true;

            	if (Controller.Handeling.IsEnemy((PlayerMobile)from, (PlayerMobile)target))
                return false;

            	if (!Controller.Handeling.InRVS((PlayerMobile)from) || !Controller.Handeling.InRVS((PlayerMobile)target))
                return false;
                
                return base.AllowBeneficial(from, target); 
            }
            else if ( from is PlayerMobile && target is BaseCreature) 
            {
            	BaseCreature targetPet = (BaseCreature) target;
                if ( !targetPet.Controlled )
                	return false;
            	
            	if (Controller.Handeling.IsAlly((PlayerMobile)from, (PlayerMobile)targetPet.ControlMaster))
                	return true;

            	if (Controller.Handeling.IsEnemy((PlayerMobile)from, (PlayerMobile)targetPet.ControlMaster))
                	return false;

            	if (!Controller.Handeling.InRVS((PlayerMobile)from) || !Controller.Handeling.InRVS((PlayerMobile)targetPet.ControlMaster))
                	return false;

            	return base.AllowBeneficial(from, target); 
            }
            else if ( from is BaseCreature && target is PlayerMobile) 
            {
            	BaseCreature fromPet = (BaseCreature)from;
                if ( !fromPet.Controlled )
                	return false;

                if (Controller.Handeling.IsAlly((PlayerMobile)fromPet.ControlMaster, (PlayerMobile)target))
                	return true;

            	if (Controller.Handeling.IsEnemy((PlayerMobile)fromPet.ControlMaster, (PlayerMobile)target))
                	return false;

            	if (!Controller.Handeling.InRVS((PlayerMobile)fromPet.ControlMaster) || !Controller.Handeling.InRVS((PlayerMobile)target))
                	return false;

            	return base.AllowBeneficial(from, target);
			}
            else if ( from is BaseCreature && target is BaseCreature) 
            {
                BaseCreature fromPet = (BaseCreature)from;
                BaseCreature targetPet = (BaseCreature)target;
                if ( !fromPet.Controlled && !targetPet.Controlled )
                	return false;
                
                if (Controller.Handeling.IsAlly((PlayerMobile)fromPet.ControlMaster, (PlayerMobile)targetPet.ControlMaster))
                	return true;

            	if (Controller.Handeling.IsEnemy((PlayerMobile)fromPet.ControlMaster, (PlayerMobile)targetPet.ControlMaster))
                	return false;

            	if (!Controller.Handeling.InRVS((PlayerMobile)fromPet.ControlMaster) || !Controller.Handeling.InRVS((PlayerMobile)targetPet.ControlMaster))
                	return false;

            	return base.AllowBeneficial(from, target); 
            }
            else
                return false;
        }

        public override bool AllowHarmful(Mobile from, Mobile target)
        {
            if (!Controller.InUse)
                return false;

            if (from is PlayerMobile && target is PlayerMobile)
            {
            	if (Controller.Handeling.IsAlly((PlayerMobile)from, (PlayerMobile)target))
                	return false;

            	if (Controller.Handeling.IsEnemy((PlayerMobile)from, (PlayerMobile)target))
                	return true;

            	if (!Controller.Handeling.InRVS((PlayerMobile)from) || !Controller.Handeling.InRVS((PlayerMobile)target))
                	return false;

            	return base.AllowHarmful(from, target);
            }
            else if ( from is PlayerMobile && target is BaseCreature) 
            {
            	BaseCreature targetPet = (BaseCreature) target;

            	if (targetPet.Controlled && Controller.Handeling.IsAlly((PlayerMobile)from, (PlayerMobile)targetPet.ControlMaster))
                	return false;

            	if (targetPet.Controlled && Controller.Handeling.IsEnemy((PlayerMobile)from, (PlayerMobile)targetPet.ControlMaster))
                	return true;

            	return base.AllowHarmful(from, target); 
            }
            else if ( from is BaseCreature && target is PlayerMobile) 
            {
            	BaseCreature fromPet = (BaseCreature)from;

                if (fromPet.Controlled && Controller.Handeling.IsAlly((PlayerMobile)fromPet.ControlMaster, (PlayerMobile)target))
                	return false;

            	if (fromPet.Controlled && Controller.Handeling.IsEnemy((PlayerMobile)fromPet.ControlMaster, (PlayerMobile)target))
                	return true;

            	return base.AllowHarmful(from, target);
			}
            else if ( from is BaseCreature && target is BaseCreature) 
            {
                BaseCreature fromPet = (BaseCreature)from;
                BaseCreature targetPet = (BaseCreature)target;
                
                if (fromPet.Controlled && targetPet.Controlled && Controller.Handeling.IsAlly((PlayerMobile)fromPet.ControlMaster, (PlayerMobile)targetPet.ControlMaster))
                	return false;

            	if (fromPet.Controlled && targetPet.Controlled && Controller.Handeling.IsEnemy((PlayerMobile)fromPet.ControlMaster, (PlayerMobile)targetPet.ControlMaster))
                	return true;

            	return base.AllowHarmful(from, target); 
            }
            else
                return false;
        }

        public override bool AllowHousing(Mobile from, Point3D p)
        {
            return false;
        }

        public override void OnDeath(Mobile m)
        {
            if (!Controller.InUse)
                return;

            if (m is PlayerMobile)
            {
	    		m.SendMessage("You have died in the RVS Duel!!");
	    		for ( int x = 0; x < 5; x++ )
	    		{
					Controller.LaunchFireworks(m);
	    		}
            	Timer.DelayCall(TimeSpan.FromSeconds(5.0), new TimerStateCallback(Controller.HandleDeath), (object)m);
	   		}
	   		else if (m is BaseCreature)//CHANGE THIS WHEN APPROPRIATE
	   		{
	   			if ( this.RegionName == "Sheep" )
	   			{
	   				Controller.SpawnEnemies( Controller.Handeling, "Rabbit", 2);
	   			}
	   			else if ( this.RegionName == "Rabbit" )
	   			{
	   				Controller.SpawnEnemies( Controller.Handeling, "Sheep", 2);
	   			}
	   		}
	   		else
	   		{
	   			m.Corpse.Delete();
	   		}
        }
        
        public override bool OnDoubleClick(Mobile m, object o)
        {
            if (!Controller.InUse)
                return false;

            if (o is Item) { }
            else
                return base.OnDoubleClick(m, o);

            Item item = (Item)o;

            if (item is Corpse)
            {
                m.SendMessage("You may not loot here.");
                return false;
            }

            if (item is BasePotion && !Controller.Handeling.Potions)
            {
                m.SendMessage("The use of potions is not allowed.");
                return false;
            }

            if (item is Bandage && !Controller.Handeling.Bandages)
            {
                m.SendMessage("The use of bandages is not allowed.");
                return false;
            }

            if (item is TrapableContainer && !Controller.Handeling.TrappedContainers)
            {
                TrapableContainer cont = (TrapableContainer)item;
                if (cont.TrapType != TrapType.None)
                {
                    m.SendMessage("The use of trapped containers is not allowed.");
                    return false;
                }
            }
            
            if (item is BaseWand && !Controller.Handeling.Wands)
            {
            	m.SendMessage("The use of Magic Wands is not allowed.");
            	return false;
            }

            if (item is Bola && !Controller.Handeling.Bolas)
            {
                m.SendMessage("The use of bolas is not allowed.");
                return false;
            }

            if (item is EtherealMount && !Controller.Handeling.Mounts)
            {
                m.SendMessage("The use of mounts is not allowed.");
                return false;
            }
            
            if (item is SkillBall || item is StatBall)
            {
                m.SendMessage("The use of skill/stat balls are not allowed here.");
                return false;
            }

            return base.OnDoubleClick(m, o);
        }
	}
	
	public class HealthRegenRegion2 : BaseRegion
    {
    	public String RegionName;
    
		public static ArrayList PlayerTimers = new ArrayList();
        public RVSController Controller;
        public HealthFlies2 Flies;

        public HealthRegenRegion2(HealthFlies2 mf, RVSController rvsc, String name)
            : base(String.Format("RVSHealthRegion{0}", name), mf.ThisRegionMap, 100, mf.ThisRegionPoint)
        {
        	RegionName = name;
            Controller = rvsc;
            Flies = mf;
            this.Register();
        }

        public override void OnEnter(Mobile m)
        {
        	if (!Controller.InUse)
                return;
        
			m.SendMessage("Your health is being regenerated faster!");
			
            if (m is PlayerMobile) 
            { 
            	HealTimer timer = new HealTimer( m );
            	PlayerTimers.Add(timer);
          		timer.Start();
            }
            
            m.Delta(MobileDelta.Noto);
            m.InvalidateProperties();
        }
        
        public override void OnExit(Mobile m)
        {
            m.SendMessage("Your health regeneration has returned to normal.", this.Name);

			if (m is PlayerMobile) 
            { 
            	for ( int i = 0; i < PlayerTimers.Count; ++i )
            	{
            		if ( PlayerTimers[i] != null && ((HealTimer)PlayerTimers[i]).m == m )
            		{
            			HealTimer timer = (HealTimer)PlayerTimers[i];
          				timer.Stop();
            			PlayerTimers[i] = null;
            		}
            	}
            }
            int count = 0;
            for ( int j = 0; j < PlayerTimers.Count; ++j )
            {
            	if ( PlayerTimers[j] == null )
            	{
					count++;
            	}
            }
            if ( count == PlayerTimers.Count )
            {
            	PlayerTimers.Clear();
            }
        }
        
        public override bool AllowBeneficial(Mobile from, Mobile target)
        {
            if (!Controller.InUse)
                return false;

            if (from is PlayerMobile && target is PlayerMobile) 
            { 
                if (Controller.Handeling.IsAlly((PlayerMobile)from, (PlayerMobile)target))
                return true;

            	if (Controller.Handeling.IsEnemy((PlayerMobile)from, (PlayerMobile)target))
                return false;

            	if (!Controller.Handeling.InRVS((PlayerMobile)from) || !Controller.Handeling.InRVS((PlayerMobile)target))
                return false;
                
                return base.AllowBeneficial(from, target); 
            }
            else if ( from is PlayerMobile && target is BaseCreature) 
            {
            	BaseCreature targetPet = (BaseCreature) target;
                if ( !targetPet.Controlled )
                	return false;
            	
            	if (Controller.Handeling.IsAlly((PlayerMobile)from, (PlayerMobile)targetPet.ControlMaster))
                	return true;

            	if (Controller.Handeling.IsEnemy((PlayerMobile)from, (PlayerMobile)targetPet.ControlMaster))
                	return false;

            	if (!Controller.Handeling.InRVS((PlayerMobile)from) || !Controller.Handeling.InRVS((PlayerMobile)targetPet.ControlMaster))
                	return false;

            	return base.AllowBeneficial(from, target); 
            }
            else if ( from is BaseCreature && target is PlayerMobile) 
            {
            	BaseCreature fromPet = (BaseCreature)from;
                if ( !fromPet.Controlled )
                	return false;

                if (Controller.Handeling.IsAlly((PlayerMobile)fromPet.ControlMaster, (PlayerMobile)target))
                	return true;

            	if (Controller.Handeling.IsEnemy((PlayerMobile)fromPet.ControlMaster, (PlayerMobile)target))
                	return false;

            	if (!Controller.Handeling.InRVS((PlayerMobile)fromPet.ControlMaster) || !Controller.Handeling.InRVS((PlayerMobile)target))
                	return false;

            	return base.AllowBeneficial(from, target);
			}
            else if ( from is BaseCreature && target is BaseCreature) 
            {
                BaseCreature fromPet = (BaseCreature)from;
                BaseCreature targetPet = (BaseCreature)target;
                if ( !fromPet.Controlled && !targetPet.Controlled )
                	return false;
                
                if (Controller.Handeling.IsAlly((PlayerMobile)fromPet.ControlMaster, (PlayerMobile)targetPet.ControlMaster))
                	return true;

            	if (Controller.Handeling.IsEnemy((PlayerMobile)fromPet.ControlMaster, (PlayerMobile)targetPet.ControlMaster))
                	return false;

            	if (!Controller.Handeling.InRVS((PlayerMobile)fromPet.ControlMaster) || !Controller.Handeling.InRVS((PlayerMobile)targetPet.ControlMaster))
                	return false;

            	return base.AllowBeneficial(from, target); 
            }
            else
                return false;
        }

        public override bool AllowHarmful(Mobile from, Mobile target)
        {
            if (!Controller.InUse)
                return false;

            if (from is PlayerMobile && target is PlayerMobile)
            {
            	if (Controller.Handeling.IsAlly((PlayerMobile)from, (PlayerMobile)target))
                	return false;

            	if (Controller.Handeling.IsEnemy((PlayerMobile)from, (PlayerMobile)target))
                	return true;

            	if (!Controller.Handeling.InRVS((PlayerMobile)from) || !Controller.Handeling.InRVS((PlayerMobile)target))
                	return false;

            	return base.AllowHarmful(from, target);
            }
            else if ( from is PlayerMobile && target is BaseCreature) 
            {
            	BaseCreature targetPet = (BaseCreature) target;

            	if (targetPet.Controlled && Controller.Handeling.IsAlly((PlayerMobile)from, (PlayerMobile)targetPet.ControlMaster))
                	return false;

            	if (targetPet.Controlled && Controller.Handeling.IsEnemy((PlayerMobile)from, (PlayerMobile)targetPet.ControlMaster))
                	return true;

            	return base.AllowHarmful(from, target); 
            }
            else if ( from is BaseCreature && target is PlayerMobile) 
            {
            	BaseCreature fromPet = (BaseCreature)from;

                if (fromPet.Controlled && Controller.Handeling.IsAlly((PlayerMobile)fromPet.ControlMaster, (PlayerMobile)target))
                	return false;

            	if (fromPet.Controlled && Controller.Handeling.IsEnemy((PlayerMobile)fromPet.ControlMaster, (PlayerMobile)target))
                	return true;

            	return base.AllowHarmful(from, target);
			}
            else if ( from is BaseCreature && target is BaseCreature) 
            {
                BaseCreature fromPet = (BaseCreature)from;
                BaseCreature targetPet = (BaseCreature)target;
                
                if (fromPet.Controlled && targetPet.Controlled && Controller.Handeling.IsAlly((PlayerMobile)fromPet.ControlMaster, (PlayerMobile)targetPet.ControlMaster))
                	return false;

            	if (fromPet.Controlled && targetPet.Controlled && Controller.Handeling.IsEnemy((PlayerMobile)fromPet.ControlMaster, (PlayerMobile)targetPet.ControlMaster))
                	return true;

            	return base.AllowHarmful(from, target); 
            }
            else
                return false;
        }

        public override bool AllowHousing(Mobile from, Point3D p)
        {
            return false;
        }

        public override void OnDeath(Mobile m)
        {
            if (!Controller.InUse)
                return;

            if (m is PlayerMobile)
            {
	    		m.SendMessage("You have died in the RVS Duel!!");
	    		for ( int x = 0; x < 5; x++ )
	    		{
					Controller.LaunchFireworks(m);
	    		}
            	Timer.DelayCall(TimeSpan.FromSeconds(5.0), new TimerStateCallback(Controller.HandleDeath), (object)m);
	   		}
	   		else if (m is BaseCreature)//CHANGE THIS WHEN APPROPRIATE
	   		{
	   			if ( this.RegionName == "Sheep" )
	   			{
	   				Controller.SpawnEnemies( Controller.Handeling, "Rabbit", 2);
	   			}
	   			else if ( this.RegionName == "Rabbit" )
	   			{
	   				Controller.SpawnEnemies( Controller.Handeling, "Sheep", 2);
	   			}
	   		}
	   		else
	   		{
	   			m.Corpse.Delete();
	   		}
        }
        
        public override bool OnDoubleClick(Mobile m, object o)
        {
            if (!Controller.InUse)
                return false;

            if (o is Item) { }
            else
                return base.OnDoubleClick(m, o);

            Item item = (Item)o;

            if (item is Corpse)
            {
                m.SendMessage("You may not loot here.");
                return false;
            }

            if (item is BasePotion && !Controller.Handeling.Potions)
            {
                m.SendMessage("The use of potions is not allowed.");
                return false;
            }

            if (item is Bandage && !Controller.Handeling.Bandages)
            {
                m.SendMessage("The use of bandages is not allowed.");
                return false;
            }

            if (item is TrapableContainer && !Controller.Handeling.TrappedContainers)
            {
                TrapableContainer cont = (TrapableContainer)item;
                if (cont.TrapType != TrapType.None)
                {
                    m.SendMessage("The use of trapped containers is not allowed.");
                    return false;
                }
            }
            
            if (item is BaseWand && !Controller.Handeling.Wands)
            {
            	m.SendMessage("The use of Magic Wands is not allowed.");
            	return false;
            }

            if (item is Bola && !Controller.Handeling.Bolas)
            {
                m.SendMessage("The use of bolas is not allowed.");
                return false;
            }

            if (item is EtherealMount && !Controller.Handeling.Mounts)
            {
                m.SendMessage("The use of mounts is not allowed.");
                return false;
            }
            
            if (item is SkillBall || item is StatBall)
            {
                m.SendMessage("The use of skill/stat balls are not allowed here.");
                return false;
            }

            return base.OnDoubleClick(m, o);
        }
	}
    
    public class HealManaTimer : Timer
    {
    	public int toHeal = 5;
        public Mobile m;

        public HealManaTimer(Mobile m_Mobile)
                : base(TimeSpan.FromSeconds(1.0), TimeSpan.FromSeconds(1.0))
        {
            Priority = TimerPriority.FiftyMS;
            m = m_Mobile;
        }

        protected override void OnTick()
        {
            // heals the player's mana
            if ( m is PlayerMobile )
            {
				if ( m.Mana < m.ManaMax && m.Alive )
				{
					m.Mana += toHeal;
					m.FixedParticles( 0x373A, 10, 15, 5012, 1265, 0, EffectLayer.Waist );
            	}
            }
        }
    }
    
    public class HealTimer : Timer
    {
    	public int toHeal = 5;
        public Mobile m;

        public HealTimer(Mobile m_Mobile)
                : base(TimeSpan.FromSeconds(1.0), TimeSpan.FromSeconds(1.0))
        {
            Priority = TimerPriority.FiftyMS;
            m = m_Mobile;
        }

        protected override void OnTick()
        {
            // heals the player
            if ( m is PlayerMobile )
            {
				if ( m.Hits < m.HitsMax && m.Alive )
				{
					m.Hits += toHeal;
					m.FixedParticles( 0x374A, 10, 15, 5021, 53, 0, EffectLayer.Waist );
            	}
            }
        }
    }
}