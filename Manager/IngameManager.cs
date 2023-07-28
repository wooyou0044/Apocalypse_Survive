using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IngameManager : MonoBehaviour
{
    [SerializeField] Text _nicknameText;
    [SerializeField] GameObject _settingWindow;
    [SerializeField] GameObject _manualWindow;
    [SerializeField] GameObject _prefabClue;
    [SerializeField] Slider _moistureSlider;
    [SerializeField] Slider _hpSlider;
    [SerializeField] GameObject _prefabTipWindow;
    [SerializeField] Transform _clueTransform;
    [SerializeField] GameObject _prefabCombineWindow;
    [SerializeField] Transform _combineWindPos;
    [SerializeField] Text _temperatureText;

    [SerializeField] GameObject _bulletCountBox;
    [SerializeField] Image _locIcon;

    Canvas _canvas;
    SettingManager _wndSetting;
    ManualMenu _wndManual;
    UserInfo _userInfo;
    ClueController _clueControl;
    LightManager _lightManager;
    TipWindow _tipWindow;
    CombineManager _combineManage;
    MapManager _mapManage;
    InventoryManager _inventManage;
    Camera _bankCamera;
    Camera _clearCamera;
    Camera _mainCam;
    AudioListener _cameraListen;

    BankManager _bankManage;

    bool _isPlay;
    bool _isCombineOpen;
    bool _isMapOpen;

    public bool _isInLockUI;
    public bool _isGetGold;
    public bool _isCloseDoor;

    int _nightCount;
    int _mornCount;

    int _count;

    GameObject go = null;
    GameObject _combineWind = null;

    Text _maxBulletCount;
    Text _remainBulletCount;

    public bool CombineOpen
    {
        get { return _isCombineOpen; }
    }


    // 임시
    PlayerController _playerCtrl;
    bool _isControl;


    void Awake()
    {
        _canvas = GameObject.Find("Canvas").GetComponent<Canvas>();
        _userInfo = GameObject.Find("UserInfo").GetComponent<UserInfo>();
        _nicknameText.text = _userInfo._nickName;
        _lightManager = GameObject.Find("Directional Light").GetComponent<LightManager>();
        _inventManage = GameObject.Find("InventoryBack").GetComponent<InventoryManager>();

        _bankCamera = GameObject.FindGameObjectWithTag("BankCamera").GetComponent<Camera>();
        _bankCamera.gameObject.SetActive(false);
        _clearCamera = GameObject.Find("ClearLocCamera").GetComponent<Camera>();
        _clearCamera.enabled = false;

        _maxBulletCount = _bulletCountBox.transform.GetChild(0).GetComponentInChildren<Text>();
        _remainBulletCount = _bulletCountBox.transform.GetChild(1).GetComponentInChildren<Text>();
        _bulletCountBox.SetActive(false);

        _cameraListen = Camera.main.GetComponent<AudioListener>();
        _mainCam = Camera.main.GetComponent<Camera>();

        _locIcon.gameObject.SetActive(false);
    }

    void Update()
    {
        _hpSlider.value = _userInfo._hp;
        _moistureSlider.value = _userInfo._moisture;
        _temperatureText.text = _userInfo._temperature.ToString();
        if (_userInfo._hp > 500)
            _userInfo._hp = 500;
        if (_userInfo._moisture > 24)
            _userInfo._moisture = 24;
        if (_userInfo._temperature > 36.5f)
            _userInfo._temperature = 36.5f;

        SetTipWindow();

        CombineWindow();

        if (_lightManager._isNight)
        {
            _mornCount = 0;
            _nightCount++;
            if (_nightCount == 1)
                SoundManager._instance.PlayBGMSound(PublicDefine.eBGMType.MapScene_Night);
        }

        else
        {
            _nightCount = 0;
            _mornCount++;
            if (_mornCount == 1)
                SoundManager._instance.PlayBGMSound(PublicDefine.eBGMType.MapScene_Morning);
        }

        // 임시
        if (PlayerController._isPlayerActivated && !_isControl)
        {
            _playerCtrl = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
            _isControl = true;
        }

        if(_isGetGold)
        {
            GetGoldReturnToMap();
            _isMapOpen = true;
            _isGetGold = false;
        }

        if(_isMapOpen)
        {
            if (Input.GetKeyDown(KeyCode.Z))
                _bankManage.OpenClearMap(false);
        }
    }

    public void OpenSettingWindow()
    {
        SoundManager._instance.PlayEffectSound(PublicDefine.eEffectSoundType.Button);
        if (_wndSetting == null)
        {
            GameObject go = Instantiate(_settingWindow, _canvas.transform);
            _wndSetting = go.GetComponent<SettingManager>();
        }
        else
            _wndSetting.gameObject.SetActive(!_wndSetting.gameObject.activeSelf);
    }

    public void OpenManualWindow()
    {
        SoundManager._instance.PlayEffectSound(PublicDefine.eEffectSoundType.ManualButton);
        if (_wndManual == null)
        {
            GameObject go = Instantiate(_manualWindow, _canvas.transform);
            _wndManual = go.GetComponent<ManualMenu>();
        }
        else
            _wndManual.gameObject.SetActive(!_wndManual.gameObject.activeSelf);
    }

    // 단서를 집아서 여는 키를 누르면 CreateClue 생성됨
    public void CreateClue()
    {
        if (_clueControl == null)
        {
            GameObject go = Instantiate(_prefabClue, _clueTransform.transform);
            _clueControl = go.GetComponent<ClueController>();
        }
        else
            _clueControl.gameObject.SetActive(true);
    }

    // 은행으로 들어가면 BankScene으로 이동
    public void StartBankScene()
    {
        _isInLockUI = true;

        _cameraListen.enabled = true;
        _mainCam.enabled = false;

        _bankCamera.gameObject.SetActive(true);
        _bankManage = GameObject.Find("BankManagerObject").GetComponent<BankManager>();
        _bankManage.MakeLockButton();
        _canvas.gameObject.SetActive(false);
        _playerCtrl.SetActiveCam(false);
    }

    void SetTipWindow()
    {
        if (CrossHair.GazeStatus)
        {
            if (_tipWindow == null)
            {
                go = Instantiate(_prefabTipWindow, _canvas.transform);
                _tipWindow = _prefabTipWindow.GetComponent<TipWindow>();
            }
            else
                go.SetActive(true);
        }
        if(_mapManage!=null)
        {
            if (_mapManage.IsMakePos)
                go.SetActive(true);
        }
    }

    void CombineWindow()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            if (_count <= 0)
            {
                OpenCombineWindow();
                _mapManage = GameObject.Find("MapSceneManager").GetComponent<MapManager>();
                _isCombineOpen = true;
                _inventManage.OpenInventorySlot();
                _count++;
            }
            else
            {
                CloseCombineWindow();
            }
        }
    }

    void OpenCombineWindow()
    {
        if (_combineManage == null)
        {
            _combineWind = Instantiate(_prefabCombineWindow, _combineWindPos.transform);
            _combineManage = _combineWind.GetComponent<CombineManager>();
        }
        else
        {
            _combineWind.SetActive(true);
        }
    }

    public void CloseCombineWindow()
    {
        _isCombineOpen = false;
        _combineWind.SetActive(false);
        _inventManage.CloseInventorySlot();
        _combineManage.DeleteSlot();
        _count = 0;
    }

    public void OpenBulletWindow()
    {
        _bulletCountBox.SetActive(true);
    }

    public void SetBulletCount(int maxCount, int storageCount)
    {
        _maxBulletCount.text = maxCount.ToString();
        _remainBulletCount.text = storageCount.ToString();
    }

    public void CloseBulletWindow()
    {
        _bulletCountBox.SetActive(false);
    }

    void GetGoldReturnToMap()
    {
        _bankManage.ReturnMap();

        _mainCam.enabled = true;
        _cameraListen.enabled = false;

        _playerCtrl.SetActiveCam(true);
        _canvas.gameObject.SetActive(true);

        // 은행은 금 획득하면 Box Collider 끄기
        _isCloseDoor = true;
        _locIcon.gameObject.SetActive(true);

        _isInLockUI = false;
    }
    
    public void GoToMapInUI()
    {
        _bankManage.DestroyLockUI();
        _mainCam.enabled = true;
        _cameraListen.enabled = false;

        _bankCamera.gameObject.SetActive(false);

        _canvas.gameObject.SetActive(true);
        _playerCtrl.SetActiveCam(true);

        _isInLockUI = false;
    }

    public void ClickLocationIcon()
    {
        _bankManage.OpenClearMap(true);
    }

    public void GetGoldSetMap()
    {
        if(_mapManage == null)
            _mapManage = GameObject.Find("MapSceneManager").GetComponent<MapManager>();
        _mapManage.ClearMap();
        _clearCamera.enabled = true;
        Vector3 pos = new Vector3(_mapManage.ClearPos.x, 0, _mapManage.ClearPos.z);
        _clearCamera.transform.position += pos;
    }

    public void OffCanvas(bool isOn)
    {
        _canvas.gameObject.SetActive(isOn);
    }
}
