using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyGizmoSphere : MonoBehaviour
{
    [SerializeField] Color _myColor = Color.yellow;
    [SerializeField] float _radius = 0.5f;

    void OnDrawGizmos()
    {
        Gizmos.color = _myColor;
        Gizmos.DrawSphere(transform.position, _radius);
    }
}
