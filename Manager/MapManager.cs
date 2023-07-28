using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    public static bool _isCreateActivated = false;

    [SerializeField] GameObject _player;
    [SerializeField] GameObject[] _fence;
    //[SerializeField] GameObject[] _fire;
    [SerializeField] GameObject _fire;

    [SerializeField] GameObject _clearMapObject;
    [SerializeField] Transform _clearPos;
    [SerializeField] GameObject _prefabClearParticle;

    Transform _playerPos;
    float _playerRotate;

    CombineManager _combineManage;
    PlayerController _playerControl;
    IngameManager _ingameManage;
    InventoryManager _inventManage;
    LightManager _lightManager;
    EyeCast _eyeCast;
    Item _fenceItem;
    GameObject _fireObject;

    Vector3 _targetPos;
    Queue<Item> _qFenceItem;
    int _fenceNum;

    bool _isMakePos;
    bool _isMakeFire;
    bool _isHaveFire;

    bool _isCreate = false;

    public bool IsMakePos
    {
        get { return _isMakePos; }
    }

    public bool IsMakeFire
    {
        get { return _isMakeFire; }
        set { _isMakeFire = value; }
    }

    public Queue<Item> QFenceItem
    {
        get { return _qFenceItem; }
        set { _qFenceItem = value; }
    }

    public Vector3 ClearPos
    {
        get { return _clearPos.position; }
    }

    void Awake()
    {
        AudioListener _cameraListen = Camera.main.GetComponent<AudioListener>();
        _cameraListen.enabled = false;

        _playerPos = GameObject.Find("PlayerPos").transform;
        GameObject go = Instantiate(_player, _playerPos.transform.position, _playerPos.transform.rotation);
        _playerControl = go.GetComponent<PlayerController>();

        _eyeCast = go.GetComponentInChildren<EyeCast>();
        
        _ingameManage = GameObject.Find("IngameManagerObject").GetComponent<IngameManager>();
        _inventManage = GameObject.Find("InventoryBack").GetComponent<InventoryManager>();
        _lightManager = GameObject.Find("Directional Light").GetComponent<LightManager>();

        _qFenceItem = new Queue<Item>();

    }

    void Update()
    {
        _targetPos = _playerControl.GetPlayerPos + _eyeCast.GetRayDirection * 2;
        _targetPos.y = 0;

        if (_ingameManage.CombineOpen && _combineManage== null)
        {
            _combineManage = GameObject.FindGameObjectWithTag("CombineWindow").GetComponent<CombineManager>();
        }

        if(_combineManage != null)
        {
            if (_combineManage._isCombineFence)
            {
                SetFence(_targetPos, _playerControl.GetPlayerRotate.y);
                if (!_combineManage.IsCombine)
                    _isMakePos = false;
            }

            if(_combineManage._isCombineFire)
            {
                _isMakePos = true;
                if (Input.GetMouseButtonDown(1))
                {
                    _fireObject = Instantiate(_fire, _targetPos, Quaternion.Euler(0, _playerControl.GetPlayerRotate.y - 50f, 0));
                    _combineManage.IsCombine = false;
                    _combineManage._isCombineFire = false;
                    _isMakeFire = true;
                    _isCreate = true;
                    _isHaveFire = true;
                }

                // tipWindow를 위해서
                if (!_combineManage.IsCombine)
                    _isMakePos = false;
            }
        }

        if (_inventManage.IsSurvivalKit)
        {
            if (_inventManage.SurvivalSlot._itemNum == 104)
            {
                _isMakePos = true;
                if (_inventManage.SurvivalSlot._itemPrefab[0] == _fence[0])
                    _playerRotate = _playerControl.GetPlayerRotate.y - 180;
                if (_inventManage.SurvivalSlot._itemPrefab[0] == _fence[1])
                    _playerRotate = _playerControl.GetPlayerRotate.y;
                if (Input.GetMouseButtonDown(1))
                {
                    _fenceItem = _qFenceItem.Dequeue();
                    Instantiate(_fenceItem._itemPrefab[0], _targetPos, Quaternion.Euler(0, _playerRotate, 0));
                    _inventManage.IsSurvivalKit = false;
                    _isMakePos = false;
                    _isCreate = true;
                }
            }
        }

        if(_isCreate)
        {
            _isCreateActivated = true;
            _isCreate = false;
        }
        else
            _isCreateActivated = false;

        if(!_lightManager._isNight)
        {
            if (_isHaveFire)
            {
                Destroy(_fireObject, 2.0f);
                _isHaveFire = false;
            }
        }
    }

    public void SetFence(Vector3 position, float rotation)
    {
        _isMakePos = true;
        int rand = Random.Range(0, _fence.Length);
        if (rand == 0)
            _playerRotate = _playerControl.GetPlayerRotate.y - 180;
        if (rand == 1)
            _playerRotate = _playerControl.GetPlayerRotate.y;
        if (Input.GetMouseButtonDown(1))
        {
            Instantiate(_fence[rand], position, Quaternion.Euler(0, rotation, 0));
            _combineManage._isCombineFence = false;
            _combineManage.IsCombine = false;
            _isCreate = true;
        }
    }

    public void SetFire(Vector3 position, float rotation)
    {
        if (Input.GetMouseButtonDown(1))
        {
            Instantiate(_fire, position, Quaternion.Euler(0, rotation, 0));
            _combineManage.IsCombine = false;
            _isCreate = true;
        }
    }

    public void ClearMap()
    {
        Instantiate(_clearMapObject, _clearPos);
        Instantiate(_prefabClearParticle, _clearPos);
    }
}
