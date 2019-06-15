using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefinesHabilidades
{   
    //                                          
    // public static string[] herois = new string[]  { "Zheng Xiulan", 
    //                                                 "Miao Lin", 
    //                                                 "Tao Jiang", 
    //                                                 "Liu Jingsheng", 
    //                                                 "Jiang Xun", 
    //                                                 "Guan Long"};

    //até inimigos desarmados têm uma definição de arma pra poder atacar
    public static Dictionary<string, Habilidade> habilidades = new Dictionary<string, Habilidade> {
        //nome, descrição, custo, alcanceMin, alcanceMax, área de efeito, precisão, chance crítica, função de uso, função de uso crítico
        {"Tiro de dispersão", new Habilidade("Tiro de dispersão", "Dispara setas contra um alvo e alvos adjacentes.", 3,
        1, 2, Habilidade.aoeCruz1, 0, 0, 
        (dono, alvo) => {
            
        }
        , null, false)},
        {"Cura", new Habilidade("Cura", "Cura 5 PV da unidade alvo.", 3, 1, 2, Habilidade.aoeCruz1, 0, 0, 
        (dono, alvo) => {
            alvo.ReceberCura(5);
        },
        (dono, alvo) => {
            alvo.ReceberCura(5);
        }, true)},
        {"Armageddon", new Habilidade("Armageddon", "CHEAT", 1, 1, 10, Habilidade.aoe0, 0, 0, 
        (dono, alvo) => {
            Personagem[] alvos = GameObject.Find("Gerenciador").GetComponent<GerenciadorScript>().personagens.ToArray();
            foreach (var p in alvos) {
                if(p.time == 1) {
                    p.ReceberDano(50, dono);
                }
            }
        }, 
        (dono, alvo) => {
            Personagem[] alvos = GameObject.Find("Gerenciador").GetComponent<GerenciadorScript>().personagens.ToArray();
            foreach (var p in alvos) {
                if(p.time == 1) {
                    p.ReceberDano(50, dono);
                }
            }
        }, false)}
    };
}
