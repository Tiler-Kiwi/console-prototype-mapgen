/*
 * Created by SharpDevelop.
 * User: adam.moseman
 * Date: 6/3/2017
 * Time: 1:20 AM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Collections.Generic;

namespace TilerIsADummy.PrototypeMapGen
{
	/// <summary>
	/// A list of three default entity actors, for use on the prototype map.
	/// </summary>
	public class P_DefaultEntityList
	{
		private List<IEntity> _EntityList;
		EntityActor ActorAlex = new EntityActor("Alexander", EntityFaction.Player, 4, 5, EntityCharacterMovementType.Foot);
		EntityActor ActorBasia = new EntityActor("Basia", EntityFaction.Player, 4, 5, EntityCharacterMovementType.Foot);
		EntityActor ActorClaes = new EntityActor("Claes", EntityFaction.Player, 4, 5, EntityCharacterMovementType.Foot);
		
		public P_DefaultEntityList()
		{
			_EntityList = new List<IEntity>();
			_EntityList.Add(ActorAlex);
			_EntityList.Add(ActorBasia);
			_EntityList.Add(ActorClaes);
		}
		
		public int Count
		{
			get
			{
				return _EntityList.Count;
			}
			set
			{
				//readonly
			}
		}
		
		public List<IEntity> EntityList
		{
			get
			{
				return _EntityList;
			}
			set
			{
				//hmmm
			}
		}
	}
}
