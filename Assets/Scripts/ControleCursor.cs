using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControleCursor : MonoBehaviour
{
    private float novoX;
    private float novoY;
    private float velhoDeslocX, velhoDeslocY;
    //verifica se o jogador está pressionando nessa direção há algum tempo,
    //para aumentar a velocidade do cursor
    private int segurando = 1; 
    private Vector3 novaPosicao;
    private float velCursor = 5f;
    // Start is called before the first frame update
    private bool podeMover = true;
    void Start()
    {
        
    }

    void FixedUpdate () {
        // if (moveToPoint)
        // {
        //     transform.position = Vector3.MoveTowards(transform.position, endPosition, moveSpeed * Time.deltaTime);
        // }
    }

    // Update is called once per frame
    void Update()
    {
        if(podeMover) {
            float deslocX = Input.GetAxisRaw("Horizontal");
            float deslocY = Input.GetAxisRaw("Vertical");
            //float addX = deslocX * velocidade * Time.deltaTime;
            novoX = transform.position.x + deslocX;
            novoY = transform.position.y + deslocY;
            //transform.position = new Vector3(novoX, novoY, transform.position.z);
            novaPosicao = new Vector3(novoX, novoY, transform.position.z);
            podeMover = false;
            if (velhoDeslocX == deslocX && velhoDeslocY == deslocY) {
                segurando++;
            } else {
                segurando = 1;
            }
            velhoDeslocX = deslocX;
            velhoDeslocY = deslocY;
        }
        if(transform.position == novaPosicao) {
            podeMover = true;
        } else {
            segurando = segurando--;
            if(segurando > 2) { segurando = 2;}
            if(segurando < 1) { segurando = 1;}
            transform.position = Vector3.MoveTowards(transform.position, novaPosicao, velCursor * Time.deltaTime * segurando);
        }
        
        // if ((ladoAnterior != deslocX) && (deslocX != 0f)){
        //     ladoAnterior = deslocX;
        //     float escalaX = transform.localScale.x * -1;
        //     transform.localScale = new Vector3(escalaX, transform.localScale.y, transform.localScale.z);
        // }
    }
}
