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
    //nessa ordem = {mpv, mpt, ataque, defesa, agilidade, movimento}
    public static Dictionary<string, int[]> atributosIniciais = new Dictionary<string, int[]>() {
        
        {herois[0], new int[] {18, 12, 12, 11, 19, 50}},
        {herois[1], new int[] {21,  6, 14, 12, 20, 50}},
        {herois[2], new int[] {25,  4, 15, 14, 16, 50}},
        {herois[3], new int[] {20,  6, 12, 10, 15, 50}},
        {herois[4], new int[] {24,  7, 14, 12, 22, 60}},
        {herois[5], new int[] {24,  8, 15, 14, 17, 50}},

        {"Jueyuan", new int[] {15,  0, 18, 10, 22, 60}},
    };

    public static void Inicializacao(string nome, GameObject objeto) {
        //mudar tudo isso se forem carregados dados na segunda fase - usar PlayerPrefs?
        objeto.GetComponent<Personagem>().andar = Andar(tiposDeAndar[nome]);
        objeto.GetComponent<Personagem>().pv = atributosIniciais[nome][0];
        objeto.GetComponent<Personagem>().mpv = atributosIniciais[nome][0];
        objeto.GetComponent<Personagem>().pt = atributosIniciais[nome][1];
        objeto.GetComponent<Personagem>().mpt = atributosIniciais[nome][1];
        objeto.GetComponent<Personagem>().ataque = atributosIniciais[nome][2];
        objeto.GetComponent<Personagem>().defesa = atributosIniciais[nome][3];
        objeto.GetComponent<Personagem>().agilidade = atributosIniciais[nome][4];
        objeto.GetComponent<Personagem>().movimento = atributosIniciais[nome][5];
    }
}
