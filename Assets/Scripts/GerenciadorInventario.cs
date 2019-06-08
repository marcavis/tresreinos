using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GerenciadorInventario : MonoBehaviour
{
    public GameObject cursor;
    public GameObject canvasAlvo;
    public GerenciadorInput gerenciadorInput;
    public int estado;
    const int SELECAO_ITEM = 0;
    const int SELECAO_ACAO = 1;
    public int entrada;
    public Vector3 direcao;

    private int posItemSelecionado;

    private int posAcaoSelecionada;
    private Item itemSelecionado;
    private Text[] slots;
    private Text armaLabel;
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
            slots[i] = GameObject.Find("Slot" + (i+1)).GetComponent<Text>();
        }
        armaLabel = GameObject.Find("ArmaLabel").GetComponent<Text>();
        acoes = new Text[3];
        acoes[0] = GameObject.Find("EquipUseBtn").GetComponent<Text>();
        acoes[1] = GameObject.Find("TrocaBtn").GetComponent<Text>();
        acoes[2] = GameObject.Find("DescartaBtn").GetComponent<Text>();
        campoDesc = GameObject.Find("Descricao").GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        if(entrada == Teclas.CANCEL) {
            entrada = 0;
            if(estado == SELECAO_ITEM) {
                VoltarAoMenuDeBatalha();
            } else if (estado == SELECAO_ACAO) {
                LimparAcoes();
                estado = SELECAO_ITEM;
            }
        } else if(entrada == Teclas.ACTION) {
            entrada = 0;
            if(estado == SELECAO_ITEM) {
                Item itemAtual = unid.inventario[posItemSelecionado];
                if(itemAtual != null) {
                    itemSelecionado = itemAtual;
                    estado = SELECAO_ACAO;
                    MostrarAcoes();
                } else {
                    //TODO: som de erro, seleção inválida
                }
            } else if(estado == SELECAO_ACAO) {
                
                if(posAcaoSelecionada == 0) {
                    if(acoes[0].text == "> Desequipar <") {
                        unid.arma = DefinesArmas.armas["Punho"];
                    } else if(acoes[0].text == "> Equipar <") {
                        //equipar a arma que tem o mesmo nome do item selecionado
                        unid.arma = DefinesArmas.armas[itemSelecionado.nome];
                    } else {
                        if(itemSelecionado.efeitoUso == null) {
                            //TODO: som de erro, não usar item, não terminar rodada
                        }
                        //usar o item
                        //TODO: deve ir para o cursor para achar um alvo
                        //itemSelecionado.efeitoUso(unid);
                    }
                } else if(posAcaoSelecionada == 1) { 
                    if(!unid.ExistemAlvos(1, true)) {
                        //som de erro
                        //print("não pode trocar item");
                    } else {
                        gerenciadorInput.cursorAtivo = 0;
                        canvasAlvo.GetComponent<Canvas>().enabled = true;
                        //o jogo ficará circulando entre os alvos permitidos, então começaremos
                        //movendo para o primeiro alvo encontrado (viés para o canto inferior esquerdo)
                        cursor.GetComponent<ControleCursor>().IrParaPrimeiroAlvoTroca();
                        cursor.GetComponent<ControleCursor>().MostrarOverlaysTroca();
                        cursor.GetComponent<ControleCursor>().indiceItemSelecionado = posItemSelecionado;
                        gameObject.GetComponent<Canvas>().enabled = false;
                    }
                } else if(posAcaoSelecionada == 2) {
                    //TODO: melhor pedir um diálogo de confirmação
                    //TODO: som de descarte

                    unid.DescartarItem(posItemSelecionado);

                    itemSelecionado = null;
                    estado = SELECAO_ITEM;
                    LimparAcoes();
                }
                MostrarItens();
                if(itemSelecionado != null) {
                    MostrarAcoes();
                }
                MostrarArmaEquipada();
            }
        }
        else if(entrada == Teclas.DPAD) {
            entrada = 0;
            if(estado == SELECAO_ITEM) {
                posItemSelecionado = posItemSelecionado - (int) direcao.y;
                posItemSelecionado = (slots.Length + posItemSelecionado) % slots.Length;
                MostrarItens();
            } else if (estado == SELECAO_ACAO) {
                posAcaoSelecionada = posAcaoSelecionada - (int) direcao.y;
                posAcaoSelecionada = (acoes.Length + posAcaoSelecionada) % acoes.Length;
                MostrarAcoes();
            }
        }
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
        Item itemAtual = unid.inventario[posItemSelecionado];
        campoDesc.text = itemAtual != null ? itemAtual.descricao : "-";
    }

    void MostrarArmaEquipada() {
        armaLabel.text = "Arma: " + unid.arma.nome;
    }

    void LimparAcoes() {
        foreach (var btn in acoes)
        {
            btn.text = "";
        }
    }

    void MostrarAcoes() {
        if(DefinesArmas.armas.ContainsKey(itemSelecionado.nome)) {
            if(itemSelecionado.nome == unid.arma.nome) {
                acoes[0].text = "Desequipar";
            } else {
                acoes[0].text = "Equipar";
            }
        } else {
            acoes[0].text = "Usar";
        }
        acoes[1].text = "Trocar";
        acoes[2].text = "Descartar";
        acoes[posAcaoSelecionada].text = "> " + acoes[posAcaoSelecionada].text + " <";
    }
    

    public void AbrirMenu(Personagem unid) {
        this.unid = unid;
        posItemSelecionado = 0;
        posAcaoSelecionada = 0;
        itemSelecionado = null;
        gameObject.GetComponent<Canvas>().enabled = true;
        MostrarItens();
        LimparAcoes();
        MostrarArmaEquipada();
    }

    public void Reabrir() {

        gameObject.GetComponent<Canvas>().enabled = true;
    }

    public void VoltarAoMenuDeBatalha() {
        gerenciadorInput.cursorAtivo = 1;
        gameObject.GetComponent<Canvas>().enabled = false;
    }

    public void FecharMenu() {
        gameObject.GetComponent<Canvas>().enabled = false;
        estado = SELECAO_ITEM;
    }
}
