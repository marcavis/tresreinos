using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public class DefinesDialogos
{   
    public static string[] objHerois = new string[]  {"ZhengXiulan", 
                                                    "MiaoLin", 
                                                    "TaoJiang", 
                                                    "LiuJingsheng", 
                                                    "JiangXun", 
                                                    "GuanLong"};                               

    //se mover uma unidade, é bom mover o cursor também para a nova posição da unidade, depois de completa a movimentação
    public static Dictionary<string, List<Action<GerenciadorDialogo>>> dialogos = new Dictionary<string, List<Action<GerenciadorDialogo>>> {
        {"Levelup", new List<Action<GerenciadorDialogo>> {
            gd => {gd.Dialogo("Subida de nível!", "{} agora está mais forte!");},
        }},
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
