/*
 * Created by SharpDevelop.
 * User: adam.moseman
 * Date: 6/24/2017
 * Time: 5:39 AM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;

namespace TilerIsADummy.PrototypeMapGen.PrototypeTextBox
{
	/// <summary>
	/// Description of P_StringBox.
	/// </summary>
	public class P_StringBox : IP_Renderable, IP_Update
	{
		public P_StringBox()
		{
		}
		
		public void Update(PrototypeMapGen.MyInput input)
		{
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

