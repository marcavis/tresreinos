using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefinesArmas
{   
    //                                          
    // public static string[] herois = new string[]  { "Zheng Xiulan", 
    //                                                 "Miao Lin", 
    //                                                 "Tao Jiang", 
    //                                                 "Liu Jingsheng", 
    //                                                 "Jiang Xun", 
    //                                                 "Guan Long"};

    //até inimigos desarmados têm uma definição de arma pra poder atacar
    public static Dictionary<string, Arma> armas = new Dictionary<string, Arma> {
        //nome, quem pode equipar, poder, alcance, precisão, chance crítica, função de ataque, função de ataque crítico
        {"Espada", new Arma("Espada", "000001", 4, 1, 0, 0, Arma.efeitoAtaquePadrao, Arma.efeitoCriticoPadrao)},
        //ataca duas vezes normalmente, ou três em criticals
        {"Chu-Ko-Nu", new Arma("Chu-Ko-Nu", "000100", 1, 2, 0, 0, 
                    (dono, alvo) => {
                        alvo.ReceberAtaque(dono.ataque + dono.arma.poder, dono.arma);
                        alvo.ReceberAtaque(dono.ataque + dono.arma.poder, dono.arma);
                    }, 
                    (dono, alvo) => {
                        alvo.ReceberAtaque(dono.ataque + dono.arma.poder, dono.arma);
                        alvo.ReceberAtaque(dono.ataque + dono.arma.poder, dono.arma);
                        alvo.ReceberAtaque(dono.ataque + dono.arma.poder, dono.arma);
                    })},

        {"Jueyuan", new Arma("Jueyuan", "000000", 0, 1, 0, 0, Arma.efeitoAtaquePadrao,
                    (dono, alvo) => {
                        alvo.ReceberAtaque(dono.ataque + dono.arma.poder, dono.arma);
                        //causar efeito especial, como confusão no critical hit?
                    })}
    };
}
