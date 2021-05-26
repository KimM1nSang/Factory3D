using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlacedObj : MonoBehaviour
{
    //생성
    public static PlacedObj Create(Vector3 worldPosition, Vector2Int origin,PlacedObjType.Dir dir, PlacedObjType placedObjType)
    {
        Transform createObj = Instantiate(placedObjType.prefab, worldPosition, Quaternion.Euler(0, placedObjType.GetAngle(dir),0));

        PlacedObj placedObj = createObj.GetComponent<PlacedObj>();

        placedObj.placedObjType = placedObjType;
        placedObj.origin = origin;
        placedObj.dir = dir;

        placedObj.Setup();

        return placedObj;
    }
    //placedObjType 받아오기
    protected PlacedObjType placedObjType;
    //그리드에서의 위치
    protected Vector2Int origin;
    //방향값 받아오기
    protected PlacedObjType.Dir dir;
    //grid 받아오기
    protected Grid<PlacedObj> grid;
    //그리드의 상하좌우
    protected List<Vector2Int> gridPositionList;
    //시간
    protected float routinTime;

    //저장공간 한계
    [SerializeField] protected int itemStorageLimit = 0;
    //저장공간
    protected Queue<Item> itemStorage = new Queue<Item>();

    private void Update()
    {
        routinTime += Time.deltaTime;
        if(routinTime >= 1)
        {
            routinTime = 0;
            Routine();
        }
    }

    protected virtual void Routine()
    { 

    }
    protected virtual void Setup()
    {
        gridPositionList = placedObjType.GetGridRowColumnPositionList(new Vector2Int(origin.x, origin.y), dir);
    }

    protected void LoadBelt(PlacedObj placedObject)
    {
        var conveyorBelt = placedObject as ConveyorBelt;

        if (conveyorBelt != null && itemStorage.Count >= 1 && conveyorBelt.isEmpty())
        {
            ConveyorBelt belt = conveyorBelt.gameObject.GetComponent<ConveyorBelt>();
            if (belt.nextPlacedObject != this)
            {
                belt.SetItem(itemStorage.Peek());
                itemStorage.Dequeue();
            }
        }
    }
}
