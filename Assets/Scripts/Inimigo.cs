using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Inimigo : MonoBehaviour
{
    private int cooldownPadrao;
    private int cooldown;
    private int estado; //0 no começo, 1 antes de andar, 
    private GerenciadorScript gs;
    private GerenciadorInput input;
    private ControleCursor cursor;
    private AttackParent ap;
    private Personagem personagem;
    private List<Personagem> inimigosAcessiveis;
    private List<Vector3> terrenoAcessivel;
    private Personagem alvoEscolhidoParaAtacar;
    private Personagem meuUltimoAlvo;
    
    //TODO: implementar
    public List<Personagem> acabaramDeMeAtacar; //grava quem atacou a unidade na última rodada

    private Tilemap tilemap;
    
    private Dictionary<Personagem, List<Vector3>> porOndeAtacar;
    private bool mostrandoOverlay;
    // Start is called before the first frame update
    void Start()
    {
        cooldownPadrao = 50;
        gs = GameObject.Find("Gerenciador").GetComponent<GerenciadorScript>();
        input = GameObject.Find("Input").GetComponent<GerenciadorInput>();
        cursor = GameObject.Find("Cursor").GetComponent<ControleCursor>();
        tilemap = GameObject.Find("Tilemap").GetComponent<Tilemap>();
        personagem = gameObject.GetComponent<Personagem>();
        ap = GameObject.Find("Placeholder").GetComponent<AttackParent>();
        acabaramDeMeAtacar = new List<Personagem>();
    }

    // Update is called once per frame
    void Update()
    {
        if (gs.canvasBatalhaAberto) return;
        if(cooldown > 0) {
            cooldown--;
        } else {
            if(estado == 1 && cursor.transform.position == transform.position) {
                cooldown = cooldownPadrao;
                if(inimigosAcessiveis.Count == 0) {
                    cursor.IrParaPosicao(EscolherPosicaoDeAproximacao(alvoEscolhidoParaAtacar));
                } else {
                    Vector3 posPotencial = EscolherPosicaoDeAtaque(alvoEscolhidoParaAtacar);
                    if (posPotencial == transform.position) {cooldown = cooldown / 5;}
                    cursor.IrParaPosicao(posPotencial);
                }
                estado = 2;
            } else if(estado == 2) {
                cooldown = cooldownPadrao;
                if(cursor.transform.position == transform.position) {cooldown = cooldown / 5;}
                Mover();
                estado = 3; //vai mudar dependendo se for atacar ou usar habilidade
            } else if(estado == 3) {
                cooldown = cooldownPadrao;
                if(inimigosAcessiveis.Count == 0) {
                    FinalizarTurno();
                } else {
                    cursor.MostrarOverlaysAtaque(personagem.TilesAlvosAcessiveis(personagem.arma.alcanceMin, personagem.arma.alcanceMax));
                    cursor.IrParaPosicao(alvoEscolhidoParaAtacar.transform.position);
                    gs.MostrarDadosDoAlvo(alvoEscolhidoParaAtacar);
                    estado = 4;
                }
            } else if(estado == 4) {
                ap.Abrir();
                ap.SetLeftAnimator(Defines.animacoesAtk[alvoEscolhidoParaAtacar.nome]);
                ap.SetRightAnimator(Defines.animacoesAtk[personagem.nome]);
                gs.canvasBatalhaAberto = true;
                StartCoroutine(SetTimeout(1f, () => personagem.Atacar(alvoEscolhidoParaAtacar), () => {
                    ap.Fechar();
                    gs.canvasBatalhaAberto = false;
                    acabaramDeMeAtacar = new List<Personagem>();
                    FinalizarTurno();
                }));
            } 
        }
        
    }

    public void Iniciar() {
        inimigosAcessiveis = new List<Personagem>();
        List<Personagem> meusInimigos = gs.personagens.Where(x => x.time == 0).ToList();
        terrenoAcessivel = personagem.TilesAcessiveis(tilemap);
        cursor.MostrarOverlaysMovimento(terrenoAcessivel);
        cursor.ultimaUnidade = personagem;
        gs.MostrarMenuBatalhaInativo(personagem);
        porOndeAtacar = new Dictionary<Personagem, List<Vector3>>();
        foreach (Personagem p in meusInimigos)
        {
            if(TilesQuePermitemAtaque(p).Count > 0) {
                porOndeAtacar[p] = TilesQuePermitemAtaque(p);
                inimigosAcessiveis.Add(p);
            }
        }
        if(inimigosAcessiveis.Count > 0) {
            alvoEscolhidoParaAtacar = EscolherHeroiAlvo();
            estado = 1;
        } else {
            alvoEscolhidoParaAtacar = EscolherHeroiParaSeguir();
            estado = 1;
            //não é possível alcançar ninguém, fazer outra coisa
            //TODO: em vez de movimento aleatório, tentar se aproximar das unidades
        }
        cooldown = cooldownPadrao;
    }

    public void Mover() {
        personagem.destinoFinal = cursor.transform.position;
        personagem.PrepararCaminho();
        //acaoDoCursor = MOVIDO;
        cursor.LimparOverlays();
        personagem.PararDePiscar();
        //gs.EntrarMenuBatalha();
    }

    public void Atacar(Personagem alvo) {

    }

    private Vector3 EscolherPosicaoDeAtaque(Personagem alvo) {
        if(porOndeAtacar[alvo].Contains(transform.position)) {
            return transform.position;
        }
        int indice = Random.Range(0, porOndeAtacar[alvo].Count);
        return porOndeAtacar[alvo][indice];
    }

    private Vector3 EscolherPosicaoDeAproximacao(Personagem alvo) {
        //TODO: melhorar para conseguir perseguir heróis até através de paredes grandes
        Vector3 posAlvo = alvo.transform.position;
        int distParaEncurtar = Random.Range(2, 2 + 1); //alterar depois, conforme agressividade
        float distanciaAtual = Personagem.Manhattan(posAlvo, transform.position);
        List<Vector3> tilesParaAproximacao = terrenoAcessivel.Where(x => Personagem.Manhattan(transform.position, x) == distParaEncurtar && gs.ObjetoNoTile(x) == null).ToList();
        tilesParaAproximacao.Sort((x, y) => Personagem.Manhattan(x, posAlvo).CompareTo(Personagem.Manhattan(y, posAlvo)));
        // foreach (var o in terrenoAcessivel)
        // {
        //     print(o + " " + posAlvo + " " + Personagem.Manhattan(o, posAlvo));
        // }
        //unidade vai escolher um dos 2 tiles que mais se aproxima do alvo
        //se o alvo estiver muito longe, nem se mover
        if(tilesParaAproximacao.Count == 0 || distanciaAtual > (personagem.Movimento()/10 * 3)) {
            //... a não ser que não haja tiles disponíveis
            return transform.position;
        }
        return tilesParaAproximacao[Random.Range(0, Mathf.Min(2, tilesParaAproximacao.Count))];
    }

    private Personagem EscolherHeroiAlvo() {
        Dictionary<Personagem, int> chanceDeEscolher = new Dictionary<Personagem, int>();
        int chanceAcumulada = 0;
        foreach (var p in inimigosAcessiveis)
        {
            chanceDeEscolher[p] = 10;
            if(p == meuUltimoAlvo) {
                chanceDeEscolher[p] += 40;
            }
            if(acabaramDeMeAtacar.Contains(p)) {
                chanceDeEscolher[p] += 40;
            }
            chanceDeEscolher[p] -= (int) Personagem.Manhattan(p.transform.position, transform.position);
            chanceDeEscolher[p] += porOndeAtacar[p].Count/2;
            chanceAcumulada += chanceDeEscolher[p];
        }
        int escolhido = Random.Range(0, chanceAcumulada);
        foreach (Personagem p in chanceDeEscolher.Keys)
        {
            if(escolhido < chanceDeEscolher[p]) {
                meuUltimoAlvo = p;
                break;
            } else {
                escolhido -= chanceDeEscolher[p];
            }
        }
        //int indice = Random.Range(0, inimigosAcessiveis.Count);
        //meuUltimoAlvo = inimigosAcessiveis[indice];
        return meuUltimoAlvo;
    }

    private Personagem EscolherHeroiParaSeguir() {
        //não podemos atacar um heroi ainda, mas podemos nos aproximar
        float distMin = 999f;
        Personagem seguido = personagem; //apenas usado para inicialização
        foreach (Personagem p in gs.personagens.Where(x => x.time == 0))
        {
            float dist = Personagem.Manhattan(p.transform.position, transform.position);
            if(dist < distMin) {
                seguido = p;
                distMin = dist;
            }
        }
        return seguido;
    }

    public void FinalizarTurno() {
        cursor.LimparOverlays();
        gs.ReiniciarLabelsAlvo();
        gs.canvas.GetComponent<Canvas>().enabled = false;
        estado = 0;
        personagem.PosTurno();
        gs.ProximoSeEmBatalha();
    }

    public List<Vector3> TilesQuePermitemAtaque(Personagem heroi) {
        //onde serão inseridos tiles de onde será possível atacar o personagem
        List<Vector3> tilesLivresParaAtaque = new List<Vector3>();
        foreach (Vector3 pos in terrenoAcessivel)
        {
            float dist = Personagem.Manhattan(heroi.transform.position, pos);
            if(dist <= personagem.arma.alcanceMax && dist >= personagem.arma.alcanceMin && VazioOuMeu(pos)) {
                tilesLivresParaAtaque.Add(pos);
                //Instantiate(cursor.blueSquare, pos, Quaternion.identity);
            }
            
        }
        return tilesLivresParaAtaque;
    }

    //diz se tal posição está livre para a unidade - seja por estar desocupada, seja por ser a posição atual
    public bool VazioOuMeu(Vector3 pos) {
        if(gs.ObjetoNoTile(pos) == null || gs.ObjetoNoTile(pos) == personagem) {
            return true;
        }
        return false;
    }
    
    public List<Personagem> GetInimigosAcessiveis() {
        List<Personagem> inimigos = new List<Personagem>();
        foreach (Vector3 item in personagem.TilesAlvosAcessiveis(personagem.arma.alcanceMin, personagem.arma.alcanceMax)) {
            Personagem p = gs.ObjetoNoTile(item);
            if (p != null && p.time != personagem.time) {
                inimigos.Add(p);
            }
        }
        return inimigos;
    }

    IEnumerator SetTimeout(float time, params System.Action[] actions) {
        for (int i = 0; i < actions.Length; i++) {
            yield return new WaitForSeconds(time);
            actions[i]();
        }
    }
}
