using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundClipPoolManager : MonoBehaviour
{
    [SerializeField] AudioClip[] _voiceClips;
    [SerializeField] AudioClip[] _bgmClips;
    [SerializeField] AudioClip[] _effClips;

    static SoundClipPoolManager _uniqueInstance;

    public static SoundClipPoolManager _instance
    {
        get { return _uniqueInstance; }
    }

    public AudioClip[] _bgms
    {
        get { return _bgmClips; }
    }

    public AudioClip[] _effs
    {
        get { return _effClips; }
    }

    public AudioClip[] _voices
    {
        get { return _voiceClips; }
    }

    void Awake()
    {
        _uniqueInstance = this;
    }

    public AudioClip GetPlayerVoice(PublicDefine.eVoiceType type, bool isMan)
    {
        int index = (int)type;
        if (!isMan)
            index += 10;
        return _voiceClips[index];
    }

    public AudioClip GetEnemyVoice(PublicDefine.eVoiceType type, PublicDefine.eEnemyKind kind)
    {
        int index = (int)type;
        index += (10 * (int)kind);
        return _voiceClips[index];
    }
}
