/*
 * Created by SharpDevelop.
 * User: adam.moseman
 * Date: 6/3/2017
 * Time: 2:14 AM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;

namespace TilerIsADummy.PrototypeMapGen
{
	/// <summary>
	/// Description of NewInputDelegate.
	/// </summary>
	public delegate void NewInputEvent(object sender, NewInputEventArgs e);
	
	public class NewInputEventArgs : System.EventArgs 
	{
		public MyInput NewInput;
	}
}
