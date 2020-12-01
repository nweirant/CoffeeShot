using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoonEntry : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            TopOfMoon top = FindObjectOfType<TopOfMoon>();
            collision.gameObject.GetComponent<PlayerStats>().OnMoon();
            collision.gameObject.GetComponent<PlayerMovement>().MoveToTopOfMoon(top.transform.position.y);
        }
    }
}
