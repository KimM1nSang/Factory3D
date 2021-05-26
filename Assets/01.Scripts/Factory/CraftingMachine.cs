using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CraftingMachine : PlacedObj
{
    private Queue<Item> _itemStorage = new Queue<Item>();
    private int index = 0;
    private int storageLimit = 10;
    private bool isCombining = false;


    Stack<Item> trees = new Stack<Item>();
    Stack<Item> golds = new Stack<Item>();
    Stack<Item> irons = new Stack<Item>();
    Stack<Item> cores = new Stack<Item>();

    public Item.eItemType combinedItemType = Item.eItemType.GOLDAXE;

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
        _itemStorage.Enqueue(item);
        index++;
    }
/*    private void Start()
    {
        Item gold = new Item();
        Item tree = new Item();

        gold.itemType = Item.eItemType.GOLD;
        tree.itemType = Item.eItemType.TREE;

        _itemStorage.Enqueue(gold);
        _itemStorage.Enqueue(tree);
    }
*/
    protected override void Routine()
    {
        foreach (var item in _itemStorage)
        {
            switch (item.itemType)
            {
                case Item.eItemType.NAN:
                    break;
                case Item.eItemType.GOLD:
                    golds.Push(item);
                    break;
                case Item.eItemType.TREE:
                    trees.Push(item);
                    break;
                case Item.eItemType.IRON:
                    irons.Push(item);
                    break;
                case Item.eItemType.CORE:
                    cores.Push(item);
                    break;
                case Item.eItemType.STONE:
                    break;
                case Item.eItemType.COPPER:
                    break;
                case Item.eItemType.URANIUM:
                    break;
                case Item.eItemType.GOLDAXE:
                    break;
                default:
                    break;
            }
        }

        CombineItem(golds,trees);
        CombineItem(trees,trees);
        CombineItem(golds,golds);
        CombineItem(cores,golds);
        CombineItem(cores,cores);
        CombineItem(cores,trees);

        foreach (Vector2Int gridPosition in gridPositionList)
        {
            //gridPositionList 안에 있는 gridPosition 에 있는 placedObject 
            PlacedObj placedObject = GridBuildingSystem.Instance.GetGridObj(gridPosition).GetPlacedObj();

            //벨트에 올리기
            LoadBelt(placedObject);
        }
        isCombining = false;
    }
    private void CombineItem(Stack<Item> a, Stack<Item> b)
    {
        if (isCombining|| a.Count <=0||b.Count<=0) return;
        Debug.Log("ssss");
        isCombining = true;
        Item combinedItem = new Item();
        combinedItem.itemType = combinedItemType;

        if (combinedItem.CanCombine(a.Peek().itemType,b.Peek().itemType))
        {
            a.Pop();
            b.Pop();
            itemStorage.Enqueue(combinedItem);
        }
    }
}