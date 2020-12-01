using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlankTile : MonoBehaviour
{
    PlayerManager manager;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            manager = collision.gameObject.GetComponent<PlayerManager>();
            StartCoroutine(Action());
        }
    }

    IEnumerator Action()
    {
        manager.EmptyTile();
        yield return new WaitForSeconds(0.5f);
        manager.EnterRollingState();

    }
}
