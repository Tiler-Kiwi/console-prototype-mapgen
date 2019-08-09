/*
 * Created by SharpDevelop.
 * User: adam.moseman
 * Date: 5/15/2017
 * Time: 1:42 AM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;

namespace TilerIsADummy
{
	/// <summary>
	/// Defines what properties tiles can posess, and the color they have. 
	/// </summary>
	/// 
	[Flags]
	public enum TileFlags
	{
		UNASSIGNED = 0,
		Passable = 2,
		SeeThru = 4
	}
	
	public enum TileColor 
	{
		UNASSIGNED = 0,
		Green = 1,
		Blue = 2,
		Red = 3,
		Gray = 4,
		Purple = 5,
		DarkGreen = 6,
		Orange = 7,
		Brown = 8,
		Black = 9
	}
	
	public enum TileRole
	{
		UNASSIGNED = 0,
		Default = 1, //main type, default
		Water = 2, //rivers
		Floor = 3, //building interior
		Wall = 4, //building walls
		Void = 5 //Special void thing i may or may not actually need!!!
	}

	public class TileType : ITileType
	{
		public string Name { get; set; }
		public TileFlags Flags { get; set; }
		public TileColor Color { get; set; }
		public TileRole Role { get; set; }

		public TileType(string name, bool passable, bool seethru, TileColor color, TileRole role)
		{
			Name = name;
			if (passable) {
				Flags = Flags | TileFlags.Passable;
			}
			if (seethru) {
				Flags = Flags | TileFlags.SeeThru;
			}
			
			Color = color;
			Role = role;
		}
	}
}

/*	
	public class Tile {
	private TileFlags _tileprop;
	public TileFlags tileprop
	{
		get{return _tileprop;}
		set{return 0;}
	}
	
	private TileTerrain _terrain;
	public TileTerrain terrain
	{
		get{return _terrain;}
		
		set{;}
	}	
		public Tile(TileTerrain terraintype)
		{
			
		}
		
	}
}
*/