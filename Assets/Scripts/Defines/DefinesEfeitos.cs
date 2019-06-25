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
        //int duracao, Func efeitoNoAtaque, 
        //Func efeitoNoDano, Func efeitoNoDanoMagico, Func efeitoNaDefesa, Func efeitoNaAgilidade, Func efeitoNoMovimento, Action efeitoFimTurno, Action efeitoExpirar)
        {"Maldição", new Efeito("Maldição",
                                3, null, null,
                                //efeitoNoDanoMagico
                                (unid, x) => {
                                    return Mathf.FloorToInt(x * 1.50f);
                                }, null, null, null, null, null)},
        {"Quebra Armadura", new Efeito("Quebra Armadura",
                                -1, null,
                                null, null, 
                                (unid, x) => {
                                    //efeitoNaDefesa
                                    return Mathf.FloorToInt(x * 0.85f);
                                }, null, null, null, null)},
        {"Ignora Terreno", new Efeito("Ignora Terreno",
                                3, null,
                                null, null, 
                                null, null, null, null, unid => unid.andar = Defines.Andar(Defines.tiposDeAndar[unid.nome]))},
    };
}
