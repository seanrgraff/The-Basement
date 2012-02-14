
/* Created by Ashlar, beloved of Morrigan
 * 
 * Concept borrowed and greatly changed from 
 * a err... something i saw about 2 years ago...
 * unknown author, unknown script. ( I know i am going to slap myself when i remember!  Sorry?!?! )
*/

                                                                                    //Comments are over here!!

using System;
using Server.Items;
using Server.ContextMenus;
using System.Collections.Generic;

namespace Server.RabbitsVsSheep                                                             //I name the namespace Ashlar not out of vanity, but because in case of a crash, the crash log is much clearer (when not in debug mode) on where my script is coming into error... i.e. there are alot of items, timers and gumps, not many made by me.
{
	public class SpinalTapBandana : BaseHat
    {                                                                               //This code will work on ANY item with minor adjustments... Color changing walls anyone?  (I am also making a version for mobiles)
        #region Declare Variables, Getters and Setters
        private InternalTimer m_Timer;                                              //I want to be able to assign a variable for the timer's delay so i declare the timer here and use it in DoHueTimer
        public bool m_AllowsFastSpeed;                                              //A bool controling if the player has permission to mke the color changes happen really quickly.
        public double m_FastestOkSpeed;                                             //If AllowsFastSpeed is false then this double is the fastest value allowable for HueDelay
        public double m_HueDelay;                                                   //HueDelay lets us change how fast the colors change...several times a second to once a minute.. err hour... err day?  Heck no real limit here. 
        public int m_minHue;                                                        //Lower limit on hue number allowed. Used to "bracket" a hue range.
		public int m_maxHue;                                                        //Upper limit on hue number allowed. Used to "bracket" a hue range.
        public bool m_active;                                                       //This allows checks to see if the timer is active
        public bool m_keepsGoing;                                                   //If true, when the hue of the item reaches the maxHue it "keeps going" from the minhue. If false, it stops the timer.
                                                                                            
        /*
         * This CommandProperty section allows the values just declared to be changed in-game.
         * Note that m_Timer does not appear here because there is nothing to change the timer TO in this code.
        */
        [CommandProperty( AccessLevel.GameMaster )]
        public bool AllowsFastSpeed { get { return m_AllowsFastSpeed; } set { m_AllowsFastSpeed = value; } }

        [CommandProperty( AccessLevel.GameMaster )]
        public double FastestOkSpeed { get { return m_FastestOkSpeed; } set { m_FastestOkSpeed = value; } }

        [CommandProperty( AccessLevel.Player )]                                     //The player could change these.. if they had the [props command.  We will allow it to change in the CrazyHueGump
        public double HueDelay { get { return m_HueDelay; } set { m_HueDelay = value; } }//m_HueDelay will appear in-game on [props as HueDelay. get is the current value on the item. set changes that value when [props is changed

        [CommandProperty( AccessLevel.Player )]
        public int MinHue { get { return m_minHue; } set { m_minHue = value; } }

		[CommandProperty( AccessLevel.Player )]
		public int MaxHue{ get{ return m_maxHue; } set{ m_maxHue = value; } }

        [CommandProperty( AccessLevel.Player )]
        public bool Active { get { return m_active; } set { m_active = value; } }

        [CommandProperty( AccessLevel.Player )]
        public bool KeepsGoing { get { return m_keepsGoing; } set { m_keepsGoing = value; } }
        #endregion

        [Constructable]
		public SpinalTapBandana() : base( 0x1540, 0 )
		{
			Name = "Spinal Tap Bandana";                                                    //Initial value for Name when the item is created
			Hue = 0;//1099, 1181 :)                                                 //Default hue number off-white (death robe color)
			Layer = Layer.OuterTorso;                                               //Robes are typicly worn on the layer OuterTorso

            m_AllowsFastSpeed = true;                                              //Change this to true if you want to make the color change really fast
            m_FastestOkSpeed = 0.3;                                                 //Due to the way the priority is set on the timer, a FastestOkSpeed of 2.0 = one color change per second
            m_HueDelay = 0.3;                                                       //Initial value for m_HueDelay when the item is created
            m_minHue = 1367;                                                           //Initial value for m_minHue when the item is created
            m_maxHue = 1372;  
            DoHueTimer("Increasing");                                                      //Initial value for m_maxHue when the item is created
            m_active = true;                                                       //Initial value for m_active when the item is created
            m_keepsGoing = true;                                                   //Initial value for m_keepsGoing when the item is created
        	
        }
        
