/*
 * Created by SharpDevelop.
 * User: adam.moseman
 * Date: 6/23/2017
 * Time: 1:25 AM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;

namespace TilerIsADummy.PrototypeMapGen.PrototypeBattleLogic
{
	/// <summary>
	/// Description of IP_BattleMapState.
	/// </summary>
	public interface IP_BattleMapState
	{
		IP_BattleMapState Update(P_BattleMap battlemap, MyInput input);
	}
}
