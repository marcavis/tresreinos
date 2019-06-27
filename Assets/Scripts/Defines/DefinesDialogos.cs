using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public class DefinesDialogos
{   
    public static string[] herois = new string[]  { "Zheng Xiulan", 
                                                    "Miao Lin", 
                                                    "Tao Jiang", 
                                                    "Liu Jingsheng", 
                                                    "Jiang Xun", 
                                                    "Guan Long"};                

    //se mover uma unidade, é bom mover o cursor também para a nova posição da unidade, depois de completa a movimentação
    public Dictionary<string, List<Action<GerenciadorDialogo>>> dialogos = new Dictionary<string, List<Action<GerenciadorDialogo>>> {
        {"Batalha1_inic", new List<Action<GerenciadorDialogo>> {
            gd => {gd.IrPara(herois[0]);},
            gd => {gd.Dialogo(herois[0], "Ficou escuro tão de repente...");},
            gd => {gd.IrPara(herois[1]);},
            gd => {gd.Dialogo(herois[1], "Bom, já está na hora de irmos embora mesmo. Vamos levar estes artefatos para a universidade.");},
            gd => {gd.IrPara(herois[0]);},
            gd => {gd.Dialogo(herois[0], "O que são esses vultos?");},
            gd => {gd.Mover(herois[0], new Vector3(-1, 3, 0));},
            gd => {gd.IrPara(herois[0]);},
            gd => {gd.Dialogo(herois[0], "Ah... Ah, não...");},
            gd => {gd.IrPara(herois[5]);},
            gd => {gd.Dialogo(herois[5], "Zheng Xiulan, volte para cá! Eu estou com uma má impressão...");},
            gd => {gd.IrPara(herois[2]);},
            gd => {gd.Dialogo(herois[2], "Estão vindo. Preparem as armas!");},
        }},
        {"Batalha1_fim", new List<Action<GerenciadorDialogo>> {
            gd => {gd.IrPara(herois[5]);},
            gd => {gd.Dialogo(herois[5], "Vamos sair da caverna... e tentar achar uma explicação para esses monstros.");},
        }},
        {"Batalha2_inic", new List<Action<GerenciadorDialogo>> {
            gd => {gd.IrPara(herois[5]);},
            gd => {gd.IrPara(herois[5]);},
            gd => {gd.IrPara(herois[5]);},
            gd => {gd.Mover(herois[5], new Vector3(0, -3, 0));},
            gd => {gd.IrPara(herois[5]);},
            gd => {gd.Dialogo(herois[5], "É, como eu esperava... O ônibus já se foi.");},
            gd => {gd.Dialogo(herois[5], "Pior ainda, nem reconheço mais o caminho!");},
            gd => {gd.IrPara(GameObject.Find("CapOgro (2)").GetComponent<Personagem>());},
            gd => {gd.Mover(GameObject.Find("CapOgro (2)").GetComponent<Personagem>(), new Vector3(2, 0, 0));},
            gd => {gd.IrPara(GameObject.Find("CapOgro (2)").GetComponent<Personagem>());},
            gd => {gd.IrPara(herois[1]);},
            gd => {gd.Mover(herois[1], new Vector3(-1, -5, 0));},
            gd => {gd.IrPara(herois[1]);},
            gd => {gd.Dialogo(herois[1], "E esse não é o maior entre os nossos problemas...");},
            gd => {gd.Mover(herois[1], new Vector3(0, 3, 0));},
            gd => {gd.IrPara(herois[1]);},
            gd => {gd.IrPara(herois[5]);},
            gd => {gd.Mover(herois[5], new Vector3(0, 2, 0));},
            gd => {gd.IrPara(herois[5]);},
        }},
        {"Batalha2_fim", new List<Action<GerenciadorDialogo>> {
            gd => {gd.Dialogo("Fim de Jogo","Obrigado por jogar o nosso protótipo de Relíquias Dos Três Reinos!");}
        }},

    };
}
