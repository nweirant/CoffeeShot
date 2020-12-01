using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Marker : MonoBehaviour
{
    private float rotationSpeed;
    public Color highlight;
    public SpriteRenderer sprite;
    public Animator anim;
    private bool marked;

    private void Start()
    {
        rotationSpeed = Random.Range(-30, 30);
    }

    void Update()
    {
        transform.Rotate(0, 0, 1f * rotationSpeed * Time.deltaTime);
        if (marked)
        {
            Invoke("Expire", 30f);
        }

    }

    public void Highlight()
    {
        if (marked)
            return;
        sprite.color = highlight;
        anim.SetTrigger("mark");
        marked = true;
    }

    void Expire()
    {
        Destroy(gameObject);
    }
}
