using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletObject : MonoBehaviour
{
    PlayerController _playerCtrl;
    Rigidbody _rigid;

    float _force = 0;

    public PlayerController _owner
    {
        get { return _playerCtrl; }
    }

    void Awake()
    {
        _rigid = GetComponent<Rigidbody>();
    }

    void Start()
    {
        _rigid.AddForce(transform.forward * _force);
    }

    
    public void InitSetData(PlayerController owner, float pow)
    {
        _playerCtrl = owner;
        _force = pow;
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer != LayerMask.NameToLayer("WEAPON"))
            Destroy(gameObject);
        if (other.gameObject.layer == LayerMask.NameToLayer("ITEM"))
        {
            Destroy(gameObject);
            Destroy(other.gameObject);
        }
        else
            Destroy(gameObject, 5);
    }
}
