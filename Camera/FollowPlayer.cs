using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    PlayerController _playerCtrl;
    EyeCast _playerCam;

    bool _isPlayer;

    void Start()
    {
        
    }

    void Update()
    {
        if(PlayerController._isPlayerActivated)
        {
            _playerCtrl = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
            _playerCam = _playerCtrl.GetComponentInChildren<EyeCast>();
            _isPlayer = true;
        }

        if(_isPlayer)
        {
            if(_playerCtrl.currnetBuilding != PublicDefine.eBuildingType.Bank)
            {
                transform.position = _playerCam.gameObject.transform.position;
                transform.rotation = _playerCam.gameObject.transform.rotation;
            }
        }
    }
}
