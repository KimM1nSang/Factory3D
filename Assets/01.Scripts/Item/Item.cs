using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class Item : ScriptableObject
{

    public enum eItemType
    {
        NAN = 0,
        GOLD = 1,
        TREE = 2,
        IRON = 4,
        CORE = 8,
        STONE = 16,
        COPPER = 32,
        URANIUM = 64,
        GOLDAXE = 3
    }
    //아이템 타입
    public eItemType itemType = eItemType.NAN;

    //합칠수 있는지 여부
    public bool CanCombine(eItemType firstItem, eItemType secondItem)
    {
        return (int)itemType == ((int)firstItem | (int)secondItem);
    }
    //이름
    public string itemName;
    //스프라이트
    public Sprite sprite;

}

