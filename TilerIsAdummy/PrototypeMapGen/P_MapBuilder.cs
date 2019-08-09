/*
 * Created by SharpDevelop.
 * User: adam.moseman
 * Date: 6/3/2017
 * Time: 1:24 AM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Collections.Generic;
using MathNet.Numerics.Interpolation;

namespace TilerIsADummy.PrototypeMapGen
{
	/// <summary>
	/// Simple map generator. Creates a tile array, for use in the battle map logic, out of the given height/width values.
	/// </summary>
	
	public static class P_MapBuilder
	{
		public static P_TileArray GenerateRandomMap(int width, int height, P_Render P_Render)
		{
			P_TileArray ReturnedTileArray = new P_TileArray(width, height);
			List<TileType> TileTypeList = DefaultTileTypeList.DefaultList;
			Random rng = new Random();
			double randomnum;
			double MapCenterX = width/2;
			double MapCenterY = height/2;
			double TileCount = width*height;
			double TileCountSqr = Math.Sqrt(TileCount);
			
			TileType Default = GetTileTypeWithRole(TileTypeList, TileRole.Default);
			TileType Water = GetTileTypeWithRole(TileTypeList, TileRole.Water);
			TileType Wall = GetTileTypeWithRole(TileTypeList, TileRole.Wall);
			TileType Floor = GetTileTypeWithRole(TileTypeList, TileRole.Floor);
			TileType Void = GetTileTypeWithRole(TileTypeList, TileRole.Void);
			
			TileFeature Goal = P_DefaultFeatureList.DefaultList[0];
			TileFeature Tree = P_DefaultFeatureList.DefaultList[1];
			TileFeature Rock = P_DefaultFeatureList.DefaultList[2];
			TileFeature Bridge = P_DefaultFeatureList.DefaultList[3];
			
			for(int i=0; i<ReturnedTileArray.Length; i++)
			{
				Tile x = ReturnedTileArray.GetTile(i);
				double TileX = i%width;
				double TileY = i/width;
				double DistToCenter = Math.Sqrt(Math.Pow((TileX - MapCenterX),2) + Math.Pow((TileY - MapCenterY),2));
				
				randomnum = rng.Next(1,(int)TileCountSqr);
				randomnum -= DistToCenter;
				if(randomnum >= (TileCountSqr/2))
				{
					x.ChangeTileType(Default, true);
				}
				else
				{
					x.ChangeTileType(Void, true);
				}
			}
			
			//4-5 algorithm for map boundries
			for(int i=0; i<3; i++)
			{
				P_TileArray CopyMap = ReturnedTileArray.DeepCopy();
				
				for(int j=0; j < ReturnedTileArray.Length; j++)
				{
					int dneighbors=0;
					int vneighbors=0;
					List<Tile> jneighbors=CopyMap.TileNeighbors(j);
					for(int k=0; k<jneighbors.Count;k++)
					{
						if (jneighbors[k].Role == TileRole.Void)
						{
							vneighbors++;
						}
						if (jneighbors[k].Role == TileRole.Default)
						{
							dneighbors++;
						}
					}
					
					if(dneighbors>=2)
					{
						//ReturnedTileArray.MyTiles[j].ChangeTileType(Void, true);
						ReturnedTileArray.MyTiles[j].ChangeTileType(Default, true);
					}
					else if(vneighbors>=2)
					{
						ReturnedTileArray.MyTiles[j].ChangeTileType(Void, true);
						//ReturnedTileArray.MyTiles[j].ChangeTileType(Default, true);
					}
				}
			}
			
			ReturnedTileArray = P_MapBuilder.TrimMapEdges(ReturnedTileArray, Void);
			ReturnedTileArray = P_MapBuilder.PurgeIsolatedTiles(ReturnedTileArray, Void, Default, 1);
			ReturnedTileArray = P_MapBuilder.PurgeIsolatedTiles(ReturnedTileArray, Default, Void, 1);
			//make a river
			int Count=1;
			
			while(Count > 0)
			{
				List<Tile> RiverPath = new List<Tile>();
				bool HitLand = false;
				TileNeighborEnum RiverDirection = TileNeighborEnum.UNASSIGNED;
				TileNeighborEnum EndRiverDirection = TileNeighborEnum.UNASSIGNED;
				int RiverStartIndex = -1;
				int RiverEndIndex = -1;
				// 0-Top 1-Left 2-Bottom 3-Right
				int RiverStartSide = rng.Next(0,4);
				int RiverEndSide = rng.Next(0,4);
				
				if (RiverStartSide==RiverEndSide)
				{
					if(RiverStartSide==1){RiverStartSide=3;}
					else if(RiverStartSide==2){RiverStartSide=0;}
					else if(RiverStartSide==3){RiverStartSide=1;}
					else{RiverStartSide=2;}
				}
				
				if(RiverStartSide==0)
				{
					RiverStartIndex=  rng.Next(0,width);
					int direction = rng.Next(0,1);
					switch(direction)
					{
						case 0:
							RiverDirection= TileNeighborEnum.DownRight;
							break;
						case 1:
							RiverDirection= TileNeighborEnum.DownLeft;
							break;
					}
				}
				if(RiverStartSide==1)
				{
					RiverStartIndex=  rng.Next(0,height)*width;
					int direction = rng.Next(0,2);
					switch(direction)
					{
						case 0:
							RiverDirection= TileNeighborEnum.Right;
							break;
						case 1:
							RiverDirection= TileNeighborEnum.DownRight;
							break;
						case 2:
							RiverDirection= TileNeighborEnum.UpRight;
							break;
					}
				}
				if(RiverStartSide==2)
				{
					RiverStartIndex= (width)*(height) - rng.Next(1,width);
					int direction = rng.Next(0,1);
					switch(direction)
					{
						case 0:
							RiverDirection= TileNeighborEnum.UpLeft;
							break;
						case 1:
							RiverDirection= TileNeighborEnum.UpRight;
							break;
					}
				}
				if(RiverStartSide==3)
				{
					RiverStartIndex =  rng.Next(0,height)*width -1 + width - 1;
					int direction = rng.Next(0,2);
					switch(direction)
					{
						case 0:
							RiverDirection= TileNeighborEnum.Left;
							break;
						case 1:
							RiverDirection= TileNeighborEnum.DownLeft;
							break;
						case 2:
							RiverDirection= TileNeighborEnum.UpLeft;
							break;
					}
				}
				
				if(RiverEndSide==0)
				{
					RiverEndIndex=  rng.Next(0,width);
					int direction = rng.Next(0,1);
					switch(direction)
					{
						case 0:
							EndRiverDirection= TileNeighborEnum.DownLeft;
							break;
						case 1:
							EndRiverDirection= TileNeighborEnum.DownRight;
							break;
					}
				}
				if(RiverEndSide==1)
				{
					RiverEndIndex= rng.Next(0,height)*width;
					int direction = rng.Next(0,2);
					switch(direction)
					{
						case 0:
							EndRiverDirection= TileNeighborEnum.Right;
							break;
						case 1:
							EndRiverDirection= TileNeighborEnum.DownRight;
							break;
						case 2:
							EndRiverDirection= TileNeighborEnum.DownLeft;
							break;
					}
				}
				if(RiverEndSide==2)
				{
					RiverEndIndex= width*height- rng.Next(1,width);
					int direction = rng.Next(0,1);
					switch(direction)
					{
						case 0:
							EndRiverDirection= TileNeighborEnum.UpRight;
							break;
						case 1:
							EndRiverDirection= TileNeighborEnum.UpLeft;
							break;
					}
				}
				if(RiverEndSide==3)
				{
					RiverEndIndex =  rng.Next(0,height)*width + width-1;
					int direction = rng.Next(0,2);
					switch(direction)
					{
						case 0:
							EndRiverDirection= TileNeighborEnum.Left;
							break;
						case 1:
							EndRiverDirection= TileNeighborEnum.DownLeft;
							break;
						case 2:
							EndRiverDirection= TileNeighborEnum.UpLeft;
							break;
					}
				}
				
				if(ReturnedTileArray.MyTiles[RiverStartIndex].Role != TileRole.Void)
				{
					HitLand = true;
				}
				
				while(!HitLand)
				{
					if(!ReturnedTileArray.ValidDirection(RiverStartIndex, RiverDirection))
					{
						break;
					}
					
					RiverStartIndex = ReturnedTileArray.NeighborIndex(RiverStartIndex, RiverDirection);
					
					if(ReturnedTileArray.MyTiles[RiverStartIndex].Role != TileRole.Void)
					{
						HitLand = true;
					}
				}
				
				if(HitLand==false)
				{
					continue;
				}
				
				HitLand = false;
				
				if(ReturnedTileArray.MyTiles[RiverEndIndex].Role != TileRole.Void)
				{
					HitLand = true;
				}
				
				while(!HitLand)
				{
					if(!ReturnedTileArray.ValidDirection(RiverEndIndex, EndRiverDirection))
					{
						break;
					}
					
					RiverEndIndex = ReturnedTileArray.NeighborIndex(RiverEndIndex, EndRiverDirection);
					
					if(ReturnedTileArray.MyTiles[RiverEndIndex].Role != TileRole.Void)
					{
						HitLand = true;
					}
				}
				
				if(HitLand==false)
				{
					continue;
				}
				
				RiverPath = P_MapBuilder.RiverMeander(ReturnedTileArray,RiverStartIndex,RiverEndIndex,RiverDirection);
				
				for(int i=0;i<RiverPath.Count;i++)
				{
					RiverPath[i].ChangeTileType(Water,true);
				}
				Count--;
			}
			
			ReturnedTileArray = P_MapBuilder.PurgeIsolatedTiles(ReturnedTileArray, Default, Void, 2);
			
			//add a bridge
			List<List<P_CubeCoords>> DefaultSpaces = ListOfHexGroups(ReturnedTileArray, Default.Role);
			if(DefaultSpaces.Count>1)
			{
				List<P_CubeCoords> ZoneARiverBank = new List<P_CubeCoords>();
				List<P_CubeCoords> ZoneBRiverBank = new List<P_CubeCoords>();
				
				for(int i=0; i<DefaultSpaces.Count;i++)
				{
					for(int j=0; j<DefaultSpaces[i].Count;j++)
					{
						List<Tile> CheckForWater = ReturnedTileArray.TileNeighbors(ReturnedTileArray.CubeToIndex(DefaultSpaces[i][j]));
						bool nexttowater=false;
						for(int k=0;k<CheckForWater.Count;k++)
						{
							
							if(CheckForWater[k].Role == TileRole.Water)
							{
								nexttowater = true;
								break;
							}
						}
						if(nexttowater)
						{
							switch(i)
							{
								case 0:
									ZoneARiverBank.Add(DefaultSpaces[i][j]);
									break;
								case 1:
									ZoneBRiverBank.Add(DefaultSpaces[i][j]);
									break;             
							}
						}
					}
				}
				
				if(ZoneARiverBank.Count > ZoneBRiverBank.Count)
				{
					List<P_CubeCoords> temp = ZoneARiverBank;
					ZoneARiverBank = ZoneBRiverBank;
					ZoneBRiverBank = temp;
				}
				P_CubeCoords BridgeStart = ZoneARiverBank[rng.Next(0,ZoneARiverBank.Count)];
				P_CubeCoords BridgeEnd = ZoneBRiverBank[0];
				double bridgedist = P_CubeCoords.cube_distance(BridgeStart, BridgeEnd);
				for(int i=1; i<ZoneBRiverBank.Count; i++)
				{
					if(P_CubeCoords.cube_distance(BridgeStart, ZoneBRiverBank[i]) < bridgedist)
					{
						BridgeEnd = ZoneBRiverBank[i];
						bridgedist = P_CubeCoords.cube_distance(BridgeStart, ZoneBRiverBank[i]);
					}
				}
				
				List<P_CubeCoords> BridgeCoords = P_CubeCoords.cube_linedraw(BridgeStart,BridgeEnd);
				for(int i=1; i<BridgeCoords.Count-1; i++)
				{
					ReturnedTileArray.MyTiles[ReturnedTileArray.CubeToIndex(BridgeCoords[i])].FeatureAdd(Bridge);
				}
			}
			
			//make a building
			
			List<P_CubeCoords> ValidSpawn = FindClearSpaces(ReturnedTileArray, Default.Role, 3);
			if(ValidSpawn.Count == 0)
			{
				throw new Exception("Can't spawn the fucking building");
			}
			
			int BuildingSource = rng.Next(0,ValidSpawn.Count);
			ReturnedTileArray.MyTiles[ReturnedTileArray.CubeToIndex(ValidSpawn[BuildingSource])].ChangeTileType(Floor,true);
			
			List<int> PossibleBuildingTiles = GetNeighborhoodTiles(ReturnedTileArray, ValidSpawn[BuildingSource], 1);
			PossibleBuildingTiles.Remove(ReturnedTileArray.CubeToIndex(ValidSpawn[BuildingSource]));
			List<int> BuildingTiles = new List<int>();
			BuildingTiles.Add(ReturnedTileArray.CubeToIndex(ValidSpawn[BuildingSource]));
			int BuildingSize=1;
			while(BuildingSize < 6)
			{
				int Nomination = rng.Next(0,PossibleBuildingTiles.Count);
				List<int> NomNeighbors = ReturnedTileArray.TileNeighborsIndex(PossibleBuildingTiles[Nomination]);
				bool valid = false;
				for(int i=0; i<NomNeighbors.Count;i++)
				{
					if(ReturnedTileArray.MyTiles[NomNeighbors[i]].Role == TileRole.Floor)
					{
						valid = true;
						break;
					}
				}
				
				if(valid)
				{
					ReturnedTileArray.MyTiles[PossibleBuildingTiles[Nomination]].ChangeTileType(Floor,true);
					BuildingTiles.Add(PossibleBuildingTiles[Nomination]);
					PossibleBuildingTiles.RemoveAt(Nomination);
					BuildingSize++;
				}
			}
			List<Tile> FloorNeighbors = new List<Tile>();
			for(int i=0; i<BuildingTiles.Count;i++)
			{
				FloorNeighbors.AddRange(ReturnedTileArray.TileNeighbors(BuildingTiles[i]));
			}
			
			List<Tile> Walls = new List<Tile>();
			for(int i=0; i<FloorNeighbors.Count;i++)
			{
				if(FloorNeighbors[i].Role != Floor.Role && FloorNeighbors[i].Role != Wall.Role)
				{
					FloorNeighbors[i].ChangeTileType(Wall, true);
					Walls.Add(FloorNeighbors[i]);
				}
			}
			
			int randomint;
			
			while(true)
			{
				randomint = rng.Next(0,FloorNeighbors.Count);
				if(FloorNeighbors[randomint].Role == Floor.Role)
				{
					FloorNeighbors[randomint].FeatureAdd(Goal);
					break;
				}
			}
			
			randomint = rng.Next(0,Walls.Count);
			Walls[randomint].ChangeTileType(Floor, true);
			
			//sprinkle tree clusters
			ReturnedTileArray = ScatterFeature(ReturnedTileArray, 10, 0, 10, 6, 10, Default.Role, Tree, rng);

			//sprinkle rocks
			ReturnedTileArray = ScatterFeature(ReturnedTileArray, 25, 0, 3, 6, 10, Default.Role, Rock, rng);
			
			return ReturnedTileArray;
			
			//throw new NotImplementedException();
		}
		
		private static List<List<P_CubeCoords>> ListOfHexGroups(P_TileArray tilearray, TileRole role)
		{
			List<List<P_CubeCoords>> ReturnValue = new List<List<P_CubeCoords>>();
			for(int i=0; i<tilearray.Length; i++)
			{
				if(tilearray.MyTiles[i].Role == role)
				{
					bool found = false;
					for(int j=0; j < ReturnValue.Count; j++)
					{
						if(ReturnValue[j].Contains(tilearray.IndexToCube(i)))
						{
							found = true;
						}
					}
					if(!found)
					{
						List<P_CubeCoords> ListOfNeighbors = new List<P_CubeCoords>();
						ListOfNeighbors.Add(tilearray.IndexToCube(i));
						List<int> ToCheck = tilearray.TileNeighborsIndex(i);
						while(ToCheck.Count > 0)
						{
							int CheckTile = ToCheck[0];
							if(tilearray.MyTiles[CheckTile].Role == role)
							{
								if(!ListOfNeighbors.Contains(tilearray.IndexToCube(CheckTile)))
								{
									ListOfNeighbors.Add(tilearray.IndexToCube(CheckTile));
									List<int> CheckNeighbors = tilearray.TileNeighborsIndex(CheckTile);
									for(int k=0; k<CheckNeighbors.Count;k++)
									{
										ToCheck.Add(CheckNeighbors[k]);
									}
								}
							}
							ToCheck.RemoveAt(0);
						}
						ReturnValue.Add(ListOfNeighbors);
					}
				}
			}
			return ReturnValue;
		}
		private static P_TileArray ScatterFeature(P_TileArray tilearray, int clustertightness, int clustercountmin, int clustercountmax, int featurecount,
		                                   int clustermaxdist, TileRole role, TileFeature feature, Random rng)
		{
			P_TileArray NewTileArray = tilearray;
			int clustercount = rng.Next(clustercountmin, clustercountmax+1);
			int featurechance = clustertightness;
			while(clustercount > 0)
			{
				List<P_CubeCoords> ValidSpawn = FindClearSpaces(NewTileArray, role, 2);
				int randomint = rng.Next(0,ValidSpawn.Count);
				P_CubeCoords featuresource = ValidSpawn[randomint];
				NewTileArray.MyTiles[NewTileArray.CubeToIndex(featuresource)].FeatureAdd(feature);
				int Count = featurecount-1;
				int Ring= 0;
				while(Count>0)
				{
					Ring++;
					if(Ring > clustermaxdist){break;}
					List<P_CubeCoords> possiblefeature = GetRing(featuresource, Ring);
					for(int i=0;i<possiblefeature.Count;i++)
					{
						int index=NewTileArray.CubeToIndex(possiblefeature[i]);
						if(NewTileArray.validindex(index))
						{
							if(rng.Next(1,101) < featurechance && NewTileArray.MyTiles[index].Role == role)
							{
								NewTileArray.MyTiles[index].FeatureAdd(feature);
								Count--;
								if(Count == 0){break;}
							}
						}
					}
				}
				clustercount--;
			}
			
			return NewTileArray;
		}
		
		private static List<P_CubeCoords> GetRing(P_CubeCoords center, int radius)
		{
			List<P_CubeCoords> HexList = new List<P_CubeCoords>();
			P_CubeCoords coord = center;
			
			for(int i=0;i<radius;i++)
			{
				coord = coord.DownLeft;
			}
			TileNeighborEnum dir = TileNeighborEnum.Right;
			int count = 0;
			int steps = radius;
			while (count<6)
			{
				for(int i=0; i<steps;i++)
				{
					HexList.Add(coord);
					coord = P_CubeCoords.StepForward(coord, dir);
				}
				dir = TurnCounterClockwise(dir);
				count++;
			}
			
			return HexList;
		}
		
		private static List<P_CubeCoords> FindClearSpaces(P_TileArray tilearray, TileRole role, int rings)
		{
			List<P_CubeCoords> ReturnValue = new List<P_CubeCoords>();
			if(rings<0)
			{
				return ReturnValue; //hurrr
			}
			for(int i=0; i<tilearray.Length;i++)
			{
				if(tilearray.MyTiles[i].Role == role)
				{
					if(rings<1)
					{
						ReturnValue.Add(tilearray.IndexToCube(i));
						continue;
					}
					//int count = rings-1;
					List<int> Neighbors = GetNeighborhoodTiles(tilearray, tilearray.IndexToCube(i), rings);
					/*
					while(count>0)
					{
						int counter = Neighbors.Count;
						for(int k=0;k<counter;k++)
						{
							List<int> NeighborNeighbors = tilearray.TileNeighborsIndex(Neighbors[k]);
							for(int j=0;j<NeighborNeighbors.Count;j++)
							{
								if(!Neighbors.Contains(NeighborNeighbors[j]))
								{
									Neighbors.Add(NeighborNeighbors[j]);
								}
							}
						}
						count--;
					}
					*/
					bool valid=true;
					for(int j=0;j<Neighbors.Count;j++)
					{
						if(tilearray.MyTiles[Neighbors[j]].Role != TileRole.Default)
						{
							valid=false;
							break;
						}
					}
					if(valid)
					{
						ReturnValue.Add(tilearray.IndexToCube(i));
					}
				}
			}
			
			return ReturnValue;
		}
		
		private static List<int> GetNeighborhoodTiles(P_TileArray tilearray, P_CubeCoords point, int rings)
		{
			List<int> ReturnValue = new List<int>();
			ReturnValue.Add(tilearray.CubeToIndex(point));
			if(rings<1)
			{
				return ReturnValue;
			}
			
			int count = rings;
			while(count>0)
			{
				int counter = ReturnValue.Count;
				for(int k=0;k<counter;k++)
				{
					List<int> NeighborNeighbors = tilearray.TileNeighborsIndex(ReturnValue[k]);
					for(int j=0;j<NeighborNeighbors.Count;j++)
					{
						if(!ReturnValue.Contains(NeighborNeighbors[j]))
						{
							ReturnValue.Add(NeighborNeighbors[j]);
						}
					}
				}
				count--;
			}
			return ReturnValue;
		}
		
		private static TileType GetTileTypeWithRole(List<TileType> tiletypelist, TileRole role)
		{
			for (int i =0; i < tiletypelist.Count; i++)
			{
				if (tiletypelist[i].Role == role)
				{
					return tiletypelist[i];
				}
			}
			throw new Exception(); // Role requested does not exist!!!
		}
		
		private static P_TileArray PurgeIsolatedTiles(P_TileArray tilearray, ITileType purgetiles, ITileType changedtiles, int maxremaininggroups)
		{
			P_TileArray PurgedTileArray = tilearray;
			List<List<Tile>> ListOfNeighborList = FindSeperatedRoleGroups(tilearray, purgetiles.Role);
			
			/*
			for(int i=0; i<tilearray.Length; i++)
			{
				if(PurgedTileArray.MyTiles[i].Role == purgetiles.Role)
				{
					bool found = false;
					for(int j=0; j < ListOfNeighborList.Count; j++)
					{
						if(ListOfNeighborList[j].Contains(PurgedTileArray.MyTiles[i]))
						{
							found = true;
						}
					}
					if(!found)
					{
						List<Tile> ListOfNeighbors = new List<Tile>();
						ListOfNeighbors.Add(PurgedTileArray.MyTiles[i]);
						List<int> ToCheck = PurgedTileArray.TileNeighborsIndex(i);
						while(ToCheck.Count > 0)
						{
							int CheckTile = ToCheck[0];
							if(PurgedTileArray.MyTiles[CheckTile].Role == purgetiles.Role)
							{
								if(!ListOfNeighbors.Contains(PurgedTileArray.MyTiles[CheckTile]))
								{
									ListOfNeighbors.Add(PurgedTileArray.MyTiles[CheckTile]);
									List<int> CheckNeighbors = PurgedTileArray.TileNeighborsIndex(CheckTile);
									for(int k=0; k<CheckNeighbors.Count;k++)
									{
										ToCheck.Add(CheckNeighbors[k]);
									}
								}
							}
							ToCheck.RemoveAt(0);
						}
						ListOfNeighborList.Add(ListOfNeighbors);
					}
				}
			}
			*/
			
			int Count = maxremaininggroups;
			while(Count>0)
			{
				if(ListOfNeighborList.Count == 0)
				{
					return PurgedTileArray;
				}
				
				List<Tile> LongestList = ListOfNeighborList[0];
				for(int i=0;i<ListOfNeighborList.Count;i++)
				{
					if(ListOfNeighborList[i].Count > LongestList.Count)
					{
						LongestList = ListOfNeighborList[i];
					}
				}
				
				ListOfNeighborList.Remove(LongestList);
				Count--;
			}
			
			for(int i=0;i<ListOfNeighborList.Count;i++)
			{
				for(int j=0;j<ListOfNeighborList[i].Count;j++)
				{
					
					ListOfNeighborList[i][j].ChangeTileType(changedtiles, true);
				}
			}
			
			return PurgedTileArray;
		}
		
		private static List<List<Tile>> FindSeperatedRoleGroups(P_TileArray tilearray, TileRole role)
		{
			List<List<Tile>> ReturnValue = new List<List<Tile>>();
			for(int i=0; i<tilearray.Length; i++)
			{
				if(tilearray.MyTiles[i].Role == role)
				{
					bool found = false;
					for(int j=0; j < ReturnValue.Count; j++)
					{
						if(ReturnValue[j].Contains(tilearray.MyTiles[i]))
						{
							found = true;
						}
					}
					if(!found)
					{
						List<Tile> ListOfNeighbors = new List<Tile>();
						ListOfNeighbors.Add(tilearray.MyTiles[i]);
						List<int> ToCheck = tilearray.TileNeighborsIndex(i);
						while(ToCheck.Count > 0)
						{
							int CheckTile = ToCheck[0];
							if(tilearray.MyTiles[CheckTile].Role == role)
							{
								if(!ListOfNeighbors.Contains(tilearray.MyTiles[CheckTile]))
								{
									ListOfNeighbors.Add(tilearray.MyTiles[CheckTile]);
									List<int> CheckNeighbors = tilearray.TileNeighborsIndex(CheckTile);
									for(int k=0; k<CheckNeighbors.Count;k++)
									{
										ToCheck.Add(CheckNeighbors[k]);
									}
								}
							}
							ToCheck.RemoveAt(0);
						}
						ReturnValue.Add(ListOfNeighbors);
					}
				}
			}
			return ReturnValue;
		}

		private static bool IsRiverBroken(P_TileArray tilearray, List<P_CubeCoords> coords)
		{
			//bool brokenriver = false;
			for(int i=0; i<coords.Count;i++)
			{
				//if(!brokenriver)
				//{
				if(tilearray.MyTiles[tilearray.CubeToIndex(coords[i])].Role == TileRole.Void)
				{
					//brokenriver=true;
					//substart = coords[i-1];
					return true;
				}
				//}
				//else
				//{
				//	if(tilearray.MyTiles[tilearray.CubeToIndex(coords[i])].Role != TileRole.Void)
				//	{
				//		subend = coords[i];
				//		sanity = true;
				//		break;
				//	}
				//}
			}
			
			return false;
		}
		
		private static List<Tile> RiverMeander(P_TileArray tilearray, int riverindex, int rivergoal, TileNeighborEnum direction)
		{
			List<Tile> ReturnTiles = new List<Tile>();
			//List<HexCubeCoords> coords = PathfindRiver(tilearray, tilearray.IndexToCube(riverindex), tilearray.IndexToCube(rivergoal));
			List<P_CubeCoords> coords = DumbRiverDraw(tilearray, riverindex, rivergoal);
			
			for(int i = 0; i<coords.Count; i++)
			{
				ReturnTiles.Add(tilearray.MyTiles[tilearray.CubeToIndex(coords[i])]);
				List<int> Drench = tilearray.TileNeighborsIndex(tilearray.CubeToIndex(coords[i]));
				for(int w=0;w<Drench.Count;w++)
				{
					if(tilearray.MyTiles[Drench[w]].Role != TileRole.Void && tilearray.MyTiles[Drench[w]].Role != TileRole.Water && !coords.Contains(tilearray.IndexToCube(Drench[w])))
					{
						ReturnTiles.Add(tilearray.MyTiles[Drench[w]]);
						break;
					}}
			}
			return ReturnTiles;
		}
		
		private static List<Tile> BresenhamRiver(P_TileArray tilearray, int riverindex, int rivergoal, TileNeighborEnum direction)
		{
			List<int> BresLine = BresenhamLine(riverindex%tilearray.Width, riverindex/tilearray.Width,
			                                   rivergoal%tilearray.Width, rivergoal/tilearray.Width, tilearray.Width);
			//List<int> BresLine = BresenhamLine(0,0, tilearray.Width-1, tilearray.MyTiles.Length/tilearray.Width-1, tilearray.Width);
			List<Tile> ReturnTiles = new List<Tile>();
			for(int i=0; i<BresLine.Count;i++)
			{
				if(!tilearray.validindex(BresLine[i]))
				{
					continue;
				}
				
				if(tilearray.MyTiles[BresLine[i]].Role == TileRole.Void){continue;}
				ReturnTiles.Add(tilearray.MyTiles[BresLine[i]]);
			}
			
			return ReturnTiles;
		}
		
		private static TileNeighborEnum TurnClockwise(TileNeighborEnum facing)
		{
			switch(facing)
			{
				case TileNeighborEnum.DownRight:
					return TileNeighborEnum.DownLeft;
				case TileNeighborEnum.DownLeft:
					return TileNeighborEnum.Left;
				case TileNeighborEnum.Left:
					return TileNeighborEnum.UpLeft;
				case TileNeighborEnum.UpLeft:
					return TileNeighborEnum.UpRight;
				case TileNeighborEnum.UpRight:
					return TileNeighborEnum.Right;
				case TileNeighborEnum.Right:
					return TileNeighborEnum.DownRight;
			}
			return facing;
		}
		
		private static TileNeighborEnum TurnCounterClockwise(TileNeighborEnum facing)
		{
			switch(facing)
			{
				case TileNeighborEnum.DownRight:
					return TileNeighborEnum.Right;
				case TileNeighborEnum.DownLeft:
					return TileNeighborEnum.DownRight;
				case TileNeighborEnum.Left:
					return TileNeighborEnum.DownLeft;
				case TileNeighborEnum.UpLeft:
					return TileNeighborEnum.Left;
				case TileNeighborEnum.UpRight:
					return TileNeighborEnum.UpLeft;
				case TileNeighborEnum.Right:
					return TileNeighborEnum.UpRight;
			}
			return facing;
		}
		
		private static List<int> BresenhamLine(int x,int y,int x2,int y2,int width) {
			List<int> ReturnMe = new List<int>();
			int XDiff = x2 - x ;
			int YDiff = y2 - y ;
			int dx1 = 0, dy1 = 0, dx2 = 0, dy2 = 0 ;
			if (XDiff<0) dx1 = -1 ; else if (XDiff>0) dx1 = 1 ;
			if (YDiff<0) dy1 = -1 ; else if (YDiff>0) dy1 = 1 ;
			if (XDiff<0) dx2 = -1 ; else if (XDiff>0) dx2 = 1 ;
			int longest = Math.Abs(XDiff) ;
			int shortest = Math.Abs(YDiff) ;
			if (!(longest>shortest)) {
				longest = Math.Abs(YDiff) ;
				shortest = Math.Abs(XDiff) ;
				if (YDiff<0) dy2 = -1 ; else if (YDiff>0) dy2 = 1 ;
				dx2 = 0 ;
			}
			int numerator = longest >> 1 ;
			for (int i=0;i<=longest;i++) {
				ReturnMe.Add(x+y*width);
				numerator += shortest ;
				if (!(numerator<longest)) {
					numerator -= longest ;
					x += dx1 ;
					y += dy1 ;
				} else {
					x += dx2 ;
					y += dy2 ;
				}
			}
			return ReturnMe;
		}
		
		private static P_TileArray TrimMapEdges(P_TileArray tilearray, ITileType type)
		{
			P_TileArray ReturnMe = tilearray;
			for(int x = 0; x<ReturnMe.Width; x++)
			{
				for(int y=0; y<ReturnMe.Length/tilearray.Width;y++)
				{
					if(y==0 || y == ReturnMe.Length/tilearray.Width -1)
					{
						tilearray.MyTiles[x+y*ReturnMe.Width].ChangeTileType(type, true);
						continue;
					}
					if(x==0 || x==tilearray.Width-1)
					{
						tilearray.MyTiles[x+y*ReturnMe.Width].ChangeTileType(type, true);
					}
				}
			}
			return ReturnMe;
		}
		
		private static List<P_CubeCoords> DumbRiverDraw(P_TileArray tilearray, int riverindex, int rivergoal)
		{
			List<P_CubeCoords> coords = P_CubeCoords.cube_linedraw(tilearray.IndexToCube(riverindex),tilearray.IndexToCube(rivergoal));
			if(!IsRiverBroken(tilearray, coords))
			{
				return coords;
			}
			
			P_CubeCoords RiverStartHex = tilearray.IndexToCube(riverindex);
			P_CubeCoords RiverEndHex = tilearray.IndexToCube(rivergoal);
			
			P_CubeCoords NewPoint = ClosestSharedVisable(tilearray, RiverStartHex, RiverEndHex);
			
			if(NewPoint.X == -1)
			{
				return PathfindRiver(tilearray, RiverStartHex,RiverEndHex);
			}
			
			P_CubeCoords startnplerp = P_CubeCoords.cube_lerp(RiverStartHex, NewPoint, .75);
			P_CubeCoords endnplerp = P_CubeCoords.cube_lerp(NewPoint, RiverEndHex, .25);
			
			coords = P_CubeCoords.cube_linedraw(RiverStartHex, startnplerp);
			coords.AddRange(DumbRiverDraw(tilearray, tilearray.CubeToIndex(startnplerp), tilearray.CubeToIndex(endnplerp)));
			coords.AddRange(P_CubeCoords.cube_linedraw(endnplerp, RiverEndHex));
			
			return coords;
		}
		
		private static List<P_CubeCoords> StupidRiverDraw(P_TileArray tilearray, int riverindex, int rivergoal)
		{
			List<P_CubeCoords> coords = P_CubeCoords.cube_linedraw(tilearray.IndexToCube(riverindex),tilearray.IndexToCube(rivergoal));
			//bool brokenriver = false;
			P_CubeCoords substart = new P_CubeCoords(-1,-1,-1);
			P_CubeCoords subend = new P_CubeCoords(-1,-1,-1);
			
			if(!IsRiverBroken(tilearray, coords))
			{
				//Console.WriteLine(riverindex + " " + rivergoal + "UNBROKEN RIVER DRAWN");
				return coords;
			}
			
			for(int i=0; i<coords.Count;i++)
			{
				if(tilearray.MyTiles[tilearray.CubeToIndex(coords[i])].Role == TileRole.Void)
				{
					substart = coords[i-1];
					break;
				}
			}
			
			for(int i=coords.Count-1; i>0; i--)
			{
				if(tilearray.MyTiles[tilearray.CubeToIndex(coords[i])].Role == TileRole.Void)
				{
					subend = coords[i-1];
					break;
				}
			}
			
			List<P_CubeCoords> ReturnValue = new List<P_CubeCoords>();
			P_CubeCoords midpoint = new P_CubeCoords((substart.X + subend.X)/2,(substart.Y+subend.Y)/2,(substart.Z+subend.Z)/2);
			int midpointindex = tilearray.CubeToIndex(midpoint);
			int midpointX = midpointindex % tilearray.Width;
			int midpointY = midpointindex / tilearray.Width;
			
			bool onefound = false;
			bool twofound = false;
			
			int substartX = tilearray.CubeToIndex(substart)%tilearray.Width;
			int substartY = tilearray.CubeToIndex(substart)/tilearray.Width;
			int perpX = -1*(substartY - midpointY);
			int perpY = (substartX - midpointX);
			
			double perpangle = Math.Atan2(perpY,perpX);
			
			//int relperpX = midpointX + perpX;
			//int relperpY = midpointY + perpY;
			
			int newpoint = 0;
			
			bool checkone = true;
			bool checktwo = true;
			int magnitude = 0;
			while(checkone)
			{
				magnitude++;
				double XCheck = Math.Round(midpointX + (Math.Cos(perpangle)*magnitude));
				double YCheck = Math.Round(midpointY + (Math.Sin(perpangle)*magnitude));
				int indexcheck = Convert.ToInt32(XCheck+YCheck*tilearray.Width);
				
				if(!tilearray.validindex(indexcheck))
				{
					checkone = false;
				}
				
				else if(tilearray.MyTiles[indexcheck].Role != TileRole.Void)
				{
					List<P_CubeCoords> line1 = P_CubeCoords.cube_linedraw(tilearray.IndexToCube(riverindex),tilearray.IndexToCube(indexcheck));
					List<P_CubeCoords> line2 = P_CubeCoords.cube_linedraw(tilearray.IndexToCube(indexcheck),tilearray.IndexToCube(rivergoal));
					if(!IsRiverBroken(tilearray, line1) && !IsRiverBroken(tilearray, line2))
					{
						newpoint = indexcheck;
						checkone = false;
					}
				}
			}
			
			int priormag = int.MaxValue;
			if(onefound){checktwo=false;}
			perpangle = perpangle + Math.PI;
			magnitude = 0;
			
			while(checktwo)
			{
				magnitude++;
				double XCheck = Math.Round(midpointX + (Math.Cos(perpangle)*magnitude));
				double YCheck = Math.Round(midpointY + (Math.Sin(perpangle)*magnitude));
				int indexcheck = Convert.ToInt32(XCheck+YCheck*tilearray.Width);
				
				if(!tilearray.validindex(indexcheck))
				{
					checktwo = false;
				}
				
				else if(tilearray.MyTiles[indexcheck].Role != TileRole.Void)
				{
					List<P_CubeCoords> line1 = P_CubeCoords.cube_linedraw(tilearray.IndexToCube(riverindex),tilearray.IndexToCube(indexcheck));
					List<P_CubeCoords> line2 = P_CubeCoords.cube_linedraw(tilearray.IndexToCube(indexcheck),tilearray.IndexToCube(rivergoal));
					if(!IsRiverBroken(tilearray, line1) && !IsRiverBroken(tilearray, line2))
					{
						newpoint = indexcheck;
						twofound = true;
						checktwo = false;
					}
				}
			}
			
			if(!onefound && !twofound)
			{
				return coords;
				throw new Exception("FUCK UH THATS NOT SUPPOSED TO HAPPEN AT ALL");
			}
			
			if(newpoint == riverindex || newpoint == rivergoal)
			{
				return coords;
			}
			//Console.WriteLine("X:"+riverindex%tilearray.Width + " Y:" + riverindex/tilearray.Width + " | X:" + newpoint%tilearray.Width + " Y:" + newpoint/tilearray.Width + " | X:" + rivergoal%tilearray.Width + " Y:" + rivergoal/tilearray.Width);
			ReturnValue.AddRange(StupidRiverDraw(tilearray, riverindex, newpoint));
			ReturnValue.AddRange(StupidRiverDraw(tilearray, newpoint, rivergoal));
			//Console.WriteLine(riverindex + " " + newpoint + " " + rivergoal + "DRAWN SUCCESFULLY!");
			return ReturnValue;
		}
		
		private static List<P_CubeCoords> BruteForceVisability(P_TileArray tilearray, P_CubeCoords location)
		{
			List<P_CubeCoords> ReturnValue = new List<P_CubeCoords>();
			for(int i=0;i<tilearray.MyTiles.Length;i++)
			{
				if(tilearray.IsVisable(location, tilearray.IndexToCube(i))){ReturnValue.Add(tilearray.IndexToCube(i));}
			}
			
			return ReturnValue;
		}
		
		private static P_CubeCoords ClosestSharedVisable(P_TileArray tilearray, P_CubeCoords HexA, P_CubeCoords HexB)
		{
			List<P_CubeCoords> VisFromA = BruteForceVisability(tilearray, HexA);
			List<P_CubeCoords> VisFromB = BruteForceVisability(tilearray, HexB);
			
			if(VisFromA.Count == 0 || VisFromB.Count == 0)
			{
				return new P_CubeCoords(-1,-1,-1);
			}
			
			List<P_CubeCoords> ShorterList = VisFromA;
			List<P_CubeCoords> LongerList = VisFromB;
			if(VisFromA.Count > VisFromB.Count)
			{
				ShorterList = VisFromB;
				LongerList = VisFromA;
			}
			
			P_CubeCoords ReturnValue = new P_CubeCoords(-1,-1,-1);
			
			for(int i=0; i<ShorterList.Count;i++)
			{
				if(LongerList.Contains(ShorterList[i]))
				{
					if(ReturnValue.X==-1)
					{
						ReturnValue = ShorterList[i];
					}
					else if(P_CubeCoords.cube_distance(ShorterList[i], HexA) + P_CubeCoords.cube_distance(ShorterList[i], HexB)
					        < P_CubeCoords.cube_distance(ReturnValue, HexA) + P_CubeCoords.cube_distance(ReturnValue, HexB))
					{
						ReturnValue = ShorterList[i];
					}
				}
			}
			
			return ReturnValue;
		}
		
		private static List<P_CubeCoords> PathfindRiver(P_TileArray tilearray, P_CubeCoords start, P_CubeCoords end)
		{
			List<P_CubeCoords> OpenList = new List<P_CubeCoords>();
			OpenList.Add(start);
			List<P_CubeCoords> ClosedList = new List<P_CubeCoords>();
			P_CubeCoords CameFrom = start;
			double gScore = double.MaxValue;
			//List<HexCubeCoords> ReturnList = new List<HexCubeCoords>();
			
			while(OpenList.Count != 0)
			{
				double bestscore = double.MaxValue;
				int addthis = -1;
				P_CubeCoords check = OpenList[OpenList.Count-1];
				List<int> neighbors = tilearray.TileNeighborsIndex(tilearray.CubeToIndex(check));
				for(int i=0; i<neighbors.Count; i++)
				{
					if(OpenList.Contains(tilearray.IndexToCube(neighbors[i])) ||
					   ClosedList.Contains(tilearray.IndexToCube(neighbors[i])))
					{
						continue;
					}
					if(tilearray.MyTiles[neighbors[i]].Role == TileRole.Void)
					{
						continue;
					}
					
					gScore = OpenList.Count + P_CubeCoords.cube_distance(end, tilearray.IndexToCube(neighbors[i]));
					if(gScore<bestscore)
					{
						bestscore = gScore;
						addthis = neighbors[i];
					}
					
				}
				
				if(addthis == -1)
				{
					ClosedList.Add(OpenList[OpenList.Count-1]);
					OpenList.RemoveAt(OpenList.Count-1);
					if(OpenList.Count == 0)
					{
						return OpenList;
					}
				}
				
				else
				{
					OpenList.Add(tilearray.IndexToCube(addthis));
					if(OpenList[OpenList.Count-1].X == end.X && OpenList[OpenList.Count-1].Y == end.Y && OpenList[OpenList.Count-1].Z == end.Z)
					{
						return OpenList;
					}
				}
			}
			return OpenList;
		}
		
	}
}
