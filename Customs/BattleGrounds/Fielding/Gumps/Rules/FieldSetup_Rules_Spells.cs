using System;
using Server;
using Server.Gumps;
using Server.Items;
using Server.Network;
using Server.Commands;
using Server.Mobiles;
using System.Collections;
using System.Collections.Generic;

namespace Server.Fielding
{
	public class FieldSetup_Rules_Spells : Gump
	{
        public Field Handeling;

		public FieldSetup_Rules_Spells(Field d) : base(0, 0)
		{
            Handeling = d;

			Closable = true;
			Dragable = true;
			Resizable = false;

            AddBackground(219, 238, 172, 254, 9300);
            AddLabel(255, 244, 70, @"Spell Use Rules");

            AddCheck(225, 265, 210, 211, Handeling.Spells, 1);
            AddLabel(255, 262, 54, @"Allow Magic");
            AddButton(225, 290, 1209, 1210, 2, GumpButtonType.Reply, 0);
            AddLabel(255, 288, 54, @"1st Circle");
            AddButton(225, 315, 1209, 1210, 3, GumpButtonType.Reply, 0);
            AddLabel(255, 312, 54, @"2nd Circle");
            AddButton(225, 340, 1209, 1210, 4, GumpButtonType.Reply, 0);
            AddLabel(255, 338, 54, @"3rd Circle");
            AddButton(225, 365, 1209, 1210, 5, GumpButtonType.Reply, 0);
            AddLabel(255, 362, 54, @"4th Circle");
            AddButton(225, 390, 1209, 1210, 6, GumpButtonType.Reply, 0);
            AddLabel(255, 388, 54, @"5th Circle");
            AddButton(225, 415, 1209, 1210, 7, GumpButtonType.Reply, 0);
            AddLabel(255, 412, 54, @"6th Circle");
            AddButton(225, 440, 1209, 1210, 8, GumpButtonType.Reply, 0);
            AddLabel(255, 438, 54, @"7th Circle");
            AddButton(225, 465, 1209, 1210, 9, GumpButtonType.Reply, 0);
            AddLabel(255, 462, 54, @"8th Circle");
		}

		public override void OnResponse( NetState sender, RelayInfo info )
		{
			Mobile from = sender.Mobile;

            Handeling.Spells = info.IsSwitched(1);

            switch (info.ButtonID)
            {
                case 0: // Closeing
                    {
                        from.SendGump(new FieldSetup_Rules(Handeling));
                        break;
                    }
                case 2: // First Circle
                    {
                        from.SendGump(new FieldSetup_Rules_Spells_1st(Handeling));
                        break;
                    }
                case 3: // Second Circle
                    {
                        from.SendGump(new FieldSetup_Rules_Spells_2nd(Handeling));
                        break;
                    }
                case 4: // Third Circle
                    {
                        from.SendGump(new FieldSetup_Rules_Spells_3rd(Handeling));
                        break;
                    }
                case 5: // Fourth Circle
                    {
                        from.SendGump(new FieldSetup_Rules_Spells_4th(Handeling));
                        break;
                    }
                case 6: // Fifth Circle
                    {
                        from.SendGump(new FieldSetup_Rules_Spells_5th(Handeling));
                        break;
                    }
                case 7: // Sixth Circle
                    {
                        from.SendGump(new FieldSetup_Rules_Spells_6th(Handeling));
                        break;
                    }
                case 8: // Seventh Circle
                    {
                        from.SendGump(new FieldSetup_Rules_Spells_7th(Handeling));
                        break;
                    }
                case 9: // Eigth Circle
                    {
                        from.SendGump(new FieldSetup_Rules_Spells_8th(Handeling));
                        break;
                    }
            }
		}
	}

    public class FieldSetup_Rules_Spells_1st : Gump
    {
        public Field Handeling;

