using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Habilidade
{
    public string nome;//variação de 0.1f para mais ou menos faz com que o ataque varie entre 90 e 110% de dano, por exemplo
    public string descricao;
    public int alcance; //1 para armas "melee", mais para armas de longo alcance
    public List<Vector3> areaDeEfeito;
    public int precisao; //usualmente 0
    public int chanceCritica;


    public Personagem dono;
    public Personagem alvo;
    public Action<Personagem, Personagem> efeitoAtaque;
    public Action<Personagem, Personagem> efeitoCritico;

    public Habilidade(string nome, string descricao, int alcance, List<Vector3> areaDeEfeito, int precisao, int chanceCritica,
                Action<Personagem, Personagem> efeitoAtaque,
                Action<Personagem, Personagem> efeitoCritico) {
        this.nome = nome;
        this.descricao = descricao;
        this.alcance = alcance;
        this.areaDeEfeito = areaDeEfeito;
        this.precisao = precisao;
        this.chanceCritica = chanceCritica;

        this.efeitoAtaque = efeitoAtaque;
        this.efeitoCritico = efeitoCritico;
    }
}