        public override bool OnEquip( Mobile m ) 
		{ 
			string modName = this.Serial.ToString();
			m.AddStatMod( new StatMod( StatType.Int, modName + "Int", (int)(.5 * m.ManaMax ), TimeSpan.Zero ) );
			m.SendMessage( "You feel smarter now!" );
			return base.OnEquip( m );
		}
		
		public override void OnRemoved( object parent ) 
		{ 
			if ( parent is Mobile ) 
			{ 
				Mobile m = (Mobile)parent;
				string modName = this.Serial.ToString();
				m.RemoveStatMod( modName + "Int" );
				m.SendMessage( "Woah you feel significantly less smart..." );
			} 
		} 

		public SpinalTapBandana( Serial serial ) : base( serial )
		{                                                                           //Everything has to have a different serial number... a rose by any other name..is still the same rose!
        }

		public override void Serialize( GenericWriter writer )
		{                                                                           //Info that is being saved must be written and read in the exact same order it is declared in the item!
			base.Serialize( writer );                                               //This item inherits from the Item class.  There are other properties than what we are editing in this script that need to be saved in the writer.  This line handles writing all the inherited values.
            writer.Write( ( int )0 ); // version                                    //if you bump he version number up by one when you change serialization and code case support for your addition, existing items will not be deleted.  See heavy modified distro scripts for examples. (I havent done anything with it yet myself..*blush*)

            /*
             * a double is a decimal value like 12345.0 or 0.00001
             * an int is a whole number like 1 or 15678
             * a bool is a true or false
            */
            writer.Write( ( bool )m_AllowsFastSpeed );                              //The values we made editable in-game need some way to save the changed values.  This line saves the bool for allowing fast color changes
            writer.Write( ( double )m_FastestOkSpeed );                             //Saves the FastestOkSpeed
            writer.Write( ( double )m_HueDelay );                                   //Save the speed of the delay
            writer.Write( ( int )m_minHue );                                        //Save the minHue
            writer.Write( ( int )m_maxHue );                                        //Save the maxHue
            writer.Write( ( bool )m_active );                                       //Save if the item is running currently
            writer.Write( ( bool )m_keepsGoing );                                   //Save if the item was supposed to keep going after reaching the maxhue number or if it was supposed to reset to minHue and shut off.
		}

		public override void Deserialize( GenericReader reader )
		{                                                                           //We need to read all the things we just saved since the item just loaded here..
			base.Deserialize( reader );                                             //This item inherits the Item class.  This line handles reading stored info from the Item class, allowing us to keep this code cleaner :)
            int version = reader.ReadInt();                                         //Read that version number assigned in the writer to know if existing items are to be converted to contain new values.

            m_AllowsFastSpeed = reader.ReadBool();                                  //Was the item flagged to allow fast color change speed?
            m_FastestOkSpeed = reader.ReadDouble();                                 //Delays are doubles so we can use decimals to have delays with milliseconds like 0.25
            m_HueDelay = reader.ReadDouble();                                       //The delay is a double so we can use decimals to have delays like 0.00000001 seconds
            m_minHue = reader.ReadInt();                                            //A Hue can be dec or hex. ReadInt is supported nicely, so we use the dec # :)
            m_maxHue = reader.ReadInt();                                            //A Hue can be dec or hex. ReadInt is supported nicely, so we use the dec # :)
            m_active = reader.ReadBool();                                           //Was the item active when it was saved?
            m_keepsGoing = reader.ReadBool();                                       //Was the item flagged to keep going after reaching maxHue or should it reset to minHue and then stop?
            if ( m_active )                                                         //Hey!  The item just loaded... was it active when it was saved?
            {
                DoHueTimer("increasing");                                                       //Guess so!  Start the timer!
            }
		}

