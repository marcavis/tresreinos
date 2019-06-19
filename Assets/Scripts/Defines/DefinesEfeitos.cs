using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefinesEfeitos
{   
    //                                          
    // public static string[] herois = new string[]  { "Zheng Xiulan", 
    //                                                 "Miao Lin", 
    //                                                 "Tao Jiang", 
    //                                                 "Liu Jingsheng", 
    //                                                 "Jiang Xun", 
    //                                                 "Guan Long"};

    //até inimigos desarmados têm uma definição de arma pra poder atacar
    public static Dictionary<string, Efeito> efeitos = new Dictionary<string, Efeito> {
        //public Efeito(string nome, 
        //int deltaPV, int deltaPT, int deltaAtaque, int deltaDefesa, int deltaAgilidade, int deltaMovimento, 
        //int duracao, Func efeitoNoAtaque, 
        //Func efeitoNoDano, Func efeitoNoDanoMagico, Func efeitoNaDefesa)
        {"Maldição", new Efeito("Maldição",
                                0, 0, 0, 0, 0, 0, 
                                1, null,
                                //efeitoNoDano
                                (unid, x) => {
                                    return Mathf.FloorToInt(x * 2.25f);
                                }, null, null)},
        {"Sangramento", new Efeito("Sangramento",
                                -100, 0, 0, 0, 0, 0,
                                -1, null, null, null, null)}
    };
}
