using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneControlManager : MonoBehaviour
{
    [SerializeField] GameObject[] _prefabLoadingWndObj;

    PublicDefine.eSceneType _currentScene;

    AsyncOperation _aOper;
    LoadingWindow _wndLoading;
    UserInfo _userInfo;

    Scene _sceneName;
    float _timeCheck = 0;

    public PublicDefine.eSceneType GetCurrentScene
    {
        get { return _currentScene; }
    }

    int random;
    static SceneControlManager _uniqueInstance;

    public static SceneControlManager _instance
    {
        get { return _uniqueInstance; }
    }

    void Awake()
    {
        _uniqueInstance = this;
        DontDestroyOnLoad(this);
    }

    IEnumerator StartLoadding(string scene, int num)
    {
        Canvas canvas = GameObject.Find("Canvas").GetComponent<Canvas>();
        _aOper = SceneManager.LoadSceneAsync(scene);
        _aOper.allowSceneActivation = false;
        float time = 0;
        while(!_aOper.isDone)
        {
            yield return null;
            time += Time.deltaTime;
            if(time > 1.5f)
            {
                _aOper.allowSceneActivation = true;
            }
            else
            {
                if(_wndLoading == null)
                {
                    GameObject go = Instantiate(_prefabLoadingWndObj[num], canvas.transform);
                    _wndLoading = go.GetComponent<LoadingWindow>();
                }
            }
        }
        if (_currentScene == PublicDefine.eSceneType.IngameScene)
        {
            int mapNum = _userInfo._clearMap + 1;
            string mapScene = "Map" + mapNum.ToString() + "Scene";
            _aOper = SceneManager.LoadSceneAsync(mapScene, LoadSceneMode.Additive);
            while (!_aOper.isDone)
            {
                yield return null;
            }
            _sceneName = SceneManager.GetSceneByName(mapScene);
            _currentScene = PublicDefine.eSceneType.MapScene;
            SceneManager.SetActiveScene(_sceneName);
        }
    }

    public void ApplicationStart()
    {
        _aOper = SceneManager.LoadSceneAsync("FirstScene");
        _aOper = null;
    }

    public void StartMainScene()
    {
        SoundManager._instance.PlayEffectSound(PublicDefine.eEffectSoundType.StartButton1);
        _currentScene = PublicDefine.eSceneType.MainScene;
        StartCoroutine(StartLoadding("MainScene", 0));
        //SoundManager._instance.PlayBGMSound(PublicDefine.eSceneType.MainScene);
    }

    public void StartLobbyScene()
    {
        SoundManager._instance.PlayEffectSound(PublicDefine.eEffectSoundType.StartButton2);
        _currentScene = PublicDefine.eSceneType.LobbyScene;
        Camera _uiCamera = GameObject.Find("UICamera").GetComponent<Camera>();
        _uiCamera.gameObject.SetActive(false);
        StartCoroutine(StartLoadding("LobbyScene",1));
        //SoundManager._instance.PlayBGMSound(PublicDefine.eSceneType.LobbyScene);
    }

    // 나중엔 UI만 있는 씬 넣어서 하기 바로 Map으로 이동하게
    public void StartIngameScene()
    {
        SoundManager._instance.PlayEffectSound(PublicDefine.eEffectSoundType.StartButton3);
        _currentScene = PublicDefine.eSceneType.IngameScene;
        _userInfo = GameObject.Find("UserInfo").GetComponent<UserInfo>();
        StartCoroutine(StartLoadding("IngameScene", 2));
    }

    // 은행으로 들어가면 BankScene으로 이동
    public void StartBankScene()
    {
        _aOper = SceneManager.LoadSceneAsync("BankScene");
        //SoundManager._instance.PlayBGMSound(PublicDefine.eSceneType.BankScene);
        _aOper = null;
    }

    public void BackLobbyScene()
    {
        _currentScene = PublicDefine.eSceneType.LobbyScene;
        StartCoroutine(StartLoadding("LobbyScene", 0));
    }
}