        public FieldSetup_Rules_Spells_1st(Field d)
            : base(0, 0)
        {
            Handeling = d;

            Closable = true;
            Dragable = true;
            Resizable = false;

            AddBackground(219, 238, 172, 229, 9300);
            AddLabel(255, 244, 70, @"1st Circle Rules");

            AddCheck(225, 265, 210, 211, Handeling.ReactiveArmor, 1);
            AddLabel(255, 265, 54, @"Reactive Armor");
            AddCheck(225, 290, 210, 211, Handeling.Clumsy, 2);
            AddLabel(255, 290, 54, @"Clumsy");
            AddCheck(225, 315, 210, 211, Handeling.CreateFood, 3);
            AddLabel(255, 315, 54, @"Create Food");
            AddCheck(225, 340, 210, 211, Handeling.Feeblemind, 4);
            AddLabel(255, 340, 54, @"Feeblemind");
            AddCheck(225, 365, 210, 211, Handeling.Heal, 5);
            AddLabel(255, 365, 54, @"Heal");
            AddCheck(225, 390, 210, 211, Handeling.MagicArrow, 6);
            AddLabel(255, 390, 54, @"Magic Arrow");
            AddCheck(225, 415, 210, 211, Handeling.NightSight, 7);
            AddLabel(255, 415, 54, @"Night Sight");
            AddCheck(225, 440, 210, 211, Handeling.Weaken, 8);
            AddLabel(255, 440, 54, @"Weaken");
        }

        public override void OnResponse(NetState sender, RelayInfo info)
        {
            Mobile from = sender.Mobile;

            Handeling.ReactiveArmor = info.IsSwitched(1);
            Handeling.Clumsy = info.IsSwitched(2);
            Handeling.CreateFood = info.IsSwitched(3);
            Handeling.Feeblemind = info.IsSwitched(4);
            Handeling.Heal = info.IsSwitched(5);
            Handeling.MagicArrow = info.IsSwitched(6);
            Handeling.NightSight = info.IsSwitched(7);
            Handeling.Weaken = info.IsSwitched(8);

            from.SendGump(new FieldSetup_Rules_Spells(Handeling));
        }
    }

    public class FieldSetup_Rules_Spells_2nd : Gump
    {
        public Field Handeling;

        public FieldSetup_Rules_Spells_2nd(Field d)
            : base(0, 0)
        {
            Handeling = d;

            Closable = true;
            Dragable = true;
            Resizable = false;

            AddBackground(219, 238, 172, 229, 9300);
            AddLabel(255, 244, 70, @"2nd Circle Rules");

            AddCheck(225, 265, 210, 211, Handeling.Agility, 1);
            AddLabel(255, 265, 54, @"Agility");
            AddCheck(225, 290, 210, 211, Handeling.Cunning, 2);
            AddLabel(255, 290, 54, @"Cunning");
            AddCheck(225, 315, 210, 211, Handeling.Cure, 3);
            AddLabel(255, 315, 54, @"Cure");
            AddCheck(225, 340, 210, 211, Handeling.Harm, 4);
            AddLabel(255, 340, 54, @"Harm");
            AddCheck(225, 365, 210, 211, Handeling.MagicTrap, 5);
            AddLabel(255, 365, 54, @"Magic Trap");
            AddCheck(225, 390, 210, 211, Handeling.Untrap, 6);
            AddLabel(255, 390, 54, @"Untrap");
            AddCheck(225, 415, 210, 211, Handeling.Protection, 7);
            AddLabel(255, 415, 54, @"Protection");
            AddCheck(225, 440, 210, 211, Handeling.Strength, 8);
            AddLabel(255, 440, 54, @"Strength");
        }

        public override void OnResponse(NetState sender, RelayInfo info)
        {
            Mobile from = sender.Mobile;

            Handeling.Agility = info.IsSwitched(1);
            Handeling.Cunning = info.IsSwitched(2);
            Handeling.Cure = info.IsSwitched(3);
            Handeling.Harm = info.IsSwitched(4);
            Handeling.MagicTrap = info.IsSwitched(5);
            Handeling.Untrap = info.IsSwitched(6);
            Handeling.Protection = info.IsSwitched(7);
            Handeling.Strength = info.IsSwitched(8);

            from.SendGump(new FieldSetup_Rules_Spells(Handeling));
        }
    }

    public class FieldSetup_Rules_Spells_3rd : Gump
    {
        public Field Handeling;

