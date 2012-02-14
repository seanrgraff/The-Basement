using System;

namespace Server.Fielding {

	public class FieldTree1 : Item {
		
		[Constructable]
		public FieldTree1() : base(0xCE0)
		{
			Name = "Walnut Tree";
			Weight = 100;
			Movable = false;
		}

		public FieldTree1(Serial serial) : base(serial)
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
	
	public class FieldTree2 : Item {
		
		[Constructable]
		public FieldTree2() : base(0xCD8)
		{
			Name = "Cedar Tree";
			Weight = 100;
			Movable = false;
		}

		public FieldTree2(Serial serial) : base(serial)
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
	
	public class FieldTree3 : Item {
		
		[Constructable]
		public FieldTree3() : base(0xCD3)
		{
			Name = "Tree";
			Weight = 100;
			Movable = false;
		}

		public FieldTree3(Serial serial) : base(serial)
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
	
	public class FieldTree4 : Item {
		
		[Constructable]
		public FieldTree4() : base(0xCCD)
		{
			Name = "Tree";
			Weight = 100;
			Movable = false;
		}

		public FieldTree4(Serial serial) : base(serial)
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