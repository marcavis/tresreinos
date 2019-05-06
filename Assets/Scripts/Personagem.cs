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

    public float movimento; 
    
    //matriz usada para saber como a unidade vai percorrer o caminho para
    //chegar na celula selecionada
    private Vector3[,] cameFrom;
    // Start is called before the first frame update
    void Start()
    {
        movimento = 3f;
    }

    // Update is called once per frame
    void Update()
    {
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

    public bool PodeAlcancar(Vector3 posicao) {
        return Vector3.Distance(this.transform.position, posicao) <= movimento;
    }

    //recebe uma matriz de tiles centrada na posição atual
    public List<Vector3> TilesAcessiveis(string[,] tiles, Tilemap tilemap) {
        int dimensaoMat = tiles.GetLength(0);

        bool[,] acessivel = new bool[dimensaoMat,dimensaoMat];
        //int meuX = (int) transform.position.x;
        int meuX = dimensaoMat / 2; //centro do quadrado avaliado
        int meuY = dimensaoMat / 2;
        List<Vector3> closedSet = new List<Vector3>();
        List<Vector3> openSet = new List<Vector3>();
        List<Vector3> acessiveis = new List<Vector3>();
        int[,] gScore = new int[dimensaoMat, dimensaoMat];
        //int[,] fScore = new int[tiles.Length,tiles.Length];
        cameFrom = new Vector3[dimensaoMat, dimensaoMat];

        for (int x = 0; x < dimensaoMat; x++) {
            for (int y = 0; y < dimensaoMat; y++) {
                gScore[x,y] = 0;
                //fScore[x,y] = 999;
            }
        }
        gScore[meuX, meuY] = 0;
        print(meuX + "+" + meuY + "+" + dimensaoMat);

        openSet.Add(new Vector3(meuX, meuY, 0));

        while(openSet.Count > 0) {
            //objListOrder.Sort((x, y) => x.OrderDate.CompareTo(y.OrderDate));
            openSet.Sort((a, b) => gScore[(int) a.x,(int) a.y].CompareTo(gScore[(int)b.x,(int)b.y]));
            //print("tamanho" + openSet.Count);
            Vector3 atual = openSet[0];
            openSet.Remove(atual);
            closedSet.Add(atual);
            foreach (Vector3 vizinho in vizinhos(atual.x, atual.y, dimensaoMat))
            {
                if(closedSet.Contains(vizinho)) {
                    continue;
                }
                int possivel_gScore = gScore[(int) atual.x, (int) atual.y] + 1;// + custo(vizinho)
                if(!openSet.Contains(vizinho)) {
                    openSet.Add(vizinho);
                } else if (possivel_gScore >= gScore[(int) vizinho.x, (int) vizinho.y]) {
                    continue;
                }

                //chegamos em um caminho melhor
                cameFrom[(int) vizinho.x, (int) vizinho.y] = atual;
                gScore[(int) vizinho.x, (int) vizinho.y] = possivel_gScore;
                //print("gscore" + vizinho.x + "," + vizinho.y + ":" + possivel_gScore);
                //fScore[(int) vizinho.x, (int) vizinho.y] = possivel_gScore
                //fScore.put(neighbor, gScore.get(neighbor) + Driver.roguelikeDistance(neighbor, destination) * 10);
			
            }
        }
        for(int i = 0; i < dimensaoMat; i++) {
            for (int j = 0; j < dimensaoMat; j++) {
                print(gScore[i,j]);
                if(gScore[i,j] <= movimento) {
                    float esteTileX = transform.position.x + i - movimento;
                    float esteTileY = transform.position.y + j - movimento;
                    acessiveis.Add(new Vector3(esteTileX, esteTileY, 0));
                    print(new Vector3(esteTileX, esteTileY, 0));
                    print(transform.position + "i" + i + "j " + j);
                }
            }
        }
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
        // foreach (Vector3 item in saida)
        // {
        //     print(item + ", " + posX + ","  + posY + "t" + tamanho);
        // }
        return saida;
    }
}
