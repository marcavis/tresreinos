using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Defines 
{   
    public const int TAMANHO_INVENTARIO = 8;
    //                                          
    public static string[] herois = new string[]  { "Zheng Xiulan", 
                                                    "Miao Lin", 
                                                    "Tao Jiang", 
                                                    "Liu Jingsheng", 
                                                    "Jiang Xun", 
                                                    "Guan Long"};

    public Personagem[] objHerois = new Personagem[]  {GameObject.Find("ZhengXiulan").GetComponent<Personagem>(), 
                                                    GameObject.Find("MiaoLin").GetComponent<Personagem>(), 
                                                    GameObject.Find("TaoJiang").GetComponent<Personagem>(), 
                                                    GameObject.Find("LiuJingsheng").GetComponent<Personagem>(), 
                                                    GameObject.Find("JiangXun").GetComponent<Personagem>(), 
                                                    GameObject.Find("GuanLong").GetComponent<Personagem>()};       
    public const int ANDAR_NORMAL = 0;
    public const int ANDAR_TRILHA = 1;
    public const int ANDAR_RUSTICO = 2;
    public const int ANDAR_AQUATICO = 3;
    public const int ANDAR_SUPER = 4;
    public static Dictionary<string, int> tiposDeAndar = new Dictionary<string, int>() {
        {herois[0], ANDAR_NORMAL},
        {herois[1], ANDAR_TRILHA},
        {herois[2], ANDAR_RUSTICO},
        {herois[3], ANDAR_NORMAL},
        {herois[4], ANDAR_NORMAL},
        {herois[5], ANDAR_NORMAL},

        {"Ogro", ANDAR_TRILHA},
        {"Chefe Ogro", ANDAR_TRILHA},
        {"Esqueleto", ANDAR_NORMAL},
        {"Xingba", ANDAR_AQUATICO},
    };
    public static Dictionary<string, int> Andar(int tipo) {
        
        switch (tipo)
        {
            case ANDAR_TRILHA:
                return new Dictionary<string, int>() {
                    {"grass", 10},
                    {"carpet", 10},
                    {"caverock", 15},
                    {"cavewater", 20},
                    {"blackpool", 20},
                    {"stalagmite", 999},
                    {"hole", 999}
                };
            case ANDAR_RUSTICO:
                return new Dictionary<string, int>() {
                    {"grass", 10},
                    {"carpet", 10},
                    {"caverock", 20},
                    {"cavewater", 25},
                    {"blackpool", 20},
                    {"stalagmite", 999},
                    {"hole", 999}
                };
            case ANDAR_AQUATICO:
                return new Dictionary<string, int>() {
                    {"grass", 20},
                    {"carpet", 20},
                    {"caverock", 25},
                    {"cavewater", 10},
                    {"blackpool", 15},
                    {"stalagmite", 999},
                    {"hole", 999}
                };
            case ANDAR_SUPER:
                return new Dictionary<string, int>() {
                    {"grass", 10},
                    {"carpet", 10},
                    {"caverock", 10},
                    {"cavewater", 10},
                    {"blackpool", 10},
                    {"stalagmite", 999},
                    {"hole", 999}
                };
            default:
                //unidades com tipo de andar "normal" se encaixarão aqui
                return new Dictionary<string, int>() {
                    {"grass", 15},
                    {"carpet", 10},
                    {"caverock", 25},
                    {"cavewater", 30},
                    {"blackpool", 20},
                    {"stalagmite", 999},
                    {"hole", 999}
                    //terrenos que não aparecerem na lista terão custo 10
                };
        }
    }

    //define atributos no nível 1
    //nessa ordem = {mpv, mpt, ataque, defesa, agilidade, movimento, nível}
    public static Dictionary<string, int[]> atributosIniciais = new Dictionary<string, int[]>() {
        
        {herois[0], new int[] {18, 12, 12, 11, 19, 50, 1}},
        {herois[1], new int[] {21,  6, 14, 12, 20, 50, 1}},
        {herois[2], new int[] {25,  4, 15, 14, 16, 50, 1}},
        {herois[3], new int[] {20,  6, 13, 10, 15, 50, 1}},
        {herois[4], new int[] {24,  7, 14, 12, 22, 60, 1}},
        {herois[5], new int[] {24,  8, 15, 14, 17, 50, 1}},

        {"Ogro",       new int[] {16,  0, 16, 10, 16, 60, 1}},
        {"Chefe Ogro", new int[] {24,  0, 18, 12, 20, 60, 3}},
        {"Esqueleto",  new int[] {12,  0, 19, 10, 18, 50, 2}},
        {"Xingba",     new int[] {20,  0, 20, 14, 22, 60, 5}},
    };

    public static Dictionary<string, float[]> crescimentoPorNivel = new Dictionary<string, float[]>() {
        {herois[0], new float[] {1.8f, 1.2f, 1.2f, 1.1f, 1.9f}},
        {herois[1], new float[] {2.1f, 0.6f, 1.4f, 1.2f, 2.0f}},
        {herois[2], new float[] {2.5f, 0.4f, 1.5f, 1.4f, 1.6f}},
        {herois[3], new float[] {2.0f, 0.6f, 1.2f, 1.0f, 1.5f}},
        {herois[4], new float[] {2.4f, 0.7f, 1.4f, 1.2f, 2.2f}},
        {herois[5], new float[] {2.4f, 0.8f, 1.5f, 1.4f, 1.7f}},

        {"Ogro",       new float[] {1.6f, 0.0f, 1.6f, 1.0f, 1.6f}},
        {"Chefe Ogro", new float[] {2.4f, 0.0f, 1.8f, 1.2f, 2.0f}},
        {"Esqueleto",  new float[] {1.2f, 0.0f, 1.9f, 1.0f, 1.8f}},
        {"Xingba",     new float[] {2.0f,  0.0f, 2.0f, 1.4f, 2.2f}},
    };
    public static Dictionary<string, string> armasIniciais = new Dictionary<string, string>() {
        
        {herois[0], "Bastão"},
        {herois[1], "Espadas-Borboleta"},
        {herois[2], "Machado"},
        {herois[3], "Chu-Ko-Nu"},
        {herois[4], "Lança"},
        {herois[5], "Espada"},
    };

    //não é necessário adicionar a arma inicial neste ponto
    public static Dictionary<string, string[]> itensIniciais = new Dictionary<string, string[]>() {
        
        {herois[0], new string[] {"Chá Verde", "Chá Verde"}},
        {herois[1], new string[] {"Chá Verde"}},
        {herois[2], new string[] {"Chá Verde"}},
        {herois[3], new string[] {"Chá Verde"}},
        {herois[4], new string[] {"Chá Verde"}},
        {herois[5], new string[] {"Chá Verde"}},

    };

    public static Dictionary<string, string[]> habilidadesIniciais = new Dictionary<string, string[]>() {
        {herois[0], new string[] {"Cura", "Reversão"}},
        {herois[1], new string[] {"Estudo de Campo"}},
        {herois[2], new string[] {"Corte Diagonal"}},
        {herois[3], new string[] {"Tiro de dispersão"}},
        {herois[4], new string[] {"Dardo Paralisante", "Armageddon"}},
        {herois[5], new string[] {"Empurrão de Escudo"}},
    };

    public static Dictionary<string, Animator> animacoesAtk = new Dictionary<string, Animator>() {
        {herois[0], Resources.Load<Animator>("Battle/Anims/ZhengXiulan")},
        {herois[1], Resources.Load<Animator>("Battle/Anims/MiaoLin")},
        {herois[2], Resources.Load<Animator>("Battle/Anims/TaoJiang")},
        {herois[3], Resources.Load<Animator>("Battle/Anims/LiuJingsheng")},
        {herois[4], Resources.Load<Animator>("Battle/Anims/JiangXun")},
        {herois[5], Resources.Load<Animator>("Battle/Anims/GuanLong")},
        {"Ogro", Resources.Load<Animator>("Battle/Anims/Ogro")},
        {"Chefe Ogro", Resources.Load<Animator>("Battle/Anims/CapOgro")},
        {"Esqueleto", Resources.Load<Animator>("Battle/Anims/Esqueleto")},
        {"Xingba", Resources.Load<Animator>("Battle/Anims/Xingba")}
    };

    public static void Inicializacao(string nome, GameObject objeto) {
        //mudar tudo isso se forem carregados dados na segunda fase - usar PlayerPrefs?
        Personagem unid = objeto.GetComponent<Personagem>();
        unid.andar = Andar(tiposDeAndar[nome]);
        unid.pv = atributosIniciais[nome][0];
        unid.SetMPVBase(atributosIniciais[nome][0]);
        unid.pt = atributosIniciais[nome][1];
        unid.SetMPTBase(atributosIniciais[nome][1]);
        unid.SetAtaqueBase(atributosIniciais[nome][2]);
        unid.SetDefesaBase(atributosIniciais[nome][3]);
        unid.SetAgilidadeBase(atributosIniciais[nome][4]);
        unid.SetMovimentoBase(atributosIniciais[nome][5]);
        unid.nivel = atributosIniciais[nome][6];
        unid.nivelBase = atributosIniciais[nome][6];
        unid.crescimento = crescimentoPorNivel[nome];
        if(itensIniciais.ContainsKey(nome)) {
            foreach (string item in itensIniciais[nome])
            {
                unid.AdicionarAoInventario(DefinesItens.itens[item]);
                //unid.inventario[i] = DefinesItens.itens[itensIniciais[nome][i]];
            }
        } else {
            unid.inventario = new Item[TAMANHO_INVENTARIO];
        }

        //Inimigos normalmente entram na segunda opção - as armas são o próprio nome
        if(armasIniciais.ContainsKey(nome)) {
            unid.arma = DefinesArmas.armas[armasIniciais[nome]];
            //também adicionar uma representação como item da arma inicial ao inventário
            unid.AdicionarAoInventario(DefinesItens.itens[unid.arma.nome]);
        } else {
            unid.arma = DefinesArmas.armas[nome];
        }
        
        if(habilidadesIniciais.ContainsKey(nome)) {
            foreach (string hab in habilidadesIniciais[nome])
            {
                unid.AdicionarHabilidade(DefinesHabilidades.habilidades[hab]);
            }
        } else {
            unid.inventario = new Item[TAMANHO_INVENTARIO];
        }
    }

}
