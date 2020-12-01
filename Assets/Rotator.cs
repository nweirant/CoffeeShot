using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotator : MonoBehaviour
{
    private float rotationSpeed;

    private void Start()
    {
        rotationSpeed = Random.Range(-20, 20);
    }

    void Update()
    {
        transform.Rotate(0, 0, 1f * rotationSpeed * Time.deltaTime);
    }
}
