using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class ControleCursor : MonoBehaviour
{
    //informa se está recebendo comandos do controle
    public bool ativo;
    private float novoX;
    private float novoY;
    private float velhoDeslocX, velhoDeslocY;
    //verifica se o jogador está pressionando nessa direção há algum tempo,
    //para aumentar a velocidade do cursor
    private int segurando = 1; 
    private Vector3 novaPosicao;
    public int cooldown;
    public bool movendoUnidade = false;
    private float velCursor = 6f;
    // Start is called before the first frame update
    private bool podeMover = true;

    public Tilemap _tilemap;
    private GerenciadorScript gs;
    private List<Vector3> acessiveisUltimaUnidade;
    private Personagem ultimaUnidade;

    public Transform blueSquare;
    void Start()
    {   
        gs = GameObject.Find("Gerenciador").GetComponent<GerenciadorScript>();
        //print(gs);

    }

    // Update is called once per frame
    void Update()
    {
        if(cooldown > 0) {
            cooldown--;
            return;
        }
        if(ativo && Input.GetButtonDown("Fire1")) {
            if(!movendoUnidade) {
                //acho que apagar depois?
                GameObject[] overlays = GameObject.FindGameObjectsWithTag("HelperOverlay");
                foreach(GameObject obj in overlays) {
                    Destroy(obj);
                }

                //seria bom substituir por algum tipo de Find
                foreach (Personagem p in gs.personagens)
                {
                    if(p.transform.position == transform.position) {
                        ultimaUnidade = p;
                        p.ComecarAPiscar();
                        gs.EntrarMenuBatalha();
                        acessiveisUltimaUnidade = p.TilesAcessiveis(_tilemap);
                        foreach (Vector3 t in acessiveisUltimaUnidade)
                        {
                            Instantiate(blueSquare, t, Quaternion.identity);
                        }
                    }
                }
            } else {
                if(acessiveisUltimaUnidade.Contains(transform.position)) {
                    ultimaUnidade.destinoFinal = transform.position;
                    ultimaUnidade.PrepararCaminho();
                } else {

                }
            }
        }
    
        if(ativo && podeMover) {
            
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
