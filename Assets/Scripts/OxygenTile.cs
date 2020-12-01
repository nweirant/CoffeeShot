using System.Collections;
using UnityEngine;

public class OxygenTile : MonoBehaviour
{
    public float _healAmount;

    PlayerStats player;
    PlayerManager manager;
    public GameObject oxygenParts;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Instantiate(oxygenParts, transform.position + new Vector3(3.4f, 0), Quaternion.Euler(0, 0, 90));
            player = collision.gameObject.GetComponent<PlayerStats>();
            manager = collision.gameObject.GetComponent<PlayerManager>();
            StartCoroutine(Action());
        }
    }

    IEnumerator Action()
    {
        player.GainOxygen(_healAmount);
        yield return new WaitForSeconds(1f);
        manager.EnterRollingState();

    }
}
