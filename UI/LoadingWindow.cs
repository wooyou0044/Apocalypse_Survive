using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadingWindow : MonoBehaviour
{
    [SerializeField] Transform _loadingIcon;

    float _speed = 0.5f;
    float _timeCheck = 0;

    void Update()
    {
        _loadingIcon.Rotate(Vector3.forward * Time.deltaTime * 90);
    }
}
