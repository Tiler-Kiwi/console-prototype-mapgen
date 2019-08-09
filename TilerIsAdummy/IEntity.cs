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
	/// Something that is tracked on the map by the tiles,
	/// but is not generated with the map.
	/// 
	/// Represents characters, and items.
	/// 
	/// Characters sometimes will have to be items (corpses)
	/// 
	/// Entities have a set of unique characteristics, but they get behavior from components.
	/// </summary>
	public interface IEntity : TilerIsADummy.PrototypeMapGen.IP_Update, TilerIsADummy.PrototypeMapGen.IP_Renderable
	{
		string Name{get; set;}
		EntityFaction Faction{get; set;}
		
		int MyTileIndex {get; set;}
		PrototypeMapGen.P_TileArray MyMap {get; set;}
		Tile MyLocation{get; set;}
		
		//string Description{get; set;}
	}
}
