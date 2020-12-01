using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxBG : MonoBehaviour
{
    private Transform cam;
    private Vector3 lastCamPos;
    [SerializeField] private float parallaxSpeed;

    void Start()
    {
        cam = Camera.main.transform;
        lastCamPos = cam.transform.position;

    }

    // Update is called once per frame
    void Update()
    {
        Vector3 deltaMovement = cam.position - lastCamPos;
        transform.position += deltaMovement * -parallaxSpeed;
        lastCamPos = cam.position;


    }
}
