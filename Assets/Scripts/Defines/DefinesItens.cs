using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefinesItens
{   
    const int ARMA = 1;
    const int CURA = 2;
    //                                          
    // public static string[] herois = new string[]  { "Zheng Xiulan", 
    //                                                 "Miao Lin", 
    //                                                 "Tao Jiang", 
    //                                                 "Liu Jingsheng", 
    //                                                 "Jiang Xun", 
    //                                                 "Guan Long"};

    //armas têm que ter sua definição aqui também, para serem usáveis no inventário
    public static Dictionary<string, Item> itens = new Dictionary<string, Item> {
        //nome, descrição, efeito quando usado
        //TODO: criar descrições para itens
        {"Chá Verde", new Item("Chá Verde", "Recupera pontos de vida", CURA, Item.efeitoNulo)},
        {"Bastão", new Item("Bastão", "pou", ARMA, Item.efeitoNulo)},
        {"Espadas-Borboleta", new Item("Espadas-Borboleta", "cha-ching", ARMA, Item.efeitoNulo)},
        {"Machado", new Item("Machado", "chop", ARMA, Item.efeitoNulo)},
        {"Chu-Ko-Nu", new Item("Chu-Ko-Nu", "pft pft", ARMA, Item.efeitoNulo)},
        {"Lança", new Item("Lança", "chuc", ARMA, Item.efeitoNulo)},
        {"Espada", new Item("Espada", "shwing", ARMA, Item.efeitoNulo)},
    };
}
