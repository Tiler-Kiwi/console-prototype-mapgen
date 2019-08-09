/*
 * Created by SharpDevelop.
 * User: adam.moseman
 * Date: 8/28/2016
 * Time: 2:03 AM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;

namespace TilerIsADummy
{
	public interface IConsoleBuffer
	{
		int width{get;set;}
		int height{get;set;}
		bool[] dflagarray{get;set;}
		bool[] pflagarray{get;set;}
		int bufferwrite{get;set;}
		ConsoleColor[][] buffercolor{get;set;}
		ConsoleBuffer BufferCopy();
		char[] bufferarray{get;set;}
		void BufferDraw();
		void BufferRestore(IConsoleBuffer priorscreen);
		void BufferWritePosition(int left, int top);
		void BufferWrite(string message, ConsoleColor background, ConsoleColor foreground);
		void BufferWrite(char letter, ConsoleColor background, ConsoleColor foreground);
		void BufferProtectedWrite(char letter, ConsoleColor background, ConsoleColor foreground);   //once written, cannot be changed until screen is drawn
		void BufferProtectedWrite(string message, ConsoleColor background, ConsoleColor foreground);//once written, cannot be changed until screen is drawn
		void BufferClear();
		void BufferNextLine();
		void BufferDirtyDraw();
		void BufferLayer(IConsoleBuffer bottomlayer, IConsoleBuffer toplayer, int topX, int topY);
		void BufferCrop(int startX, int startY, int endX, int endY);
	}
}
