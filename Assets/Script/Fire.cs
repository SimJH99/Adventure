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
