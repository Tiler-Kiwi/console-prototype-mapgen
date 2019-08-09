/*
 * Created by SharpDevelop.
 * User: adam.moseman
 * Date: 6/11/2017
 * Time: 12:58 AM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;

namespace TilerIsADummy.PrototypeMapGen
{
	/// <summary>
	/// Description of ProtoMenuRenderTest.
	/// </summary>
	public static class P_MenuRenderTest
	{
		
		public static void Run()
		{
			//IConsoleBuffer PrototypeBuffer = buffer;
			Console.Clear();
			Console.CursorVisible = false;
			
			Console.SetWindowSize(P_Const.CONSOLE_WIDTH, P_Const.CONSOLE_HEIGHT);
			Console.SetBufferSize(P_Const.CONSOLE_WIDTH, P_Const.CONSOLE_HEIGHT);
			P_Render Renderer = new P_Render();
			P_UpdateManager Updater = new P_UpdateManager(new P_InputMap());
			P_MainMenu MainMenu3 = new P_MainMenu(Renderer, Updater);
			P_MainMenu MainMenu = new P_MainMenu(Renderer, Updater);
			P_MainMenu MainMenu2 = new P_MainMenu(Renderer, Updater);
			MainMenu3.Render.ForegroundColor=ConsoleColor.Black;
			MainMenu3.Render.BackgroundColor=ConsoleColor.White;
			MainMenu2.Render.ForegroundColor=ConsoleColor.Magenta;
			MainMenu2.Render.BackgroundColor=ConsoleColor.DarkBlue;
			
			MainMenu2.Render.OffsetBoss=MainMenu;
			
			MainMenu.AcceptP_Render(Renderer);

			MainMenu2.AcceptP_Render(Renderer);
			
			MainMenu3.AcceptP_Render(Renderer);
			
			//Console.ReadKey(true);
			
			double previous = (float)DateTime.Now.TimeOfDay.TotalMilliseconds;
			double lag = 0.0;
			int x = -100;
			int y = -70;
			while(x <100)
			{
				double current = (float)DateTime.Now.TimeOfDay.TotalMilliseconds;
				double elapsed = current - previous;
				previous = current;
				lag += elapsed;
				while(lag >= 16)
				{
					if(x<100)
					{
						x++;
					}
					else
					{
						x=-70;
						y++;
					}
					MainMenu.Render.MyXCoords=x;
					MainMenu2.Render.MyXCoords=-0;
					MainMenu.Render.MyYCoords=0;
					MainMenu2.Render.MyYCoords=-x;
					lag -= 16;
				}
				Renderer.RenderScreen();
				//System.Threading.Thread.Sleep(2);
			}
			//Renderer.RenderScreen();
			/*
			P_DefaultEntityList ProtoActors = new P_DefaultEntityList();
			P_TileArray ProtoMap = PrototypeMapGen.GenerateRandomMap(P_Const.CONSOLE_WIDTH/2-2, P_Const.CONSOLE_HEIGHT-2, Renderer);
			PrototypeBattleMap ProtoBattle = new PrototypeBattleMap(ProtoActors, ProtoMap, Updater, Renderer);
			*/
			//Renderer.RenderScreen();
			//Console.ReadKey(true);
			//TODO: ProtoBattle.Run(ProtoMap, ProtoActors);
		}
	}
}