using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RiffleController : MonoBehaviour
{
    [SerializeField] float _fireRate = 0.08f;
    [SerializeField] bool _isFire = false;
    [SerializeField] float _bulletDistance = 100;
    public int _bulletOnceCount = 5;

    [SerializeField] Transform _shootPos;
    public int _maxBulletCount;

    MeshRenderer _muzzleFlash;
    MeshRenderer _bulletTrailRend;
    Transform _bulletTrail;

    GameObject _prefabEmptyBulletObj;

    IngameManager _ingameManage;

    int _remainNumber;
    int _remainBulletCount;

    bool _isRange = false;

    RaycastHit _rHit;

    float _nextFireTime;

    public int RemainNum
    {
        get { return _remainNumber; }
    }

    public int RemainBulletCount
    {
        get { return _remainBulletCount; }
        set { _remainBulletCount = value; }
    }

    void Awake()
    {
        _prefabEmptyBulletObj = Resources.Load("Object/45ACP Casing") as GameObject;
        _ingameManage = GameObject.Find("IngameManagerObject").GetComponent<IngameManager>();

        _muzzleFlash = transform.GetChild(0).GetComponent<MeshRenderer>();
        _bulletTrail = _muzzleFlash.transform.GetChild(0);
        _bulletTrailRend = _muzzleFlash.GetComponentInChildren<MeshRenderer>();

        //_muzzleFlash.enabled = false;
        _muzzleFlash.gameObject.SetActive(false);
        //_bulletTrailRend.enabled = false;
        _bulletTrailRend.gameObject.SetActive(false);

        _nextFireTime = 0;
    }

    public void ReloadBullet()
    {
        _remainNumber = _maxBulletCount;
        _remainBulletCount -= _maxBulletCount;
        if (_remainBulletCount < 0)
            _remainBulletCount = 0;

        _ingameManage.SetBulletCount(_remainNumber, _remainBulletCount);
        //SoundManager._instance.PlayEffectSound(PublicDefine.eEffectSoundType.Reload);
    }

    public void ChargeRemainBullet()
    {
        _remainNumber = _maxBulletCount;
        _ingameManage.SetBulletCount(_remainNumber, _remainBulletCount);
    }

    public void ChargeBullet(int count)
    {
        _remainBulletCount += (_maxBulletCount * count);
        _ingameManage.SetBulletCount(_remainNumber, _remainBulletCount);
    }

    public void ShootBullet()
    {
        // 총알 안 날라가면 위치 확인해서 메세지 전송
        SoundManager._instance.PlayEffectSound(PublicDefine.eEffectSoundType.Fire_Riffle);
        StartCoroutine(FireEffect());
        _remainNumber--;
        Instantiate(_prefabEmptyBulletObj, _shootPos.position, _shootPos.rotation);
        SoundManager._instance.PlayEffectSound(PublicDefine.eEffectSoundType.Fall_Riffle_Empty_Bullet);
        _ingameManage.SetBulletCount(_remainNumber, _remainBulletCount);
        // 소리, 이펙트 생성 => 안 되면 플레이어 스크립트에서 이 함수 불러온 다음 소리 재생
        //SoundManager._instance.PlayEffectSound(PublicDefine.eEffectSoundType.Fire_Riffle);
    }

    public void SendMessageEnemy()
    {
        _rHit.transform.gameObject.SendMessage("Hit", SendMessageOptions.DontRequireReceiver);
        Debug.Log(_rHit.transform.gameObject.name);
    }

    public bool IsInRange()
    {
        Ray rayCustom = new Ray(_shootPos.position, _shootPos.forward * _bulletDistance);
        Debug.DrawRay(rayCustom.origin, rayCustom.direction * _bulletDistance, Color.blue);
        int lMask = 1 << LayerMask.NameToLayer("ENEMY");
        if (Physics.Raycast(_shootPos.position, _shootPos.forward, out _rHit, _bulletDistance, lMask))
            return true;
        else
            return false;
    }

    public bool IsInRange(Transform pos)
    {
        Ray rayCustom = new Ray(pos.position, pos.forward * _bulletDistance);
        Debug.DrawRay(rayCustom.origin, rayCustom.direction * _bulletDistance, Color.red);
        int lMask = 1 << LayerMask.NameToLayer("ENEMY");
        if (Physics.Raycast(pos.position, pos.forward, out _rHit, _bulletDistance, lMask))
            return true;
        else
            return false;
    }

    IEnumerator FireEffect()
    {
        Vector2 vRand = Random.insideUnitCircle;
        transform.localPosition += new Vector3(0, vRand.x * 0.01f, vRand.y * 0.01f);

        _muzzleFlash.transform.Rotate(Vector3.forward, Random.Range(0.0f, 360.0f));

        _muzzleFlash.gameObject.SetActive(true);
        _bulletTrailRend.gameObject.SetActive(true);
        yield return new WaitForSeconds(0.08f);

        _muzzleFlash.gameObject.SetActive(false);
        _bulletTrailRend.gameObject.SetActive(false);
        yield return new WaitForSeconds(0.08f);
    }
}
