using System;
using Server.Network;
using Server.Prompts;
using Server.Items;
using System.Collections;
using Server.Gumps;
using Server.Targeting;
using Server.Misc;
using Server.Accounting;
using System.Xml;
using Server.Mobiles; 

namespace Server.Items
{
	public class PetBondDeed : Item
	{
		[Constructable]
		public PetBondDeed() : base( 0x14F0 )
		{
			base.Weight = 0;
			base.Name = "A Pet Bond Deed";
		}		

		public override void OnDoubleClick( Mobile from )
		{
			if ( IsChildOf( from.Backpack ) )
			{
				from.Target = new InternalTarget(from, this);
			}
			else
			{
				from.SendMessage("The Bonding Deed must be in your backpack.");
			}
		}
		
		public PetBondDeed( Serial serial ) : base( serial )
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
	
		public class InternalTarget : Target
		{
			private Mobile m_From;
			private PetBondDeed m_Deed;
			
			public InternalTarget( Mobile from, PetBondDeed deed ) :  base ( 3, false, TargetFlags.None )
			{
				m_Deed = deed;
				m_From = from;
				from.SendMessage("Select a pet to bond to you.");
		
				
			}
			
			protected override void OnTarget( Mobile from, object targeted )
			{
				
				if (m_Deed.IsChildOf( m_From.Backpack ) )
				{					
					if ( targeted is Mobile )
					{
						if ( targeted is BaseCreature )
						{
							BaseCreature creature = (BaseCreature)targeted;
							if( !creature.Tamable ){
								from.SendMessage("That creature is not tameable.");
							}
							else if(  !creature.Controlled || creature.ControlMaster != from ){
								from.SendMessage("That creature is not controlled by you.");
							}
							else if( creature.IsDeadPet ){
								from.SendMessage("That pet is dead...");
							}
							else if ( creature.Summoned ){
								from.SendMessage("Cannot bond summoned creatures.");
							}
							else if ( creature.Body.IsHuman ){
								from.SendMessage("Cannot bond humanoids.");
							}
							else{	
								
								if( creature.IsBonded == true ){
									from.SendMessage("Your animal is already bonded.");
								}
								else{
									
									if( from.Skills[SkillName.AnimalTaming].Base  < creature.MinTameSkill ){
										from.SendMessage("You need to increase your taming skill.");
									}
									else if( from.Skills[SkillName.AnimalLore].Base  < creature.MinTameSkill ){
											from.SendMessage("You need to increase your animal lore skill.");
										}
									else{
										try{
											creature.IsBonded = true;
											from.SendMessage("{0} has been bonded to you.",creature.Name);
											m_Deed.Delete();
										}
										catch{
											from.SendMessage("There seems to be a problem with this animal. Contact Staff");
										}
											
									}
								}
							}							
						}
						else{
							from.SendMessage("Target must be an creature.");
						}
					}
					else{
							from.SendMessage("Target must not be an item.");
						}
				}
				else{
					from.SendMessage("Bonding Deed must be in your backpack.");
				}			
		}
	}
}
