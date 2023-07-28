using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] float _rotateSpeed = 2.0f;
    float _mouseY;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        _mouseY += Input.GetAxis("Mouse Y") * _rotateSpeed;
        _mouseY = Mathf.Clamp(_mouseY, -25.0f, 25.0f);
        Debug.Log(_mouseY);
        transform.localEulerAngles = new Vector3(-_mouseY, 0, 0);
    }
}
