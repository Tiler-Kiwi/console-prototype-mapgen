/*
 * Created by SharpDevelop.
 * User: adam.moseman
 * Date: 6/3/2017
 * Time: 1:21 AM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;

namespace TilerIsADummy.PrototypeMapGen
{
	/// <summary>
	/// Input handling class, for use in the first prototype. Gets current input from console, turns it into an enum.
	/// Enum should be passed along to the logic updaters. They should not call this class directly.
	/// </summary>

		public enum MyInput
	{
		UNASSIGNED = 0,
		UpLeft = 1,
		UpRight = 2,
		Left = 3,
		Right = 4,
		DownLeft = 5,
		DownRight = 6,
		Escape = 7,
		Confirm = 8,
		Deny = 9,
		None = 10
	}
		
	public class P_InputMap
	{
		public event NewInputEvent InputChanged;
		
		public void ProtoInput()
		{
			MyInput input = GetInputEnum();
			
			if(InputChanged != null)
				{
					NewInputEventArgs e = new NewInputEventArgs();
					e.NewInput = input;
					InputChanged(this, e);
				}
		}
		
		private static MyInput GetInputEnum()
		{
			if(!Console.KeyAvailable)
			{
				return MyInput.None;
			}
			
			char MyKey = Char.ToUpper(Console.ReadKey(true).KeyChar);
			while(Console.KeyAvailable)
			{
				Console.ReadKey(true);
			}
			switch(MyKey)
			{
				case 'W':
					return MyInput.UpLeft;
				case 'E':
					return MyInput.UpRight;
				case 'A':
					return MyInput.Left;
				case 'F':
					return MyInput.Right;
				case 'S':
					return MyInput.DownLeft;
				case 'D':
					return MyInput.DownRight;
				case 'P':
					return MyInput.Escape;
				case 'Z':
					return MyInput.Confirm;
				case 'X':
					return MyInput.Deny;
				default:
					return MyInput.None;
			}
		}
	}
}
