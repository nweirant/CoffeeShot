using UnityEngine;
using System.Collections;
using TMPro;
using UnityEngine.UI;
using System;
using UnityEngine.SceneManagement;

public enum PlayerState { ROLLING, MOVING, FIGHTING, INTERACTING, LOCKED, FALLING, CHOOSING, INTRO, FINISHED }

public class PlayerManager : MonoBehaviour
{
    public PlayerMovement movement;
    public PlayerState state;
    public PlayerStats player;
    public Animator anim;

    public int _rolls;
    public int _maxRolls = 2;
    public int _rollAmount;

    public EnemyTile _currentEnemy;
    OptionTile _option;
    BoostOptionTile _boostTile;
    public TextMeshProUGUI _dialogText;
    public TextMeshProUGUI _scoreUI;
    public Image dialogImage;
    public Rigidbody2D rb;

    public GameObject rollButton;
    public GameObject moveButton;
    public GameObject bribeButton;
    public GameObject attackButton;
    public GameObject declineButton;
    public GameObject acceptButton;
    public GameObject gainAttackButton;
    public GameObject gainHealthButton;

    public Animator uiAnim;
    public AudioSource click;

    private void Start()
    {
        ClearDialogImage();
        state = PlayerState.INTRO;
        Intro();
    }


    private void Update()
    {
        if(state == PlayerState.FINISHED)
        {
            if(Input.GetKeyDown(KeyCode.Escape))
            {
                Application.Quit();
            }
        }
        
        if (state == PlayerState.ROLLING)
        {
            RollState();
        }
        if(state == PlayerState.FIGHTING)
        {
            BattleState();
        }
        if(state == PlayerState.INTERACTING)
        {
            InteractingState();
        }
        if(state == PlayerState.CHOOSING)
        {
            ChoosingState();
        }
        if(state == PlayerState.FALLING)
        {
            FallingState(); 
            if(transform.position.y <= 5)
            {
                movement.ToggleCollider(true);
            }
        }
        
    }

    public void CompletedLevel()
    {
        state = PlayerState.FINISHED;
        anim.SetTrigger("sit");
        _dialogText.text = "Theres my coffee!, Mission Complete!";
        uiAnim.SetTrigger("complete");

    }

    private void Intro()
    {
        if(state == PlayerState.INTRO)
        {
            StartCoroutine(IntroScene());
        }
    }

    IEnumerator IntroScene()
    {
        _dialogText.text = "... Guess I'm not taking the ship.";
        yield return new WaitForSeconds(2f);
        EnterRollingState();

    }

    public void SetDialogImage(Sprite sprite, bool clear=false)
    {

       dialogImage.color = Color.white;
       dialogImage.sprite = sprite;
    }

    public void ClearDialogImage()
    {
        dialogImage.color = Color.clear;
    }


    void FallingState()
    {
        _dialogText.text = "Game Over";
        rb.gravityScale = 1;

        
    }

    public void EnterFallingState()
    {
        anim.SetBool("isFalling", true);
        uiAnim.SetTrigger("gameover");
        movement.ToggleCollider(false);
        state = PlayerState.FALLING;
        Invoke("Restart", 2f);
    }

    void Restart()
    {
        int scene = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(scene, LoadSceneMode.Single);
    }

