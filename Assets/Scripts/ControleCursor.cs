using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

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

    public Tilemap _tilemap;
    private GerenciadorScript gs;

    public Transform blueSquare;
    void Start()
    {   
        gs = GameObject.Find("Gerenciador").GetComponent<GerenciadorScript>();
        //print(gs);

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
        //print(_tilemap.GetSprite(new Vector3Int(Mathf.RoundToInt(transform.position.x - 0.5f), Mathf.RoundToInt(transform.position.y - 0.5f), 0) ));
        if(Input.GetButtonDown("Fire1")) {
            foreach (Personagem p in gs.personagens)
            {
                //print(p.transform.position);
                //print(arredPosicao(transform.position));
                if(p.transform.position == arredPosicao(transform.position)) {
                    print(p.transform.position);
                    p.Piscar();
                    for (int x = (int) -p.movimento; x <= (p.movimento); x++)
                    {
                        for (int y = (int) -p.movimento; y <= (p.movimento); y++) {
                            Vector3 estaPosicao = new Vector3(p.transform.position.x + x, p.transform.position.y + y, 0);
                            if(p.PodeAlcancar(estaPosicao)) {
                                Instantiate(blueSquare, estaPosicao, Quaternion.identity);
                            }
                            //print(_tilemap.GetTile(new Vector3Int(Mathf.RoundToInt(p.transform.position.x + x - 0.5f), Mathf.RoundToInt(p.transform.position.y + y - 0.5f), 0) ));
                        }
                    }
                }
                
            }
            //print (transform.position);
            //print(_tilemap.GetTile(new Vector3Int(Mathf.RoundToInt(transform.position.x - 0.5f), Mathf.RoundToInt(transform.position.y - 0.5f), 0) ));
        }
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

    private Vector3 arredPosicao(Vector3 v) {
        return new Vector3(Mathf.Floor(v.x) + 0.5f, Mathf.Floor(v.y) + 0.5f, 0.0f);
    }
    
}
