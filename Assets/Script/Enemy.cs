using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    Rigidbody2D rigid;
    Animator ani;
    SpriteRenderer sprite;
    [SerializeField] GameObject player;
    [SerializeField] GameMng gameMng;

    public int nextMove;

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        ani = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();

        Invoke("Think", 3);
    }

    void FixedUpdate()
    {
        rigid.velocity = new Vector2(nextMove, rigid.velocity.y);

        PlatformCheck();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "JumpTrigger")
        {
            ani.SetTrigger("Hit");
        }
    }

    //���� üũ
    void PlatformCheck()
    {
        Vector2 frontVec = new Vector2(rigid.position.x + nextMove * 0.3f, rigid.position.y);
        Debug.DrawRay(frontVec, Vector3.down, new Color(0, 1, 0));
        RaycastHit2D rayhit = Physics2D.Raycast(frontVec, Vector3.down, 1, LayerMask.GetMask("Ground"));
        //if (rayhit.collider == null)
        //{
        //    Turn();
        //}
    }

    //�ൿ ���� ��,�� �̵� or ���ֱ� //����Լ�
    void Think()
    {
        nextMove = Random.Range(-1, 2);

        ani.SetInteger("Walk", nextMove);

        if (nextMove != 0)
        {
            sprite.flipX = (nextMove == 1);
        }

        float nextThinkTime = Random.Range(2f, 5f);
        Invoke("Think", nextThinkTime);
    }


    void Turn()
    {
        nextMove = nextMove * (-1);
        sprite.flipX = (nextMove == 1); //nextMove�� 1�̸� ����ٲٱ�
        CancelInvoke();
        Invoke("Think", 2);
    }

    void Death()
    {
        gameObject.SetActive(false);
        gameMng.killPoint += 300;
    }

    public void Respawn()
    {
        if (gameObject.activeSelf == false)
        {
            gameObject.SetActive(true);
            nextMove = 0;
            CancelInvoke();
            Invoke("Think", 2);
        }
    }
}
//private void OnTriggerEnter2D(Collider2D collision)
//{
//    if (collision.gameObject.tag == "Player")
//    {
//        traceTarget = collision.gameObject;

//        StopCoroutine("Think");
//    }
//}

//private void OnTriggerStay2D(Collider2D collision)
//{
//    if (collision.gameObject.tag == "Player")
//    {
//        isTracing = true;
//    }
//}

//private void OnTriggerExit2D(Collider2D collision)
//{
//    if (collision.gameObject.tag == "Player")
//    {
//        isTracing = false;
//        StartCoroutine("Think");
//    }
//}

//void Trace()
//{
//    Vector3 moveVelocity = Vector3.zero;

//    if (isTracing == true)
//    {
//        StopCoroutine("Think");
//        Vector3 playerPos = traceTarget.transform.position;

//        if (playerPos.x < transform.position.x)
//        {
//            moveVelocity = new Vector2(-1, rigid.velocity.y);
//            //ani.SetInteger("Walk", -1);
//        }
//        else if (playerPos.x > transform.position.x)
//        {
//            moveVelocity = new Vector2(1, rigid.velocity.y);
//        }
//    }

//    transform.position += moveVelocity * Time.deltaTime;
//}