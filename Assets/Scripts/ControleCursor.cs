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

    private Vector3 novaPosicao;
    public int cooldown;
    
    private float velCursor = 10f;
    // Start is called before the first frame update
    private bool podeMover = true;

    public Tilemap _tilemap;
    private GerenciadorScript gs;
    private List<Vector3> acessiveisUltimaUnidade;
    private Personagem ultimaUnidade;
    private Vector3 posicaoInicialDaUnidade;

    public Transform blueSquare;
    public int acaoDoCursor = 0;
    const int NADA = 0;
    const int SELECIONADO = 1;
    const int MOVIDO = 2;

    public GerenciadorInput gerenciadorInput;

    public int entrada;
    public Vector3 direcao;

    void Start()
    {   
        novaPosicao = transform.position;
        _tilemap = GameObject.Find("Tilemap").GetComponent<Tilemap>();
        gs = GameObject.Find("Gerenciador").GetComponent<GerenciadorScript>();
        gerenciadorInput = GameObject.Find("Input").GetComponent<GerenciadorInput>();
        //print(gs);

    }

    // Update is called once per frame
    void Update()
    {
        //variável para evitar que mais de um input seja tratado neste frame
        // bool aceitaInput = true;

        //evitar que um toque ligeiramente mais demorado execute várias ações
        if(cooldown > 0) {
            cooldown--;
            return;
        }

        if(entrada == Teclas.CANCEL) {
            entrada = 0;
            if(acaoDoCursor == SELECIONADO) {
                LimparOverlays();
                if(ultimaUnidade != null) {
                    ultimaUnidade.PararDePiscar();
                }
                ultimaUnidade = null;
                acaoDoCursor = NADA;
            } else if(acaoDoCursor == MOVIDO) {
                //tratado em DesfazerAcaoAtual() pois no status MOVIDO o controle estará no menu de batalha
            }
        }
        
        if(entrada == Teclas.ACTION) {
            entrada = 0;
            if(acaoDoCursor == NADA) {
                //acho que apagar depois?
                LimparOverlays();
                //seria bom substituir por algum tipo de Find
                foreach (Personagem p in gs.personagens)
                {
                    if(p.transform.position == transform.position) {
                        ultimaUnidade = p;
                        p.ComecarAPiscar();
                        posicaoInicialDaUnidade = p.transform.position;
                        //gs.EntrarMenuBatalha();
                        acessiveisUltimaUnidade = p.TilesAcessiveis(_tilemap);
                        
                        //não permitir que a unidade ocupe o mesmo tile de um companheiro
                        RemoverOcupados(acessiveisUltimaUnidade);

                        MostrarOverlaysMovimento();
                        
                        acaoDoCursor = SELECIONADO;
                    }
                }
            } else if(acaoDoCursor == SELECIONADO) {
                if(acessiveisUltimaUnidade.Contains(transform.position)) {
                    //apenas aceitar nova movimentação se a unidade já tiver graficamente voltado a seu posto
                    if(ultimaUnidade.transform.position == posicaoInicialDaUnidade) {
                        ultimaUnidade.destinoFinal = transform.position;
                        ultimaUnidade.PrepararCaminho();
                        acaoDoCursor = MOVIDO;
                        LimparOverlays();
                        ultimaUnidade.PararDePiscar();
                        gs.EntrarMenuBatalha();
                        gerenciadorInput.cursorAtivo = 1;
                    }
                } else {
                    //TODO: tocar som de erro?
                }
            }
        }
    
        if(podeMover && entrada == Teclas.DPAD) {
            entrada = 0;
            
            novoX = transform.position.x + direcao.x;
            novoY = transform.position.y + direcao.y;
            novaPosicao = new Vector3(novoX, novoY, transform.position.z);
            podeMover = false;
        }

        if(transform.position == novaPosicao) {
            podeMover = true;
        } else {
            transform.position = Vector3.MoveTowards(transform.position, novaPosicao, velCursor * Time.deltaTime);
        }
    }

    public void DesfazerAcaoAtual() {
        if(acaoDoCursor == MOVIDO) {
            MostrarOverlaysMovimento();
            acaoDoCursor = SELECIONADO;
            ultimaUnidade.DesfazerMovimento();
        }
    }

    public void MostrarOverlaysMovimento() {
        foreach (Vector3 t in acessiveisUltimaUnidade)
        {
            Instantiate(blueSquare, t, Quaternion.identity);
        }
    }

    public void LimparOverlays()
    {
        GameObject[] overlays = GameObject.FindGameObjectsWithTag("HelperOverlay");
        foreach(GameObject obj in overlays) {
            Destroy(obj);
        }
    }

    //terminada a rodada, agora o cursor pode selecionar unidades de novo
    public void Liberar() {
        acaoDoCursor = NADA;
    }

    public void RemoverOcupados(List<Vector3> tiles) {
        List<Vector3> ocupados = new List<Vector3>();
        foreach (var personagem in gs.personagens)
        {
            //if(personagem.transform.position)
            if(personagem != ultimaUnidade) {
                print(personagem.transform.position);
                tiles.Remove(personagem.transform.position);
            }
        }
    }
}
