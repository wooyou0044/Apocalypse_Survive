using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BankManager : MonoBehaviour
{
    [SerializeField] GameObject _prefabLock;
    [SerializeField] GameObject _prefabOpenLock;
    [SerializeField] GameObject _prefabGoldParticle;

    [SerializeField] GameObject _prefabClearLocation;

    LockButtonController _lockControl;
    IngameManager _ingameManage;

    Canvas _canvas;
    Image _getImage;
    Text _getText;
    Camera _bankCamera;
    Camera _bankUICamera;

    GameObject _openLock;
    GameObject _goldParticle;
    GameObject _lockUI;
    GameObject _clearMapObject;

    float _time;
    bool _isMakeGold;
    bool _isGetGold;
    bool _isStoreClearMap;

    public bool _isOpen;

    void Awake()
    {
        _canvas = GameObject.Find("BankCanvas").GetComponent<Canvas>();
        _ingameManage = GameObject.Find("IngameManagerObject").GetComponent<IngameManager>();
    }

    void Update()
    {
        if(_isOpen)
        {
            // BankCamera 끄기
            _bankUICamera = GameObject.FindGameObjectWithTag("BankCamera").GetComponent<Camera>();
            _bankUICamera.gameObject.SetActive(false);

            _openLock = Instantiate(_prefabOpenLock);
            _goldParticle = Instantiate(_prefabGoldParticle);

            _bankCamera = _openLock.GetComponentInChildren<Camera>();
            _getText = _bankCamera.GetComponentInChildren<Text>();
            _getText.text = "돈 획득(E)";

            _isMakeGold = true;
            _isOpen = false;
        }

        if(_isMakeGold)
        {
            if(Input.GetKeyDown(KeyCode.E))
            {
                StartCoroutine(DestroyGold());
                _time = 0;
                _isMakeGold = false;
            }
        }

        if (_isGetGold)
        {
            _ingameManage.GetGoldSetMap();
            _clearMapObject = Instantiate(_prefabClearLocation, _canvas.transform);
            _isStoreClearMap = true;
            _isGetGold = false;
        }

        if(_isStoreClearMap)
        {
            if(Input.GetKeyDown(KeyCode.Z))
            {
                _clearMapObject.SetActive(false);
                _ingameManage._isGetGold = true;
                _isStoreClearMap = false;
            }
        }
    }

    IEnumerator DestroyGold()
    {
        int count = 3;
        while(count > 0)
        {
            _openLock.transform.GetChild(1).gameObject.SetActive(true);
            _goldParticle.SetActive(true);
            yield return new WaitForSeconds(0.5f);
            _openLock.transform.GetChild(1).gameObject.SetActive(false);
            _goldParticle.SetActive(false);
            yield return new WaitForSeconds(0.5f);
            count--;
        }
        SoundManager._instance.PlayEffectSound(PublicDefine.eEffectSoundType.Have_Gold);
        _getText.text = "돈 획득했습니다.";
        yield return new WaitForSeconds(2f);
        _getText.GetComponentInParent<Image>().gameObject.SetActive(false);

        _isGetGold = true;
    }

    public void MakeLockButton()
    {
        _lockUI = Instantiate(_prefabLock, _canvas.transform);
        _lockControl = _lockUI.GetComponent<LockButtonController>();
    }

    public void ReturnMap()
    {
        _bankCamera.gameObject.SetActive(false);

        Destroy(_openLock);
    }

    public void DestroyLockUI()
    {
        Destroy(_lockUI);
    }

    public void OpenClearMap(bool isOn)
    {
        _clearMapObject.SetActive(isOn);
    }
}
