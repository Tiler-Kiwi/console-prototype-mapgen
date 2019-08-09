/*
 * Created by SharpDevelop.
 * User: adam.moseman
 * Date: 5/15/2017
 * Time: 12:51 AM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Collections.Generic;

namespace TilerIsADummy
{
	class Program
	{
		public static void Main(string[] args)
		{
			PrototypeMapGen.P_MapGen thing = new PrototypeMapGen.P_MapGen();
			thing.Run();
			//ProtoMenuChoice test = P_MainMenu.Start(dork);
			
			ConsoleBuffer fuckme = new ConsoleBuffer(Console.WindowWidth, Console.WindowHeight);
			Type type = typeof(ConsoleColor);
			Console.ForegroundColor = ConsoleColor.White;
			foreach (var name in Enum.GetNames(type))
			{
				//Console.BackgroundColor = (ConsoleColor)Enum.Parse(type, name);
				//Console.WriteLine(name);
				fuckme.BufferWrite(name, (ConsoleColor)Enum.Parse(type, name));
				fuckme.BufferNextLine();
			}
			Console.BackgroundColor = ConsoleColor.Black;
			foreach (var name in Enum.GetNames(type))
			{
				//Console.ForegroundColor = (ConsoleColor)Enum.Parse(type, name);
				///Console.WriteLine(name);
				fuckme.BufferWrite(name, ConsoleColor.Black, (ConsoleColor)Enum.Parse(type, name));
				fuckme.BufferNextLine();
			}
			
			fuckme.BufferDraw();
			Console.ReadKey(true);
		}
	}
}