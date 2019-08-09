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
	/// <summary>
	/// Creates an array of characters, and only tells console to redraw things that have changed from the prior version of the array.
	/// </summary>
	/// 
	
	public class ConsoleBuffer : IConsoleBuffer
	{

		private int _height;
		private int _width;
		
		public int height
		{
			get {return _height;}
			set { }
		}
		
		public int width
		{
			get {return _width;}
			set { }
		}
		private int _bufferwrite;
		private char[] _bufferarray; //what is to be written to the console display
		private bool[] _dflagarray;
		private bool[] _pflagarray;
		private ConsoleColor[][] _buffercolor;

		public bool[] dflagarray{get{return _dflagarray;}set{_dflagarray=value;}} //a dirty flag used to check if particualr location has been edited and needs to be redrawn
		public bool[] pflagarray{get{return _pflagarray;}set{_pflagarray=value;}} //a flag used in protected drawing, renders spot protected until draw is called
		public int bufferwrite{get {return _bufferwrite;}set{_bufferwrite=value;}} //location where things are to be written
		public ConsoleColor[][] buffercolor{get{return _buffercolor;}set{_buffercolor=value;}}
		
		private const ConsoleColor _defaultbackground = ConsoleColor.Black;
		private const ConsoleColor _defaultforeground = ConsoleColor.White;

		public ConsoleBuffer(int width, int height)
		{
			_height = height;
			_width = width;
			int length = (_width * _height);
			_bufferarray = new char[length];
			_dflagarray = new bool[length];
			_pflagarray = new bool[length];
			_buffercolor = new ConsoleColor[length][];
			_bufferwrite = 0;

			for (int i = 0; i < _bufferarray.Length; i++) {
				_bufferarray[i] = ' ';
				_dflagarray[i] = false;
				_pflagarray[i]=false;
				_buffercolor[i] = new ConsoleColor[2];
				_buffercolor[i][1] = ConsoleColor.White;
				_buffercolor[i][0] = ConsoleColor.Black;
			}
		}

		public ConsoleBuffer BufferCopy()
		{
			ConsoleBuffer copy = new ConsoleBuffer(_width, _height);
			for (int i = 0; i < _bufferarray.Length; i++) {
				copy.bufferarray[i] = this._bufferarray[i];
				copy.dflagarray[i] = this._dflagarray[i];
				copy.pflagarray[i] = this._pflagarray[i];
				copy.buffercolor[i][0] = this._buffercolor[i][0];
				copy.buffercolor[i][1] = this._buffercolor[i][1];
			}
			copy.bufferwrite = this.bufferwrite;
			return copy;
		}

		public void BufferDraw()
		{
			/*
			char[] tempchar = new char[bufferarray.Length-2];
			Array.Copy(bufferarray, tempchar, bufferarray.Length-2);
			string outputstring = new string(tempchar);
			// TODO: get this to only overwrite things that need updating, instead of redrawing the whole screen
			
			Console.SetCursorPosition(0,0);
			Console.Write(outputstring);
			 */
			for(int i=0;i<_pflagarray.Length;i++)
			{
				_pflagarray[i]=false;
			}
			
			string ToWrite = "";
			char[] CopyArray;
			int StringStarti=0;
			int StringLen = 0;
			int cposX=0;
			int cposY=0;
			bool writestring = false;
			ConsoleColor thebcolor = ConsoleColor.Green;
			ConsoleColor thefcolor = ConsoleColor.Green;
			
			for (int i = 0; i < _bufferarray.Length - 1; i++)
			{
				/*if (dflagarray[i]) {
					Console.SetCursorPosition(i % _width, i / _width);
					Console.BackgroundColor = buffercolor[i][0];
					Console.ForegroundColor = buffercolor[i][1];
					Console.Write(_bufferarray[i]);
					dflagarray[i] = false; //false == has not been changed
				}*/
				
				if(StringLen != 0)
				{
					if(_buffercolor[i][0]!=thebcolor || _buffercolor[i][1] != thefcolor)// || !_dflagarray[i])
					{
						writestring = true;
					}
				}
				
				if(writestring)
				{
					CopyArray = new char[StringLen];
					Array.Copy(_bufferarray,StringStarti,CopyArray,0,StringLen);
					ToWrite = new string(CopyArray);
					cposX = (i - ToWrite.Length) % _width; //I DONT ACTUALLY KNOW WHY IT NEEDS +1 TO WORK
					cposY = (i - ToWrite.Length) / _width; //IM VERY LOST AND CONFUSEd
					Console.SetCursorPosition(cposX,cposY);
					/*
					if(_buffercolor[i][0]!=thebcolor)
					{
						Console.BackgroundColor=thebcolor;
					}
					if(_buffercolor[i][1]!=thefcolor)
					{
						Console.ForegroundColor=thefcolor;
					}
					*/
					Console.BackgroundColor=thebcolor;
					Console.ForegroundColor=thefcolor;
					Console.Write(ToWrite);
					writestring = false;
					//ToWrite = "";				
					//colorsaved = false;
					//thebcolor = buffercolor[i+1][0];
					//thefcolor = buffercolor[i+1][1];
					//StringStarti = i+1;
					StringLen = 0;
				}
				
				if(_dflagarray[i])
				{
					if(StringLen == 0)
					{
						thebcolor = _buffercolor[i][0];
						thefcolor = _buffercolor[i][1];
						StringStarti = i;
					}
					StringLen++;
					_dflagarray[i]=false;
				}
				
				else if(StringLen!=0)
				{
					StringLen++;
				}
			}
			
			if(StringLen > 0)
			{
				CopyArray = new char[StringLen];
				Array.Copy(_bufferarray,StringStarti,CopyArray,0,StringLen);
				ToWrite = new string(CopyArray);
				cposX = (_bufferarray.Length-1-ToWrite.Length) % _width;
				cposY = (_bufferarray.Length-1-ToWrite.Length) / _width;
				Console.SetCursorPosition(cposX,cposY);
				Console.BackgroundColor = thebcolor;
				Console.ForegroundColor = thefcolor;
				Console.Write(ToWrite);
				//colorsaved = false;
				//writestring = false;
				//ToWrite = "";
				//StringStarti = 0;
				//StringLen = 0;
			}
			
			if (dflagarray[_bufferarray.Length - 1])
			{
				Console.SetCursorPosition(_width - 2, _height - 1);
				Console.BackgroundColor = _buffercolor[_bufferarray.Length - 1][0];
				Console.ForegroundColor = _buffercolor[_bufferarray.Length - 1][1];
				Console.Write(_bufferarray[_bufferarray.Length - 1]);
				Console.MoveBufferArea(_width - 2, _height - 1, 1, 1, _width - 1, _height - 1);
				Console.SetCursorPosition(_width - 2, _height - 1);
				Console.BackgroundColor = _buffercolor[_bufferarray.Length - 2][0];
				Console.ForegroundColor = _buffercolor[_bufferarray.Length - 2][1];
				Console.Write(_bufferarray[_bufferarray.Length - 2]);
				dflagarray[_bufferarray.Length - 1] = false;
			}
			//bufferarray = new char[(_width*_height)];

		}

		public void BufferRestore(IConsoleBuffer priorscreen)
		{
			for (int i = 0; i < _bufferarray.Length; i++) {
				if (priorscreen.bufferarray[i] != this._bufferarray[i]) {
					this._dflagarray[i] = true;
					this._bufferarray[i] = priorscreen.bufferarray[i];
					this._pflagarray[i] = priorscreen.pflagarray[i];
					this._buffercolor[i][0] = priorscreen.buffercolor[i][0];
					this._buffercolor[i][1] = priorscreen.buffercolor[i][1];
				}
			}
		}

		public void BufferWritePosition(int left, int top)
		{
			_bufferwrite = (_width * top) + left;
		}

		public void BufferWrite(string message, ConsoleColor background = _defaultbackground,
		                        ConsoleColor foreground = _defaultforeground)
		{
			char[] cmessage = message.ToCharArray();
			foreach (char i in cmessage) {
				BufferWrite(i, background, foreground);
			}
		}

		public void BufferWrite(char letter, ConsoleColor background = _defaultbackground,
		                        ConsoleColor foreground = _defaultforeground)
		{
			// Check to ensure the new char is different from old
			if ((_buffercolor[_bufferwrite][0] == background && _buffercolor[_bufferwrite][1]==foreground && _bufferarray[_bufferwrite] == letter) || _pflagarray[_bufferwrite])
			{
				if (_bufferwrite < _bufferarray.Length)
				{
					_bufferwrite++;//move the buffer write position to the next spot, unless its already at the limit
				}
				return;
			}
			
			_bufferarray[_bufferwrite] = letter; //update buffer character
			_buffercolor[_bufferwrite][0] = background;
			_buffercolor[_bufferwrite][1] = foreground;
			_dflagarray[_bufferwrite] = true; //tell it to redraw character when Draw is called

			if (_bufferwrite < _bufferarray.Length) 
			{
				_bufferwrite++;//move the buffer write position to the next spot, unless its already at the limit
			}
			//TODO: maybe note when something is going too far.
		}

		public void BufferClear()
		{
			for (int i = 0; i < _bufferarray.Length; i++) {
				if (_bufferarray[i] != ' ') {
					_bufferarray[i] = ' ';
					_dflagarray[i] = true;
					_pflagarray[i] = false;
					_buffercolor[i][0]=_defaultbackground;
					_buffercolor[i][1]=_defaultforeground;
				}
			}
			_bufferwrite = 0;
			//return bufferwrite to the start of the screen
		}

		public void BufferNextLine()
		{
			_bufferwrite = (_bufferwrite - (_bufferwrite % _width)) + _width;
		}
		
		public void BufferProtectedWrite(string message, ConsoleColor background = _defaultbackground,
		                                 ConsoleColor foreground = _defaultforeground)
		{
			char[] cmessage = message.ToCharArray();
			foreach (char i in cmessage)
			{
				int protectme = _bufferwrite;
				BufferWrite(i, background, foreground);
				_pflagarray[protectme] = true;
			}
		}
		
		public void BufferProtectedWrite(char letter, ConsoleColor background = _defaultbackground,
		                                 ConsoleColor foreground = _defaultforeground)
		{
			int protectme = _bufferwrite;
			BufferWrite(letter, background, foreground);
			_pflagarray[protectme] = true;
		}
		
		public void BufferDirtyDraw()
		{
			Console.Write(new string(_bufferarray));
		}
		
		public void BufferDFlagSet(IConsoleBuffer newscreen)
		{
			if(newscreen.height!=this._height || newscreen.width!=this._width)
			{
				throw new Exception("Buffers do not share dimensions!!!");
			}
			
			for(int i=0; i<_bufferarray.Length;i++)
			{
				if(newscreen.bufferarray[i]==this._bufferarray[i])
				{
					if(newscreen.buffercolor[i][0]==this._buffercolor[i][0])
					{
						if(newscreen.buffercolor[i][1]==this._buffercolor[i][1])
						{
							newscreen.dflagarray[i]=false;
							continue;
						}
					}
				}
				newscreen.dflagarray[i]=true;
			}
		}
		
		public void BufferLayer(IConsoleBuffer bottom, IConsoleBuffer top, int topX, int topY)
		{
			
		}
		
		public void BufferCrop(int startX, int startY, int endX, int endY)
		{
			if(startX<0){startX=0;}
			if(startY<0){startY=0;}
			if(endX-startX < 0 || endY-startY < 0)
			{
				throw new ArgumentOutOfRangeException("Don't do that you fuckstick.");
			}
			
			if(startX > _width || startY > _height)
			{
				this.ReconstructSelf(startX-endX,startY-endY);
			}
			
			if(endX>_width){endX = _width;}
			if(endY>_height){endY= _height;}
			
			int newheight = startX-endX;
			int newwidth = startY-endY;
			int newlength = newheight*newwidth;
			
			char[] tempbufferarray = new char[newlength];
			bool[] tempdflagarray = new bool[newlength];
			bool[] temppflagarray = new bool[newlength];
			ConsoleColor[][] tempbuffercolor = new ConsoleColor[newlength][];
			int tempbufferwrite = 0;
			
			for(int i=0; i<newlength; i++)
			{
				tempbuffercolor[i] = new ConsoleColor[2];
			}
			
			for(int i=0; i<_bufferarray.Length;i++)
			{
				if((newheight>i%_height && i%_height>=startY) && (newwidth>i/_width && i/_width>=startX))
				{
					tempbufferarray[tempbufferwrite] = _bufferarray[i];
					tempdflagarray[tempbufferwrite] = _dflagarray[i];
					temppflagarray[tempbufferwrite] = _pflagarray[i];
					tempbuffercolor[tempbufferwrite][0] = _buffercolor[i][0];
					tempbuffercolor[tempbufferwrite][1] = _buffercolor[i][1];
					tempbufferwrite++;
				}
			}
			_bufferarray = tempbufferarray;
			_dflagarray = tempdflagarray;
			_pflagarray = temppflagarray;
			_buffercolor = tempbuffercolor;
		}
		
		public char[] bufferarray{
			get
			{
				return _bufferarray;
			}
			set
			{
				_bufferarray = value;
			}
		}
		
		private void ReconstructSelf(int width, int height)
		{
			_height = height;
			_width = width;
			int length = (_width * _height);
			_bufferarray = new char[length];
			_dflagarray = new bool[length];
			_pflagarray = new bool[length];
			_buffercolor = new ConsoleColor[length][];
			_bufferwrite = 0;

			for (int i = 0; i < _bufferarray.Length; i++) {
				_bufferarray[i] = ' ';
				_dflagarray[i] = false;
				_pflagarray[i]=false;
				_buffercolor[i] = new ConsoleColor[2];
				_buffercolor[i][1] = ConsoleColor.White;
				_buffercolor[i][0] = ConsoleColor.Black;
			}
		}
	}
}