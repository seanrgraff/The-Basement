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

namespace Server.Dueling
{
    public class DuelRegion : BaseRegion
    {
        public static int Count;

        public DuelController Controller;

        public DuelRegion(DuelController dc)
            : base(String.Format("DuelRegion{0}", Count.ToString()), dc.ThisRegionMap, 100, dc.ThisRegionPoint)
        {
            Controller = dc;
            this.Register();
            Count += 1;
        }

        public override void OnEnter(Mobile m)
        {
            if (!Controller.InUse)
                return;

            m.Delta(MobileDelta.Noto);
            m.InvalidateProperties();
        }

        public override bool OnMoveInto(Mobile m, Direction d, Point3D newLocation, Point3D oldLocation)
        {
            if (!Controller.InUse)
                return false;

            if (m is PlayerMobile) { }
            else
                return false;

            if (!Duel_Config.InADuel((PlayerMobile)m))
                return false;

            return base.OnMoveInto(m, d, newLocation, oldLocation);
        }

        public override bool AllowBeneficial(Mobile from, Mobile target)
        {
            if (!Controller.InUse)
                return false;

            if (from is PlayerMobile && target is PlayerMobile) { }
            else
                return false;

            if (Controller.Handeling.IsAlly((PlayerMobile)from, (PlayerMobile)target))
                return true;

            if (Controller.Handeling.IsEnemy((PlayerMobile)from, (PlayerMobile)target))
                return false;

            if (!Controller.Handeling.InDuel((PlayerMobile)from) || !Controller.Handeling.InDuel((PlayerMobile)target))
                return false;

            return base.AllowBeneficial(from, target);
        }

