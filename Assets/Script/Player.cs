using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.U2D;
using UnityEngine;

public class Player : MonoBehaviour
{
    SpriteRenderer sprite;
    Camera cam;
    public Rigidbody2D rigid;
    public Animator ani;

    [SerializeField] Enemy enemy;
    [SerializeField] GameMng gameMng;
    [SerializeField] float jumpPower;
    [SerializeField] float moveSpeed;

    public bool isJumping = false;
    public bool isDoubleJump;
    public float jumpCount;

    float isRight = 1; //�ٶ󺸴� ���� 1 = ������, -1 = ����
    public Transform wallcheck;
    public float wallcheckDistance;
    public LayerMask wallLayer;

    bool isWall;
    public float slidingSpeed;
    public float wallJumpPower;
    public bool isWallJump;

    Vector2 moveDir;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        ani = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
        cam = Camera.main;
    }

    void Update()
    {
        Move();
        Jump();
        ObjectOutCamera();
        
        WallJump();

        //�� üũ�ϱ�
        isWall = Physics2D.Raycast(wallcheck.position, Vector2.right * isRight, wallcheckDistance, wallLayer);
        ani.SetBool("isSliding", isWall);

    }

    private void FixedUpdate()
    {
        if (!isWallJump)
        {
            moveDir.y = rigid.velocity.y;
            rigid.velocity = moveDir;
        }
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            Dead();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Finish")
        {
            gameMng.NextStage();
            gameMng.stagePoint += 500;
        }
        else if (collision.gameObject.tag == "Traps")
        {
            Dead();
        }
        else if (collision.gameObject.tag == "Item")
        {
            bool isApple = collision.gameObject.name.Contains("Apple");
            bool isBananas = collision.gameObject.name.Contains("Bananas");
            bool isCherries = collision.gameObject.name.Contains("Cherries");

            if (isApple)
            {
                gameMng.stagePoint += 50;
            }
            else if (isBananas)
            {
                gameMng.stagePoint += 100;
            }
            else if (isCherries)
            {
                gameMng.stagePoint += 200;
            }
            collision.gameObject.SetActive(false);
        }
    }

    //WallLayer Ȯ���ϱ�
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawRay(wallcheck.position, Vector2.right * isRight * wallcheckDistance);
    }

    private void Move()
    {
        //�÷��̾� �̵�
        float hor = Input.GetAxisRaw("Horizontal");

        moveDir.x = hor * moveSpeed;

        //�÷��̾� �ִϸ��̼�
        ani.SetInteger("hor", Mathf.Abs((int)hor));

        //�÷��̾� ���� ��ȯ
        if (!isWallJump)
        {
            if ((moveDir.x > 0 && isRight < 0) || (moveDir.x < 0 && isRight > 0))
            {
                FlipPlayer();
            }
        }
    }

    //�÷��̾� ���� ��ȯ
    void FlipPlayer()
    {
        transform.eulerAngles = new Vector3(0, Mathf.Abs(transform.eulerAngles.y - 180), 0);
        isRight = isRight * -1;
    }

    private void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isJumping == true)
        {
            Debug.Log("input Jump");
            moveDir.y = jumpPower;
            rigid.velocity = moveDir;
            jumpCount--;
            isDoubleJump = true;
            ani.SetBool("Jump", true);

            if (jumpCount == 0)
            {
                isJumping = false;
                ani.SetBool("DoubleJump", isDoubleJump);
            }
        }
    }

    private void WallJump()
    {
        if (isWall)
        {
            //���� �Ŵ޷��� �� �̲�����
            jumpCount = 1;
            isDoubleJump = true;
            isWallJump = false;
            rigid.velocity = new Vector2(rigid.velocity.x, rigid.velocity.y * slidingSpeed);
        }

        
        if (Input.GetKeyDown(KeyCode.Space) && isWall)
        {
            isWallJump = true;
            Invoke("FreezeX", 0.3f);
            rigid.AddForce(new Vector2(-isRight * wallJumpPower, 0.9f * wallJumpPower), ForceMode2D.Impulse);
            FlipPlayer();
        }
    }
    void FreezeX()
    {
        isWallJump = false;
    }

    //�� ������ �������� �� ���
    private void ObjectOutCamera()
    {
        Vector3 CamOut = Camera.main.WorldToViewportPoint(transform.position);
        if (CamOut.x < -0f || CamOut.x > 1f || CamOut.y < -0f || CamOut.y > 1f)
        {
            Dead();
        }
    }

    //�׾��� �� ��ŸƮ��ġ�� �ʱ�ȭ
    private void Dead()
    {
        //������������ ������
        gameObject.SetActive(false);
        if (gameMng.stageIndex < gameMng.Stages.Length)
        {
            transform.position = gameMng.StartPoints[gameMng.startIndex].transform.position;
        }
        gameObject.SetActive(true);

        gameMng.deathPoint -= 100;
        enemy.Respawn();
    }
}