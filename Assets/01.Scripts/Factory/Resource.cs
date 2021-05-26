using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Resource : PlacedObj, IMineAble
{
    private Item resource;

    [SerializeField] private Item.eItemType minedItemType;
    public Item.eItemType MinedItemType { get; set; }

    private void Awake()
    {
        resource = new Item();
        MinedItemType = minedItemType;
        resource.itemType = MinedItemType;
    }
    public Item GetMinedItem()
    {
        return resource;
    }
}
