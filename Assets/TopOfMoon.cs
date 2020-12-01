using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TopOfMoon : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            collision.gameObject.GetComponent<PlayerManager>().StopMovingAnimation();

            Debug.Log("Game Won! player anim");
        }
    }
}
