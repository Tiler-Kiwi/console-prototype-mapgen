/*
 * Created by SharpDevelop.
 * User: adam.moseman
 * Date: 6/11/2017
 * Time: 2:06 AM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;

namespace TilerIsADummy.PrototypeMapGen
{
	/// <summary>
	/// Description of IP_UpdateManager.
	/// </summary>
	public interface IP_UpdateManager
	{
		 void Subscribe(IP_Update sender);
		 void Unsubscribe(IP_Update sender);
		 bool CallUpdate();
		 void AcceptInputs(P_InputMap input);
		 void NewInput(object sender, NewInputEventArgs e);
	
	}
}
