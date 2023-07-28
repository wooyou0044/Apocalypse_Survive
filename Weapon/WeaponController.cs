using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    [SerializeField] Transform _shootPos;
    public int _maxBulletCount;
    [SerializeField] Transform _muzzlePos;
    [SerializeField] GameObject _muzzleObj;

    GameObject _prefabBulletObj;
    GameObject _prefabEmptyBulletObj;

    IngameManager _ingameManage;

    int _remainNumber;

    int _remainBulletCount;

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
        _prefabBulletObj = Resources.Load("Object/45ACP Bullet_Head") as GameObject;
        //_prefabEmptyBulletObj = Resources.Load("Object/45ACP Bullet_Casing") as GameObject;
        _prefabEmptyBulletObj = Resources.Load("Object/45ACP Casing") as GameObject;

        _ingameManage = GameObject.Find("IngameManagerObject").GetComponent<IngameManager>();
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
        GameObject go = Instantiate(_prefabBulletObj, _shootPos.position, _shootPos.rotation);
        BulletObject bo = go.GetComponent<BulletObject>();
        bo.InitSetData(this.GetComponentInParent<PlayerController>(), 100);

        // 소리, 이펙트 생성 => 안 되면 플레이어 스크립트에서 이 함수 불러온 다음 소리 재생
        SoundManager._instance.PlayEffectSound(PublicDefine.eEffectSoundType.Fire_Gun);

        Instantiate(_muzzleObj, _muzzlePos);
        _remainNumber--;

        Instantiate(_prefabEmptyBulletObj, _shootPos.position, _shootPos.rotation);
        SoundManager._instance.PlayEffectSound(PublicDefine.eEffectSoundType.Fall_Gun_Empty_Bullet);
        _ingameManage.SetBulletCount(_remainNumber, _remainBulletCount);
    }

    public void ShootAimedBullet(Transform pos)
    {
        GameObject go = Instantiate(_prefabBulletObj, pos.position, pos.rotation);
        BulletObject bo = go.GetComponent<BulletObject>();
        bo.InitSetData(this.GetComponentInParent<PlayerController>(), 100);

        // 소리, 이펙트 생성 => 안 되면 플레이어 스크립트에서 이 함수 불러온 다음 소리 재생
        SoundManager._instance.PlayEffectSound(PublicDefine.eEffectSoundType.Fire_Gun);

        Instantiate(_muzzleObj, pos);
        _remainNumber--;
        Instantiate(_prefabEmptyBulletObj, _shootPos.position, _shootPos.rotation);
        SoundManager._instance.PlayEffectSound(PublicDefine.eEffectSoundType.Fall_Gun_Empty_Bullet);
        _ingameManage.SetBulletCount(_remainNumber, _remainBulletCount);
    }
}
