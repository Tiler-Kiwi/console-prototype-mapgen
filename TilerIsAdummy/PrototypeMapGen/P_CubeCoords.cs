/*
 * Created by SharpDevelop.
 * User: adam.moseman
 * Date: 6/3/2017
 * Time: 1:22 AM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Collections.Generic;

namespace TilerIsADummy.PrototypeMapGen
{
	/// <summary>
	/// Contains an X,Y,and Z integer, meant to correspond to a position on a hexagon grid.
	/// </summary>
	public struct P_CubeCoords
	{
		public int X;
		public int Y;
		public int Z;
		
		public P_CubeCoords(int x, int y, int z)
		{
			X=x;
			Y=y;
			Z=z;
		}
		
		public P_CubeCoords(int x, int y)
		{
			X=x;
			Y=y;
			Z=-1*x-y;
		}
		
		public P_CubeCoords UpRight
		{
			get
			{
				return new P_CubeCoords(X,Y+1,Z-1);
			}
			set{}
		}
		
		public P_CubeCoords UpLeft
		{
			get
			{
				return new P_CubeCoords(X+1,Y,Z-1);
			}
			set{}
		}
		
		public P_CubeCoords Right
		{
			get
			{
				return new P_CubeCoords(X-1,Y+1,Z);
			}
			set{}
		}
		
		public P_CubeCoords Left
		{
			get
			{
				return new P_CubeCoords(X+1,Y-1,Z);
			}
			set{}
		}
		
		public P_CubeCoords DownRight
		{
			get
			{
				return new P_CubeCoords(X-1,Y,Z+1);
			}
			set{}
		}
		
		public P_CubeCoords DownLeft
		{
			get
			{
				return new P_CubeCoords(X, Y-1, Z+1);
			}
			set{}
		}
		
		public static P_CubeCoords RoundedCubes(double X, double Y, double Z)
		{
			double roundedX = Math.Round(X);
			double roundedY = Math.Round(Y);
			double roundedZ = Math.Round(Z);
			
			double x_diff = Math.Abs(roundedX - X);
			double y_diff = Math.Abs(roundedY - Y);
			double z_diff = Math.Abs(roundedZ - Z);
			
			if(x_diff > y_diff && x_diff > z_diff)
			{
				roundedX = -1*roundedY-roundedZ;
			}
			else if(y_diff > z_diff)
			{
				roundedY = -1*roundedX-roundedZ;
			}
			else
			{
				roundedZ = -1*roundedX-roundedY;
			}
			
			int intX = Convert.ToInt32(roundedX);
			int intY = Convert.ToInt32(roundedY);
			int intZ = Convert.ToInt32(roundedZ);
			
			return new P_CubeCoords(intX,intY,intZ);
		}
		
		public static double lerp(double a, double b, double t)
		{
			return a+(b-a)*t;
		}
		
		public static P_CubeCoords cube_lerp(P_CubeCoords a, P_CubeCoords b, double t)
		{
			return RoundedCubes(lerp(a.X, b.X, t),
			                     lerp(a.Y, b.Y, t),
			                     lerp(a.Z, b.Z, t));
		}
		
		public static double cube_distance(P_CubeCoords a, P_CubeCoords b)
		{
			return (Math.Abs(a.X-b.X) + Math.Abs(a.Y - b.Y) + Math.Abs(a.Z - b.Z)) /2;
		}
		
		public static List<P_CubeCoords> cube_linedraw(P_CubeCoords a, P_CubeCoords b)
		{
			double N = cube_distance(a,b);
			
			List<P_CubeCoords> results = new List<P_CubeCoords>();
			
			if(N==0)
			{
				return results;
			}
			
			for(int i=0; i<=N;i++)
			{
				results.Add(cube_lerp(a,b,1.0/N*i));
			}
			return results;
		}
		
		public static P_CubeCoords StepForward(P_CubeCoords point, TileNeighborEnum direction)
		{
			switch(direction)
			{
			case TileNeighborEnum.DownLeft:
				return point.DownLeft;
			case TileNeighborEnum.DownRight:
				return point.DownRight;
			case TileNeighborEnum.Right:
				return point.Right;
			case TileNeighborEnum.UpRight:
				return point.UpRight;
			case TileNeighborEnum.UpLeft:
				return point.UpLeft;
			case TileNeighborEnum.Left:
				return point.Left;
			}
			throw new Exception("What THE FUCK did you just say to me????");
		}
	}
}
