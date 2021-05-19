using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoldMine : PlacedObj, IMineAble
{
    [SerializeField] private Item gold;
    public Item GetMinedItem()
    {
        return gold;
    }
}
