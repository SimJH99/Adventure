using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class JumpTrigger : MonoBehaviour
{
    [SerializeField] Player player;
    [SerializeField] float jumpZoneForce;
    [SerializeField] float enemyJumpForce;

    bool isJumpZone = false;
    bool enemyJump = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Ground" || collision.gameObject.tag
            == "Platforms" || collision.gameObject.tag == "Start")
        {
            player.isJumping = true;
            //player.isDoubleJump = false;
            player.jumpCount = 2;
            player.ani.SetBool("Jump", false);
            player.ani.SetBool("DoubleJump", false);
            player.ani.SetBool("isFall", false);
        }
        else if (collision.gameObject.tag == "Trampoline")
        {
            isJumpZone = true;
            Jumping();
        }
        else if (collision.gameObject.tag == "Enemy")
        {
            enemyJump = true;
            Enemyjump();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            player.ani.SetBool("isFall", true);
        }
    }

    //Trampoline
    void Jumping()
    {
        if (isJumpZone)
        {
            player.rigid.velocity = Vector3.zero;
            player.rigid.AddForce(new Vector2(0, jumpZoneForce), ForceMode2D.Impulse);
            player.jumpCount = 1;
            isJumpZone = false;
        }
    }

    //Enemy
    void Enemyjump()
    {
        if (enemyJump)
        {
            player.rigid.velocity = Vector3.zero;
            player.rigid.AddForce(new Vector2(0, enemyJumpForce), ForceMode2D.Impulse);
            player.jumpCount = 1;
            enemyJump = false;
        }
    }

}
