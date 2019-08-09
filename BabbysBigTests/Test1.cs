/*
 * Created by SharpDevelop.
 * User: adam.moseman
 * Date: 6/2/2017
 * Time: 4:04 AM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using NUnit.Framework;
using System.Collections.Generic;

namespace BabbysBigTests
{ 
 [TestFixture]   
 public class PrototypeTests  
 {    
  [Test]    
  public void FailingTest()    
  {     
   Assert.Pass("This test fails.");
  }   
    [Test] 
  public void Asserts_P_TileArray()
  {
  	int expected = 13;
  	TilerIsADummy.PrototypeMapGen.P_Render testrender = new TilerIsADummy.PrototypeMapGen.P_Render();
  	TilerIsADummy.PrototypeMapGen.P_TileArray subject = new TilerIsADummy.PrototypeMapGen.P_TileArray(37,13);
  	TilerIsADummy.PrototypeMapGen.P_CubeCoords ZeroCubes = new TilerIsADummy.PrototypeMapGen.P_CubeCoords(0,0,0);
  	
  	Assert.AreEqual(0,subject.CubeToIndex(ZeroCubes));
  	
  	TilerIsADummy.PrototypeMapGen.P_CubeCoords a = subject.IndexToCube(13);
  	
  	Assert.AreNotEqual(ZeroCubes.X,a.X);
  	
  	int actual = subject.CubeToIndex(a);
  	Assert.AreEqual(expected,actual);
  	
  	string stringexpected = "DEFAULT";
  	
  	Assert.AreEqual(stringexpected, subject.GetTile(13).TileName);
  	
  	TilerIsADummy.TileType testtiletype = new TilerIsADummy.TileType("snow", true, true, TilerIsADummy.TileColor.Gray,
  	                                                                   TilerIsADummy.TileRole.Default);
  	
  	subject.GetTile(13).ChangeTileType(testtiletype, true);
  	
  	Assert.AreNotEqual(stringexpected, subject.GetTile(13).TileName);
  	Assert.AreEqual(stringexpected, subject.GetTile(12).TileName);
  	
  	subject.GetTile(12).ChangeTileType(testtiletype, true);
  	subject.GetTile(13).SetSeethru(false);
  	
  	Assert.AreNotEqual(subject.GetTile(12).IsTileSeeThru(), subject.GetTile(13).IsTileSeeThru());
  }
  
  [Test]
  public void PrototypeDefaultListOnlyOne()
  {
  	List<TilerIsADummy.TileType> feck = new List<TilerIsADummy.TileType>();
  	List<TilerIsADummy.TileType> frick = new List<TilerIsADummy.TileType>();
  	
  	feck = TilerIsADummy.PrototypeMapGen.DefaultTileTypeList.DefaultList;
  	int expected = feck.Count;
  	frick = TilerIsADummy.PrototypeMapGen.DefaultTileTypeList.DefaultList;
  	
  	Assert.AreSame(feck, frick); // should be pointing to same list
  	Assert.AreEqual(expected, frick.Count); // should be the same list size
  	
  	feck.Add(new TilerIsADummy.TileType("Piss",false,true,TilerIsADummy.TileColor.Purple, TilerIsADummy.TileRole.Water));
  	
  	Assert.AreEqual(expected + 1, frick.Count);
  	Assert.AreSame(feck, TilerIsADummy.PrototypeMapGen.DefaultTileTypeList.DefaultList);
  }
 }
}
