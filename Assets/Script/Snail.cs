using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEditor;
using UnityEngine;

public class Snail : MonoBehaviour
{
    Animator ani;
    Rigidbody rigid;
    SpriteRenderer sprite;
    [SerializeField] GameObject target;
    [SerializeField] float raidus;
    [SerializeField] float speed;

    bool isTrace;

    private void Awake()
    {
        ani = GetComponent<Animator>();
        rigid = GetComponent<Rigidbody>();
        sprite = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        PlayerCheck();
        Trace();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawSphere(transform.position, raidus);
    }

    //플레이어 체크
    //플레이어 추적
    //애니메이션
    //사망

    void PlayerCheck()
    {
        RaycastHit2D rayhit = Physics2D.CircleCast(transform.position, raidus, Vector2.right,1, LayerMask.GetMask("Player"));
        if (rayhit.collider != null)
        {
            isTrace = true;
        }
    }

    void Trace()
    {
        if (isTrace)
        {
            Vector3 playerPos= target.transform.position;

            if (playerPos.x > transform.position.x)
            {
                transform.localScale = new Vector3(1f, 1f, 1f);
            }
            else if (playerPos.x < transform.position.x)
            {
                transform.localScale = new Vector3(-1f, 1f, 1f);
            }

            transform.position = Vector2.MoveTowards(transform.position, target.transform.position, speed * Time.fixedDeltaTime);
        }
    }
}
