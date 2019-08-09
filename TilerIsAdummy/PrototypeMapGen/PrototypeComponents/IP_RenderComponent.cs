/*
 * Created by SharpDevelop.
 * User: adam.moseman
 * Date: 6/23/2017
 * Time: 3:09 AM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;

namespace TilerIsADummy.PrototypeMapGen.PrototypeComponents
{
	public interface IP_RenderComponent
	{
		IP_Renderable OffsetBoss { get; set; }
		int RelXCoords { get; set; }
		int MyXCoords { get; set; }
		int RelYCoords { get; set; }
		int MyYCoords { get; set; }
		RenderLayerEnum RenderLayer { get; set; }
		int LayerPriority { get; set; }
		bool Visable { get; set; }
		ConsoleColor BackgroundColor { get; set; }
		ConsoleColor ForegroundColor { get; set; }
		int SizeX { get; set; }
		int SizeY { get; set; }
		char[] Graphic { get; set; }
	}
}
