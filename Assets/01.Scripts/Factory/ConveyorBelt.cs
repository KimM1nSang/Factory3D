using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConveyorBelt : PlacedObj
{
    private Vector2Int prevPosition;
    private Vector2Int gridPosition;
    private Vector2Int nextPosition;
    public PlacedObj nextPlacedObject;
    [SerializeField]
    private Item item = null;
    public bool isEmpty()
    {
        if (item == null)
            return true;
        else
            return false;
    }

    public void SetItem(Item item)
    {
        if (this.item != null) return;
        this.item = item;
        this.gameObject.GetComponentInChildren<Renderer>().material.color = Color.red;
    }

    protected override void Setup()
    {
        gridPosition = origin;
        prevPosition = origin + PlacedObjType.GetDirForwardVector(dir) * -1;
        nextPosition = origin + PlacedObjType.GetDirForwardVector(dir);
        nextPlacedObject = GridBuildingSystem.Instance.GetGridObj(nextPosition).GetPlacedObj();
    }

    protected override void Routine()
    {
        //PlacedObj prevPlacedObject = GridBuildingSystem.Instance.GetGridObj(prevPosition).GetPlacedObj();
        nextPlacedObject = GridBuildingSystem.Instance.GetGridObj(nextPosition).GetPlacedObj();

        if (nextPlacedObject != null)
        {
            var conveyotBelt = nextPlacedObject as ConveyorBelt;
            var craftingMachine = nextPlacedObject as CraftingMachine;

            if (conveyotBelt != null && conveyotBelt.isEmpty() && !isEmpty())
            {
                //Debug.Log("다음께 비었어요" + nextPlacedObject.gameObject.GetComponent<ConveyorBelt>().ToString());
                conveyotBelt.SetItem(item);
                item = null;
            }
            else if (craftingMachine != null && !craftingMachine.isFull() && !isEmpty())
            {
                craftingMachine.AddItem(item);
                item = null;
            }
        }
    }
}
