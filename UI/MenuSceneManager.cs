using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuSceneManager : MonoBehaviour
{
    [SerializeField] GameObject _settingWindow;

    SettingManager _wndSetting;

    Canvas _canvas;

    SceneControlManager _sceneControl;

    public float _bgmVolume;
    public float _effectVolume;

    void Awake()
    {
        _canvas = GameObject.Find("Canvas").GetComponent<Canvas>();
        _sceneControl = GameObject.Find("SceneManagerObject").GetComponent<SceneControlManager>();
        DontDestroyOnLoad(this);
    }

    public void OpenSettingWindow()
    {
        _canvas = GameObject.Find("Canvas").GetComponent<Canvas>();
        SoundManager._instance.PlayEffectSound(PublicDefine.eEffectSoundType.Button);
        if (_wndSetting == null)
        {
            GameObject go = null;
            if (_sceneControl.GetCurrentScene == PublicDefine.eSceneType.MainScene)
            {
                go = Instantiate(_settingWindow,_canvas.transform.position + new Vector3(70,8,0), _canvas.transform.rotation, _canvas.transform);
            }
            else
            {
                go = Instantiate(_settingWindow, _canvas.transform);
            }
            _wndSetting = go.GetComponent<SettingManager>();
        }
        else
            _wndSetting.gameObject.SetActive(!_wndSetting.gameObject.activeSelf);
    }
}
