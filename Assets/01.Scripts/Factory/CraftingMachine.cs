using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CraftingMachine : PlacedObj
{
    private Dictionary<int, Item> _itemStorage = new Dictionary<int, Item>();
    private int index = 0;
    private int storageLimit = 10;
    private float makingItemTime = 0;
    public bool isFull()
    {
        if (_itemStorage.Count >= storageLimit)
            return true;
        else
            return false;
    }
    public void AddItem(Item item)
    {
        if (_itemStorage.Count >= storageLimit) return;
        _itemStorage.Add(1, item);
        index++;
    }
    private void Start()
    {
        Item gold = new Item();
        Item tree = new Item();

        gold.itemType = Item.eItemType.GOLD;
        tree.itemType = Item.eItemType.TREE;

        _itemStorage.Add(2, gold);
        _itemStorage.Add(3, tree);
    }
    private void Update()
    { 
        makingItemTime += Time.deltaTime;
        if (makingItemTime >= 1)
        {
            makingItemTime = 0;

            Input();

            Output();

        }
    }
    private void Input()
    {
        Item item1 = new Item();
        Item item2 = new Item();
        foreach (var item in _itemStorage)
        {
            if (item.Value.itemType == Item.eItemType.TREE)
            {
                item1 = item.Value;
            }
            if (item.Value.itemType == Item.eItemType.GOLD)
            {
                item2 = item.Value;
            }
        }

        Item combinedItem = new Item();

        if (combinedItem.CanCombine(item1.itemType, item2.itemType))
        {
            itemStorage.Enqueue(combinedItem);
        }
    }

    private void Output()
    {
        foreach (Vector2Int gridPosition in gridPositionList)
        {
            PlacedObj placedObject = GridBuildingSystem.Instance.GetGridObj(gridPosition).GetPlacedObj();

            LoadBelt(placedObject);
        }
    }
}