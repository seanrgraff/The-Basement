using System; 
using System.Collections;
using System.Collections.Generic;
using Server.Items; 
using Server.ContextMenus; 
using Server.Misc; 
using Server.Network; 

namespace Server.Mobiles 
{ 
    public class HireThief : BaseHire 
    { 
        [Constructable] 
        public HireThief()
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
            
		    Title = "the thief";
           
            
        
			Utility.AssignRandomHair( this );			
			
		

            SetStr( 81, 95 ); 
            SetDex( 86, 100 ); 
            SetInt( 61, 75 ); 

            SetDamage( 10, 23 ); 

            SetSkill( SkillName.Stealing, 66.0, 97.5 ); 
            SetSkill( SkillName.Peacemaking, 65.0, 87.5 ); 
            SetSkill( SkillName.MagicResist, 25.0, 47.5 ); 
            SetSkill( SkillName.Healing, 65.0, 87.5 ); 
            SetSkill( SkillName.Tactics, 65.0, 87.5 ); 
            SetSkill( SkillName.Fencing, 65.0, 87.5 ); 
            SetSkill( SkillName.Parry, 45.0, 60.5 ); 
		SetSkill( SkillName.Lockpicking, 65, 87 );
		SetSkill( SkillName.Hiding, 65, 87 );
		SetSkill( SkillName.Snooping, 65, 87 );	
            Fame = 100; 
            Karma = 0; 

            AddItem( new Sandals( Utility.RandomNeutralHue() ) ); 
		AddItem( new Dagger() );
		switch ( Utility.Random( 2 ) )
		{
			case 0: AddItem( new Doublet( Utility.RandomNeutralHue() ) ); break;
			case 1: AddItem( new Shirt( Utility.RandomNeutralHue() ) ); break;
		}
		
            PackGold( 0, 25 ); 
        }
        
        
	public override bool ClickTitle{ get{ return false; } }
        public HireThief( Serial serial ) : base( serial ) 
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
