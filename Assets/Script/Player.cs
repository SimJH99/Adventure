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

    [SerializeField] Enemy[] enemy;
    [SerializeField] Snail[] snail;
    [SerializeField] GameMng gameMng;
    [SerializeField] float jumpPower;
    [SerializeField] float moveSpeed;

    public bool isJumping = false;
    public bool isDoubleJump;
    public float jumpCount;

    float isRight = 1; //바라보는 방향 1 = 오른쪽, -1 = 왼쪽
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
        WallJump();

        //벽 체크하기
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
            ani.SetTrigger("Hit");
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
            ani.SetTrigger("Hit");
        }
        else if (collision.gameObject.tag == "DeathZone")
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

    //WallLayer 확인하기
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawRay(wallcheck.position, Vector2.right * isRight * wallcheckDistance);
    }

    private void Move()
    {
        //플레이어 이동
        float hor = Input.GetAxisRaw("Horizontal");

        moveDir.x = hor * moveSpeed;

        //플레이어 애니메이션
        ani.SetInteger("hor", Mathf.Abs((int)hor));

        //플레이어 방향 전환
        if (!isWallJump)
        {
            if ((moveDir.x > 0 && isRight < 0) || (moveDir.x < 0 && isRight > 0))
            {
                FlipPlayer();
            }
        }
    }

    //플레이어 방향 전환
    void FlipPlayer()
    {
        transform.eulerAngles = new Vector3(0, Mathf.Abs(transform.eulerAngles.y - 180), 0);
        isRight = isRight * -1;
    }

    private void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isJumping == true)
        {
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
            //벽에 매달렸을 때 미끄러짐
            jumpCount = 1;
            isDoubleJump = true;
            isWallJump = false;
            rigid.velocity = new Vector2(rigid.velocity.x, rigid.velocity.y * slidingSpeed);
        }

        
        if (Input.GetKeyDown(KeyCode.Space) && isWall)
        {
            isWallJump = true;
            Invoke("FreezeX", 0.3f);
            rigid.AddForce(new Vector2(-isRight * wallJumpPower, wallJumpPower), ForceMode2D.Impulse);
            FlipPlayer();
        }
    }
    void FreezeX()
    {
        isWallJump = false;
    }

    //죽었을 때 스타트위치로 초기화
    private void Dead()
    {
        //시작지점으로 리스폰
        gameObject.SetActive(false);
        if (gameMng.stageIndex < gameMng.Stages.Length)
        {
            transform.position = gameMng.StartPoints[gameMng.startIndex].transform.position;
        }
        gameObject.SetActive(true);
        gameMng.deathPoint -= 100;

        //2stage 적 스폰
        if (gameMng.Stages[1].gameObject.activeSelf == true)
        {
            for (int i = 0; i < enemy.Length - 1; i++)
            {
                if (enemy[i].gameObject.activeSelf == false)
                {
                    enemy[i].Respawn();
                }
            }
        }


        //3stage 적 스폰
        if (gameMng.Stages[2].gameObject.activeSelf == true)
        {
            if (snail[0].gameObject.activeSelf == false)
            {
                snail[0].Respawn();
            }
            if (snail[1].gameObject.activeSelf == false)
            {
                snail[1].Respawn();
            }
            if (enemy[2].gameObject.activeSelf == false)
            {
                enemy[2].Respawn();
            }
        }
    }
}