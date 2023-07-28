using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeControl : MonoBehaviour
{
    [SerializeField] GameObject[] _fireWood;
    [SerializeField] int _maxLife;

    int _woodCount;
    int _nowLife;

    Vector3 _targetPos;

    public int GetLife
    {
        get { return _nowLife; }
    }

    void Awake()
    {
        _nowLife = _maxLife;
        _woodCount = 0;
    }

    void Update()
    {
        if (_nowLife <= 0)
            Destroy(gameObject);
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Axe"))
        {
            _nowLife--;
            SoundManager._instance.PlayEffectSound(PublicDefine.eEffectSoundType.Cut_Tree);
            int rand = Random.Range(0, _fireWood.Length);

            Vector3 _hitPos = other.transform.position;
            Quaternion _hitRot = other.transform.rotation;

            if(_woodCount <= _maxLife)
            {
                Instantiate(_fireWood[rand], _hitPos, Quaternion.Euler(-_hitRot.x, 0, 0));
                _woodCount++;
            }
        }
    }
}