        public FieldSetup_Rules_Spells_3rd(Field d)
            : base(0, 0)
        {
            Handeling = d;

            Closable = true;
            Dragable = true;
            Resizable = false;

            AddBackground(219, 238, 172, 229, 9300);
            AddLabel(255, 244, 70, @"3rd Circle Rules");

            AddCheck(225, 265, 210, 211, Handeling.Bless, 1);
            AddLabel(255, 265, 54, @"Bless");
            AddCheck(225, 290, 210, 211, Handeling.Fireball, 2);
            AddLabel(255, 290, 54, @"Fireball");
            AddCheck(225, 315, 210, 211, Handeling.MagicLock, 3);
            AddLabel(255, 315, 54, @"Magic Lock");
            AddCheck(225, 340, 210, 211, Handeling.Poison, 4);
            AddLabel(255, 340, 54, @"Poison");
            AddCheck(225, 365, 210, 211, Handeling.Telekinisis, 5);
            AddLabel(255, 365, 54, @"Telekinesis");
            AddCheck(225, 390, 210, 211, Handeling.Teleport, 6);
            AddLabel(255, 390, 54, @"Teleport");
            AddCheck(225, 415, 210, 211, Handeling.Unlock, 7);
            AddLabel(255, 415, 54, @"Unlock Spell");
            AddCheck(225, 440, 210, 211, Handeling.WallOfStone, 8);
            AddLabel(255, 440, 54, @"Wall of Stone");
        }

        public override void OnResponse(NetState sender, RelayInfo info)
        {
            Mobile from = sender.Mobile;

            Handeling.Bless = info.IsSwitched(1);
            Handeling.Fireball = info.IsSwitched(2);
            Handeling.MagicLock = info.IsSwitched(3);
            Handeling.Poison = info.IsSwitched(4);
            Handeling.Telekinisis = info.IsSwitched(5);
            Handeling.Teleport = info.IsSwitched(6);
            Handeling.Unlock = info.IsSwitched(7);
            Handeling.WallOfStone = info.IsSwitched(8);

            from.SendGump(new FieldSetup_Rules_Spells(Handeling));
        }
    }

    public class FieldSetup_Rules_Spells_4th : Gump
    {
        public Field Handeling;

        public FieldSetup_Rules_Spells_4th(Field d)
            : base(0, 0)
        {
            Handeling = d;

            Closable = true;
            Dragable = true;
            Resizable = false;

            AddBackground(219, 238, 172, 229, 9300);
            AddLabel(255, 244, 70, @"4th Circle Rules");

            AddCheck(225, 265, 210, 211, Handeling.ArchCure, 1);
            AddLabel(255, 265, 54, @"Arch Cure");
            AddCheck(225, 290, 210, 211, Handeling.ArchProtection, 2);
            AddLabel(255, 290, 54, @"Arch Protection");
            AddCheck(225, 315, 210, 211, Handeling.Curse, 3);
            AddLabel(255, 315, 54, @"Curse");
            AddCheck(225, 340, 210, 211, Handeling.FireField, 4);
            AddLabel(255, 340, 54, @"Firefield");
            AddCheck(225, 365, 210, 211, Handeling.GreaterHeal, 5);
            AddLabel(255, 365, 54, @"Greater Heal");
            AddCheck(225, 390, 210, 211, Handeling.Lightning, 6);
            AddLabel(255, 390, 54, @"Lightning");
            AddCheck(225, 415, 210, 211, Handeling.ManaDrain, 7);
            AddLabel(255, 415, 54, @"Mana Drain");
            AddCheck(225, 440, 210, 211, Handeling.Recall, 8);
            AddLabel(255, 440, 54, @"Recall");
        }

        public override void OnResponse(NetState sender, RelayInfo info)
        {
            Mobile from = sender.Mobile;

            Handeling.ArchCure = info.IsSwitched(1);
            Handeling.ArchProtection = info.IsSwitched(2);
            Handeling.Curse = info.IsSwitched(3);
            Handeling.FireField = info.IsSwitched(4);
            Handeling.GreaterHeal = info.IsSwitched(5);
            Handeling.Lightning = info.IsSwitched(6);
            Handeling.ManaDrain = info.IsSwitched(7);
            Handeling.Recall = info.IsSwitched(8);

            from.SendGump(new FieldSetup_Rules_Spells(Handeling));
        }
    }

    public class FieldSetup_Rules_Spells_5th : Gump
    {
        public Field Handeling;

