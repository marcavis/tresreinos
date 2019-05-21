using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GerenciadorInput : MonoBehaviour
{
    //referência para o cursor 
    
    public GameObject cursor;
    public GameObject gs;
    public GameObject[] cursores;
    public int cursorAtivo;
    // Start is called before the first frame update
    void Start()
    {
        cursor = GameObject.Find("Cursor");
        gs = GameObject.Find("Gerenciador");
        cursores = new GameObject[] { cursor, gs };
        cursorAtivo = 0; //cursor de seleção de unidade no campo
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetButtonDown("Fire1"))
        {
            if (cursorAtivo == 0)
            {
                cursor.GetComponent<ControleCursor>().entrada = ControleCursor.ACTION;
            } else if (cursorAtivo == 1)
            {
                gs.GetComponent<GerenciadorScript>().entrada = ControleCursor.ACTION;
            }
        }
    }

    
}
