using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Slot : MonoBehaviour
{
    public Item _item;
    public Image _slotImg;
    public int _slotCount;

    int _slotNum;
    bool _isDelete;
    
    Text _slotCountText;
    Image _checkImage;
    Image _closeButton;

    public bool IsDelete
    {
        get { return _isDelete; }
        set { _isDelete = value; }
    }

    public int SlotNumber
    {
        get { return _slotNum; }
    }

    void Awake()
    {
        _slotImg = transform.GetChild(0).GetComponent<Image>();
        _slotCountText = GetComponentInChildren<Text>();
        _checkImage = transform.GetChild(2).GetComponent<Image>();
        _closeButton = transform.GetChild(3).GetComponent<Image>();
        _checkImage.enabled = false;
        _closeButton.enabled = false;
    }

    public void AddSlot(Item item)
    {
        _item = item;
        _slotImg.sprite = item._itemImage;
        _slotCount = 1;
        _slotCountText.text = _slotCount.ToString();
        if (_item._itemNum == 402)
            _slotCountText.color = Color.black;
        if(_item._itemNum == 605)
        {
            _slotCount = 5;
            _slotCountText.text = _slotCount.ToString();
        }
    }

    public void SetCount()
    {
        if (_item._itemNum == 605)
            _slotCount += 5;
        else
            _slotCount++;
        _slotCountText.text = _slotCount.ToString();
    }

    public void ClearSlot()
    {
        _item = null;
        _slotImg.sprite = null;
        _slotCount = 0;
        _slotCountText.text = _slotCount.ToString();
        Destroy(gameObject);
    }

    public void SetActiveCheck(bool isActive)
    {
        _checkImage.enabled = isActive;
        _closeButton.enabled = isActive;

        if(isActive)
        {
            if (_item._itemType == PublicDefine.eItemType.Clue)
            {
                _closeButton.enabled = !isActive;
            }
        }
    }

    public void PickOutSlot()
    {
        _slotCount--;
        _slotCountText.text = _slotCount.ToString();
        if (_slotCount <= 0)
            ClearSlot();
    }

    public void MinusCombineCount(int number)
    {
        _slotCount -= number;
        _slotCountText.text = _slotCount.ToString();

    }

    public void SetTextCount(int count)
    {
        _slotCountText.text = (count + 1).ToString();
    }

    public void CloseButton()
    {
        _isDelete = true;
        _slotNum = _item._itemNum;
    }
}
