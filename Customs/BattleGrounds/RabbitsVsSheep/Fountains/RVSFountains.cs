using System;
using System.Collections;
using System.Collections.Generic;
using Server;
using Server.Items;
using Server.Gumps;
using Server.Mobiles;
using Server.ContextMenus;
using Server.Network;
using Server.Commands;

namespace Server.RabbitsVsSheep
{
	public class ManaFlies : Item
	{
		protected bool abled;
        [CommandProperty(AccessLevel.GameMaster)]
        public bool Enabled
        {
            get { return abled; }
            set { abled = value; }
        }
		
		public String Name;
		
		public RVSController rvsc = null;
	
		public Map regionmap = Map.Felucca;
	    [CommandProperty(AccessLevel.GameMaster)]
        public Map ThisRegionMap
        {
            get { return regionmap; }
            set { regionmap = value; }
        }
        
        public Rectangle2D regionpoint;
        [CommandProperty(AccessLevel.GameMaster)]
        public Rectangle2D ThisRegionPoint
        {
            get { return regionpoint; }
            set { regionpoint = value; }
        }
        
        public static ManaRegenRegion ManaRegion;

		[Constructable]
		public ManaFlies() : base( 0x91B )
		{
			Movable = false;
			Hue = 1265;
		}
		
		public override void OnLocationChange( Point3D oldLocation )
		{
			if ( !Enabled )
				return; 
				
			regionpoint = new Rectangle2D( X - 5, Y - 5, 11, 11 );
			StartManaRegen();
		}
		
		public void StartManaRegen()
        {
        
			if ( ManaRegion != null)
            	ManaRegion.Unregister();
			
         	ManaRegion = new ManaRegenRegion(this, rvsc, Name);
        }
        
        public void StopManaRegen()
        {
        
			if ( ManaRegion != null)
            	ManaRegion.Unregister();

          	ManaRegion = null;
        }
        
        bool started = true;
        
       	public override void OnDoubleClick( Mobile from )
		{
		    if ( !Enabled )
				return; 
		
			if ( from.AccessLevel == AccessLevel.Owner && started == false )
			{
					from.SendMessage("Region Enabled");
					started = true;
					StartManaRegen();
			}
			else if ( from.AccessLevel == AccessLevel.Owner && started == true )
			{
					from.SendMessage("Region Disabled");
					started = false;
					StopManaRegen();
			}
		}

		public ManaFlies( Serial serial ) : base( serial )
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
		
		public override void Delete()
		{
			if ( ManaRegion != null)
            	ManaRegion.Unregister();
			
			base.Delete();
		}

	}
	
	public class ManaFlies2 : Item
	{
		protected bool abled;
        [CommandProperty(AccessLevel.GameMaster)]
        public bool Enabled
        {
            get { return abled; }
            set { abled = value; }
        }
		
		public String Name;
		
		public RVSController rvsc = null;
	
		public Map regionmap = Map.Felucca;
	    [CommandProperty(AccessLevel.GameMaster)]
        public Map ThisRegionMap
        {
            get { return regionmap; }
            set { regionmap = value; }
        }
        
        public Rectangle2D regionpoint;
        [CommandProperty(AccessLevel.GameMaster)]
        public Rectangle2D ThisRegionPoint
        {
            get { return regionpoint; }
            set { regionpoint = value; }
        }
        
        public static ManaRegenRegion2 ManaRegion;

		[Constructable]
		public ManaFlies2() : base( 0x91B )
		{
			Movable = false;
			Hue = 1265;
		}
		
		public override void OnLocationChange( Point3D oldLocation )
		{
			if ( !Enabled )
				return; 
				
			regionpoint = new Rectangle2D( X - 5, Y - 5, 11, 11 );
			StartManaRegen();
		}
		
		public void StartManaRegen()
        {
        
			if ( ManaRegion != null)
            	ManaRegion.Unregister();
			
         	ManaRegion = new ManaRegenRegion2(this, rvsc, Name);
        }
        
        public void StopManaRegen()
        {
        
			if ( ManaRegion != null)
            	ManaRegion.Unregister();

          	ManaRegion = null;
        }
        
        bool started = true;
        
       	public override void OnDoubleClick( Mobile from )
		{
		    if ( !Enabled )
				return; 
		
			if ( from.AccessLevel == AccessLevel.Owner && started == false )
			{
					from.SendMessage("Region Enabled");
					started = true;
					StartManaRegen();
			}
			else if ( from.AccessLevel == AccessLevel.Owner && started == true )
			{
					from.SendMessage("Region Disabled");
					started = false;
					StopManaRegen();
			}
		}

