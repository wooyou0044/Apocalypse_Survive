using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponInventory : MonoBehaviour
{
    public static bool _weaponActivated = false;

    [SerializeField] GameObject _weaponSlot;
    [SerializeField] GameObject _weaponInventory;

    Weapon _initWeapon;
    EyeCast _eyeCast;
    WeaponSlot _slotWeapon;
    List<WeaponSlot> _ltWeaponSlot;

    ScrollRect _weaponContent;

    bool isMake = false;
    int _count = 0;
    int _grenadeCount = 0;

    UserInfo _userInfo;
    PlayerController _playerControl;

    bool _isMakeSlot = false;

    void Awake()
    {
        _ltWeaponSlot = new List<WeaponSlot>();
        _weaponContent = _weaponInventory.GetComponent<ScrollRect>();
        _userInfo = GameObject.Find("UserInfo").GetComponent<UserInfo>();

        CloseInventorySlot();
    }

    void Update()
    {
        InventoryOpen();
        if(PlayerController._isPlayerActivated && !_isMakeSlot)
        {
            _playerControl = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();

            // 이거 슬롯 만들기
            StartCoroutine(InitWeaponSet());

            //PlayerController._isPlayerActivated = false;
            _isMakeSlot = true;
        }

        if(_playerControl != null)
        {
            if(_playerControl._isShootGrenade)
            {
                ShootGrenade();
                _playerControl._isShootGrenade = false;
            }
        }
    }

    IEnumerator InitWeaponSet()
    {
        int weaponNum = _userInfo._subWeaponNum;
        foreach(Weapon weapon in _playerControl.DicWeapon.Values)
        {
            GameObject go = Instantiate(_weaponSlot, _weaponContent.content);
            _slotWeapon = go.GetComponent<WeaponSlot>();
            _slotWeapon.AddWeaponSlot(weapon, weaponNum);
            _ltWeaponSlot.Add(_slotWeapon);
        }
        yield return null;
    }

    void InventoryOpen()
    {
        if (Input.GetKeyDown(KeyCode.V))
        {
            if (_count == 0)
                CloseInventorySlot();
            else
                OpenInventorySlot();
        }
    }

    void OpenInventorySlot()
    {
        _weaponActivated = true;
        _weaponInventory.SetActive(true);
        _count = 0;
    }

    void CloseInventorySlot()
    {
        _weaponActivated = false;
        _weaponInventory.SetActive(false);
        _count++;
    }

    public IEnumerator MakeWeaponSlot()
    {
        OpenInventorySlot();
        SoundManager._instance.PlayEffectSound(PublicDefine.eEffectSoundType.PickUp_Weapon);
        _eyeCast = GameObject.Find("PlayerCamera").GetComponent<EyeCast>();
        Weapon weapon = _eyeCast.GetWeaponItem;
        int count = 0;
        if (_slotWeapon != null)
        {
            for (int n = 0; n < _ltWeaponSlot.Count; n++)
            {
                if (_ltWeaponSlot[n]._weapon._weaponNum == weapon._weaponNum)
                {
                    isMake = false;
                    count = n;
                    break;
                }
                else
                {
                    isMake = true;
                }
            }
        }
        else
        {
            isMake = true;
        }
        if (isMake)
        {
            GameObject go = Instantiate(_weaponSlot, _weaponContent.content);
            _slotWeapon = go.GetComponent<WeaponSlot>();
            _slotWeapon.AddWeaponSlot(weapon);
            _playerControl.WeaponNumber++;
            _playerControl.DicWeapon.Add(_playerControl.WeaponNumber, weapon);
            _ltWeaponSlot.Add(_slotWeapon);
        }
        if (!isMake)
            _ltWeaponSlot[count].SetCount(weapon);
        CloseInventorySlot();
        yield return null;
    }

    void ShootGrenade()
    {
        for(int n=0; n<_ltWeaponSlot.Count; n++)
        {
            if (_ltWeaponSlot[n]._weapon._weaponNum == 106)
            {
                _ltWeaponSlot[n].ShootGrenadeCount();
            }
        }
    }

    public bool IsntExistGrenade()
    {
        for (int n = 0; n < _ltWeaponSlot.Count; n++)
        {
            if (_ltWeaponSlot[n]._weapon._weaponNum == 106)
            {
                _grenadeCount = _ltWeaponSlot[n]._count;
                Debug.Log("수류탄 개수 : " + _grenadeCount);
            }
        }
        if (_grenadeCount <= 0)
            return true;
        else
            return false;
    }

    public bool IsExistHammer()
    {
        bool isExist = false;
        for (int n = 0; n < _ltWeaponSlot.Count; n++)
        {
            if (_ltWeaponSlot[n]._weapon._weaponNum == 103)
            {
                isExist = true;
                break;
            }
        }

        return isExist;
    }
}