        public override bool AllowHarmful(Mobile from, Mobile target)
        {
            if (!Controller.InUse)
                return false;

            if (from is PlayerMobile && target is PlayerMobile) { }
            else
                return false;

            if (Controller.Handeling.IsAlly((PlayerMobile)from, (PlayerMobile)target))
                return false;

            if (Controller.Handeling.IsEnemy((PlayerMobile)from, (PlayerMobile)target))
                return true;

            if (!Controller.Handeling.InDuel((PlayerMobile)from) || !Controller.Handeling.InDuel((PlayerMobile)target))
                return false;

            return base.AllowHarmful(from, target);
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
	    m.SendMessage("You have lost the Duel!!");
	    for ( int x = 0; x < 5; x++ )
	    {
		Controller.LaunchFireworks(m);
	    }
            Timer.DelayCall(TimeSpan.FromSeconds(5.0), new TimerStateCallback(Controller.HandleDeath), (object)m);
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

        public override bool CheckAccessibility(Item item, Mobile from)
        {
            if (!Controller.InUse)
                return true;

            if (item is BaseWeapon)
            {
                BaseWeapon weap = (BaseWeapon)item;

                if (!Controller.Handeling.Weapons)
                    return false;
                if (!Controller.Handeling.Poisoned && weap.PoisonCharges > 0)
                    return false;
                if (!Controller.Handeling.Magical && !weap.Attributes.IsEmpty || !Controller.Handeling.Magical && weap.AccuracyLevel != WeaponAccuracyLevel.Regular || !Controller.Handeling.Magical && weap.DamageLevel != WeaponDamageLevel.Regular || !Controller.Handeling.Magical && weap.DurabilityLevel != WeaponDurabilityLevel.Regular)
                    return false;
                if (!Controller.Handeling.RunicWeapons && weap.Quality != WeaponQuality.Regular)
                    return false;
            }
            else if (item is BaseArmor)
            {
                BaseArmor armor = (BaseArmor)item;

                if (!Controller.Handeling.Armor)
                    return false;
                if (!Controller.Handeling.MagicalArmor && !armor.Attributes.IsEmpty)
                    return false;
                if (!Controller.Handeling.MagicalArmor && armor.ProtectionLevel != ArmorProtectionLevel.Regular)
                    return false;
            }
            else if (item is BaseShield && !Controller.Handeling.Shields)
            {    
		return false;
	    }
	    else if (item is BaseWand && !Controller.Handeling.Wands)
		return false;

            return base.CheckAccessibility(item, from);
        }

        public override bool OnSkillUse(Mobile m, int Skill)
        {
            if (!Controller.InUse)
                return false;

            if (Skill == (int)SkillName.Anatomy && !Controller.Handeling.Anatomy)
            {
                m.SendMessage("The use of the skill anatomy is not allowed.");
                return false;
            }

            if (Skill == (int)SkillName.DetectHidden && !Controller.Handeling.DetectHidden)
            {
                m.SendMessage("The use of the skill detecting hidden is not allowed.");
                return false;
            }

            if (Skill == (int)SkillName.EvalInt && !Controller.Handeling.EvaluatingIntelligence)
            {
                m.SendMessage("The use of the skill evaluating intelligence is not allowed.");
                return false;
            }

            if (Skill == (int)SkillName.Hiding && !Controller.Handeling.Hiding)
            {
                m.SendMessage("The use of the skill hiding is not allowed.");
                return false;
            }

            if (Skill == (int)SkillName.Poisoning && !Controller.Handeling.Poisoning)
            {
                m.SendMessage("The use of the poisoning is not allowed.");
                return false;
            }

            if (Skill == (int)SkillName.Snooping && !Controller.Handeling.Snooping)
            {
                m.SendMessage("The use of the skill snooping is not allowed.");
                return false;
            }

            if (Skill == (int)SkillName.Stealing && !Controller.Handeling.Stealing)
            {
                m.SendMessage("The use of the skill stealing is not allowed.");
                return false;
            }

            if (Skill == (int)SkillName.SpiritSpeak && !Controller.Handeling.SpiritSpeak)
            {
                m.SendMessage("The use of the skill spirit speak is not allowed.");
                return false;
            }

            if (Skill == (int)SkillName.Stealth && !Controller.Handeling.Stealth)
            {
                m.SendMessage("The use of the skill stealth is not allowed.");
                return false;
            }

            return base.OnSkillUse(m, Skill);
        }

        public override bool OnBeginSpellCast(Mobile m, ISpell s)
        {
            if (!Controller.InUse)
                return false;

            if (!Controller.Handeling.Spells)
            {
                m.SendMessage("The use of magic is not allowed.");
                return false;
            }

            // 1st Circle
            if (s is ReactiveArmorSpell && !Controller.Handeling.ReactiveArmor)
            {
                m.SendMessage("The use of this spell is now allowed.");
                return false;
            }

            if (s is ClumsySpell && !Controller.Handeling.Clumsy)
            {
                m.SendMessage("The use of this spell is now allowed.");
                return false;
            }

            if (s is CreateFoodSpell && !Controller.Handeling.CreateFood)
            {
                m.SendMessage("The use of this spell is now allowed.");
                return false;
            }

            if (s is FeeblemindSpell && !Controller.Handeling.Feeblemind)
            {
                m.SendMessage("The use of this spell is now allowed.");
                return false;
            }

            if (s is HealSpell && !Controller.Handeling.Heal)
            {
                m.SendMessage("The use of this spell is now allowed.");
                return false;
            }

            if (s is MagicArrowSpell && !Controller.Handeling.MagicArrow)
            {
                m.SendMessage("The use of this spell is now allowed.");
                return false;
            }

            if (s is NightSightSpell && !Controller.Handeling.NightSight)
            {
                m.SendMessage("The use of this spell is now allowed.");
                return false;
            }

            if (s is WeakenSpell && !Controller.Handeling.Weaken)
            {
                m.SendMessage("The use of this spell is now allowed.");
                return false;
            }

            // 2nd Circle
            if (s is AgilitySpell && !Controller.Handeling.Agility)
            {
                m.SendMessage("The use of this spell is now allowed.");
                return false;
            }

            if (s is CunningSpell && !Controller.Handeling.Cunning)
            {
                m.SendMessage("The use of this spell is now allowed.");
                return false;
            }

            if (s is CureSpell && !Controller.Handeling.Cure)
            {
                m.SendMessage("The use of this spell is now allowed.");
                return false;
            }

            if (s is HarmSpell && !Controller.Handeling.Harm)
            {
                m.SendMessage("The use of this spell is now allowed.");
                return false;
            }

            if (s is MagicTrapSpell && !Controller.Handeling.MagicTrap)
            {
                m.SendMessage("The use of this spell is now allowed.");
                return false;
            }

            if (s is RemoveTrapSpell && !Controller.Handeling.Untrap)
            {
                m.SendMessage("The use of this spell is now allowed.");
                return false;
            }

            if (s is ProtectionSpell && !Controller.Handeling.Protection)
            {
                m.SendMessage("The use of this spell is now allowed.");
                return false;
            }

            if (s is StrengthSpell && !Controller.Handeling.Strength)
            {
                m.SendMessage("The use of this spell is now allowed.");
                return false;
            }

            // 3rd Circle
            if (s is BlessSpell && !Controller.Handeling.Bless)
            {
                m.SendMessage("The use of this spell is now allowed.");
                return false;
            }

            if (s is FireballSpell && !Controller.Handeling.Fireball)
            {
                m.SendMessage("The use of this spell is now allowed.");
                return false;
            }

            if (s is MagicLockSpell && !Controller.Handeling.MagicLock)
            {
                m.SendMessage("The use of this spell is now allowed.");
                return false;
            }

            if (s is PoisonSpell && !Controller.Handeling.Poison)
            {
                m.SendMessage("The use of this spell is now allowed.");
                return false;
            }

            if (s is TelekinesisSpell && !Controller.Handeling.Telekinisis)
            {
                m.SendMessage("The use of this spell is now allowed.");
                return false;
            }

            if (s is TeleportSpell && !Controller.Handeling.Teleport)
            {
                m.SendMessage("The use of this spell is now allowed.");
                return false;
            }

            if (s is UnlockSpell && !Controller.Handeling.Unlock)
            {
                m.SendMessage("The use of this spell is now allowed.");
                return false;
            }

            if (s is WallOfStoneSpell && !Controller.Handeling.WallOfStone)
            {
                m.SendMessage("The use of this spell is now allowed.");
                return false;
            }

            //4th Circle
            if (s is ArchCureSpell && !Controller.Handeling.ArchCure)
            {
                m.SendMessage("The use of this spell is now allowed.");
                return false;
            }

            if (s is ArchProtectionSpell && !Controller.Handeling.ArchProtection)
            {
                m.SendMessage("The use of this spell is now allowed.");
                return false;
            }

            if (s is CurseSpell && !Controller.Handeling.Curse)
            {
                m.SendMessage("The use of this spell is now allowed.");
                return false;
            }

            if (s is FireFieldSpell && !Controller.Handeling.FireField)
            {
                m.SendMessage("The use of this spell is now allowed.");
                return false;
            }

            if (s is GreaterHealSpell && !Controller.Handeling.GreaterHeal)
            {
                m.SendMessage("The use of this spell is now allowed.");
                return false;
            }

            if (s is LightningSpell && !Controller.Handeling.Lightning)
            {
                m.SendMessage("The use of this spell is now allowed.");
                return false;
            }

            if (s is ManaDrainSpell && !Controller.Handeling.ManaDrain)
            {
                m.SendMessage("The use of this spell is now allowed.");
                return false;
            }

            if (s is RecallSpell)
            {
                m.SendMessage("The use of this spell is now allowed.");
                return false;
            }

            // 5th Circle
            if (s is BladeSpiritsSpell && !Controller.Handeling.BladeSpirits)
            {
                m.SendMessage("The use of this spell is now allowed.");
                return false;
            }

            if (s is DispelFieldSpell && !Controller.Handeling.DispelField)
            {
                m.SendMessage("The use of this spell is now allowed.");
                return false;
            }

            if (s is IncognitoSpell && !Controller.Handeling.Incognito)
            {
                m.SendMessage("The use of this spell is now allowed.");
                return false;
            }

            if (s is MagicReflectSpell && !Controller.Handeling.MagicReflection)
            {
                m.SendMessage("The use of this spell is now allowed.");
                return false;
            }

            if (s is MindBlastSpell && !Controller.Handeling.MindBlast)
            {
                m.SendMessage("The use of this spell is now allowed.");
                return false;
            }

            if (s is ParalyzeSpell && !Controller.Handeling.Paralyze)
            {
                m.SendMessage("The use of this spell is now allowed.");
                return false;
            }

            if (s is PoisonFieldSpell && !Controller.Handeling.PoisonField)
            {
                m.SendMessage("The use of this spell is now allowed.");
                return false;
            }

            if (s is SummonCreatureSpell && !Controller.Handeling.SummonCreature)
            {
                m.SendMessage("The use of this spell is now allowed.");
                return false;
            }

            // 6th Circle
            if (s is DispelSpell && !Controller.Handeling.Dispel)
            {
                m.SendMessage("The use of this spell is now allowed.");
                return false;
            }

            if (s is EnergyBoltSpell && !Controller.Handeling.EnergyBolt)
            {
                m.SendMessage("The use of this spell is now allowed.");
                return false;
            }

            if (s is ExplosionSpell && !Controller.Handeling.Explosion)
            {
                m.SendMessage("The use of this spell is now allowed.");
                return false;
            }

            if (s is InvisibilitySpell && !Controller.Handeling.Invisibility)
            {
                m.SendMessage("The use of this spell is now allowed.");
                return false;
            }

            if (s is MarkSpell && !Controller.Handeling.Mark)
            {
                m.SendMessage("The use of this spell is now allowed.");
                return false;
            }

            if (s is MassCurseSpell && !Controller.Handeling.MassCurse)
            {
                m.SendMessage("The use of this spell is now allowed.");
                return false;
            }

            if (s is ParalyzeFieldSpell && !Controller.Handeling.ParalyzeField)
            {
                m.SendMessage("The use of this spell is now allowed.");
                return false;
            }

            if (s is  RevealSpell && !Controller.Handeling.Reveal)
            {
                m.SendMessage("The use of this spell is now allowed.");
                return false;
            }

            // 7th Circle
            if (s is ChainLightningSpell && !Controller.Handeling.ChainLightning)
            {
                m.SendMessage("The use of this spell is now allowed.");
                return false;
            }

            if (s is EnergyFieldSpell && !Controller.Handeling.EnergyField)
            {
                m.SendMessage("The use of this spell is now allowed.");
                return false;
            }

            if (s is FlameStrikeSpell && !Controller.Handeling.FlameStrike)
            {
                m.SendMessage("The use of this spell is now allowed.");
                return false;
            }

            if (s is GateTravelSpell && !Controller.Handeling.GateTravel)
            {
                m.SendMessage("The use of this spell is now allowed.");
                return false;
            }

            if (s is ManaVampireSpell && !Controller.Handeling.ManaVampire)
            {
                m.SendMessage("The use of this spell is now allowed.");
                return false;
            }

            if (s is MassDispelSpell && !Controller.Handeling.MassDispel)
            {
                m.SendMessage("The use of this spell is now allowed.");
                return false;
            }

            if (s is MeteorSwarmSpell && !Controller.Handeling.MeteorSwarm)
            {
                m.SendMessage("The use of this spell is now allowed.");
                return false;
            }

            if (s is PolymorphSpell && !Controller.Handeling.Polymorph)
            {
                m.SendMessage("The use of this spell is now allowed.");
                return false;
            }

            // 8th Circle
            if (s is EarthquakeSpell && !Controller.Handeling.EarthQuake)
            {
                m.SendMessage("The use of this spell is now allowed.");
                return false;
            }

            if (s is EnergyVortex && !Controller.Handeling.EnergyVotex)
            {
                m.SendMessage("The use of this spell is now allowed.");
                return false;
            }

            if (s is ResurrectionSpell && !Controller.Handeling.Resurrection)
            {
                m.SendMessage("The use of this spell is now allowed.");
                return false;
            }

            if (s is AirElementalSpell && !Controller.Handeling.SummonAirElemental)
            {
                m.SendMessage("The use of this spell is now allowed.");
                return false;
            }

            if (s is SummonDaemonSpell && !Controller.Handeling.SummonDaemon)
            {
                m.SendMessage("The use of this spell is now allowed.");
                return false;
            }

            if (s is EarthElementalSpell && !Controller.Handeling.SummonEarthElemental)
            {
                m.SendMessage("The use of this spell is now allowed.");
                return false;
            }

            if (s is FireElementalSpell && !Controller.Handeling.SummonFireElemental)
            {
                m.SendMessage("The use of this spell is now allowed.");
                return false;
            }

            if (s is WaterElementalSpell && !Controller.Handeling.SummonWaterElemental)
            {
                m.SendMessage("The use of this spell is now allowed.");
                return false;
            }
            //Bushido
            if(s is SamuraiSpell || s is SamuraiMove && !Controller.Handeling.AllowSamuraiSpells)
            {
                m.SendMessage("The use of this spell is now allowed.");
                return false;
            }

            if (s is Confidence && !Controller.Handeling.Confidence)
            {
                m.SendMessage("The use of this spell is now allowed.");
                return false;
            }

            if (s is CounterAttack && !Controller.Handeling.CounterAttack)
            {
                m.SendMessage("The use of this spell is now allowed.");
                return false;
            }

            if (s is Evasion && !Controller.Handeling.Evasion)
            {
                m.SendMessage("The use of this spell is now allowed.");
                return false;
            }

            if (s is HonorableExecution && !Controller.Handeling.HonorableExecution)
            {
                m.SendMessage("The use of this spell is now allowed.");
                return false;
            }

            if (s is LightningStrike && !Controller.Handeling.LightningStrike)
            {
                m.SendMessage("The use of this spell is now allowed.");
                return false;
            }

            if (s is MomentumStrike && !Controller.Handeling.MomentumStrike)
            {
                m.SendMessage("The use of this spell is now allowed.");
                return false;
            }
            //Chivalry
            if (s is PaladinSpell && !Controller.Handeling.AllowChivalry)
            {
                m.SendMessage("The use of this spell is now allowed.");
                return false;
            }

            if (s is CleanseByFireSpell && !Controller.Handeling.ClenseByFire)
            {
                m.SendMessage("The use of this spell is now allowed.");
                return false;
            }

            if (s is CloseWoundsSpell && !Controller.Handeling.CloseWounds)
            {
                m.SendMessage("The use of this spell is now allowed.");
                return false;
            }

            if (s is ConsecrateWeaponSpell && !Controller.Handeling.ConsecrateWeapon)
            {
                m.SendMessage("The use of this spell is now allowed.");
                return false;
            }

            if (s is DispelEvilSpell && !Controller.Handeling.DispellEvil)
            {
                m.SendMessage("The use of this spell is now allowed.");
                return false;
            }

            if (s is DivineFurySpell && !Controller.Handeling.DivineFury)
            {
                m.SendMessage("The use of this spell is now allowed.");
                return false;
            }

            if (s is EnemyOfOneSpell && !Controller.Handeling.EnemyOfOne)
            {
                m.SendMessage("The use of this spell is now allowed.");
                return false;
            }

            if (s is HolyLightSpell && !Controller.Handeling.HolyLight)
            {
                m.SendMessage("The use of this spell is now allowed.");
                return false;
            }

            if (s is NobleSacrificeSpell && !Controller.Handeling.NobleSacrafice)
            {
                m.SendMessage("The use of this spell is now allowed.");
                return false;
            }

            if (s is RemoveCurseSpell && !Controller.Handeling.RemoveCurse)
            {
                m.SendMessage("The use of this spell is now allowed.");
                return false;
            }

            if (s is SacredJourneySpell)
            {
                m.SendMessage("The use of this spell is now allowed.");
                return false;
            }
            //Necromany Spells
            if(s is NecromancerSpell && !Controller.Handeling.AllowNecromancy)
            {
                m.SendMessage("The use of this spell is now allowed.");
                return false;
            }

            if (s is AnimateDeadSpell && !Controller.Handeling.AnimateDead)
            {
                m.SendMessage("The use of this spell is now allowed.");
                return false;
            }

            if (s is BloodOathSpell && !Controller.Handeling.BloodOath)
            {
                m.SendMessage("The use of this spell is now allowed.");
                return false;
            }

            if (s is CorpseSkinSpell && !Controller.Handeling.CorpseSkin)
            {
                m.SendMessage("The use of this spell is now allowed.");
                return false;
            }

            if (s is CurseWeaponSpell && !Controller.Handeling.CurseWeapon)
            {
                m.SendMessage("The use of this spell is now allowed.");
                return false;
            }

            if (s is EvilOmenSpell && !Controller.Handeling.EvilOmen)
            {
                m.SendMessage("The use of this spell is now allowed.");
                return false;
            }

            if (s is ExorcismSpell && !Controller.Handeling.Exorcisim)
            {
                m.SendMessage("The use of this spell is now allowed.");
                return false;
            }

            if (s is HorrificBeastSpell && !Controller.Handeling.HorrificBeast)
            {
                m.SendMessage("The use of this spell is now allowed.");
                return false;
            }

            if (s is LichFormSpell && !Controller.Handeling.LichForm)
            {
                m.SendMessage("The use of this spell is now allowed.");
                return false;
            }

            if (s is MindRotSpell && !Controller.Handeling.MindRot)
            {
                m.SendMessage("The use of this spell is now allowed.");
                return false;
            }

            if (s is PainSpikeSpell && !Controller.Handeling.PainSpike)
            {
                m.SendMessage("The use of this spell is now allowed.");
                return false;
            }

            if (s is PoisonStrikeSpell && !Controller.Handeling.PoisonStrike)
            {
                m.SendMessage("The use of this spell is now allowed.");
                return false;
            }

            if (s is StrangleSpell && !Controller.Handeling.Strangle)
            {
                m.SendMessage("The use of this spell is now allowed.");
                return false;
            }

            if (s is SummonFamiliarSpell && !Controller.Handeling.SummonFamiliar)
            {
                m.SendMessage("The use of this spell is now allowed.");
                return false;
            }

            if (s is VampiricEmbraceSpell && !Controller.Handeling.VampiricEmbrace)
            {
                m.SendMessage("The use of this spell is now allowed.");
                return false;
            }

            if (s is VengefulSpiritSpell && !Controller.Handeling.VengefulSpirit)
            {
                m.SendMessage("The use of this spell is now allowed.");
                return false;
            }

            if (s is WitherSpell && !Controller.Handeling.Wither)
            {
                m.SendMessage("The use of this spell is now allowed.");
                return false;
            }

            if (s is WraithFormSpell && !Controller.Handeling.WraithForm)
            {
                m.SendMessage("The use of this spell is now allowed.");
                return false;
            }
            //Ninja Moves
            if (s is NinjaSpell || s is NinjaMove && !Controller.Handeling.AllowNinjaSpells)
            {
                m.SendMessage("The use of this spell is now allowed.");
                return false;
            }

            if (s is AnimalForm && !Controller.Handeling.AnimalForm)
            {
                m.SendMessage("The use of this spell is now allowed.");
                return false;
            }

            if (s is Backstab && !Controller.Handeling.Backstab)
            {
                m.SendMessage("The use of this spell is now allowed.");
                return false;
            }

            if (s is DeathStrike && !Controller.Handeling.DeathStrike)
            {
                m.SendMessage("The use of this spell is now allowed.");
                return false;
            }

            if (s is FocusAttack && !Controller.Handeling.FocusAttack)
            {
                m.SendMessage("The use of this spell is now allowed.");
                return false;
            }

            if (s is KiAttack && !Controller.Handeling.KiAttack)
            {
                m.SendMessage("The use of this spell is now allowed.");
                return false;
            }

            if (s is MirrorImage && !Controller.Handeling.MirrorImage)
            {
                m.SendMessage("The use of this spell is now allowed.");
                return false;
            }

            if (s is Shadowjump)
            {
                m.SendMessage("The use of this spell is now allowed.");
                return false;
            }

            if (s is SurpriseAttack && !Controller.Handeling.SurpriseAttack)
            {
                m.SendMessage("The use of this spell is now allowed.");
                return false;
            }

            return base.OnBeginSpellCast(m, s);
        }
    }
}