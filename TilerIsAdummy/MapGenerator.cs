/*
 * Created by SharpDevelop.
 * User: adam.moseman
 * Date: 5/18/2017
 * Time: 12:30 AM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Collections.Generic;

namespace TilerIsADummy
{
	/// <summary>
	/// Description of MapGenerator.
	/// </summary>
	public class MapGenerator
	{
		private ITileType _TileType;
		public List<ITileType> tileTypeList = new List<ITileType>();
		
		public MapGenerator(ITileType tiletype)
		{
			this._TileType = tiletype;
			GenerateTileTypeList();
			
		}
		
		private void GenerateTileTypeList()
		{
		}
	}
}
