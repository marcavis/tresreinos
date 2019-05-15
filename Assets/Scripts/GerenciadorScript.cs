using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GerenciadorScript : MonoBehaviour
{
    public List<Personagem> personagens {get; set;}
    public Personagem prefabPersonagem;
    private Personagem persNovo;
    private Personagem persNovo2, persNovo3;

    public GameObject cursor;
    
    public GameObject canvas;
    public int opcaoMenuBatalha;
    private bool menuAtivo;
    private bool podeMover;
    public Text[] menuBatalha;
    private string[] textoMenuBatalha = {"Mover", "Atacar", "Habilidades", "Itens", "Parar"};
    // Start is called before the first frame update
    void Start()
    {
        canvas.GetComponent<Canvas>().enabled = false;
        cursor.GetComponent<ControleCursor>().ativo = true;
        persNovo = Instantiate<Personagem>(prefabPersonagem, new Vector3Int(2, 2, 0), Quaternion.identity);
        personagens = new List<Personagem>();
        personagens.Add(persNovo);
        persNovo.nome = "Jim";
        persNovo2 = Instantiate<Personagem>(prefabPersonagem, new Vector3Int(5, 0, 0), Quaternion.identity);
        personagens.Add(persNovo2);
        persNovo3 = Instantiate<Personagem>(prefabPersonagem, new Vector3Int(-4, -3, 0), Quaternion.identity);
        personagens.Add(persNovo3);
    }

    // Update is called once per frame
    void Update()
    {
        if(menuAtivo && podeMover) {
            float deslocY = Input.GetAxisRaw("Vertical");
            opcaoMenuBatalha -= (int) deslocY;
            opcaoMenuBatalha = (menuBatalha.Length + opcaoMenuBatalha) % menuBatalha.Length;
            print(opcaoMenuBatalha);
            for (int i = 0; i < menuBatalha.Length; i++)
            {
                menuBatalha[i].text = textoMenuBatalha[i];
            }
            menuBatalha[opcaoMenuBatalha].text = "> " + menuBatalha[opcaoMenuBatalha].text + " <";
            
        }
        //print(persNovo.nome);
    }

    public void EntrarMenuBatalha(GameObject origem) {
        origem.GetComponent<ControleCursor>().ativo = false;
        canvas.GetComponent<Canvas>().enabled = true;
        menuAtivo = true;
        podeMover = true;
        // menuBatalha[0].text = "fdsfs";
        // menuBatalha[0].text = textoMenuBatalha[0];

    }
}
