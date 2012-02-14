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
    public class RVSRegion : BaseRegion
    {
        public static int Count;

        public RVSController Controller;
        public String RegionName;

        public RVSRegion(RVSController dc)
            : base(String.Format("RVSRegion{0}", Count.ToString()), dc.sheepRegionMap, 100, dc.sheepRegionPoint)
        {
            Controller = dc;
            this.RegionName = "Sheep";
            this.Register();
            Count += 1;
        }
        
        public RVSRegion(RVSController dc, Map RegionMap, Rectangle2D RegionPoint, String regionName)
            : base(String.Format("RVSRegion{0}", Count.ToString()), RegionMap, 100, RegionPoint)
        {
            Controller = dc;
            this.RegionName = regionName;
            this.Register();
            Count += 1;
        }

        public override void OnEnter(Mobile m)
        {
            if (!Controller.InUse)
                return;
                
            if (m is EnergyVortex)
            {
            	EnergyVortex ev = (EnergyVortex) m;
            	Controller.EVsAndBSsAdd(ev);
            }
            else if( m is BladeSpirits )
            {
            	BladeSpirits bs = (BladeSpirits) m;
            	Controller.EVsAndBSsAdd(bs);
            }

            m.Delta(MobileDelta.Noto);
            m.InvalidateProperties();
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
	   			if ( Controller.SheepPoints >= Controller.Handeling.MonstersOverwhelm )
	   			{
	   				Controller.DeleteEnemies();
	   				Controller.EndRVS(1);
	   				//Rabbits Have Won
	   			}
	   			else if ( Controller.RabbitPoints >= Controller.Handeling.MonstersOverwhelm )
	   			{
	   				Controller.DeleteEnemies();
	   				Controller.EndRVS(2);
	   				//Sheep Have Won
	   			}
	   			else if ( this.RegionName == "Sheep" )
	   			{
	   				Controller.SpawnEnemies( Controller.Handeling, "Rabbit", 2);
	   				Controller.SheepPoints--;
	   				Controller.RabbitPoints++;
	   				Controller.RabbitPoints++;
	   				Controller.Handeling.EchoMessage(String.Format("Team Rabbit has {0} Points!", Controller.RabbitPoints.ToString()));
	   				
	   			}
	   			else if ( this.RegionName == "Rabbit" )
	   			{
	   				Controller.SpawnEnemies( Controller.Handeling, "Sheep", 2);
	   				Controller.RabbitPoints--;
	   				Controller.SheepPoints++;
	   				Controller.SheepPoints++;
	   				Controller.Handeling.EchoMessage(String.Format("Team Sheep has {0} Points!", Controller.SheepPoints.ToString()));
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
}