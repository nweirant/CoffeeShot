using UnityEngine;
using System.Collections;

public class DefenseBoostTile : MonoBehaviour
{
    public float _amount;
    PlayerStats player;
    PlayerManager manager;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            player = collision.gameObject.GetComponent<PlayerStats>();
            manager = collision.gameObject.GetComponent<PlayerManager>();
            StartCoroutine(Action());
        }
    }

    IEnumerator Action()
    {
        player.GainDefense(_amount);
        yield return new WaitForSeconds(0.5f);
        manager.EnterRollingState();

    }
}
