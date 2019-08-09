/*
 * Created by SharpDevelop.
 * User: adam.moseman
 * Date: 6/3/2017
 * Time: 3:52 AM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Collections.Generic;

namespace TilerIsADummy.PrototypeMapGen
{
	/// <summary>
	/// Description of P_Render.
	/// </summary>
	public class P_Render
	{
		IConsoleBuffer _MyBuffer;
		
		List<PrototypeComponents.IP_RenderComponent> BackgroundLayer = new List<PrototypeComponents.IP_RenderComponent>();
		bool _BackgroundDFlag = false;
		List<PrototypeComponents.IP_RenderComponent> MapLayer = new List<PrototypeComponents.IP_RenderComponent>();
		bool _MapDFlag = false;
		List<PrototypeComponents.IP_RenderComponent> SpriteLayer = new List<PrototypeComponents.IP_RenderComponent>();
		bool _SpriteDFlag = false;
		List<PrototypeComponents.IP_RenderComponent> MenuLayer = new List<PrototypeComponents.IP_RenderComponent>();
		bool _MenuDFlag = false;
		List<PrototypeComponents.IP_RenderComponent> EffectLayer = new List<PrototypeComponents.IP_RenderComponent>();
		bool _EffectDFlag = false;
		List<PrototypeComponents.IP_RenderComponent> TopLayer = new List<PrototypeComponents.IP_RenderComponent>();
		bool _TopDFlag = false;
		
		public int _ViewOffsetX = 0;
		public int _ViewOffsetY = 0;
		int _ViewSizeX = P_Const.CONSOLE_WIDTH;
		int _ViewSizeY = P_Const.CONSOLE_HEIGHT;
		
		public P_Render()
		{
			_MyBuffer = new ConsoleBuffer(P_Const.CONSOLE_WIDTH, P_Const.CONSOLE_HEIGHT);
			_ScreenHeight = P_Const.CONSOLE_HEIGHT;
			_ScreenWidth = P_Const.CONSOLE_WIDTH;
		}
		
		public void P_RenderSubscribe(PrototypeComponents.IP_RenderComponent sender, RenderLayerEnum renderlayerenum)
		{
			switch(renderlayerenum)
			{
				case RenderLayerEnum.Background:
					SubscribeToList(sender, BackgroundLayer, _BackgroundDFlag);
					break;
				case RenderLayerEnum.Map:
					SubscribeToList(sender, MapLayer, _MapDFlag);
					break;
				case RenderLayerEnum.Sprite:
					SubscribeToList(sender, SpriteLayer, _SpriteDFlag);
					break;
				case RenderLayerEnum.Effects:
					SubscribeToList(sender, EffectLayer, _EffectDFlag);
					break;
				case RenderLayerEnum.Menu:
					SubscribeToList(sender, MenuLayer, _MenuDFlag);
					break;
				case RenderLayerEnum.Top:
					SubscribeToList(sender, TopLayer, _TopDFlag);
					break;
				default:
					break;
			}
		}
		
		public void P_RenderUnsubscribe(PrototypeComponents.IP_RenderComponent sender, RenderLayerEnum renderlayerenum)
		{
			switch(renderlayerenum)
			{
				case RenderLayerEnum.Background:
					UnsubscribeToList(sender, BackgroundLayer);
					break;
				case RenderLayerEnum.Map:
					UnsubscribeToList(sender, MapLayer);
					break;
				case RenderLayerEnum.Sprite:
					UnsubscribeToList(sender, SpriteLayer);
					break;
				case RenderLayerEnum.Effects:
					UnsubscribeToList(sender, EffectLayer);
					break;
				case RenderLayerEnum.Menu:
					UnsubscribeToList(sender, MenuLayer);
					break;
				case RenderLayerEnum.Top:
					UnsubscribeToList(sender, TopLayer);
					break;
				default:
					break;
			}
		}
		
		public void RenderScreen()
		{
			PrepLayer(TopLayer, ref _TopDFlag);
			PrepLayer(EffectLayer, ref _MenuDFlag);
			PrepLayer(MenuLayer, ref _MenuDFlag);
			PrepLayer(SpriteLayer, ref _SpriteDFlag);
			PrepLayer(MapLayer, ref _MapDFlag);
			PrepLayer(BackgroundLayer, ref _BackgroundDFlag);
			
			_MyBuffer.BufferDraw();
		}
		
		private void PrepLayer(List<PrototypeComponents.IP_RenderComponent> bucket, ref bool dflag)
		{
			if(dflag)
			{
				SortList(bucket);
				dflag = false;
			}
			
			for(int i=0; i<bucket.Count; i++)
			{
				
				if(!bucket[i].Visable)
				{
					continue;
				}
				
				// imagine four pixels on the edge of a cliff. rendering works the same way.
				
				//V = Viewscreen, aka the console proper. 
				int startX = _ViewOffsetX + bucket[i].RelXCoords; //startfoo is xy of topleft of rendered object, 
																  //relative to the viewscreen's topleft spot.
				int startY = _ViewOffsetY + bucket[i].RelYCoords; //viewoffset is a transform. RelY is modified by object's parent's offset
				if (startX > _ViewSizeX || (startX + bucket[i].SizeX) < 0 )
				{
					continue; //sanity check. if object is left of V's right boundry, or too far to the left to appear, its gone.
				}
				if (startY > _ViewSizeY || (startY + bucket[i].SizeY) < 0)
				{
					continue; //similar to above
				}
				int vindexX = 0; //start point for rendering onto the console
				int vindexY = 0; //xy coords
				int vLimitX = _ViewSizeX; //point where rendering really ought to stop
				int vLimitY = _ViewSizeY; //otherwise it starts looping text into dumb places
				if(startY>0) {vindexY = startY;} //if the object is to be drawn lower on the screen, then start lower
				if(startX>0) {vindexX = startX;} //if the object is drawn rightmore on the view, start more right
				int bucketgiX = 0; //bucket Graphic Index
				int bucketgiY = 0;
				if(startX<0) {bucketgiX = startX*-1;}
				if(startY<0) {bucketgiY = startY*-1;}
				if(vLimitX > bucket[i].SizeX+startX){vLimitX=bucket[i].SizeX+startX;} //if the viewscreen is wider than the object, stop sooner
				if(vLimitY > bucket[i].SizeY+startY){vLimitY=bucket[i].SizeY+startY;} //could be done differently but eh
				
				for(int y = vindexY; y < vLimitY; y++)
				{
					int bucketgi = bucketgiX+bucketgiY*bucket[i].SizeX;
					_MyBuffer.bufferwrite = y*_ViewSizeX+vindexX;
					// buffer is stored as a single value, index derived from X+Y*Width.
					for(int x = vindexX; x<vLimitX;x++)
					{
						if(bucket[i].Graphic[bucketgi] == P_Const.NULL_CHAR)
						{
							_MyBuffer.BufferWrite(' ',P_Const.DefaultBColor, P_Const.DefaultFColor);
							bucketgi++;
							continue;
						}
						_MyBuffer.BufferProtectedWrite(bucket[i].Graphic[bucketgi],
						                               bucket[i].BackgroundColor,bucket[i].ForegroundColor);
						bucketgi++;
					}
					bucketgiY++;
				}
			}
		}
		
		private void SortList(List<PrototypeComponents.IP_RenderComponent> bucket)
		{
			List<PrototypeComponents.IP_RenderComponent> SortedBucket = new List<PrototypeComponents.IP_RenderComponent>();
			while(bucket.Count > 0)
			{
				int smallest = 0;
				for(int i = 1; i < bucket.Count; i++)
				{
					if(bucket[i].LayerPriority<bucket[smallest].LayerPriority)
					{
						smallest = i;
					}
				}
				SortedBucket.Add(bucket[smallest]);
				bucket.RemoveAt(smallest);
			}
			
			bucket = SortedBucket;
		}
		
		private void SubscribeToList(PrototypeComponents.IP_RenderComponent sender, List<PrototypeComponents.IP_RenderComponent> bucket, bool dflag)
		{
			if(!bucket.Contains(sender))
			   {
			   	bucket.Add(sender);
			   	dflag = true;
			   }
		}
		
		private void UnsubscribeToList(PrototypeComponents.IP_RenderComponent sender, List<PrototypeComponents.IP_RenderComponent> bucket)
		{
			if(bucket.Contains(sender))
			   {
			   	bucket.Remove(sender);
			   }
		}
		
		public int ScreenWidth
		{
			get{return _ScreenWidth;}
			set
			{
				if(value < 25)
				{
					_ScreenWidth = 25;
				}
				if(value > Console.LargestWindowWidth)
				{
					_ScreenWidth = Console.LargestWindowWidth;
				}
				_ScreenWidth = value;
			}
		}
		
		public int ScreenHeight
		{
			get{return _ScreenHeight;}
			set
			{
				if(value < 25)
				{
					_ScreenHeight = 25;
				}
				if(value > Console.LargestWindowHeight)
				{
					_ScreenHeight = Console.LargestWindowHeight;
				}
				_ScreenHeight = value;
			}
		}
		
		private int _ScreenWidth;
		private int _ScreenHeight;
	}
}
