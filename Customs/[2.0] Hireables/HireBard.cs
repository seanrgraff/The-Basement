using System; 
using System.Collections; 
using Server.Items; 
using Server.ContextMenus; 
using Server.Misc; 
using Server.Network; 

namespace Server.Mobiles 
{ 
    public class HireBard : BaseHire 
    { 
        [Constructable] 
        public HireBard()
        { 
            SpeechHue = Utility.RandomDyedHue(); 
            Hue = Utility.RandomSkinHue(); 

            if ( this.Female = Utility.RandomBool() ) 
            { 
                Body = 0x191; 
                Name = NameList.RandomName( "female" ); 

		        switch ( Utility.Random ( 2 ) )
		        {
			      case 0: AddItem( new Skirt ( Utility.RandomNeutralHue() ) ); break;
			      case 1: AddItem( new Kilt ( Utility.RandomNeutralHue() ) ); break;
		          }
            }
             
            else 
            { 
                Body = 0x190; 
                Name = NameList.RandomName( "male" ); 
                AddItem( new ShortPants( Utility.RandomNeutralHue() ) ); 
            }
            
        
		   Title = "the bard";
           Utility.AssignRandomHair( this );

            SetStr( 86, 100 ); 
            SetDex( 81, 100 ); 
            SetInt( 81, 100 ); 

            SetDamage( 10, 23 ); 

		SetSkill( SkillName.Tactics, 35, 57 ); 
		SetSkill( SkillName.Magery, 22, 22 );
		SetSkill( SkillName.Swords, 45, 67 );
		SetSkill( SkillName.Archery, 36, 67 );
		SetSkill( SkillName.Parry, 45, 60 );
		SetSkill( SkillName.Musicianship, 66.0, 97.5 );
		SetSkill( SkillName.Peacemaking, 65.0, 87.5 );

            Fame = 100; 
            Karma = 100; 

            AddItem( new Shoes( Utility.RandomNeutralHue() ) ); 

		switch ( Utility.Random( 2 ) )
		{
			case 0: AddItem( new Doublet( Utility.RandomDyedHue() ) ); break;
			case 1: AddItem( new Shirt( Utility.RandomDyedHue() ) ); break;
		}
		switch ( Utility.Random( 4 ) )
		{
			case 0: PackItem( new Harp() ); break;
			case 1: PackItem( new Lute() ); break;
			case 2: PackItem( new Drums() ); break;
			case 3: PackItem( new Tambourine() ); break;
		}

			AddItem( new Longsword() ); 
			PackItem( new Bow() ); 
			PackItem( new Arrow(100) );
            PackGold( 10, 50 ); 
        
        }
	public override bool ClickTitle{ get{ return false; } }
        public HireBard( Serial serial ) : base( serial ) 
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