		public override void OnDoubleClick( Mobile from )
		{                                                                           //There are many many ways the timer can be started... Doubleclick is an expected method on many items.
			if( this.RootParent == from )                                           //If the item is currently being worn by the player that doubleclicked it ( keeps other people from turning your clothes on or off :)
            {
				BeginLaunch( from );
				BeginLaunch( from );
				BeginLaunch( from );
			}
		}

		public void BeginLaunch( Mobile from )
		{
			Map map = from.Map;

			if ( map == null || map == Map.Internal )
				return;

			//from.SendLocalizedMessage( 502615 ); // You launch a firework!

			Point3D ourLoc = GetWorldLocation();

			Point3D startLoc = new Point3D( ourLoc.X, ourLoc.Y, ourLoc.Z + 10 );
			Point3D endLoc = new Point3D( startLoc.X + Utility.RandomMinMax( -2, 2 ), startLoc.Y + Utility.RandomMinMax( -2, 2 ), startLoc.Z + 32 );

			Effects.SendMovingEffect( new Entity( Serial.Zero, startLoc, map ), new Entity( Serial.Zero, endLoc, map ),
				0x36E4, 5, 0, false, false );

			Timer.DelayCall( TimeSpan.FromSeconds( 1.0 ), new TimerStateCallback( FinishLaunch ), new object[]{ from, endLoc, map } );
		}

		private void FinishLaunch( object state )
		{
			object[] states = (object[])state;

			Mobile from = (Mobile)states[0];
			Point3D endLoc = (Point3D)states[1];
			Map map = (Map)states[2];

			int hue = Utility.Random( 40 );

			if ( hue < 8 )
				hue = 1367;
			else if ( hue < 10 )
				hue = 1368;
			else if ( hue < 12 )
				hue = 1369;
			else if ( hue < 16 )
				hue = 1370;
			else if ( hue < 20 )
				hue = 1371;
			else
				hue = 1372;

			if ( Utility.RandomBool() )
				hue = Utility.RandomList( 1367, 1368, 1369, 1370, 1371 );

			int renderMode = Utility.RandomList( 0, 2, 3, 4, 5, 7 );

			Effects.PlaySound( endLoc, map, Utility.Random( 0x11B, 4 ) );
			Effects.SendLocationEffect( endLoc, map, 0x373A + (0x10 * Utility.Random( 4 )), 16, 10, hue, renderMode );
		}

        public void DoHueTimer( String order )
        {                                                                           //All the code for starting the timer is here, allowing us to assign variables to the timer since the timer needs the timespan info in its call...at the same time the item is linked to it...( lockpick method :)
            if ( this.m_HueDelay >= this.m_FastestOkSpeed || this.AllowsFastSpeed ) //If the assigned HueDelay is greater than the FastestOkSpeed OR the item AllowsFastSpeed color changes.
            {
                TimeSpan next = TimeSpan.FromSeconds( this.m_HueDelay );            //This assigns the HueDelay from earlier to be the TimeSpan variable called next.
                m_Timer = new InternalTimer( this, next, order );                          //Creates an instance of the internal timer we declared up in the item and passes it the variables Item this and the TimeSpan next as the delay.
                m_Timer.Start();                                                    //Starts the timer that was just instanced
            }
            else
            {                                                                       //The item does not allow fast color changes and the HueDelay is currently not acceptable.
                TimeSpan next = TimeSpan.FromSeconds( this.m_FastestOkSpeed );      //This assigns the FastestOkSpeed from earlier to be the TimeSpan variable called next.
                m_Timer = new InternalTimer( this, next, order );                          //Creates an instance of the internal timer we declared up in the item and passes it the variables Item this and the TimeSpan next as the delay.
                m_Timer.Start();                                                    //Starts the timer that was just instanced
            }
        }

