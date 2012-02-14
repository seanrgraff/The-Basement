using System;
using Server.Items;
using Server.Mobiles;
using System.Collections;
using System.Collections.Generic;

namespace Server.Items
{
	public class EmptyStone : Item
	{
		public override string DefaultName
		{
			get { return "Backpack Cleaning Stone"; }
		}

		[Constructable]
		public EmptyStone() : base( 0xED4 )
		{
			Movable = false;
			Hue = 2418;
		}

		public override void OnDoubleClick( Mobile from )
		{
			PlayerMobile pm = from as PlayerMobile;
			Container pack = pm.Backpack;
            if (pack == null) return;

            List<Item> items = new List<Item>();
            foreach (Item item in pack.Items)
            {
            	if (item is BasePotion || item is BaseReagent || item is Pouch || item is Bag || item is Bottle)
                {
                	items.Add(item);
                }
            }

            foreach (Item item in items)
            {
            	item.Delete();
            } 
            pm.SendMessage("Your backpack has been cleaned!");
		}

		public EmptyStone( Serial serial ) : base( serial )
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