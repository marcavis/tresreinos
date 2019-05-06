using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GerenciadorScript : MonoBehaviour
{
    public List<Personagem> personagens {get; set;}
    public Personagem prefabPersonagem;
    private Personagem persNovo;
    private Personagem persNovo2, persNovo3;
    // Start is called before the first frame update
    void Start()
    {
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
        //print(persNovo.nome);
    }


}
