using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Atributo 
{
    public string nome;
    public int atual;
    public int maximo;
    
    public Atributo(string nome, int atual, int maximo) {
        this.nome = nome;
        this.atual = atual;
        this.maximo = maximo;
    }
}
