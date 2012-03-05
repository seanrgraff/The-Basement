/*Scripted by  _____         
*	  		   \_   \___ ___ 
*			    / /\/ __/ _ \
*		     /\/ /_| (_|  __/
*			 \____/ \___\___|
*/

using System;
using Server.Misc;
using Server.Items;
using Server.Mobiles;
using Server.Targeting;
using System.Collections;
using Server.Engines.Craft;

namespace Server.Mobiles
{
    [CorpseName("a human corpse")]
    public class Cracker : BaseCreature
    {
	    public override int GetIdleSound()
		{
			return 1055; 
		}

		public override int GetDeathSound()
		{
			return 814; //Yea, I know he screams like a little girl when he gets killed.
		}

		public override int GetHurtSound()
		{
			return 1070;
		}

		public override int GetAngerSound()
		{
			return 1098; 
		}
	    private Timer m_Timer; 
	    [Constructable]
        public Cracker() 
        : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {	        
	        Name = NameList.RandomName( "male" );
	        Title = "The vicious murderer";
            Body = 0x190;
            Kills = 5;
			Hue = Utility.RandomSkinHue();

			SetStr(276, 350);
			SetDex(150, 200);
			SetInt(126, 150);

			SetHits(350, 400);

			SetDamage(11, 14);

			SetResistance( ResistanceType.Physical, 5 );
			SetDamageType(ResistanceType.Physical, 100);

			SetSkill(SkillName.Tactics, 80.1, 100.0);
			SetSkill(SkillName.MagicResist, 100.1, 110.0);
			SetSkill(SkillName.Anatomy, 80.1, 100.0);
			SetSkill(SkillName.Healing, 80.1, 100.0);
			SetSkill(SkillName.Fencing, 85.1, 100);

			Fame = 9000;
			Karma = -15000;

            PackItem(new Apple(Utility.RandomMinMax(3, 5)));
            PackItem(new Bandage(Utility.RandomMinMax(15, 55)));

            	CrackerDagger chd = new CrackerDagger();
				chd.Crafter = this;
				chd.Name = this.Name + "'s Dagger";
            	CrackerBuckler chb = new CrackerBuckler();
				chb.Crafter = this;
				chb.Name = this.Name + "'s Buckler";
            if (0.5 > Utility.RandomDouble())
				AddItem(chd);
            else
                AddItem(new Dagger());
           
            if (0.10 > Utility.RandomDouble())
				AddItem(chb);
            else
                AddItem(new Buckler());
           
            //if (0.15 > Utility.RandomDouble())
			//	 AddItem(new RandomAddonTicket());
                
			//m_Timer = new InternalTimer( this );
			//m_Timer.Start();
			
			//CraftResource res = CraftResource.None;;
			//switch (Utility.Random(4))
			//{
			//	case 0: res = CraftResource.RegularLeather; break;
			//	case 1: res = CraftResource.SpinedLeather; break;
			//	case 2: res = CraftResource.HornedLeather; break;
			//	case 3: res = CraftResource.BarbedLeather; break;
			//}
            //if (0.05 > Utility.RandomDouble())
            //    AddItem(new RunicSewingKit(res, Utility.RandomMinMax(1, 5)));
            //else
            //   AddItem(new SewingKit());
                
			LeatherChest Tunic = new LeatherChest();
			Tunic.ItemID = 9859;
			Tunic.Name = "Leather Hood Robe";
			Tunic.Layer = Layer.OuterTorso;
			//Tunic.Resource = res;
			Tunic.Movable = false;
			Tunic.Quality = ArmorQuality.Exceptional;
			Tunic.Crafter = this;
			AddItem(Tunic);

			LeatherLegs Legs = new LeatherLegs();
			//Legs.Resource = res;
			Legs.Movable = false;
			AddItem(Legs);
			
			Boots boot = new Boots();
			//boot.Resource = res;
			boot.Movable = false;
			AddItem(boot);

			LeatherArms Arms = new LeatherArms();
			//Arms.Resource = res;
			Arms.Movable = false;
			AddItem(Arms);

			LeatherGloves Gloves = new LeatherGloves();
			//Gloves.Resource = res;
			Gloves.Movable = false;
			Gloves.Quality = ArmorQuality.Exceptional;
			Gloves.Crafter = this;
			AddItem(Gloves);

			LeatherCap Helm = new LeatherCap();
			//Helm.Resource = res;
			Helm.Movable = false;
			Helm.Quality = ArmorQuality.Exceptional;
			Helm.Crafter = this;
			AddItem(Helm);

			Head hd = new Head();
			hd.Name = "the head of " + NameList.RandomName( "male" );
			AddItem(hd);
			
			//EtherealOstard mt = new EtherealOstard();
			//mt.Hue = Utility.RandomNeutralHue();
			//mt.Movable = false;
			//mt.Rider = this;
        }
        public override void OnDamage( int amount, Mobile from, bool willKill )
		{
			if ( from != null && !willKill && amount > 5 && from.Player && 5 > Utility.Random( 100 ) )
			{
				string[] toSay = new string[]
					{
						"My, Ostard is best!",
						"See you later!",
						"DIE {0}!!"
					};

				this.Say( true, String.Format( toSay[Utility.Random( toSay.Length )], from.Name ) );
			}

			base.OnDamage( amount, from, willKill );
		}
        public override void GenerateLoot()
        {
           AddLoot( LootPack.Rich, 10 );
        }		       
        public override WeaponAbility GetWeaponAbility()
		{
			return Utility.RandomBool() ? WeaponAbility.InfectiousStrike : WeaponAbility.ArmorIgnore;
		}     
        public override bool CanHeal { get { return true; } }
        public override bool CanRummageCorpses { get { return true; } }
        public override int Meat { get { return 2; } }

