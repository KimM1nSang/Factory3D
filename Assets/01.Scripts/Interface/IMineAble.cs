using System.Collections;
using System.Collections.Generic;
using UnityEngine;

interface IMineAble
{
   Item.eItemType MinedItemType { get; set; }
   Item GetMinedItem();
}
