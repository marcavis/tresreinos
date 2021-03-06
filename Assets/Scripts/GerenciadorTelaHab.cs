﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GerenciadorTelaHab : MonoBehaviour
{
    public GameObject cursor;
    public GameObject canvasAlvo;
    public GerenciadorInput gerenciadorInput;
    private GerenciadorScript gs;
    public int entrada;
    public Vector3 direcao;

    private int posHabSelecionada;

    //private Item itemSelecionado;
    private Text[] slots;
    private Text custo;
    private Text alcance;
    private Text campoDesc;

    private Text[] acoes;
    //a unidade dona do inventário acessado
    public Personagem unid;
    // Start is called before the first frame update
    void Start()
    {
        cursor = GameObject.Find("Cursor");
        canvasAlvo = GameObject.Find("CanvasAlvo");
        gerenciadorInput = GameObject.Find("Input").GetComponent<GerenciadorInput>();
        gs = GameObject.Find("Gerenciador").GetComponent<GerenciadorScript>();
        slots = new Text[8];
        for (int i = 0; i < 8; i++)
        {
            slots[i] = GameObject.Find("Slot" + (i+1) + "Hab").GetComponent<Text>();
        }
        campoDesc = GameObject.Find("DescricaoHab").GetComponent<Text>();
        custo = GameObject.Find("CustoLabel").GetComponent<Text>();
        alcance = GameObject.Find("AlcanceLabel").GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        if(entrada == Teclas.CANCEL) {
            entrada = 0;
            gs.PlaySoundMenuCancel();
            VoltarAoMenuDeBatalha();
        }
        else if(entrada == Teclas.ACTION) {
            entrada = 0;
            bool somErro = false;
            Habilidade aEscolher = unid.habilidades[posHabSelecionada];    
            
            if(aEscolher == null || unid.pt < aEscolher.custo || !unid.ExistemPersonagensAlvos(aEscolher.alcanceMin, aEscolher.alcanceMax, aEscolher.seMesmoTime)) {
                somErro = true;
                //print("não pode usar habilidade");
            } else {
                cursor.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>(aEscolher.cursorSprite);
                unid.habilidadeAtual = aEscolher;
                gerenciadorInput.cursorAtivo = 0;
                canvasAlvo.GetComponent<Canvas>().enabled = true;
                //o jogo ficará circulando entre os alvos permitidos, então começaremos
                //movendo para o primeiro alvo encontrado (viés para o canto inferior esquerdo)
                cursor.GetComponent<ControleCursor>().IrParaPrimeiroAlvoHabilidade(unid.habilidadeAtual.seMesmoTime);
                cursor.GetComponent<ControleCursor>().MostrarOverlaysHabilidades(unid.habilidadeAtual.seMesmoTime);
                gameObject.GetComponent<Canvas>().enabled = false;
            }
            gs.PlaySoundMenuSelect(somErro);
        }
        else if(entrada == Teclas.DPAD) {
            entrada = 0;
            gs.PlaySoundMenuChange();
            posHabSelecionada = posHabSelecionada - (int) direcao.y;
            posHabSelecionada = (slots.Length + posHabSelecionada) % slots.Length;
            MostrarHabilidades();
        }
    }

    public void MostrarHabilidades() {
        for (int i = 0; i < 8; i++)
        {
            if(unid.habilidades[i] != null) {
                slots[i].text = unid.habilidades[i].nome;
            } else {
                slots[i].text = " - ";
            }
        }
        slots[posHabSelecionada].text = "> " + slots[posHabSelecionada].text + " <";
        Habilidade atual = unid.habilidades[posHabSelecionada];
        campoDesc.text = atual != null ? atual.descricao : "-";
        custo.text = atual != null ? "Custo: " + (unid.pt >= atual.custo ?
                                     "<color=#ffffffff>"+atual.custo + "</color>" : "<color=#ff0000ff>" + atual.custo + "</color>") + " PT" : "-";
        alcance.text = atual != null ? "Alcance: " + atual.alcanceMin + " a " + atual.alcanceMax + " células": "-";
    }

    public void AbrirMenu(Personagem unid) {
        this.unid = unid;
        posHabSelecionada = 0;
        //itemSelecionado = null;
        gameObject.GetComponent<Canvas>().enabled = true;
        //TODO:Continuar
        MostrarHabilidades();
    }

    public void Reabrir() {
        gameObject.GetComponent<Canvas>().enabled = true;
    }
    public void VoltarAoMenuDeBatalha() {
        gerenciadorInput.cursorAtivo = 1;
        gameObject.GetComponent<Canvas>().enabled = false;
    }


}
