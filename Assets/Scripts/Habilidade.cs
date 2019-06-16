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
    public int precisao; //usualmente 0
    public int chanceCritica;


    public Personagem dono;
    public Personagem alvo;
    public Action<Personagem, Personagem> efeitoAtaque;
    public Action<Personagem, Personagem> efeitoCritico;
    public bool seMesmoTime;

    public Habilidade(string nome, string descricao, int custo, float variacao, int alcanceMin, int alcanceMax,
                List<Vector3> areaDeEfeito, int precisao, int chanceCritica,
                Action<Personagem, Personagem> efeitoAtaque,
                Action<Personagem, Personagem> efeitoCritico,
                bool seMesmoTime) {
        this.nome = nome;
        this.descricao = descricao;
        this.custo = custo;
        this.variacao = variacao;
        this.alcanceMin = alcanceMin;
        this.alcanceMax = alcanceMax;
        this.areaDeEfeito = areaDeEfeito;
        this.precisao = precisao;
        this.chanceCritica = chanceCritica;

        this.efeitoAtaque = efeitoAtaque;
        this.efeitoCritico = efeitoCritico;
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
}
