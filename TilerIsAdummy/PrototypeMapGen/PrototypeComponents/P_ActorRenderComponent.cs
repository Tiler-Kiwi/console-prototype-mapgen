/*
 * Created by SharpDevelop.
 * User: adam.moseman
 * Date: 6/23/2017
 * Time: 3:09 AM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;

namespace TilerIsADummy.PrototypeMapGen.PrototypeComponents
{
	/// <summary>
	/// Description of P_ActorRenderComponent.
	/// </summary>
	public class P_ActorRenderComponent : IP_RenderComponent
	{
		public P_ActorRenderComponent(EntityActor actor)
		{
			_Graphic = new char[1];
			_Graphic[0] = '@';
			_MyActor = actor;
		}
		
		private EntityActor _MyActor;

		private int _MyXCoords = 1;
		private int _MyYCoords = 0;
		private PrototypeMapGen.RenderLayerEnum _RenderLayer = PrototypeMapGen.RenderLayerEnum.Sprite;
		private int _LayerPriority = 50;
		private bool _Visable = true;
		private ConsoleColor _BackgroundColor = PrototypeMapGen.P_Const.DefaultBColor;
		private ConsoleColor _ForegroundColor = PrototypeMapGen.P_Const.DefaultFColor;
		private int _SizeX = 1;
		private int _SizeY = 1;
		private char[] _Graphic;

		public PrototypeMapGen.IP_Renderable OffsetBoss
		{
			get
			{
				return _MyActor.MyLocation;
			}
			set
			{	}
		}

		public int RelXCoords {
			get {
				if (OffsetBoss != null) {
					return OffsetBoss.Render.RelXCoords + _MyXCoords;
				}
				return _MyXCoords;
			}
			//Read only.
			set { }
		}

		public int MyXCoords {
			get { return _MyXCoords;  }
			set { }
		}

		public int RelYCoords {
			get {
				if (OffsetBoss != null) {
					return OffsetBoss.Render.RelYCoords + _MyYCoords;
				}
				return _MyYCoords;
			}
			//read only!
			set { }
		}

		public int MyYCoords {
			get { return _MyYCoords; }
			set {  }
		}

		public PrototypeMapGen.RenderLayerEnum RenderLayer {
			get { return _RenderLayer; }
			//_RenderLayer = value;

			// It is probably a really bad idea to change this on the fly!
			set { }
		}

		public int LayerPriority {
			get { return _LayerPriority; }
			set { _LayerPriority = value; }
		}

		public bool Visable {
			get {
				if (Graphic == null) {
					return false;
				}
				return _Visable;
			}
			set { _Visable = value; }
		}

		public ConsoleColor BackgroundColor {
			get 
			{
				if(_MyActor.MyLocation != null)
				{
					return _MyActor.MyLocation.Render.BackgroundColor;
				}
				return _BackgroundColor; 
			}
			set { }
		}

		public ConsoleColor ForegroundColor {
			get 
			{
				if(_MyActor.TurnStatus == EntityCharacterTurnStatus.MyTurn)
				{
					int time = DateTime.Now.Millisecond % 1000;
					if(time > 500)
					{
						return ConsoleColor.Black;
					}
				}
				return _ForegroundColor;
			}
			set { _ForegroundColor = value; }
		}

		public int SizeX {
			get { return _SizeX; }
			set { _SizeX = value; }
		}

		public int SizeY {
			get { return _SizeY; }
			set { _SizeY = value; }
		}

		public char[] Graphic {
			get {
				if (_SizeX == 0 || _SizeY == 0) {
					return null;
				}
				return _Graphic;
			}
			set { _Graphic = value; }
		}
	}
}
