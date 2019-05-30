using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Tilemaps;

public class GerenciadorScript : MonoBehaviour
{
    public List<Personagem> personagens = new List<Personagem>();
    public Personagem prefabPersonagem;

    public GameObject cursor;
    public GerenciadorInput gerenciadorInput;
    
    public GameObject canvas;
    public int opcaoMenuBatalha;
    
    public Text[] menuBatalha;
    private Text[] labels;
    private Text[] alvoLabels;
    private string[] textoMenuBatalha = {"Atacar", "Habilidades", "Itens", "Esperar"};

    public int entrada;
    public Vector3 direcao;

    public void AdicionarPersonagem(GameObject obj) {
        personagens.Add(obj.GetComponent<Personagem>());
    }
    // Start is called before the first frame update
    void Start()
    {
        gerenciadorInput = GameObject.Find("Input").GetComponent<GerenciadorInput>();
        canvas.GetComponent<Canvas>().enabled = false;
        labels = new Text[4];
        labels[0] = GameObject.Find("NomeLabel").GetComponent<Text>();
        labels[1] = GameObject.Find("PVLabel").GetComponent<Text>();
        labels[2] = GameObject.Find("PTLabel").GetComponent<Text>();
        labels[3] = GameObject.Find("StatusLabel").GetComponent<Text>();

        alvoLabels = new Text[2];
        alvoLabels[0] = GameObject.Find("AlvoLabel").GetComponent<Text>();
        alvoLabels[1] = GameObject.Find("AlvoPVLabel").GetComponent<Text>();

        personagens.Sort( (a, b) => (turnosAteAgir(a).CompareTo(turnosAteAgir(b))));
        foreach (var p in personagens)
        {
            print(p.nome);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(entrada == Teclas.CANCEL) {
            entrada = 0;
            SairMenuBatalha();
            cursor.GetComponent<ControleCursor>().LimparOverlays();
            cursor.GetComponent<ControleCursor>().DesfazerAcaoAtual();
        }
        else if(entrada == Teclas.ACTION) {
            entrada = 0;
            //atacar 
            if(opcaoMenuBatalha == 0){
                // SairMenuBatalha();
                Personagem unid = cursor.GetComponent<ControleCursor>().ultimaUnidade;
                if(!unid.ExistemAlvos()) {
                    //som de erro
                    //print("não pode atacar");
                } else {
                    gerenciadorInput.cursorAtivo = 0;
                    //o jogo ficará circulando entre os alvos permitidos, então começaremos
                    //movendo para o primeiro alvo encontrado (viés para o canto inferior esquerdo)
                    cursor.GetComponent<ControleCursor>().IrParaPrimeiroAlvo();
                    //print("pode atacar");
                }
            }
            //habilidades
            else if(opcaoMenuBatalha == 1){
            
            }
            //itens
            else if(opcaoMenuBatalha == 2){
            
            }
            //esperar
            else {
                SairMenuBatalha();
                cursor.GetComponent<ControleCursor>().Liberar();
                //print(cursor.GetComponent<ControleCursor>().acaoDoCursor);
            }
        } else if(entrada == Teclas.DPAD) {
            entrada = 0;
            opcaoMenuBatalha = opcaoMenuBatalha - (int) direcao.y;
            opcaoMenuBatalha = (menuBatalha.Length + opcaoMenuBatalha) % menuBatalha.Length;
            for (int i = 0; i < menuBatalha.Length; i++)
            {
                menuBatalha[i].text = textoMenuBatalha[i];
            }
            menuBatalha[opcaoMenuBatalha].text = "> " + menuBatalha[opcaoMenuBatalha].text + " <";
            cursor.GetComponent<ControleCursor>().LimparOverlays();
            if(opcaoMenuBatalha == 0) {
                cursor.GetComponent<ControleCursor>().MostrarOverlaysAtaque();
            }
        }
    }

    public int turnosAteAgir(Personagem p) {
        return Mathf.CeilToInt((1000f - p.iniciativa)/p.agilidade);
    }

    public void EntrarMenuBatalha() {
        canvas.GetComponent<Canvas>().enabled = true;
        for (int i = 0; i < menuBatalha.Length; i++)
        {
            menuBatalha[i].text = textoMenuBatalha[i];
        }
        //se houver alvo próximo, definir opção como 0, se não houver, como 3
        opcaoMenuBatalha = 0;
        menuBatalha[opcaoMenuBatalha].text = "> " + menuBatalha[opcaoMenuBatalha].text + " <";

        if(opcaoMenuBatalha == 0) {
            cursor.GetComponent<ControleCursor>().LimparOverlays();
            cursor.GetComponent<ControleCursor>().MostrarOverlaysAtaque();
        }

        Personagem unid = cursor.GetComponent<ControleCursor>().ultimaUnidade;
        labels[0].text = unid.nome + " Nv. " + unid.nivel;
        labels[1].text = string.Format("PV: {0,3:D3}/{1,3:D3}", unid.pv, unid.mpv);
        labels[2].text = string.Format("PT: {0,3:D3}/{1,3:D3}", unid.pt, unid.mpt);
        labels[3].text = string.Format("ATQ: {0,2:D2} DEF: {1,2:D2} AGI: {2,2:D2} MOV: {3,2:D2}", unid.ataque, unid.defesa, unid.agilidade, unid.movimento/10);
    }

    public void SairMenuBatalha() {
        gerenciadorInput.cursorAtivo = 0;
        canvas.GetComponent<Canvas>().enabled = false;
    }

    public Personagem ObjetoNoTile(Vector3 alvo) {
        foreach (Personagem p in personagens)
        {
            if(p.transform.position == alvo) {
                return p;
            }
        }
        return null;
    }

    public void MostrarDadosDoAlvo(Personagem unid) {
        alvoLabels[0].text = unid.nome + " Nv. " + unid.nivel;
        alvoLabels[1].text = string.Format("PV: {0,3:D3}/{1,3:D3}", unid.pv, unid.mpv);
    }

    public void ReiniciarLabelsAlvo() {
        alvoLabels[0].text = "";
        alvoLabels[1].text = "";
    }
}
