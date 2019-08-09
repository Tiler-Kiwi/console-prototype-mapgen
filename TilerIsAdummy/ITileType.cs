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
	public interface ITileType
	{
		string Name { get; set; }
		TileFlags Flags { get; set; }
		TileColor Color {get; set; }
		TileRole Role {get; set;}
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
