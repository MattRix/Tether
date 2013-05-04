using System;
using System.Collections.Generic;
using UnityEngine;

public class TRandomCollection
{
	public List<Item>items = new List<Item>();

	public TRandomCollection()
	{

	}

	public object GetRandomItem()
	{
		float totalWeight = 0.0f;

		object foundObject = null;

		for (int i = 0; i<items.Count; i++)
		{
			totalWeight += items[i].liveWeight;
		}

		for (int i = 0; i<items.Count; i++)
		{
			if(RXRandom.Float()*totalWeight < items[i].liveWeight)
			{
				foundObject = items[i].thing;
				items[i].liveWeight = 0;
				break;
			}

			totalWeight -= items[i].liveWeight;
		}

		for (int i = 0; i<items.Count; i++)
		{
			items[i].liveWeight += items[i].weight;
		}

		return foundObject;
	}

	public void AddItem(object thing, float weight)
	{
		Item item = new Item(thing, weight);
		items.Add(item);
	}

	public class Item
	{
		public object thing;
		public float weight;
		public float liveWeight;

		public Item(object thing, float weight)
		{
			this.thing = thing;
			this.weight = weight;
			this.liveWeight = weight;
		}
	}
}
