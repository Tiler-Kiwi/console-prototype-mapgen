/*
 * Created by SharpDevelop.
 * User: adam.moseman
 * Date: 6/23/2017
 * Time: 3:25 AM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;

namespace TilerIsADummy.PrototypeMapGen.PrototypeComponents
{
	/// <summary>
	/// Description of P_TileRenderComponent.
	/// </summary>
	public class P_TileRenderComponent : TilerIsADummy.PrototypeMapGen.PrototypeComponents.IP_RenderComponent
	{
		public P_TileRenderComponent(Tile tile)
		{
			_MyTile = tile;
		}
		private Tile _MyTile;
		private PrototypeMapGen.IP_Renderable _OffsetBoss;
		private int _RelXCoords;
		private int _MyXCoords = 0;
		private int _RelYCoords;
		private int _MyYCoords = 0;
		private static int _LayerPriority = 50;
		private bool _Visable = true;
		private static int _SizeX = 2;
		private static int _SizeY = 1;
		//private char[] _Graphic;
	
		public PrototypeMapGen.IP_Renderable OffsetBoss
		{ 
			get
			{
				return _OffsetBoss;
			}
			set
			{
				bool checking = true;
				PrototypeMapGen.IP_Renderable checkingthis = value;
				while (checking)
				{
					if(checkingthis != null)
					{
						if(checkingthis == this)
						{
							throw new AccessViolationException("Value's OffsetBoss heirarchy includes the would-be subordinate object!");
						}
						checkingthis = checkingthis.Render.OffsetBoss;
					}
					else{checking = false;}
				}
				_OffsetBoss = value;
			}
		}
		
		public int RelXCoords
		{ 
			get
			{
				if(_OffsetBoss != null)
				{
					return _OffsetBoss.Render.RelXCoords + _MyXCoords;
				}
				return _MyXCoords;
			}
			set
			{
				//Read only.
			}
		}
		
		public int MyXCoords
		{ 
			get
			{
				return _MyXCoords;
			}
			set
			{
				_MyXCoords = value;
			}
		}
		
		public int RelYCoords
		{ 
			get
			{
				if(_OffsetBoss != null)
				{
					return _OffsetBoss.Render.RelYCoords + _MyYCoords;
				}
				return _MyYCoords;
			}
			set
			{
				//read only!
			}
		}
		
		public int MyYCoords
		{ 
			get
			{
				return _MyYCoords;
			}
			set
			{
				_MyYCoords = value;
			}
		}
		
		public PrototypeMapGen.RenderLayerEnum RenderLayer
		{ 
			get
			{
				return PrototypeMapGen.RenderLayerEnum.Map;
			}
			set
			{
				//_RenderLayer = value;
				
				// It is probably a really bad idea to change this on the fly!
			}
		}
		
		public int LayerPriority
		{ 
			get
			{
				return _LayerPriority;
			}
			set
			{
				_LayerPriority = value;
			}
		}
		
		public bool Visable
		{ 
			get
			{
				return _Visable;
			}
			set
			{
				_Visable = value;
			}
		}
		
		public ConsoleColor BackgroundColor
		{ 
			get
			{
				ConsoleColor Feck;
				TileColor background = this._MyTile.TTileColor;
				if(_MyTile.TTileFeatureList.Count>0)
				{
					background = this._MyTile.TTileFeatureList[0].Color;
				}
					switch(background)
					{
						case TileColor.Black:
							Feck = ConsoleColor.Black;
							break;
						case TileColor.Blue:
							Feck = ConsoleColor.Blue;
							break;
						case TileColor.Brown:
							Feck = ConsoleColor.DarkRed;
							break;
						case TileColor.DarkGreen:
							Feck = ConsoleColor.DarkGreen;
							break;
						case TileColor.Gray:
							Feck = ConsoleColor.Gray;
							break;
						case TileColor.Green:
							Feck = ConsoleColor.Green;
							break;
						case TileColor.Orange:
							Feck = ConsoleColor.DarkYellow;
							break;
						case TileColor.Purple:
							Feck = ConsoleColor.Magenta;
							break;
						case TileColor.Red:
							Feck = ConsoleColor.Red;
							break;
						default:
							Feck = ConsoleColor.Black;
							break;
					}
					return Feck;
			}
			set
			{
				//_BackgroundColor = value;
			}
		}
		
		public ConsoleColor ForegroundColor
		{ 
			get
			{
				return PrototypeMapGen.P_Const.DefaultFColor;
			}
			set
			{
				//_ForegroundColor = value;
			}
		}
		
		public int SizeX
		{ 
			get
			{
				return _SizeX;
			}
			set
			{
				_SizeX = value;
			}
		}
		
		public int SizeY
		{ 
			get
			{
				return _SizeY;
			}
			set
			{
				_SizeY = value;
			}
		}
		
		public char[] Graphic
		{ 
			get
			{
				if(_SizeX == 0 || _SizeY == 0)
				{
					return null;
				}
				char[] Feck = new char[2];
				
				if(this._MyTile.Role==TileRole.Void)
				{
					Feck[0] = ' ';
					Feck[1] = ' ';
					return Feck;
				}
				
				Feck[0] = '.';
				if(_MyTile.TTileFeatureList.Count>0)
				{
					Feck[0] = _MyTile.TTileFeatureList[0].Graphic;
				}
				
				//Feck[1] = TileHeight.ToString()[0];
				Feck[1] = ' ';
				return Feck;
			}
			set
			{
				//_Graphic = value;
			}
		}
	}
}
