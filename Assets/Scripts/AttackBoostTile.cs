using UnityEngine;
using System.Collections;

public class AttackBoostTile : MonoBehaviour
{
    public float _amount;
    PlayerStats player;
    PlayerManager manager;
    public GameObject attackParts;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Instantiate(attackParts, transform.position + new Vector3(3.4f, 0), Quaternion.Euler(0, 0, 90));
            player = collision.gameObject.GetComponent<PlayerStats>();
            manager = collision.gameObject.GetComponent<PlayerManager>();
            StartCoroutine(Action());
        }
    }

    IEnumerator Action()
    {
        player.GainAttack(_amount);
        yield return new WaitForSeconds(1f);
        manager.EnterRollingState();

    }
}

