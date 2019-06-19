using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GerenciadorDialogo : MonoBehaviour
{
    public ControleCursor cursor;
    public GerenciadorScript gs;
    public int entrada;
    public Vector3 direcao;
    private Text caixaPersonagem;
    private Text caixaTexto;

    private string mensagem = "";
    private float progressoMsg;

    private List<Action<GerenciadorDialogo>> acoes;
    private int progressoAcoes;

    private int cooldown;
    private int cooldownPadrao;

    private bool finalizado;
    private int ultimoTipoDeAcao;
    const int NADA = 0;
    const int MOVECURSOR = 1;
    const int MOVEUNIDADE = 2;
    const int MENSAGEM = 3;

    // Start is called before the first frame update
    
    void Start()
    {
        cooldownPadrao = 50;
        cursor = GameObject.Find("Cursor").GetComponent<ControleCursor>();
        gs = GameObject.Find("Gerenciador").GetComponent<GerenciadorScript>();
        caixaPersonagem = GameObject.Find("DialogSpeaker").GetComponent<Text>();
        caixaTexto = GameObject.Find("DialogText").GetComponent<Text>();
        progressoMsg = 0f;
        //finalizado = true;
    }

    // Update is called once per frame
    void Update()
    {
        if(!finalizado) {
            if(ultimoTipoDeAcao != MENSAGEM) {
                cooldown--;
            }
            if(ultimoTipoDeAcao == MENSAGEM) {
                progressoMsg += 0.25f;
                if(caixaTexto.text.Length > 0) {
                    if(entrada == Teclas.CANCEL | entrada == Teclas.ACTION) {
                        entrada = 0;
                        if(progressoMsg > mensagem.Length) {
                            cooldown = 0;
                        } else {
                            progressoMsg = mensagem.Length;
                        }
                    }
                    if(entrada == Teclas.DPAD) {
                        entrada = 0;
                        progressoMsg += 0.75f;
                    }
                } else {
                    entrada = 0;
                }
                progressoMsg = Mathf.Min(progressoMsg, mensagem.Length);
                caixaTexto.text = mensagem.Substring(0, Mathf.FloorToInt(progressoMsg));
            }
            if(cooldown <= 0) {
                cooldown = cooldownPadrao;
                if (ProximaAcao() == -1) {
                    
                    Finalizar();
                }
            }
        }
    }

    public void Executar(string cena) {
        finalizado = false;
        if(DefinesDialogos.dialogos.ContainsKey(cena)) {
            acoes = DefinesDialogos.dialogos[cena];
        } else {
            acoes = new List<Action<GerenciadorDialogo>> {};
        }
        progressoMsg = 0f;
        progressoAcoes = 0;
        cooldown = cooldownPadrao / 4;
        ultimoTipoDeAcao = NADA;
    }

    public void Executar(List<Action<GerenciadorDialogo>> acoes) {
        finalizado = false;
        this.acoes = acoes;
        progressoMsg = 0f;
        progressoAcoes = 0;
        cooldown = cooldownPadrao / 4;
        ultimoTipoDeAcao = NADA;
    }

    public void Finalizar() {
        finalizado = true;
        //se estiver na batalha, então o gerenciador de script é que cuida 
        if(gs.estadoBatalha != 1) {
            gs.estadoBatalha++;
        }
        gs.AvancarCena();
    }

    public int ProximaAcao() {
        progressoMsg = 0f;
        gameObject.GetComponent<Canvas>().enabled = false;
        if(progressoAcoes >= acoes.Count) {
            return -1;
        } else {
            acoes[progressoAcoes](this);
            progressoAcoes++;
            return progressoAcoes - 1;
        }
    }

    //movimenta o cursor conforme pede a cena
    public void IrPara(Personagem unid) {
        ultimoTipoDeAcao = MOVECURSOR;
        cursor.IrParaPosicao(unid.transform.position);
    }

    //movimenta o cursor para uma posição fixa em vez de ir para uma unidade
    public void IrPara(Vector3 pos) {
        ultimoTipoDeAcao = MOVECURSOR;
        cursor.IrParaPosicao(pos);
    }

    public void Mover(Personagem unid, Vector3 pos) {
        ultimoTipoDeAcao = MOVEUNIDADE;
        unid.destinoFinal = unid.transform.position + pos;
        unid.TilesAcessiveis(cursor._tilemap);
        unid.PrepararCaminho();
    }

    //quando é personagem falando, usar o objeto personagem
    public void Dialogo(Personagem unid, string mensagem) {
        ultimoTipoDeAcao = MENSAGEM;
        this.caixaPersonagem.text = unid.nome;
        this.mensagem = mensagem;
        progressoMsg = 0f;
        gameObject.GetComponent<Canvas>().enabled = true;
    }

    //quando não é uma fala, o texto de título da caixa de mensagem pode ser outra coisa
    public void Dialogo(string titulo, string mensagem) {
        ultimoTipoDeAcao = MENSAGEM;
        this.caixaPersonagem.text = titulo;
        this.mensagem = mensagem;
        progressoMsg = 0f;
        gameObject.GetComponent<Canvas>().enabled = true;
    }

    public void FecharCanvasBatallha() {
        GameObject.Find("Placeholder").GetComponent<AttackParent>().Fechar();
    }
}
