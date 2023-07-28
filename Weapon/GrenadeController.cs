using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrenadeController : MonoBehaviour
{
    [SerializeField] float _explosionRadius = 10f;
    [SerializeField] float _explosionForce = 500f;
    [SerializeField] float _throwForce = 100f;
    [SerializeField] float _timeToExplosion = 5f;
    [SerializeField] ParticleSystem _prefabExplosionEff;

    Rigidbody _rigid;

    bool _isCountDownExplosion = false;
    bool _isDamge = false;

    ParticleSystem _explosionEff;

    int _explosionDamage;

    void Awake()
    {
        _rigid = GetComponent<Rigidbody>();
        _rigid.isKinematic = true;
    }

    public void Shoot(Vector3 rotation)
    {
        _rigid.isKinematic = false;
        _rigid.AddForce(rotation * _throwForce);
    }

    public void CountDown()
    {
        if (_isCountDownExplosion)
            return;
        _isCountDownExplosion = true;
        Invoke("Explode", _timeToExplosion);
    }

    public void Explode()
    {
        ParticleSystem explosionEff = Instantiate(_prefabExplosionEff, transform.position, transform.rotation);
        explosionEff.Play();

        //소리 재생
        //SoundManager._instance.PlayEffectSound(PublicDefine.eEffectSoundType.Fire_Grenade);

        Destroy(explosionEff.gameObject, 3.7f);

        SoundManager._instance.PlayEffectSound(PublicDefine.eEffectSoundType.Explode_Grenade);

        Collider[] colliders = Physics.OverlapSphere(transform.position, _explosionRadius);

        foreach(Collider hit in colliders)
        {
            if (Vector3.Distance(hit.gameObject.transform.position, transform.position) <= 8.0f)
                _explosionDamage = 30;
            else if (Vector3.Distance(hit.gameObject.transform.position, transform.position) <= 10.0f)
                _explosionDamage = 15;
            ZombieController zombieCtrl = hit.GetComponent<ZombieController>();
            if (zombieCtrl != null)
            {
                zombieCtrl.TakeDamge(_explosionDamage);
                continue;
            }
            PlayerController playerCtrl = hit.GetComponent<PlayerController>();
            if(playerCtrl != null)
            {
                playerCtrl.HitDamage((int)(_explosionDamage * 0.2f));
                continue;
            }
            Rigidbody rigid = hit.GetComponent<Rigidbody>();
            if(rigid != null)
            {
                rigid.AddExplosionForce(_explosionForce, transform.position, _explosionRadius);
            }
        }

        Destroy(gameObject);
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            SoundManager._instance.PlayEffectSound(PublicDefine.eEffectSoundType.Fall_On_Grass);
            CountDown();
        }
    }
}
