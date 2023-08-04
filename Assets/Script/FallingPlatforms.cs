using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class FallingPlatforms : MonoBehaviour
{
    Rigidbody2D rigid;
    Animator ani;
    Player player;

    Vector2 firstPosition;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        ani = GetComponent<Animator>();

        firstPosition = transform.position;

        rigid.bodyType = RigidbodyType2D.Kinematic;
        rigid.gravityScale = 0;
    }

    void Update()
    {
        ObjectOutCamera();
        Respwan();
    }

    //ÇÃ·§ÆûÀÌ ¶³¾îÁü.
    IEnumerator Falling()
    { 
        yield return new WaitForSeconds(0.5f);
        rigid.bodyType = RigidbodyType2D.Dynamic;
        rigid.gravityScale = 1;
        ani.SetBool("Touch", true);
    }

    //ÇÃ·§Æû ¸®½ºÆù
    private void Respwan()
    {
        if (gameObject.activeSelf == false)
        {
            transform.position = firstPosition;
            gameObject.SetActive(true);
            rigid.bodyType = RigidbodyType2D.Kinematic;
            rigid.gravityScale = 0;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "JumpTrigger")
        {
            StartCoroutine(Falling());
        }
    }

    //ÇÃ·§ÆûÀÌ ¸Ê¹ÛÀ¸·Î ³ª°¬À» ¶§, ¸®½ºÆù
    private void ObjectOutCamera()
    {
        Vector3 CamOut = Camera.main.WorldToViewportPoint(transform.position);
        if (CamOut.x < -0.5f || CamOut.x > 1.5f || CamOut.y < -0.5f || CamOut.y > 1.5f)
        {
            gameObject.SetActive(false);
            StopCoroutine(Falling());
        }
    }
}
