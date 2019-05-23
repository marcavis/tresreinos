using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GerenciadorInput : MonoBehaviour
{
    //referência para o cursor 
    
    public GameObject cursor;
    public GameObject gs;
    public GameObject[] cursores;

    public int framesDPADSegurado = 0;
    public int travaDPAD = 0;
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
        // travaDPAD--;
        // if(travaDPAD < 0) {travaDPAD = 0;}
        if(Input.GetButtonDown("Fire1"))
        {
            SetEntrada(cursores[cursorAtivo], Teclas.ACTION);
        } else if (Input.GetButtonDown("Cancel")) {
            SetEntrada(cursores[cursorAtivo], Teclas.CANCEL);
        } else {
            float hori = Input.GetAxisRaw("Horizontal");
            float vert = Input.GetAxisRaw("Vertical");
            Vector3 novaDirecao = new Vector3(hori, vert, 0f);
            //jogador soltou o direcional, entao podemos permitir novo uso
            if(novaDirecao == Vector3.zero) {
                travaDPAD = 0;
                framesDPADSegurado = 0;
            } else {
                framesDPADSegurado++;
                //não faz sentido segurar a direção no menu de batalha, talvez
                if (cursorAtivo == 0 && framesDPADSegurado > 14) 
                {
                    travaDPAD = 0;
                }
            }
            if(travaDPAD == 0) {
                if (novaDirecao != Vector3.zero)
                {
                    SetEntrada(cursores[cursorAtivo], Teclas.DPAD);
                    SetDirecao(cursores[cursorAtivo], novaDirecao);
                    travaDPAD = 1;
                }
            }
        }
    }

    void SetCursorAtivo(int c) {
        cursorAtivo = c;
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
