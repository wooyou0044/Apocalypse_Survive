using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingManager : MonoBehaviour
{
    [SerializeField] Toggle _soundOn;
    [SerializeField] Toggle _soundOff;
    [SerializeField] Toggle _musicOn;
    [SerializeField] Toggle _musicOff;
    [SerializeField] Slider _soundSlider;
    [SerializeField] Slider _musicSlider;

    // effect Sound
    Image _soundImageOn;
    Text _soundLabelOn;
    Image _soundImageOff;
    Text _soundLabelOff;

    // BGM Sound
    Image _bgmImageOn;
    Text _bgmLabelOn;
    Image _bgmImageOff;
    Text _bgmLabelOff;

    bool _bgmMute;
    bool _effectMute;

    float _bgmVolume;
    float _effVolume;

    void InitSet()
    {
        _soundImageOn = _soundOn.GetComponentInChildren<Image>();
        _soundLabelOn = _soundOn.transform.GetChild(1).GetComponent<Text>();
        _soundImageOff = _soundOff.GetComponentInChildren<Image>();
        _soundLabelOff = _soundOff.transform.GetChild(1).GetComponent<Text>();

        _bgmImageOn = _musicOn.GetComponentInChildren<Image>();
        _bgmLabelOn = _musicOn.transform.GetChild(1).GetComponent<Text>();
        _bgmImageOff = _musicOff.GetComponentInChildren<Image>();
        _bgmLabelOff = _musicOff.transform.GetChild(1).GetComponent<Text>();
    }

    void Start()
    {
        InitSet();
    }

    void Update()
    {
        _bgmVolume = _musicSlider.value;
        _effVolume = _soundSlider.value;
        SoundManager._instance.AdjustBGMSound(_bgmMute, _bgmVolume);
        SoundManager._instance.AdjustEffectSound(_effectMute, _effVolume);
    }

    public void CloseButton()
    {
        SoundManager._instance.PlayEffectSound(PublicDefine.eEffectSoundType.Button);
        gameObject.SetActive(false);
    }

    void SoundOnUI()
    {
        _soundImageOn.color = new Color(120 / 255f, 0, 0);
        _soundLabelOn.color = new Color(1, 1, 1);
        _soundImageOff.color = new Color(1, 1, 1);
        _soundLabelOff.color = new Color(120 / 255f, 0, 0);
    }

    void SoundOffUI()
    {
        _soundImageOn.color = new Color(1, 1, 1);
        _soundLabelOn.color = new Color(120 / 255f, 0, 0);
        _soundImageOff.color = new Color(120 / 255f, 0, 0);
        _soundLabelOff.color = new Color(1, 1, 1);
    }

    public void SoundOff()
    {
        SoundOffUI();
        _soundSlider.value = 0;
        _effectMute = true;
        _effVolume = _soundSlider.value;
    }

    public void SoundOn()
    {
        SoundOnUI();
        _soundSlider.value = _soundSlider.maxValue / 2;
        _effectMute = false;
        _effVolume = _soundSlider.value;
    }

    public void SoundSlider()
    {
        if (_soundSlider.value <= 0)
        {
            _effectMute = true;
            SoundOffUI();
        }
        else
        {
            _effectMute = false;
            SoundOnUI();
        }
    }

    void MusicOnUI()
    {
        _bgmImageOn.color = new Color(0, 0, 0);
        _bgmLabelOn.color = new Color(1, 1, 1);
        _bgmImageOff.color = new Color(1, 1, 1);
        _bgmLabelOff.color = new Color(0, 0, 0);
    }

    void MusicOffUI()
    {
        _bgmImageOn.color = new Color(1, 1, 1);
        _bgmLabelOn.color = new Color(0, 0, 0);
        _bgmImageOff.color = new Color(0, 0, 0);
        _bgmLabelOff.color = new Color(1, 1, 1);
    }

    public void MusicOff()
    {
        MusicOffUI();
        _musicSlider.value = 0;
        _bgmMute = true;
        _bgmVolume = _musicSlider.value;
    }

    public void MusicOn()
    {
        MusicOnUI();
        _musicSlider.value = _musicSlider.maxValue / 2;
        _bgmMute = false;
        _bgmVolume = _musicSlider.value;
    }

    public void MusicSlider()
    {
        if (_musicSlider.value <= 0)
        {
            _bgmMute = true;
            MusicOffUI();
        }
        else
        {
            _bgmMute = false;
            MusicOnUI();
        }
    }
}
