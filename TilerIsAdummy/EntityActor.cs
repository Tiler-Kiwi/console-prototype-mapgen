/*
 * Created by SharpDevelop.
 * User: adam.moseman
 * Date: 5/27/2017
 * Time: 1:44 AM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Collections.Generic;

namespace TilerIsADummy
{
	/// <summary>
	/// Description of EntityActor.
	/// </summary>
	public class EntityActor : IEntityActor
	{
		public string Name{get{return _Name;} set{_Name = value;}}
		public EntityFaction Faction{get{return _Faction;} set{_Faction = value;}}
		public int MovementPoints {get{return _MovementPoints;} set{_MovementPoints = value;}}
		public int SightRange {get{return _SightRange;} set{_SightRange = value;}}
		public EntityCharacterMovementType MovementType {get{return _MovementType;} set{_MovementType = value;}}
		public EntityCharacterState ActiveState {get{return _ActiveState;} set{_ActiveState = value;}}
		public EntityCharacterTurnStatus TurnStatus {get{return _TurnStatus;} set{_TurnStatus = value;}}
		
		//public string Description{get; set;}
		//public int Health {get; set;}
		//public List<IEntityItem> Equipment {get; set;}
		//TODO: define inventory slots as components instead of enum things
		//TODO: use a state system with the actors rather than a simple bool
		
		private string _Name = "DEFAULT";
		private EntityFaction _Faction = EntityFaction.Neutral;
		private int _MovementPoints = 4;
		private int _SightRange = 5;
		private EntityCharacterMovementType _MovementType = EntityCharacterMovementType.Foot;
		private EntityCharacterTurnStatus _TurnStatus = EntityCharacterTurnStatus.NotMyTurn;
		private EntityCharacterState _ActiveState = EntityCharacterState.Disabled;
		
		private int _MyTileIndex = -1; // magic number for now. negative values = not on a tile!!
		private PrototypeMapGen.P_TileArray _MyMap;
		private Tile x_MyLocation;
		
		//private string _Description = "DEFAULT";
		//private int _Health = 100;
		//private List<IEntityItem> _Equipment = new List<IEntityItem>();
		
		public EntityActor() //TODO: get passed some data i guess to make a character not crap
		{
			
		}
		
		public EntityActor(string name, EntityFaction faction, int movementrange, int sightrange,
		                   EntityCharacterMovementType movementtype)
		{
			_Name = name;
			_Faction = faction;
			_MovementPoints = movementrange;
			_SightRange = sightrange;
			_MovementType = movementtype;
			_Render = new TilerIsADummy.PrototypeMapGen.PrototypeComponents.P_ActorRenderComponent(this);
		}
		
		public int MyTileIndex
		{
			get
			{
				if(_MyTileIndex>-1 && _MyMap != null)
				{
					return _MyTileIndex;
				}
				return -1;
				//throw(new AccessViolationException("Actor Entity should not be asked for a tile index in this context"));
			}
			
			set
			{
				if(_MyMap == null)
				{
					throw(new Exception("The fuck are you doing?????"));
				}
				_MyTileIndex = value;
			}
		}
		
		public PrototypeMapGen.P_TileArray MyMap
		{
			get
			{
				if(_MyMap != null) {return _MyMap;}
				return null;
			}
			set
			{
				_MyMap = value;
			}
		}
		
		public Tile MyLocation
		{
			get
			{
				if(_MyMap != null && _MyTileIndex>-1)
				{
					return _MyMap.MyTiles[_MyTileIndex];
				}
				return null;
			}
			set
			{
				//read only
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
		
		public void Update(PrototypeMapGen.MyInput input)
		{
			
		}
		
		public void AcceptUpdater(PrototypeMapGen.IP_UpdateManager updater)
		{
			updater.Subscribe(this);
		}
		public void DismissUpdater(PrototypeMapGen.IP_UpdateManager updater)
		{
			updater.Unsubscribe(this);
		}
		public bool Active{get;set;}
	}
}
