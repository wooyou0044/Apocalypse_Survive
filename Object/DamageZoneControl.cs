using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageZoneControl : MonoBehaviour
{
    BoxCollider _collider;

    void Awake()
    {
        _collider = GetComponent<BoxCollider>();
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            Debug.Log("때렸습니다.");
        }

        if(other.CompareTag("Fence"))
        {
            Debug.Log("펜스에 부딪힘");
            gameObject.SetActive(false);
        }
    }
}
