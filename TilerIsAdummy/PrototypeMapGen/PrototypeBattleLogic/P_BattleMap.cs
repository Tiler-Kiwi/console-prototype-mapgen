/*
 * Created by SharpDevelop.
 * User: adam.moseman
 * Date: 6/3/2017
 * Time: 1:19 AM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Collections.Generic;

namespace TilerIsADummy.PrototypeMapGen.PrototypeBattleLogic
{
	/// <summary>
	/// Prototype battle map data.
	/// 
	/// Contains a map (tile array), and an entity list, and the methods needed for their interaction.
	/// 
	/// Shouldn't have to render anything.
	/// </summary>
	///
	
	public class P_BattleMap : IP_Renderable, IP_Update
	{
		
		public P_TileArray TheMap;
		public List<IEntity> MapEntity;
		public IP_UpdateManager Updater;
		public P_Render Renderer;
		public EntityFaction ActiveFaction;
		
		public IP_BattleMapState MyState;
		
		public bool Active{get;set;}
		public void AcceptUpdater(IP_UpdateManager updater)
		{
			updater.Subscribe(this);
		}
		public void DismissUpdater(IP_UpdateManager updater)
		{
			updater.Unsubscribe(this);
		}

		public P_BattleMap(List<TilerIsADummy.IEntity> entitylist, P_TileArray tilearray, IP_UpdateManager updater, P_Render renderer)
		{
			TheMap = tilearray;
			MapEntity = entitylist;
			
			Updater = updater;
			Renderer = renderer;
			_Render = new PrototypeComponents.P_RenderComponent();
			
			this.AcceptP_Render(Renderer);
			this.AcceptUpdater(Updater);
			Active = true;
			this.ActiveFaction = EntityFaction.Player;

			for(int i = 0; i<entitylist.Count;i++)
			{
				entitylist[i].MyMap=TheMap;
				entitylist[i].AcceptP_Render(renderer);
				entitylist[i].AcceptUpdater(updater);
			}
			
			foreach(EntityActor thing in entitylist)
			{
				thing.ActiveState = EntityCharacterState.Active;
			}
			
			for(int i=0; i<tilearray.MyTiles.Length;i++)
			{
				tilearray.MyTiles[i].AcceptP_Render(renderer);
				tilearray.MyTiles[i].Render.OffsetBoss = this;
			}
			
			PlaceActorsOnMap(entitylist);
			GiveFactionActorsATurn();
			
			MyState = new P_BMSPlayerSelectingTile(this);
			//tilearray.AcceptUpdater(updater);
		}
		
		public void Update(MyInput input)
		{
			MyState.Update(this, input);
		}
		

		private void PlaceActorsOnMap(List<IEntity> entitylist)
		{
			Random rnd = new Random();
			for(int i=0; i < entitylist.Count; i++)
			{
				var thing = entitylist[i].GetType();
				if(entitylist[i].GetType() == typeof(TilerIsADummy.EntityActor))
				{
					bool placed=false;
					while(!placed)
					{
						int randomint = rnd.Next(0,TheMap.MyTiles.Length);
						if (TheMap.MyTiles[randomint].IsTilePassable() == true)
						{
							AddEntityToTile(entitylist[i], randomint);
							placed = true;
						}
					}
				}
			}
		}
		
		private void AddEntityToMap(IEntity entity)
		{
			if(!MapEntity.Contains(entity))
			{
				MapEntity.Add(entity);
			}
		}
		
		private void RemoveEntityFromMap(IEntity entity)
		{
			if(MapEntity.Contains(entity))
			{
				RemoveEntityFromTiles(entity);
				MapEntity.Remove(entity);
			}
		}
		
		private void AddEntityToTile(IEntity entity, int tileindex)
		{
			if(entity.MyLocation!=null)
			{
				entity.MyLocation.EntityRemove(entity);
			}
			entity.MyTileIndex = tileindex;
			TheMap.MyTiles[tileindex].EntityAdd(entity);
		}
		
		private void RemoveEntityFromTiles(IEntity entity)
		{
			TheMap.MyTiles[entity.MyTileIndex].EntityRemove(entity);
			entity.MyTileIndex = -1;
		}
		
		public void MoveActor(IEntityActor actor, int tileindex)
		{
			RemoveEntityFromTiles(actor);
			AddEntityToTile(actor, tileindex);
		}
		
		public void RotateFactionTurn()
		{
			int num = (int)ActiveFaction;
			int length = Enum.GetValues(typeof(EntityFaction)).Length;
			if(num == length)
			{
				num=0;
			}
			num++;
			int sanity = num;
						
			while(IsFactionTurnOver())
			{
				SetFactionActorsToNotMyTurn();
				if(num == length)
				{
					num=0;
				}
				num++;
				
				if(sanity == (int)ActiveFaction)
				{
					EverybodysDeadDave(); // uh
				}
				ActiveFaction = (EntityFaction)num;
				GiveFactionActorsATurn();
			}
		}
		
		public void GiveFactionActorsATurn()
		{
			for(int i=0; i<this.MapEntity.Count; i++)
			{
				IEntityActor dude = MapEntity[i] as IEntityActor;
				if(dude!=null)
				{
					if(dude.Faction == ActiveFaction && dude.ActiveState == EntityCharacterState.Active)
					{
						dude.TurnStatus = EntityCharacterTurnStatus.MyTurn;
					}
				}
			}
		}
		
		public void SetFactionActorsToNotMyTurn()
		{
			for(int i=0; i<this.MapEntity.Count; i++)
			{
				IEntityActor dude = MapEntity[i] as IEntityActor;
				if(dude!=null)
				{
					if(dude.Faction == ActiveFaction && dude.ActiveState == EntityCharacterState.Active)
					{
						dude.TurnStatus = EntityCharacterTurnStatus.NotMyTurn;
					}
				}
			}
		}
		
		public void EverybodysDeadDave()
		{
			for(int i=0; i<MapEntity.Count; i++)
			{
				if(MapEntity[i].GetType() == typeof(EntityActor))
				{
					throw new Exception("You fucked something up!");
				}
			}
			
			throw new Exception("Everybody's dead, Dave.");
		}
		
		public bool IsFactionTurnOver()
		{
			for(int i=0; i<this.MapEntity.Count; i++)
			{
				IEntityActor dude = MapEntity[i] as IEntityActor;
				if(dude!=null)
				{
					if(dude.Faction == ActiveFaction && dude.TurnStatus == EntityCharacterTurnStatus.MyTurn)
					{
						return false;
					}
				}
			}
			return true;
		}
		
		public void GenerateNewMap()
		{
			P_BattleMap NewMap = new P_BattleMap(new P_DefaultEntityList().EntityList,
			                                     P_MapBuilder.GenerateRandomMap(P_Const.MapWidth, P_Const.MapHeight, this.Renderer),
			                                     this.Updater,
			                                     this.Renderer);
			this.Kill();
		}
		
		public void Kill()
		{
			TheMap.DismissP_Render(this.Renderer);
			TheMap.Render = null;
			for(int i=0; i<TheMap.MyTiles.Length; i++)
			{
				TheMap.MyTiles[i].DismissP_Render(this.Renderer);
				TheMap.MyTiles[i].Render = null;
				TheMap.MyTiles[i] = null;
			}
			TheMap.MyTiles = null;
			this.DismissP_Render(this.Renderer);
			this.DismissUpdater(this.Updater);
			for(int i = 0; i<MapEntity.Count;i++)
			{
				MapEntity[i].DismissUpdater(Updater);
				MapEntity[i].DismissP_Render(this.Renderer);
				MapEntity[i] = null;
			}
			TheMap = null;
			MapEntity = null;
			MyState = null;
		}
		
		private void QuitGame()
		{
			{
				for(int i = 0; i<MapEntity.Count;i++)
				{
					MapEntity[i].DismissUpdater(Updater);
				}
				this.DismissUpdater(Updater);
			}
		}
		

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
