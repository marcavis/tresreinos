using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Item
{
    public string nome;
    public string descricao;

    public Action<Personagem, Personagem> efeitoUso;

    public static Action<Personagem, Personagem> efeitoNulo = (dono, alvo) => {};
    
    public Item(string nome, string descricao, 
                Action<Personagem, Personagem> efeitoUso){
        this.nome = nome;
        this.descricao = descricao;
        this.efeitoUso = efeitoUso;
    }

    public Item() {}
}
