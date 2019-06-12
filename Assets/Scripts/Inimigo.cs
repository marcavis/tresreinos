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
    private Personagem personagem;
    private List<Personagem> inimigosAcessiveis;
    private List<Vector3> terrenoAcessivel;
    private Personagem alvoEscolhidoParaAtacar;
    private Tilemap tilemap;
    
    private Dictionary<Personagem, List<Vector3>> porOndeAtacar;
    private float lastTime;
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
    }

    // Update is called once per frame
    void Update()
    {
        if(cooldown > 0) {
            cooldown--;
        } else {
            if(estado == 1 && cursor.transform.position == transform.position) {
                cooldown = cooldownPadrao;
                if(inimigosAcessiveis.Count == 0) {
                    cursor.IrParaPosicao(EscolherPosicaoDeAproximacao(alvoEscolhidoParaAtacar));
                } else {
                    cursor.IrParaPosicao(EscolherPosicaoDeAtaque(alvoEscolhidoParaAtacar));
                }
                estado = 2;
            } else if(estado == 2) {
                cooldown = cooldownPadrao;
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
                personagem.Atacar(alvoEscolhidoParaAtacar);
                FinalizarTurno();
                //TODO: bug quando unidade está cercada
            } 
        }
        
    }

    public void Iniciar() {
        inimigosAcessiveis = new List<Personagem>();
        List<Personagem> meusInimigos = gs.personagens.Where(x => x.time == 0).ToList();
        terrenoAcessivel = personagem.TilesAcessiveis(tilemap);
        cursor.MostrarOverlaysMovimento(terrenoAcessivel);
        cursor.ultimaUnidade = personagem;
        gs.MostrarMenuBatalhaDoInimigo();
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
        //int indice = Random.Range(0, )
        int indice = Random.Range(0, porOndeAtacar[alvo].Count);
        return porOndeAtacar[alvo][indice];
    }

    private Vector3 EscolherPosicaoDeAproximacao(Personagem alvo) {
        //TODO: melhorar para conseguir perseguir heróis até através de paredes grandes
        Vector3 posAlvo = alvo.transform.position;
        int distParaEncurtar = Random.Range(2, 2 + 1); //alterar depois, conforme agressividade
        float distanciaAtual = Personagem.Manhattan(posAlvo, transform.position);
        List<Vector3> tilesParaAproximacao = terrenoAcessivel.Where(x => Personagem.Manhattan(transform.position, x) == distParaEncurtar).ToList();
        tilesParaAproximacao.Sort((x, y) => Personagem.Manhattan(x, posAlvo).CompareTo(Personagem.Manhattan(y, posAlvo)));
        // foreach (var o in terrenoAcessivel)
        // {
        //     print(o + " " + posAlvo + " " + Personagem.Manhattan(o, posAlvo));
        // }
        //unidade vai escolher um dos 2 tiles que mais se aproxima do alvo
        if(tilesParaAproximacao.Count == 0) {
            //... a não ser que não haja tiles disponíveis
            return transform.position;
        }
        return tilesParaAproximacao[Random.Range(0, Mathf.Min(2, tilesParaAproximacao.Count))];
    }

    private Personagem EscolherHeroiAlvo() {
        //aprimorar isso depois
        int indice = Random.Range(0, inimigosAcessiveis.Count);
        return inimigosAcessiveis[indice];
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

    private bool transicaoCompletou() {
        return Time.time - lastTime >= 2f;
    }

    public void FinalizarTurno() {
        cursor.LimparOverlays();
        gs.ReiniciarLabelsAlvo();
        gs.canvas.GetComponent<Canvas>().enabled = false;
        estado = 0;
        gs.Proximo();
    }

    public List<Vector3> TilesQuePermitemAtaque(Personagem heroi) {
        //onde serão inseridos tiles de onde será possível atacar o personagem
        List<Vector3> tilesLivresParaAtaque = new List<Vector3>();
        foreach (Vector3 pos in terrenoAcessivel)
        {
            float dist = Personagem.Manhattan(heroi.transform.position, pos);
            if(dist <= personagem.arma.alcanceMax && dist >= personagem.arma.alcanceMin && gs.ObjetoNoTile(pos) == null) {
                tilesLivresParaAtaque.Add(pos);
                //Instantiate(cursor.blueSquare, pos, Quaternion.identity);
            }
        }
        return tilesLivresParaAtaque;
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
}
