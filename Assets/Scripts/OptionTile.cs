using System.Collections;
using UnityEngine;

public class OptionTile : MonoBehaviour
{
    public float amount;

    PlayerStats player;
    PlayerManager manager;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            player = collision.gameObject.GetComponent<PlayerStats>();
            manager = collision.gameObject.GetComponent<PlayerManager>();
            manager.SetCurrentOption(this);
            manager.EnterInteractingState();
            manager.UpdateDialog("Gain " + amount + " Health or Attack?");
        }
    }


    IEnumerator Action(string type)
    {
        if(type == "heal")
        {
            player.Heal(amount);
            yield return new WaitForSeconds(1f);
        }
        else if (type == "gainAttack")
        {
            player.GainAttack(amount);
            yield return new WaitForSeconds(1f);
        }
        else if (type == "gainOxygen")
        {
            player.GainOxygen(amount);
            yield return new WaitForSeconds(1f);
        }
        manager.EnterRollingState();
    }

    public void Option1()
    {
        //player.Heal(amount);
        StartCoroutine(Action("heal"));
    }

    public void Option2()
    {
        //player.GainAttack(amount);
        StartCoroutine(Action("gainAttack"));

    }

}
