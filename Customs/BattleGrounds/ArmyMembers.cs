/*
 Army System v1.0 Beta
 By: Shadow-Sigma
 
 If you have any questions or concerns, please leave me a private message on the RunUO forums (Username: Shadow-Sigma), or send me an e-mail at intranetworkster@gmail.com
 
 Enjoy!
 
 Please do not remove this comment from these scripts, thank you! :)
 */
using System;
using Server.Items;
using Server.Mobiles;
using Server.Spells;

namespace Server.Mobiles
{
	public class ArmyBaseMace : ArmyBase
	{

		[Constructable]
		public ArmyBaseMace(int Team) : base(Team, AIType.AI_Melee, FightMode.Closest, 15, 1, 0.2, 0.6)
		{

            // An ArmyBase Hammerman
            int iHue = 20 + Team * 40;
            int jHue = 25 + Team * 40;

            // Skills and Stats
            this.InitStats((90 + Utility.Random(35)), (90 + Utility.Random(35)), (75 + Utility.Random(15)));
            this.Skills[SkillName.Macing].Base = (90 + Utility.Random(30));
            this.Skills[SkillName.Anatomy].Base = (90 + Utility.Random(30));
            this.Skills[SkillName.Healing].Base = (90 + Utility.Random(30));
            this.Skills[SkillName.Tactics].Base = (90 + Utility.Random(30));


            // Name
            this.Name = "Hammerman";

			// Equip
			WarHammer war = new WarHammer();
			war.Movable = true;
			war.Crafter = this;
			war.Quality = WeaponQuality.Regular;
			AddItem( war );

			Boots bts = new Boots();
			bts.Hue = iHue;
			AddItem( bts );

			ChainChest cht = new ChainChest();
			cht.Movable = false;
			cht.LootType = LootType.Newbied;
			cht.Crafter = this;
			cht.Quality = ArmorQuality.Regular;
			AddItem( cht );

			ChainLegs chl = new ChainLegs();
			chl.Movable = false;
			chl.LootType = LootType.Newbied;
			chl.Crafter = this;
			chl.Quality = ArmorQuality.Regular;
			AddItem( chl );

			PlateArms pla = new PlateArms();
			pla.Movable = false;
			pla.LootType = LootType.Newbied;
			pla.Crafter = this;
			pla.Quality = ArmorQuality.Regular;
			AddItem( pla );

			Bandage band = new Bandage( 50 );
			AddToBackpack( band );
		}

		public ArmyBaseMace( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();
		}
	}

	public class ArmyBaseFence : ArmyBase
	{

		[Constructable]
		public ArmyBaseFence(int Team) : base(Team, AIType.AI_Melee, FightMode.Closest, 15, 1, 0.2, 0.6)
		{
            // A ArmyBase Fencer
            int iHue = 20 + Team * 40;
            int jHue = 25 + Team * 40;

            // Skills and Stats
            this.InitStats((90 + Utility.Random(35)), (90 + Utility.Random(35)), (75 + Utility.Random(15)));
            this.Skills[SkillName.Fencing].Base = (90 + Utility.Random(30));
            this.Skills[SkillName.Anatomy].Base = (90 + Utility.Random(30));
            this.Skills[SkillName.Healing].Base = (90 + Utility.Random(30));
            this.Skills[SkillName.Tactics].Base = (90 + Utility.Random(30));

            // Name
            this.Name = "Fencer";

			// Equip
			Spear ssp = new Spear();
			ssp.Movable = true;
			ssp.Crafter = this;
			ssp.Quality = WeaponQuality.Regular;
			AddItem( ssp );

			Boots snd = new Boots();
			snd.Hue = iHue;
			snd.LootType = LootType.Newbied;
			AddItem( snd );

			ChainChest cht = new ChainChest();
			cht.Movable = false;
			cht.LootType = LootType.Newbied;
			cht.Crafter = this;
			cht.Quality = ArmorQuality.Regular;
			AddItem( cht );

			ChainLegs chl = new ChainLegs();
			chl.Movable = false;
			chl.LootType = LootType.Newbied;
			chl.Crafter = this;
			chl.Quality = ArmorQuality.Regular;
			AddItem( chl );

			PlateArms pla = new PlateArms();
			pla.Movable = false;
			pla.LootType = LootType.Newbied;
			pla.Crafter = this;
			pla.Quality = ArmorQuality.Regular;
			AddItem( pla );

			Bandage band = new Bandage( 50 );
			AddToBackpack( band );
		}

