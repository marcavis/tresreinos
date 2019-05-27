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
        {"Espada", new Arma("Espada", "000001", 4, 20f, 1, 0, 0, Arma.efeitoAtaquePadrao, Arma.efeitoCriticoPadrao)},
        {"Lança", new Arma("Lança", "000010", 4, 30f, 1, 0, 0, Arma.efeitoAtaquePadrao, Arma.efeitoCriticoPadrao)},
        
        //ataca duas vezes normalmente, ou três em criticals
        {"Chu-Ko-Nu", new Arma("Chu-Ko-Nu", "000100", 1, 25f, 2, 0, 0, 
                    (dono, alvo) => {
                        alvo.ReceberAtaque(dono.ataque + dono.arma.poder, dono.arma);
                        alvo.ReceberAtaque(dono.ataque + dono.arma.poder, dono.arma);
                    }, 
                    (dono, alvo) => {
                        alvo.ReceberAtaque(dono.ataque + dono.arma.poder, dono.arma);
                        alvo.ReceberAtaque(dono.ataque + dono.arma.poder, dono.arma);
                        alvo.ReceberAtaque(dono.ataque + dono.arma.poder, dono.arma);
                    })},
        
        //talvez dar um efeito crítico de redução de defesa?
        {"Machado", new Arma("Machado", "001000", 5, 35f, 1, 0, 0, Arma.efeitoAtaquePadrao, Arma.efeitoCriticoPadrao)},
        {"Espadas-Borboleta", new Arma("Espadas-Borboleta", "010000", 3, 15f, 1, 0, 0, Arma.efeitoAtaquePadrao,
                    (dono, alvo) => {
                        alvo.ReceberAtaque(dono.ataque + dono.arma.poder, dono.arma);
                        alvo.ReceberAtaque(dono.ataque + dono.arma.poder, dono.arma);
                    })},

        {"Bastão", new Arma("Bastão", "100000", 2, 20f, 1, 0, 0, Arma.efeitoAtaquePadrao,
                    (dono, alvo) => {
                        alvo.ReceberAtaque(dono.ataque + dono.arma.poder, dono.arma);
                        //talvez fazer tropeçar e perder progresso até a próxima rodada?
                        alvo.iniciativa -= 400;
                    })},
        {"Jueyuan", new Arma("Jueyuan", "000000", 0, 20f, 1, 0, 0, Arma.efeitoAtaquePadrao,
                    (dono, alvo) => {
                        alvo.ReceberAtaque(dono.ataque + dono.arma.poder, dono.arma);
                        //causar efeito especial, como confusão no critical hit?
                    })}
    };
}
