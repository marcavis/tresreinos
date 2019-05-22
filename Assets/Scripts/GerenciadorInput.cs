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
            SetEntrada(cursores[cursorAtivo], Teclas.ACTION);
        } else {
            
            Vector3 novaDirecao = new Vector3(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"), 0);
            if(novaDirecao != Vector3.zero) {
                SetEntrada(cursores[cursorAtivo], Teclas.DPAD);
                SetDirecao(cursores[cursorAtivo], novaDirecao);
            }
        }
    }

    void SetEntrada(GameObject c, int entrada) {
        if(c == cursor) {cursor.GetComponent<ControleCursor>().entrada = entrada;}
        if(c == gs) {gs.GetComponent<GerenciadorScript>().entrada = entrada;}
    }

    void SetDirecao(GameObject c, Vector3 direcao) {
        if(c == cursor) {cursor.GetComponent<ControleCursor>().direcao = direcao;}
        if(c == gs) {gs.GetComponent<GerenciadorScript>().direcao = direcao;}
    }
}