		public ArmyBaseFence( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();
		}
	}

	public class ArmyBaseSword : ArmyBase
	{

		[Constructable]
		public ArmyBaseSword(int Team) : base(Team, AIType.AI_Melee, FightMode.Closest, 15, 1, 0.2, 0.6)
		{
            // An ArmyBase Swordsman
            int iHue = 20 + Team * 40;
            int jHue = 25 + Team * 40;

            // Skills and Stats
            this.InitStats((90 + Utility.Random(35)), (90 + Utility.Random(35)), (75 + Utility.Random(15)));
            this.Skills[SkillName.Swords].Base = (90 + Utility.Random(30));
            this.Skills[SkillName.Anatomy].Base = (90 + Utility.Random(30));
            this.Skills[SkillName.Healing].Base = (90 + Utility.Random(30));
            this.Skills[SkillName.Tactics].Base = (90 + Utility.Random(30));
            this.Skills[SkillName.Parry].Base = (90 + Utility.Random(30));


            // Name
            this.Name = "Swordsman";

			// Equip
			Katana kat = new Katana();
			kat.Crafter = this;
			kat.Movable = true;
			kat.Quality = WeaponQuality.Regular;
			AddItem( kat );

			Boots bts = new Boots();
			bts.Hue = iHue;
			AddItem( bts );

			ChainChest cht = new ChainChest();
			cht.Movable = false;
			cht.LootType = LootType.Newbied;
			cht.Crafter = this;
			cht.Quality = ArmorQuality.Regular;
			AddItem( cht );

			ChainLegs chl = new ChainLegs();
			chl.Movable = false;
			chl.LootType = LootType.Newbied;
			chl.Crafter = this;
			chl.Quality = ArmorQuality.Regular;
			AddItem( chl );

			PlateArms pla = new PlateArms();
			pla.Movable = false;
			pla.LootType = LootType.Newbied;
			pla.Crafter = this;
			pla.Quality = ArmorQuality.Regular;
			AddItem( pla );

			Bandage band = new Bandage( 50 );
			AddToBackpack( band );
		}

		public ArmyBaseSword( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();
		}
	}

	public class ArmyBaseNox : ArmyBase
	{

		[Constructable]
		public ArmyBaseNox(int Team) : base(Team, AIType.AI_Mage, FightMode.Closest, 15, 1, 0.2, 0.6)
		{

            // An ArmyBase Nox Mage
            int iHue = 20 + Team * 40;
            int jHue = 25 + Team * 40;

            // Skills and Stats
            this.InitStats((75 + Utility.Random(15)), (75 + Utility.Random(15)), (90 + Utility.Random(35)));
            this.Skills[SkillName.Magery].Base = (90 + Utility.Random(30));
            this.Skills[SkillName.EvalInt].Base = (90 + Utility.Random(30));
            this.Skills[SkillName.Inscribe].Base = (80 + Utility.Random(20));
            this.Skills[SkillName.Wrestling].Base = (90 + Utility.Random(30));
            this.Skills[SkillName.Meditation].Base = (90 + Utility.Random(30));
            this.Skills[SkillName.Poisoning].Base = (80 + Utility.Random(20));


            // Name
            this.Name = "Nox Mage";

			// Equip
			Spellbook book = new Spellbook();
			book.Movable = false;
			book.LootType = LootType.Newbied;
			book.Content =0xFFFFFFFFFFFFFFFF;
			AddItem( book );

			Kilt kilt = new Kilt();
			kilt.Hue = jHue;
			AddItem( kilt );

			Sandals snd = new Sandals();
			snd.Hue = iHue;
			snd.LootType = LootType.Newbied;
			AddItem( snd );

			SkullCap skc = new SkullCap();
			skc.Hue = iHue;
			AddItem( skc );

			// Spells
			AddSpellAttack( typeof(Spells.First.MagicArrowSpell) );
			AddSpellAttack( typeof(Spells.First.WeakenSpell) );
			AddSpellAttack( typeof(Spells.Third.FireballSpell) );
			AddSpellDefense( typeof(Spells.Third.WallOfStoneSpell) );
			AddSpellDefense( typeof(Spells.First.HealSpell) );
		}

