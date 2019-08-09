/*
 * Created by SharpDevelop.
 * User: adam.moseman
 * Date: 6/3/2017
 * Time: 1:23 AM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Collections.Generic;

namespace TilerIsADummy.PrototypeMapGen
{
	/// <summary>
	/// A static class, containing a list of tiletypes as a field.
	/// Modifying the list must be done via a reference, not via the field.
	/// </summary>
	public static class DefaultTileTypeList
	{
		private static bool listmade = false;
		private static List<TileType> _DefaultList = new List<TileType>();
		
		public static List<TileType> DefaultList
		{
			get{
				if(!listmade)
				{
					_DefaultList.Add(new TileType("Grass", true, true, TileColor.Green, TileRole.Default));
					_DefaultList.Add(new TileType("Water", false, true, TileColor.Blue, TileRole.Water));
					_DefaultList.Add(new TileType("Wall", false, false, TileColor.Red, TileRole.Wall));
					_DefaultList.Add(new TileType("Floor", true, true, TileColor.Gray, TileRole.Floor));
					_DefaultList.Add(new TileType("Void", false, false, TileColor.Black, TileRole.Void));
					//_DefaultList.Add(new TileType("Objective", true, true, TileColor.Purple));
					//_DefaultList.Add(new TileType("Trees", false, false, TileColor.DarkGreen));
					//_DefaultList.Add(new TileType("Rocks", false, true, TileColor.Orange));
					//_DefaultList.Add(new TileType("Bridge", true, true, TileColor.Brown));
					listmade=true;
				}
				
				return _DefaultList;
			}
			
			set
			{
				//readonly
			}
		}
	}
}
