using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GerenciadorInput : MonoBehaviour
{
    //referência para o cursor 
    
    public GameObject cursor;
    public GameObject gs;
    private int cursorAtivo;
    // Start is called before the first frame update
    void Start()
    {
        cursor = GameObject.Find("Cursor");
        gs = GameObject.Find("Gerenciador");

        cursorAtivo = 1; //cursor de seleção de unidade no campo
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