		public ArmyBaseNox( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();
		}
	}

	public class ArmyBaseStun : ArmyBase
	{

		[Constructable]
		public ArmyBaseStun(int Team) : base(Team, AIType.AI_Mage, FightMode.Closest, 15, 1, 0.2, 0.6)
		{

            // An ArmyBase Stun Mage
            int iHue = 20 + Team * 40;
            int jHue = 25 + Team * 40;

            // Skills and Stats
            this.InitStats((75 + Utility.Random(15)), (75 + Utility.Random(15)), (90 + Utility.Random(35)));
            this.Skills[SkillName.Magery].Base = (80 + Utility.Random(20));
            this.Skills[SkillName.EvalInt].Base = (90 + Utility.Random(30));
            this.Skills[SkillName.Anatomy].Base = (70 + Utility.Random(10));
            this.Skills[SkillName.Wrestling].Base = (70 + Utility.Random(10));
            this.Skills[SkillName.Meditation].Base = (90 + Utility.Random(30));
            this.Skills[SkillName.Poisoning].Base = (90 + Utility.Random(30));


            // Name
            this.Name = "Stun Mage";

			// Equip
			Spellbook book = new Spellbook();
			book.Movable = false;
			book.LootType = LootType.Newbied;
			book.Content =0xFFFFFFFFFFFFFFFF;
			AddItem( book );

			LeatherArms lea = new LeatherArms();
			lea.Movable = false;
			lea.LootType = LootType.Newbied;
			lea.Crafter = this;
			lea.Quality = ArmorQuality.Regular;
			AddItem( lea );

			LeatherChest lec = new LeatherChest();
			lec.Movable = false;
			lec.LootType = LootType.Newbied;
			lec.Crafter = this;
			lec.Quality = ArmorQuality.Regular;
			AddItem( lec );

			LeatherGorget leg = new LeatherGorget();
			leg.Movable = false;
			leg.LootType = LootType.Newbied;
			leg.Crafter = this;
			leg.Quality = ArmorQuality.Regular;
			AddItem( leg );

			LeatherLegs lel = new LeatherLegs();
			lel.Movable = false;
			lel.LootType = LootType.Newbied;
			lel.Crafter = this;
			lel.Quality = ArmorQuality.Regular;
			AddItem( lel );

			Boots bts = new Boots();
			bts.Hue = iHue;
			AddItem( bts );

			Cap cap = new Cap();
			cap.Hue = iHue;
			AddItem( cap );

			// Spells
			AddSpellAttack( typeof(Spells.First.MagicArrowSpell) );
			AddSpellAttack( typeof(Spells.First.WeakenSpell) );
			AddSpellAttack( typeof(Spells.Third.FireballSpell) );
			AddSpellDefense( typeof(Spells.Third.WallOfStoneSpell) );
			AddSpellDefense( typeof(Spells.First.HealSpell) );
		}

		public ArmyBaseStun( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();
		}
	}

	public class ArmyBaseSuper : ArmyBase
	{