        public FieldSetup_Rules_Spells_5th(Field d)
            : base(0, 0)
        {
            Handeling = d;

            Closable = true;
            Dragable = true;
            Resizable = false;

            AddBackground(219, 238, 172, 229, 9300);
            AddLabel(255, 244, 70, @"5th Circle Rules");

            AddCheck(225, 265, 210, 211, Handeling.BladeSpirits, 1);
            AddLabel(255, 265, 54, @"Blade Spirits");
            AddCheck(225, 290, 210, 211, Handeling.DispelField, 2);
            AddLabel(255, 290, 54, @"Dispel Field");
            AddCheck(225, 315, 210, 211, Handeling.Incognito, 3);
            AddLabel(255, 315, 54, @"Incognito");
            AddCheck(225, 340, 210, 211, Handeling.MagicReflection, 4);
            AddLabel(255, 340, 54, @"Magic Reflection");
            AddCheck(225, 365, 210, 211, Handeling.MagicReflection, 5);
            AddLabel(255, 365, 54, @"Mind Blast");
            AddCheck(225, 390, 210, 211, Handeling.MindBlast, 6);
            AddLabel(255, 390, 54, @"Paralyze");
            AddCheck(225, 415, 210, 211, Handeling.Paralyze, 7);
            AddLabel(255, 415, 54, @"Poison Field");
            AddCheck(225, 440, 210, 211, Handeling.SummonCreature, 8);
            AddLabel(255, 440, 54, @"Summon Creature");
        }

        public override void OnResponse(NetState sender, RelayInfo info)
        {
            Mobile from = sender.Mobile;

            Handeling.BladeSpirits = info.IsSwitched(1);
            Handeling.DispelField = info.IsSwitched(2);
            Handeling.Incognito = info.IsSwitched(3);
            Handeling.MagicReflection = info.IsSwitched(4);
            Handeling.MindBlast = info.IsSwitched(5);
            Handeling.Paralyze = info.IsSwitched(6);
            Handeling.PoisonField = info.IsSwitched(7);
            Handeling.SummonCreature = info.IsSwitched(8);

            from.SendGump(new FieldSetup_Rules_Spells(Handeling));
        }
    }

    public class FieldSetup_Rules_Spells_6th : Gump
    {
        public Field Handeling;

        public FieldSetup_Rules_Spells_6th(Field d)
            : base(0, 0)
        {
            Handeling = d;

            Closable = true;
            Dragable = true;
            Resizable = false;

            AddBackground(219, 238, 172, 229, 9300);
            AddLabel(255, 244, 70, @"6th Circle Rules");

            AddCheck(225, 265, 210, 211, Handeling.Dispel, 1);
            AddLabel(255, 265, 54, @"Dispel");
            AddCheck(225, 290, 210, 211, Handeling.EnergyBolt, 2);
            AddLabel(255, 290, 54, @"Energy Bolt");
            AddCheck(225, 315, 210, 211, Handeling.Explosion, 3);
            AddLabel(255, 315, 54, @"Explosion");
            AddCheck(225, 340, 210, 211, Handeling.Invisibility, 4);
            AddLabel(255, 340, 54, @"Invisibility");
            AddCheck(225, 365, 210, 211, Handeling.Mark, 5);
            AddLabel(255, 365, 54, @"Mark");
            AddCheck(225, 390, 210, 211, Handeling.MassCurse, 6);
            AddLabel(255, 390, 54, @"Mass Curse");
            AddCheck(225, 415, 210, 211, Handeling.ParalyzeField, 7);
            AddLabel(255, 415, 54, @"Paralyze Field");
            AddCheck(225, 440, 210, 211, Handeling.Reveal, 8);
            AddLabel(255, 440, 54, @"Reveal");
        }

        public override void OnResponse(NetState sender, RelayInfo info)
        {
            Mobile from = sender.Mobile;

            Handeling.Dispel = info.IsSwitched(1);
            Handeling.EnergyBolt = info.IsSwitched(2);
            Handeling.Explosion = info.IsSwitched(3);
            Handeling.Invisibility = info.IsSwitched(4);
            Handeling.Mark = info.IsSwitched(5);
            Handeling.MassCurse = info.IsSwitched(6);
            Handeling.ParalyzeField = info.IsSwitched(7);
            Handeling.Reveal = info.IsSwitched(8);

            from.SendGump(new FieldSetup_Rules_Spells(Handeling));
        }
    }

