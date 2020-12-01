using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EnemyTile : MonoBehaviour
{
    public bool boss;
    public Enemy[] enemies;
    public Enemy[] enemiesLvl2;
    public Enemy[] enemiesLvl1;

    public Enemy enemy;
    public string _name;
    public float _defense;
    public float _attack;
    public float _health;
    public float _bribeAmount;
    public Sprite _art;

    public SpriteRenderer _artPos;

    private PlayerStats stats;
    private PlayerManager manager;

    public TextMeshProUGUI healthUI;
    public TextMeshProUGUI attackUI;
    public TextMeshProUGUI _dialogText;

    public GameObject gfx;

    public GameObject chest;
    public Sprite openChest;
    public float coinAmount;
    public bool hasChest;
    public bool canAttack = false;

    public Animator anim;
    public GameObject goldParts;
    public GameObject _damagePopup;

    private void Start()
    {
        hasChest = true;
        if (!boss)
        {
            if (transform.position.y < 93)
            {
                enemy = enemies[Random.Range(0, enemies.Length)];
            }
            else if (transform.position.y > 93 && transform.position.y <= 230)
            {
                enemy = enemiesLvl1[Random.Range(0, enemies.Length)];

            }
            else if (transform.position.y > 230)
            {
                enemy = enemiesLvl2[Random.Range(0, enemies.Length)];

            }
        }
        Setup();
        UpdateUI();
    }



    void Setup()
    {
        _name = enemy._name;
        _defense = enemy._defense;
        _attack = enemy._attack;
        _health = enemy._health;
        _art = enemy._sprite;
        _artPos.sprite = _art;
        _bribeAmount = Mathf.Ceil((_health + _attack) / 3);
        if(hasChest)
        {
            coinAmount = Mathf.Max(_attack, _health);
        } else
        {
            chest.SetActive(false);
        }
    }

    public void UpdateUI()
    {
        healthUI.text = _health.ToString();
        attackUI.text = _attack.ToString();
    }
    public bool TakeDamage(float amount)
    {
        if (stats.CritAttack())
        {
            _health -= (amount*2);
            _dialogText.text = $"Crit! Attacked {_name} for {amount} damage.";
            DamagePopUp(amount * 2);
        }
        else
        {
            _health -= amount;
            _dialogText.text = $"Attacked {_name} for {amount} damage.";
            DamagePopUp(amount);
        }
        UpdateUI();

        if (_health <= 0)
        {
            _health = 0;
            Die();
            UpdateUI();
            stats.increaseCritChance();

            return true;
        }
        return false;
    }

    void DamagePopUp(float amount)
    {
        Vector3 offset = new Vector3(0f, 1f, 0f);
        GameObject popup = Instantiate(_damagePopup, gfx.transform.position + offset, Quaternion.identity);
        popup.GetComponentInChildren<TextMeshPro>().SetText(amount.ToString());
    }
    void DamagePopUpPlayer(float amount)
    {
        Vector3 offset = new Vector3(-0.8f, 2f, 0f);
        GameObject popup = Instantiate(_damagePopup, manager.transform.position + offset, Quaternion.identity);
        popup.GetComponentInChildren<TextMeshPro>().SetText(amount.ToString());
    }

    void Die()
   {
        Invoke("Expire", 10f);
   }

    void Expire()
    {
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            GameObject player = collision.gameObject;
            _dialogText = FindObjectOfType<DialogText>().GetComponent<TextMeshProUGUI>();
            manager = collision.gameObject.GetComponent<PlayerManager>();
            stats = collision.gameObject.GetComponent<PlayerStats>();
            if(boss && _health <= 0)
            {
                manager.EnterRollingState();
            } else
            {
                manager.SetCurrentEnemy(this);
                manager.SetDialogImage(_art);
                StartBattle();
            }


            if(boss)
            {
                gfx.transform.position = new Vector2(gfx.transform.position.x, player.transform.position.y);
            }
        }
    }

    public void StartBattle()
    {
        StartCoroutine(Battle(stats, manager));
    }

    public void Bribe()
    {
        StartCoroutine(Bribed());
    }

    IEnumerator Bribed()
    {
        _dialogText.text = $"You pay {_bribeAmount} to go about your business";
        stats.GainCoins(-_bribeAmount);
        yield return new WaitForSeconds(1f);
        manager.EnterRollingState();
    }

    IEnumerator Battle(PlayerStats player, PlayerManager manager)
    {
        canAttack = false;
        manager.AttackAnimation();
        if (TakeDamage(player._attack))
        {
            if (!hasChest)
            {
                gfx.SetActive(false);
                _dialogText.text = $"{_name} has been defeated";
            }
            else
            {
                gfx.SetActive(false);
                chest.GetComponent<SpriteRenderer>().sprite = openChest;
                Instantiate(goldParts, gfx.transform.position, Quaternion.identity);
                stats.GainCoins(coinAmount);
                _dialogText.text = $" {_name} has been defeated, you collect {coinAmount} coins and increase crit chance by 2%!";
            }
            yield return new WaitForSeconds(1f);
            manager.EnterRollingState();
        }

        yield return new WaitForSeconds(0.5f);

        if (_health > 0)
        {
            anim.SetTrigger("attack");
            player.TakeDamage(_attack);
            DamagePopUpPlayer(_attack);
            _dialogText.text = $"{_name} hits you for {_attack} damage. Attack or Bribe for {_bribeAmount} gold?";
            if(player._health <= 0)
            {
                _dialogText.text = "Game Over!";
                yield return new WaitForSeconds(0.5f);
                player.GameOver();
            } else
            {
                yield return new WaitForSeconds(0.5f);
                manager.EnterBattleState();
                canAttack = true;
            }

        } 
    }
}