		[Constructable]
		public ArmyBaseSuper(int Team) : base(Team, AIType.AI_Mage, FightMode.Closest, 15, 1, 0.2, 0.6)
		{
            // An ArmyBase Super Mage
            int iHue = 20 + Team * 40;
            int jHue = 25 + Team * 40;

            // Skills and Stats
            this.InitStats((90 + Utility.Random(35)), (90 + Utility.Random(35)), (90 + Utility.Random(35)));
            this.Skills[SkillName.Magery].Base = (90 + Utility.Random(30));
            this.Skills[SkillName.EvalInt].Base = (90 + Utility.Random(30));
            this.Skills[SkillName.Anatomy].Base = (90 + Utility.Random(30));
            this.Skills[SkillName.Wrestling].Base = (90 + Utility.Random(30));
            this.Skills[SkillName.Meditation].Base = (90 + Utility.Random(30));
            this.Skills[SkillName.Poisoning].Base = (80 + Utility.Random(20));
            this.Skills[SkillName.Inscribe].Base = (80 + Utility.Random(20));

            // Name
            this.Name = "Super Mage";

			// Equip
			Spellbook book = new Spellbook();
			book.Movable = false;
			book.LootType = LootType.Newbied;
			book.Content =0xFFFFFFFFFFFFFFFF;
			AddItem( book );

			LeatherArms lea = new LeatherArms();
			lea.Movable = false;
			lea.LootType = LootType.Newbied;
			lea.Crafter = this;
			lea.Quality = ArmorQuality.Regular;
			AddItem( lea );

			LeatherChest lec = new LeatherChest();
			lec.Movable = false;
			lec.LootType = LootType.Newbied;
			lec.Crafter = this;
			lec.Quality = ArmorQuality.Regular;
			AddItem( lec );

			LeatherGorget leg = new LeatherGorget();
			leg.Movable = false;
			leg.LootType = LootType.Newbied;
			leg.Crafter = this;
			leg.Quality = ArmorQuality.Regular;
			AddItem( leg );

			LeatherLegs lel = new LeatherLegs();
			lel.Movable = false;
			lel.LootType = LootType.Newbied;
			lel.Crafter = this;
			lel.Quality = ArmorQuality.Regular;
			AddItem( lel );

			Sandals snd = new Sandals();
			snd.Hue = iHue;
			snd.LootType = LootType.Newbied;
			AddItem( snd );

			JesterHat jhat = new JesterHat();
			jhat.Hue = iHue;
			AddItem( jhat );

			Doublet dblt = new Doublet();
			dblt.Hue = iHue;
			AddItem( dblt );

			// Spells
			AddSpellAttack( typeof(Spells.First.MagicArrowSpell) );
			AddSpellAttack( typeof(Spells.First.WeakenSpell) );
			AddSpellAttack( typeof(Spells.Third.FireballSpell) );
			AddSpellDefense( typeof(Spells.Third.WallOfStoneSpell) );
			AddSpellDefense( typeof(Spells.First.HealSpell) );
		}

		public ArmyBaseSuper( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();
		}
	}

	public class ArmyBaseHealer : ArmyBase
	{

