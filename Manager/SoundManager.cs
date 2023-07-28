using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    static SoundManager _uniqueInstance;

    AudioClip[] _bgmClips;
    AudioClip[] _effectClips;

    float _bgmVolume;
    bool _bgmMute;
    bool _bgmLoop;

    float _effVolume;
    bool _effMute;

    AudioSource _bgmPlayer;
    AudioSource _effPlayer;
    List<AudioSource> _effSoundPlayer = new List<AudioSource>();

    SoundClipPoolManager _soundClip;

    public static SoundManager _instance
    {
        get { return _uniqueInstance; }
    }
    void Init()
    {
        _bgmVolume = 1.0f;
        _bgmMute = false;
        _bgmLoop = true;
        _effVolume = 0.5f;
        _effMute = false;
        _bgmPlayer = GetComponent<AudioSource>();
        _effPlayer = GetComponent<AudioSource>();
    }

    void Awake()
    {
        _uniqueInstance = this;
        _soundClip = GameObject.Find("SoundClipPool").GetComponent<SoundClipPoolManager>();
        DontDestroyOnLoad(gameObject);
        Init();
    }

    public void InitSetReference()
    {
        _bgmClips = _soundClip._bgms;
        _effectClips = _soundClip._effs;
    }

    public void PlayBGMSound(PublicDefine.eBGMType type)
    {
        _bgmPlayer.clip = _bgmClips[(int)type];
        _bgmPlayer.volume = _bgmVolume;
        _bgmPlayer.mute = _bgmMute;
        _bgmPlayer.loop = _bgmLoop;

        _bgmPlayer.Play();
    }

    public void AdjustBGMSound(bool isMute, float volume)
    {
        _bgmPlayer.volume = volume;
        _bgmPlayer.mute = isMute;
    }

    public void AdjustEffectSound(bool isMute, float volume)
    {
        _effVolume = volume;
        _effMute = isMute;
    }

    public GameObject PlayEffectSound(PublicDefine.eEffectSoundType type, bool isLoop = false)
    {
        GameObject go = new GameObject("EffectSoundPlay");
        go.transform.parent = transform;
        _effPlayer = go.AddComponent<AudioSource>();
        _effPlayer.clip = _effectClips[(int)type];
        _effPlayer.volume = _effVolume;
        _effPlayer.mute = _effMute;
        _effPlayer.loop = isLoop;
        _effPlayer.Play();

        _effSoundPlayer.Add(_effPlayer);
        Destroy(go, 4);

        return go;
    }
}