		public ManaFlies2( Serial serial ) : base( serial )
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
		
		public override void Delete()
		{
			if ( ManaRegion != null)
            	ManaRegion.Unregister();
			
			base.Delete();
		}

	}
	
	public class HealthFlies : Item
	{
		protected bool abled;
        [CommandProperty(AccessLevel.GameMaster)]
        public bool Enabled
        {
            get { return abled; }
            set { abled = value; }
        }
		
		public String Name;
		
		public RVSController rvsc = null;
		
		public Map regionmap = Map.Felucca;
	    [CommandProperty(AccessLevel.GameMaster)]
        public Map ThisRegionMap
        {
            get { return regionmap; }
            set { regionmap = value; }
        }
        
        public Rectangle2D regionpoint;
        [CommandProperty(AccessLevel.GameMaster)]
        public Rectangle2D ThisRegionPoint
        {
            get { return regionpoint; }
            set { regionpoint = value; }
        }
        
        public static HealthRegenRegion HealthRegion;

		[Constructable]
		public HealthFlies() : base( 0x91B )
		{
			Movable = false;
			Hue = 1360;
		}
		
		public override void OnLocationChange( Point3D oldLocation )
		{
			if ( !Enabled )
				return;
		
			regionpoint = new Rectangle2D( X - 5, Y - 5, 11, 11 );
			StartHealthRegen();
		}
		
		public void StartHealthRegen()
        {
			if ( HealthRegion != null)
            	HealthRegion.Unregister();

          	HealthRegion = new HealthRegenRegion(this, rvsc, Name);
        }
        
        public void StopHealthRegen()
        {
			if ( HealthRegion != null)
            	HealthRegion.Unregister();

          	HealthRegion = null;
        }
        
        bool started = true;
        
       	public override void OnDoubleClick( Mobile from )
		{
			if ( !Enabled )
				return;
		
			if ( from.AccessLevel == AccessLevel.Owner && started == false )
			{
					from.SendMessage("Region Enabled");
					started = true;
					StartHealthRegen();
			}
			else if ( from.AccessLevel == AccessLevel.Owner && started == true )
			{
					from.SendMessage("Region Disabled");
					started = false;
					StopHealthRegen();
			}
		}

		public HealthFlies( Serial serial ) : base( serial )
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
		
		public override void Delete()
		{
			if ( HealthRegion != null)
            	HealthRegion.Unregister();
			
			base.Delete();
		}
	}
	
	
	public class HealthFlies2 : Item
	{
		protected bool abled;
        [CommandProperty(AccessLevel.GameMaster)]
        public bool Enabled
        {
            get { return abled; }
            set { abled = value; }
        }
		
		public String Name;
		
		public RVSController rvsc = null;
		
		public Map regionmap = Map.Felucca;
	    [CommandProperty(AccessLevel.GameMaster)]
        public Map ThisRegionMap
        {
            get { return regionmap; }
            set { regionmap = value; }
        }
        
        public Rectangle2D regionpoint;
        [CommandProperty(AccessLevel.GameMaster)]
        public Rectangle2D ThisRegionPoint
        {
            get { return regionpoint; }
            set { regionpoint = value; }
        }
        
        public static HealthRegenRegion2 HealthRegion;

		[Constructable]
		public HealthFlies2() : base( 0x91B )
		{
			Movable = false;
			Hue = 1360;
		}
		
		public override void OnLocationChange( Point3D oldLocation )
		{
			if ( !Enabled )
				return;
		
			regionpoint = new Rectangle2D( X - 5, Y - 5, 11, 11 );
			StartHealthRegen();
		}
		
		public void StartHealthRegen()
        {
			if ( HealthRegion != null)
            	HealthRegion.Unregister();

          	HealthRegion = new HealthRegenRegion2(this, rvsc, Name);
        }
        
        public void StopHealthRegen()
        {
			if ( HealthRegion != null)
            	HealthRegion.Unregister();

          	HealthRegion = null;
        }
        
        bool started = true;
        
       	public override void OnDoubleClick( Mobile from )
		{
			if ( !Enabled )
				return;
		
			if ( from.AccessLevel == AccessLevel.Owner && started == false )
			{
					from.SendMessage("Region Enabled");
					started = true;
					StartHealthRegen();
			}
			else if ( from.AccessLevel == AccessLevel.Owner && started == true )
			{
					from.SendMessage("Region Disabled");
					started = false;
					StopHealthRegen();
			}
		}

		public HealthFlies2( Serial serial ) : base( serial )
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
		
		public override void Delete()
		{
			if ( HealthRegion != null)
            	HealthRegion.Unregister();
			
			base.Delete();
		}
	}
	
	
}