    public class FieldSetup_Rules_Spells_7th : Gump
    {
        public Field Handeling;

        public FieldSetup_Rules_Spells_7th(Field d)
            : base(0, 0)
        {
            Handeling = d;

            Closable = true;
            Dragable = true;
            Resizable = false;

            AddBackground(219, 238, 172, 229, 9300);
            AddLabel(255, 244, 70, @"7th Circle Rules");

            AddCheck(225, 265, 210, 211, Handeling.ChainLightning, 1);
            AddLabel(255, 265, 54, @"Chain Lightning");
            AddCheck(225, 290, 210, 211, Handeling.EnergyField, 2);
            AddLabel(255, 290, 54, @"Energy Field");
            AddCheck(225, 315, 210, 211, Handeling.FlameStrike, 3);
            AddLabel(255, 315, 54, @"Flame Strike");
            AddCheck(225, 340, 210, 211, Handeling.GateTravel, 4);
            AddLabel(255, 340, 54, @"Gate Travel");
            AddCheck(225, 365, 210, 211, Handeling.ManaVampire, 5);
            AddLabel(255, 365, 54, @"Mana Vampire");
            AddCheck(225, 390, 210, 211, Handeling.MassDispel, 6);
            AddLabel(255, 390, 54, @"Mass Dispel");
            AddCheck(225, 415, 210, 211, Handeling.MeteorSwarm, 7);
            AddLabel(255, 415, 54, @"Meteor Swarm");
            AddCheck(225, 440, 210, 211, Handeling.Polymorph, 8);
            AddLabel(255, 440, 54, @"Polymorph");
        }

        public override void OnResponse(NetState sender, RelayInfo info)
        {
            Mobile from = sender.Mobile;

            Handeling.ChainLightning = info.IsSwitched(1);
            Handeling.EnergyField = info.IsSwitched(2);
            Handeling.FlameStrike = info.IsSwitched(3);
            Handeling.GateTravel = info.IsSwitched(4);
            Handeling.ManaVampire = info.IsSwitched(5);
            Handeling.MassDispel = info.IsSwitched(6);
            Handeling.MeteorSwarm = info.IsSwitched(7);
            Handeling.Polymorph = info.IsSwitched(8);

            from.SendGump(new FieldSetup_Rules_Spells(Handeling));
        }
    }

    public class FieldSetup_Rules_Spells_8th : Gump
    {
        public Field Handeling;

        public FieldSetup_Rules_Spells_8th(Field d)
            : base(0, 0)
        {
            Handeling = d;

            Closable = true;
            Dragable = true;
            Resizable = false;

            AddBackground(219, 238, 172, 229, 9300);
            AddLabel(255, 244, 70, @"8th Circle Rules");

            AddCheck(225, 265, 210, 211, Handeling.EarthQuake, 1);
            AddLabel(255, 265, 54, @"Earthquake");
            AddCheck(225, 290, 210, 211, Handeling.EnergyVotex, 2);
            AddLabel(255, 290, 54, @"Energy Vortex");
            AddCheck(225, 315, 210, 211, Handeling.Resurrection, 3);
            AddLabel(255, 315, 54, @"Resurrection");
            AddCheck(225, 340, 210, 211, Handeling.SummonAirElemental, 4);
            AddLabel(255, 340, 54, @"Summon Air");
            AddCheck(225, 365, 210, 211, Handeling.SummonDaemon, 5);
            AddLabel(255, 365, 54, @"Summon Daemon");
            AddCheck(225, 390, 210, 211, Handeling.SummonEarthElemental, 6);
            AddLabel(255, 390, 54, @"Summon Earth");
            AddCheck(225, 415, 210, 211, Handeling.SummonFireElemental, 7);
            AddLabel(255, 415, 54, @"Summon Fire");
            AddCheck(225, 440, 210, 211, Handeling.SummonWaterElemental, 8);
            AddLabel(255, 440, 54, @"Summon Water");
        }

