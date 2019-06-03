using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GerenciadorInventario : MonoBehaviour
{
    public GerenciadorInput gerenciadorInput;
    public int estado;
    const int SELECAO_ITEM = 0;
    const int SELECAO_ACAO = 1;
    public int entrada;
    public Vector3 direcao;

    private int posItemSelecionado;

    private Text[] slots;
    private Text campoDesc;
    //a unidade dona do inventário acessado
    public Personagem unid;
    // Start is called before the first frame update
    void Start()
    {
        gerenciadorInput = GameObject.Find("Input").GetComponent<GerenciadorInput>();
        slots = new Text[8];
        for (int i = 0; i < 8; i++)
        {
            slots[i] = GameObject.Find("Slot" + (i+1)).GetComponent<Text>();
        }
        campoDesc = GameObject.Find("Descricao").GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        if(entrada == Teclas.CANCEL) {
            entrada = 0;
            if(estado == SELECAO_ITEM) {
                FecharMenu();
            }
        } else if(entrada == Teclas.DPAD) {
            entrada = 0;
            posItemSelecionado = posItemSelecionado - (int) direcao.y;
            posItemSelecionado = (slots.Length + posItemSelecionado) % slots.Length;
            foreach (Text t in slots)
            {
                LimparSelecaoNome(t);
            }
            slots[posItemSelecionado].text = "> " + slots[posItemSelecionado].text + " <";
            Item itemSelecionado = unid.inventario[posItemSelecionado];
            campoDesc.text = itemSelecionado != null ? itemSelecionado.descricao : "-";
        }
    }
    
    public void LimparSelecaoNome(Text t) {
        t.text = t.text.Replace("> ", "");
        t.text = t.text.Replace(" <", "");
    }

    void MostrarItens() {
        for (int i = 0; i < 8; i++)
        {
            if(unid.inventario[i] != null) {
                slots[i].text = unid.inventario[i].nome;
            } else {
                slots[i].text = " - ";
            }
        }
        slots[posItemSelecionado].text = "> " + slots[posItemSelecionado].text + " <";
    }

    public void AbrirMenu(Personagem unid) {
        this.unid = unid;
        posItemSelecionado = 0;
        gameObject.GetComponent<Canvas>().enabled = true;
        MostrarItens();
    }

    public void FecharMenu() {
        gerenciadorInput.cursorAtivo = 1;
        gameObject.GetComponent<Canvas>().enabled = false;
    }
}
