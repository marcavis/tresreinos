using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public class Efeito
{
    public int deltaPV;
    public int deltaPT;
    public int deltaAtaque; //remover?
    public int deltaDefesa; //remover?
    public int deltaAgilidade; //remover?
    public int deltaMovimento;
    public int duracao;
    public string nome;

    public bool inativo = false;

    public Personagem dono;
    public Personagem alvo;
    public Func<Personagem, int, int> efeitoNoAtaque;
    public Func<Personagem, int, int> efeitoNoDano;
    public Func<Personagem, int, int> efeitoNoDanoMagico;
    public Func<Personagem, int, int> efeitoNaDefesa;

    // public Arma(string nome, string podeEquipar, int poder, float variacao,
    //             int alcanceMin, int alcanceMax, int precisao, int chanceCritica,
    //             Action<Personagem, Personagem> efeitoAtaque,
    //             Action<Personagem, Personagem> efeitoCritico) {
    //     this.nome = nome;
    //     this.podeEquipar = podeEquipar;
    //     this.poder = poder;
    //     this.variacao = variacao;
    //     this.alcanceMin = alcanceMin;
    //     this.alcanceMax = alcanceMax;
    //     this.precisao = precisao;
    //     this.chanceCritica = chanceCritica;

    //     this.efeitoAtaque = efeitoAtaque;
    //     this.efeitoCritico = efeitoCritico;
    // }
    public Efeito(string nome, int deltaPV, int deltaPT, int deltaAtaque, int deltaDefesa, int deltaAgilidade, int deltaMovimento, int duracao, 
        Func<Personagem, int, int> efeitoNoAtaque, 
        Func<Personagem, int, int> efeitoNoDano, 
        Func<Personagem, int, int> efeitoNoDanoMagico, 
        Func<Personagem, int, int> efeitoNaDefesa)
    {
        this.deltaPV = deltaPV;
        this.deltaPT = deltaPT;
        this.deltaAtaque = deltaAtaque;
        this.deltaDefesa = deltaDefesa;
        this.deltaAgilidade = deltaAgilidade;
        this.deltaMovimento = deltaMovimento;
        this.duracao = duracao;
        this.nome = nome;
        this.efeitoNoAtaque = efeitoNoAtaque;
        this.efeitoNoDano = efeitoNoDano;
        this.efeitoNoDanoMagico = efeitoNoDanoMagico;
        this.efeitoNaDefesa = efeitoNaDefesa;
    }
}