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