        public Cracker(Serial serial): base(serial)
        {
			Frozen = false;
			//m_Timer = new InternalTimer( this );
			//m_Timer.Start();
        }
        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0);
        }
        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
        private class InternalTimer : Timer
		{
			private Cracker m_Owner;
			private int m_Count = 0;

			public InternalTimer( Cracker owner ) : base( TimeSpan.FromSeconds( 0.40 ), TimeSpan.FromSeconds( 0.12 ) )
			{
				m_Owner = owner;
			}

			protected override void OnTick()
			{
/*				if(	m_Owner.Combatant == null )
*				{	
*					if ( (m_Count++ & 0x3) == 0 )
*					{
*						m_Owner.Direction = (Direction)(Utility.Random( 8 ) | 0x80);
*					}
*				}
*/				m_Owner.Move( m_Owner.Direction );
			}
		}
    }
}

namespace Server.Items
{
	public class CrackerBuckler : BaseShield
	{
		public override int BasePhysicalResistance{ get{ return 0; } }
		public override int BaseFireResistance{ get{ return 0; } }
		public override int BaseColdResistance{ get{ return 0; } }
		public override int BasePoisonResistance{ get{ return 1; } }
		public override int BaseEnergyResistance{ get{ return 0; } }

		public override int InitMinHits{ get{ return 40; } }
		public override int InitMaxHits{ get{ return 50; } }

		public override int AosStrReq{ get{ return 20; } }

		public override int ArmorBase{ get{ return 7; } }

		[Constructable]
		public CrackerBuckler() : base( 0x1B73 )
		{
			Weight = 5.0;
			
			Name = null;
			Quality = ArmorQuality.Exceptional;
			Attributes.DefendChance = 10;
			ArmorAttributes.SelfRepair = 2;
		}

		public CrackerBuckler( Serial serial ) : base(serial)
		{
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int)0 );//version
		}
	}
	[FlipableAttribute( 0xF52, 0xF51 )]
	public class CrackerDagger : BaseKnife
	{
		public override WeaponAbility PrimaryAbility{ get{ return WeaponAbility.InfectiousStrike; } }
		public override WeaponAbility SecondaryAbility{ get{ return WeaponAbility.ShadowStrike; } }

		public override int AosStrengthReq{ get{ return 12; } }
		public override int AosMinDamage{ get{ return 13; } }
		public override int AosMaxDamage{ get{ return 16; } }
		public override int AosSpeed{ get{ return 57; } }

		public override int OldStrengthReq{ get{ return 3; } }
		public override int OldMinDamage{ get{ return 6; } }
		public override int OldMaxDamage{ get{ return 16; } }
		public override int OldSpeed{ get{ return 57; } }

		public override int InitMinHits{ get{ return 31; } }
		public override int InitMaxHits{ get{ return 40; } }

		public override SkillName DefSkill{ get{ return SkillName.Fencing; } }
		public override WeaponType DefType{ get{ return WeaponType.Piercing; } }
		public override WeaponAnimation DefAnimation{ get{ return WeaponAnimation.Pierce1H; } }

		[Constructable]
		public CrackerDagger() : base( 0xF52 )
		{
			Weight = 1.0;
			Name = null;
			Poison = Poison.Lethal;
			PoisonCharges = 6;
			Quality = WeaponQuality.Exceptional;
			Attributes.WeaponSpeed = 20;
			Attributes.RegenStam = 2;
		}

		public CrackerDagger( Serial serial ) : base( serial )
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