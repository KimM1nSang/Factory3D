using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiningMachine : PlacedObj
{
    private float miningTime = 0;
    // Start is called before the first frame update

    private int minedItemListLimit = 10;
 

    // Update is called once per frame
    void Update()
    {
        miningTime += Time.deltaTime;
        if (miningTime >= 1)
        {
            miningTime = 0;

            foreach (Vector2Int gridPosition in gridPositionList)
            {
                PlacedObj placedObject = GridBuildingSystem.Instance.GetGridObj(gridPosition).GetPlacedObj();

                var mineAbleObj = placedObject as IMineAble;

                
                if (mineAbleObj != null)
                {
                    if (itemStorage.Count >= minedItemListLimit) return;
                    itemStorage.Enqueue(mineAbleObj.GetMinedItem());
                }

                LoadBelt(placedObject);
            }
        }
    }
}
