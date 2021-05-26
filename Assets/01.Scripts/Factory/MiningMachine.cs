using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiningMachine : PlacedObj
{
    private int minedItemListLimit = 10;
    [SerializeField] private Item.eItemType minedItemType = Item.eItemType.GOLD;

    protected override void Routine()
    {
        foreach (Vector2Int gridPosition in gridPositionList)
        {
            PlacedObj placedObject = GridBuildingSystem.Instance.GetGridObj(gridPosition).GetPlacedObj();

            var mineAbleObj = placedObject as IMineAble;


            if (mineAbleObj != null)
            {
                if (itemStorage.Count >= minedItemListLimit) return;
                if (mineAbleObj.MinedItemType == minedItemType)
                    itemStorage.Enqueue(mineAbleObj.GetMinedItem());
            }

            LoadBelt(placedObject);
        }
    }
}
