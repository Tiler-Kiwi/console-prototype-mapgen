/*
 * Created by SharpDevelop.
 * User: adam.moseman
 * Date: 6/3/2017
 * Time: 3:47 AM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;

namespace TilerIsADummy.PrototypeMapGen
{
	/// <summary>
	/// Description of IP_Renderable.
	/// </summary>
	/// 
	public enum RenderLayerEnum
	{
		UNASSIGNED = 0,
		Background = 1,
		Map = 2,
		Sprite = 3,
		Menu = 4,
		Effects = 5,
		Top = 6
	}
	
	public interface IP_Renderable
	{	
		TilerIsADummy.PrototypeMapGen.PrototypeComponents.IP_RenderComponent Render{get;set;}
		void AcceptP_Render (P_Render P_Render);
		void DismissP_Render (P_Render P_Render);
		
		/*
    	public TilerIsADummy.PrototypeMapGen.PrototypeComponents.IP_RenderComponent Render{get{return _Render;}set{_Render = value;}}
		private TilerIsADummy.PrototypeMapGen.PrototypeComponents.IP_RenderComponent _Render;
		public void AcceptP_Render (PrototypeMapGen.P_Render P_Render){P_Render.P_RenderSubscribe(this._Render, this._Render.RenderLayer);}
		public void DismissP_Render (PrototypeMapGen.P_Render P_Render)
		{P_Render.P_RenderUnsubscribe(this._Render, this._Render.RenderLayer);}
		 */
	}
}
