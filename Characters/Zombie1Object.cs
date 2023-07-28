using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zombie1Object : BaseStat
{
    Animator _aniCtrl;
    ZombieController _zombieCtrl;
    StatusMiniWindow _statusWin;

    int _halfRate;
    int _zombiePowAtt;

    bool _isNormalIdle = false;
    bool _isBigAttack = false;

    public int ZombiePowAtt
    {
        get { return _zombiePowAtt; }
    }

    public int ZombieHP
    {
        get { return _hp; }
    }

    public int ZombieDef
    {
        get { return _powDef; }
    }

    void Awake()
    {
        InitSetData(20, 10, 5);
        _zombiePowAtt = _powAtt;
        _halfRate = 50;

        _aniCtrl = GetComponent<Animator>();
        _zombieCtrl = GetComponent<ZombieController>();
        _statusWin = GetComponentInChildren<StatusMiniWindow>();
    }

    void Start()
    {
        _statusWin.InitSetWindow(_hpRate, transform);

        _zombieCtrl.PlayVoice(PublicDefine.eVoiceType.Zombie_NormalIdle1, PublicDefine.eEnemyKind.Zombie_Single);
    }

    void LateUpdate()
    {
        if (_zombieCtrl.IsChange)
        {
            ChangeAnimation(_zombieCtrl.CurrentState);
            _zombieCtrl.IsChange = false;
        }
    }

    void ChangeAnimation(PublicDefine.eEnemyActionState state)
    {
        switch (state)
        {
            case PublicDefine.eEnemyActionState.IDLE:
                _isNormalIdle = SetRandomRate();
                _aniCtrl.SetBool("IsNormalIdle", _isNormalIdle);
                if (SetRandomRate())
                    _zombieCtrl.PlayVoice(PublicDefine.eVoiceType.Zombie_NormalIdle1, PublicDefine.eEnemyKind.Zombie_Single);
                else
                    _zombieCtrl.PlayVoice(PublicDefine.eVoiceType.Zombie_NormalIdle2, PublicDefine.eEnemyKind.Zombie_Single);
                break;
            case PublicDefine.eEnemyActionState.WALK:
                _zombieCtrl.PlayVoice(PublicDefine.eVoiceType.Zombie_WALK, PublicDefine.eEnemyKind.Zombie_Single);
                break;
            case PublicDefine.eEnemyActionState.RUN:
                _zombieCtrl.PlayVoice(PublicDefine.eVoiceType.Zombie_RUN, PublicDefine.eEnemyKind.Zombie_Single);
                break;
            case PublicDefine.eEnemyActionState.BACKHOME:
                _zombieCtrl.PlayVoice(PublicDefine.eVoiceType.Zombie_RUN, PublicDefine.eEnemyKind.Zombie_Single);
                break;
            case PublicDefine.eEnemyActionState.ATTACK:
                _isBigAttack = SetRandomRate();
                _aniCtrl.SetBool("IsBigAttack", _isBigAttack);
                if(SetRandomRate())
                {
                    SoundManager._instance.PlayEffectSound(PublicDefine.eEffectSoundType.Zombie_Attack);
                    _zombieCtrl.PlayVoice(PublicDefine.eVoiceType.Zombie_ATTACK1, PublicDefine.eEnemyKind.Zombie_Single);
                }
                else
                    _zombieCtrl.PlayVoice(PublicDefine.eVoiceType.Zombie_ATTACK2, PublicDefine.eEnemyKind.Zombie_Single);
                break;
            case PublicDefine.eEnemyActionState.DEATH:
                break;
        }
    }

    bool SetRandomRate()
    {
        int random = Random.Range(0, 100);
        if (random <= _halfRate)
            return false;
        return true;
    }

}
