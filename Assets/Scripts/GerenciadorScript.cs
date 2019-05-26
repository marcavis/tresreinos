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
    }

    // Update is called once per frame
    void Update()
    {
        if(entrada == Teclas.CANCEL) {
            entrada = 0;
            SairMenuBatalha();
            cursor.GetComponent<ControleCursor>().DesfazerAcaoAtual();
        }
        else if(entrada == Teclas.ACTION) {
            entrada = 0;
            //atacar 
            if(opcaoMenuBatalha == 0){
                //dizer ao cursor que está no modo de escolher posição para mover alguém
                //cursor.GetComponent<ControleCursor>().movendoUnidade = true;
                // SairMenuBatalha();
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
        }
    }

    public void EntrarMenuBatalha() {
        canvas.GetComponent<Canvas>().enabled = true;
        for (int i = 0; i < menuBatalha.Length; i++)
        {
            menuBatalha[i].text = textoMenuBatalha[i];
        }
        opcaoMenuBatalha = 0;
        menuBatalha[opcaoMenuBatalha].text = "> " + menuBatalha[opcaoMenuBatalha].text + " <";        
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
}
