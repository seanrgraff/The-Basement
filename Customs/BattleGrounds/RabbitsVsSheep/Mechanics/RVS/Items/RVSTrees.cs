using System;

namespace Server.RabbitsVsSheep {

	public class RVSTree1 : Item {
		
		[Constructable]
		public RVSTree1() : base(0xCE0)
		{
			Name = "Walnut Tree";
			Weight = 100;
			Movable = false;
		}

		public RVSTree1(Serial serial) : base(serial)
		{
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);

			writer.Write((int) 0);
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);

			int version = reader.ReadInt();
		}
		
	}
	
	public class RVSTree2 : Item {
		
		[Constructable]
		public RVSTree2() : base(0xCD8)
		{
			Name = "Cedar Tree";
			Weight = 100;
			Movable = false;
		}

		public RVSTree2(Serial serial) : base(serial)
		{
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);

			writer.Write((int) 0);
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);

			int version = reader.ReadInt();
		}
		
	}
	
	public class RVSTree3 : Item {
		
		[Constructable]
		public RVSTree3() : base(0xCD3)
		{
			Name = "Tree";
			Weight = 100;
			Movable = false;
		}

		public RVSTree3(Serial serial) : base(serial)
		{
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);

			writer.Write((int) 0);
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);

			int version = reader.ReadInt();
		}
		
	}
	
	public class RVSTree4 : Item {
		
		[Constructable]
		public RVSTree4() : base(0xCCD)
		{
			Name = "Tree";
			Weight = 100;
			Movable = false;
		}

		public RVSTree4(Serial serial) : base(serial)
		{
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);

			writer.Write((int) 0);
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);

			int version = reader.ReadInt();
		}
		
	}

}