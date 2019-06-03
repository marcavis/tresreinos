using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Item
{
    public string nome;
    public string descricao;
    public int tipo;


    public Action<Personagem, Personagem> efeitoUso;

    public static Action<Personagem, Personagem> efeitoNulo = (dono, alvo) => {};
    
    public Item(string nome, string descricao, int tipo,
                Action<Personagem, Personagem> efeitoUso){
        this.nome = nome;
        this.descricao = descricao;
        this.tipo = tipo;
        this.efeitoUso = efeitoUso;
    }

    public Item() {}
}
