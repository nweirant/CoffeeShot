using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public enum Direction { UP, DOWN, LEFT, RIGHT }
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private Collider2D _coll;

    public int _speed;
    private int _spaceMultiplier = 3;
    public bool _isMoving;
    public float _stopDelay = 0.5f;

    public PlayerManager manager;
    public PlayerStats player;

    public Vector3 _cornerBR;
    public Vector3 _cornerTR;
    public Vector3 _cornerTL;
    public Vector3 _corneerBL;
    public Slider distanceUI;


    private bool canMove = true;
    Direction dir;
    
    private void Start()
    {
        _isMoving = false;
        ToggleCollider(false);
        dir = Direction.RIGHT;
    }

    public void ToggleCollider(bool state)
    {
        _coll.enabled = state;
    }

    public void Move(int spaces)
    {
        _isMoving = true;
        ToggleCollider(false);

        if(dir == Direction.RIGHT)
        {
            StartCoroutine(MoveUp((spaces * _spaceMultiplier) + transform.position.y));
        }
    }

    public void MoveToTopOfMoon(float top)
    {
        ToggleCollider(false);
        manager.StartMoving();
        StartCoroutine(MoveUp(top, true));
        manager.UpdateDialog("I coming for ya!");
    }

    IEnumerator MoveUp(float target, bool moon=false)
    {
        

        while (transform.position.y != target && canMove)
        {
            distanceUI.value = transform.position.y;
            transform.position = Vector2.MoveTowards(transform.position, new Vector2(transform.position.x, target), _speed * Time.deltaTime);
            if(transform.position.y % 3 <= 0.01f)
            {
                player.LoseOxygen();
            }
            yield return null;
        }
        yield return new WaitForSeconds(_stopDelay);
        if(moon) {
            manager.CompletedLevel();
        }
        manager.StopMovingAnimation();
        _isMoving = false;
        ToggleCollider(true);
    }


    public void CanMove(bool state)
    {
        canMove = state;
    }
}
