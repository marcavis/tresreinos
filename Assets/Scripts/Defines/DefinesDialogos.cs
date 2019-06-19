using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public class DefinesDialogos
{   
    public static Personagem[] objHerois = new Personagem[]  {GameObject.Find("ZhengXiulan").GetComponent<Personagem>(), 
                                                    GameObject.Find("MiaoLin").GetComponent<Personagem>(), 
                                                    GameObject.Find("TaoJiang").GetComponent<Personagem>(), 
                                                    GameObject.Find("LiuJingsheng").GetComponent<Personagem>(), 
                                                    GameObject.Find("JiangXun").GetComponent<Personagem>(), 
                                                    GameObject.Find("GuanLong").GetComponent<Personagem>()};                               

    //se mover uma unidade, é bom mover o cursor também para a nova posição da unidade, depois de completa a movimentação
    public static Dictionary<string, List<Action<GerenciadorDialogo>>> dialogos = new Dictionary<string, List<Action<GerenciadorDialogo>>> {
        {"Batalha1_inic", new List<Action<GerenciadorDialogo>> {
            gd => {gd.IrPara(objHerois[0]);},
            gd => {gd.Dialogo(objHerois[0], "O que são esses vultos?");},
            gd => {gd.Mover(objHerois[0], new Vector3(-1, 3, 0));},
            gd => {gd.IrPara(objHerois[0]);},
            gd => {gd.IrPara(objHerois[5]);},
            gd => {gd.Dialogo(objHerois[5], "Zheng Xiulan, volte para cá! Eu estou com uma má impressão...");},
        }},
        {"Batalha1_fim", new List<Action<GerenciadorDialogo>> {
            gd => {gd.IrPara(new Vector3(0, 0, 0));}
        }}

    };
}
