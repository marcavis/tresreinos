using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public class Efeito
{
    public int duracao;
    public string nome;

    public bool inativo = false;

    public Personagem dono;
    public Personagem alvo;
    public Func<Personagem, int, int> efeitoNoAtaque;
    public Func<Personagem, int, int> efeitoNoDano;
    public Func<Personagem, int, int> efeitoNoDanoMagico;
    public Func<Personagem, int, int> efeitoNaDefesa;
    public Func<Personagem, int, int> efeitoNaAgilidade;
    public Func<Personagem, int, int> efeitoNoMovimento;
    public Action<Personagem> efeitoFimTurno;

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
    public Efeito(string nome, int duracao, 
        Func<Personagem, int, int> efeitoNoAtaque, 
        Func<Personagem, int, int> efeitoNoDano, 
        Func<Personagem, int, int> efeitoNoDanoMagico, 
        Func<Personagem, int, int> efeitoNaDefesa,
        Func<Personagem, int, int> efeitoNaAgilidade,
        Func<Personagem, int, int> efeitoNoMovimento,
        Action<Personagem> efeitoFimTurno)
    {
        this.duracao = duracao;
        this.nome = nome;
        this.efeitoNoAtaque = efeitoNoAtaque;
        this.efeitoNoDano = efeitoNoDano;
        this.efeitoNoDanoMagico = efeitoNoDanoMagico;
        this.efeitoNaDefesa = efeitoNaDefesa;
        this.efeitoNaAgilidade = efeitoNaAgilidade;
        this.efeitoNoMovimento = efeitoNoMovimento;
        this.efeitoFimTurno = efeitoFimTurno;
    }
}