using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trampoline : MonoBehaviour
{
    Animator ani;    

    private void Awake()
    {
        ani = GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "JumpTrigger")
        {
            Animation();
        }
    }

    void Animation()
    {
        ani.SetTrigger("Touch");
        ani.SetTrigger("Idle");
    }

}
