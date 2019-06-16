using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System;

public class ControleCursor : MonoBehaviour
{
    private float novoX;
    private float novoY;

    public Vector3 novaPosicao;
    
    private float velCursor = 10f;
    // Start is called before the first frame update
    private bool podeMover = true;

    public Tilemap _tilemap;
    public Tilemap acessoCursor;
    private GerenciadorScript gs;
    
    public GerenciadorInventario menuInventario;
    public GerenciadorInventarioTroca menuInventarioTroca;
    public GerenciadorTelaHab menuHabilidades;
    private AttackParent ap;
    private List<Vector3> acessiveisUltimaUnidade;
    private List<Vector3> acessiveisAtaqueUltimaUnidade;

    private int indiceAlvoSelecionado;
    public int indiceItemSelecionado;
    public Personagem ultimaUnidade;
    private Vector3 posicaoInicialDaUnidade;

    public Transform blueSquare, redSquare, greenSquare;
    public int acaoDoCursor = 0;
    public const int NADA = 0;
    const int SELECIONADO = 1;
    const int MOVIDO = 2;

    const int PROCURA_ALVO_ATAQUE = 3;
    const int ATACOU = 4;
    const int PROCURA_ALVO_TROCA = 13;
    const int PROCURA_ALVO_HAB = 23;

    public GerenciadorInput gerenciadorInput;

    public int entrada;
    public Vector3 direcao;

    void Start()
    {   
        novaPosicao = transform.position;
        _tilemap = GameObject.Find("Tilemap").GetComponent<Tilemap>();
        acessoCursor = GameObject.Find("CursorAccess").GetComponent<Tilemap>();
        gs = GameObject.Find("Gerenciador").GetComponent<GerenciadorScript>();
        gerenciadorInput = GameObject.Find("Input").GetComponent<GerenciadorInput>();
        menuInventario = GameObject.Find("CanvasInventario").GetComponent<GerenciadorInventario>();
        menuInventarioTroca = GameObject.Find("CanvasInventarioTroca").GetComponent<GerenciadorInventarioTroca>();
        menuHabilidades = GameObject.Find("CanvasHabilidades").GetComponent<GerenciadorTelaHab>();
        ap = GameObject.Find("Placeholder").GetComponent<AttackParent>();
        ap.Init();
    }

