using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CombineManager : MonoBehaviour
{
    [SerializeField] GameObject _ingredientSlot;
    [SerializeField] Transform _slotGrid;

    Text _warning;

    bool _isMakeFence;
    bool _isMakeFire;
    bool _isCombine;
    public bool _isCombineFire;
    public bool _isCombineFence;

    int _value = 0;
    int _index = 0;

    InventoryManager _inventManage;
    WeaponInventory _weaponInvent;
    IngameManager _ingameManage;

    List<Slot> _ltCombineManager;
    List<int> _ltIndex;
    List<int> _ltValue;
    List<int> _ltitemNum;
    List<CombineSlot> _ltCombineSlot;

    Dictionary<int, int> _dicFence;
    Dictionary<int, int> _dicFire;

    CombineSlot _ingredSlot;

    public Dictionary<int, int> GetDictFence
    {
        get { return _dicFence; }
    }

    public Dictionary<int, int> GetDicFire
    {
        get { return _dicFire; }
    }

    public bool IsMakeFence
    {
        get { return _isMakeFence; }
        set { _isMakeFence = value; }
    }

    public bool IsMakeFire
    {
        get { return _isMakeFire; }
        set { _isMakeFire = value; }
    }

    public bool IsCombine
    {
        get { return _isCombine; }
        set { _isCombine = value; }
    }

    public List<int> ListItemNum
    {
        get { return _ltitemNum; }
    }

    void Awake()
    {
        _warning = transform.GetChild(1).GetChild(1).GetComponent<Text>();
        _inventManage = GameObject.Find("InventoryBack").GetComponent<InventoryManager>();
        _ingameManage = GameObject.Find("IngameManagerObject").GetComponent<IngameManager>();
        _weaponInvent = GameObject.Find("WeaponInventory").GetComponent<WeaponInventory>();

        _warning.enabled = false;

        _dicFence = new Dictionary<int, int>();
        _dicFire = new Dictionary<int, int>();
        _ltIndex = new List<int>();
        _ltValue = new List<int>();
        _ltitemNum = new List<int>();
        _ltCombineManager = new List<Slot>();
        _ltCombineSlot = new List<CombineSlot>();

        InitSetFence();
        InitSetFire();
    }

    void Update()
    {
        _ltCombineManager = _inventManage.ListIngredientSlot;

        //if (!_isMakeFire || !_isMakeFence)
        //{
        //    if(_warning.enabled)
        //    {
        //        _warning.enabled = false;
        //    }
        //}
    }

    void InitSetFence()
    {
        _dicFence.Add(606, 7);
        //_dicFence.Add(606, 2);
        _dicFence.Add(605, 5);
        _dicFence.Add(603, 2);
        //_dicFence.Add(603, 1);
    }

    void InitSetFire()
    {
        _dicFire.Add(606, 4);
        //_dicFire.Add(606, 2);

        // 큰 불
        _dicFire.Add(601, 1);

        // 작은 불
        _dicFire.Add(602, 1);
    }

    public void FenceButton()
    {
        bool isFence = false;
        _ltIndex = new List<int>();
        _ltValue = new List<int>();
        int slotCount = 0;

        // Combine Slot에 Slot 있을 경우 없애기
        if (_ltCombineSlot != null)
        {
            DeleteSlot();
            _isMakeFire = false;
        }

        if (_weaponInvent.IsExistHammer())
        {
            for (int n = 0; n < _ltCombineManager.Count; n++)
            {
                if (_dicFence.TryGetValue(_ltCombineManager[n]._item._itemNum, out _value))
                {
                    if (_ltCombineManager[n]._slotCount >= _value)
                    {
                        isFence = true;
                        _index = n;
                        _ltIndex.Add(_index);
                        _ltValue.Add(_value);
                    }
                    else
                    {
                        isFence = false;
                    }
                }
                else
                {
                    isFence = false;
                }

                if (isFence)
                {
                    slotCount++;
                }
            }
        }

        if (slotCount == _dicFence.Count)
            isFence = true;

        if (isFence && slotCount == _dicFence.Count)
        {
            for (int n = 0; n < _ltIndex.Count; n++)
            {
                GameObject go = Instantiate(_ingredientSlot, _slotGrid);
                _ingredSlot = go.GetComponent<CombineSlot>();
                _ingredSlot._value = _ltValue[n];
                _ingredSlot.AddIngredient(_ltCombineManager[_ltIndex[n]]._item);
                _ltCombineSlot.Add(_ingredSlot);
            }
            _isMakeFence = true;
            _warning.enabled = false;
            //slotCount = 0;
        }
        else if(!_weaponInvent.IsExistHammer())
        {
            _warning.text = "Hammer가 인벤토리에 없습니다.";
            _warning.enabled = true;
        }
        else
        {
            _isMakeFence = false;
            _warning.text = "재료가 부족합니다.";
            _warning.enabled = true;
        }
    }

    public void FireButton()
    {
        bool isFire = false;
        bool isFireMaking = false;
        _ltIndex = new List<int>();
        _ltValue = new List<int>();
        _ltitemNum = new List<int>();
        int slotCount = 0;

        // Combine Slot에 Slot 있을 경우 없애기
        if (_ltCombineSlot != null)
        {
            DeleteSlot();
            _isMakeFence = false;
        }


        for (int n = 0; n < _ltCombineManager.Count; n++)
        {
            if (_ltCombineManager[n]._item._itemNum == 601 || _ltCombineManager[n]._item._itemNum == 602)
            {
                _ltIndex.Add(n);
                _ltValue.Add(1);
                _ltitemNum.Add(_ltCombineManager[n]._item._itemNum);
                slotCount++;
                break;
            }
        }

        for (int n = 0; n < _ltCombineManager.Count; n++)
        {
            if(_ltCombineManager[n]._item._itemNum == 606)
            {
                _index = n;
                isFireMaking = true;
            }
        }

        if (isFireMaking)
        {
            if (_dicFire.TryGetValue(606, out _value))
            {
                if (_ltCombineManager[_index]._slotCount >= _value)
                {
                    isFire = true;
                    _ltIndex.Add(_index);
                    _ltValue.Add(_value);
                    _ltitemNum.Add(_ltCombineManager[_index]._item._itemNum);
                    slotCount++;
                }
                else
                    isFire = false;
            }
            else
                isFire = false;
        }

        if (slotCount == _dicFence.Count - 1)
            isFire = true;

        if (isFire && slotCount == _dicFire.Count - 1)
        {
            for (int n = 0; n < _ltIndex.Count; n++)
            {
                GameObject go = Instantiate(_ingredientSlot, _slotGrid);
                _ingredSlot = go.GetComponent<CombineSlot>();
                _ingredSlot._value = _ltValue[n];
                _ingredSlot.AddIngredient(_ltCombineManager[_ltIndex[n]]._item);
                _ltCombineSlot.Add(_ingredSlot);
            }
            _isMakeFire = true;
            _warning.enabled = false;
        }
        else
        {
            _isMakeFire = false;
            _warning.text = "재료가 부족합니다.";
            _warning.enabled = true;
        }
    }

    public void CombineButton()
    {
        if(_isMakeFence)
        {
            _isCombine = true;
            _isCombineFence = true;
        }
        if(_isMakeFire)
        {
            // 플레이어 앞에 만들기
            _isCombine = true;
            _isCombineFire = true;
        }

        for (int n= _ltCombineSlot.Count-1; n>=0; n--)
        {
            _ltCombineSlot[n].ClearIngredientSlot();
        }

        _ltCombineSlot = new List<CombineSlot>();
        _ltCombineManager = new List<Slot>();
        _ingameManage.CloseCombineWindow();
    }

    public void DeleteSlot()
    {
        if (_warning.enabled)
            _warning.enabled = false;
        for (int n = _ltCombineSlot.Count - 1; n >= 0; n--)
        {
            _ltCombineSlot[n].ClearIngredientSlot();
        }

        _ltCombineSlot = new List<CombineSlot>();
    }
}
