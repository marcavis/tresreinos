using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class ControleCursor : MonoBehaviour
{
    private float novoX;
    private float novoY;

    public Vector3 novaPosicao;
    
    private float velCursor = 10f;
    // Start is called before the first frame update
    private bool podeMover = true;

    public Tilemap _tilemap;
    private GerenciadorScript gs;
    private List<Vector3> acessiveisUltimaUnidade;
    private List<Vector3> acessiveisAtaqueUltimaUnidade;

    private int indiceAlvoSelecionado;
    public Personagem ultimaUnidade;
    private Vector3 posicaoInicialDaUnidade;

    public Transform blueSquare, redSquare;
    public int acaoDoCursor = 0;
    const int NADA = 0;
    const int SELECIONADO = 1;
    const int MOVIDO = 2;

    const int PROCURA_ALVO_ATAQUE = 3;
    const int ATACOU = 4;

    public GerenciadorInput gerenciadorInput;

    public int entrada;
    public Vector3 direcao;

    void Start()
    {   
        novaPosicao = transform.position;
        _tilemap = GameObject.Find("Tilemap").GetComponent<Tilemap>();
        gs = GameObject.Find("Gerenciador").GetComponent<GerenciadorScript>();
        gerenciadorInput = GameObject.Find("Input").GetComponent<GerenciadorInput>();
    }

    // Update is called once per frame
    void Update()
    {
        if(entrada == Teclas.CANCEL) {
            //todo objeto, ao tratar um input, o consome para que não o trate novamente no próximo frame
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
            } else if (acaoDoCursor == PROCURA_ALVO_ATAQUE) {
                novaPosicao = ultimaUnidade.transform.position;
                acaoDoCursor = MOVIDO;
                gerenciadorInput.cursorAtivo = 1;
                gs.ReiniciarLabelsAlvo();
            }
        }
        
        if(entrada == Teclas.ACTION) {
            entrada = 0;
            bool finalizado = false;
            if(acaoDoCursor == NADA) {
                //acho que apagar depois?
                LimparOverlays();
                //seria bom substituir por algum tipo de Find
                foreach (Personagem p in gs.personagens)
                {
                    if(p.transform.position == transform.position) {
                        if(gs.SeAtual(p)) {
                            SelecionarUnidade(p);
                            //print("pode ir");
                        } else {
                            //TODO: mostrar status da unidade que tentou selecionar
                            //print ("não pode");
                        }
                    }
                }
            } else if(acaoDoCursor == SELECIONADO) {
                //apenas permitir movimento para tiles acessíveis pela unidade
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
            } else if(acaoDoCursor == MOVIDO) {
                //nesse ponto o cursor não está ativo - o jogador está no menu de batalha
            } else if(acaoDoCursor == PROCURA_ALVO_ATAQUE) {
                //se o cursor ainda não tiver chegado num tile válido, aguardar até isso acontecer
                if(transform.position == novaPosicao) {
                    ultimaUnidade.Atacar(gs.ObjetoNoTile(transform.position));
                    Liberar();
                    LimparOverlays();
                    gs.SairMenuBatalha();
                    gs.ReiniciarLabelsAlvo();
                    //TODO: botar um delay
                    gs.Proximo();
                    finalizado = true;
                }
            }
            //se o cursor foi acionado quando o cursor ainda não havia chegado à posição para qual foi movido,
            //manter o input até o próximo frame, para nova tentativa
            if(transform.position != novaPosicao && !finalizado) {
                entrada = Teclas.ACTION;
            }
        }
    
        if(podeMover && entrada == Teclas.DPAD) {
            entrada = 0;
            if(acaoDoCursor == PROCURA_ALVO_ATAQUE) {
                //fazer o cursor circular pelos alvos permitidos
                if(direcao.x < 0 || direcao.y > 0) {
                    indiceAlvoSelecionado++;
                } else if(direcao.x > 0 || direcao.y < 0) {
                    indiceAlvoSelecionado--;
                }
                List<Vector3> alvos = ultimaUnidade.AlvosAcessiveisFiltrados();
                indiceAlvoSelecionado = (alvos.Count + indiceAlvoSelecionado) % alvos.Count;
                novaPosicao = alvos[indiceAlvoSelecionado];
                gs.MostrarDadosDoAlvo(gs.ObjetoNoTile(ultimaUnidade.AlvosAcessiveisFiltrados()[indiceAlvoSelecionado]));
            } else {
                novoX = transform.position.x + direcao.x;
                novoY = transform.position.y + direcao.y;
                novaPosicao = new Vector3(novoX, novoY, transform.position.z);
            }
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
            ultimaUnidade.ComecarAPiscar();
        }
    }

    public void MostrarOverlaysMovimento() {
        foreach (Vector3 t in acessiveisUltimaUnidade)
        {
            Instantiate(blueSquare, t, Quaternion.identity);
        }
    }

    public void MostrarOverlaysAtaque() {
        acessiveisAtaqueUltimaUnidade = ultimaUnidade.TilesAlvosAcessiveis();
        foreach (Vector3 t in acessiveisAtaqueUltimaUnidade)
        {
            Instantiate(redSquare, t, Quaternion.identity);
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
        entrada = 0;
    }

    public void IrParaUnidade(Personagem p) {
        novaPosicao = p.transform.position;
        podeMover = false;
        SelecionarUnidade(p);
    }

    public void SelecionarUnidade(Personagem p) {
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

    public void IrParaPrimeiroAlvo() {
        acaoDoCursor = PROCURA_ALVO_ATAQUE;
        novaPosicao = ultimaUnidade.AlvosAcessiveisFiltrados()[0];
        podeMover = false;
        gs.MostrarDadosDoAlvo(gs.ObjetoNoTile(ultimaUnidade.AlvosAcessiveisFiltrados()[0]));
    }

    //previne que tiles ocupados por amigos sejam considerados acessíveis pela unidade
    //porém, isso não impede que a unidade atravesse o terreno ocupado por uma unidade amiga
    public void RemoverOcupados(List<Vector3> tiles) {
        List<Vector3> ocupados = new List<Vector3>();
        foreach (var personagem in gs.personagens)
        {
            //if(personagem.transform.position)
            if(personagem != ultimaUnidade) {
                tiles.Remove(personagem.transform.position);
            }
        }
    }
}
