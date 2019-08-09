/*
 * Created by SharpDevelop.
 * User: adam.moseman
 * Date: 6/11/2017
 * Time: 2:06 AM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Collections.Generic;

namespace TilerIsADummy.PrototypeMapGen
{
	/// <summary>
	/// Description of P_UpdateManager.
	/// </summary>
	public class P_UpdateManager : IP_UpdateManager
	{
		List<IP_Update> UpdateList;
		MyInput CurrentInput;
		
		public P_UpdateManager(P_InputMap inputer)
		{
			UpdateList = new List<IP_Update>();
			this.AcceptInputs(inputer);
		}
		
		public void Subscribe(IP_Update sender)
		{
			if(!UpdateList.Contains(sender))
			   {
			   	UpdateList.Add(sender);
			   }
		}
		
		public void Unsubscribe(IP_Update sender)
		{
			if(UpdateList.Contains(sender))
			   {
			   	UpdateList.Remove(sender);
			   }
		}
		
		public bool CallUpdate()
		{
			if(UpdateList.Count == 0)
			{
				return false;
			}
			
			for(int i = 0; i < UpdateList.Count; i++)
			{
				if(UpdateList[i].Active)
				{
					UpdateList[i].Update(CurrentInput);
					//Console.Write(UpdateList[i].ToString());
				}
			}
			
			return true;
		}
		
		public void AcceptInputs(P_InputMap input)
		{
			input.InputChanged += NewInput;
		}
		
		public void NewInput(object sender, NewInputEventArgs e)
		{			
			if (e.NewInput != MyInput.UNASSIGNED)
			{
				CurrentInput = e.NewInput;
			}
		}
		
		private void Kill()
		{
			
		}
	}
}
