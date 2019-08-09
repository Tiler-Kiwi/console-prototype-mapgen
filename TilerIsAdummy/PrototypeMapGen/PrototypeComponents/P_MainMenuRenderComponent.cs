/*
 * Created by SharpDevelop.
 * User: adam.moseman
 * Date: 6/23/2017
 * Time: 3:28 AM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;

namespace TilerIsADummy.PrototypeMapGen.PrototypeComponents
{
	/// <summary>
	/// Description of P_MainMenuRenderComponent.
	/// </summary>
	public class P_MainMenuRenderComponent : IP_RenderComponent
	{
		public P_MainMenuRenderComponent(P_MainMenu mainmenu)
		{
		}
		
		private IP_Renderable _OffsetBoss;
		private int _RelXCoords;
		private int _MyXCoords = 0;
		private int _RelYCoords;
		private int _MyYCoords = 0;
		private RenderLayerEnum _RenderLayer = RenderLayerEnum.Menu;
		private int _LayerPriority = 50;
		private bool _Visable = true;
		private ConsoleColor _BackgroundColor = P_Const.DefaultBColor;
		private ConsoleColor _ForegroundColor = P_Const.DefaultFColor;
		private int _SizeX = P_Const.CONSOLE_WIDTH;
		private int _SizeY = P_Const.CONSOLE_HEIGHT;
		private char[] _Graphic;
		
		public IP_Renderable OffsetBoss
		{
			get
			{
				return _OffsetBoss;
			}
			set
			{
				bool checking = true;
				IP_Renderable checkingthis = value;
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
		
		public RenderLayerEnum RenderLayer
		{
			get
			{
				return _RenderLayer;
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
				if(Graphic == null)
				{
					return false;
				}
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
				return _BackgroundColor;
			}
			set
			{
				_BackgroundColor = value;
			}
		}
		
		public ConsoleColor ForegroundColor
		{
			get
			{
				return _ForegroundColor;
			}
			set
			{
				_ForegroundColor = value;
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
				return _Graphic;
			}
			set
			{
				_Graphic = value;
			}
		}
	}
}
