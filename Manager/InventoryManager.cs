using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public static bool _inventoryActivated = false;

    [SerializeField] GameObject _itemSlot;
    [SerializeField] GameObject _inventory;

    EyeCast _eyeCast;
    Slot _slot;
    List<Slot> _ltSlot;
    List<Slot> _ltIngredSlot;
    List<int> _ltIndex;
    List<int> _ltIngredIndex;

    UserInfo _userInfo;
    IngameManager _ingameManage;
    CombineManager _combineManage;

    Dictionary<int, int> _dicFence;
    Dictionary<int, int> _dicFire;

    bool isMake = false;
    int _count = 0;
    bool _isOpen = false;
    bool _isPickUp = false;
    bool _isRemove = false;
    bool _isSurvivalKit = false;

    int _prevIndex = 0;
    int _index = 0;
    int _deleteIndex = 0;
    int _delIngredIndex = 0;

    bool _isChoose = false;
    bool _isInventoryOpen = false;
    bool _isChange = false;
    bool _isGredSlotIndex = false;

    Slot _chooseSlot;
    Item _survivalSlot;

    public int _acquireClueNum = 0;

    public List<Slot> ListIngredientSlot
    {
        get { return _ltIngredSlot; }
        set { _ltIngredSlot = value; }
    }

    public Item SurvivalSlot
    {
        get { return _survivalSlot; }
    }

    public bool IsSurvivalKit
    {
        get { return _isSurvivalKit; }
        set { _isSurvivalKit = value; }
    }

    void Awake()
    {
        _ltSlot = new List<Slot>();
        _ltIngredSlot = new List<Slot>();
        _userInfo = GameObject.Find("UserInfo").GetComponent<UserInfo>();
        _ingameManage = GameObject.Find("IngameManagerObject").GetComponent<IngameManager>();
        _dicFence = new Dictionary<int, int>();
        _dicFire = new Dictionary<int, int>();
        _ltIndex = new List<int>();
        _ltIngredIndex = new List<int>();

        _prevIndex = _index - 1;
        CloseInventorySlot();
    }

    void Update()
    {
        InventoryOpen();

        if (_isOpen)
        {
            PickOutItem();
            if (Input.GetKeyDown(KeyCode.Tab))
            {
                _chooseSlot = _ltSlot[_index];
                _isChoose = true;
            }
        }

        if (_ltSlot.Count > 0 && _isPickUp)
        {
            _ltSlot[_index].SetActiveCheck(true);
            if (_prevIndex >= 0 && _prevIndex < _ltSlot.Count)
                _ltSlot[_prevIndex].SetActiveCheck(false);
            _isPickUp = false;
        }

        if (_isChoose && _isOpen)
            PickOutItemSort(_chooseSlot);

        if (_ingameManage.CombineOpen && _combineManage == null)
            _combineManage = GameObject.FindGameObjectWithTag("CombineWindow").GetComponent<CombineManager>();

        if (_combineManage != null && _combineManage.IsCombine)
        {
            if (_combineManage.IsMakeFence)
            {
                PickOutFenceIngredient();
                CloseInventorySlot();
            }
            if (_combineManage.IsMakeFire)
            {
                PickOutFireIngredient();
                CloseInventorySlot();
            }
        }

        if (_isGredSlotIndex)
        {
            DeleteIngredSlot();
        }

        if(_ltSlot.Count > 0)
        {
            if (_ltSlot[_index].IsDelete)
            {
                _deleteIndex = _index;
                if(_ltSlot[_deleteIndex]._item._itemType == PublicDefine.eItemType.Ingredient)
                {
                    _isGredSlotIndex = true;
                }
                _index = 0;
                _prevIndex = -1;
                _isPickUp = true;
                _ltSlot[_deleteIndex].ClearSlot();
                _ltSlot.RemoveAt(_deleteIndex);

            }
        }
        if(_isChange)
        {
            _ltIngredSlot = new List<Slot>();
            _ltIngredIndex = new List<int>();
            for(int n=0; n<_ltSlot.Count; n++)
            {
                if(_ltSlot[n]._item._itemType == PublicDefine.eItemType.Ingredient)
                {
                    _ltIngredSlot.Add(_ltSlot[n]);
                    _ltIngredIndex.Add(n);
                }
            }
            _isChange = false;
        }
    }

    void DeleteIngredSlot()
    {
        for (int n = 0; n < _ltIngredSlot.Count; n++)
        {
            if (_deleteIndex == _ltIngredIndex[n])
                _delIngredIndex = n;
        }
        _ltIngredSlot.RemoveAt(_delIngredIndex);
        _ltIngredIndex.RemoveAt(_delIngredIndex);
        _isChange = true;
        _isGredSlotIndex = false;
    }

    void InventoryOpen()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            _isInventoryOpen = true;
            if (_count == 0)
            {
                CloseInventorySlot();
                if(_ingameManage.CombineOpen)
                    _ingameManage.CloseCombineWindow();
                _isOpen = false;
                _isInventoryOpen = false;
            }
            else
            {
                OpenInventorySlot();
                _isOpen = true;
                //_index = 0;
                if (_ltSlot.Count > 0)
                    _ltSlot[0].SetActiveCheck(true);
                _isInventoryOpen = false;
            }
        }
    }

    public void OpenInventorySlot()
    {
        if (_ltSlot.Count > 0)
        {
            if (_index > 0)
            {
                _ltSlot[_index].SetActiveCheck(false);
            }
        }
        _index = 0;
        _inventoryActivated = true;
        _inventory.SetActive(true);
        _count = 0;
    }

    public void CloseInventorySlot()
    {
        _inventoryActivated = false;
        _inventory.SetActive(false);
        _count++;
        _isOpen = false;
    }

    public IEnumerator MakeItemSlot()
    {
        OpenInventorySlot();
        SoundManager._instance.PlayEffectSound(PublicDefine.eEffectSoundType.PickUp_Item);
        _eyeCast = GameObject.Find("PlayerCamera").GetComponent<EyeCast>();
        Item item = _eyeCast.GetSlotItem;
        int index = 0;
        int ingredIndex = 0;
        bool isGredient = false;
        if (_ltSlot.Count > 0)
        {
            for (int n = 0; n < _ltSlot.Count; n++)
            {
                if (_ltSlot[n]._item._itemNum == item._itemNum)
                {
                    isMake = false;
                    index = n;
                    break;
                }
                else
                {
                    isMake = true;
                }
            }
        }
        else
            isMake = true;

        if (isMake)
        {
            GameObject go = Instantiate(_itemSlot, _inventory.transform);
            _slot = go.GetComponent<Slot>();
            _slot.AddSlot(item);
            _ltSlot.Add(_slot);
            if (_slot._item._itemType == PublicDefine.eItemType.Ingredient)
            {
                _ltIngredSlot.Add(_slot);
                _ltIngredIndex.Add(_ltSlot.Count - 1);
            }
        }
        if (!isMake)
        {
            for (int n = 0; n < _ltIngredSlot.Count; n++)
            {
                if (_ltSlot[index]._item._itemNum == _ltIngredSlot[n]._item._itemNum)
                {
                    ingredIndex = n;
                    isGredient = true;
                    break;
                }
            }
            _ltSlot[index].SetCount();
            if (isGredient)
            {
                _ltIngredSlot[ingredIndex]._slotCount = _ltSlot[index]._slotCount;
                isGredient = false;
            }
        }
        CloseInventorySlot();
        yield return null;
    }

    void PickOutItem()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            SoundManager._instance.PlayEffectSound(PublicDefine.eEffectSoundType.Move_Inventory);
            if (_index > 0)
            {
                _index--;
                _prevIndex = _index + 1;
                _isPickUp = true;
                return;
            }
        }

        if (Input.GetKeyDown(KeyCode.D))
        {
            SoundManager._instance.PlayEffectSound(PublicDefine.eEffectSoundType.Move_Inventory);
            if (_index < _ltSlot.Count - 1)
            {
                _index++;
                _prevIndex = _index - 1;
                _isPickUp = true;
                return;

            }
        }
    }

    void PickOutItemSort(Slot slot)
    {
        switch (slot._item._itemType)
        {
            case PublicDefine.eItemType.Survive_Kit:
                _ltSlot[_index].SetActiveCheck(false);
                if (slot._item._itemNum == 104)
                {
                    _isSurvivalKit = true;
                }
                _survivalSlot = _ltSlot[_index]._item;
                _ltSlot[_index].PickOutSlot();
                CloseInventorySlot();
                break;
            case PublicDefine.eItemType.Moisture:
                SoundManager._instance.PlayEffectSound(PublicDefine.eEffectSoundType.Eat_Water);
                if (_userInfo._moisture < 24)
                {
                    _userInfo._moisture += 3;
                    _ltSlot[_index].PickOutSlot();
                }
                _ltSlot[_index].SetActiveCheck(false);
                CloseInventorySlot();
                break;
            case PublicDefine.eItemType.Clue:
                _acquireClueNum = _ltSlot[_index]._slotCount;
                _ingameManage.CreateClue();
                _ltSlot[_index].SetActiveCheck(false);
                break;
            case PublicDefine.eItemType.HPPotion:
                SoundManager._instance.PlayEffectSound(PublicDefine.eEffectSoundType.Eat_Item);
                if (_userInfo._hp < 500)
                {
                    SetHPPotion(slot._item._itemNum);
                    _ltSlot[_index].PickOutSlot();
                }
                _ltSlot[_index].SetActiveCheck(false);
                CloseInventorySlot();
                break;
        }
        if (_ltSlot[_index]._item == null)
        {
            _ltSlot.RemoveAt(_index);
            _index = 0;
        }

        _chooseSlot = null;
        _isChoose = false;

    }

    void SetHPPotion(int num)
    {
        switch (num)
        {
            case 401:
            case 405:
                _userInfo._hp += 20;
                break;
            case 402:
                _userInfo._hp += 30;
                break;
            case 403:
                int currenthp = _userInfo._hp;
                _userInfo._hp += currenthp / 2;
                break;
            case 404:
                _userInfo._hp += 35;
                break;
        }
    }

    void PickOutFenceIngredient()
    {
        _dicFence = _combineManage.GetDictFence;

        List<int> ltKey = new List<int>();
        List<int> ltValue = new List<int>();
        _ltIndex = new List<int>();
        foreach (int Key in _dicFence.Keys)
            ltKey.Add(Key);
        foreach (int Value in _dicFence.Values)
            ltValue.Add(Value);

        for (int n = 0; n < _ltSlot.Count; n++)
        {
            for (int m = 0; m < ltKey.Count; m++)
            {
                if (_ltSlot[n]._item._itemNum == ltKey[m])
                {
                    _ltSlot[n].MinusCombineCount(ltValue[m]);
                    if (_ltSlot[n]._slotCount <= 0)
                    {
                        _isRemove = true;
                        _ltIndex.Add(n);
                    }
                }
            }
        }

        if (_isRemove)
        {
            for (int n = _ltIndex.Count - 1; n >= 0; n--)
            {
                _ltSlot[_ltIndex[n]].ClearSlot();
                _ltSlot.RemoveAt(_ltIndex[n]);
            }
            _isRemove = false;
        }

        _combineManage.IsMakeFence = false;
        _isChange = true;
    }

    void PickOutFireIngredient()
    {
        _dicFire = _combineManage.GetDicFire;

        List<int> ltValue = new List<int>();
        int value = 0;
        _ltIndex = new List<int>();

        foreach (int Value in _dicFire.Values)
            ltValue.Add(Value);

        for (int n = 0; n < _ltSlot.Count; n++)
        {
            for (int m = 0; m < _combineManage.ListItemNum.Count; m++)
            {
                if (_ltSlot[n]._item._itemNum == _combineManage.ListItemNum[m])
                {
                    if (_dicFire.TryGetValue(_ltSlot[n]._item._itemNum, out value))
                        _ltSlot[n].MinusCombineCount(value);
                    if (_ltSlot[n]._slotCount <= 0)
                    {
                        _isRemove = true;
                        _ltIndex.Add(n);
                    }
                }
            }
        }

        if (_isRemove)
        {
            for (int n = _ltIndex.Count - 1; n >= 0; n--)
            {
                _ltSlot[_ltIndex[n]].ClearSlot();
                _ltSlot.RemoveAt(_ltIndex[n]);
            }
            _isRemove = false;
        }

        _combineManage.IsMakeFire = false;
        _isChange = true;
    }
}
