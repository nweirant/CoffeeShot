using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerNav : MonoBehaviour
{
    public PlayerMovement movement;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "stop")
        {
            Debug.Log("STOP");
            movement.CanMove(false);
            transform.parent.transform.position = Vector2.MoveTowards(transform.parent.transform.position, 
                new Vector2(transform.parent.transform.position.x, collision.gameObject.transform.position.y), movement._speed * Time.deltaTime);
            //Vector3.Lerp(transform.parent.transform.position, collision.gameObject.transform.position, .05f);

        }
    }
}
