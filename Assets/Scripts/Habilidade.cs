using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Habilidade
{
    public string nome;//variação de 0.1f para mais ou menos faz com que o ataque varie entre 90 e 110% de dano, por exemplo
    public string descricao;
    public int custo; //custo em PT para usar a habilidade
    public float variacao;
    public int alcanceMin, alcanceMax; 
    public List<Vector3> areaDeEfeito;
    public string cursorSprite; //deve ser sempre o mesmo formato da areaDeEfeito


    public Personagem dono;
    public Personagem alvo;
    public Action<Personagem, Personagem> efeitoUso;
    public bool seMesmoTime;

    public Habilidade(string nome, string descricao, int custo, float variacao, int alcanceMin, int alcanceMax,
                List<Vector3> areaDeEfeito, string cursorSprite,
                Action<Personagem, Personagem> efeitoUso,
                bool seMesmoTime) {
        this.nome = nome;
        this.descricao = descricao;
        this.custo = custo;
        this.variacao = variacao;
        this.alcanceMin = alcanceMin;
        this.alcanceMax = alcanceMax;
        this.areaDeEfeito = areaDeEfeito;
        this.cursorSprite = cursorSprite;

        this.efeitoUso = efeitoUso;
        this.seMesmoTime = seMesmoTime;
    }

    public static List<Vector3> aoe0 = new List<Vector3> {new Vector3(0, 0, 0)};
    
    public static List<Vector3> aoeCruz1 = new List<Vector3> {
        //formato de '+'
        new Vector3(-1, 0, 0),
        new Vector3(0, -1, 0),
        new Vector3(0, 0, 0),
        new Vector3(1, 0, 0),
        new Vector3(0, 1, 0)
    };

    public static List<Vector3> aoeCruz2 = gerarAOE(2);
    public static List<Vector3> aoeCruz3 = gerarAOE(3);
    public static List<Vector3> aoeCruz7 = gerarAOE(7);

    public static List<Vector3> gerarAOE(int tamanho) {
        List<Vector3> result = new List<Vector3>();
        for(int i = -tamanho; i <= tamanho; i++) {
            for(int j = -tamanho; j <= tamanho; j++) {
                if(Mathf.Abs(i) + Mathf.Abs(j) <= tamanho) {
                    result.Add(new Vector3(i, j, 0));
                }
            }
        }
        return result;
    }
}
