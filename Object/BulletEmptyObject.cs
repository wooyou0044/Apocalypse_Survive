using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletEmptyObject : MonoBehaviour
{
    void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Ground"))
        {
            Destroy(gameObject, 5);
        }

        //if(collision.gameObject.CompareTag("InsideGround"))
        //{
        //    // 빈 총알통 떨어질때 나는 소리 내게 하기
        //    Destroy(gameObject, 10);
        //}
    }
}
