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
	/// A class, containing an array of tiles. Methods are for accessing these tiles, based either on the index, or via hex cube coordinates.
	/// </summary>
	/// 
	public enum TileNeighborEnum
	{
		UNASSIGNED = 0,
		UpLeft = 1,
		UpRight = 2,
		Right = 3,
		DownRight = 4,
		DownLeft = 5,
		Left = 6
	}
	
	public class P_TileArray : IP_Renderable
	{
		private Tile[] _MyTiles;
		private int _Width;
		
		public P_TileArray(int width,int height)
		{
			_Render = new TilerIsADummy.PrototypeMapGen.PrototypeComponents.P_RenderComponent();
			_MyTiles = new Tile[width*height];
			_Width = width;
			TileType DefaultTileType = new TileType("DEFAULT", false, false, TileColor.Black, TileRole.Void);
			
			for(int i = 0; i < _MyTiles.Length; i++)
			{
				Tile Thing = new Tile(DefaultTileType);
				_MyTiles[i] = Thing;
			}
			
			_Render.SizeX = 0;
			_Render.SizeY = 0;
			
			int woffset = 0;
			
			for(int i = 0; i<_MyTiles.Length;i++)
			{
				_MyTiles[i].Render.OffsetBoss = this;
				if(i%_Width != 0){woffset+=1;}
				else
				{
					if ((i/_Width&1) == 0) {woffset = 0;}
					else {woffset = 1;}
				}
				
				MyTiles[i].Render.MyXCoords = i%_Width+woffset;
				MyTiles[i].Render.MyYCoords = i/_Width;
			}
		}
		
		public P_CubeCoords IndexToCube(int index)
		{
			int XOffset = index%_Width;
			
			int Z = (index-XOffset)/_Width; // Y
			int Y = XOffset - (Z-(Z&1))/2; 
			int X = -Y-Z;
			P_CubeCoords returnthis = new P_CubeCoords(X,Y,Z);
			return returnthis;
		}
		
		public int Length
		{
			get
			{
				return _MyTiles.Length;
			}
			set
			{
				//nope
			}
		}
		
		public int CubeToIndex(P_CubeCoords cube)
		{
			int returnme = ((cube.Z * _Width)+(cube.Y + (cube.Z - (cube.Z&1))/2));
			return returnme;
		}
		
		public Tile GetTile(P_CubeCoords cube)
		{
			return _MyTiles[CubeToIndex(cube)];
		}
		
		public Tile GetTile(int tindex)
		{
			return _MyTiles[tindex];
		}
		
		public Tile[] MyTiles
		{
			get {return _MyTiles;}
			set {_MyTiles = value;}
		}
		
		public bool IsTopRow(int index)
		{
			if(index/_Width == 0)
			{
				return true;
			}
			return false;
		}
		
		public bool IsLeftRow(int index)
		{
			if(index%_Width == 0)
			{
				return true;
			}
			return false;
		}
		
		public bool IsRightRow(int index)
		{
			if(index%_Width == _Width-1)
			{
				return true;
			}
			return false;
		}
		
		public bool IsBottomRow(int index)
		{
			if(index/_Width==this._MyTiles.Length/_Width)
			{
				return true;
			}
			return false;
		}

		public List<int> TileNeighborsIndex(int tileindex)
		{
			P_CubeCoords tilehex = this.IndexToCube(tileindex);
			List<int> neighbors = new List<int>();
			if(!this.IsLeftRow(tileindex))
			{
				if(validindex(this.CubeToIndex(tilehex.Left)))
				{
					neighbors.Add(this.CubeToIndex(tilehex.Left));
				}
				if(!this.IsTopRow(tileindex))
				{
					if(validindex(this.CubeToIndex(tilehex.UpLeft)))
					{
						neighbors.Add(this.CubeToIndex(tilehex.UpLeft));
					}
				}
				if(!this.IsBottomRow(tileindex))
				{
					if(validindex(this.CubeToIndex(tilehex.DownLeft)))
					{
						neighbors.Add(this.CubeToIndex(tilehex.DownLeft));
					}
				}
			}
			if(!this.IsRightRow(tileindex))
			{
				
				if(validindex(this.CubeToIndex(tilehex.Right)))
				{
					neighbors.Add(this.CubeToIndex(tilehex.Right));
				}
				if(!this.IsTopRow(tileindex))
				{
					if(validindex(this.CubeToIndex(tilehex.UpRight)))
					{
						neighbors.Add(this.CubeToIndex(tilehex.UpRight));
					}
				}
				if(!this.IsBottomRow(tileindex))
				{
					if(validindex(this.CubeToIndex(tilehex.DownRight)))
					{
						neighbors.Add(this.CubeToIndex(tilehex.DownRight));
					}
				}
			}
			return neighbors;
		}
		
		public List<Tile> TileNeighbors(int tileindex)
		{
			P_CubeCoords tilehex = this.IndexToCube(tileindex);
			List<Tile> neighbors = new List<Tile>();
			if(!this.IsLeftRow(tileindex))
			{
				if(validindex(this.CubeToIndex(tilehex.Left)))
				{
					neighbors.Add(this.MyTiles[this.CubeToIndex(tilehex.Left)]);
				}
				if(!this.IsTopRow(tileindex))
				{
					if(validindex(this.CubeToIndex(tilehex.UpLeft)))
					{
						neighbors.Add(this.MyTiles[this.CubeToIndex(tilehex.UpLeft)]);
					}
				}
				if(!this.IsBottomRow(tileindex))
				{
					if(validindex(this.CubeToIndex(tilehex.DownLeft)))
					{
						neighbors.Add(this.MyTiles[this.CubeToIndex(tilehex.DownLeft)]);
					}
				}
			}
			if(!this.IsRightRow(tileindex))
			{
				
				if(validindex(this.CubeToIndex(tilehex.Right)))
				{
					neighbors.Add(this.MyTiles[this.CubeToIndex(tilehex.Right)]);
				}
				if(!this.IsTopRow(tileindex))
				{
					if(validindex(this.CubeToIndex(tilehex.UpRight)))
					{
						neighbors.Add(this.MyTiles[this.CubeToIndex(tilehex.UpRight)]);
					}
				}
				if(!this.IsBottomRow(tileindex))
				{
					if(validindex(this.CubeToIndex(tilehex.DownRight)))
					{
						neighbors.Add(this.MyTiles[this.CubeToIndex(tilehex.DownRight)]);
					}
				}
			}
			return neighbors;
		}
		
		public List<Tile> TileNeighbors(P_CubeCoords tilehex)
		{
			int tileindex = this.CubeToIndex(tilehex);
			List<Tile> neighbors = new List<Tile>();
			if(!this.IsLeftRow(tileindex))
			{
				if(validindex(this.CubeToIndex(tilehex.Left)))
				{
					neighbors.Add(this.MyTiles[this.CubeToIndex(tilehex.Left)]);
				}
				if(!this.IsTopRow(tileindex))
				{
					if(validindex(this.CubeToIndex(tilehex.UpLeft)))
					{
						neighbors.Add(this.MyTiles[this.CubeToIndex(tilehex.UpLeft)]);
					}
				}
				if(!this.IsBottomRow(tileindex))
				{
					if(validindex(this.CubeToIndex(tilehex.DownLeft)))
					{
						neighbors.Add(this.MyTiles[this.CubeToIndex(tilehex.DownLeft)]);
					}
				}
			}
			if(!this.IsRightRow(tileindex))
			{
				
				if(validindex(this.CubeToIndex(tilehex.Right)))
				{
					neighbors.Add(this.MyTiles[this.CubeToIndex(tilehex.Right)]);
				}
				if(!this.IsTopRow(tileindex))
				{
					if(validindex(this.CubeToIndex(tilehex.UpRight)))
					{
						neighbors.Add(this.MyTiles[this.CubeToIndex(tilehex.UpRight)]);
					}
				}
				if(!this.IsBottomRow(tileindex))
				{
					if(validindex(this.CubeToIndex(tilehex.DownRight)))
					{
						neighbors.Add(this.MyTiles[this.CubeToIndex(tilehex.DownRight)]);
					}
				}
			}
			return neighbors;
		}
		
		public List<P_CubeCoords> CubeCoordNeighbors(P_CubeCoords tilehex)
		{
			int tileindex = this.CubeToIndex(tilehex);
			List<P_CubeCoords> neighbors = new List<P_CubeCoords>();
			if(!this.IsLeftRow(tileindex))
			{
				if(validindex(this.CubeToIndex(tilehex.Left)))
				{
					neighbors.Add(tilehex.Left);
				}
				if(!this.IsTopRow(tileindex))
				{
					if(validindex(this.CubeToIndex(tilehex.UpLeft)))
					{
						neighbors.Add(tilehex.UpLeft);
					}
				}
				if(!this.IsBottomRow(tileindex))
				{
					if(validindex(this.CubeToIndex(tilehex.DownLeft)))
					{
						neighbors.Add(tilehex.DownLeft);
					}
				}
			}
			if(!this.IsRightRow(tileindex))
			{
				
				if(validindex(this.CubeToIndex(tilehex.Right)))
				{
					neighbors.Add(tilehex.Right);
				}
				if(!this.IsTopRow(tileindex))
				{
					if(validindex(this.CubeToIndex(tilehex.UpRight)))
					{
						neighbors.Add(tilehex.UpRight);
					}
				}
				if(!this.IsBottomRow(tileindex))
				{
					if(validindex(this.CubeToIndex(tilehex.DownRight)))
					{
						neighbors.Add(tilehex.DownRight);
					}
				}
			}
			return neighbors;
		}
		
		public bool ValidDirection(int tileindex, TileNeighborEnum direction)
		{
			switch(direction)
			{
				case TileNeighborEnum.UpLeft:
					if(!validindex(CubeToIndex(IndexToCube(tileindex).UpLeft))){return false;}
					if(!this.IsLeftRow(this.NeighborIndex(tileindex,direction)) && this.IsLeftRow(tileindex)){return false;}
					return true;
				case TileNeighborEnum.UpRight:
					if(!validindex(CubeToIndex(IndexToCube(tileindex).UpRight))){return false;}
					if(!this.IsRightRow(this.NeighborIndex(tileindex, direction)) && this.IsRightRow(tileindex)){return false;}
					return true;
				case TileNeighborEnum.Left:
					if(!validindex(CubeToIndex(IndexToCube(tileindex).Left))){return false;}
					if(!this.IsLeftRow(this.NeighborIndex(tileindex,direction)) && this.IsLeftRow(tileindex)){return false;}
					return true;
				case TileNeighborEnum.Right:
					if(!validindex(CubeToIndex(IndexToCube(tileindex).Right))){return false;}
					if(!this.IsRightRow(this.NeighborIndex(tileindex, direction)) && this.IsRightRow(tileindex)){return false;}
					return true;
				case TileNeighborEnum.DownLeft:
					if(!validindex(CubeToIndex(IndexToCube(tileindex).DownLeft))){return false;}
					if(!this.IsLeftRow(this.NeighborIndex(tileindex,direction)) && this.IsLeftRow(tileindex)){return false;}
					return true;
				case TileNeighborEnum.DownRight:
					if(!validindex(CubeToIndex(IndexToCube(tileindex).DownRight))){return false;}
					if(!this.IsRightRow(this.NeighborIndex(tileindex, direction)) && this.IsRightRow(tileindex)){return false;}
					return true;
			}
			return false;
		}
		
		public int NeighborIndex(int tileindex, TileNeighborEnum direction)
		{
			switch(direction)
			{
				case TileNeighborEnum.UpLeft:
					if(validindex(CubeToIndex(IndexToCube(tileindex).UpLeft))){return CubeToIndex(IndexToCube(tileindex).UpLeft);}
					throw new Exception("Tried to attain index of nonexistant tile!");
				case TileNeighborEnum.UpRight:
					if(validindex(CubeToIndex(IndexToCube(tileindex).UpRight))){return CubeToIndex(IndexToCube(tileindex).UpRight);}
					throw new Exception("Tried to attain index of nonexistant tile!");
				case TileNeighborEnum.Left:
					if(validindex(CubeToIndex(IndexToCube(tileindex).Left))){return CubeToIndex(IndexToCube(tileindex).Left);}
					throw new Exception("Tried to attain index of nonexistant tile!");
				case TileNeighborEnum.Right:
					if(validindex(CubeToIndex(IndexToCube(tileindex).Right))){return CubeToIndex(IndexToCube(tileindex).Right);}
					throw new Exception("Tried to attain index of nonexistant tile!");
				case TileNeighborEnum.DownLeft:
					if(validindex(CubeToIndex(IndexToCube(tileindex).DownLeft))){return CubeToIndex(IndexToCube(tileindex).DownLeft);}
					throw new Exception("Tried to attain index of nonexistant tile!");
				case TileNeighborEnum.DownRight:
					if(validindex(CubeToIndex(IndexToCube(tileindex).DownRight))){return CubeToIndex(IndexToCube(tileindex).DownRight);}
					throw new Exception("Tried to attain index of nonexistant tile!");
			}
			throw new Exception("EAT A DICK");
		}
		public bool validindex(int index)
		{
			if(index < 0)
			{
				return false;
			}
			if(index>=this.MyTiles.Length)
			{
				return false;
			}
			return true;
		}
		
		public P_TileArray DeepCopy()
		{
			P_TileArray ReturnValue = new P_TileArray(_Width, _MyTiles.Length/_Width);
			
			for(int i=0;i<_MyTiles.Length;i++)
			{
				ReturnValue._MyTiles[i] = MyTiles[i].DeepCopy();
			}
			
			return ReturnValue;
		}
		
		public int Width
		{
			get
			{
				return _Width;
			}
			set
			{
				
			}
		}
		
		public bool IsVisable(P_CubeCoords HexA, P_CubeCoords HexB)
		{
			List<P_CubeCoords> coords = P_CubeCoords.cube_linedraw(HexA, HexB);
			
			for(int i=0; i<coords.Count; i++)
			{
				if(_MyTiles[CubeToIndex(coords[i])].Role == TileRole.Void){return false;}
			}
			
			return true;
		}
		
		/* 
		 * 
		 *         GRAPHICS
		 * 
		 */	

		public TilerIsADummy.PrototypeMapGen.PrototypeComponents.IP_RenderComponent Render{get{return _Render;}set{_Render = value;}}
		private TilerIsADummy.PrototypeMapGen.PrototypeComponents.IP_RenderComponent _Render;
		public void AcceptP_Render (PrototypeMapGen.P_Render P_Render)
		{
			P_Render.P_RenderSubscribe(this._Render, this._Render.RenderLayer);
		}
		
		public void DismissP_Render (PrototypeMapGen.P_Render P_Render)
		{
			P_Render.P_RenderUnsubscribe(this._Render, this._Render.RenderLayer);
		}
	}
}
