using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Defines 
{   
    //                                           0              1          2            3                  4           5
    static string[] herois = new string[] {"Zheng Xiulan", "Miao Lin", "Tao Jiang", "Liu Jingsheng", "Jiang Xun", "Guan Long"};
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
                };
        }
    }

    public static void Inicializacao(string nome, GameObject objeto) {
        objeto.GetComponent<Personagem>().andar = Andar(tiposDeAndar[nome]);

    }
}
