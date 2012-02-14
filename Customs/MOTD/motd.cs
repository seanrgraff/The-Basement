using System;
using Server;
using Server.Gumps;
using Server.Mobiles;

namespace Server.Gumps
{
	public class motd : Gump
	{
		public motd()
			: base( 0, 0 )
		{
            this.Closable = true;
            this.Disposable = false;
            this.Dragable = true;
            this.Resizable = false;
            this.AddPage(0);
            this.AddBackground(110, 129, 370, 46, 9270);
            this.AddBackground(110, 87, 157, 44, 9270);
            this.AddLabel(126, 98, 42, @"UoLamers Crybrid");
            this.AddLabel(125, 144, 17, String.Format(" Players Online: {0}", Server.Network.NetState.Instances.Count));
            //this.AddLabel(321, 141, 8, @"Play To Win!");
            this.AddBackground(110, 172, 370, 247, 9270);
            this.AddAlphaRegion(125, 187, 340, 216);
            this.AddLabel(131, 190, 890, @"News");
            this.AddHtml(133, 213, 324, 183, 
            
            	@"<basefont size=5>" 
            	
            					   +"<BASEFONT COLOR=#3300CC>Thursday 7/28/11<BASEFONT COLOR=#000000>"
            					   +"\n\n    Rabbits Vs Sheep is fully implemented. There are more expansions on the idea to come however, such as buying weapons/items with kill points. I am also working on a new Spellbook for an event. There will be blood. \n\n"
            					   +"<BASEFONT COLOR=#3300CC>Change Log:<BASEFONT COLOR=#000000>\n"
            					   +"-Added command to enter the rvs Arena: I wish to RVS \n"
            					   +"-Added points system to RVS\n"
            					   +"-RVS now removes EVs and BSs after each round. \n"
            					   +"-Added Impersonate and Bone Wall to New Spellbook. \n"
            					   +"                   <BASEFONT COLOR=#3300CC>Doob.<BASEFONT COLOR=#000000>\n\n" 
            	
            					   +"<BASEFONT COLOR=#3300CC>Tuesday 10/09/10<BASEFONT COLOR=#000000>"
            					   +"\n\n    Fixed a small issue with wands not showing properties. If anyone detects bugs you can always email wakkadoobie@gmail.com with details. \n\n"
            					   +"                   <BASEFONT COLOR=#3300CC>Doob.<BASEFONT COLOR=#000000>\n\n" 
            	
            					   +"<BASEFONT COLOR=#3300CC>Saturday 9/18/10<BASEFONT COLOR=#000000>"
            					   +"\n\n    Today I worked on a lot of the rudimentary game mechanics for RVS. \n\n"
            					   +"<BASEFONT COLOR=#3300CC>Change Log:<BASEFONT COLOR=#000000>\n"
            					   +"-More code for RvS\n"
            					   +"-Fixed issue with viewing wand rules in field/duels \n"
            					   +"                   <BASEFONT COLOR=#3300CC>Doob.<BASEFONT COLOR=#000000>\n\n" 
            	
            					   +"<BASEFONT COLOR=#3300CC>Thursday 9/16/10<BASEFONT COLOR=#000000>\n"
            					   +"<BASEFONT COLOR=#3300CC>Change Log:<BASEFONT COLOR=#000000>\n"
            					   +"-More code for RvS\n"
            					   +"-Coded AnkeOfResurrection\n"
            					   +"-Coded PotionOfInvulnerability\n"
            					   +"                   <BASEFONT COLOR=#3300CC>Doob.<BASEFONT COLOR=#000000>\n\n"
            					   
            					   +"<BASEFONT COLOR=#3300CC>Sunday 9/12/10<BASEFONT COLOR=#000000>"
            					   +"\n\n    All preliminary special Weapons and Armor have been coded for RvS, I am working on special items now. \n\n"
            					   +"<BASEFONT COLOR=#3300CC>Change Log:<BASEFONT COLOR=#000000>\n"
            					   +"-Coded Book of the Dead\n"
            					   +"                   <BASEFONT COLOR=#3300CC>Doob.<BASEFONT COLOR=#000000>\n\n" 
            	
            	            	   +"<BASEFONT COLOR=#3300CC>Wednesday 9/08/10<BASEFONT COLOR=#000000>\n"
            					   +"<BASEFONT COLOR=#3300CC>Change Log:<BASEFONT COLOR=#000000>\n"
            					   +"-More code for RvS\n"
            					   +"-Coded Lightning Bow\n"
            					   +"                   <BASEFONT COLOR=#3300CC>Doob.<BASEFONT COLOR=#000000>\n\n" 
            
            	          		   +"<BASEFONT COLOR=#3300CC>Sunday 9/05/10<BASEFONT COLOR=#000000>\n"
            					   +"<BASEFONT COLOR=#3300CC>Change Log:<BASEFONT COLOR=#000000>\n"
            					   +"-More code for RvS\n"
            					   +"-Coded Poison Kryss\n"
            					   +"-Coded Searing Spear\n"
            					   +"-Coded Meteor Mace\n"
            					   +"-Coded Crossbow of Death\n"
            					   +"-Coded Spinal Tap Bandana\n"
            					   +"                   <BASEFONT COLOR=#3300CC>Doob.<BASEFONT COLOR=#000000>\n\n" 
            	
            					   +"<BASEFONT COLOR=#3300CC>Sunday 8/29/10<BASEFONT COLOR=#000000>\n"
            					   +"<BASEFONT COLOR=#3300CC>Change Log:<BASEFONT COLOR=#000000>\n"
            					   +"-More code for RvS\n"
            					   +"-Coded Swift Katana\n"
            					   +"-Coded Staff of the Dead\n"
            					   +"                   <BASEFONT COLOR=#3300CC>Doob.<BASEFONT COLOR=#000000>\n\n" 
            					   
            					   +"<BASEFONT COLOR=#3300CC>Thursday 8/26/10<BASEFONT COLOR=#000000>"
            					   +"\n\n    The new game for the shard is going under the code word RvS and I have increased its scope so it will take longer than anticipated to complete. \n\n"
            					   +"<BASEFONT COLOR=#3300CC>Change Log:<BASEFONT COLOR=#000000>\n"
            					   +"-Coded mana and health 'Flies' for RvS\n"
            					   +"-Coded Speed Boots for RvS\n"
            					   +"-Coded Cleaving Halberd for RvS\n"
            					   +"                   <BASEFONT COLOR=#3300CC>Doob.<BASEFONT COLOR=#000000>\n\n" 
            	
            					   +"<BASEFONT COLOR=#3300CC>Friday 8/20/10<BASEFONT COLOR=#000000>\n"
            					   +"<BASEFONT COLOR=#3300CC>Change Log:<BASEFONT COLOR=#000000>\n"
            					   +"-Revamped the Skill Ball\n"
            					   +"                   <BASEFONT COLOR=#3300CC>Doob.<BASEFONT COLOR=#000000>\n\n" 
            					   
            					   +"<BASEFONT COLOR=#3300CC>Thursday 8/19/10<BASEFONT COLOR=#000000>"
            					   +"\n\n    Hope everyone is enjoying the field and it improves your survivability on Hybrid. I am scripting a new game for UOLamers, within the next two weeks I should have a prototype working. Stay patient and practice those bola tamers. \n\n"
            					   +"                   <BASEFONT COLOR=#3300CC>Doob.<BASEFONT COLOR=#000000>\n\n"   
            					   
            					   +"<BASEFONT COLOR=#3300CC>Wednesday 8/18/10<BASEFONT COLOR=#000000>"
            					   +"\n\n    Nick Successfully Hijacked the server for a short period of time this week. However, I was able to regain control and I have taken steps to make sure that does not happen again. \n\n"
            					   +"<BASEFONT COLOR=#3300CC>Change Log:<BASEFONT COLOR=#000000>\n"
            					   +"-Made Wands Identified\n"
            					   +"-Made Pets Fieldable\n"
            					   +"-Added Pet Bonding Deeds\n"
            					   +"-Added Pet Skill Balls\n"
            					   +"-Added Traps to Field Rules\n"
            					   +"-Added Wands to Field and Duel Rules\n"
            					   +"                   <BASEFONT COLOR=#3300CC>Doob.<BASEFONT COLOR=#000000></basefont>", 
            	
            (bool)true, (bool)true);
            this.AddImage(59, 27, 10400);
            this.AddImage(60, 200, 10401);
            this.AddImage(60, 360, 10402);
            //this.AddBackground(374, 417, 106, 51, 9270);
            //this.AddButton(389, 435, 12006, 12008, 0, GumpButtonType.Reply, 0);
            //this.AddImage(467, 371, 30082);
            //this.AddImage(421, 189, 991);

		}

        public static void Initialize()
        {

            EventSink.Login += new LoginEventHandler(EventSink_Login);
        }
        private static void EventSink_Login(LoginEventArgs args)
        {
            Mobile m = args.Mobile;
            if ( m is PlayerMobile )
            {
            	m.CloseGump(typeof(motd));
            	m.SendGump(new motd());
        	}
        }

	}
}