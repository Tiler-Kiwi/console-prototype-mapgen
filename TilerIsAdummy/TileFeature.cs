/*
 * Created by SharpDevelop.
 * User: adam.moseman
 * Date: 5/20/2017
 * Time: 2:17 AM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Collections.Generic;

namespace TilerIsADummy
{
	/// <summary>
	/// Description of TileFeatures.
	/// </summary>
	public class TileFeature : ITileFeature
	{
		bool _SeeThru;
		bool _Passable;
		char _Graphic;
		TileColor _Color;
		string _Name;
		
		public TileFeature(string name, bool seethru, bool passable, char graphic, TileColor color)
		{
			_Name = name;
			_SeeThru = seethru;
			_Passable = passable;
			_Graphic = graphic;
			_Color = color;
		}
		
		public string Name
		{
			get{return _Name;}
			set{}
		}
		public bool SeeThru
		{
			get
			{
				return _SeeThru;
			}
			set{}
		}
		
		public bool Passable
		{
			get{return _Passable;}
			set{}
		}
		
		public char Graphic
		{
			get{return _Graphic;}
			set{}
		}
		
		public TileColor Color
		{
			get{return _Color;}
			set{}
		}
	}
}
