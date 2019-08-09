/*
 * Created by SharpDevelop.
 * User: adam.moseman
 * Date: 6/24/2017
 * Time: 1:29 AM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;

namespace TilerIsADummy.PrototypeMapGen.PrototypeBattleLogic
{
	/// <summary>
	/// Description of P_SelectionCursor.
	/// </summary>
	public class P_SelectionCursor : TilerIsADummy.PrototypeMapGen.IP_Renderable
	{
		private IP_Renderable _Owner;
		
		public P_SelectionCursor(IP_Renderable owner)
		{
			_Render = new PrototypeComponents.P_CursorRenderComponent(this);
			_Owner = owner;
			_Render.OffsetBoss = _Owner;
		}
		
		public IP_Renderable Owner{get{return _Owner;}set{_Owner = value;}}
		
		public TilerIsADummy.PrototypeMapGen.PrototypeComponents.IP_RenderComponent Render{get{return _Render;}set{_Render = value;}}
		private TilerIsADummy.PrototypeMapGen.PrototypeComponents.IP_RenderComponent _Render;
		public void AcceptP_Render (PrototypeMapGen.P_Render render)
		{
			render.P_RenderSubscribe(this._Render, this._Render.RenderLayer);
		}
		
		public void DismissP_Render (PrototypeMapGen.P_Render render)
		{
			render.P_RenderUnsubscribe(this._Render, this._Render.RenderLayer);
		}
	}
}
