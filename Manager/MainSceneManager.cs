using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainSceneManager : MonoBehaviour
{
    [SerializeField] Image _coinBack;
    SceneControlManager _sceneManager;

    UserInfo _userInfo;
    Text _coin;

    void Awake()
    {
        _sceneManager = GameObject.Find("SceneManagerObject").GetComponent<SceneControlManager>();
        _userInfo = GameObject.Find("UserInfo").GetComponent<UserInfo>();
        _coin = _coinBack.GetComponentInChildren<Text>();
        _coin.text = _userInfo._coin.ToString();
    }

    public void PlayButton()
    {
        _sceneManager.StartLobbyScene();
    }
}
