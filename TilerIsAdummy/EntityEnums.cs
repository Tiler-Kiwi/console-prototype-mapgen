/*
 * Created by SharpDevelop.
 * User: adam.moseman
 * Date: 5/22/2017
 * Time: 3:46 AM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;

namespace TilerIsADummy
{
	/// <summary>
	/// Description of EntityEnums.
	/// </summary>
	public enum EntityFaction
	{
		UNASSIGNED = 0,
		Neutral = 1,
		Player = 2,
		BadGuy = 3,
		FactionA = 4,
		FactionB = 5,
		FactionC = 6,
		FactionD = 7
	}
	
	public enum EntityItemSlot
	{
		UNASSIGNED = 0,
		Head = 1,
		Torso = 2,
		Arm1 = 3, //one arm
		Arm2 = 4, //both arms
		Hand1 = 5, //one hand
		Hand2 = 6, //both hands
		Belt = 7, //waist area
		Leg1 = 8, //etc
		Leg2 = 9,
		Foot1 = 10,
		Foot2 = 11,
		Face = 12, //masks
		Mount = 13 //for dudes that ride things
	}
	
	public enum EntityItemType
	{
		UNASSIGNED = 0,
		Weapon = 1, //used to hit people
		Wear = 2, //worn rather than used to hit people
		Consumable = 3, //cant be equipped, can be used for something
		Key = 4 //cant be consumed or equipped
	}
	
	[Flags]
	public enum EntityItemProperties
	{
		UNASSIGNED = 0,
		Hidden = 2, //cannot be seen on map or in inventory
		Cursed = 4, //cant be removed
		Equipped = 8, //actor is wearing/using this
		Remember = 16, //dont ever delete from memory
		Bound = 32, //cannot be dropped
		NoPickup = 64, //cannot be picked up
		Solo = 128, //cannot stack
		NoEquip = 256, //cannot equip
	}
	
	public enum EntityItemWeaponClass
	{
		UNASSIGNED = 0,
		Sword = 1,
		Pole = 2,
		Blunt = 3,
		Launcher = 4, //Bows are 1 handed, but need ammo, which is placed automatically in other hand
		Missle = 5 //thrown/fired things. Can carry multiple.
	}
	
	public enum EntityCharacterMovementType
	{
		UNASSIGNED = 0,
		Foot = 1,
		Hoof = 2,
		Fly = 3
	}
	
	[Flags]
	public enum EntityCharacterFreeSlot
	{
		UNASSIGNED = 0,
		Head = 2,
		Torso = 4,
		ArmL = 8,
		ArmR = 16, 
		HandL = 32, 
		HandR = 64, 
		Belt = 128, 
		LegL = 256, 
		LegR = 512,
		FootL = 1024,
		FootR = 2048,
		Face = 4096, 
		Mount = 8192
	}
	
	public enum EntityCharacterTurnStatus
	{
		UNASSIGNED = 0,
		MyTurn = 1,
		Moved = 2,
		NotMyTurn = 3 
	}
	
	public enum EntityCharacterState
	{
		UNASSIGNED = 0,
		Active = 1, //takes turns from controlling faction
		Passive = 2, //does not take turns
		Dead = 3, //does not take turns, cannot be targeted in combat, rendered as a dead thing.
		Disabled = 4 //Invisable, not selectable, but data is present in battle map.
	}
}