        public override void OnResponse(NetState sender, RelayInfo info)
        {
            Mobile from = sender.Mobile;

            Handeling.EarthQuake = info.IsSwitched(1);
            Handeling.EnergyVotex = info.IsSwitched(2);
            Handeling.Resurrection = info.IsSwitched(3);
            Handeling.SummonAirElemental = info.IsSwitched(4);
            Handeling.SummonDaemon = info.IsSwitched(5);
            Handeling.SummonEarthElemental = info.IsSwitched(6);
            Handeling.SummonFireElemental = info.IsSwitched(7);
            Handeling.SummonWaterElemental = info.IsSwitched(8);

            from.SendGump(new FieldSetup_Rules_Spells(Handeling));
        }
    }

    public class FieldSetup_Rules_Samurai : Gump
    {
        public Field Handeling;

        public FieldSetup_Rules_Samurai(Field d)
            : base(0, 0)
        {
            Handeling = d;

            Closable = true;
            Dragable = true;
            Resizable = false;

            AddBackground(219, 238, 172, 204, 9300);
            AddLabel(255, 244, 70, @"Samurai Spell Rules");

            AddCheck(225, 265, 210, 211, Handeling.AllowSamuraiSpells, 1);
            AddLabel(255, 265, 54, @"Allow Samurai Spells");
            AddCheck(225, 290, 210, 211, Handeling.Confidence, 2);
            AddLabel(255, 290, 54, @"Confidence");
            AddCheck(225, 315, 210, 211, Handeling.CounterAttack, 3);
            AddLabel(255, 315, 54, @"Counter Attack");
            AddCheck(225, 340, 210, 211, Handeling.Evasion, 4);
            AddLabel(255, 340, 54, @"Evasion");
            AddCheck(225, 365, 210, 211, Handeling.HonorableExecution, 5);
            AddLabel(255, 365, 54, @"Honorable Execution");
            AddCheck(225, 390, 210, 211, Handeling.LightningStrike, 6);
            AddLabel(255, 390, 54, @"Lightning Strike");
            AddCheck(225, 415, 210, 211, Handeling.MomentumStrike, 7);
            AddLabel(255, 415, 54, @"Momentum Strike");
        }

        public override void OnResponse(NetState sender, RelayInfo info)
        {
            Mobile from = (Mobile)sender.Mobile;

            from.SendGump(new FieldSetup_Rules(Handeling));

            Handeling.AllowSamuraiSpells = info.IsSwitched(1);
            Handeling.Confidence = info.IsSwitched(2);
            Handeling.CounterAttack = info.IsSwitched(3);
            Handeling.Evasion = info.IsSwitched(4);
            Handeling.HonorableExecution = info.IsSwitched(5);
            Handeling.LightningStrike = info.IsSwitched(6);
            Handeling.MomentumStrike = info.IsSwitched(7);
        }
    }

    public class FieldSetup_Rules_Chivalry : Gump
    {
        public Field Handeling;

        public FieldSetup_Rules_Chivalry(Field d)
            : base(0, 0)
        {
            Handeling = d;

            Closable = true;
            Dragable = true;
            Resizable = false;

            AddBackground(219, 238, 172, 304, 9300);
            AddLabel(255, 244, 70, @"Paladin Spell Rules");

            AddCheck(225, 265, 210, 211, Handeling.AllowChivalry, 1);
            AddLabel(255, 265, 54, @"Allow Chivalry");
            AddCheck(225, 290, 210, 211, Handeling.ClenseByFire, 2);
            AddLabel(255, 290, 54, @"Cleanse by Fire");
            AddCheck(225, 315, 210, 211, Handeling.CloseWounds, 3);
            AddLabel(255, 315, 54, @"Close Wounds");
            AddCheck(225, 340, 210, 211, Handeling.ConsecrateWeapon, 4);
            AddLabel(255, 340, 54, @"Consecrate Weapon");
            AddCheck(225, 365, 210, 211, Handeling.DispellEvil, 5);
            AddLabel(255, 365, 54, @"Dispell Evil");
            AddCheck(225, 390, 210, 211, Handeling.DivineFury, 6);
            AddLabel(255, 390, 54, @"Divine Fury");
            AddCheck(225, 415, 210, 211, Handeling.EnemyOfOne, 7);
            AddLabel(255, 415, 54, @"Enemy of One");
            AddCheck(225, 440, 210, 211, Handeling.HolyLight, 8);
            AddLabel(255, 440, 54, @"Holy Light");
            AddCheck(225, 465, 210, 211, Handeling.NobleSacrafice, 9);
            AddLabel(255, 465, 54, @"Noble Sacrafice");
            AddCheck(225, 490, 210, 211, Handeling.RemoveCurse, 10);
            AddLabel(255, 490, 54, @"Remove Curse");
            AddCheck(225, 515, 210, 211, Handeling.SacredJourny, 11);
            AddLabel(255, 515, 54, @"Sacred Journey");
        }

