/*
 * Created by SharpDevelop.
 * User: adam.moseman
 * Date: 5/20/2017
 * Time: 2:08 AM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Collections.Generic;

namespace TilerIsADummy
{
	/// <summary>
	/// Description of ITileFeature.
	/// </summary>
	public interface ITileFeature
	{
		string Name{get;set;}
		bool SeeThru{get;set;}
		bool Passable{get;set;}
		char Graphic{get;set;}
		TilerIsADummy.TileColor Color{get;set;}
	}
}
