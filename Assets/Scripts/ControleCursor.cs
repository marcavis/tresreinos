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
            //acho que apagar depois?
            GameObject[] overlays = GameObject.FindGameObjectsWithTag("HelperOverlay");
            foreach(GameObject obj in overlays) {
                Destroy(obj);
            }

            foreach (Personagem p in gs.personagens)
            {
                if(p.transform.position == transform.position) {
                    p.Piscar();
                    
                    List<Vector3> aPintar = p.TilesAcessiveis(_tilemap);
                    foreach (Vector3 t in aPintar)
                    {
                        Instantiate(blueSquare, t, Quaternion.identity);
                    }
                }
                
            }
            //print (transform.position);
            //print(_tilemap.GetTile(new Vector3Int(Mathf.RoundToInt(transform.position.x - 0.5f), Mathf.RoundToInt(transform.position.y - 0.5f), 0) ));
        }
        if(podeMover) {
            
            float deslocX = Input.GetAxisRaw("Horizontal");
            float deslocY = Input.GetAxisRaw("Vertical");
            novoX = transform.position.x + deslocX;
            novoY = transform.position.y + deslocY;
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
        
    }

}