        public override void OnResponse(NetState sender, RelayInfo info)
        {
            Mobile from = (Mobile)sender.Mobile;

            from.SendGump(new FieldSetup_Rules(Handeling));

            Handeling.AllowChivalry = info.IsSwitched(1);
            Handeling.ClenseByFire = info.IsSwitched(2);
            Handeling.CloseWounds = info.IsSwitched(3);
            Handeling.ConsecrateWeapon = info.IsSwitched(4);
            Handeling.DispellEvil = info.IsSwitched(5);
            Handeling.DivineFury = info.IsSwitched(6);
            Handeling.EnemyOfOne = info.IsSwitched(7);
            Handeling.HolyLight = info.IsSwitched(8);
            Handeling.NobleSacrafice = info.IsSwitched(9);
            Handeling.RemoveCurse = info.IsSwitched(10);
            Handeling.SacredJourny = info.IsSwitched(11);
        }
    }

    public class FieldSetup_Rules_Ninjitsu : Gump
    {
        public Field Handeling;

        public FieldSetup_Rules_Ninjitsu(Field d)
            : base(0, 0)
        {
            Handeling = d;

            Closable = true;
            Dragable = true;
            Resizable = false;

            AddBackground(219, 238, 172, 254, 9300);
            AddLabel(255, 244, 70, @"Ninja Spell Rules");

            AddCheck(225, 265, 210, 211, Handeling.AllowNinjaSpells, 1);
            AddLabel(255, 265, 54, @"Allow Ninjitsu");
            AddCheck(225, 290, 210, 211, Handeling.AnimalForm, 2);
            AddLabel(255, 290, 54, @"Animal Form");
            AddCheck(225, 315, 210, 211, Handeling.Backstab, 3);
            AddLabel(255, 315, 54, @"Backstab");
            AddCheck(225, 340, 210, 211, Handeling.DeathStrike, 4);
            AddLabel(255, 340, 54, @"Death Strike");
            AddCheck(225, 365, 210, 211, Handeling.FocusAttack, 5);
            AddLabel(255, 365, 54, @"Focus Attack");
            AddCheck(225, 390, 210, 211, Handeling.KiAttack, 6);
            AddLabel(255, 390, 54, @"Ki Attack");
            AddCheck(225, 415, 210, 211, Handeling.MirrorImage, 7);
            AddLabel(255, 415, 54, @"Mirror Image");
            AddCheck(225, 440, 210, 211, Handeling.ShadowJump, 8);
            AddLabel(255, 440, 54, @"Shadow Jump");
            AddCheck(225, 465, 210, 211, Handeling.SurpriseAttack, 9);
            AddLabel(255, 465, 54, @"Surprise Attack");
        }

        public override void OnResponse(NetState sender, RelayInfo info)
        {
            Mobile from = (Mobile)sender.Mobile;

            from.SendGump(new FieldSetup_Rules(Handeling));

            Handeling.AllowNinjaSpells = info.IsSwitched(1);
            Handeling.AnimalForm = info.IsSwitched(2);
            Handeling.Backstab = info.IsSwitched(3);
            Handeling.DeathStrike = info.IsSwitched(4);
            Handeling.FocusAttack = info.IsSwitched(5);
            Handeling.KiAttack = info.IsSwitched(6);
            Handeling.MirrorImage = info.IsSwitched(7);
            Handeling.ShadowJump = info.IsSwitched(8);
            Handeling.SurpriseAttack = info.IsSwitched(9);
        }
    }

    public class FieldSetup_Rules_Necromancy : Gump
    {
        public Field Handeling;

