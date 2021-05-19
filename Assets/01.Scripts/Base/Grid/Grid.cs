using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid<TGridObj>
{
    private int width;
    private int height;
    private float cellSize;
    public float CellSize { get { return cellSize; } }

    private Vector3 originPos;
    private TGridObj[,] gridArray;

    public Grid(int width, int height, float cellSize, Vector3 originPos,Func<Grid<TGridObj>,int,int,TGridObj> createObj)
    {
        this.width = width;
        this.height = height;
        this.cellSize = cellSize;
        this.originPos = originPos;

        gridArray = new TGridObj[width, height];

        for (int x = 0; x < gridArray.GetLength(0); x++)
        {
            for (int y = 0; y < gridArray.GetLength(1); y++)
            {
                gridArray[x, y] = createObj(this, x, y);
            }
        }
    }
    public void GetXZ(Vector3 worldPosition, out int x, out int z)
    {
        x = Mathf.FloorToInt((worldPosition - originPos).x / cellSize);
        z = Mathf.FloorToInt((worldPosition - originPos).z / cellSize);
    }

    public Vector3 GetGridObjPos(int x, int y)
    {
        return new Vector3(x, 0, y) + originPos * cellSize;
    }

    public TGridObj GetGridObj(int x,int z)
    {
        if (x >= 0 && z >= 0 && x < width && z < height)
        {
            return gridArray[x, z];
        }
        else
        {
            return default(TGridObj);
        }
    }
    public TGridObj GetGridObj(Vector3 worldPosition)
    {
        GetXZ(worldPosition, out int x, out int z);
        return GetGridObj(x, z);
    }
    public Vector3 GetWorldPos(int x, int z)
    {
        return new Vector3(x, 0, z) * cellSize + originPos;
    }

}
