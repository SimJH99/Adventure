using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEditor;
using UnityEngine;

public class Snail : MonoBehaviour
{
    Animator ani;
    Rigidbody rigid;
    [SerializeField] Vector3 spawnPoint;
    [SerializeField] GameObject target;
    [SerializeField] GameMng gameMng;
    [SerializeField] float raidus;
    [SerializeField] float speed;

    bool isTrace;

    private void Awake()
    {
        ani = GetComponent<Animator>();
        rigid = GetComponent<Rigidbody>();
    }

    void Update()
    {
        PlayerCheck();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawSphere(transform.position, raidus);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "JumpTrigger")
        {
            ani.SetTrigger("Hit");
        }
    }

    //�÷��̾� ���� üũ
    void PlayerCheck()
    {
        RaycastHit2D rayhit = Physics2D.CircleCast(transform.position, raidus, Vector2.right,1, LayerMask.GetMask("Player"));
        if (rayhit.collider != null)
        {
            Trace();
        }
        else
        {
            ani.SetBool("Run", false);
        }
    }

    //�÷��̾� ����
    void Trace()
    {
        Vector3 playerPos = target.transform.position;
        Vector3 pos = target.transform.position - transform.position;

        //�̵��� ���� ���� ȸ��
        if (playerPos.x > transform.position.x)
        {
            transform.localScale = new Vector3(-1f, 1f, 1f);
            ani.SetBool("Run", true);
        }
        else if (playerPos.x < transform.position.x)
        {
            transform.localScale = new Vector3(1f, 1f, 1f);
            ani.SetBool("Run", true);
        }

        transform.position += pos * speed * Time.fixedDeltaTime;
    }

    //������ ���
    void Dead()
    {
        gameObject.SetActive(false);
        gameMng.killPoint += 300;
    }

    //�÷��̾� ��� �� ������ ������
    public void Respawn()
    {
        if (gameObject.activeSelf == false)
        {
            gameObject.SetActive(true);
            transform.localPosition = spawnPoint;
        }
    }
}
