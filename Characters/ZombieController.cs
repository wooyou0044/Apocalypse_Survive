using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ZombieController : BaseStat
{
    [SerializeField] float _limitAxisXDistance = 7.0f;
    [SerializeField] float _limitAxisZDistance = 7.0f;
    [SerializeField] float _minDelayTime = 3.0f;
    [SerializeField] float _maxDelayTime = 6.0f;
    [SerializeField] float _walkSpeed = 1.0f;
    [SerializeField] float _runSpeed = 3.5f;
    [SerializeField] BoxCollider _damageZone1;
    [SerializeField] BoxCollider _damageZone2;
    [SerializeField] Transform _sightLocation;
    [SerializeField] GameObject _prefabBloodEffect;
    [SerializeField] GameObject _prefabGroundBlood;
    [SerializeField] Transform _bloodPos;

    PublicDefine.eRoamingType _roamType;
    PublicDefine.eEnemyActionState _currentState;

    SpawnControl _spawnCtrl;
    PlayerController _playerCtrl;
    NavMeshAgent _navAgent;
    Animator _aniCtrl;
    Rigidbody _rigid;
    StatusMiniWindow _statusWin;
    Zombie1Object _zombie1;
    Zombie2Object _zombie2;

    AudioSource _voiceClip;

    List<Vector3> _ltRoamingPoint;

    Vector3 _spawnPos;
    Vector3 _goalPos;
    Vector3 _startPos;
    Vector3 _damagePos;

    int _index = 0;
    int _damageNum = 0;
    float _timeCheck = 0;
    float _idleDelayTime = 0;
    float _sightRange = 20f;
    float _followRange = 100f;
    float _attackRange = 1.5f;
    float _gunRange = 6.0f;

    bool _isDead;
    bool _isSelectedAI = false;
    bool _isReverse = false;
    bool _isKnifeAttack = false;
    bool _isChange = false;
    bool _isNormalIdle = false;
    bool _isBigAttack = false;
    bool _isFence = false;
    bool _isDamage = false;
    bool _isZombie1 = false;

    public bool IsKnifeAttack
    {
        get { return _isKnifeAttack; }
        set { _isKnifeAttack = value; }
    }

    public PublicDefine.eEnemyActionState CurrentState
    {
        get { return _currentState; }
    }

    public bool IsChange
    {
        get { return _isChange; }
        set { _isChange = value; }
    }

    void Awake()
    {
        _ltRoamingPoint = new List<Vector3>();
        _navAgent = GetComponent<NavMeshAgent>();
        _aniCtrl = GetComponent<Animator>();
        _rigid = GetComponent<Rigidbody>();
        _statusWin = GetComponentInChildren<StatusMiniWindow>();
        _voiceClip = GetComponent<AudioSource>();

        _navAgent.enabled = false;
        _goalPos = _spawnPos = transform.position;

        if (gameObject.CompareTag("Enemy1"))
        {
            _zombie1 = GetComponent<Zombie1Object>();
            _isZombie1 = true;
        }
        if (gameObject.CompareTag("Enemy2"))
            _zombie2 = GetComponent<Zombie2Object>();
    }

    void Start()
    {
        if (_isZombie1)
            InitSetData(_zombie1.ZombieHP, _zombie1.ZombiePowAtt, _zombie1.ZombieDef);
        else
            InitSetData(_zombie2.ZombieHP, _zombie2.ZombiePowAtt, _zombie2.ZombieDef);
    }

    void Update()
    {
        if (_isDead)
        {
            return;
        }
        _rigid.velocity = Vector3.zero;
        _rigid.angularVelocity = Vector3.zero;

        if (_isDamage)
        {
            Hit();
            _isDamage = false;
        }
    }

    void LateUpdate()
    {
        if (_playerCtrl != null)
        {
            ChangeAniZombiePos(_currentState);
            SelectionAI();
        }
        else
            _playerCtrl = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
    }

    void ChangeAnimation(PublicDefine.eEnemyActionState state)
    {
        _isChange = true;
        switch (state)
        {
            case PublicDefine.eEnemyActionState.IDLE:
                _navAgent.enabled = false;
                //_isNormalIdle = SetRandomRate();
                //_aniCtrl.SetBool("IsNormalIdle", _isNormalIdle);
                _timeCheck = 0;
                break;
            case PublicDefine.eEnemyActionState.WALK:
                _navAgent.enabled = true;
                _navAgent.speed = _walkSpeed;
                _navAgent.stoppingDistance = 0;
                break;
            case PublicDefine.eEnemyActionState.RUN:
                _navAgent.enabled = true;
                _navAgent.speed = _runSpeed;
                _navAgent.stoppingDistance = _attackRange;
                _damageZone1.enabled = false;
                _damageZone2.enabled = false;
                _statusWin.EnableWindow(true);
                break;
            case PublicDefine.eEnemyActionState.BACKHOME:
                _navAgent.enabled = true;
                _navAgent.speed = _runSpeed * 2;
                _navAgent.stoppingDistance = 0;
                _damageZone1.enabled = false;
                _damageZone2.enabled = false;
                break;
            case PublicDefine.eEnemyActionState.ATTACK:
                _navAgent.enabled = true;
                _damageZone1.enabled = true;
                _damageZone2.enabled = true;
                if (_isFence)
                {
                    _damageZone1.gameObject.SetActive(false);
                    _damageZone2.gameObject.SetActive(false);
                    //_damageZone1.enabled = false;
                    //_damageZone2.enabled = false;
                }
                else
                {
                    _damageZone1.gameObject.SetActive(true);
                    _damageZone2.gameObject.SetActive(true);
                }
                //_isBigAttack = SetRandomRate();
                //_aniCtrl.SetBool("IsBigAttack", _isBigAttack);
                break;
            case PublicDefine.eEnemyActionState.DEATH:
                // 좀비의 HP가 0일때 _isDead = true;, SetTrigger("Death");
                if (_nowHP <= 0)
                {
                    _isDead = true;
                    GetComponent<CapsuleCollider>().enabled = false;
                    Destroy(gameObject, 7);
                }
                break;
        }
        _aniCtrl.SetInteger("ActionState", (int)state);
        _currentState = state;
    }

    void ChangeAniZombiePos(PublicDefine.eEnemyActionState state)
    {
        switch (_currentState)
        {
            case PublicDefine.eEnemyActionState.IDLE:
                if (Vector3.Distance(transform.position, _playerCtrl.transform.position) <= _sightRange)
                {
                    if (CheckObstacle())
                    {
                        _startPos = transform.position;
                        ChangeAnimation(PublicDefine.eEnemyActionState.RUN);
                        _goalPos = _playerCtrl.transform.position;
                    }
                    else
                        _isSelectedAI = false;
                }
                else
                {
                    _timeCheck += Time.deltaTime;
                    if (_timeCheck >= _idleDelayTime)
                        _isSelectedAI = false;
                }
                break;
            case PublicDefine.eEnemyActionState.WALK:
                if (Vector3.Distance(transform.position, _playerCtrl.transform.position) <= _sightRange)
                {
                    if (CheckObstacle())
                    {
                        _startPos = transform.position;
                        ChangeAnimation(PublicDefine.eEnemyActionState.RUN);
                        _goalPos = _playerCtrl.transform.position;
                    }
                }
                else
                {
                    if (Vector3.Distance(transform.position, _goalPos) < 0.5f)
                    {
                        _isSelectedAI = false;
                    }
                }
                break;
            case PublicDefine.eEnemyActionState.RUN:
                if (Vector3.Distance(transform.position, _playerCtrl.transform.position) <= _followRange)
                {
                    if (Vector3.Distance(transform.position, _goalPos) <= _attackRange)
                    {
                        _isKnifeAttack = true;
                        ChangeAnimation(PublicDefine.eEnemyActionState.ATTACK);
                        transform.LookAt(_playerCtrl.transform);
                    }
                    else if (Vector3.Distance(transform.position, _goalPos) > 4.0f && Vector3.Distance(transform.position, _playerCtrl.transform.position) <= _gunRange)
                    {
                        _isKnifeAttack = false;
                        if (gameObject.CompareTag("Enemy2"))
                        {
                            ChangeAnimation(PublicDefine.eEnemyActionState.ATTACK);
                            transform.LookAt(_playerCtrl.transform);
                        }
                    }
                    else
                    {
                        _goalPos = _playerCtrl.transform.position;
                        _navAgent.destination = _goalPos;
                    }
                }
                else
                {
                    ChangeAnimation(PublicDefine.eEnemyActionState.BACKHOME);
                    _goalPos = _startPos;
                    _navAgent.destination = _goalPos;
                }
                break;
            case PublicDefine.eEnemyActionState.BACKHOME:
                if (Vector3.Distance(transform.position, _goalPos) <= 0.2f)
                    _isSelectedAI = false;
                break;
            case PublicDefine.eEnemyActionState.ATTACK:
                if (Vector3.Distance(transform.position, _playerCtrl.transform.position) > _attackRange)
                {
                    ChangeAnimation(PublicDefine.eEnemyActionState.RUN);
                    _goalPos = _playerCtrl.transform.position;
                    _navAgent.destination = _goalPos;
                }
                else
                    transform.LookAt(_playerCtrl.transform);
                break;
            case PublicDefine.eEnemyActionState.DEATH:
                if (_nowHP > 0)
                {
                    if (Vector3.Distance(transform.position, _playerCtrl.transform.position) > _attackRange)
                    {
                        ChangeAnimation(PublicDefine.eEnemyActionState.RUN);
                        _goalPos = _playerCtrl.transform.position;
                        _navAgent.destination = _goalPos;
                    }
                    else
                        ChangeAnimation(PublicDefine.eEnemyActionState.ATTACK);
                }
                else
                    _navAgent.enabled = false;
                break;
        }
    }

    bool CheckObstacle()
    {
        Vector3 targetPoint = _playerCtrl.transform.position + Vector3.up;
        RaycastHit rHit;
        Vector3 dir = (targetPoint - _sightLocation.position).normalized;
        if (Physics.Raycast(_sightLocation.position, dir, out rHit, _sightRange))
        {
            if (rHit.transform.CompareTag("Player"))
                return true;
        }
        return false;
    }

    void Hit()
    {
        if (CalcHit(_damageNum))
        {
            if(_isZombie1)
                PlayVoice(PublicDefine.eVoiceType.Zombie_DEATH, PublicDefine.eEnemyKind.Zombie_Single, false);
            else
                PlayVoice(PublicDefine.eVoiceType.Zombie_DEATH, PublicDefine.eEnemyKind.Zombie_Couple, false);
            _aniCtrl.SetTrigger("Dead");
            ChangeAnimation(PublicDefine.eEnemyActionState.DEATH);
        }
        else
        {
            if (_isZombie1)
                PlayVoice(PublicDefine.eVoiceType.Zombie_HIT, PublicDefine.eEnemyKind.Zombie_Single, false);
            else
                PlayVoice(PublicDefine.eVoiceType.Zombie_DEATH, PublicDefine.eEnemyKind.Zombie_Couple, false);
            Debug.Log("들어옴");
            ChangeAnimation(PublicDefine.eEnemyActionState.DEATH);
        }

        //Instantiate(_prefabBloodEffect, transform.position + new Vector3(-0.2f,1.2f,0.3f), Quaternion.Euler(new Vector3(0, transform.rotation.y - 100, 0)));
        GameObject go = Instantiate(_prefabBloodEffect);
        go.transform.SetParent(_bloodPos.transform, false);
        go.transform.rotation = Quaternion.Euler(new Vector3(0, -_playerCtrl.transform.rotation.y, 0));
        GameObject blood = Instantiate(_prefabGroundBlood, transform.position + new Vector3(-1f, 0, 1f), transform.rotation);
        Destroy(go, 1);
        Destroy(blood, 4);
        _statusWin.SetHPValue(_hpRate);
    }

    void SelectionAI()
    {
        if (!_isSelectedAI)
        {
            int random = Random.Range(0, 2);
            if (random == 0)
            {
                ChangeAnimation(PublicDefine.eEnemyActionState.IDLE);
                _idleDelayTime = Random.Range(_minDelayTime, _maxDelayTime);
            }
            else
            {
                ChangeAnimation(PublicDefine.eEnemyActionState.WALK);
                _goalPos = GetNextMovePosition();
                _navAgent.destination = _goalPos;
            }
            _isSelectedAI = true;
        }
    }

    Vector3 GetNextMovePosition()
    {
        Vector3 pos = Vector3.zero;
        switch (_roamType)
        {
            case PublicDefine.eRoamingType.RandomPosition:
                float randX = Random.Range(-_limitAxisXDistance, _limitAxisXDistance);
                float randZ = Random.Range(-_limitAxisZDistance, _limitAxisZDistance);
                pos = _spawnPos + new Vector3(randX, 0, randZ);
                break;
            case PublicDefine.eRoamingType.RandomPoint:
                int random = Random.Range(0, _ltRoamingPoint.Count);
                pos = _ltRoamingPoint[random];
                break;
            case PublicDefine.eRoamingType.PatrolPoint:
                pos = _ltRoamingPoint[_index];
                _index++;
                if (_index >= _ltRoamingPoint.Count)
                    _index = 0;
                break;
            case PublicDefine.eRoamingType.BackNForth:
                if (_isReverse)
                {
                    pos = _ltRoamingPoint[_index];
                    _index--;
                    if (_index < 0)
                    {
                        _index = 1;
                        _isReverse = !_isReverse;
                    }
                }
                else
                {
                    pos = _ltRoamingPoint[_index];
                    _index++;
                    if (_index >= _ltRoamingPoint.Count)
                    {
                        _index = _ltRoamingPoint.Count - 1;
                        _isReverse = !_isReverse;
                    }
                }
                break;
        }
        return pos;
    }

    public void InitSetEnemy(SpawnControl spawnCtrl, Transform root, PublicDefine.eRoamingType type)
    {
        _spawnCtrl = spawnCtrl;
        _roamType = type;

        for (int n = 0; n < root.childCount; n++)
            _ltRoamingPoint.Add(root.GetChild(n).position);

        _damageZone1.enabled = false;
        _damageZone2.enabled = false;
    }

    public void TakeDamge(int damageNum)
    {
        _damageNum = damageNum;
        _isDamage = true;
    }

    public void PlayVoice(PublicDefine.eVoiceType voice, PublicDefine.eEnemyKind kind, bool isLoop = true)
    {
        _voiceClip.clip = SoundClipPoolManager._instance.GetEnemyVoice(voice, kind);
        _voiceClip.loop = isLoop;
        _voiceClip.Play();
    }

    // 플레이어가 쏜 총이나 휘두른 무기에 맞았을 때 플레이어 HP가 깎이게
    void OnTriggerEnter(Collider other)
    {
        //if (_isDead)
        //    return;
        if (other.CompareTag("BulletObj"))
        {
            _damageNum = _playerCtrl.WeaponDamageNum;
            _isDamage = true;
            Destroy(other.gameObject);
        }

        if (other.CompareTag("PlayerDamageZone"))
        {
            _damageNum = _playerCtrl.WeaponDamageNum;
            _isDamage = true;
        }

        if (other.CompareTag("Fence"))
        {
            _isFence = true;
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Fence"))
        {
            _isFence = true;
        }

        else if (!collision.gameObject.CompareTag("Fence"))
        {
            _isFence = false;
        }
    }
}
