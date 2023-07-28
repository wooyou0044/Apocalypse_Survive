using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapCameraControl : MonoBehaviour
{
    Transform _targetPos;

    Camera _cam;

    bool _isActivate;

    PlayerController _playerCtrl;

    void Awake()
    {
        _cam = GetComponent<Camera>();
    }

    void Update()
    {
        if(PlayerController._isPlayerActivated)
        {
            _isActivate = true;
        }

        if(_isActivate)
        {
            GameObject go = GameObject.FindGameObjectWithTag("Player");
            _targetPos = go.transform;
            transform.position = new Vector3(_targetPos.position.x, transform.position.y, _targetPos.position.z);
            transform.LookAt(_targetPos);
        }


    }
}
