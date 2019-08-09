/*
 * Created by SharpDevelop.
 * User: adam.moseman
 * Date: 6/23/2017
 * Time: 1:40 AM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Collections.Generic;

namespace TilerIsADummy.PrototypeMapGen.PrototypeBattleLogic
{
	/// <summary>
	/// Description of P_BattleMapStateCollection.
	/// </summary>
	/// 
	
	public static class P_BMS
	{
		public static TileNeighborEnum InputToDirection(MyInput input)
		{
			switch(input)
			{
				case MyInput.DownLeft:
					return TileNeighborEnum.DownLeft;
				case MyInput.DownRight:
					return TileNeighborEnum.DownRight;
				case MyInput.Left:
					return TileNeighborEnum.Left;
				case MyInput.Right:
					return TileNeighborEnum.Right;
				case MyInput.UpLeft:
					return TileNeighborEnum.UpLeft;
				case MyInput.UpRight:
					return TileNeighborEnum.UpRight;
				default:
					throw new Exception("Input is not a direction!!!");
			}
			throw new Exception("Input is not a direction!!!");
		}
		
		public static void MoveTileCursor(P_BattleMap battlemap, TileNeighborEnum direction, P_SelectionCursor cursor, ref int cursorindex)
		{
			if(battlemap.TheMap.ValidDirection(cursorindex,direction))
			{
				cursorindex = battlemap.TheMap.NeighborIndex(cursorindex, direction);
				cursor.Owner = battlemap.TheMap.MyTiles[cursorindex];
			}
		}
		
		public static void CursorOffScreen(P_BattleMap battlemap, P_SelectionCursor cursor)
		{
			if(cursor.Render.RelXCoords > P_Const.CONSOLE_WIDTH)
			{
				battlemap.Render.MyXCoords = 0 - P_Const.CONSOLE_WIDTH/2 - (cursor.Render.RelXCoords - battlemap.Render.MyXCoords);
			}
			if(cursor.Render.RelXCoords < 0)
			{
				battlemap.Render.MyXCoords = P_Const.CONSOLE_WIDTH/2 - (cursor.Render.RelXCoords - battlemap.Render.MyXCoords);
			}
			if(cursor.Render.RelYCoords > P_Const.CONSOLE_HEIGHT)
			{
				battlemap.Render.MyYCoords = 0 - P_Const.CONSOLE_HEIGHT/2 - (cursor.Render.RelYCoords - battlemap.Render.MyYCoords);
			}
			if(cursor.Render.RelYCoords < 0)
			{
				battlemap.Render.MyYCoords = P_Const.CONSOLE_HEIGHT/2 - (cursor.Render.RelYCoords - battlemap.Render.MyYCoords);
			}
		}
	}
	
	public class P_BMSPlayerSelectingTile : IP_BattleMapState
	{
		public P_SelectionCursor _Cursor;
		public int CursorIndex;
		public PrototypeTextBox.P_DebugBattleBox DebugBox;
		public P_BMSPlayerSelectingTile(P_BattleMap map)
		{
			map.MyState = this;
			Random rng = new Random();
			DebugBox = new TilerIsADummy.PrototypeMapGen.PrototypeTextBox.P_DebugBattleBox(map);
			DebugBox.AcceptP_Render(map.Renderer);
			DebugBox.AcceptUpdater(map.Updater);
			while(true)
			{
				int num = rng.Next(0, map.MapEntity.Count);
				if(map.MapEntity[num].GetType() == typeof(EntityActor))
				{
					EntityActor check = map.MapEntity[num] as EntityActor;
					if(check.TurnStatus == EntityCharacterTurnStatus.MyTurn && check.MyTileIndex > 0)
					{
						_Cursor = new P_SelectionCursor(check.MyLocation);
						_Cursor.AcceptP_Render(map.Renderer);
						CursorIndex = check.MyTileIndex;
						break;
					}
				}
			}
		}
		
		public IP_BattleMapState Update(P_BattleMap battlemap, MyInput input)
		{
			switch(input)
			{
				case MyInput.Confirm:
					Tile selectedtile = _Cursor.Owner as Tile;
					if(selectedtile == null)
					{
						break;
					}
					if(selectedtile.EntityList.Count > 0)
					{
						if(selectedtile.EntityList[0].GetType() == typeof(EntityActor))
						{
							EntityActor selectedactor = selectedtile.EntityList[0] as EntityActor;
							if(selectedactor.TurnStatus == EntityCharacterTurnStatus.MyTurn)
							{
							this.Exit(battlemap);
							return new P_BMSChose_Movement(battlemap, selectedactor);
							}
						}
					}
					break;
				case MyInput.Escape:
					battlemap.GenerateNewMap();
					this.Exit(battlemap);
					return this;
				case MyInput.DownLeft:
				case MyInput.DownRight:
				case MyInput.Left:
				case MyInput.Right:
				case MyInput.UpLeft:
				case MyInput.UpRight:
					P_BMS.MoveTileCursor(battlemap, P_BMS.InputToDirection(input), _Cursor, ref CursorIndex);
					break;
				default:
					break;	
			}
			P_BMS.CursorOffScreen(battlemap, _Cursor);
			return this;
		}
		
		private void Exit(P_BattleMap battlemap)
		{
			DebugBox.DismissP_Render(battlemap.Renderer);
			DebugBox.DismissUpdater(battlemap.Updater);
			DebugBox = null;
			_Cursor.DismissP_Render(battlemap.Renderer);
			_Cursor = null;
		}
		
		private void TeleportCursor(P_BattleMap battlemap, int tileindex)
		{
			if(battlemap.TheMap.validindex(tileindex))
			{
				CursorIndex = tileindex;
				_Cursor.Owner = battlemap.TheMap.MyTiles[CursorIndex];
			}
		}
	}
	
	public class P_BMSChose_Nonfaction_Actor : IP_BattleMapState
	{
		public P_BMSChose_Nonfaction_Actor(P_BattleMap map)
		{
			map.MyState = this;
		}
		
		public IP_BattleMapState Update(P_BattleMap battlemap, MyInput input)
		{
			throw new NotImplementedException();
		}
		
	}
	
	public class P_BMSChose_Faction_Actor : IP_BattleMapState
	{
		public P_BMSChose_Faction_Actor(P_BattleMap map, IEntityActor actor)
		{
			map.MyState = this;
		}
		
		public IP_BattleMapState Update(P_BattleMap battlemap, MyInput input)
		{
			throw new NotImplementedException();
		}
		
	}
	
	public class P_BMSChose_Info : IP_BattleMapState
	{
		public P_BMSChose_Info(P_BattleMap map)
		{
			map.MyState = this;
		}
		
		public IP_BattleMapState Update(P_BattleMap battlemap, MyInput input)
		{
			throw new NotImplementedException();
		}
		
	}
	
	public class P_BMSChose_Movement : IP_BattleMapState
	{
		private IEntityActor _Actor;
		private P_SelectionCursor _Cursor;
		private int _CursorIndex;
		private List<int> _ValidMoveIndex;
		private P_MovementRangeLight _MoveLights;
		
		public P_BMSChose_Movement(P_BattleMap map, IEntityActor actor)
		{
			_Actor=actor;
			_Cursor = new P_SelectionCursor(actor.MyLocation);
			_Cursor.AcceptP_Render(map.Renderer);
			_CursorIndex = actor.MyTileIndex;
			_ValidMoveIndex = MovementRange(map, actor);
			
			_MoveLights = new P_MovementRangeLight(map, _ValidMoveIndex, actor);
			_MoveLights.AcceptP_Render(map.Renderer);
			_MoveLights.AcceptUpdater(map.Updater);
			
			map.MyState = this;
		}
		
		public IP_BattleMapState Update(P_BattleMap battlemap, MyInput input)
		{
			switch(input)
			{
				case MyInput.Confirm:
					{
						if(IsMovementValid(battlemap, _ValidMoveIndex, _CursorIndex))
						{
							battlemap.MoveActor(_Actor, _CursorIndex);
							_Actor.TurnStatus = EntityCharacterTurnStatus.Moved;
							if(_Actor.MyLocation.TileFeatureList.Count>0)
							{
								if(_Actor.MyLocation.TileFeatureList[0].Name == "Objective")
								{
									this.Exit(battlemap);
									return new P_BMSMapEnd(battlemap);
								}
							}
							if(battlemap.IsFactionTurnOver())
							{
								battlemap.SetFactionActorsToNotMyTurn();
								battlemap.RotateFactionTurn();
								battlemap.GiveFactionActorsATurn();
								this.Exit(battlemap);
								return new P_BMSPlayerSelectingTile(battlemap);
							}
							this.Exit(battlemap);
							return new P_BMSPlayerSelectingTile(battlemap);
							
						}
						break;
					}
				case MyInput.DownLeft:
				case MyInput.DownRight:
				case MyInput.Left:
				case MyInput.Right:
				case MyInput.UpLeft:
				case MyInput.UpRight:
					P_BMS.MoveTileCursor(battlemap, P_BMS.InputToDirection(input), _Cursor, ref _CursorIndex);
					break;
				case MyInput.Deny:
					this.Exit(battlemap);
					return new P_BMSPlayerSelectingTile(battlemap);
					
			}
			P_BMS.CursorOffScreen(battlemap, _Cursor);
			return this;
		}
		
		private void Exit(P_BattleMap battlemap)
		{
			_MoveLights.DismissP_Render(battlemap.Renderer);
			_MoveLights.DismissUpdater(battlemap.Updater);
			_MoveLights = null;
			
			_Cursor.DismissP_Render(battlemap.Renderer);
			_Cursor = null;
		}
		
		private bool IsMovementValid(P_BattleMap battlemap, List<int> indexlist, int cursorindex)
		{
			if(indexlist.Contains(cursorindex))
			{
				return true;
			}
			return false;
		}
		
		private List<int> MovementRange(P_BattleMap battlemap, IEntityActor actor)
		{
			List<int> ReturnValue = new List<int>();
			List<int> Check = new List<int>();
			Check.Add(actor.MyTileIndex);
			while(Check.Count>0)
			{
				List<int> NeighborsIndex = battlemap.TheMap.TileNeighborsIndex(Check[0]);
				for(int i=0; i< NeighborsIndex.Count; i++)
				{
					if(Check.Contains(NeighborsIndex[i]) || ReturnValue.Contains(NeighborsIndex[i]))
					{
						continue;
					}
					if(battlemap.TheMap.MyTiles[NeighborsIndex[i]].IsTilePassable() == false)
					{
						continue;
					}
					if(battlemap.TheMap.MyTiles[NeighborsIndex[i]].EntityList.Count > 0)
					{
						continue;
					}
					List<P_CubeCoords> path = P_HexPath.GetPath(battlemap.TheMap, battlemap.TheMap.IndexToCube(NeighborsIndex[i]), battlemap.TheMap.IndexToCube(actor.MyTileIndex));
					if(path.Count > actor.MovementPoints+1)
					{
						continue;
					}
					
					Check.Add(NeighborsIndex[i]);
				}
				ReturnValue.Add(Check[0]);
				Check.RemoveAt(0);
			}
			return ReturnValue;
		}
		
	}
	
	public class P_BMSChose_EmptyTile : IP_BattleMapState
	{
		public P_BMSChose_EmptyTile(P_BattleMap map)
		{
			map.MyState = this;
		}
		
		public IP_BattleMapState Update(P_BattleMap battlemap, MyInput input)
		{
			throw new NotImplementedException();
		}
		
	}
	
	public class P_BMSConfirming_Move : IP_BattleMapState
	{
		public P_BMSConfirming_Move(P_BattleMap map)
		{
			map.MyState = this;
		}
		
		public IP_BattleMapState Update(P_BattleMap battlemap, MyInput input)
		{
			throw new NotImplementedException();
		}
		
	}
	
	public class P_BMSSetUp : IP_BattleMapState
	{
		public P_BMSSetUp(P_BattleMap map)
		{
			map.MyState = this;
		}
		
		public IP_BattleMapState Update(P_BattleMap battlemap, MyInput input)
		{
			throw new NotImplementedException();
		}
		
	}
	
	public class P_BMSMapEnd : IP_BattleMapState
	{
		public P_BMSMapEnd(P_BattleMap map)
		{
			map.MyState = this;
		}
		
		public IP_BattleMapState Update(P_BattleMap battlemap, MyInput input)
		{
			Random rng = new Random();
			int swoosh = rng.Next(1,11);
			ConsoleColor foo = (ConsoleColor)rng.Next(0,Enum.GetNames(typeof(ConsoleColor)).Length-1);
			ConsoleColor bar = (ConsoleColor)rng.Next(0,Enum.GetNames(typeof(ConsoleColor)).Length-1);
			ConsoleColor HECKb = (ConsoleColor)rng.Next(0,Enum.GetNames(typeof(ConsoleColor)).Length-1);
			ConsoleColor HECKf = (ConsoleColor)rng.Next(0,Enum.GetNames(typeof(ConsoleColor)).Length-1);
			Console.BackgroundColor= foo;
			Console.ForegroundColor= bar;
			Console.Clear();
			Console.WriteLine("Game Over.");
			switch(swoosh)
			{
				case 1:
					Console.WriteLine("You're winner!!!!!!");
					break;
				case 2:
					Console.WriteLine("Congrats on not breaking shit");
					break;
				case 3:
					Console.WriteLine("This took an actual month to make, FUCK.");
					break;
				case 4:
					Console.WriteLine("You caught AIDS and died. The End.");
					break;
				case 5:
					Console.WriteLine("You probably had better things to do than this.");
					break;
				case 6:
					Console.WriteLine("heck.");
					break;
				case 7:
					Console.WriteLine("There's 10 of these messages, collect them all!");
					break;
				case 8:
					Console.WriteLine("Congrats on winning! A random file on your computer was just deleted in celebration.");
					break;
				case 9:
					Console.WriteLine("your the best");
					break;
				case 10:
					Console.WriteLine("i'd put a big furry ascii butt here to annoy you but that would take a lot of effort");
					break;
			}
			P_Globals.TimesPlayed++;
			switch(P_Globals.TimesPlayed)
			{
				case 4:
					Console.WriteLine("Stop.");
					break;
				case 5:
					Console.WriteLine("Piss off.");
					break;
				case 6:
					Console.WriteLine("Nothing cool here.");
					break;
				case 7:
					Console.WriteLine("I'm impressed nothing's broke yet.");
					break;
				case 8:
					Console.WriteLine("Because if it did, you'll have to sit through these again...");
					break;
				case 9:
					Console.WriteLine("Assuming you're playing now, just to see what I write next.");
					break;
				case 10:
					Console.WriteLine("If you're that eager to read what I write, you know, you could just @ me on Discord...");
					break;
				case 11:
					Console.WriteLine("Last unique message. It goes back to just reciting a number, now.");
					break;
				default:
					Console.WriteLine("You've played this {0} times.", P_Globals.TimesPlayed);
					break;
			}
			Console.ReadKey(true);
			P_MainMenu what = new P_MainMenu(battlemap.Renderer, battlemap.Updater);
			what.AcceptUpdater(battlemap.Updater);
			what.AcceptP_Render(battlemap.Renderer);
			for(int i=0;i<666;i++)
			{
				System.Threading.Thread.Sleep(3);
				if(i%3==0)
				{
					if(i%5==0)
					{
						Console.Write("\n".PadRight(rng.Next(0,P_Const.CONSOLE_WIDTH-4)));
						Console.BackgroundColor=HECKb;
						Console.ForegroundColor = HECKf;
						Console.Write("HECK");
						Console.BackgroundColor = foo;
						Console.ForegroundColor = bar;
						continue;
					}
					Console.WriteLine("bark".PadLeft(rng.Next(0,P_Const.CONSOLE_WIDTH)));
					continue;
					
				}
				if(i%5==0)
				{
					Console.WriteLine("bork".PadLeft(rng.Next(0,P_Const.CONSOLE_WIDTH)));
					continue;
				}
				Console.WriteLine(+i);
			}
			Console.BackgroundColor=P_Const.DefaultBColor;
			Console.ForegroundColor=P_Const.DefaultFColor;
			Console.Clear();
			what.Start();
			battlemap.Kill();
			return null;
		}
		
	}
	/*
	public class P_BMSPaused : IP_BattleMapState
	{
		public P_BMSPaused()
		{
			
		}
		
		public IP_BattleMapState Update(P_BattleMap battlemap, MyInput input)
		{
			throw new NotImplementedException();
		}
		
		public void Enter(P_BattleMap battlemap)
		{
			throw new NotImplementedException();
		}
	}
	 */
}
