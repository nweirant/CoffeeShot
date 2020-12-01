using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealingTile : MonoBehaviour
{
    public float _healAmount;
    public GameObject healParts;

    PlayerStats player;
    PlayerManager manager;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Instantiate(healParts, transform.position + new Vector3(3.4f, 0), Quaternion.Euler(0, 0, 90));
            player = collision.gameObject.GetComponent<PlayerStats>();
            manager = collision.gameObject.GetComponent<PlayerManager>();
            StartCoroutine(Action());
        }
    }

    IEnumerator Action()
    {
        player.Heal(_healAmount);
        yield return new WaitForSeconds(1f);
        manager.EnterRollingState();

    }
}
