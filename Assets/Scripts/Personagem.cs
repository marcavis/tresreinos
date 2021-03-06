﻿using System;
using System.Linq;
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
    public int nivelBase; //1 para herois, varia para inimigos
    public int exp;

    public float[] crescimento; //informa como os stats crescem conforme a unidade ganha experiência

    public int pv, mpv; //pontos de vida, pontos de vida máximos
    public int pt, mpt; //pontos de técnica, pontos de técnica máximos
    private int ataque;
    private int defesa;
    private int agilidade; 
    private int movimento; 

    //variável que mostra o quão próxima a unidade está da próxima rodada
    public int iniciativa;

    public Arma arma;

    public List<Efeito> efeitos;
    
    public Item[] inventario;
    public int posItemAtual;
    public Item itemAtual;
    public Habilidade[] habilidades;
    public Habilidade habilidadeAtual;

    public Vector3 destinoFinal;
    private List<Vector3> rota;
    //manter salva uma rota inversa se o jogador desistir de mover-se
    private List<Vector3> rotaBacktrack;
    //matriz usada para saber como a unidade vai percorrer o caminho para
    //chegar na celula selecionada
    private Vector3[,] cameFrom;

    
    private GerenciadorScript gs;
    private Animator anim;
    private AttackParent ap;
    private int direcao;
    private Arma ultimaArma;

    // Start is called before the first frame update
    void Start()
    {
        //andar = Defines.Andar("normal");
        efeitos = new List<Efeito>();
        
        //valores auxiliares para movimentação no campo de batalha
        rota = new List<Vector3>();
        rotaBacktrack = new List<Vector3>();
        destinoFinal = transform.position;

        inventario = new Item[8];
        habilidades = new Habilidade[8];
        //atributos
        Defines.Inicializacao(nome, gameObject);
        
        gs = GameObject.Find("Gerenciador").GetComponent<GerenciadorScript>();
        gs.AdicionarPersonagem(gameObject);
        anim = GetComponent<Animator>();
        ControladorDano.Init();
        ap = GameObject.Find("Placeholder").GetComponent<AttackParent>();
    }

    // Update is called once per frame
    void Update()
    {
        if(pv == 0) {
            Color nova = gameObject.GetComponent<SpriteRenderer>().color;
            nova.a -= 0.006f;
            nova.a = Mathf.Abs(nova.a);
            gameObject.GetComponent<SpriteRenderer>().color = nova;
            if(nova.a < 0.01f) {
                Destroy(gameObject);
            }
        }
        //mover-se para algum tile, se houver essa ordem
        if(rota.Count > 0) {
            PararDePiscar(); //voltar depois?

            if(transform.position == rota[0]) {
                rota.RemoveAt(0);
            }
            if(rota.Count > 0) {
                setDirecao(rota[0].x, rota[0].y);
                transform.position = Vector3.MoveTowards(transform.position, rota[0], 10f * Time.deltaTime);
            }
        } else {
            if (anim != null) anim.SetInteger("direcao", -1);
        }

        if(piscando) Piscar(); // ;)
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
        return Vector3.Distance(this.transform.position, posicao) <= Movimento();
    }

    //retorna o caminho que a unidade percorrerá graficamente ao mover-se
    public void PrepararCaminho() {
        Vector3 destino = destinoFinal;
        rota = new List<Vector3>();
        Vector3 ponto = PosicaoNaMatrizMov(destino);
        rota.Add(destino);

        while(ponto != new Vector3(Movimento()/10, Movimento()/10, 0)) {
            ponto = cameFrom[(int) ponto.x, (int) ponto.y];
            rota.Add(PosicaoNoMapa(ponto));
        }
        rotaBacktrack = new List<Vector3>(rota);
        rota.Reverse();
        //remover o primeiro item da rota, que é a própria posição atual
        rota.RemoveAt(0);
    }

    public void DesfazerMovimento() {
        destinoFinal = rotaBacktrack[rotaBacktrack.Count - 1];
        rota = new List<Vector3>(rotaBacktrack);
    }

    //converte entre coordenadas do mapa em coordenadas na matriz de movimentos possíveis
    public Vector3 PosicaoNaMatrizMov(Vector3 entrada) {
        int m = Movimento()/10;
        return new Vector3(m + entrada.x - transform.position.x, m + entrada.y - transform.position.y, 0);
    }

    //converte entre coordenadas da matriz de movimentos possíveis em coordenadas no mapa
    public Vector3 PosicaoNoMapa(Vector3 entrada) {
        int m = Movimento()/10;
        return new Vector3(-m + entrada.x + transform.position.x, -m + entrada.y + transform.position.y, 0);
    }

    // public Dictionary<Vector3, int> DistanciasParaAtingir(int mov, Vector3 alvo) {
    //     Tilemap tilemap = GameObject.Find("Tilemap").GetComponent<Tilemap>();
    // }

    public List<Vector3> TilesAcessiveis(Tilemap tilemap) {
        return TilesAcessiveis(Movimento(), tilemap);
    }

    public List<Vector3> TilesAcessiveis(int mov, Tilemap tilemap) {
        //System.Diagnostics.Stopwatch st = new System.Diagnostics.Stopwatch();
        //st.Start();
        int dimensaoMat = (int) (mov * 2 / 10 + 1);

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
        fScore[meuX, meuY] = mov;

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
                float esteTileX = transform.position.x + vizinho.x - (mov/10);
                float esteTileY = transform.position.y + vizinho.y - (mov/10);
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
                if(Mathf.Floor(gScore[i,j]) <= mov) {
                    float esteTileX = transform.position.x + i - mov/10;
                    float esteTileY = transform.position.y + j - mov/10;
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
        int custo = 10;
        TileBase tipoTile = tilemap.GetTile(alvo);
        
        if(tipoTile == null) {
            //nao tem tile aqui, entao provavelmente é o chão normal da fase, com custo normal
            return 10;
        }
        //se houver inimigo no tile, ele não pode ser cruzado
        Personagem ocupante = gs.ObjetoNoTile(alvo);
        if(ocupante != null && ocupante.time != time) {
            return 999;
        }
        
        string prefixoDoTipo = tipoTile.name.Split('-')[0];
        if(andar.ContainsKey(prefixoDoTipo)) {
            custo = andar[prefixoDoTipo];
        }
        return custo;
    }

    //verifica se o tile é de tipo que pode ser ocupado pela unidade
    public bool TerrenoOcupavel(Vector3Int pos) {
        Tilemap tilemap = GameObject.Find("Tilemap").GetComponent<Tilemap>();
        return CustoParaAndar(pos, tilemap) < 999;
    }

    public bool ExistemPersonagensAlvos(int alcanceMin, int alcanceMax, bool seMesmoTime) {
        return AlvosAcessiveisFiltrados(alcanceMin, alcanceMax, seMesmoTime).Count > 0;
    }

    //necessário filtrar alvos que podem ser selecionados dependendo do time
    public List<Vector3> AlvosAcessiveisFiltrados(int alcanceMin, int alcanceMax, bool seMesmoTime) {
        bool condicaoTime;
        List<Vector3> alvos = new List<Vector3>();
        foreach (Vector3 posicao in TilesAlvosAcessiveis(alcanceMin, alcanceMax))
        {
            Personagem ocupante = gs.ObjetoNoTile(posicao);
            if(ocupante == null) {
                continue;
            }
            if(seMesmoTime) {condicaoTime = ocupante.time == time;}
            else {condicaoTime = ocupante.time != time;}
            if(condicaoTime) {
                //então temos ao menos um alvo possível
                alvos.Add(ocupante.transform.position);
            }
        }
        return alvos;
    }

    public static float Manhattan(Vector3 v, Vector3 w) {
        return Mathf.Abs(v.x - w.x) + Mathf.Abs(v.y - w.y);
    }

    // public List<Vector3> TilesAlvosAcessiveis(int alcanceMax) {
    //     return TilesAlvosAcessiveis(1, alcanceMax);
    // }
    public List<Vector3> TilesAlvosAcessiveis(int alcanceMin, int alcanceMax) {
        List<Vector3> acessiveis = new List<Vector3>();
        int dimensaoMat = 2 * alcanceMax + 1;
        Vector3 posicao;
        for (int i = 0; i < dimensaoMat; i++) {
            for (int j = 0; j < dimensaoMat; j++) {
                posicao = destinoFinal + new Vector3(i - alcanceMax, j - alcanceMax, 0);
                float manhattan = Manhattan(posicao, destinoFinal);
                if(manhattan <= alcanceMax && manhattan >= alcanceMin) {
                    acessiveis.Add(posicao);
                }
            }
        }
        return acessiveis;
    }

    public void Atacar(Personagem alvo) {
        int dado = UnityEngine.Random.Range(0, 100);
        if (dado + arma.precisao < 5) {
            //erro
            //será que vai ser interessante implementar
            //arma.efeitoFalha() ?
            if (time == 1)
            ap.PlayLeft("MISS",true);
            else 
            ap.PlayRight("MISS");
            print(nome + " falhou ao atacar " + alvo.nome);
        } else {
            dado = UnityEngine.Random.Range(0, 100);
            if (dado + arma.chanceCritica > 95) {
                arma.efeitoCritico(this, alvo);
            } else {
                arma.efeitoAtaque(this, alvo);
            }
        }
        //atualizar a interface quanto à situação do inimigo que atacamos
        gs.MostrarDadosDoAlvo(alvo);
    }

    public void ReceberAtaque(int poder, Personagem atacante) {
        Arma arma = atacante.arma;
        int dano = poder - Defesa();
        float danoCalculado = dano * (100 + UnityEngine.Random.Range(-arma.variacao, arma.variacao)) / 100;
        int novoDano = Mathf.FloorToInt(danoCalculado);
        novoDano = AplicarEfeitosNoDano(novoDano);
        ReceberDano(novoDano, atacante);
    }

    public void ReceberAtaqueMagico(int poder, Personagem atacante) {
        Habilidade hab = atacante.habilidadeAtual;
        float danoCalculado = poder * (100 + UnityEngine.Random.Range(-hab.variacao, hab.variacao)) / 100;
        int novoDano = Mathf.FloorToInt(danoCalculado);
        novoDano = AplicarEfeitosNoDanoMagico(novoDano);
        ReceberDano(novoDano, atacante);
    }

    public void ReceberDano(int dano, Personagem atacante) {
        // Arma arma = atacante.arma;
        int vidaInicial = pv;
        // float danoCalculado = dano * (100 + UnityEngine.Random.Range(-arma.variacao, arma.variacao)) / 100;
        // int novoDano = Mathf.FloorToInt(danoCalculado);
        if(dano < 1) {dano = 1;}
        pv = Mathf.Max(0, pv - dano);
        if(time == 1) {
            GetComponent<Inimigo>().acabaramDeMeAtacar.Add(atacante);
        }

        //debug
        //ControladorDano.criaTextoDano(novoDano.ToString(), transform);
        if (time == 1) {
            if (ultimaArma == atacante.arma) ap.PlayLeft(dano.ToString(), true, 1.35f, 1);
            else ap.PlayLeft(dano.ToString(), true);
        } else {
            if (ultimaArma == atacante.arma) ap.PlayRight(dano.ToString(), -1.35f, 1);
            else ap.PlayRight(dano.ToString());
        }
        ultimaArma = atacante.arma;
        print(nome + " sofreu " + dano + " pontos de dano.");

        //dar 20 exp base se morrer, e variando de 0 a 30 linearmente conforme o dano proporcional causado pelo inimigo
        int expDada = Mathf.Min(30 * vidaInicial/MPV(), 30 * dano/MPV()) + (pv == 0 ? 20 : 0);
        expDada = EscalaExp(expDada, nivel - atacante.nivel);
        atacante.ReceberExperiencia(Mathf.Max(1, expDada));

        if(pv == 0) {
            //TODO: animação de morte? explosão? som de morte?
            //já remover da lista de personagens para não ser utilizado
            //nesse ponto a unidade já está morta, mas 
            gs.personagens.Remove(this);
        }
    }

    public int EscalaExp(int expDada, int delta) {
        if(delta > 0) {return Mathf.CeilToInt(expDada * (1 + delta * 0.25f));}
        else if (delta == 0) {return expDada;}
        else {return Mathf.CeilToInt(expDada * (1 + delta * 0.1f));}
        //pode retornar negativo se o atacante estiver 11 níveis acima do atacado,
        //mas nesse caso é garantido o mínimo de 1 exp para o atacante
    }

    public void ReceberCura(int valor) {
        int vidaInicial = pv;
        pv += valor;
        if(pv > MPV()) {pv = MPV();}
        ap.PlayLeft((pv - vidaInicial).ToString(), false);
        print(nome + " ganhou " + (pv - vidaInicial) + " pontos de vida.");
    }

    public void ReceberExperiencia(int expRecebida) {
        exp += expRecebida;
        int niveisAGanhar = 0;
        if(exp > 100) {
            niveisAGanhar = exp/100;
            exp = exp % 100;
            int [] atributosAntigos = new int[] {MPV(), MPT(), Ataque(), Defesa(), Agilidade()};
            nivel += niveisAGanhar;
            gs.AdicionarMsgNivel(this, niveisAGanhar, atributosAntigos);
        }
    }

    //retorna -1 se não houver espaço
    public int AdicionarAoInventario(Item item) {
        int espacoVazio = -1;
        for (int i = 0; i < Defines.TAMANHO_INVENTARIO; i++)
        {
            if (inventario[i] == null) {
                espacoVazio = i;
                break;
            }
        }
        if(espacoVazio != -1) {
            inventario[espacoVazio] = item;
        }
        return espacoVazio;
    }

    public void DescartarItem(int indice) {
        //remover arma, se descartada
        if(inventario[indice].nome == arma.nome) {
            arma = DefinesArmas.armas["Punho"];
        }
        inventario[indice] = null;
    }

    public void AdicionarHabilidade(Habilidade hab) {
        for (int i = 0; i < Defines.TAMANHO_INVENTARIO; i++)
        {
            if (habilidades[i] == null) {
                habilidades[i] = hab;
                return;
            }
        }
    }

    public void UsarItem(Personagem unid) {
        
        GerenciadorScript gs = GameObject.Find("Gerenciador").GetComponent<GerenciadorScript>();
        AttackParent ap = GameObject.Find("Placeholder").GetComponent<AttackParent>();
        List<Action<GerenciadorDialogo>> list = new List<Action<GerenciadorDialogo>>();
        list.Add(gd => {
            gd.IrPara(unid.transform.position);
        });
        list.Add(gd => {
            ap.Abrir();
            ap.SetLeftAnimator(Defines.animacoesAtk[this.nome]);
            ap.SetRightAnimator(Defines.animacoesAtk[unid.nome]);
        });
        list.Add(gd => {
            itemAtual.efeitoUso(this, unid);
        });
        list.Add(gd => {
            ap.Fechar();
        });
        inventario[posItemAtual] = null;
        gs.mensagensPendentes.Add(list);
    }

    public void UsarHabilidade(Vector3 posCentral) {
        
        //quais alvos selecionar, dependendo se a habilidade afeta alvos do mesmo time ou do time inimigo
        int timeAlvo = habilidadeAtual.seMesmoTime ? this.time : (this.time + 1) % 2;
        List<Vector3> tilesAlcancados = habilidadeAtual.areaDeEfeito;
        List<Personagem> alvos = new List<Personagem>();
        GerenciadorScript gs = GameObject.Find("Gerenciador").GetComponent<GerenciadorScript>();
        AttackParent ap = GameObject.Find("Placeholder").GetComponent<AttackParent>();
        List<Action<GerenciadorDialogo>> list = new List<Action<GerenciadorDialogo>>();
        foreach (Vector3 offset in tilesAlcancados)
        {
            //considerando posCentral como centro, tenta ver se encontra um alvo possível em todos os offsets
            //da área de efeito da habilidade
            Personagem alvoPossivel = gs.ObjetoNoTile(posCentral + offset);
            if(alvoPossivel == null) {
                continue;
            } else if(alvoPossivel.time == timeAlvo) {

                list.Add(gd => {
                    gd.IrPara(posCentral + offset);
                });
                list.Add(gd => {
                    ap.Abrir();
                    ap.SetLeftAnimator(Defines.animacoesAtk[this.nome]);
                    ap.SetRightAnimator(Defines.animacoesAtk[alvoPossivel.nome]);
                });
                list.Add(gd => {
                    habilidadeAtual.efeitoUso(this, alvoPossivel);
                });
                list.Add(gd => {
                    ap.Fechar();
                });
            }
        }
        gs.mensagensPendentes.Add(list);
    }

    //métodos necessários pois personagens podem ter diversos modificadores agindo nos status
    //stats atualizados são calculados aqui, conforme vetor de crescimentos de stat por nível
    public int MPV() {
        int valorBase = Mathf.FloorToInt(mpv + (nivel - nivelBase) * crescimento[0]);
        return valorBase;
    }

    public int MPT() {
        int valorBase = Mathf.FloorToInt(mpt + (nivel - nivelBase) * crescimento[1]);
        return valorBase;
    }
    public int Ataque(bool seBase) {
        int valorBase = Mathf.FloorToInt(ataque + (nivel - nivelBase) * crescimento[2]);
        int valor = valorBase;
        if(!seBase) {
            foreach (Efeito e in efeitos)
            {
                if(e.efeitoNoAtaque != null) {
                    valor = e.efeitoNoAtaque(this, valor);
                }
            }
        }
        return valor + arma.poder;
    }

    public int Ataque() {
        return Ataque(false);
    }

    public int Defesa(bool seBase) {
        int valorBase = Mathf.FloorToInt(defesa + (nivel - nivelBase) * crescimento[3]);
        int valor = valorBase;
        if(!seBase) {
            foreach (Efeito e in efeitos)
            {
                if(e.efeitoNaDefesa != null) {
                    valor = e.efeitoNaDefesa(this, valor);
                }
            }
        }
        return valor;
    }

    public int Defesa() {
        return Defesa(false);
    }

    public int Agilidade(bool seBase) {
        int valorBase = Mathf.FloorToInt(agilidade + (nivel - nivelBase) * crescimento[4]);
        int valor = valorBase;
        if(!seBase) {
            foreach (Efeito e in efeitos)
            {
                if(e.efeitoNaAgilidade != null) {
                    valor = e.efeitoNaAgilidade(this, valor);
                }
            }
        }
        return valor;
    }

    public int Agilidade() {
        return Agilidade(false);
    }

    public int Movimento(bool seBase) {
        int valorBase = movimento;
        int valor = valorBase;
        if(!seBase) {
            foreach (Efeito e in efeitos)
            {
                if(e.efeitoNoMovimento != null) {
                    valor = e.efeitoNoMovimento(this, valor);
                }
            }
        }
        return valor;
    }

    public int Movimento() {
        return Movimento(false);
    }

    public void SetMPVBase(int valor) {
        mpv = valor;
    }
    public void SetMPTBase(int valor) {
        mpt = valor;
    }

    public void SetAtaqueBase(int valor) {
        ataque = valor;
    }

    public void SetDefesaBase(int valor) {
        defesa = valor;
    }

    public void SetAgilidadeBase(int valor) {
        agilidade = valor;
    }

    public void SetMovimentoBase(int valor) {
        movimento = valor;
    }

    //reduzir contagem de efeitos pendentes, e excluir inativos/expirados
    public void DegradarEfeitos() {
        List<Efeito> aApagar = new List<Efeito>();
        foreach (Efeito e in efeitos)
        {
            if(e.duracao > 0) {
                e.duracao--;
            }
            if(e.duracao == 0 || e.inativo) {
                aApagar.Add(e);
            }
        }
        foreach (Efeito e in aApagar)
        {
            efeitos.Remove(e);
            if(e.efeitoExpirar != null) {
                e.efeitoExpirar(this); //executar o comando que ocorre quando expira o efeito
            }
            GerenciadorScript gs = GameObject.Find("Gerenciador").GetComponent<GerenciadorScript>();
            string texto = this.nome + " não está mais sob efeito de " + e.nome + "!";
            List<Action<GerenciadorDialogo>> mensagem =  new List<Action<GerenciadorDialogo>> {
                gd => {gd.Dialogo("Efeito", texto);}
            };
            gs.mensagensPendentes.Add(mensagem);    
        }
    }

    public void PreTurno() {
        
    }

    public void PosTurno() {
        //não implementado neste momento
        DegradarEfeitos();
    }

    public void AdicionarEfeito(string nome) {
        if(this.pv == 0) {
            return;
        }
        bool jaExiste = false;
        Efeito novo = DefinesEfeitos.efeitos[nome];
        foreach (var e in efeitos)
        {
            if(e.nome == nome) {
                //se efeito já existe, reiniciar a contagem da duração
                //melhoria possível - efeito pode ter uma função informando o que fazer (nada, resetar, stack)
                e.duracao = novo.duracao;
                jaExiste = true;
            }
        }
        if(!jaExiste) {
            efeitos.Add(novo);
            print(nome);
            GerenciadorScript gs = GameObject.Find("Gerenciador").GetComponent<GerenciadorScript>();
            string texto = this.nome + " agora está sob efeito de " + novo.nome + "!";
            List<Action<GerenciadorDialogo>> mensagem =  new List<Action<GerenciadorDialogo>> {
                gd => {gd.Dialogo("Efeito", texto);}
            };
            gs.mensagensPendentes.Add(mensagem);
        }
    }

    public int AplicarEfeitosNoDano(int valor) {
        foreach (Efeito e in efeitos)
        {
            //a prioridade de aplicação de efeitos é aleatória, nesse ponto
            if (e.efeitoNoDano != null) {
                valor = e.efeitoNoDano(this, valor);
            }
        }
        return valor;
    }

    public int AplicarEfeitosNoDanoMagico(int valor) {
        foreach (Efeito e in efeitos)
        {
            if (e.efeitoNoDanoMagico != null) {
                valor = e.efeitoNoDanoMagico(this, valor);
            }
        }
        return valor;
    }

    private void setDirecao(float x, float y) {
        if (transform.position.x < x) {
            direcao = 2;
        } else if (transform.position.x > x) {
            direcao = 3;
        } else if (transform.position.y < y) {
            direcao = 1;
        } else if (transform.position.y > y) {
            direcao = 0;
        }
        if (anim != null) anim.SetInteger("direcao", direcao);
    }
}
