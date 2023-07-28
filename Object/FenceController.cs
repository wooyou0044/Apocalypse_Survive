using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FenceController : MonoBehaviour
{
    int _life;
    bool _isCrash;

    float _timeCheck = 0;

    BoxCollider _collisionZone;

    void Awake()
    {
        _life = 30;
        _collisionZone = GetComponent<BoxCollider>();
    }

    void Update()
    {
        if (_isCrash)
        {
            _collisionZone.enabled = false;
            _timeCheck += Time.deltaTime;
            if (_timeCheck >= 5f)
            {
                _collisionZone.enabled = true;
                _isCrash = false;
                _timeCheck = 0;
            }
        }
    }

    void LateUpdate()
    {
        if (_life <= 0)
        {
            Destroy(transform.root.gameObject);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy1") || other.CompareTag("Enemy2"))
        {
            _life--;
            _isCrash = true;
        }
    }
}
