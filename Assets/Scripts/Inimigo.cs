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
    public bool posicaoDefinida;
    private int targetIndex = -1;
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
        // if (vezInimigo && cursor.transform.position == transform.position) {
            
        //     Agir();
        //     posicaoDefinida = false;
        // }
        if(cooldown > 0) {
            cooldown--;
        } else {
            if(estado == 1 && cursor.transform.position == transform.position) {
                cooldown = cooldownPadrao;
                cursor.IrParaPosicao(EscolherPosicao(alvoEscolhidoParaAtacar));
                estado = 2;
            } else if(estado == 2) {
                cooldown = cooldownPadrao;
                Mover();
                estado = 3; //vai mudar dependendo se for atacar ou usar habilidade
            } else if(estado == 3) {
                cooldown = cooldownPadrao;
                cursor.MostrarOverlaysAtaque(personagem.TilesAlvosAcessiveis(personagem.arma.alcanceMin, personagem.arma.alcanceMax));
                cursor.IrParaPosicao(alvoEscolhidoParaAtacar.transform.position);
                estado = 4;
            } else if(estado == 4) {
                personagem.Atacar(alvoEscolhidoParaAtacar);
                cursor.LimparOverlays();
                gs.Proximo();
                estado = 0;
                input.cursorAtivo = 0;
            }
        }
        
    }

    public void Iniciar() {
        inimigosAcessiveis = new List<Personagem>();
        input.cursorAtivo = 5;
        List<Personagem> meusInimigos = gs.personagens.Where(x => x.time == 0).ToList();
        terrenoAcessivel = personagem.TilesAcessiveis(tilemap);
        cursor.MostrarOverlaysMovimento(terrenoAcessivel);
        // foreach (Personagem p in meusInimigos)
        // {
            //print(p.Manhattan(p.transform.position, transform.position));
            //print(TilesQuePermitemAtaque(p).Count + ":" + p.nome);
        //}
        porOndeAtacar = new Dictionary<Personagem, List<Vector3>>();
        foreach (Personagem p in meusInimigos)
        {
            if(TilesQuePermitemAtaque(p).Count > 0) {
                porOndeAtacar[p] = TilesQuePermitemAtaque(p);
                inimigosAcessiveis.Add(p);
            }
        }
        alvoEscolhidoParaAtacar = EscolherHeroiAlvo();
        estado = 1;
        
        cooldown = cooldownPadrao;
        // if (targetIndex == -1) {
        //     inimigosAcessiveis = GetInimigosAcessiveis();
        //     targetIndex = Random.Range(0, inimigosAcessiveis.Count);
        //     Vector3 novaPosicao = inimigosAcessiveis[targetIndex].transform.position;
        //     gs.cursor.GetComponent<ControleCursor>().novaPosicao = novaPosicao;
        //     lastTime = Time.time;
        // }
        // if (gs.cursor.GetComponent<ControleCursor>().transform.position == gs.cursor.GetComponent<ControleCursor>().novaPosicao) {
        //     if (!mostrandoOverlay) {
        //         gs.cursor.GetComponent<ControleCursor>().MostrarOverlaysAtaque();
        //         mostrandoOverlay = true;
        //     }
        //     if (transicaoCompletou()) {
        //         gameObject.GetComponent<Personagem>().Atacar(inimigosAcessiveis[targetIndex]);
        //         vezInimigo = false;
        //         targetIndex = -1;
        //         gs.cursor.GetComponent<ControleCursor>().LimparOverlays();
        //         gs.ReiniciarLabelsAlvo();
        //         gs.Proximo();
        //     } 
        // }
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

    private Vector3 EscolherPosicao(Personagem alvo) {
        //int indice = Random.Range(0, )
        int indice = Random.Range(0, porOndeAtacar[alvo].Count);
        return porOndeAtacar[alvo][indice];
    }

    private Personagem EscolherHeroiAlvo() {
        //aprimorar isso depois
        int indice = Random.Range(0, inimigosAcessiveis.Count);
        return inimigosAcessiveis[indice];
    }

    private bool transicaoCompletou() {
        return Time.time - lastTime >= 2f;
    }

    public List<Vector3> TilesQuePermitemAtaque(Personagem heroi) {
        //onde serão inseridos tiles de onde será possível atacar o personagem
        List<Vector3> tilesLivresParaAtaque = new List<Vector3>();
        foreach (Vector3 pos in terrenoAcessivel)
        {
            float dist = heroi.Manhattan(heroi.transform.position, pos);
            if(dist <= personagem.arma.alcanceMax && dist >= personagem.arma.alcanceMin) {
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
