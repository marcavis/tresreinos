using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class TileOverlay : MonoBehaviour
{
    private float valor = 0f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        valor += 0.20f;
        Color nova = gameObject.GetComponent<SpriteRenderer>().color;
        nova.a = 0.75f + Mathf.Cos(valor)/4;
        gameObject.GetComponent<SpriteRenderer>().color = nova;
    }
}
