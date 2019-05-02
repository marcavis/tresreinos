using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GerenciadorScript : MonoBehaviour
{
    private List<Personagem> personagens;
    public Personagem prefabPersonagem;
    private Personagem persNovo;
    private Personagem persNovo2;
    // Start is called before the first frame update
    void Start()
    {
        persNovo = Instantiate<Personagem>(prefabPersonagem, new Vector3(1.5f, 1.5f, 0), Quaternion.identity);
        personagens.Add(persNovo);
        persNovo.nome = "Jim";
        persNovo.GetComponent<Personagem>().nome = "Joe";
        persNovo2 = Instantiate<Personagem>(prefabPersonagem, new Vector3(2.5f, 3.5f, 0), Quaternion.identity);
        personagens.Add(persNovo2);
    }

    // Update is called once per frame
    void Update()
    {
        print(persNovo.nome);
    }
}
