using System;
using System.Collections.Generic;
using System.Reflection;
using Server;
using Server.Gumps; 
using Server.Network;
using Solaris.BoardGames;


namespace Server.Items
{
	//a test of the boardgame system
	public class GenericControlItem : BoardGameControlItem
	{
		public override string GameName{ get{ return BoardGameName; } }
		
		public String BoardGameName = "GenericName";
		
		public override string GameDescription{ get{ return "Generic Game."; } }
		public override string GameRules
		{ 
			get
			{ 
				return "There are no rules.";
			}
		}
		
		public GenericControlItem( Serial serial ) : base( serial )
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