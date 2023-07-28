using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static bool _isPlayerActivated = false;

    [SerializeField] float _moveSpeed = 2.0f;
    [SerializeField] float _rotateSpeed = 2.0f;
    [SerializeField] float _jumpSpeed = 3.0f;
    [SerializeField] float _changeDelayTime = 1.5f;
    [SerializeField] Transform _weaponPos;
    [SerializeField] Transform _targetPos;
    [SerializeField] GameObject[] _weapon;
    [SerializeField] Transform _headPos;
    [SerializeField] BoxCollider _damageZone;

    [SerializeField] Transform _crossHairTrans;

    EyeCast _eyeCast;
    Camera _playerCam;
    Animator _aniCtrl;
    RuntimeAnimatorController _runAniControl;
    Zombie1Object _zombie1;
    Zombie2Object _zombie2;
    WeaponController _weaponCtrl;
    RiffleController _riffleCtrl;
    GrenadeController _grenadeCtrl;
    WeaponInventory _weaponInvent;
    AudioSource _voiceClip;

    CharacterController _charCtrl;
    PublicDefine.eActionState _currentState;
    PublicDefine.eWeaponKind _currentWeapon;
    PublicDefine.ePickUP_Location _pickLocation;
    PublicDefine.eLocation _eLocation;
    PublicDefine.eBuildingType _eCurrentBuilding;

    UserInfo _userInfo;
    Weapon _cWeapon;
    WeaponInfo _weaponInfo;

    Vector3 _pickUpObjectPos;
    Vector3 _move;
    Vector3 _rotate;
    Vector3 _pos;
    Vector3 _originPos;
    Quaternion _originRot;
    Vector3 _prevPos;
    Transform _hospPlayerPos;
    Transform _cameraPos;

    float _mouseX;
    float _mouseY;
    float _posY;
    float _timeCheck;
    float _reloadTime;
    float _reloadEffTime;
    float _posX;
    float _posZ;

    // 움직이는 애니메이션
    bool _isRunBack = false;
    bool _isWalkLeft = false;
    bool _isWalkRight = false;
    bool _isChanging = false;
    bool _isAimed = false;
    bool _isReload = false;
    bool _isInRange = false;

    // 죽는 애니메이션
    bool _isDead = false;
    bool _isPickup = false;
    bool _isKnockDown = false;
    bool _isntFirstMake = false;
    bool _isClear = false;

    Dictionary<int, Weapon> _dicWeapon;
    List<GameObject> _ltWeapon;

    int _weaponNumber = 0;
    int _hitPower = 0;
    int _weaponDamageNum;
    int _gunPickNum = 0;
    int _rifflePickNum = 0;
    int _maxBulletCount = 0;

    LightManager _lightManager;
    MapManager _mapManager;
    SceneControlManager _sceneControl;
    IngameManager _ingameManage;

    GameObject _oWeapon;

    public bool _isShootGrenade;
    public bool _isDoorOpen;

    public bool PickUp
    {
        get { return _isPickup; }
        set { _isPickup = value; }
    }

    public int WeaponNum
    {
        get { return (int)_currentWeapon; }
    }

    public int WeaponDamageNum
    {
        get { return _weaponDamageNum; }
    }

    public Vector3 GetPlayerPos
    {
        get { return _move; }
    }

    public Vector3 GetPlayerRotate
    {
        get { return _rotate; }
    }

    public Dictionary<int, Weapon> DicWeapon
    {
        get { return _dicWeapon; }
        set { _dicWeapon = value; }
    }

    public int WeaponNumber
    {
        get { return _weaponNumber; }
        set { _weaponNumber = value; }
    }

    public PublicDefine.eLocation currentLocation
    {
        get { return _eLocation; }
    }
    
    public PublicDefine.eBuildingType currnetBuilding
    {
        get { return _eCurrentBuilding; }
    }

    void Awake()
    {
        _dicWeapon = new Dictionary<int, Weapon>();
        _ltWeapon = new List<GameObject>();

        _playerCam = GetComponentInChildren<Camera>();
        _aniCtrl = GetComponent<Animator>();
        _charCtrl = GetComponent<CharacterController>();
        _eyeCast = GetComponentInChildren<EyeCast>();
        _cameraPos = _eyeCast.gameObject.transform;
        _voiceClip = GetComponent<AudioSource>();

        _userInfo = GameObject.Find("UserInfo").GetComponent<UserInfo>();
        _lightManager = GameObject.Find("Directional Light").GetComponent<LightManager>();
        _mapManager = GameObject.Find("MapSceneManager").GetComponent<MapManager>();
        _sceneControl = GameObject.Find("SceneManagerObject").GetComponent<SceneControlManager>();
        _ingameManage = GameObject.Find("IngameManagerObject").GetComponent<IngameManager>();
        _weaponInvent = GameObject.Find("WeaponInventory").GetComponent<WeaponInventory>();

        _hospPlayerPos = GameObject.Find("HospitalPlayerPos").transform;

        InitWeapon();

        _isPlayerActivated = true;

        _originPos = _weaponPos.localPosition;
        _originRot = _weaponPos.localRotation;

        _damageZone.enabled = false;
        _prevPos = Vector3.zero;

        _eLocation = PublicDefine.eLocation.OutDoor;
        _eCurrentBuilding = PublicDefine.eBuildingType.Map;
    }

    void Start()
    {
        //_playerHP = _userInfo._hp;

        //_isPlayerActivated = true;

        // 총이나 라이플이 아니면 Reload => 고친지 얼마 안 됨
        if (_currentWeapon == PublicDefine.eWeaponKind.GUN)
        {
            _ingameManage.OpenBulletWindow();
            _weaponCtrl = _oWeapon.GetComponent<WeaponController>();
            _weaponCtrl.ReloadBullet();
        }

        if (_currentWeapon == PublicDefine.eWeaponKind.RIFFLE)
        {
            _ingameManage.OpenBulletWindow();
            _riffleCtrl = _oWeapon.GetComponent<RiffleController>();
            _riffleCtrl.ReloadBullet();

        }

        if (_currentWeapon == PublicDefine.eWeaponKind.GRENADE)
        {
            _grenadeCtrl = _oWeapon.GetComponent<GrenadeController>();
        }
    }

    void Update()
    {
        if (_isDead)
        {
            _playerCam.transform.position = Vector3.MoveTowards(_playerCam.transform.position, _headPos.position + new Vector3(0, 0.2f, 0), 0.5f);
            _playerCam.transform.rotation = Quaternion.Euler(new Vector3(_headPos.rotation.x - 10, _headPos.rotation.y, _headPos.rotation.z));
            _timeCheck += Time.deltaTime;
            if (_timeCheck >= 3.5f)
            {
                _eyeCast.ShowBlackScreen();
                _playerCam.GetComponent<AudioListener>().enabled = false;
                AudioListener cameraListen = Camera.main.GetComponent<AudioListener>();
                if (!cameraListen.enabled)
                    cameraListen.enabled = true;
            }
            if (_timeCheck >= 5.0f)
            {
                _sceneControl.BackLobbyScene();
                SoundManager._instance.PlayBGMSound(PublicDefine.eBGMType.UIScene);
                _timeCheck = 0;
            }

            return;
        }

        if (!InventoryManager._inventoryActivated && !WeaponInventory._weaponActivated && !_isDead && !_isReload && !_ingameManage._isInLockUI)
        {
            Move();
            Rotate();
            _posX = _playerCam.transform.localPosition.x;
            _posZ = _playerCam.transform.localPosition.z;

            if(_headPos.position.y > 1.7f)
                _playerCam.transform.localPosition = new Vector3(_posX, _headPos.position.y - transform.position.y, _posZ);
            else
                _playerCam.transform.localPosition = new Vector3(_posX, _headPos.position.y, _posZ);

            //Debug.Log(_headPos.position.y);
            //_playerCam.transform.localPosition = new Vector3(posX, _headPos.position.y, posZ);

            // 은행 비밀번호 누를때는 공격 안하기
            if (_currentWeapon == PublicDefine.eWeaponKind.GUN || _currentWeapon == PublicDefine.eWeaponKind.RIFFLE)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    _weaponDamageNum = _cWeapon._damageNum;
                    AttackGunNRiffle(_currentWeapon);
                }

                if (!MapManager._isCreateActivated)
                {
                    if (Input.GetMouseButtonDown(1))
                    {
                        ChangeAnimation(PublicDefine.eActionState.AIMING);
                        AimTarget();
                    }
                }
            }
            else
            {
                if (Input.GetMouseButtonDown(0))
                    Attack(_currentWeapon);
            }
        }
        else
        {
            _playerCam.transform.localPosition = new Vector3(_posX, _headPos.position.y - transform.position.y, _posZ);
        }
        PickUpSomething();

        _move = transform.position;
        _rotate = transform.localEulerAngles;

        // 불이 없으면 온도가 떨어지게 한다.
        if (_lightManager._isNight && _eLocation == PublicDefine.eLocation.OutDoor)
        {
            if (!_mapManager.IsMakeFire)
            {
                if (_lightManager.IsOneHour)
                    _userInfo._temperature -= 0.1f;
            }
            else
                _userInfo._temperature = 36.5f;
        }
        if (!_lightManager._isNight)
            _mapManager.IsMakeFire = false;

        // 잠자는 시간이 5시간 미만이면 HP 50 감소 이상이면 HP 50 증가
        if (_lightManager.IsOneHour)
        {
            _userInfo._moisture--;
            _lightManager.IsOneHour = false;
        }

        // 수분이 0이면 게임 종료
        if (_userInfo._hp <= 0 || _userInfo._moisture <= 0 || _userInfo._temperature <= 35.5f)
        {
            ChangeAnimation(PublicDefine.eActionState.DIE);
            _isDead = true;
        }

        if (!_isChanging)
            ChangeWeapon();

        if (_isDead && !_isKnockDown)
        {
            PlayVoice(PublicDefine.eVoiceType.Player_Dead);
            _isKnockDown = true;
        }

        if (_isClear)
        {
            _sceneControl.BackLobbyScene();
            SoundManager._instance.PlayBGMSound(PublicDefine.eBGMType.UIScene);
            _isClear = false;
        }
    }

    void LateUpdate()
    {
        // 총탄 개수는 총을 주어야 Reload 가능
        if (_weaponCtrl != null)
        {
            // 가지고 있는 총알의 개수
            if (_weaponCtrl.RemainNum <= 0 && _weaponCtrl.RemainBulletCount > 0)
            {
                _reloadTime += Time.deltaTime;
                _reloadEffTime += Time.deltaTime;
                _isReload = true;
                _aniCtrl.SetBool("isReload", _isReload);
                if (_reloadEffTime >= 0.1f)
                {
                    SoundManager._instance.PlayEffectSound(PublicDefine.eEffectSoundType.Reload);
                    _reloadEffTime = 0;
                }
                if (_reloadTime >= 2f)
                {
                    _weaponCtrl.ReloadBullet();
                    _isReload = false;
                    _aniCtrl.SetBool("isReload", _isReload);
                    _reloadTime = 0;
                }
            }
        }

        if (_eyeCast._isGunCharge)
        {
            if (_currentWeapon == PublicDefine.eWeaponKind.GUN)
                _weaponCtrl.ChargeBullet(1);
            else
                _gunPickNum++;
            _eyeCast._isGunCharge = false;
        }

        if (_riffleCtrl != null)
        {
            if (_isAimed)
            {
                if (_riffleCtrl.IsInRange(_crossHairTrans))
                    _isInRange = true;
                else
                    _isInRange = false;
            }
            else
            {
                if (_riffleCtrl.IsInRange())
                    _isInRange = true;
                else
                    _isInRange = false;
            }

            if (_riffleCtrl.RemainNum <= 0 && _riffleCtrl.RemainBulletCount > 0)
            {
                _reloadTime += Time.deltaTime;
                _reloadEffTime += Time.deltaTime;
                _isReload = true;
                _aniCtrl.SetBool("isReload", _isReload);
                if (_reloadEffTime >= 1f)
                {
                    SoundManager._instance.PlayEffectSound(PublicDefine.eEffectSoundType.Reload);
                    _reloadEffTime = 0;
                }
                if (_reloadTime >= 3.2f)
                {
                    _riffleCtrl.ReloadBullet();
                    _isReload = false;
                    _aniCtrl.SetBool("isReload", _isReload);
                    _reloadTime = 0;
                }
            }
        }

        if (_eyeCast._isRiffleCharge)
        {
            if (_currentWeapon == PublicDefine.eWeaponKind.RIFFLE)
                _riffleCtrl.ChargeBullet(1);
            else
                _rifflePickNum++;
            _eyeCast._isRiffleCharge = false;
        }

        if (_eyeCast._isDoor || _eyeCast._isDrawer)
        {
            if (Input.GetKeyDown(KeyCode.Q))
            {
                // 문 열기
                _isDoorOpen = true;
            }
        }
    }

    void InitWeapon()
    {
        _currentWeapon = _userInfo._subWeapon;
        _oWeapon = Instantiate(_weapon[(int)_currentWeapon - 1], _weaponPos);
        _weaponInfo = _oWeapon.GetComponent<WeaponInfo>();

        _cWeapon = _weaponInfo._weapon;
        //_ltWeapon.Add(_oWeapon);
        _weaponNumber++;
        _dicWeapon.Add(_weaponNumber, _cWeapon);
        if (_currentWeapon != PublicDefine.eWeaponKind.GRENADE)
        {
            _ltWeapon.Add(_oWeapon);
            _oWeapon.SetActive(false);
        }
        else
            Destroy(_oWeapon);

        _currentWeapon = _userInfo._baseWeapon;
        _oWeapon = Instantiate(_weapon[(int)_currentWeapon - 1], _weaponPos);
        _weaponInfo = _oWeapon.GetComponent<WeaponInfo>();

        _cWeapon = _weaponInfo._weapon;
        _ltWeapon.Add(_oWeapon);
        _weaponNumber++;
        _dicWeapon.Add(_weaponNumber, _cWeapon);
    }

    void ChangeAnimation(PublicDefine.eActionState state)
    {
        switch (state)
        {
            case PublicDefine.eActionState.IDLE:
                _aniCtrl.SetInteger("Weapon_Num", (int)_currentWeapon);
                _damageZone.enabled = false;
                break;
            case PublicDefine.eActionState.MOVE:
                _aniCtrl.SetInteger("Weapon_Num", (int)_currentWeapon);
                _aniCtrl.SetBool("isWalkLeft", _isWalkLeft);
                _aniCtrl.SetBool("isWalkRight", _isWalkRight);
                _aniCtrl.SetBool("isRunBack", _isRunBack);
                _aniCtrl.SetBool("isAimed", _isAimed);
                //if (_isAimed)
                //    AimTarget();
                _damageZone.enabled = false;
                break;
            case PublicDefine.eActionState.AIMING:
                _aniCtrl.SetInteger("Weapon_Num", (int)_currentWeapon);

                _damageZone.enabled = false;
                break;
            case PublicDefine.eActionState.ATTACK:
                _aniCtrl.SetInteger("Weapon_Num", (int)_currentWeapon);
                break;
            case PublicDefine.eActionState.PICKUP:
                PlayerPickLocation(_eyeCast.GetRayCastTrans);
                _aniCtrl.SetInteger("PickUp_Location", (int)_pickLocation);
                WeaponActive(false, _currentWeapon);
                //_oWeapon.SetActive(false);
                PlayVoice(PublicDefine.eVoiceType.Player_PickUp);
                _damageZone.enabled = false;
                break;
            case PublicDefine.eActionState.JUMP:
                PlayVoice(PublicDefine.eVoiceType.Player_JUMP);
                _damageZone.enabled = false;
                break;
            case PublicDefine.eActionState.DIE:
                _aniCtrl.SetTrigger("Dead");
                _damageZone.enabled = false;
                GetComponent<CharacterController>().enabled = false;
                break;
        }
        _aniCtrl.SetInteger("ActionState", (int)state);
        _currentState = state;
    }

    void Move()
    {
        _isRunBack = false;
        _isWalkLeft = false;
        _isWalkRight = false;
        ChangeAnimation(PublicDefine.eActionState.IDLE);

        float _moveX = Input.GetAxisRaw("Horizontal");
        float _moveZ = Input.GetAxisRaw("Vertical");

        _pos = new Vector3(_moveX, 0, _moveZ);
        _pos = transform.TransformDirection(_pos);
        if (_moveX > 0)
        {
            _isWalkLeft = true;
            ChangeAnimation(PublicDefine.eActionState.MOVE);
        }
        if (_moveX < 0)
        {
            _isWalkRight = true;
            ChangeAnimation(PublicDefine.eActionState.MOVE);
        }
        if (_moveZ > 0)
            ChangeAnimation(PublicDefine.eActionState.MOVE);
        if (_moveZ < 0)
        {
            _isRunBack = true;
            ChangeAnimation(PublicDefine.eActionState.MOVE);
        }
        if (Input.GetButtonDown("Jump"))
            Jump();
        _charCtrl.Move(_pos * _moveSpeed * Time.deltaTime);
    }

    void Rotate()
    {
        _mouseX += Input.GetAxis("Mouse X") * _rotateSpeed;
        transform.localEulerAngles = new Vector3(0, _mouseX, 0);

        _mouseY += Input.GetAxis("Mouse Y") * _rotateSpeed;
        _mouseY = Mathf.Clamp(_mouseY, -60.0f, 60.0f);
        _playerCam.transform.localEulerAngles = new Vector3(-_mouseY, 0, 0);

    }

    void Jump()
    {
        ChangeAnimation(PublicDefine.eActionState.JUMP);
        _pos.y = _jumpSpeed;
    }

    void PickUpSomething()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (CrossHair.GazeStatus)
            {
                ChangeAnimation(PublicDefine.eActionState.PICKUP);
                StartCoroutine(PickUpItem());
            }
        }
    }

    IEnumerator PickUpItem()
    {
        float _camPosY = _playerCam.transform.position.y;
        int num = (int)_aniCtrl.GetCurrentAnimatorStateInfo(0).normalizedTime;
        float time = _aniCtrl.GetCurrentAnimatorStateInfo(0).normalizedTime - num;
        while (time <= 1.0f)
        {
            _isPickup = false;
            if (_aniCtrl.GetCurrentAnimatorStateInfo(0).normalizedTime - num < 0)
            {
                if (_aniCtrl.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.65f)
                {
                    _isPickup = true;
                    yield return new WaitForSeconds(1f);
                    WeaponActive(true, _currentWeapon);
                    break;
                }
            }
            if (num == 0)
            {
                if (_aniCtrl.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.65f)
                {
                    _isPickup = true;
                    yield return new WaitForSeconds(1f);
                    WeaponActive(true, _currentWeapon);
                    break;
                }
            }
            yield return null;
        }
    }

    void Attack(PublicDefine.eWeaponKind weapon)
    {
        ChangeAnimation(PublicDefine.eActionState.ATTACK);
        _weaponDamageNum = _cWeapon._damageNum;
        Debug.Log("weapon damage number : " + _weaponDamageNum);
        switch (weapon)
        {
            case PublicDefine.eWeaponKind.NONE:
                _damageZone.enabled = false;
                break;
            case PublicDefine.eWeaponKind.GRENADE:
                _damageZone.enabled = false;
                break;
            case PublicDefine.eWeaponKind.KNIFE:
            case PublicDefine.eWeaponKind.AXE:
            case PublicDefine.eWeaponKind.HAMMER:
            case PublicDefine.eWeaponKind.BAT:
                SoundManager._instance.PlayEffectSound(PublicDefine.eEffectSoundType.Attack_BatNKnifeNAxeNHammer);
                PlayVoice(PublicDefine.eVoiceType.Player_Attack);
                _damageZone.enabled = true;
                break;
        }
        _aniCtrl.SetInteger("Weapon_Num", (int)weapon);
    }

    void AttackGunNRiffle(PublicDefine.eWeaponKind weapon)
    {
        switch (weapon)
        {
            case PublicDefine.eWeaponKind.GUN:
                if (_weaponCtrl.RemainNum > 0)
                {
                    if (_isAimed)
                        _weaponCtrl.ShootAimedBullet(_crossHairTrans);
                    else
                    {
                        _aniCtrl.SetTrigger("Shoot");
                        _weaponCtrl.ShootBullet();
                    }
                    ChangeAnimation(PublicDefine.eActionState.AIMING);
                }
                break;
            case PublicDefine.eWeaponKind.RIFFLE:
                if (_riffleCtrl.RemainNum > 0)
                {
                    if (_isInRange)
                        _riffleCtrl.SendMessageEnemy();
                    _riffleCtrl.ShootBullet();
                    _aniCtrl.SetTrigger("Shoot");
                    ChangeAnimation(PublicDefine.eActionState.AIMING);
                }
                break;
        }
    }

    void ChangeWeapon()
    {
        //KeyCode.Alpha0 == 48
        //if(Input.GetKeyDown((KeyCode)48)
        foreach (int key in _dicWeapon.Keys)
        {
            if (Input.GetKeyDown((KeyCode)48 + key))
            {
                _isChanging = true;
                _oWeapon.SetActive(false);
                // _dicWeapon[key]에 Weapon이 적용된 오브젝트 생성/SetActive(true)
                _currentWeapon = _dicWeapon[key]._weaponType;
                _cWeapon = _dicWeapon[key];

                if (_currentWeapon != PublicDefine.eWeaponKind.GRENADE)
                    StartCoroutine(SwitchWeapon());
                else
                {
                    _ingameManage.CloseBulletWindow();
                    _isChanging = false;
                }
            }
        }
        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            _currentWeapon = PublicDefine.eWeaponKind.NONE;
            _oWeapon.SetActive(false);
            _ingameManage.CloseBulletWindow();
        }
    }

    IEnumerator SwitchWeapon()
    {
        yield return new WaitForSeconds(_changeDelayTime);
        // 생성된 적이 있으면 SetActive(true) / 생성된 적이 없으면 Instantiate
        if (!IsWeaponExist(_cWeapon))
        {
            _oWeapon = Instantiate(_cWeapon._weaponPrefab, _weaponPos);
            _ltWeapon.Add(_oWeapon);
            // 아니면 삭제
            if (_currentWeapon == PublicDefine.eWeaponKind.GUN)
            {
                _weaponCtrl = _oWeapon.GetComponent<WeaponController>();
                _weaponCtrl.ChargeRemainBullet();
                _isntFirstMake = false;
            }
            if (_currentWeapon == PublicDefine.eWeaponKind.RIFFLE)
            {
                _riffleCtrl = _oWeapon.GetComponent<RiffleController>();
                _riffleCtrl.ChargeRemainBullet();
                _isntFirstMake = false;
            }
        }
        else
        {
            _oWeapon.SetActive(true);
            if (_currentWeapon == PublicDefine.eWeaponKind.GUN)
                _isntFirstMake = true;
            if (_currentWeapon == PublicDefine.eWeaponKind.RIFFLE)
                _isntFirstMake = true;
        }
        _weaponInfo = _oWeapon.GetComponent<WeaponInfo>();

        if (_currentWeapon == PublicDefine.eWeaponKind.RIFFLE)
        {
            _ingameManage.OpenBulletWindow();
            _riffleCtrl = _oWeapon.GetComponent<RiffleController>();
            if (_isntFirstMake)
                _riffleCtrl.ChargeBullet(_rifflePickNum);
            else
                _riffleCtrl.ChargeBullet(_rifflePickNum - 1);
            _rifflePickNum = 0;
            _isntFirstMake = false;
            _weaponPos.localPosition = Vector3.MoveTowards(_weaponPos.localPosition, _targetPos.localPosition, 0.8f);
            _weaponPos.localRotation = _targetPos.localRotation;
        }
        else
        {
            _ingameManage.CloseBulletWindow();
            _weaponPos.localPosition = Vector3.MoveTowards(_weaponPos.localPosition, _originPos, 0.8f);
            _weaponPos.localRotation = _originRot;
        }

        if (_currentWeapon == PublicDefine.eWeaponKind.GUN)
        {
            _ingameManage.OpenBulletWindow();
            if (_isntFirstMake)
                _weaponCtrl.ChargeBullet(_gunPickNum);
            if (!_isntFirstMake)
                _weaponCtrl.ChargeBullet(_gunPickNum - 1);
            _gunPickNum = 0;
            _isntFirstMake = false;
        }

        if (_isAimed)
            AimTarget();
        _isChanging = false;
    }

    bool IsWeaponExist(Weapon weapon)
    {
        for (int n = 0; n < _ltWeapon.Count; n++)
        {
            WeaponInfo weaponInfo = _ltWeapon[n].GetComponent<WeaponInfo>();
            if (weaponInfo._weapon == weapon)
            {
                _oWeapon = _ltWeapon[n];
                return true;
            }
        }
        return false;
    }

    void PlayerPickLocation(float pickUpObjectPosY)
    {
        if (pickUpObjectPosY <= 0.35f)
            _pickLocation = PublicDefine.ePickUP_Location.LOW;
        else if (pickUpObjectPosY < 2.5f)
            _pickLocation = PublicDefine.ePickUP_Location.MIDDLE;
        else if (pickUpObjectPosY <= 3.5f)
            _pickLocation = PublicDefine.ePickUP_Location.HIGH;
    }

    void AimTarget()
    {
        _isAimed = !_isAimed;
        if (_isAimed)
            _playerCam.fieldOfView = 15;
        else
            _playerCam.fieldOfView = 60;
    }

    void WeaponActive(bool isActive, PublicDefine.eWeaponKind weapon)
    {
        // 혹시 _isChanging 때문에 안 되면 지우기
        if (weapon != PublicDefine.eWeaponKind.NONE && !_isChanging)
            _oWeapon.SetActive(isActive);
    }

    public void AttackGrenade()
    {
        SoundManager._instance.PlayEffectSound(PublicDefine.eEffectSoundType.Throw_Grenade);
        // 스류탄 개수가 0보다 크면 생성되게
        if (!_weaponInvent.IsntExistGrenade())
        {
            GameObject go = Instantiate(_cWeapon._weaponPrefab, _weaponPos.position, _weaponPos.rotation);
            _grenadeCtrl = go.GetComponent<GrenadeController>();
            _grenadeCtrl.Shoot(transform.forward);
            _isShootGrenade = true;
        }
    }

    public void HitDamage(int damage)
    {
        StartCoroutine(_eyeCast.ShakeScreen(0.1f, 0.3f));
        _userInfo._hp -= damage;
        Debug.Log("damage : " + damage);
    }

    void PlayVoice(PublicDefine.eVoiceType voice, bool isMan = true, bool isLoop = false)
    {
        _voiceClip.clip = SoundClipPoolManager._instance.GetPlayerVoice(voice, isMan);
        _voiceClip.loop = isLoop;
        _voiceClip.Play();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("DamageZone"))
        {
            if (other.transform.parent.CompareTag("Enemy1"))
            {
                _zombie1 = other.transform.parent.GetComponent<Zombie1Object>();
                _hitPower = _zombie1.ZombiePowAtt;
                PlayVoice(PublicDefine.eVoiceType.Player_Hit);
            }
            if (other.transform.parent.CompareTag("Enemy2"))
            {
                _zombie2 = other.transform.parent.GetComponent<Zombie2Object>();
                _hitPower = _zombie2.ZombiePowAtt;
                PlayVoice(PublicDefine.eVoiceType.Player_Hit);
            }

            HitDamage(_hitPower);

            // 맞는 소리
        }
    }

    public void SetActiveCam(bool isOn)
    {
        _playerCam.gameObject.SetActive(isOn);
    }

    public void GoToOtherBuilding(PublicDefine.eBuildingType type)
    {
        if (type == PublicDefine.eBuildingType.Hospital)
        {
            Debug.Log("병원");
            _prevPos = transform.position;
            transform.position = _hospPlayerPos.position;
            _lightManager.IsLight(false);
            _eLocation = PublicDefine.eLocation.InDoor;
            _eCurrentBuilding = PublicDefine.eBuildingType.Hospital;
        }
        if (type == PublicDefine.eBuildingType.Bank)
        {
            Debug.Log("은행");
            _ingameManage.StartBankScene();
            _eLocation = PublicDefine.eLocation.InDoor;
            _eCurrentBuilding = PublicDefine.eBuildingType.Bank;
        }
        if (type == PublicDefine.eBuildingType.Map)
        {
            transform.position = _prevPos + new Vector3(0, 0, -2);
            _lightManager.IsLight(true);
            Debug.Log("맵");
            _eLocation = PublicDefine.eLocation.OutDoor;
            _eCurrentBuilding = PublicDefine.eBuildingType.Map;
        }
    }

    public void ClearMapPlayer()
    {
        if (_userInfo._clearMap == 0)
            _userInfo._coin += 2000000;
        _userInfo._clearMap++;
        _userInfo._record = _lightManager.GetDay;

        _playerCam.GetComponent<AudioListener>().enabled = false;
        AudioListener cameraListen = Camera.main.GetComponent<AudioListener>();
        if (!cameraListen.enabled)
            cameraListen.enabled = true;
        _ingameManage.OffCanvas(false);
        _isClear = true;
    }
}
