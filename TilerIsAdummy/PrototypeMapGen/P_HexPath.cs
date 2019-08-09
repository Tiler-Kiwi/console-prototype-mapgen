/*
 * Created by SharpDevelop.
 * User: adam.moseman
 * Date: 6/18/2017
 * Time: 5:26 AM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Collections.Generic;

namespace TilerIsADummy.PrototypeMapGen
{
	/// <summary>
	/// draws line between two points and shit like that
	/// fuck you
	/// </summary>
	public static class P_HexPath
	{
		public static List<P_CubeCoords> GetPath(P_TileArray tilearray, P_CubeCoords start, P_CubeCoords end)
		{
			List<P_CubeCoords> ReturnValue = new List<P_CubeCoords>();
			
			List<HexNode> closedset = new List<HexNode>();
			List<HexNode> openset = new List<HexNode>();
			
			HexNode thing = new HexNode(start, end, null);
			thing.H = P_CubeCoords.cube_distance(start,end);
			openset.Add(thing);
			
			while(openset.Count > 0)
			{
				HexNode current = openset[0];
				for(int i = 1; i< openset.Count; i++)
				{
					if(openset[i].Score < current.Score)
					{
						current = openset[i];
					}
				}
				
				if(current.Coords.X == end.X && current.Coords.Y == end.Y && current.Coords.Z == end.Z)
				{
					return RebuildPath(current);
				}
				
				openset.Remove(current);
				closedset.Add(current);
				
				List<P_CubeCoords> neighbors = tilearray.CubeCoordNeighbors(current.Coords);
				for(int i = 0; i<neighbors.Count; i++)
				{
					if(tilearray.MyTiles[tilearray.CubeToIndex(neighbors[i])].IsTilePassable() == false)
					{
						continue;
					}
					bool found = false;
					for(int j = 0; j< closedset.Count; j++)
					{
						if(closedset[j].Coords.X == neighbors[i].X &&
						   closedset[j].Coords.Y == neighbors[i].Y &&
						   closedset[j].Coords.Z == neighbors[i].Z )
						{
							found=true;
							break;
						}
					}
					if(found){continue;}
					for(int j=0; j< openset.Count; j++)
					{
						if(openset[j].Coords.X == neighbors[i].X && 
						   openset[j].Coords.Y == neighbors[i].Y &&
						   openset[j].Coords.Z == neighbors[i].Z )
						{
							found=true;
							break;
						}
					}
					if(found){continue;}
					
					HexNode NewNode = new HexNode(neighbors[i], end, current);
					openset.Add(NewNode);
				}
			}
			return ReturnValue;
		}
		
		public static List<P_CubeCoords> RebuildPath(HexNode current)
		{
			List<P_CubeCoords> ReturnValue = new List<P_CubeCoords>();
			ReturnValue.Add(current.Coords);
			while(current.Parent != null)
			{
				current = current.Parent;
				ReturnValue.Add(current.Coords);
			}
			return ReturnValue;
		}
	}
	
	public class HexNode
	{
		private HexNode _Parent;
		private double _H;
		private P_CubeCoords _Coords;
		
		public HexNode Parent
		{
			get{return _Parent;}
			set{_Parent = value;}
		}
		
		public P_CubeCoords Coords
		{
			get{return _Coords;}
			set{_Coords = value;}
			
		}
		
		public double H
		{
			get
			{
				return _H;
			}
			set
			{
				_H = value;
			}
		}
		
		public double G
		{
			get {if(_Parent != null){return 1+_Parent.G;}return 0;}
			set { }
		}
		
		public double Score
		{ 
			get {return G+H;}
			set {	}
		}
		
		public HexNode(TilerIsADummy.PrototypeMapGen.P_CubeCoords location, TilerIsADummy.PrototypeMapGen.P_CubeCoords goal, HexNode parent = null)
		{
			_H = TilerIsADummy.PrototypeMapGen.P_CubeCoords.cube_distance(location,goal);
			_Parent = parent;
			_Coords = location;
		}
		
		
	}
}
