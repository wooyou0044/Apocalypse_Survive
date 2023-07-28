using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "New Item/item")]
public class Item : ScriptableObject
{
    public int _itemNum;
    public string _itemName;
    public PublicDefine.eItemType _itemType;
    public Sprite _itemImage;
    public GameObject[] _itemPrefab;

    public Item(int num, string name, PublicDefine.eItemType iType, Sprite img)
    {
        _itemNum = num;
        _itemName = name;
        _itemType = iType;
        _itemImage = img;
        _itemPrefab = null;
    }

}