		[Constructable]
		public ArmyBaseHealer(int Team) : base(Team, AIType.AI_Healer, FightMode.Closest, 15, 1, 0.2, 0.6)
		{
            // An ArmyBase Healer Mage
            int iHue = 20 + Team * 40;
            int jHue = 25 + Team * 40;

            // Skills and Stats
            this.InitStats((90 + Utility.Random(35)), (90 + Utility.Random(35)), (90 + Utility.Random(35)));
            this.Skills[SkillName.Magery].Base = (90 + Utility.Random(30));
            this.Skills[SkillName.EvalInt].Base = (90 + Utility.Random(30));
            this.Skills[SkillName.Anatomy].Base = (90 + Utility.Random(30));
            this.Skills[SkillName.Wrestling].Base = (90 + Utility.Random(30));
            this.Skills[SkillName.Meditation].Base = (90 + Utility.Random(30));
            this.Skills[SkillName.Healing].Base = (80 + Utility.Random(20));

            // Name
            this.Name = "Healer";

			// Equip
			Spellbook book = new Spellbook();
			book.Movable = false;
			book.LootType = LootType.Newbied;
			book.Content =0xFFFFFFFFFFFFFFFF;
			AddItem( book );

			LeatherArms lea = new LeatherArms();
			lea.Movable = false;
			lea.LootType = LootType.Newbied;
			lea.Crafter = this;
			lea.Quality = ArmorQuality.Regular;
			AddItem( lea );

			LeatherChest lec = new LeatherChest();
			lec.Movable = false;
			lec.LootType = LootType.Newbied;
			lec.Crafter = this;
			lec.Quality = ArmorQuality.Regular;
			AddItem( lec );

			LeatherGorget leg = new LeatherGorget();
			leg.Movable = false;
			leg.LootType = LootType.Newbied;
			leg.Crafter = this;
			leg.Quality = ArmorQuality.Regular;
			AddItem( leg );

			LeatherLegs lel = new LeatherLegs();
			lel.Movable = false;
			lel.LootType = LootType.Newbied;
			lel.Crafter = this;
			lel.Quality = ArmorQuality.Regular;
			AddItem( lel );

			Sandals snd = new Sandals();
			snd.Hue = iHue;
			snd.LootType = LootType.Newbied;
			AddItem( snd );

			Cap cap = new Cap();
			cap.Hue = iHue;
			AddItem( cap );

			Robe robe = new Robe();
			robe.Hue = iHue;
			AddItem( robe );

		}

		public ArmyBaseHealer( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();
		}
	}

	public class ArmyBaseAssassin : ArmyBase
	{

		[Constructable]
		public ArmyBaseAssassin(int Team) : base(Team, AIType.AI_Melee, FightMode.Closest, 15, 1, 0.2, 0.6)
		{
            // An ArmyBase Assassin
            int iHue = 20 + Team * 40;
            int jHue = 25 + Team * 40;

            // Skills and Stats
            this.InitStats((90 + Utility.Random(15)), (90 + Utility.Random(15)), (90 + Utility.Random(15)));
            this.Skills[SkillName.Magery].Base = (90 + Utility.Random(30));
            this.Skills[SkillName.EvalInt].Base = (90 + Utility.Random(30));
            this.Skills[SkillName.Swords].Base = (90 + Utility.Random(30));
            this.Skills[SkillName.Tactics].Base = (90 + Utility.Random(30));
            this.Skills[SkillName.Meditation].Base = (90 + Utility.Random(30));
            this.Skills[SkillName.Poisoning].Base = (80 + Utility.Random(20));

            // Name
            this.Name = "Assassin";

			// Equip
			Spellbook book = new Spellbook();
			book.Movable = false;
			book.LootType = LootType.Newbied;
			book.Content =0xFFFFFFFFFFFFFFFF;
			AddToBackpack( book );

			Katana kat = new Katana();
			kat.Movable = false;
			kat.LootType = LootType.Newbied;
			kat.Crafter = this;
			kat.Poison = Poison.Deadly;
			kat.PoisonCharges = 12;
			kat.Quality = WeaponQuality.Regular;
			AddToBackpack( kat );

			LeatherArms lea = new LeatherArms();
			lea.Movable = false;
			lea.LootType = LootType.Newbied;
			lea.Crafter = this;
			lea.Quality = ArmorQuality.Regular;
			AddItem( lea );

			LeatherChest lec = new LeatherChest();
			lec.Movable = false;
			lec.LootType = LootType.Newbied;
			lec.Crafter = this;
			lec.Quality = ArmorQuality.Regular;
			AddItem( lec );

			LeatherGorget leg = new LeatherGorget();
			leg.Movable = false;
			leg.LootType = LootType.Newbied;
			leg.Crafter = this;
			leg.Quality = ArmorQuality.Regular;
			AddItem( leg );

			LeatherLegs lel = new LeatherLegs();
			lel.Movable = false;
			lel.LootType = LootType.Newbied;
			lel.Crafter = this;
			lel.Quality = ArmorQuality.Regular;
			AddItem( lel );

			Sandals snd = new Sandals();
			snd.Hue = iHue;
			snd.LootType = LootType.Newbied;
			AddItem( snd );

			Cap cap = new Cap();
			cap.Hue = iHue;
			AddItem( cap );

			Robe robe = new Robe();
			robe.Hue = iHue;
			AddItem( robe );

			DeadlyPoisonPotion pota = new DeadlyPoisonPotion();
			pota.LootType = LootType.Newbied;
			AddToBackpack( pota );

			DeadlyPoisonPotion potb = new DeadlyPoisonPotion();
			potb.LootType = LootType.Newbied;
			AddToBackpack( potb );

			DeadlyPoisonPotion potc = new DeadlyPoisonPotion();
			potc.LootType = LootType.Newbied;
			AddToBackpack( potc );

			DeadlyPoisonPotion potd = new DeadlyPoisonPotion();
			potd.LootType = LootType.Newbied;
			AddToBackpack( potd );

			Bandage band = new Bandage( 50 );
			AddToBackpack( band );

		}

