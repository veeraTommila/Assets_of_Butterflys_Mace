using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// That the health bar will always be turned to camera.
public class Billboard : MonoBehaviour
{
    public Transform cam;

    // LateUpdate is called after the regular Update function.
    void LateUpdate()
    {
        transform.LookAt(transform.position + cam.forward);
    }
}
