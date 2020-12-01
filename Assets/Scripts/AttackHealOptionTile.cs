using System.Collections;
using UnityEngine;
using TMPro;

public class AttackHealOptionTile : MonoBehaviour
{
    public float amount;

    PlayerStats player;
    PlayerManager manager;
    public TextMeshProUGUI dialogUI;
    public bool onTile = false; 


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            onTile = true;
            dialogUI = FindObjectOfType<DialogText>().GetComponent<TextMeshProUGUI>();
            player = collision.gameObject.GetComponent<PlayerStats>();
            manager = collision.gameObject.GetComponent<PlayerManager>();
            StartCoroutine(Action());
        }
    }


    IEnumerator Action()
    {
        dialogUI.text = $"Press Space to Heal for {amount} or Press W to Gain {amount} attack";
        yield return new WaitForSeconds(1f);
       
    }
}
