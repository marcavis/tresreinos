using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public class Arma
{
    public string nome;
    public string podeEquipar; //string que diz quem pode equipar - se "111111" são todos, se "000001" só Guan Long pode equipar, etc.
    public int poder; //adicionado ao ataque
    public float variacao; //variação de 0.1f para mais ou menos faz com que o ataque varie entre 90 e 110% de dano, por exemplo
    public int alcance; //1 para armas "melee", mais para armas de longo alcance
    public int precisao; //usualmente 0
    public int chanceCritica;

    public Personagem dono;
    public Personagem alvo;
    public Action<Personagem, Personagem> efeitoAtaque;
    public Action<Personagem, Personagem> efeitoCritico;

    public static Action<Personagem, Personagem> efeitoAtaquePadrao = (dono, alvo) => alvo.ReceberAtaque(dono.ataque + dono.arma.poder, dono.arma);
    public static Action<Personagem, Personagem> efeitoCriticoPadrao = (dono, alvo) => 
        {
            alvo.ReceberAtaque(dono.ataque + dono.arma.poder, dono.arma);
            alvo.ReceberDano(dono.arma.poder, dono.arma);
        };

    public Arma(string nome, string podeEquipar, int poder, float variacao,
                int alcance, int precisao, int chanceCritica,
                Action<Personagem, Personagem> efeitoAtaque,
                Action<Personagem, Personagem> efeitoCritico){
        this.nome = nome;
        this.podeEquipar = podeEquipar;
        this.poder = poder;
        this.variacao = variacao;
        this.alcance = alcance;
        this.precisao = precisao;
        this.chanceCritica = chanceCritica;
        this.efeitoAtaque = efeitoAtaque;
        this.efeitoCritico = efeitoCritico;
    }

}