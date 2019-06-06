using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GerenciadorTelaHab : MonoBehaviour
{
    public GameObject cursor;
    public GameObject canvasAlvo;
    public GerenciadorInput gerenciadorInput;
    public int entrada;
    public Vector3 direcao;

    private int posHabSelecionada;

    //private Item itemSelecionado;
    private Text[] slots;
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
        slots = new Text[8];
        for (int i = 0; i < 8; i++)
        {
            slots[i] = GameObject.Find("Slot" + (i+1) + "Hab").GetComponent<Text>();
        }
        campoDesc = GameObject.Find("DescricaoHab").GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        if(entrada == Teclas.CANCEL) {
            entrada = 0;
            VoltarAoMenuDeBatalha();
        }
    }

    public void AbrirMenu(Personagem unid) {
        this.unid = unid;
        posHabSelecionada = 0;
        //itemSelecionado = null;
        gameObject.GetComponent<Canvas>().enabled = true;
        //TODO:Continuar
        //MostrarHabilidades();
        
    }

    public void VoltarAoMenuDeBatalha() {
        gerenciadorInput.cursorAtivo = 1;
        gameObject.GetComponent<Canvas>().enabled = false;
    }


}
