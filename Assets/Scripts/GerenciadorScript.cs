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
    
    public GameObject canvas;
    public int opcaoMenuBatalha;
    private int cooldownMenuBatalha = 5;
    private bool menuAtivo;
    
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
        cooldownMenuBatalha++;
        if(menuAtivo && cooldownMenuBatalha > 4) {
            float deslocY = Input.GetAxisRaw("Vertical");
            if(deslocY != 0) {
                cooldownMenuBatalha = 0;
            }
            opcaoMenuBatalha -= (int) deslocY;
            opcaoMenuBatalha = (menuBatalha.Length + opcaoMenuBatalha) % menuBatalha.Length;
            for (int i = 0; i < menuBatalha.Length; i++)
            {
                menuBatalha[i].text = textoMenuBatalha[i];
            }
            menuBatalha[opcaoMenuBatalha].text = "> " + menuBatalha[opcaoMenuBatalha].text + " <";
            
        }
        if(menuAtivo) {
            if(Input.GetButtonDown("Cancel")) {
                SairMenuBatalha();
            }
            else if(Input.GetButtonDown("Fire1")) {
                //mover 
                if(opcaoMenuBatalha == 0){
                    cursor.GetComponent<ControleCursor>().cooldown = 6;
                    //dizer ao cursor que está no modo de escolher posição para mover alguém
                    cursor.GetComponent<ControleCursor>().movendoUnidade = true;
                    SairMenuBatalha();
                }
            }
        }
    }

    public void EntrarMenuBatalha() {
        cursor.GetComponent<ControleCursor>().ativo = false;
        canvas.GetComponent<Canvas>().enabled = true;
        menuAtivo = true;
        for (int i = 0; i < menuBatalha.Length; i++)
        {
            menuBatalha[i].text = textoMenuBatalha[i];
        }
        // menuBatalha[0].text = "fdsfs";
        // menuBatalha[0].text = textoMenuBatalha[0];

    }

    public void SairMenuBatalha() {
        cursor.GetComponent<ControleCursor>().ativo = true;
        canvas.GetComponent<Canvas>().enabled = false;
        menuAtivo = false;
    }
}
