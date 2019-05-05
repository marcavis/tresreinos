using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Personagem : MonoBehaviour
{
    public string nome;
    private bool piscando = false;
    private int blinkFrames;
    private int chp, mhp;
    public float movimento;

    // Start is called before the first frame update
    void Start()
    {
        movimento = 3f;
    }

    // Update is called once per frame
    void Update()
    {
        if(piscando) {
            blinkFrames--;
            if(blinkFrames == 0) {
                blinkFrames = 6;
                //inverte a variável enabled, apagando ou redesenhando o objeto
                gameObject.GetComponent<SpriteRenderer>().enabled ^= true;
            }
        }
    }

    public void Piscar() {
        gameObject.GetComponent<SpriteRenderer>().enabled = false;
        blinkFrames = 6;
        piscando = true;
    }

    public bool PodeAlcancar(Vector3 posicao) {
        return Vector3.Distance(this.transform.position, posicao) <= movimento;
    }
}
