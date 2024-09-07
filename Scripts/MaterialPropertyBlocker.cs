using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaterialPropertyBlocker : MonoBehaviour
{
    public Material material;
    private Renderer rend;
    private MaterialPropertyBlock propBlock;

    void Start()
    {
        rend = GetComponent<Renderer>();
        propBlock = new MaterialPropertyBlock();
    }

    void Update()
    {
        float scale = Mathf.PingPong(Time.time, 1.0f) + 1.0f; // Example scaling value
        rend.GetPropertyBlock(propBlock);
        propBlock.SetFloat("_Scale", scale);
        rend.SetPropertyBlock(propBlock);
    }
}
