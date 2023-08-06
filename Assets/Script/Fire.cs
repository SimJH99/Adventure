using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Fire : MonoBehaviour
{
    Animator ani;

    float hitTime;

    void Start()
    {
        ani = GetComponent<Animator>();
    }

    private void Update()
    {
        FirePatten();
    }

    //시간에 따라 애니메이션 순서대로 실행
    void FirePatten()
    {
        hitTime += Time.smoothDeltaTime;

        ani.SetFloat("Time", hitTime);

        if (hitTime >= 4.5f)
        {
            hitTime = 0;
        }
    }
}
