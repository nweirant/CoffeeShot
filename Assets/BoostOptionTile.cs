using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoostOptionTile : MonoBehaviour
{
    private bool interacted;
    PlayerStats player;
    PlayerManager manager;

    public SpriteRenderer _sprite;
    public string _increasedStat;
    public string _decreasedStat;
    [Range(1f, 200f)]
    public float _amountInc;
    [Range(.2f, .9f)]
    public float _amountDecPercentage;

    private float _amountDec;

    private float _playerAttack;
    private float _playerHealth;
    private float _playerOxygen;
    private float _playerCoins;

    public float coinCost;
    private bool merchant;

    private void Start()
    {
        if(coinCost > 0)
        {
            merchant = true;
        }
    }

    void GetPlayerStats()
    {
        _playerAttack = player._attack;
        _playerHealth = player._health;
        _playerOxygen = player._oxygen;
        _playerCoins = player._coins;
    }

    float GetDecValue(float stat)
    {
        float boost = stat * _amountDecPercentage;
        _amountDec = boost;
        return (boost);
    }

    void GetIncValue()
    {
        //float boost = stat * _amountIncPercentage;
        //_amountDec = boost;
        //return (stat + boost);
    }

    void Expire()
    {
        Destroy(gameObject);
    }

    public void Accept()
    {
        StartCoroutine(AcceptAction());
        Invoke("Expire", 30);
    }

    public void Decline()
    {
        StartCoroutine(DeclineAction());
        Invoke("Expire", 30);
    }


    IEnumerator AcceptAction()
    {
        if(merchant && _playerCoins < coinCost)
        {
            manager.UpdateDialog("Not enough Gold!");
      
        }

        else if (!interacted)
        {
            if (_increasedStat == "health")
            {
                player.Heal(_amountInc);
            }
            if (_decreasedStat == "health")
            {
                player.Heal(-GetDecValue(_playerHealth));
            }
            if (_increasedStat == "attack")
            {
                player.GainAttack(_amountInc);
            }
            if (_decreasedStat == "attack")
            {
                player.GainAttack(-GetDecValue(_playerAttack));
            }
            if (_increasedStat == "oxygen")
            {
                player.GainOxygen(_amountInc);
            }
            if (_decreasedStat == "oxygen")
            {
                player.LoseOxygen(GetDecValue(_playerOxygen));
            }
            if (_increasedStat == "gold")
            {
                player.GainCoins(_amountInc);
            }
            if (_decreasedStat == "gold")
            {
                player.GainCoins(-coinCost);
            }
            interacted = true;
        }
        else
        {
            manager.UpdateDialog("No Returns!");

        }

        yield return new WaitForSeconds(1f);
        manager.EnterRollingState();

    }

    IEnumerator DeclineAction()
    {
        interacted = true;
        manager.UpdateDialog("Good Luck");
        yield return new WaitForSeconds(1f);
        manager.EnterRollingState();

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            player = collision.gameObject.GetComponent<PlayerStats>();
            manager = collision.gameObject.GetComponent<PlayerManager>();
            GetPlayerStats();
            manager.SetCurrentBoost(this);
            manager.EnterMarket();
            manager.SetDialogImage(_sprite.sprite);

            if(!interacted && merchant)
            {
                if(_decreasedStat == "gold")
                {
                    manager.UpdateDialog($"Pay {coinCost} to increase {_increasedStat} by {_amountInc}?");
                } else if (_increasedStat == "gold")
                {
                    manager.UpdateDialog($"Recieve {_amountInc} by decreasing {_decreasedStat} by {_amountDecPercentage * 100}%?.");
                }

            }

            else if (!interacted && !merchant)
            {
                manager.UpdateDialog($"Increase {_increasedStat} by {_amountInc} and Decrease {_decreasedStat} by {_amountDecPercentage*100}%");
            } else
            {
                manager.UpdateDialog("... you're still here?");
            }
        }  

    }
}
