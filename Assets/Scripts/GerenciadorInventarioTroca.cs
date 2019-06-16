using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GerenciadorInventarioTroca : MonoBehaviour
{
   public GameObject cursor;
    public GameObject canvasAlvo;
    public GerenciadorInput gerenciadorInput;
    private GerenciadorScript gs;
    public int entrada;
    public Vector3 direcao;

    //posição do item da unidade de origem, para excluir se a troca for completada
    private int posItemOferecido;
    private int posItemSelecionado;
    private Text[] slots;

    //origem é a unidade que iniciou a troca, destino é aquela com inventário cheio
    public Personagem origem, destino;
    // Start is called before the first frame update
    void Start()
    {
        cursor = GameObject.Find("Cursor");
        canvasAlvo = GameObject.Find("CanvasAlvo");
        gerenciadorInput = GameObject.Find("Input").GetComponent<GerenciadorInput>();
        slots = new Text[8];
        for (int i = 0; i < 8; i++)
        {
            slots[i] = GameObject.Find("Slot" + (i+1) + "Troca").GetComponent<Text>();
        }
        gs = GameObject.Find("Gerenciador").GetComponent<GerenciadorScript>();
    }

    // Update is called once per frame
    void Update()
    {
        if(entrada == Teclas.CANCEL) {
            entrada = 0;
            gerenciadorInput.cursorAtivo = 0;
            canvasAlvo.GetComponent<Canvas>().enabled = true;
            //o jogo ficará circulando entre os alvos permitidos, então começaremos
            //movendo para o primeiro alvo encontrado (viés para o canto inferior esquerdo)
            cursor.GetComponent<ControleCursor>().IrParaPrimeiroAlvoTroca();
            cursor.GetComponent<ControleCursor>().MostrarOverlaysTroca();
            //cursor.GetComponent<ControleCursor>().indiceItemSelecionado = posItemSelecionado;
            gameObject.GetComponent<Canvas>().enabled = false;
        } 
        else if(entrada == Teclas.ACTION) {
            entrada = 0;
            Item itemADevolver = destino.inventario[posItemSelecionado];
            Item itemAFornecer = origem.inventario[posItemOferecido];
            origem.DescartarItem(posItemOferecido);
            origem.AdicionarAoInventario(itemADevolver);
            destino.DescartarItem(posItemSelecionado);
            destino.AdicionarAoInventario(itemAFornecer);
            FecharMenu();
            cursor.GetComponent<ControleCursor>().Liberar();
            gs.SairMenuBatalha();
            gs.ProximoSeEmBatalha();
            GameObject.Find("CanvasInventario").GetComponent<GerenciadorInventario>().FecharMenu();
        }
        else if(entrada == Teclas.DPAD) {
            entrada = 0;
            posItemSelecionado = posItemSelecionado - (int) direcao.y;
            posItemSelecionado = (slots.Length + posItemSelecionado) % slots.Length;
            MostrarItens();
        }
    }

    void MostrarItens() {
        for (int i = 0; i < 8; i++)
        {
            if(destino.inventario[i] != null) {
                slots[i].text = destino.inventario[i].nome;
            } else {
                slots[i].text = " - ";
            }
        }
        slots[posItemSelecionado].text = "> " + slots[posItemSelecionado].text + " <";
    }

    

    public void AbrirMenu(Personagem origem, Personagem destino, int posItemOferecido) {
        this.origem = origem;
        this.destino = destino;
        this.posItemOferecido = posItemOferecido;
        posItemSelecionado = 0;
        gameObject.GetComponent<Canvas>().enabled = true;
        MostrarItens();
    }

    public void Reabrir() {

        gameObject.GetComponent<Canvas>().enabled = true;
    }

    public void FecharMenu() {
        gameObject.GetComponent<Canvas>().enabled = false;
    }
}
