using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zombie2Object : BaseStat
{
    [SerializeField] Transform _shootPos;
    [SerializeField] GameObject _muzzleObj;
    [SerializeField] float _electronicDistance = 7f;

    Animator _aniCtrl;
    ZombieController _zombieCtrl;
    StatusMiniWindow _statusWin;

    RaycastHit _rHit;

    GameObject _prefabBulletObj;

    int _zombiePowAtt;
    int _halfRate;
    int _bulletDamage = 10;

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
        InitSetData(30, 15, 10);
        _zombiePowAtt = _powAtt;

        _aniCtrl = GetComponent<Animator>();
        _zombieCtrl = GetComponent<ZombieController>();
        _statusWin = GetComponentInChildren<StatusMiniWindow>();

        _prefabBulletObj = Resources.Load("Object/45ACP Bullet_Head") as GameObject;
    }

    void Start()
    {
        _statusWin.InitSetWindow(_hpRate, transform);

        _zombieCtrl.PlayVoice(PublicDefine.eVoiceType.Zombie_NormalIdle1, PublicDefine.eEnemyKind.Zombie_Couple);
    }

    void LateUpdate()
    {
        if(_zombieCtrl.IsChange)
        {
            ChangeAnimation(_zombieCtrl.CurrentState);
            _zombieCtrl.IsChange = false;
        }
    }

    void ChangeAnimation(PublicDefine.eEnemyActionState state)
    {
        switch(state)
        {
            case PublicDefine.eEnemyActionState.IDLE:
                _zombieCtrl.PlayVoice(PublicDefine.eVoiceType.Zombie_NormalIdle1, PublicDefine.eEnemyKind.Zombie_Couple);
                break;
            case PublicDefine.eEnemyActionState.WALK:
                _zombieCtrl.PlayVoice(PublicDefine.eVoiceType.Zombie_WALK, PublicDefine.eEnemyKind.Zombie_Couple);
                break;
            case PublicDefine.eEnemyActionState.RUN:
                _zombieCtrl.PlayVoice(PublicDefine.eVoiceType.Zombie_RUN, PublicDefine.eEnemyKind.Zombie_Couple);
                break;
            case PublicDefine.eEnemyActionState.BACKHOME:
                _zombieCtrl.PlayVoice(PublicDefine.eVoiceType.Zombie_RUN, PublicDefine.eEnemyKind.Zombie_Couple);
                break;
            case PublicDefine.eEnemyActionState.ATTACK:
                _aniCtrl.SetBool("IsKnife", _zombieCtrl.IsKnifeAttack);
                if(_zombieCtrl.IsKnifeAttack)
                {
                    SoundManager._instance.PlayEffectSound(PublicDefine.eEffectSoundType.Zombie_Attack);
                    _zombieCtrl.PlayVoice(PublicDefine.eVoiceType.Zombie_ATTACK1, PublicDefine.eEnemyKind.Zombie_Couple);
                }
                else
                    _zombieCtrl.PlayVoice(PublicDefine.eVoiceType.Zombie_ATTACK2, PublicDefine.eEnemyKind.Zombie_Couple);
                break;
            case PublicDefine.eEnemyActionState.DEATH:
                break;
        }
    }

    public void Shoot()
    {
        GameObject effect = Instantiate(_muzzleObj, _shootPos.position, _shootPos.rotation, _shootPos);
        Destroy(effect, 1f);

        SoundManager._instance.PlayEffectSound(PublicDefine.eEffectSoundType.Zombie2_Attack_Gun);

        Ray rayCustom = new Ray(_shootPos.position, _shootPos.forward * _electronicDistance);
        Debug.DrawRay(rayCustom.origin, rayCustom.direction * _electronicDistance, Color.green);
        int lMask = 1 << LayerMask.NameToLayer("PLAYER");
        if (Physics.Raycast(_shootPos.position, _shootPos.forward, out _rHit, _electronicDistance, lMask))
        {
            _rHit.transform.gameObject.GetComponent<PlayerController>().HitDamage(_bulletDamage);
        }
    }
}
