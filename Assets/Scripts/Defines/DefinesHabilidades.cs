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
        1, 2, Habilidade.aoeCruz1, 0, 
        (dono, alvo) => {
            
        }
        , false)},
        {"Cura", new Habilidade("Cura", "Cura 5 PV da unidade alvo.", 3, 20f, 0, 2, Habilidade.aoeCruz1, 0,
        (dono, alvo) => {
            alvo.ReceberCura(5);
        }, true)},
        {"Armageddon", new Habilidade("Armageddon", "CHEAT", 1, 35f, 1, 10, Habilidade.aoe0, 0,
        (dono, alvo) => {
            alvo.ReceberAtaqueHabilidade(50, dono);
            // GerenciadorScript gs = GameObject.Find("Gerenciador").GetComponent<GerenciadorScript>();
            // AttackParent ap = GameObject.Find("Placeholder").GetComponent<AttackParent>();
            // Personagem[] alvos = gs.personagens.ToArray();
            // List<Action<GerenciadorDialogo>> list = new List<Action<GerenciadorDialogo>>();
            // foreach (var p in alvos) {
            //     if(p.time == 1) {
            //         list.Add(gd => {
            //             gd.IrPara(p);
            //         });
            //         list.Add(gd => {
            //             ap.Abrir();
            //             ap.SetLeftAnimator(Defines.animacoesAtk[dono.nome]);
            //             ap.SetRightAnimator(Defines.animacoesAtk[p.nome]);
            //         });
            //         list.Add(gd => {
            //             p.ReceberAtaqueHabilidade(50, dono);
            //         });
            //         list.Add(gd => {
            //             ap.Fechar();
            //         });
            //     }
            // }
            // gs.mensagensPendentes.Add(list);
        }, false)}
    };

    // public void Wrapper(Habilidade hab) {
    //     GerenciadorScript gs = GameObject.Find("Gerenciador").GetComponent<GerenciadorScript>();
    //     AttackParent ap = GameObject.Find("Placeholder").GetComponent<AttackParent>();
    //     List<Action<GerenciadorDialogo>> list = new List<Action<GerenciadorDialogo>>();
    //     list.Add(gd => {
    //                     gd.IrPara(p);
    //     });
    //     list.Add(gd => {
    //         ap.Abrir();
    //         ap.SetLeftAnimator(Defines.animacoesAtk[dono.nome]);
    //         ap.SetRightAnimator(Defines.animacoesAtk[p.nome]);
    //     });
    //     gs.mensagensPendentes.Add(list);
    // }
}

