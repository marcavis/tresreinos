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
        //nome, descrição, custo, variacao, alcanceMin, alcanceMax, área de efeito, precisão, chance crítica, função de uso, função de uso crítico
        {"Tiro de dispersão", new Habilidade("Tiro de dispersão", "Dispara setas contra um alvo e alvos adjacentes.", 3, 20f,
        1, 2, Habilidade.aoeCruz1, 0, 0, 
        (dono, alvo) => {
            
        }
        , null, false)},
        {"Cura", new Habilidade("Cura", "Cura 5 PV da unidade alvo.", 3, 20f, 0, 2, Habilidade.aoeCruz1, 0, 0, 
        (dono, alvo) => {
            alvo.ReceberCura(5);
        },
        (dono, alvo) => {
            alvo.ReceberCura(5);
        }, true)},
        {"Armageddon", new Habilidade("Armageddon", "CHEAT", 1, 35f, 1, 10, Habilidade.aoe0, 0, 0, 
        (dono, alvo) => {
            GerenciadorScript gs = GameObject.Find("Gerenciador").GetComponent<GerenciadorScript>();
            AttackParent ap = GameObject.Find("Placeholder").GetComponent<AttackParent>();
            Personagem[] alvos = gs.personagens.ToArray();
            List<Action<GerenciadorDialogo>> list = new List<Action<GerenciadorDialogo>>();
            foreach (var p in alvos) {
                if(p.time == 1) {
                    list.Add(gd => {
                        gd.IrPara(p);
                    });
                    list.Add(gd => {
                        ap.Abrir();
                        ap.SetLeftAnimator(Defines.animacoesAtk[dono.nome]);
                        ap.SetRightAnimator(Defines.animacoesAtk[p.nome]);
                    });
                    list.Add(gd => {
                        p.ReceberAtaqueHabilidade(50, dono);
                    });
                    list.Add(gd => {
                        ap.Fechar();
                    });
                }
            }
            gs.mensagensPendentes.Add(list);
        }, 
        (dono, alvo) => {
            GerenciadorScript gs = GameObject.Find("Gerenciador").GetComponent<GerenciadorScript>();
            AttackParent ap = GameObject.Find("Placeholder").GetComponent<AttackParent>();
            Personagem[] alvos = gs.personagens.ToArray();
            List<Action<GerenciadorDialogo>> list = new List<Action<GerenciadorDialogo>>();
            foreach (var p in alvos) {
                if(p.time == 1) {
                    list.Add(gd => {
                        gd.IrPara(p);
                    });
                    list.Add(gd => {
                        ap.Abrir();
                        ap.SetLeftAnimator(Defines.animacoesAtk[dono.nome]);
                        ap.SetRightAnimator(Defines.animacoesAtk[p.nome]);
                    });
                    list.Add(gd => {
                        p.ReceberAtaqueHabilidade(50, dono);
                    });
                    list.Add(gd => {
                        ap.Fechar();
                    });
                }
            }
            gs.mensagensPendentes.Add(list);
        }, false)}
    };
}