		public ArmyBaseAssassin( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();
		}
	}

	public class ArmyBaseThief : ArmyBase
	{

		[Constructable]
		public ArmyBaseThief(int Team) : base(Team, AIType.AI_Thief, FightMode.Closest, 15, 1, 0.2, 0.6)
		{
            // An ArmyBase Hybrid Thief
            int iHue = 20 + Team * 40;
            int jHue = 25 + Team * 40;

            // Skills and Stats
            this.InitStats((90 + Utility.Random(15)), (90 + Utility.Random(15)), (90 + Utility.Random(15)));
            this.Skills[SkillName.Healing].Base = (90 + Utility.Random(30));
            this.Skills[SkillName.Anatomy].Base = (90 + Utility.Random(30));
            this.Skills[SkillName.Stealing].Base = (90 + Utility.Random(30));
            this.Skills[SkillName.ArmsLore].Base = (80 + Utility.Random(20));
            this.Skills[SkillName.Meditation].Base = (90 + Utility.Random(30));
            this.Skills[SkillName.Wrestling].Base = (90 + Utility.Random(30));

            // Name
            this.Name = "Thief";

			// Equip
			Spellbook book = new Spellbook();
			book.Movable = false;
			book.LootType = LootType.Newbied;
			book.Content =0xFFFFFFFFFFFFFFFF;
			AddItem( book );

			LeatherArms lea = new LeatherArms();
			lea.Movable = false;
			lea.LootType = LootType.Newbied;
			lea.Crafter = this;
			lea.Quality = ArmorQuality.Regular;
			AddItem( lea );

			LeatherChest lec = new LeatherChest();
			lec.Movable = false;
			lec.LootType = LootType.Newbied;
			lec.Crafter = this;
			lec.Quality = ArmorQuality.Regular;
			AddItem( lec );

			LeatherGorget leg = new LeatherGorget();
			leg.Movable = false;
			leg.LootType = LootType.Newbied;
			leg.Crafter = this;
			leg.Quality = ArmorQuality.Regular;
			AddItem( leg );

			LeatherLegs lel = new LeatherLegs();
			lel.Movable = false;
			lel.LootType = LootType.Newbied;
			lel.Crafter = this;
			lel.Quality = ArmorQuality.Regular;
			AddItem( lel );

			Sandals snd = new Sandals();
			snd.Hue = iHue;
			snd.LootType = LootType.Newbied;
			AddItem( snd );

			Cap cap = new Cap();
			cap.Hue = iHue;
			AddItem( cap );

			Robe robe = new Robe();
			robe.Hue = iHue;
			AddItem( robe );

			Bandage band = new Bandage( 50 );
			AddToBackpack( band );
		}

		public ArmyBaseThief( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();
		}
	}
}