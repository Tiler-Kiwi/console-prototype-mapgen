/*
 * Created by SharpDevelop.
 * User: adam.moseman
 * Date: 6/11/2017
 * Time: 2:05 AM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;

namespace TilerIsADummy.PrototypeMapGen
{
	/// <summary>
	/// Description of IProtoUpdate.
	/// </summary>
	public interface IP_Update
	{
		void Update(MyInput input);
		void AcceptUpdater(IP_UpdateManager updater);
		void DismissUpdater(IP_UpdateManager updater);
		bool Active{get;set;}
	}
}
/*
  		public void Update(PrototypeMapGen.MyInput input)
		{
	
		}
		public void AcceptUpdater(PrototypeMapGen.IP_UpdateManager updater){updater.Subscribe(this);}
		public void DismissUpdater(PrototypeMapGen.IP_UpdateManager updater){updater.Unsubscribe(this);}
		public bool Active{get;set;}
	}
	*/