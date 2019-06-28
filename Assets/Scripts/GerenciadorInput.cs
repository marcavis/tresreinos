using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GerenciadorInput : MonoBehaviour
{   
    public GameObject cursor;
    public GameObject gs;
    public GameObject telaInv;
    public GameObject telaInvTroca;
    public GameObject telaHab;
    public GameObject telaPause;

    public GameObject dialogo;
    public GameObject[] cursores;

    public int framesDPADSegurado = 0;
    public int travaDPAD = 0;
    public int cursorAtivo;
    // Start is called before the first frame update
    void Start()
    {
        cursor = GameObject.Find("Cursor");
        gs = GameObject.Find("Gerenciador");
        telaInv = GameObject.Find("CanvasInventario");
        telaInvTroca = GameObject.Find("CanvasInventarioTroca");
        telaHab = GameObject.Find("CanvasHabilidades");
        dialogo = GameObject.Find("CanvasDialogo");
        telaPause = GameObject.Find("CanvasPausa");
        cursores = new GameObject[] { cursor, gs, telaInv, telaInvTroca, telaHab, null, dialogo };
        cursorAtivo = 0; //cursor de seleção de unidade no campo
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetButtonDown("Fire1"))
        {
            SetEntrada(cursores[cursorAtivo], Teclas.ACTION);
        } else if (Input.GetButtonDown("Cancel")) {
            SetEntrada(cursores[cursorAtivo], Teclas.CANCEL);
        } else if (Input.GetKeyDown(KeyCode.P)) {
            telaPause.GetComponent<Canvas>().enabled = true;
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
                //cursor do mapa pode mover-se continuamente, outros precisam de um cooldown
                if (cursorAtivo == 0 && framesDPADSegurado > 14) 
                {
                    travaDPAD = 0;
                }
                if (cursorAtivo != 0) 
                {
                    if(framesDPADSegurado > 14 || cursorAtivo == 6) {
                        travaDPAD = 0;
                        framesDPADSegurado = 10;
                    }
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
        if(c == telaInv) {telaInv.GetComponent<GerenciadorInventario>().entrada = entrada;}
        if(c == telaInvTroca) {telaInvTroca.GetComponent<GerenciadorInventarioTroca>().entrada = entrada;}
        if(c == telaHab) {telaHab.GetComponent<GerenciadorTelaHab>().entrada = entrada;}
        if(c == null) {} //cursor nulo, sendo controlado pelo jogo/inimigo
        if(c == dialogo) {dialogo.GetComponent<GerenciadorDialogo>().entrada = entrada;}
    }

    void SetDirecao(GameObject c, Vector3 direcao) {
        if(c == cursor) {cursor.GetComponent<ControleCursor>().direcao = direcao;}
        if(c == gs) {gs.GetComponent<GerenciadorScript>().direcao = direcao;}
        if(c == telaInv) {telaInv.GetComponent<GerenciadorInventario>().direcao = direcao;}
        if(c == telaInvTroca) {telaInvTroca.GetComponent<GerenciadorInventarioTroca>().direcao = direcao;}
        if(c == telaHab) {telaHab.GetComponent<GerenciadorTelaHab>().direcao = direcao;}
        if(c == null) {} //cursor nulo, sendo controlado pelo jogo/inimigo
        if(c == dialogo) {dialogo.GetComponent<GerenciadorDialogo>().direcao = direcao;}
    }

    public void VoltarMainMenu() {
        SceneManager.LoadScene(0);
    }
}
