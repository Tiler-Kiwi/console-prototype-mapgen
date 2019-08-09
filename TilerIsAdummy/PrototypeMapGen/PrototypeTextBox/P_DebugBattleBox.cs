/*
 * Created by SharpDevelop.
 * User: adam.moseman
 * Date: 6/24/2017
 * Time: 5:52 AM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;

namespace TilerIsADummy.PrototypeMapGen.PrototypeTextBox
{
	/// <summary>
	/// Description of P_DebugBattleBox.
	/// </summary>
	public class P_DebugBattleBox : IP_Update, IP_Renderable
	{
		PrototypeBattleLogic.P_BattleMap _BattleMap;
		Tile TileImViewing;
		
		public P_DebugBattleBox(PrototypeBattleLogic.P_BattleMap battlemap)
		{
			this.Active = true;
			_BattleMap = battlemap;
			_Render = new PrototypeComponents.P_RenderComponent();
			_Render.LayerPriority = 60;
			_Render.RenderLayer = RenderLayerEnum.Menu;
			_Render.BackgroundColor = ConsoleColor.White;
			_Render.ForegroundColor = ConsoleColor.Black;
			_Render.SizeX = 30;
			_Render.SizeY = 8;
			_Render.Graphic = new char[_Render.SizeX * _Render.SizeY];
			foreach(char i in _Render.Graphic)
			{
				Render.Graphic[i]=' ';
			}
			_Render.MyXCoords = P_Const.CONSOLE_WIDTH - _Render.SizeX;
			_Render.MyYCoords = P_Const.CONSOLE_HEIGHT - _Render.SizeY;
			_Render.Visable = true;
		}
		
		public void Update(MyInput input)
		{
			if(_BattleMap.MyState.GetType() == typeof(PrototypeBattleLogic.P_BMSPlayerSelectingTile))
			{
				PrototypeBattleLogic.P_BMSPlayerSelectingTile mapstate =
					_BattleMap.MyState as PrototypeBattleLogic.P_BMSPlayerSelectingTile;
				Tile selectedtile = mapstate._Cursor.Owner as Tile;
				
				if(selectedtile != TileImViewing)
				{
					TileImViewing = selectedtile;
					string one ="", two ="", thr="", fou="", fiv="", six="", sev="", eig="";
					
					one = "Tile: " + selectedtile.TileName;
					two = "Role: " + selectedtile.Role.ToString();
					if(selectedtile.TileFeatureList.Count > 0)
					{
						thr = "Feature: " + selectedtile.TileFeatureList[0].Name;
					}
					fou = "Passable: " + selectedtile.IsTilePassable().ToString();
					fiv = "SeeThru: " + selectedtile.IsTileSeeThru().ToString();
					if(selectedtile.EntityList.Count > 0)
					{
						six = "Entity: " + selectedtile.EntityList[0].Name + "/" + selectedtile.EntityList[0].GetType().Name;
						sev = "Faction: " + selectedtile.EntityList[0].Faction.ToString();
						if(selectedtile.EntityList[0].GetType() == typeof(EntityActor))
						   {
						   	EntityActor dude = selectedtile.EntityList[0] as EntityActor;
						   	eig = "Movement: " + dude.MovementPoints + "/" + dude.MovementType + "/" + dude.TurnStatus;
						   }
						}
					
					
					one = PadString(one, _Render.SizeX);
					two = PadString(two, _Render.SizeX);
					thr = PadString(thr, _Render.SizeX);
					fou = PadString(fou, _Render.SizeX);
					fiv = PadString(fiv, _Render.SizeX);
					six = PadString(six, _Render.SizeX);
					sev = PadString(sev, _Render.SizeX);
					eig = PadString(eig, _Render.SizeX);
					char[] textblob = new char[_Render.SizeX*_Render.SizeY];
					
					string bigstring = one + two + thr + fou + fiv + six + sev + eig;
					bigstring.ToCharArray().CopyTo(textblob, 0);
					
					_Render.Graphic=textblob;
				}
			}
		}
		
		private string PadString(string words, int length)
		{
			return words.PadRight(length, ' ');
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
