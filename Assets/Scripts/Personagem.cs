using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Personagem : MonoBehaviour
{
    public string nome;
    //podem ser unidades, inimigos ou NPCs neutros
    public int time;
    private bool piscando = false;
    private int blinkFrames;

    //a forma com que a unidade anda - se sofre penalidades de movimento em terrenos difíceis
    public Dictionary<string, int> andar;

    public int nivel;
    public int exp;

    public int pv, mpv; //pontos de vida, pontos de vida máximos
    public int pt, mpt; //pontos de técnica, pontos de técnica máximos
    public int ataque;
    public int defesa;
    public int agilidade; 
    public int movimento; 

    public Arma arma;
    

    public Vector3 destinoFinal;
    private List<Vector3> rota;
    //manter salva uma rota inversa se o jogador desistir de mover-se
    private List<Vector3> rotaBacktrack;
    //matriz usada para saber como a unidade vai percorrer o caminho para
    //chegar na celula selecionada
    private Vector3[,] cameFrom;

    // Start is called before the first frame update
    void Start()
    {
        //andar = Defines.Andar("normal");
        
        //valores auxiliares para movimentação no campo de batalha
        rota = new List<Vector3>();
        rotaBacktrack = new List<Vector3>();
        destinoFinal = transform.position;
        
        //atributos
        movimento = 70; 
        
        PlayerPrefs.SetString("nome", "Jim");
        PlayerPrefs.Save();
    }

    // Update is called once per frame
    void Update()
    {
        //mover-se para algum tile, se houver essa ordem
        if(rota.Count > 0) {
            PararDePiscar(); //voltar depois?

            if(transform.position == rota[0]) {
                rota.RemoveAt(0);
            }
            if(rota.Count > 0) {
                transform.position = Vector3.MoveTowards(transform.position, rota[0], 10f * Time.deltaTime);
            }
        }

        if(piscando) Piscar(); // ;)
    }

    public void Inicializar(string nome) {
        Defines.Inicializacao(nome, gameObject);
    }

    public void Piscar() {
        blinkFrames--;
        if(blinkFrames == 0) {
            blinkFrames = 6;
            //inverte a variável enabled, apagando ou redesenhando o objeto
            gameObject.GetComponent<SpriteRenderer>().enabled ^= true;
        }
    }
    public void ComecarAPiscar() {
        gameObject.GetComponent<SpriteRenderer>().enabled = false;
        blinkFrames = 6;
        piscando = true;
    }

    public void PararDePiscar() {
        gameObject.GetComponent<SpriteRenderer>().enabled = true;
        blinkFrames = 6;
        piscando = false;
    }

    public bool PodeAlcancar(Vector3 posicao) {
        return Vector3.Distance(this.transform.position, posicao) <= movimento;
    }

    //retorna o caminho que a unidade percorrerá graficamente ao mover-se
    public void PrepararCaminho() {
        Vector3 destino = destinoFinal;
        rota = new List<Vector3>();
        Vector3 ponto = PosicaoNaMatrizMov(destino);
        rota.Add(destino);

        while(ponto != new Vector3(movimento/10, movimento/10, 0)) {
            ponto = cameFrom[(int) ponto.x, (int) ponto.y];
            rota.Add(PosicaoNoMapa(ponto));
        }
        rotaBacktrack = new List<Vector3>(rota);
        rota.Reverse();
        //remover o primeiro item da rota, que é a própria posição atual
        rota.RemoveAt(0);
        //print(rotaBacktrack[0]);
        //print(rota[0]);
    }

    public void DesfazerMovimento() {
        destinoFinal = rotaBacktrack[rotaBacktrack.Count - 1];
        rota = new List<Vector3>(rotaBacktrack);
    }

    //converte entre coordenadas do mapa em coordenadas na matriz de movimentos possíveis
    public Vector3 PosicaoNaMatrizMov(Vector3 entrada) {
        int m = movimento/10;
        return new Vector3(m + entrada.x - transform.position.x, m + entrada.y - transform.position.y, 0);
    }

    //converte entre coordenadas da matriz de movimentos possíveis em coordenadas no mapa
    public Vector3 PosicaoNoMapa(Vector3 entrada) {
        int m = movimento/10;
        return new Vector3(-m + entrada.x + transform.position.x, -m + entrada.y + transform.position.y, 0);
    }

    public List<Vector3> TilesAcessiveis(Tilemap tilemap) {
        //TODO: incluir custo 999 em tiles ocupados por inimigos
        //System.Diagnostics.Stopwatch st = new System.Diagnostics.Stopwatch();
        //st.Start();
        int dimensaoMat = (int) (movimento * 2 / 10 + 1);

        int meuX = dimensaoMat / 2; //centro do quadrado avaliado
        int meuY = dimensaoMat / 2;
        List<Vector3> closedSet = new List<Vector3>();
        List<Vector3> openSet = new List<Vector3>();
        List<Vector3> acessiveis = new List<Vector3>();
        float[,] gScore = new float[dimensaoMat, dimensaoMat];
        float[,] fScore = new float[dimensaoMat, dimensaoMat];
        cameFrom = new Vector3[dimensaoMat, dimensaoMat];

        for (int x = 0; x < dimensaoMat; x++) {
            for (int y = 0; y < dimensaoMat; y++) {
                gScore[x,y] = 999f;
                fScore[x,y] = 999f;
            }
        }
        gScore[meuX, meuY] = 0;
        fScore[meuX, meuY] = movimento;

        openSet.Add(new Vector3(meuX, meuY, 0));

        while(openSet.Count > 0) {
            openSet.Sort((a, b) => fScore[(int) a.x,(int) a.y].CompareTo(fScore[(int)b.x,(int)b.y]));
            
            Vector3 atual = openSet[0];
            openSet.Remove(atual);
            closedSet.Add(atual);
            foreach (Vector3 vizinho in Vizinhos(atual.x, atual.y, dimensaoMat))
            {
                if(closedSet.Contains(vizinho)) {
                    continue;
                }
                //vizinho é um vetor com coordenadas relativas à posição atual, mas precisamos de
                //dados da célula real 
                float esteTileX = transform.position.x + vizinho.x - (movimento/10);
                float esteTileY = transform.position.y + vizinho.y - (movimento/10);
                Vector3Int posRealVizinho = new Vector3Int((int) esteTileX, (int) esteTileY, 0);
                float possivel_gScore = gScore[(int) atual.x, (int) atual.y] + CustoParaAndar(posRealVizinho, tilemap);// + custo(vizinho)
                if(!openSet.Contains(vizinho)) {
                    openSet.Add(vizinho);
                } else if (possivel_gScore >= gScore[(int) vizinho.x, (int) vizinho.y]) {
                    continue;
                }

                //chegamos em um caminho melhor
                cameFrom[(int) vizinho.x, (int) vizinho.y] = atual;
                gScore[(int) vizinho.x, (int) vizinho.y] = possivel_gScore;
                float manhattan = Mathf.Abs(meuX - vizinho.x) + Mathf.Abs(meuY - vizinho.y);
                fScore[(int) vizinho.x, (int) vizinho.y] = gScore[(int) vizinho.x, (int) vizinho.y] + manhattan;
			
            }
        }
        for(int i = 0; i < dimensaoMat; i++) {
            for (int j = 0; j < dimensaoMat; j++) {
                if(Mathf.Floor(gScore[i,j]) <= movimento) {
                    float esteTileX = transform.position.x + i - movimento/10;
                    float esteTileY = transform.position.y + j - movimento/10;
                    acessiveis.Add(new Vector3(esteTileX, esteTileY, 0));
                }
            }
        }
        //st.Stop();
        //print(st.ElapsedMilliseconds);
        return acessiveis;
    }

    //para uma posição numa matriz, retorna as posições adjacentes, se couberem na matriz
    private List<Vector3> Vizinhos(float posX, float posY, int tamanho) {
        List<Vector3> saida = new List<Vector3>();
        //vizinho à esquerda
        if(posX - 1 >= 0) saida.Add(new Vector3(posX - 1, posY, 0));
        //direita
        if(posX + 1 < tamanho) saida.Add(new Vector3(posX + 1, posY, 0));
        //superior
        if(posY - 1 >= 0) saida.Add(new Vector3(posX, posY - 1, 0));
        //inferior
        if(posY + 1 < tamanho) saida.Add(new Vector3(posX, posY + 1, 0));
        return saida;
    }

    private int CustoParaAndar(Vector3Int alvo, Tilemap tilemap) {
        //custo padrão, portanto muitos tiles não precisarão ter seu custo definido
        //TODO: tirar daqui pois unidades diferentes terão perfis de custo diferentes por terreno
        int custo = 10;
        TileBase tipoTile = tilemap.GetTile(alvo);
        
        if(tipoTile == null) {
            //nao tem tile aqui, entao nao eh passavel
            return 200;
        }
        
        string prefixoDoTipo = tipoTile.name.Split('-')[0];
        if(andar.ContainsKey(prefixoDoTipo)) {
            custo = andar[prefixoDoTipo];
        }
        return custo;
    }


    public void ReceberAtaque(int poder, Arma arma) {
        ReceberDano(poder - defesa, arma);
    }

    public void ReceberDano(int dano, Arma arma) {
        float danoCalculado = dano * (1 + UnityEngine.Random.Range(-arma.variacao, arma.variacao));
        int novoDano = Mathf.FloorToInt(danoCalculado);
        if(novoDano < 1) {novoDano = 1;}
        pv -= novoDano;
        if(pv <= 0) {
            //MORRER
        }
    }
}