    void InteractingState()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            _option.Option1();
            state = PlayerState.LOCKED;
        }
        if (Input.GetKeyDown(KeyCode.W))
        {
            _option.Option2();
            state = PlayerState.LOCKED;
        }
    }

    void ChoosingState()
    {
        acceptButton.SetActive(true);
        declineButton.SetActive(true);

        /*
        if (Input.GetKeyDown(KeyCode.Space))
        {
            _boostTile.Accept();
            state = PlayerState.LOCKED;
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            _boostTile.Decline();
            state = PlayerState.LOCKED;
        }
        */
    }

    public void EmptyTile()
    {
        _dialogText.text = $"...";
    }

    public void SetCurrentEnemy(EnemyTile enemy)
    {
        anim.SetBool("isMoving", false);
        _currentEnemy = enemy;
    }

    public void StopMovingAnimation()
    {
        anim.SetBool("isMoving", false);
    }

    public void AttackAnimation()
    {
        anim.SetTrigger("attack");
    }

    public void SetCurrentOption(OptionTile tile)
    {
        _option = tile;
    }

    public void SetCurrentBoost(BoostOptionTile tile)
    {
        _boostTile = tile;
    }
    
    void BattleState()
    {
        //Show Attack Buttons 
        if (player._health > 0 && _currentEnemy != null)
        {
            if (Input.GetKeyDown(KeyCode.Space) && _currentEnemy.canAttack)
            {
                _currentEnemy.StartBattle();
            }

            if (Input.GetKeyDown(KeyCode.B))
            {
                if(_currentEnemy._bribeAmount <= player._coins)
                {
                    _currentEnemy.Bribe();

                } else
                {
                    _dialogText.text = $"Not enough gold to bribe...";
                }
            }
        }
    }

    // ** BUTTONS ***************
    public void AttackButton()
    {
        
        if (player._health > 0 && _currentEnemy != null && _currentEnemy.canAttack)
        {
            _currentEnemy.StartBattle();
        }
        attackButton.SetActive(false);
        bribeButton.SetActive(false);
    }

    public void BribeButton()
    {
        if (_currentEnemy._bribeAmount <= player._coins)
        {
            click.Play();
            _currentEnemy.Bribe();
            attackButton.SetActive(false);
            bribeButton.SetActive(false);
        } else
        {
            _dialogText.text = $"Not enough gold to bribe...";
        }
    }

    public void MoveButton()
    {
        click.Play();
        if (_rolls > 0)
        {
            state = PlayerState.MOVING;
            anim.SetBool("isMoving", true);
            movement.Move(_rollAmount);
        }
        moveButton.SetActive(false);
        rollButton.SetActive(false);
    }
    public void AcceptButton()
    {
        click.Play();
        _boostTile.Accept();
        acceptButton.SetActive(false);
        declineButton.SetActive(false);
        state = PlayerState.LOCKED;
    }

    public void DeclineButton()
    {
        click.Play();
        _boostTile.Decline();
        acceptButton.SetActive(false);
        declineButton.SetActive(false);
        state = PlayerState.LOCKED;  
    }

    public void HealthButton()
    {
        click.Play();
        _option.Option1();
        gainAttackButton.SetActive(false);
        gainHealthButton.SetActive(false); 
        state = PlayerState.LOCKED;
    }
    public void AttackPickupButton()
    {
        click.Play();
        _option.Option2();
        gainAttackButton.SetActive(false);
        gainHealthButton.SetActive(false);
        state = PlayerState.LOCKED;
    }
    // ******************** 

    public void UpdateDialog(string message)
    {
        _dialogText.text = message;
    }

    public void EnterInteractingState()
    {
        anim.SetBool("isMoving", false);
        gainAttackButton.SetActive(true);
        gainHealthButton.SetActive(true);
        state = PlayerState.INTERACTING;
    }

    public void EnterLockedState()
    {
        state = PlayerState.LOCKED;
    }

    public void EnterMarket()
    {
        anim.SetBool("isMoving", false);
        state = PlayerState.CHOOSING;
    }

    public void EnterBattleState()
    {
        state = PlayerState.FIGHTING;
        attackButton.SetActive(true);
        bribeButton.SetActive(true);
        anim.SetBool("isMoving", false);
    }

    public void EnterRollingState()
    {
        ClearDialogImage();        
        ResetRolls();
        anim.SetBool("isMoving", false);
        _dialogText.text = "Roll (1-3 spots)";
        rollButton.SetActive(true);
        state = PlayerState.ROLLING;
    }

    void ResetRolls()
    {
        _rolls = 0;
        _rollAmount = 0;
        HighlightMarker(0);
    }

    public void StartMoving()
    {
        anim.SetBool("isMoving", true);
    }

    void RollState()
    {
        /*
        if (Input.GetKeyDown(KeyCode.Space) && _rolls <= _maxRolls)
        {
            Roll();
        }

        if (Input.GetKeyDown(KeyCode.W) && _rolls > 0 || _rolls == _maxRolls)
        {
            state = PlayerState.MOVING;
            anim.SetBool("isMoving", true);
            movement.Move(_rollAmount);
        }
        */

    }

    public void Roll()
    {
        click.Play();
        _rolls += 1;
        int rolled = UnityEngine.Random.Range(1, 4);
        _rollAmount += rolled;

        if (_rolls < _maxRolls)
        {
            moveButton.SetActive(true);
            _dialogText.text = $"You rolled a {rolled}! Roll again and move further? or Move now?";
        }
        else
        {
            _dialogText.text = $"You rolled a {_rollAmount}!";
            moveButton.SetActive(false);
            rollButton.SetActive(false);
        }

        HighlightMarker(_rollAmount * 3);

        if (_rolls == _maxRolls)
        {
            state = PlayerState.MOVING;
            anim.SetBool("isMoving", true);
            movement.Move(_rollAmount);
        }

    }

    void HighlightMarker(float range)
    {
         Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, range);
        for (int i = 0; i < colliders.Length; i++)
        {
            if(colliders[i].gameObject.CompareTag("marker") && colliders[i].gameObject.transform.position.y > transform.position.y)
            {
                colliders[i].GetComponent<Marker>().Highlight();
            }
        }
    }
}
