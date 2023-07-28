using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatusMiniWindow : MonoBehaviour
{
    [SerializeField] Slider _zombieHp;

    Transform _originPos;
    Camera _camera;

    float _visibleTime = 3;
    float _timeCheck = 0;

    ZombieController _zombieCtrl;

    void Awake()
    {
        _zombieCtrl = GetComponentInParent<ZombieController>();
    }

    void LateUpdate()
    {
        if(_zombieCtrl.CurrentState == PublicDefine.eEnemyActionState.IDLE || _zombieCtrl.CurrentState == PublicDefine.eEnemyActionState.WALK)
        {
            if (_zombieHp.gameObject.activeSelf)
            {
                _timeCheck += Time.deltaTime;
                if (_timeCheck >= _visibleTime)
                    EnableWindow(false);
            }
        }
    }

    public void InitSetWindow(float hpRate, Transform pos)
    {
        SetHPValue(hpRate);
        _originPos = pos;
        _camera = Camera.main;
    }

    public void SetHPValue(float rate)
    {
        _zombieHp.value = rate;
    }

    public void EnableWindow(bool isOn)
    {
        _zombieHp.gameObject.SetActive(isOn);
    }
}
