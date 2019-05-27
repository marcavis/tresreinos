using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Defines 
{   
    //                                          
    public static string[] herois = new string[]  { "Zheng Xiulan", 
                                                    "Miao Lin", 
                                                    "Tao Jiang", 
                                                    "Liu Jingsheng", 
                                                    "Jiang Xun", 
                                                    "Guan Long"};
    const int ANDAR_NORMAL = 0;
    const int ANDAR_TRILHA = 1;
    const int ANDAR_RUSTICO = 2;
    static Dictionary<string, int> tiposDeAndar = new Dictionary<string, int>() {
        {herois[0], ANDAR_NORMAL},
        {herois[1], ANDAR_TRILHA},
        {herois[2], ANDAR_RUSTICO},
        {herois[3], ANDAR_NORMAL},
        {herois[4], ANDAR_NORMAL},
        {herois[5], ANDAR_NORMAL},

        {"Jueyuan", ANDAR_TRILHA},
    };
    public static Dictionary<string, int> Andar(int tipo) {
        
        switch (tipo)
        {
            case ANDAR_TRILHA:
                return new Dictionary<string, int>() {
                    {"grass", 10},
                    {"carpet", 10}
                };
            default:
                //unidades com tipo de andar "normal" se encaixarão aqui
                return new Dictionary<string, int>() {
                    {"grass", 15},
                    {"carpet", 10}
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
        {herois[3], new int[] {20,  6, 12, 10, 15, 50, 1}},
        {herois[4], new int[] {24,  7, 14, 12, 22, 60, 1}},
        {herois[5], new int[] {24,  8, 15, 14, 17, 50, 1}},

        {"Jueyuan", new int[] {15,  0, 18, 10, 22, 60, 2}},
    };
    public static Dictionary<string, string> armasIniciais = new Dictionary<string, string>() {
        
        {herois[0], "Bastão"},
        {herois[1], "Espadas-Borboleta"},
        {herois[2], "Machado"},
        {herois[3], "Chu-Ko-Nu"},
        {herois[4], "Lança"},
        {herois[5], "Espada"},

        //{"Jueyuan", new int[] {15,  0, 18, 10, 22, 60, 2}},
    };

    public static void Inicializacao(string nome, GameObject objeto) {
        //mudar tudo isso se forem carregados dados na segunda fase - usar PlayerPrefs?
        Personagem unid = objeto.GetComponent<Personagem>();
        unid.andar = Andar(tiposDeAndar[nome]);
        unid.pv = atributosIniciais[nome][0];
        unid.mpv = atributosIniciais[nome][0];
        unid.pt = atributosIniciais[nome][1];
        unid.mpt = atributosIniciais[nome][1];
        unid.ataque = atributosIniciais[nome][2];
        unid.defesa = atributosIniciais[nome][3];
        unid.agilidade = atributosIniciais[nome][4];
        unid.movimento = atributosIniciais[nome][5];
        unid.nivel = atributosIniciais[nome][6];
        //Inimigos normalmente entram na segunda opção - as armas são o próprio nome
        if(armasIniciais.ContainsKey(nome)) {
            unid.arma = DefinesArmas.armas[armasIniciais[nome]];
        } else {
            unid.arma = DefinesArmas.armas[nome];
        }
        
    }
}
