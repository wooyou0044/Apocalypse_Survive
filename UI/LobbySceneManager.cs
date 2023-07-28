using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LobbySceneManager : MonoBehaviour
{
    [SerializeField] Image _coinBack;
    [SerializeField] InputField _nickNameInput;
    [SerializeField] GameObject _prefabWarning;
    [SerializeField] Image _mapBack;
    [SerializeField] Transform[] _mapPos;
    [SerializeField] GameObject _prefabMapStatus;
    [SerializeField] Sprite _cover;
    [SerializeField] Sprite _clear;
    [SerializeField] GameObject _prefabPreviewMap;
    [SerializeField] Sprite[] _mapPreviewSprite;

    UserInfo _userInfo;
    Text _warningText;
    StageObj _stage;
    MapPreview _mapPreview;
    SceneControlManager _sceneControl;
    MenuSceneManager _menuManager;

    Text _coin;
    void Awake()
    {
        _userInfo = GameObject.Find("UserInfo").GetComponent<UserInfo>();
        _sceneControl = GameObject.Find("SceneManagerObject").GetComponent<SceneControlManager>();
        _menuManager = GameObject.Find("MainManagerObject").GetComponent<MenuSceneManager>();
        _coin = _coinBack.GetComponentInChildren<Text>();
        _coin.text = _userInfo._coin.ToString();
        _prefabWarning.SetActive(false);
    }

    void Start()
    {
        InitSet();
    }

    void Update()
    {
        if (_stage != null)
            PreviewMap();
        if (_nickNameInput.text == string.Empty)
        {
            _prefabWarning.SetActive(true);
            _warningText = _prefabWarning.transform.GetChild(1).GetComponent<Text>();
            _warningText.text = "닉네임을 입력해주세요!!!!";
        }
        else
            _prefabWarning.SetActive(false);
    }

    public void InitSet()
    {
        for (int n = 0; n < _userInfo._clearMap + 1; n++)
        {
            GameObject go = Instantiate(_prefabMapStatus, _mapPos[n].transform.position, _mapPos[n].transform.rotation, _mapBack.transform);
            Image _mapStatus = go.GetComponent<Image>();
            _stage = go.GetComponent<StageObj>();
            if (n == _userInfo._clearMap)
            {
                _mapStatus.sprite = _cover;
            }
            else if (n < _userInfo._clearMap)
            {
                _mapStatus.sprite = _clear;
            }
        }
    }

    public void PreviewMap()
    {
        if(_mapPreview == null)
        {
            if(_stage._isEnter && !_stage._isExit)
            {
                GameObject go = Instantiate(_prefabPreviewMap, _mapPos[_userInfo._clearMap].transform.position + new Vector3(0, 200, 0), _mapPos[_userInfo._clearMap].transform.rotation, _mapBack.transform);
                _mapPreview = go.GetComponent<MapPreview>();

                Image mapImage= go.transform.GetChild(0).GetComponent<Image>();
                mapImage.sprite = _mapPreviewSprite[_userInfo._clearMap];

                _stage._isEnter = false;
            }
        }
        else
        {
            if (_stage._isEnter && !_stage._isExit)
            {
                _mapPreview.gameObject.SetActive(!_mapPreview.gameObject.activeSelf);
                _stage._isEnter = false;
            }
            else if(!_stage._isEnter && _stage._isExit)
            {
                _mapPreview.gameObject.SetActive(!_mapPreview.gameObject.activeSelf);
                _stage._isExit = false;
            }
        }
    }

    public void SetUserName()
    {
        _userInfo._nickName = _nickNameInput.text;
    }

    public void SurviveButton()
    {
        _sceneControl.StartIngameScene();
    }

    public void BackButton()
    {
        _sceneControl.StartMainScene();
    }

    public void SettingBUtton()
    {
        _menuManager.OpenSettingWindow();
    }
}
