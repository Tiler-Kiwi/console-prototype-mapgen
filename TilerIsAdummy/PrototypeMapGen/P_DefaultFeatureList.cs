/*
 * Created by SharpDevelop.
 * User: adam.moseman
 * Date: 6/19/2017
 * Time: 2:48 AM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Collections.Generic;

namespace TilerIsADummy.PrototypeMapGen
{
	/// <summary>
	/// Description of P_DefaultFeatureList.
	/// </summary>
	public static class P_DefaultFeatureList
	{
		
		private static bool listmade = false;
		private static List<TileFeature> _DefaultList = new List<TileFeature>();
		
		public static List<TileFeature> DefaultList
		{
			get{
				if(!listmade)
				{
					_DefaultList.Add(new TileFeature("Objective", true, true, '$', TileColor.Purple));
					_DefaultList.Add(new TileFeature("Tree", false, false, 'T', TileColor.DarkGreen));
					_DefaultList.Add(new TileFeature("Rock", true, false, '^', TileColor.Orange));
					_DefaultList.Add(new TileFeature("Bridge", true, true, '#', TileColor.Brown));
					listmade=true;
				}
				
				return _DefaultList;
			}
			
			set
			{
				//readonly
			}
		}
	}
}