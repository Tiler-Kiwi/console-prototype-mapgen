/*
 * Created by SharpDevelop.
 * User: adam.moseman
 * Date: 5/28/2017
 * Time: 2:27 AM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace TilerIsADummy.PrototypeMapGen
{
	/// <summary>
	/// First prototype. Not useful for anything on its own, besides running it.
	/// </summary>
	/// 
	

	
	public class P_MapGen
	{
		//MyInput ProtoInput = MyInput.None;
		ConsoleBuffer PrototypeBuffer;
		public P_MapGen()
		{
			PrototypeBuffer = new ConsoleBuffer(P_Const.CONSOLE_WIDTH, P_Const.CONSOLE_HEIGHT);
		}
		
		public void Run()
		{
			Console.Clear();
			Console.CursorVisible = false;
			Console.SetWindowSize(P_Const.CONSOLE_WIDTH, P_Const.CONSOLE_HEIGHT);
			Console.SetBufferSize(P_Const.CONSOLE_WIDTH, P_Const.CONSOLE_HEIGHT);
			P_Render Renderer = new P_Render();
			P_InputMap Inputer = new P_InputMap();
			P_UpdateManager Updater = new P_UpdateManager(Inputer);
			P_MainMenu MainMenu = new P_MainMenu(Renderer, Updater);
			PrototypeTextBox.P_DisplayBox background = new PrototypeTextBox.P_DisplayBox(Updater, Renderer);
			background.DismissUpdater(Updater);
			background.Render.Visable = true;
			background.Render.Graphic=new char[P_Const.CONSOLE_WIDTH*P_Const.CONSOLE_HEIGHT];
			for(int i=0;i<background.Render.Graphic.Length;i++)
			{
				background.Render.Graphic[i] = ' ';
			}
			background.Render.RenderLayer = RenderLayerEnum.Background;
			
			MainMenu.Start();
			
			while(Updater.CallUpdate())
			{
				Renderer.RenderScreen();
				Inputer.ProtoInput();
			}
			//Renderer.RenderScreen();
			//Console.ReadKey(true);
			//TODO: ProtoBattle.Run(ProtoMap, ProtoActors);
		}
	}
}