        public FieldSetup_Rules_Necromancy(Field d)
            : base(0, 0)
        {
            Handeling = d;

            Closable = true;
            Dragable = true;
            Resizable = false;

            AddBackground(219, 238, 172, 304, 9300);
            AddLabel(255, 244, 70, @"Paladin Spell Rules");

            AddCheck(225, 265, 210, 211, Handeling.AllowNecromancy, 1);
            AddLabel(255, 265, 54, @"Allow Necromancy");
            AddCheck(225, 290, 210, 211, Handeling.AnimateDead, 2);
            AddLabel(255, 290, 54, @"Animate Dead");
            AddCheck(225, 315, 210, 211, Handeling.BloodOath, 3);
            AddLabel(255, 315, 54, @"Blood Oath");
            AddCheck(225, 340, 210, 211, Handeling.CorpseSkin, 4);
            AddLabel(255, 340, 54, @"Corpse Skin");
            AddCheck(225, 365, 210, 211, Handeling.CurseWeapon, 5);
            AddLabel(255, 365, 54, @"Curse Weapon");
            AddCheck(225, 390, 210, 211, Handeling.EvilOmen, 6);
            AddLabel(255, 390, 54, @"Evil Omen");
            AddCheck(225, 415, 210, 211, Handeling.Exorcisim, 7);
            AddLabel(255, 415, 54, @"Exorcism");
            AddCheck(225, 440, 210, 211, Handeling.HorrificBeast, 8);
            AddLabel(255, 440, 54, @"Horrific Beast");
            AddCheck(225, 465, 210, 211, Handeling.LichForm, 9);
            AddLabel(255, 465, 54, @"Lich Form");
            AddCheck(225, 490, 210, 211, Handeling.MindRot, 10);
            AddLabel(255, 490, 54, @"Mind Rot");
            AddCheck(225, 515, 210, 211, Handeling.PainSpike, 11);
            AddLabel(255, 515, 54, @"Pain Spike");
            AddCheck(225, 540, 210, 211, Handeling.PoisonStrike, 12);
            AddLabel(255, 540, 54, @"Poison Strike");
            AddCheck(225, 565, 210, 211, Handeling.Strangle, 13);
            AddLabel(255, 565, 54, @"Strangle");
            AddCheck(225, 590, 210, 211, Handeling.SummonFamiliar, 14);
            AddLabel(255, 590, 54, @"Summon Familiar");
            AddCheck(225, 615, 210, 211, Handeling.VampiricEmbrace, 15);
            AddLabel(255, 615, 54, @"Vampiric Embrace");
            AddCheck(225, 640, 210, 211, Handeling.VengefulSpirit, 16);
            AddLabel(255, 640, 54, @"Vengeful Spirit");
            AddCheck(225, 665, 210, 211, Handeling.Wither, 17);
            AddLabel(255, 665, 54, @"Wither");
            AddCheck(225, 690, 210, 211, Handeling.WraithForm, 18);
            AddLabel(255, 690, 54, @"Wraith Form");
        }

        public override void OnResponse(NetState sender, RelayInfo info)
        {
            Mobile from = (Mobile)sender.Mobile;

            from.SendGump(new FieldSetup_Rules(Handeling));

            Handeling.AllowNecromancy = info.IsSwitched(1);
            Handeling.AnimateDead = info.IsSwitched(2);
            Handeling.BloodOath = info.IsSwitched(3);
            Handeling.CorpseSkin = info.IsSwitched(4);
            Handeling.CurseWeapon = info.IsSwitched(5);
            Handeling.EvilOmen = info.IsSwitched(6);
            Handeling.Exorcisim = info.IsSwitched(7);
            Handeling.HorrificBeast = info.IsSwitched(8);
            Handeling.LichForm = info.IsSwitched(9);
            Handeling.MindRot = info.IsSwitched(10);
            Handeling.PainSpike = info.IsSwitched(11);
            Handeling.PoisonStrike = info.IsSwitched(12);
            Handeling.Strangle = info.IsSwitched(13);
            Handeling.SummonFamiliar = info.IsSwitched(14);
            Handeling.VampiricEmbrace = info.IsSwitched(15);
            Handeling.VengefulSpirit = info.IsSwitched(16);
            Handeling.Wither = info.IsSwitched(17);
            Handeling.WraithForm = info.IsSwitched(18);
        }
    }
}
