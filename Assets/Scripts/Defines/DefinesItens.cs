using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefinesItens
{   
    //                                          
    // public static string[] herois = new string[]  { "Zheng Xiulan", 
    //                                                 "Miao Lin", 
    //                                                 "Tao Jiang", 
    //                                                 "Liu Jingsheng", 
    //                                                 "Jiang Xun", 
    //                                                 "Guan Long"};

    //até inimigos desarmados têm uma definição de arma pra poder atacar
    public static Dictionary<string, Item> itens = new Dictionary<string, Item> {
        //nome, quem pode equipar, poder, alcance, precisão, chance crítica, função de ataque, função de ataque crítico
        {"Chá Verde", new Item("Chá Verde", "Recupera pontos de vida", Arma.efeitoCriticoPadrao)},
        
    };
}
