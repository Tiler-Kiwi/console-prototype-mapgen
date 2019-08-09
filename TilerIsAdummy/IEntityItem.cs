/*
 * Created by SharpDevelop.
 * User: adam.moseman
 * Date: 5/22/2017
 * Time: 2:24 AM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;

namespace TilerIsADummy
{
	/// <summary>
	/// Description of IItem.
	/// </summary>
	public interface IEntityItem : IEntity
	{
		//EntityItemSlot ItemSlot {get; set;}
		
		EntityItemProperties ItemProperties {get; set;}
		EntityItemWeaponClass ItemClass {get; set;}
	}
}
