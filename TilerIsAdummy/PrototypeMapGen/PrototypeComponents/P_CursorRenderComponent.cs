/*
 * Created by SharpDevelop.
 * User: adam.moseman
 * Date: 6/24/2017
 * Time: 1:31 AM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;

namespace TilerIsADummy.PrototypeMapGen.PrototypeComponents
{
	/// <summary>
	/// Description of P_CursorRenderComponent.
	/// </summary>
	public class P_CursorRenderComponent : IP_RenderComponent
	{
		public P_CursorRenderComponent(PrototypeBattleLogic.P_SelectionCursor cursor)
		{
			_MyCursor = cursor;
		}
		
		private PrototypeBattleLogic.P_SelectionCursor _MyCursor;
		private RenderLayerEnum _RenderLayer = RenderLayerEnum.Effects;
		private bool _Visable = true;
		private ConsoleColor _BackgroundColor = ConsoleColor.White;
		private ConsoleColor _ForegroundColor =P_Const.DefaultFColor;
		
		public IP_Renderable OffsetBoss
		{
			get
			{
				return _MyCursor.Owner;
			}
			set
			{

			}
		}
		
		public int RelXCoords
		{
			get
			{
				if(_MyCursor != null)
				{
					return _MyCursor.Owner.Render.RelXCoords + MyXCoords;
				}
				return MyXCoords;
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
				return 0;
			}
			set
			{
			}
		}
		
		public int RelYCoords
		{
			get
			{
				if(_MyCursor != null)
				{
					return _MyCursor.Owner.Render.RelYCoords + MyYCoords;
				}
				return MyYCoords;
			}
			set
			{
				
			}
		}
		
		public int MyYCoords
		{
			get
			{
				return 0;
			}
			set
			{
				
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
				return OffsetBoss.Render.LayerPriority;
			}
			set
			{

			}
		}
		
		public bool Visable
		{
			get
			{
				if(_Visable == true)
				{
					int time = DateTime.Now.Millisecond % 500;
					if(time > 200)
					{
						return true;
					}
				}
				return false;
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
				return OffsetBoss.Render.SizeX;
			}
			set
			{		}
		}
		
		public int SizeY
		{
			get
			{
				return OffsetBoss.Render.SizeY;
			}
			set
			{	}
		}
		
		public char[] Graphic
		{
			get
			{
				if(SizeX < 1 || SizeY < 1)
				{
					return null;
				}
				char[] ReturnValue = new char[SizeX*SizeY];
				for(int i = 0; i< ReturnValue.Length; i++)
				{
					ReturnValue[i] = ' ';
				}
				return ReturnValue;
			}
			set
			{

			}
		}
	}
}
