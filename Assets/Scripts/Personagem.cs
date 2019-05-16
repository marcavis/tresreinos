using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Personagem : MonoBehaviour
{
    public string nome;
    public string time;
    private bool piscando = false;
    private int blinkFrames;
    private int chp, mhp;

    public int movimento; 
    private Vector3 destinoFinal, destinoIntermediario;
    private List<Vector3> rota;
    
    //matriz usada para saber como a unidade vai percorrer o caminho para
    //chegar na celula selecionada
    private Vector3[,] cameFrom;
    // Start is called before the first frame update
    void Start()
    {
        movimento = 50;
        destinoFinal = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if(transform.position != destinoFinal) {
            PararDePiscar(); //voltar depois?
            
            // if(transform.position != destinoIntermediario) {
            //     destinoIntermediario = cameFrom
            // }
            transform.position = Vector3.MoveTowards(transform.position, destinoIntermediario, 2f * Time.deltaTime);
        }
        if(piscando) {
            blinkFrames--;
            if(blinkFrames == 0) {
                blinkFrames = 6;
                //inverte a variável enabled, apagando ou redesenhando o objeto
                gameObject.GetComponent<SpriteRenderer>().enabled ^= true;
            }
        }
    }

    public void Piscar() {
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

    //tem q arrumar
    public void MoverPara(Vector3 destino) {
        destinoFinal = destino;
        rota = new List<Vector3>();
        Vector3 ponto = destinoFinal;
        int x0 = (int) transform.position.x;
        int y0 = (int) transform.position.x;
        print("X" + ponto);
        print(cameFrom.Length);
        while(ponto != transform.position) {
            print("X" + ponto);
            print(cameFrom.Length);
            ponto = cameFrom[(int) ponto.x - x0, (int) ponto.y - y0];
            print("|" + ponto);
            rota.Add(ponto);
        }
        foreach (Vector3 p in rota)
        {
            print(p);
        }
    }

    public List<Vector3> TilesAcessiveis(Tilemap tilemap) {
        //System.Diagnostics.Stopwatch st = new System.Diagnostics.Stopwatch();
        //st.Start();
        int dimensaoMat = (int) (movimento * 2 / 10 + 1);

        //int meuX = (int) transform.position.x;
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
        //print(meuX + "+" + meuY + "+" + dimensaoMat);

        openSet.Add(new Vector3(meuX, meuY, 0));

        while(openSet.Count > 0) {
            openSet.Sort((a, b) => fScore[(int) a.x,(int) a.y].CompareTo(fScore[(int)b.x,(int)b.y]));
            
            Vector3 atual = openSet[0];
            openSet.Remove(atual);
            closedSet.Add(atual);
            foreach (Vector3 vizinho in vizinhos(atual.x, atual.y, dimensaoMat))
            {
                if(closedSet.Contains(vizinho)) {
                    continue;
                }
                //vizinho é um vetor com coordenadas relativas à posição atual, mas precisamos de
                //dados da célula real 
                float esteTileX = transform.position.x + vizinho.x - (movimento/10);
                float esteTileY = transform.position.y + vizinho.y - (movimento/10);
                Vector3Int posRealVizinho = new Vector3Int((int) esteTileX, (int) esteTileY, 0);
                float possivel_gScore = gScore[(int) atual.x, (int) atual.y] + custoParaAndar(posRealVizinho, tilemap);// + custo(vizinho)
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

    private List<Vector3> vizinhos(float posX, float posY, int tamanho) {
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

    private int custoParaAndar(Vector3Int alvo, Tilemap tilemap) {
        //custo padrão, portanto muitos tiles não precisarão ter seu custo definido
        //TODO: tirar daqui pois unidades diferentes terão perfis de custo diferentes por terreno
        int custo = 10;
        TileBase tipoTile = tilemap.GetTile(alvo);
        
        if(tipoTile == null) {
            //nao tem tile aqui, entao nao eh passavel
            return 200;
        }
        
        switch (tipoTile.name.Split('-')[0])
        {
            case "carpet":
                custo = 10;
                break;
            case "grass":
                custo = 15;
                break;
            default:
                break;
        }
        return custo;
    }

}
