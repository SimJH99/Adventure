using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestionMark : MonoBehaviour
{
    [SerializeField] GameObject textBox;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            textBox.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            textBox.SetActive(false);
        }
    }
}
