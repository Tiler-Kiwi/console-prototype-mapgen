/*
 * Created by SharpDevelop.
 * User: adam.moseman
 * Date: 5/22/2017
 * Time: 2:25 AM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Collections.Generic;

namespace TilerIsADummy
{
	/// <summary>
	/// Description of IActor.
	/// </summary>
	public interface IEntityActor : IEntity
	{	
		//int Health {get; set;}
		//List<IEntityItem> Equipment {get; set;}
		//EntityCharacterMovementType MovementType {get; set;}
		//EntityCharacterFreeSlot IsFreeSlot {get; set;}
		
		int MovementPoints {get; set;}
		EntityCharacterTurnStatus TurnStatus {get; set;}
		EntityCharacterState ActiveState {get; set;}
		
	}
}
