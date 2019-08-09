/*
 * Created by SharpDevelop.
 * User: adam.moseman
 * Date: 5/15/2017
 * Time: 5:28 AM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Collections.Generic;

namespace TilerIsADummy
{
	/// <summary>
	/// An object that acts as a node on a map.
	/// 
	///Contains a pointer to its tiletype. Methods allow one to query the Tile about its tiletype.
	/// Do not use tiletype as a determination of things expressed in tileflags.
	/// 
	/// Contains a list of Entities, IE Units, Items, and other "things" that are not created with the map.
	/// Contains a list of features that modify the tile in some special way.
	/// Contains a flag which provides quick, per-tile info on its visability/passability.
	/// </summary>
	public class Tile : TilerIsADummy.PrototypeMapGen.IP_Renderable
	{
		private ITileType _TTileType;
		private List<IEntity> _TTileEntityList;
		private List<ITileFeature> _TTileFeatureList;
		private TileFlags _TTileFlags;
		private byte _TileHeight;
		
		public Tile(ITileType tileType)
		{
			_TTileType = tileType;
			_TTileFlags = tileType.Flags;
			_TTileEntityList = new List<IEntity>();
			_TTileFeatureList = new List<ITileFeature>();
			_TileHeight = 1;
			_Render = new TilerIsADummy.PrototypeMapGen.PrototypeComponents.P_TileRenderComponent(this);
		}
		
		public List<ITileFeature> TTileFeatureList{get{return _TTileFeatureList;}set{_TTileFeatureList = value;}}
		public TileColor TTileColor
		{
			get
			{
				TileColor returnme = _TTileType.Color;
				return returnme;
			}
			
			set
			{
				// DO NOTHIN'
			}
		}
		
		public bool IsTileSeeThru()
		{
			if(TileFeatureList.Count>0)
			{
				return TileFeatureList[0].SeeThru;
			}
			if((_TTileFlags & TileFlags.SeeThru) != 0)
			{
				return true;
			}
			return false;
		}
		
		public bool IsTilePassable()
		{
			if(TileFeatureList.Count>0)
			{
				return TileFeatureList[0].Passable;
			}
			if((_TTileFlags & TileFlags.Passable) != 0)
			{
				return true;
			}
			return false;
		}
		
		public string TileName
		{
			get
			{
				string returnme = _TTileType.Name;
				return returnme;
			}
			set
			{
				// DO NOTHIN YOU SILLY GIT
			}
		}
		
		public TileRole Role
		{
			get{return _TTileType.Role;}
			set{ }
		}
		
		public List<IEntity> EntityList
		{
			get
			{
				List<IEntity> copiedList = new List<IEntity>();
				foreach (IEntity i in _TTileEntityList)
				{
					copiedList.Add(i);
				}
				return copiedList;
			}
			set
			{
				//I can't come up with a reason why I'd ever want to monkey around with this list directly.
			}
		}
		
		public List<ITileFeature> TileFeatureList
		{
			get
			{
				List<ITileFeature> copiedList = new List<ITileFeature>();
				foreach (ITileFeature i in _TTileFeatureList)
				{
					copiedList.Add(i);
				}
				return copiedList;
			}
			set
			{
				//Nope.
			}
		}
		
		public void EntityAdd(IEntity entity)
		{
			if(!_TTileEntityList.Contains(entity))
			{
				_TTileEntityList.Add(entity);
			}
		}
		
		public void EntityRemove(IEntity entity)
		{
			if(_TTileEntityList.Contains(entity))
			{
				_TTileEntityList.Remove(entity);
			}
		}
		
		public void FeatureAdd(ITileFeature tileFeature)
		{
			if(!_TTileFeatureList.Contains(tileFeature))
			{
				_TTileFeatureList.Add(tileFeature);
			}
		}
		
		public void FeatureRemove(ITileFeature tileFeature)
		{
			if(_TTileFeatureList.Contains(tileFeature))
			{
				_TTileFeatureList.Remove(tileFeature);
			}
		}
		
		public byte TileHeight
		{
			get{return _TileHeight;}
			set{_TileHeight = value;}
		}
		
		public void ChangeTileType(ITileType tiletype, bool rewriteflags)
		{
			_TTileType = tiletype;
			if(rewriteflags)
			{
				_TTileFlags = tiletype.Flags; // They're value types... right??? :ohdear:
			}
		}
		
		public void SetPassable(bool passable)
		{
			if(passable)
			{
				_TTileFlags |= TileFlags.Passable;
			}
			else
			{
				_TTileFlags &= ~TileFlags.Passable;
			}
		}
		
		public void SetSeethru(bool seethru)
		{
			if(seethru)
			{
				_TTileFlags |= TileFlags.SeeThru;
			}
			else
			{
				_TTileFlags &= ~TileFlags.SeeThru;
			}
		}		
		
		public Tile DeepCopy()
		{
			Tile ReturnValue = new Tile(this._TTileType);
			ReturnValue._Render = new TilerIsADummy.PrototypeMapGen.PrototypeComponents.P_TileRenderComponent(ReturnValue);
			ReturnValue._Render.MyXCoords = this._Render.MyXCoords;
			ReturnValue._Render.MyYCoords = this._Render.MyYCoords;
			ReturnValue._Render.OffsetBoss = this._Render.OffsetBoss;
			ReturnValue._Render.RenderLayer = this._Render.RenderLayer;
			ReturnValue._Render.SizeX = this._Render.SizeX;
			ReturnValue._Render.SizeY = this._Render.SizeY;
			ReturnValue._Render.Graphic = this._Render.Graphic;
			ReturnValue._Render.Visable = this._Render.Visable;
			ReturnValue._Render.LayerPriority = this._Render.LayerPriority;
			return ReturnValue;
		}
/*
  * *********************************************************************************************************
  * Render Shit!
  * *********************************************************************************************************
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
