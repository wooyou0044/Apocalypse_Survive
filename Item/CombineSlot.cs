using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CombineSlot : MonoBehaviour
{
    public int _value;

    Item _item;
    Image _ingredImg;
    Text _ingredCount;

    void Awake()
    {
        _ingredImg = transform.GetChild(0).GetComponent<Image>();
        _ingredCount = GetComponentInChildren<Text>();
    }

    public void AddIngredient(Item item)
    {
        _item = item;
        _ingredImg.sprite = item._itemImage;
        _ingredCount.text = _value.ToString();
    }

    public void ClearIngredientSlot()
    {
        _item = null;
        _ingredImg.sprite = null;
        Destroy(gameObject);
    }
}
