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
        //nome, quem pode equipar, poder, variação, alcanceMin, alcanceMax, precisão, chance crítica, função de uso, função de ataque, função de ataque crítico
        {"Punho", new Arma("Punho", "111111", 0, 30f, 1, 1, 0, 0, Arma.efeitoAtaquePadrao, Arma.efeitoCriticoPadrao)},
        {"Espada", new Arma("Espada", "000001", 4, 20f, 1, 1, 0, 0, Arma.efeitoAtaquePadrao, Arma.efeitoCriticoPadrao)},
        {"Lança", new Arma("Lança", "000010", 4, 30f, 1, 1, 0, 0, Arma.efeitoAtaquePadrao, Arma.efeitoCriticoPadrao)},
        
        //ataca duas vezes normalmente, ou três em criticals
        {"Chu-Ko-Nu", new Arma("Chu-Ko-Nu", "000100", 1, 25f, 1, 2, 0, 0, 
                    (dono, alvo) => {
                        alvo.ReceberAtaque(dono.Ataque() + dono.arma.poder, dono);
                        alvo.ReceberAtaque(dono.Ataque() + dono.arma.poder, dono);
                    }, 
                    (dono, alvo) => {
                        alvo.ReceberAtaque(dono.Ataque() + dono.arma.poder, dono);
                        alvo.ReceberAtaque(dono.Ataque() + dono.arma.poder, dono);
                        alvo.ReceberAtaque(dono.Ataque() + dono.arma.poder, dono);
                    })},
        
        //talvez dar um efeito crítico de redução de defesa?
        {"Machado", new Arma("Machado", "001000", 5, 35f, 1, 1, 0, 0, Arma.efeitoAtaquePadrao, Arma.efeitoCriticoPadrao)},
        {"Espadas-Borboleta", new Arma("Espadas-Borboleta", "010000", 3, 15f, 1, 1, 0, 0, Arma.efeitoAtaquePadrao,
                    (dono, alvo) => {
                        alvo.ReceberAtaque(dono.Ataque() + dono.arma.poder, dono);
                        alvo.ReceberAtaque(dono.Ataque() + dono.arma.poder, dono);
                    })},

        {"Bastão", new Arma("Bastão", "100000", 2, 20f, 1, 1, 0, 0, Arma.efeitoAtaquePadrao,
                    (dono, alvo) => {
                        alvo.ReceberAtaque(dono.Ataque() + dono.arma.poder, dono);
                        //talvez fazer tropeçar e perder progresso até a próxima rodada?
                        alvo.iniciativa -= 400;
                    })},
        {"Jueyuan", new Arma("Jueyuan", "000000", 0, 20f, 2, 3, 0, 0, Arma.efeitoAtaquePadrao,
                    (dono, alvo) => {
                        alvo.ReceberAtaque(dono.Ataque() + dono.arma.poder, dono);
                        //causar efeito especial, como confusão no critical hit?
                    })}

    };
}
