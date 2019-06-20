using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

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
        //public Habilidade(string nome, string descricao, int custo, float variacao, int alcanceMin, int alcanceMax,
        //      List<Vector3> areaDeEfeito, int precisao,
        //      Action<Personagem, Personagem> efeitoAtaque,
        //      bool seMesmoTime)
        {"Tiro de dispersão", new Habilidade("Tiro de dispersão", "Dispara setas contra um alvo e alvos adjacentes.", 3, 20f,
        1, 2, Habilidade.aoeCruz1, "GUI/cursor1", 0, 
        (dono, alvo) => {
            
        }
        , false)},
        {"Cura", new Habilidade("Cura", "Cura, no mínimo, 5 PV da unidade alvo.", 3, 20f, 0, 2, Habilidade.aoeCruz1, "GUI/cursor1", 0,
        (dono, alvo) => {
            alvo.ReceberCura(5 + Mathf.RoundToInt(dono.nivel/4));
        }, true)},
        {"Armageddon", new Habilidade("Armageddon", "CHEAT", 1, 35f, 1, 10, Habilidade.aoeCruz3, "GUI/cursor3", 0,
        (dono, alvo) => {
            alvo.ReceberAtaqueHabilidade(50, dono);
        }, false)}
    };
}

