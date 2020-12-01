using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class PlayerStats : MonoBehaviour
{
    public float _startingHealth;
    public float _startingAttack;
    public float _startingDefense;
    public float _startingOxygen;
    public float _health;
    public float _attack;
    public float _defense;
    public float _oxygen;
    public bool _outOfOxygen = false;
    public float _oxygenHealthDecay = 1f;
    public float _coins;
    [Range(1f, 100f)]
    public int _critChance;
    public PlayerManager manager;
    public PlayerMovement movement;

    public TextMeshProUGUI attackUI;
    public TextMeshProUGUI healthUI;
    public TextMeshProUGUI oxygenUI;
    public TextMeshProUGUI goldUI;
    public TextMeshProUGUI critUI;
    public int _increaseCritAmount = 2;



    public TextMeshProUGUI dialogUI;

    public Animator anim;

    public GameObject healthParts;
    public GameObject oxygenParts;
    public GameObject attackParts;

    private void Start()
    {
        _health = _startingHealth;
        _attack = _startingAttack;
        _defense = _startingDefense;
        _oxygen = _startingOxygen;
        UpdateUI();
    }

    public void Spawn(GameObject go)
    {
        Instantiate(go, transform.position, Quaternion.identity);
    }

    public void UpdateUI()
    {
        attackUI.text = _attack.ToString();
        healthUI.text = _health.ToString();
        oxygenUI.text = _oxygen.ToString();
        goldUI.text = _coins.ToString();
        critUI.text = _critChance.ToString() + "%";
    }

    public void Heal(float amount)
    {
        Spawn(healthParts);
        _health += amount;
        dialogUI.text = "Gained " + amount + " Health";
        UpdateUI();
    }

    public void increaseCritChance()
    {
        _critChance += _increaseCritAmount;
        dialogUI.text = "Critchange incrased by " + _increaseCritAmount;

    }

    public void GainCoins(float amount)
    {
        _coins += amount;
        _coins = Mathf.Ceil(_coins);
        UpdateUI();
    }

    public void GainOxygen(float amount)
    {
        _oxygen += amount;
        _oxygen = Mathf.Ceil(_oxygen);
        Spawn(oxygenParts);

        if (_oxygen >= 0)
        {
            _outOfOxygen = false;
        }

        dialogUI.text = "Gained " + amount + " Oxygen";  
        UpdateUI();
    }

    public bool CritAttack()
    {
        if(_critChance >= Random.Range(0,100))
        {
            return true;
        }
        return false;
    }

    public void LoseOxygen(float amount=1)
    {

        if(_oxygen <= 0)
        {
            _oxygen = 0;
            _outOfOxygen = true;
            Debug.Log("No More Oxygen, will now lose 1 heal per space moved");
            TakeDamage(_oxygenHealthDecay);
        } else
        {
            _oxygen -= amount;
            _oxygen = Mathf.Ceil(_oxygen);
        }
        UpdateUI();
    }

    public bool HasOxygen()
    {
        return _outOfOxygen;
    }

    public void GainAttack(float amount)
    {
        Spawn(attackParts);
        dialogUI.text = "Gained " + amount + " Attack";
        _attack += amount;
        _attack = Mathf.Ceil(_attack);
        UpdateUI();
    }

    public void GainDefense(float amount)
    {
        Debug.Log("Gained Defense:  " + amount);
        _defense += amount;
        UpdateUI();
    }

    public void TakeDamage(float amount)
    {
        _health -= amount;
        _health = Mathf.Ceil(_health);
        if(_oxygen > 0)
        {
            anim.SetTrigger("hit");
        }
        UpdateUI();
        if (_health <= 0)
        {
            manager.EnterFallingState();
            GameOver();
        }
    }

    public void OnMoon()
    {
        _health = 999;
        _oxygen = 999;
    }

    public void GameOver()
    {
        dialogUI.text = "Game Over";

        //int scene = SceneManager.GetActiveScene().buildIndex;
        //SceneManager.LoadScene(scene, LoadSceneMode.Single);
    }
}
