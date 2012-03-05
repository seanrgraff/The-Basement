using System;
using Server;
using Server.Items;
using Server.Factions;

namespace Server.Mobiles
{
	[CorpseName( "a Psycho's corpse" )]
	public class Psycho : BaseCreature
	{
		[Constructable]
		public Psycho() : base( AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = NameList.RandomName( "male" );
			Title = "the Psycho";
			Body = 183;
			BaseSoundID = 0x3E9;

			SetStr( 416, 505 );
			SetDex( 146, 165 );
			SetInt( 566, 655 );

			SetHits( 250, 303 );

			SetDamage( 11, 13 );

			SetDamageType( ResistanceType.Physical, 0 );
			SetDamageType( ResistanceType.Cold, 60 );
			SetDamageType( ResistanceType.Energy, 40 );

			SetResistance( ResistanceType.Physical, 40, 50 );
			SetResistance( ResistanceType.Fire, 30, 40 );
			SetResistance( ResistanceType.Cold, 50, 60 );
			SetResistance( ResistanceType.Poison, 50, 60 );
			SetResistance( ResistanceType.Energy, 40, 50 );

			//SetSkill( SkillName.Necromancy, 90, 110.0 );
			//SetSkill( SkillName.SpiritSpeak, 90.0, 110.0 );

			SetSkill( SkillName.EvalInt, 90.1, 100.0 );
			SetSkill( SkillName.Magery, 90.1, 100.0 );
			SetSkill( SkillName.MagicResist, 150.5, 200.0 );
			SetSkill( SkillName.Tactics, 50.1, 70.0 );
			SetSkill( SkillName.Wrestling, 60.1, 80.0 );
			SetSkill( SkillName.Fencing, 60.1, 80.0 );
			SetSkill( SkillName.Macing, 60.1, 80.0 );
			SetSkill( SkillName.Swords, 60.1, 80.0 );

			Fame = -8000;
			Karma = -8000;
			
			switch ( Utility.Random(10) ) // hand
			{
  				case 0: AddItem( new Kryss() ); break;
  				case 1: AddItem( new Katana() ); break;
  				case 2: AddItem( new Halberd() ); break;
  				case 3: AddItem( new Spear() ); break;
  				case 4: AddItem( new Dagger() ); break;
  				case 5: AddItem( new ButcherKnife() ); break;
  				case 6: AddItem( new Cleaver() ); break;
  				case 7: AddItem( new Club() ); break;
  				case 8: AddItem( new Broadsword() ); break;
  				case 9: AddItem( new VikingSword() ); break;
			}
			switch ( Utility.Random(11) ) // hat
			{
  				case 0: AddItem( new FloppyHat(Utility.RandomNeutralHue()) ); break;
  				case 1: AddItem( new WideBrimHat(Utility.RandomNeutralHue()) ); break;
  				case 2: AddItem( new Cap(Utility.RandomNeutralHue()) ); break;
  				case 3: AddItem( new SkullCap(Utility.RandomNeutralHue()) ); break;
  				case 4: AddItem( new Bandana(Utility.RandomNeutralHue()) ); break;
  				case 5: AddItem( new BearMask() ); break;
  				case 6: AddItem( new TallStrawHat(Utility.RandomNeutralHue()) ); break;
  				case 7: AddItem( new StrawHat(Utility.RandomNeutralHue()) ); break;
  				case 8: AddItem( new OrcishKinMask() ); break;
  				case 9: AddItem( new WizardsHat(Utility.RandomNeutralHue()) ); break;
  				case 10: break;
			}
			switch ( Utility.Random(7) ) // middletorso
			{
  				case 0: AddItem( new BodySash(Utility.RandomNeutralHue()) ); break;
  				case 1: AddItem( new FullApron(Utility.RandomNeutralHue()) ); break;
  				case 2: AddItem( new Doublet(Utility.RandomNeutralHue()) ); break;
  				case 3: AddItem( new Tunic(Utility.RandomNeutralHue()) ); break;
  				case 4: break;
  				case 5: AddItem( new JesterSuit(Utility.RandomNeutralHue()) ); break;
  				case 6: break;
			}
			switch ( Utility.Random(3) ) // outerlegs
			{
  				case 0: AddItem( new Skirt(Utility.RandomNeutralHue()) ); break;
  				case 1: AddItem( new Kilt(Utility.RandomNeutralHue()) ); break;
  				case 2: break;
			}
			switch ( Utility.Random(2) ) // Pants
			{
  				case 0: AddItem( new ShortPants(Utility.RandomNeutralHue()) ); break;
  				case 1: AddItem( new LongPants(Utility.RandomNeutralHue()) ); break;
			}
			switch ( Utility.Random(7) ) // Shirts
			{
  				case 0: AddItem( new FancyShirt(Utility.RandomNeutralHue()) ); break;
  				case 1: AddItem( new Shirt(Utility.RandomNeutralHue()) ); break;
  				case 2: AddItem( new Doublet(Utility.RandomNeutralHue()) ); break;
  				case 3: AddItem( new Tunic(Utility.RandomNeutralHue()) ); break;
  				case 4: AddItem( new FormalShirt(Utility.RandomNeutralHue()) ); break;
  				case 5: AddItem( new JesterSuit(Utility.RandomNeutralHue()) ); break;
  				case 6: break;
			}
			switch ( Utility.Random(2) ) // Shirts
			{
  				case 0: AddItem( new HalfApron(Utility.RandomNeutralHue()) ); break;
  				case 1: break;
			}
			switch ( Utility.Random(2) ) // Shirts
			{
  				case 0: AddItem( new Cloak(Utility.RandomNeutralHue()) ); break;
  				case 1: break;
			}
			PackItem( new BagOfReagents() );
			PackItem( new BagOfPotions() );
			PackNecroReg( 17, 24 );
		}

		public override void GenerateLoot()
		{
			AddLoot( LootPack.Rich );
			AddLoot( LootPack.MedScrolls, 10 );
		}
		
		public bool EquipWeapon()
		{
			Container pack = this.Backpack;

			if ( pack == null )
				return false;

			Item weapon = pack.FindItemByType( typeof( BaseWeapon ) );

			if ( weapon == null )
				return false;

			return this.EquipItem( weapon );
		}
		
		public override void OnThink()
		{
		}

		public override OppositionGroup OppositionGroup
		{
			get{ return OppositionGroup.FeyAndUndead; }
		}

		public override bool CanRummageCorpses{ get{ return true; } }	
		
		public Psycho( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );
			writer.Write( (int) 0 );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );
			int version = reader.ReadInt();
		}
	}
}