    // Update is called once per frame
    void Update()
    {   
        if (gs.canvasBatalhaAberto) return;
        if(entrada == Teclas.CANCEL) {
            //todo objeto, ao tratar um input, o consome para que não o trate novamente no próximo frame
            entrada = 0;
            if (acaoDoCursor == NADA) {
                //outro cancelar ("ESC") retorna o cursor anteriormente selecionada
                novaPosicao = posicaoInicialDaUnidade;
                podeMover = false;
            } else if(acaoDoCursor == SELECIONADO) {
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
            } else if (acaoDoCursor == PROCURA_ALVO_TROCA) {
                novaPosicao = ultimaUnidade.transform.position;
                acaoDoCursor = MOVIDO;
                gerenciadorInput.cursorAtivo = 2;
                gs.ReiniciarLabelsAlvo();
                LimparOverlays();
                menuInventario.Reabrir();
            } else if (acaoDoCursor == PROCURA_ALVO_HAB) {
                novaPosicao = ultimaUnidade.transform.position;
                acaoDoCursor = MOVIDO;
                gerenciadorInput.cursorAtivo = 4;
                gs.ReiniciarLabelsAlvo();
                LimparOverlays();
                menuHabilidades.Reabrir();
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
                    ap.Abrir();
                    Personagem alvo = gs.ObjetoNoTile(transform.position);
                    ap.SetLeftAnimator(Defines.animacoesAtk[ultimaUnidade.nome]);
                    ap.SetRightAnimator(Defines.animacoesAtk[alvo.nome]);
                    StartCoroutine(SetTimeout(1f, () => {
                        ultimaUnidade.Atacar(alvo);
                        // ap.PlayLeft(10);
                        Liberar();
                        LimparOverlays();
                        print("atacou");
                    }, () => {
                        ap.Fechar();
                        gs.canvasBatalhaAberto = false;
                        gs.SairMenuBatalha();
                        gs.ReiniciarLabelsAlvo();
                        //TODO: botar um delay
                        gs.ProximoSeEmBatalha();
                        finalizado = true;
                    }));
                }
            } else if(acaoDoCursor == PROCURA_ALVO_TROCA) {
                //se o cursor ainda não tiver chegado num tile válido, aguardar até isso acontecer
                if(transform.position == novaPosicao) {
                    Item trocado = ultimaUnidade.inventario[indiceItemSelecionado];
                    Personagem recebedor = gs.ObjetoNoTile(transform.position);
                    if(recebedor.AdicionarAoInventario(trocado) != -1) {
                        //simples envio de item - o destinatário já recebeu, nesse ponto
                        ultimaUnidade.DescartarItem(indiceItemSelecionado);
                        Liberar();
                        LimparOverlays();
                        gs.SairMenuBatalha();
                        menuInventario.FecharMenu();
                        gs.ReiniciarLabelsAlvo();
                        //TODO: botar um delay
                        gs.ProximoSeEmBatalha();
                        finalizado = true;
                    } else {
                        //TODO:implementar troca pedindo um item do recebedor
                        LimparOverlays();
                        gerenciadorInput.cursorAtivo = 3;
                        menuInventarioTroca.AbrirMenu(ultimaUnidade, recebedor, indiceItemSelecionado);
                        //gerenciadorInput.GetComponent<GerenciadorInput>().cursorAtivo = 3;
                    }
                }
            } else if(acaoDoCursor == PROCURA_ALVO_HAB) {
                if(transform.position == novaPosicao) {
                    //informar onde o cursor está para o personagem - este vai definir quais alvos serão afetados
                    //acessando a variável areaDeEfeito da habilidadeAtual
                    ultimaUnidade.UsarHabilidade(transform.position);
                    Liberar();
                    LimparOverlays();
                    gs.SairMenuBatalha();
                    gs.ReiniciarLabelsAlvo();
                    //TODO: botar um delay
                    gs.ProximoSeEmBatalha();
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
                List<Vector3> alvos = ultimaUnidade.AlvosAcessiveisFiltrados(ultimaUnidade.arma.alcanceMin,
                                                                            ultimaUnidade.arma.alcanceMax,
                                                                             false);
                indiceAlvoSelecionado = (alvos.Count + indiceAlvoSelecionado) % alvos.Count;
                novaPosicao = alvos[indiceAlvoSelecionado];
                //obtém o alvo apontado pelo cursor, e mostra seus dados no canvas do alvo
                gs.MostrarDadosDoAlvo(gs.ObjetoNoTile(alvos[indiceAlvoSelecionado]));
            } else if(acaoDoCursor == PROCURA_ALVO_TROCA) {
                //fazer o cursor circular pelos alvos permitidos
                if(direcao.x < 0 || direcao.y > 0) {
                    indiceAlvoSelecionado++;
                } else if(direcao.x > 0 || direcao.y < 0) {
                    indiceAlvoSelecionado--;
                }
                List<Vector3> alvos = ultimaUnidade.AlvosAcessiveisFiltrados(1, 1, true);
                indiceAlvoSelecionado = (alvos.Count + indiceAlvoSelecionado) % alvos.Count;
                novaPosicao = alvos[indiceAlvoSelecionado];
                //obtém o alvo apontado pelo cursor, e mostra seus dados no canvas do alvo
                gs.MostrarDadosDoAlvo(gs.ObjetoNoTile(alvos[indiceAlvoSelecionado]));
            } else if(acaoDoCursor == PROCURA_ALVO_HAB) {
                //fazer o cursor circular pelos alvos permitidos
                if(direcao.x < 0 || direcao.y > 0) {
                    indiceAlvoSelecionado++;
                } else if(direcao.x > 0 || direcao.y < 0) {
                    indiceAlvoSelecionado--;
                }
                Habilidade atual = ultimaUnidade.habilidadeAtual;
                List<Vector3> alvos = ultimaUnidade.AlvosAcessiveisFiltrados(
                    atual.alcanceMin, atual.alcanceMax, ultimaUnidade.habilidadeAtual.seMesmoTime);
                indiceAlvoSelecionado = (alvos.Count + indiceAlvoSelecionado) % alvos.Count;
                novaPosicao = alvos[indiceAlvoSelecionado];
                //obtém o alvo apontado pelo cursor, e mostra seus dados no canvas do alvo
                gs.MostrarDadosDoAlvo(gs.ObjetoNoTile(alvos[indiceAlvoSelecionado]));
            } else {
                
                novoX = transform.position.x + direcao.x;
                novoY = transform.position.y + direcao.y;
                Vector3Int talvezNovaPosicao1 = new Vector3Int((int) novoX, (int) novoY, 0);
                Vector3Int talvezNovaPosicao2 = new Vector3Int((int) transform.position.x, (int) novoY, 0);
                Vector3Int talvezNovaPosicao3 = new Vector3Int((int) novoX, (int) transform.position.y, 0);
                foreach (var pos in new List<Vector3Int> {talvezNovaPosicao1, talvezNovaPosicao2, talvezNovaPosicao3})
                {
                    if(acessoCursor.GetTile(pos)) {
                        novaPosicao = pos;
                        break;
                    }
                }
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

    //sem argumento, usa os dados do último herói
    public void MostrarOverlaysMovimento() {
        foreach (Vector3 t in acessiveisUltimaUnidade)
        {
            Instantiate(blueSquare, t, Quaternion.identity);
        }
    }

    //com argumento, pode receber dados por ex. de um inimigo
    public void MostrarOverlaysMovimento(List<Vector3> tilesAcessiveis) {
        foreach (Vector3 t in tilesAcessiveis)
        {
            Instantiate(blueSquare, t, Quaternion.identity);
        }
    }

    public void MostrarOverlaysAtaque() {
        acessiveisAtaqueUltimaUnidade = ultimaUnidade.TilesAlvosAcessiveis(ultimaUnidade.arma.alcanceMin, ultimaUnidade.arma.alcanceMax);
        foreach (Vector3 t in acessiveisAtaqueUltimaUnidade)
        {
            Instantiate(redSquare, t, Quaternion.identity);
        }
    }

    //versão para inimigos
    public void MostrarOverlaysAtaque(List<Vector3> tilesAcessiveis) {
        foreach (Vector3 t in tilesAcessiveis)
        {
            Instantiate(redSquare, t, Quaternion.identity);
        }
    }

    public void MostrarOverlaysTroca() {
        List<Vector3> acessiveisTroca = ultimaUnidade.TilesAlvosAcessiveis(1, 1);
        foreach (Vector3 t in acessiveisTroca)
        {
            Instantiate(greenSquare, t, Quaternion.identity);
        }
    }

    public void MostrarOverlaysHabilidades(bool seMesmoTime) {
        Habilidade atual = ultimaUnidade.habilidadeAtual;
        List<Vector3> acessiveisHab = ultimaUnidade.TilesAlvosAcessiveis(atual.alcanceMin, atual.alcanceMax);
        foreach (Vector3 t in acessiveisHab)
        {
            if(seMesmoTime) {
                Instantiate(greenSquare, t, Quaternion.identity);
            } else {
                Instantiate(redSquare, t, Quaternion.identity);
            }
        }
    }

    //versão para inimigos
    public void MostrarOverlaysHabilidades(List<Vector3> tilesAcessiveis, bool seMesmoTime) {
        foreach (Vector3 t in tilesAcessiveis)
        {
            if(seMesmoTime) {
                Instantiate(greenSquare, t, Quaternion.identity);
            } else {
                Instantiate(redSquare, t, Quaternion.identity);
            }
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

    //usado principalmente por inimigos para mover o cursor e demonstrar a ação atual
    public void IrParaPosicao(Vector3 v) {
        novaPosicao = v;
        podeMover = false;
    }

    public void SelecionarUnidade(Personagem p) {
        ultimaUnidade = p;
        if (p.time != 1) {
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

    public void IrParaPrimeiroAlvo() {
        acaoDoCursor = PROCURA_ALVO_ATAQUE;
        novaPosicao = ultimaUnidade.AlvosAcessiveisFiltrados(ultimaUnidade.arma.alcanceMin, ultimaUnidade.arma.alcanceMax, false)[0];
        podeMover = false;
        gs.MostrarDadosDoAlvo(gs.ObjetoNoTile(novaPosicao));
    }

    public void IrParaPrimeiroAlvoTroca() {
        acaoDoCursor = PROCURA_ALVO_TROCA;
        novaPosicao = ultimaUnidade.AlvosAcessiveisFiltrados(ultimaUnidade.arma.alcanceMin, ultimaUnidade.arma.alcanceMax, true)[0];
        podeMover = false;
        gs.MostrarDadosDoAlvo(gs.ObjetoNoTile(novaPosicao));
    }

    //variável seMesmoTime diz se o cursor deve focar em alvos do mesmo time (habilidades de cura, etc)
    //ou alvos inimigos
    public void IrParaPrimeiroAlvoHabilidade(bool seMesmoTime) {
        acaoDoCursor = PROCURA_ALVO_HAB;
        novaPosicao = ultimaUnidade.AlvosAcessiveisFiltrados(ultimaUnidade.habilidadeAtual.alcanceMin, 
                        ultimaUnidade.habilidadeAtual.alcanceMax, seMesmoTime)[0];
        podeMover = false;
        gs.MostrarDadosDoAlvo(gs.ObjetoNoTile(novaPosicao));
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

    IEnumerator SetTimeout(float time, params Action[] actions) {
        for (int i = 0; i < actions.Length; i++) {
            yield return new WaitForSeconds(time);
            actions[i]();
        }
    }
}
