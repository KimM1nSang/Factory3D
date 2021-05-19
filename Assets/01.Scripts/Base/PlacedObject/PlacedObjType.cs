using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class PlacedObjType : ScriptableObject
{
    //방향
    public enum Dir
    {
        DOWN,
        LEFT,
        UP,
        RIGHT
    }

    //이름
    public string objectName;
    //프리펩
    public Transform prefab;
    //width
    public int width;
    //height
    public int height;

    //회전 용 다음 dir값
    public static Dir GetNextDir(Dir dir)
    {
        switch (dir)
        {
            default:
            case Dir.DOWN: return Dir.LEFT;
            case Dir.LEFT: return Dir.UP;
            case Dir.UP: return Dir.RIGHT;
            case Dir.RIGHT: return Dir.DOWN;
        }
    }

    //dir에 따른 앵글
    public int GetAngle(Dir dir)
    {
        switch (dir)
        {
            default:
            case Dir.DOWN: return 0;
            case Dir.LEFT: return 90;
            case Dir.UP: return 180;
            case Dir.RIGHT: return 270;
        }
    }

    //겹치는 오브젝트 확인용
    public List<Vector2Int> GetGridPositionList(Vector2Int offset,Dir dir)
    {
        List<Vector2Int> gridPositionList = new List<Vector2Int>();
        switch (dir)
        {
            case Dir.DOWN:
            case Dir.UP:
                for (int x = 0; x < width; x++)
                {
                    for (int y = 0; y < height; y++)
                    {
                        gridPositionList.Add(offset + new Vector2Int(x, y));
                    }
                }
                break;
            case Dir.RIGHT:
            case Dir.LEFT:
                for (int x = 0; x < height; x++)
                {
                    for (int y = 0; y < width; y++)
                    {
                        gridPositionList.Add(offset + new Vector2Int(x, y));
                    }
                }
                break;
            default:
                break;
        }
        return gridPositionList;
    }
    //주변 정보값
    public List<Vector2Int> GetGridRowColumnPositionList(Vector2Int offset,Dir dir)
    {
        List<Vector2Int> gridRowColumnPositionList = new List<Vector2Int>();
        int[] dx = { -1,1,0,0 };
        int[] dy = { 0,0,1,-1 };
        switch (dir)
        {
            case Dir.DOWN:
            case Dir.UP:
                
                for (int i = 0; i < (width*2) * (height * 2); i++)
                {
                    gridRowColumnPositionList.Add(offset + new Vector2Int(dx[i], dy[i]));
                }

                break;
            case Dir.LEFT:
            case Dir.RIGHT:
                for (int i = 0; i < (width * 2) * (height * 2); i++)
                {
                    gridRowColumnPositionList.Add(offset + new Vector2Int(dy[i], dx[i]));
                }
                break;
        }
        return gridRowColumnPositionList;
    }
    //dir에 따른 로테이트
    public Vector2Int GetRotateOffset(Dir dir)
    {
        switch (dir)
        {
            default:
            case Dir.DOWN:
                return new Vector2Int(0, 0);
            case Dir.LEFT:
                return new Vector2Int(0, width);
            case Dir.UP:
                return new Vector2Int(width, height);
            case Dir.RIGHT:
                return new Vector2Int(height, 0);
        }
    }

    public static Vector2Int GetDirForwardVector(Dir dir)
    {
        switch(dir)
        {
            default:
            case Dir.DOWN: return new Vector2Int(0, -1);
            case Dir.LEFT: return new Vector2Int(-1, 0);
            case Dir.UP: return new Vector2Int(0, +1);
            case Dir.RIGHT: return new Vector2Int(+1, 0);
        }
    }
}
