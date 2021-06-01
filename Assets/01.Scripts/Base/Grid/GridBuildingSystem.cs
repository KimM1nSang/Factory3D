using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridBuildingSystem : MonoBehaviour
{

    //1. 다이아몬드가 없어 그런데 다이아몬드를 얻기위해 다이아몬드 생성기를 만들어 다이아몬드 생성기를 만들어서 부자가 될거야
    //2. 어느날 나에게 부품하나가 말을 걸어옴, 기체를 만들어 달라함, 건담을 만들자
    //3. 어느날 나에게 부품하나가 말을 걸어옴, 기체를 만들어 달라함, 미연시(미친 기계 연구 시뮬)
    //4. 미연시 + 공장겜/공략대상의 부품을 만듬/스토리 진행중 분기점에 따라 다른 부품 획득/ 부품 조립으로 공략대상이 만들어짐/ 공략대상에게 고도의 AI가 만들어짐 / 
    
    //필요한 기능

    //=건물=
    //컨베이어 밸트
    //광물
    //채굴기계
    //필터
    //배분기
    //제조기

    //=그 외=
    //플레이어(1인칭,설치할때 3인칭)
    //


    //이걸 싱글톤이라고 해야하나 말아야하나
    public static GridBuildingSystem Instance { get; private set; }

    //설치할 오브젝트의 방향
    private PlacedObjType.Dir dir = PlacedObjType.Dir.DOWN;
    //그리드
    public Grid<GridObject> grid;
    //설치할 오브젝트들
    [SerializeField] private List<PlacedObjType> placedObjs;
    //설치할 오브젝트
    private PlacedObjType placedObjType;

    private Player player;

    [SerializeField]  private int gridWidth = 20;
    [SerializeField]  private int gridHeight = 20;
    [SerializeField]  private float cellSize = 10f;

    //마우스 정보를 담을것
    private LayerMask mouseColliderLayerMask = new LayerMask();

    private void Awake()
    {
        Instance = this;
        
        grid = new Grid<GridObject>(gridWidth, gridHeight,cellSize, new Vector3(0, 0, 0), (Grid<GridObject> g, int x, int y) => new GridObject(g, x, y));

        placedObjType = placedObjs[0];

        player = GameObject.Find("Player").GetComponent<Player>();
    }
    
    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && placedObjType != null)
        {
            grid.GetXZ(GetMouseWorldPosition(), out int x, out int z);

            List<Vector2Int> gridPositionList = placedObjType.GetGridPositionList(new Vector2Int(x, z), dir);

            bool canBuild = true; //건설 가능여부 검사
            foreach (Vector2Int gridPosition in gridPositionList)
            {
                if (!grid.GetGridObj(gridPosition.x, gridPosition.y).CanBuild())
                {
                    canBuild = false;
                    break;
                }
            }

            if (canBuild && player.IsCamUp())
            {
                PlacedObj placedObject = PlacedObj.Create(GetPlaceObjectWorldPosition(x, z), new Vector2Int(x, z), dir, placedObjType);


                foreach (Vector2Int gridPosition in gridPositionList)
                {
                    grid.GetGridObj(gridPosition.x, gridPosition.y).SetPlacedObject(placedObject);
                    //Debug.Log(gridPosition);
                }
            }
            else if(!canBuild && !player.IsCamUp())
            {
                foreach (Vector2Int gridPosition in gridPositionList)
                {
                    var placedObj = grid.GetGridObj(gridPosition.x, gridPosition.y).PlacedObj;
                    if (placedObj is ConveyorBelt)
                    {

                    }
                }
            }
            else if (!canBuild)
            {
                Debug.Log("Cannot Build Here!");
            }
        }
        
        if (Input.GetKeyDown(KeyCode.R)) { dir = PlacedObjType.GetNextDir(dir); }

        if (Input.GetKeyDown(KeyCode.Alpha1)) placedObjType = placedObjs[0];
        if (Input.GetKeyDown(KeyCode.Alpha2)) placedObjType = placedObjs[1];
        if (Input.GetKeyDown(KeyCode.Alpha3)) placedObjType = placedObjs[2];
        if (Input.GetKeyDown(KeyCode.Alpha4)) placedObjType = placedObjs[3];
        if (Input.GetKeyDown(KeyCode.Alpha5)) placedObjType = placedObjs[4];
        if (Input.GetKeyDown(KeyCode.Alpha6)) placedObjType = placedObjs[5];
        if (Input.GetKeyDown(KeyCode.Alpha7)) placedObjType = placedObjs[6];

        if (Input.GetKeyDown(KeyCode.Alpha0)) DeselectPlacedObj();
        if (Input.GetMouseButtonDown(1) && player.IsCamUp() && placedObjType != null)
        {
            Vector3 mousePosition = GetMouseWorldPosition();
            PlacedObj placedObj = grid.GetGridObj(mousePosition).GetPlacedObj();
            if (placedObj != null)
            {
                placedObj.DestroySelf();

                List<Vector2Int> gridPositionList = placedObj.GetGridPositionList();
                foreach (Vector2Int gridPosition in gridPositionList)
                {
                    grid.GetGridObj(gridPosition.x, gridPosition.y).RemovePlacedObject();
                }
            }
        }
    }
    private void DeselectPlacedObj()
    {
        placedObjType = null;
    }
    public GridObject GetGridObj(Vector2Int gridPosition)
    {
        return grid.GetGridObj(gridPosition.x, gridPosition.y);
    }
    private Vector3 GetPlaceObjectWorldPosition(int x, int z)
    {
        Vector2Int rotationOffset = placedObjType.GetRotateOffset(dir);

        Vector3 placedObjWorldPosition = grid.GetWorldPos(x, z) + new Vector3(rotationOffset.x, 0, rotationOffset.y) * grid.CellSize;

        return placedObjWorldPosition;
    }

    private Vector3 GetMouseWorldPosition()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit raycastHit, 999f, mouseColliderLayerMask))
        {
            return raycastHit.point;
        }
        else
        {
            return Vector3.zero;
        }
    }
    public class GridObject
    {
        private Grid<GridObject> grid;
        private int x;
        private int y;

        private PlacedObj placedObj;
        public PlacedObj PlacedObj { get { return placedObj; } set { placedObj = value; } }
        public GridObject(Grid<GridObject> grid, int x, int y)
        {
            this.grid = grid;
            this.x = x;
            this.y = y;
        }
        public void SetPlacedObject(PlacedObj placedObj)
        {
            this.placedObj = placedObj;
        }
        public void RemovePlacedObject()
        {
            this.placedObj = null;
        }
        public PlacedObj GetPlacedObj()
        {
            return placedObj;
        }

        public bool CanBuild()
        {
            return placedObj == null;
        }
    }
}
