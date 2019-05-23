using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GerenciadorScript : MonoBehaviour
{
    public List<Personagem> personagens;
    public Personagem prefabPersonagem;
    private Personagem persNovo;
    private Personagem persNovo2, persNovo3;

    public GameObject cursor;
    public GerenciadorInput gerenciadorInput;
    
    public GameObject canvas;
    public int opcaoMenuBatalha;
    
    public Text[] menuBatalha;
    private string[] textoMenuBatalha = {"Atacar", "Habilidades", "Itens", "Esperar"};

    public int entrada;
    public Vector3 direcao;

    // Start is called before the first frame update
    void Start()
    {
        gerenciadorInput = GameObject.Find("Input").GetComponent<GerenciadorInput>();
        canvas.GetComponent<Canvas>().enabled = false;
        cursor.GetComponent<ControleCursor>().ativo = true;
        persNovo = Instantiate<Personagem>(prefabPersonagem, new Vector3Int(2, 2, 0), Quaternion.identity);
        personagens = new List<Personagem>();
        personagens.Add(persNovo);
        persNovo.Inicializar("Zheng Xiulan");
        persNovo2 = Instantiate<Personagem>(prefabPersonagem, new Vector3Int(5, 0, 0), Quaternion.identity);
        personagens.Add(persNovo2);
        persNovo2.Inicializar("Miao Lin");
        persNovo3 = Instantiate<Personagem>(prefabPersonagem, new Vector3Int(-4, -3, 0), Quaternion.identity);
        personagens.Add(persNovo3);
        persNovo3.Inicializar("Guan Long");
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
}