        #region InternalTimer
        private class InternalTimer : Timer
		{
            private SpinalTapBandana m_item;                                            //Declare the variable m_item to be of type CrazyHueRobe
            
            private static TimeSpan TwoMinutes = TimeSpan.FromMinutes( 2.0 );       //Declare what TwoMinutes is.  These declarations are nesessary because TimeSpans are usually From(some measurement) and not an actual 'hard' number
            private static TimeSpan ThirtySeconds = TimeSpan.FromSeconds( 30.0 );   //Declare what ThirtySeconds is.
            private static TimeSpan TenSeconds = TimeSpan.FromSeconds( 10.0 );      //Declare what TenSecons is.
            private static TimeSpan OneSecond = TimeSpan.FromSeconds( 1.0 );        //Declare what OneSecond is.
	
			private String order;
	
            public InternalTimer( SpinalTapBandana item, TimeSpan next, String shit ): base( next )
            {
                m_item = item;     
                order = shit;                                                 //Populate the variable m_item to be the Item that sent the timer
                                                                                    //This section of assigning timer priority came from the RunUO Distro script: 
                if ( next >= TwoMinutes )                                           //If the TimeSpan next is greater than or equal to TwoMinutes
                    Priority = TimerPriority.OneMinute;                             //This timer's priority is OneMinute
                else if ( next >= ThirtySeconds )                                   //the rest of this section reads in the same way, just different values
                    Priority = TimerPriority.FiveSeconds;
                else if ( next >= TenSeconds )
                    Priority = TimerPriority.OneSecond;
                else if ( next >= OneSecond )
                    Priority = TimerPriority.TwoFiftyMS;
                else
                    Priority = TimerPriority.TwentyFiveMS;                          //This is called if next is less than OneSecond
			}
			protected override void OnTick()
			{    
				if ( order == "increasing" )    
				{                                                               //When next is over... Do the following
	                if ( m_item.m_active )                                              //If the item says the timer is active
	                {
	                    if ( m_item.Hue < m_item.m_minHue )                             //If ( the item's hue is less than the minHue )  Possible if the items properties are change either from the CrazyHueGump or [props
	                    {
	                        m_item.Hue = m_item.m_minHue;                               //Change the current hue to be the minHue
	                        m_item.DoHueTimer("increasing");                                        //Start the next timer
	                    }
	                    else if ( m_item.Hue < m_item.m_maxHue )                        //If ( the item's hue is less than or equal to the maxHue )
	                    {
	                        m_item.Hue++;                                               //Increase the Hue of the item by 1
	                        m_item.DoHueTimer("increasing");
	                    }
	                    else
	                    {                                                               //This is called when the item's hue is greater than the minHue and greater than the maxHue both
	                        m_item.Hue--;                               //Change the items current hue to be the minHue value
	                        if ( m_item.m_keepsGoing )                                  //If the item is flagged to keep going instead of stopping when it reaches maxHue and resets the hue to minHue
	                        {
	                            m_item.DoHueTimer("decreasing");                                    //Then keep going!  (start the next timer)
	                        }
	                    }
	                }
	            }
	            else
	            {
	            	if ( m_item.m_active )                                              //If the item says the timer is active
	                {
	                    if ( m_item.Hue > m_item.m_minHue )                        //If ( the item's hue is less than or equal to the maxHue )
	                    {                     
	                        m_item.Hue--;  
	                        m_item.DoHueTimer("decreasing");     
	                    }
	                    else
	                    {                                                              //This is called when the item's hue is greater than the minHue and greater than the maxHue both
	                        m_item.Hue++;                               //Change the items current hue to be the minHue value
	                        if ( m_item.m_keepsGoing )                                  //If the item is flagged to keep going instead of stopping when it reaches maxHue and resets the hue to minHue
	                        {
	                            m_item.DoHueTimer("increasing");                                    //Then keep going!  (start the next timer)
	                        }
	                    }
	                }
	            }
			}
        }
        #endregion
    }
}
