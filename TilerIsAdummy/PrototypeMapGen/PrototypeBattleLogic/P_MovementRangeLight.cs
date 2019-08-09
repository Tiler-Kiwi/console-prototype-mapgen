/*
 * Created by SharpDevelop.
 * User: adam.moseman
 * Date: 6/25/2017
 * Time: 2:41 AM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Collections.Generic;

namespace TilerIsADummy.PrototypeMapGen.PrototypeBattleLogic
{
	/// <summary>
	/// Description of P_MovementRangeLight.
	/// </summary>
	public class P_MovementRangeLight : IP_Renderable, IP_Update
	{
		private List<List<int>> _Rings;
		private List<int> _MovementRange;
		private P_BattleMap _BattleMap;
		private double _OldTime;
		private double _Lag;
		private int _State;
		private IEntityActor _Actor;
		private int _Value;
		
		public P_MovementRangeLight(P_BattleMap battlemap, List<int> movementrange, IEntityActor actor)
		{
			_OldTime = (float)DateTime.Now.TimeOfDay.TotalMilliseconds;
			_Lag = 250;
			_Actor = actor;
			_Value = -1;
			
			_BattleMap = battlemap;
			_MovementRange = new List<int>(movementrange);
			_State = 1;
			
			_Rings = LightDist(battlemap, _MovementRange, actor);
			
			_Render = new PrototypeComponents.P_RenderComponent();
			_Render.SizeX = _BattleMap.TheMap.Width * P_Const.TILE_WIDTH + 1;
			_Render.SizeY = _BattleMap.TheMap.Length / _BattleMap.TheMap.Width;
			_Render.OffsetBoss = _BattleMap;
			_Render.RenderLayer = RenderLayerEnum.Effects;
			_Render.LayerPriority = 51;
			_Render.BackgroundColor = ConsoleColor.Cyan;
			_Render.Visable = true;
			_Render.Graphic = new char[_Render.SizeX*_Render.SizeY];
			for(int i=0;i<_Render.Graphic.Length;i++)
			{
				_Render.Graphic[i] = P_Const.NULL_CHAR;
			}
			Active = true;
		}
		
		public void Update(PrototypeMapGen.MyInput input)
		{
			double current = (float)DateTime.Now.TimeOfDay.TotalMilliseconds;
			int priorState = _State;
			_Lag = _Lag + (current - _OldTime);
			_OldTime = current;
			while(_Lag > 200)
			{
				if(_State == _Rings.Count-1 || _State == 1)
				{
					_Value = _Value*-1;
				}
				_State+=_Value;
				_Lag -= 200;
			}
			
			if(priorState != _State)
			{
			_Render.Graphic = IndexToTileGraphics(_BattleMap, _Rings[_State], this);
			}
			
		}
		
		private static List<List<int>> LightDist(P_BattleMap battlemap, List<int> moverange, IEntityActor actor)
		{
			List<List<int>> ReturnValue = new List<List<int>>();
			for(int i=0; i<=actor.MovementPoints+1;i++)
			{
				List<int> Range = new List<int>();
				for(int j=0; j<moverange.Count; j++)
				{
					if(P_HexPath.GetPath(battlemap.TheMap, battlemap.TheMap.IndexToCube(actor.MyTileIndex), battlemap.TheMap.IndexToCube(moverange[j])).Count == i)
					{
						Range.Add(moverange[j]);
					}
				}
				for(int j=0;j<Range.Count; j++)
				{
					moverange.Remove(Range[j]);
				}
				ReturnValue.Add(Range);
			}
			for(int i=0;i<ReturnValue.Count;i++)
			{
				for(int j=0; j<ReturnValue[j].Count;j++)
				{
					moverange.Add(ReturnValue[i][j]);
				}
			}
			return ReturnValue;
		}
		private static List<List<int>> LightRings(P_BattleMap battlemap, List<int> moverange, IEntityActor actor)
		{
			List<List<int>> ReturnValue = new List<List<int>>();
			for(int i=0; i<=actor.MovementPoints;i++)
			{
				List<P_CubeCoords> foo = GetRing(battlemap.TheMap.IndexToCube(actor.MyTileIndex), i);
				List<int> bar = new List<int>();
				for(int j=0;j<foo.Count;j++)
				{
					int fuk = battlemap.TheMap.CubeToIndex(foo[j]);
					if(moverange.Contains(fuk))
					{
						bar.Add(fuk);
					}
				}
				ReturnValue.Add(bar);
			}
			return ReturnValue;
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
		
		private static char[] IndexToTileGraphics(P_BattleMap battlemap, List<int> ringindex, P_MovementRangeLight lightobj)
		{
			int SizeX = lightobj.Render.SizeX; // +1 on map width
			int SizeY = lightobj.Render.SizeY; // map height
			char[] ReturnValue = new char[SizeX*SizeY];
			for(int i=0;i<ReturnValue.Length;i++)
			{
				ReturnValue[i] = P_Const.NULL_CHAR;
			}
			
			int woffset = 0;
			for(int i = 0; i<ringindex.Count;i++)
			{
				int RingIndexX = ringindex[i]%battlemap.TheMap.Width; //THE TILE INDEX
				int RingIndexY = ringindex[i]/battlemap.TheMap.Width; //ALSO THE TILE INDEX
				
				RingIndexX = RingIndexX*2;
				
				//RingIndexX = RingIndexX + RingIndexY;
				if ((RingIndexY&1) == 0) {woffset = 0;}
				else {woffset = 1;}
				int asshole = RingIndexX+woffset+(RingIndexY*(battlemap.TheMap.Width*2 + 1));
				ReturnValue[asshole] = ' ';
				ReturnValue[asshole+1] = ' ';
			}
			/*
			for(int i=0; i<battlemap.TheMap.Length;i++)
			{
				if(i%2 == 0)
				{
					int fartX = i%battlemap.TheMap.Width;
					int fartY = i/battlemap.TheMap.Width;
					
					fartX = fartX*2;
					//fartX = fartX+fartY;
					
					if ((fartY%2) == 0) {woffset = 0;}
					else {woffset = 1;}
					int fart = (fartX+woffset)+((battlemap.TheMap.Width*2+1)*fartY);
					ReturnValue[fart] = 'X';
					ReturnValue[fart+1] = 'X';
				}
			}
			*/
			return ReturnValue;
		}
		
		public void AcceptUpdater(PrototypeMapGen.IP_UpdateManager updater){updater.Subscribe(this);}
		public void DismissUpdater(PrototypeMapGen.IP_UpdateManager updater){updater.Unsubscribe(this);}
		public bool Active{get;set;}
		
		public TilerIsADummy.PrototypeMapGen.PrototypeComponents.IP_RenderComponent Render{get{return _Render;}set{_Render = value;}}
		private TilerIsADummy.PrototypeMapGen.PrototypeComponents.IP_RenderComponent _Render;
		public void AcceptP_Render (PrototypeMapGen.P_Render P_Render){P_Render.P_RenderSubscribe(this._Render, this._Render.RenderLayer);}
		public void DismissP_Render (PrototypeMapGen.P_Render P_Render)
		{P_Render.P_RenderUnsubscribe(this._Render, this._Render.RenderLayer);}
	}
}
