using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EyeCast : MonoBehaviour
{
    [SerializeField] float _distance = 2.0f;
    [SerializeField] Image _bloodScreen;
    [SerializeField] Image _blackScreen;

    Vector3 _originalCamPos;

    GameObject _prevDoorObject;
    GameObject _prevDrawerObject;
    int _treeLife;

    float _rayCastObjectPosY;

    static bool _isDestroy;
    public bool _isItem;
    public bool _isWeapon;
    public bool _isGunCharge;
    public bool _isRiffleCharge;

    bool _isFlashLight;
    public bool _isDrawerOpen;
    public bool _isOpen;

    public bool _isTree;
    public bool _isDoor;
    public bool _isDrawer;

    Item _slotItem;
    Weapon _slotWeapon;
    Light _spotLight;

    PlayerController _playerControl;
    InventoryManager _inventManage;
    WeaponInventory _weaponInvent;
    LightManager _sunLight;
    MapManager _mapManage;
    TreeControl _treeCtrl;

    Vector3 _rayDirection;

    public static bool IsDestroy
    {
        get { return _isDestroy; }
        set { _isDestroy = value; }
    }

    public Item GetSlotItem
    {
        get { return _slotItem; }
    }

    public Weapon GetWeaponItem
    {
        get { return _slotWeapon; }
    }

    public Vector3 GetRayDirection
    {
        get { return _rayDirection; }
    }

    public float GetRayCastTrans
    {
        get { return _rayCastObjectPosY; }
    }

    public int GetTreeLife
    {
        get { return _treeLife; }
    }

    void Awake()
    {
        _originalCamPos = transform.localPosition;
        _playerControl = GetComponentInParent<PlayerController>();
        _inventManage = GameObject.Find("InventoryBack").GetComponent<InventoryManager>();
        _weaponInvent = GameObject.Find("WeaponInventory").GetComponent<WeaponInventory>();
        _sunLight = GameObject.Find("Directional Light").GetComponent<LightManager>();
        _spotLight = GetComponentInChildren<Light>();
        _spotLight.enabled = false;
    }

    void Start()
    {
        _bloodScreen.enabled = false;
        _blackScreen.enabled = false;
        _mapManage = GameObject.Find("MapSceneManager").GetComponent<MapManager>();
    }

    void Update()
    {
        RaycastHit rHit;

        Ray rayCustom = new Ray(transform.position, transform.forward * _distance);
        Debug.DrawRay(rayCustom.origin, rayCustom.direction * _distance, Color.yellow);

        _rayDirection = rayCustom.direction * _distance;

        int lMask = (1 << LayerMask.NameToLayer("ITEM")) | (1 << LayerMask.NameToLayer("WEAPON")) | (1 << LayerMask.NameToLayer("TREE")) | (1 << LayerMask.NameToLayer("Door")) | (1 << LayerMask.NameToLayer("Drawer"));
        if (Physics.Raycast(rayCustom, out rHit, _distance, lMask))
        {
            CrossHair.GazeStatus = true;
            if (rHit.collider.gameObject.layer == LayerMask.NameToLayer("ITEM"))
            {
                _isItem = true;
                _slotItem = rHit.collider.gameObject.GetComponent<ItemPickUp>()._item;
                _rayCastObjectPosY = rHit.collider.gameObject.transform.position.y;
                if (_playerControl.PickUp)
                {
                    if (_slotItem._itemNum == 104)
                    {
                        _mapManage.QFenceItem.Enqueue(_slotItem);
                    }

                    Destroy(rHit.collider.gameObject);

                    if (_slotItem._itemNum != 102)
                        StartCoroutine(_inventManage.MakeItemSlot());
                    else
                        _isFlashLight = true;
                    _isDestroy = true;
                    _playerControl.PickUp = false;
                }
            }
            else
                _isItem = false;

            if (rHit.collider.gameObject.layer == LayerMask.NameToLayer("WEAPON"))
            {
                //_isItem = false;
                _isWeapon = true;
                _slotWeapon = rHit.collider.gameObject.GetComponent<WeaponInfo>()._weapon;
                _rayCastObjectPosY = rHit.collider.gameObject.transform.position.y;
                if (_playerControl.PickUp)
                {
                    Destroy(rHit.collider.gameObject);
                    if (_slotWeapon._weaponNum != 110)
                        StartCoroutine(_weaponInvent.MakeWeaponSlot());
                    if (_slotWeapon._weaponNum == 110 || _slotWeapon._weaponNum == 101)
                    {
                        SoundManager._instance.PlayEffectSound(PublicDefine.eEffectSoundType.Have_BulletEmpty);
                        _isGunCharge = true;
                    }
                    if (_slotWeapon._weaponNum == 102)
                        _isRiffleCharge = true;
                    _isDestroy = true;
                    _playerControl.PickUp = false;
                }
            }
            else
                _isWeapon = false;

            if (rHit.collider.gameObject.layer == LayerMask.NameToLayer("TREE"))
            {
                _treeCtrl = rHit.collider.gameObject.GetComponent<TreeControl>();
                _treeLife = _treeCtrl.GetLife;
                _isTree = true;
            }
            else
                _isTree = false;

            if (rHit.collider.gameObject.layer == LayerMask.NameToLayer("Door"))
            {
                _isDoor = true;
                Quaternion objectRot = rHit.collider.gameObject.transform.rotation;
                Quaternion targetRot;
                if (_prevDoorObject != rHit.collider.gameObject)
                    _isOpen = false;
                else
                    _isOpen = true;
                if (!_isOpen)
                {
                    if (rHit.collider.gameObject.CompareTag("LeftDoor"))
                        targetRot = Quaternion.Euler(new Vector3(0, 120, 0));
                    else
                        targetRot = Quaternion.Euler(new Vector3(0, -120, 0));
                }
                else
                {
                    if (rHit.collider.gameObject.CompareTag("LeftDoor"))
                        targetRot = Quaternion.Euler(new Vector3(0, -120, 0));
                    else
                        targetRot = Quaternion.Euler(new Vector3(0, 120, 0));
                }
                if (_playerControl._isDoorOpen)
                {
                    _isOpen = !_isOpen;
                    _prevDoorObject = rHit.collider.gameObject;
                    rHit.collider.gameObject.transform.rotation = objectRot * targetRot;
                    _playerControl._isDoorOpen = false;
                }
            }
            else
                _isDoor = false;

            if (rHit.collider.gameObject.layer == LayerMask.NameToLayer("Drawer"))
            {
                _isDrawer = true;
                if (_prevDrawerObject != rHit.collider.gameObject)
                    _isDrawerOpen = false;
                else
                    _isDrawerOpen = true;
                if (_playerControl._isDoorOpen && !_isDrawerOpen)
                {
                    _isDrawerOpen = !_isDrawerOpen;
                    if(!_isDrawerOpen)
                    {
                        rHit.collider.gameObject.transform.position += new Vector3(0, 0, -0.4f);
                        _prevDrawerObject = rHit.collider.gameObject;
                        _playerControl._isDoorOpen = false;
                    }
                    if(_isDrawerOpen)
                    {
                        rHit.collider.gameObject.transform.position += new Vector3(0, 0, 0.4f);
                        _prevDrawerObject = rHit.collider.gameObject;
                        _playerControl._isDoorOpen = false;
                    }
                }
            }
            else
                _isDrawer = false;
        }

        else
        {
            CrossHair.GazeStatus = false;
        }

        if(_isFlashLight)
        {
            if(_playerControl.currentLocation == PublicDefine.eLocation.InDoor)
                _spotLight.enabled = true;

            if (_playerControl.currentLocation != PublicDefine.eLocation.InDoor && _sunLight.IsMorning)
                _spotLight.enabled = false;

            if (!_sunLight.IsMorning)
                _spotLight.enabled = true;
        }
    }

    public IEnumerator ShakeScreen(float amount, float duration)
    {
        float timer = 0;
        while(timer <= duration)
        {
            _bloodScreen.enabled = true;
            transform.localPosition = (Vector3)Random.insideUnitCircle * amount + _originalCamPos;

            timer += Time.deltaTime;
            yield return null;
        }

        transform.localPosition = _originalCamPos;
        _bloodScreen.enabled = false;
    }

    public void ShowBlackScreen()
    {
        _blackScreen.enabled = true;
    }
}
