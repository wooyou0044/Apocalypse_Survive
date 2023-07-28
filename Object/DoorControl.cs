using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorControl : MonoBehaviour
{
    [SerializeField] float _waitTime = 2.0f;
    [SerializeField] PublicDefine.eBuildingType _buildingType;

    float _timeCheck = 0;
    bool _isPlayerIn;

    PlayerController _playerCtrl;
    IngameManager _ingameManage;
    SceneControlManager _sceneControl;

    BoxCollider _collider;

    void Awake()
    {
        _collider = GetComponent<BoxCollider>();
        _ingameManage = GameObject.Find("IngameManagerObject").GetComponent<IngameManager>();
        _sceneControl = GameObject.Find("SceneManagerObject").GetComponent<SceneControlManager>();
    }

    void Update()
    {
        if (_isPlayerIn)
        {
            _timeCheck += Time.deltaTime;
            if (_timeCheck >= _waitTime)
            {
                if (_playerCtrl == null)
                    _isPlayerIn = false;
                else
                {
                    _playerCtrl.ClearMapPlayer();
                    _timeCheck = 0;
                    _isPlayerIn = false;
                }
            }
        }

        if (_ingameManage._isCloseDoor)
        {
            if(_buildingType == PublicDefine.eBuildingType.Bank)
            {
                _collider.enabled = false;
                _ingameManage._isCloseDoor = false;
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _playerCtrl = other.GetComponent<PlayerController>();
            if (_buildingType == PublicDefine.eBuildingType.ClearMap)
            {
                _isPlayerIn = true;
                _timeCheck = 0;
                Debug.Log("워프에 들어갔음");
            }
            else
            {
                _playerCtrl.GoToOtherBuilding(_buildingType);
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if(_buildingType == PublicDefine.eBuildingType.ClearMap)
        {
            if (other.CompareTag("Player"))
            {
                _isPlayerIn = false;
                _timeCheck = 0;
                _playerCtrl = null;
                Debug.Log("워프에서 나감");
            }
        }
    }
}
