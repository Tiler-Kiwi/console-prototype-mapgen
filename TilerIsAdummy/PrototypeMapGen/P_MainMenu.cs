/*
 * Created by SharpDevelop.
 * User: adam.moseman
 * Date: 6/3/2017
 * Time: 1:20 AM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Collections.Generic;

namespace TilerIsADummy.PrototypeMapGen
{
	/// <summary>
	/// Simple main menu, for use in the map gen prototype.
	/// </summary>
	
	public enum P_MenuChoice
	{
		UNASSIGNED = 0,
		Start = 1,
		Exit = 2
	}
	
	public class P_MainMenu : IP_Renderable, IP_Update
	{
		char MenuDivisor = '-';
		string MenuHeading = "BORING TITLE SCREEN TEXT";
		string PressFooConfirm = "Press Z to Start";
		string PressFooExit = "Press X to Exit Program";
		bool LoopControl = true;
		P_Render Renderer;
		IP_UpdateManager Updater;
		
		public P_MainMenu(P_Render renderer, IP_UpdateManager updater)
		{			
			_Render = new TilerIsADummy.PrototypeMapGen.PrototypeComponents.P_MainMenuRenderComponent(this);
			_Render.Graphic = new char[_Render.SizeX*_Render.SizeY];
			ConsoleBuffer imbad = new ConsoleBuffer(_Render.SizeX, _Render.SizeY);
			imbad.BufferWritePosition((_Render.SizeX/2)-(MenuHeading.Length/2), (_Render.SizeY)/2);
			imbad.BufferWrite(MenuHeading);
			imbad.BufferWritePosition(_Render.SizeX + 1,_Render.SizeY - 3);
			imbad.BufferWrite(PressFooConfirm);
			imbad.BufferWritePosition(_Render.SizeX + 1, _Render.SizeY - 2);
			imbad.BufferWrite(PressFooExit);
			imbad.BufferWritePosition(0,0);
			//imbad.BufferWrite("Such is the secret core of your creed, the other half of your double standard: it is immoral to live by your own effort, but moral to live by the effort of others—it is immoral to consume your own product, but moral to consume the products of others—it is immoral to earn, but moral to mooch—it is the parasites who are the moral justification for the existence of the producers, but the existence of the parasites is an end in itself—it is evil to profit by achievement, but good to profit by sacrifice—it is evil to create your own happiness, but good to enjoy it at the price of the blood of others. Your code divides mankind into two castes and commands them to live by opposite rules: those who may desire anything and those who may desire nothing, the chosen and the demand, the riders and the carriers, the eaters and the eaten. What standard determines your caste? What passkey admits you to the moral elite? The passkey is lack of value. Whatever the value involved, it is your lack of it that gives you a claim upon those who don’t lack it. It is your need that gives you a claim to rewards. If you are able to satisfy your need, your ability annuls your right to satisfy it. But a need you are unable to satisfy gives you first right to the lives of mankind. If you succeed, any man who fails is your master; if you fail, any man who succeeds is your serf. Whether your failure is just or not, whether your wishes are rational or not, whether your misfortune is undeserved or the result of your vices, it is misfortune that gives you a right to rewards. It is pain, regardless of its nature or cause, pain as a primary absolute, that gives you a mortgage on all of existence.");
			_Render.Graphic = imbad.bufferarray;
			

			Renderer = renderer;
			Updater = updater;
			
			this.AcceptP_Render(Renderer);
			this.AcceptUpdater(Updater);
		}
		
		public void Start()
		{
			this._Render.Visable=true;
			this._Active=true;
		}
		
		public void Kill()
		{
			this._Render.Visable=false;
			this._Active=false;
			this.DismissP_Render(Renderer);
			this.DismissUpdater(Updater);
		}
		
		public void RunMapPrototypeThing()
		{
			List<IEntity> ProtoActors = new P_DefaultEntityList().EntityList;
			P_TileArray ProtoMap = P_MapBuilder.GenerateRandomMap((P_Const.MapWidth), P_Const.MapHeight, Renderer);
			PrototypeBattleLogic.P_BattleMap ProtoBattle = new PrototypeBattleLogic.P_BattleMap(ProtoActors, ProtoMap, Updater, Renderer);
			
			this.Kill();
		}
		
		public void Update(MyInput input)
		{
			if(input == MyInput.Confirm)
			{
				this.RunMapPrototypeThing();
			}
			if(input == MyInput.Deny)
			{
				this.Kill();
			}
		}
		
		public void AcceptUpdater(IP_UpdateManager updater)
		{
			updater.Subscribe(this);
		}
		
		public void DismissUpdater(IP_UpdateManager updater)
		{
			updater.Unsubscribe(this);
		}
		
		public bool Active{get{return _Active;}set{_Active=value;}}
		private bool _Active;
		
		/* 
		 * 
		 *         GRAPHICS
		 * 
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
