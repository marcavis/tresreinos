using System.Collections;
using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;

public class GerenciadorScript : MonoBehaviour
{
    public bool temProxFase;
    public List<Personagem> personagens = new List<Personagem>();
    public Personagem prefabPersonagem;

    public GameObject cursor;
    public GerenciadorInput gerenciadorInput;
    public GameObject _camera;
    
    public GameObject canvas;
    public GameObject canvasAlvo;

    public GameObject canvasInventario;
    public GameObject canvasInventarioTroca;
    public GameObject canvasHabilidades;
    public GerenciadorDialogo dialogo;

    public List<List<Action<GerenciadorDialogo>>> mensagensPendentes;
    public int opcaoMenuBatalha;
    
    public Text[] menuBatalha;
    private Text[] labels;
    private Text[] alvoLabels;
    private string[] textoMenuBatalha = {"Atacar", "Habilidades", "Itens", "Esperar"};

    public int entrada;
    public Vector3 direcao;
    public bool canvasBatalhaAberto;

    public int estadoBatalha; //diz se estamos antes, durante ou depois da batalha
    public AudioSource srcMenu;
    public AudioClip clipMenuSelect;
    public AudioClip clipMenuCancel;
    public AudioClip clipMenuChange;
    public AudioClip clipMenuErro;

    public void AdicionarPersonagem(GameObject obj) {
        personagens.Add(obj.GetComponent<Personagem>());
    }
    // Start is called before the first frame update
    void Start()
    {
        estadoBatalha = 0;
        gerenciadorInput = GameObject.Find("Input").GetComponent<GerenciadorInput>();
        canvas = GameObject.Find("CanvasMenuBatalha");
        canvas.GetComponent<Canvas>().enabled = false;
        canvasAlvo = GameObject.Find("CanvasAlvo");
        canvasAlvo.GetComponent<Canvas>().enabled = false;
        canvasInventario = GameObject.Find("CanvasInventario");
        canvasInventario.GetComponent<Canvas>().enabled = false;
        canvasInventarioTroca = GameObject.Find("CanvasInventarioTroca");
        canvasInventarioTroca.GetComponent<Canvas>().enabled = false;
        canvasHabilidades = GameObject.Find("CanvasHabilidades");
        canvasHabilidades.GetComponent<Canvas>().enabled = false;

        GameObject.Find("CanvasDialogo").GetComponent<Canvas>().enabled = false;
        dialogo = GameObject.Find("CanvasDialogo").GetComponent<GerenciadorDialogo>();
        mensagensPendentes = new List<List<Action<GerenciadorDialogo>>>();

        menuBatalha = new Text[] {
            GameObject.Find("AttackBtn").GetComponent<Text>(),
            GameObject.Find("SkillBtn").GetComponent<Text>(),
            GameObject.Find("ItemBtn").GetComponent<Text>(),
            GameObject.Find("WaitBtn").GetComponent<Text>()
        };

        labels = new Text[4];
        labels[0] = GameObject.Find("NomeLabel").GetComponent<Text>();
        labels[1] = GameObject.Find("PVLabel").GetComponent<Text>();
        labels[2] = GameObject.Find("PTLabel").GetComponent<Text>();
        labels[3] = GameObject.Find("StatusLabel").GetComponent<Text>();

        alvoLabels = new Text[2];
        alvoLabels[0] = GameObject.Find("AlvoLabel").GetComponent<Text>();
        alvoLabels[1] = GameObject.Find("AlvoPVLabel").GetComponent<Text>();
        
        AvancarCena();
        if(SceneManager.GetActiveScene().buildIndex == 0) {
            SalvarAtributos(true);
        }
        CarregarAtributosSalvos();
    }

    // Update is called once per frame
    void Update()
    {
        if(entrada == Teclas.CANCEL) {
            entrada = 0;
            PlaySoundMenuCancel();
            SairMenuBatalha();
            cursor.GetComponent<ControleCursor>().LimparOverlays();
            cursor.GetComponent<ControleCursor>().DesfazerAcaoAtual();
        }
        else if(entrada == Teclas.ACTION) {
            entrada = 0;
            bool somErro = false;
            //atacar 
            if(opcaoMenuBatalha == 0){
                Personagem unid = cursor.GetComponent<ControleCursor>().ultimaUnidade;
                if(!unid.ExistemPersonagensAlvos(unid.arma.alcanceMin, unid.arma.alcanceMax, false)) {
                    somErro = true;
                    //print("não pode atacar");
                } else {
                    gerenciadorInput.cursorAtivo = 0;
                    canvasAlvo.GetComponent<Canvas>().enabled = true;
                    //o jogo ficará circulando entre os alvos permitidos, então começaremos
                    //movendo para o primeiro alvo encontrado (viés para o canto inferior esquerdo)
                    cursor.GetComponent<ControleCursor>().IrParaPrimeiroAlvo();
                }
            }
            //habilidades
            else if(opcaoMenuBatalha == 1){
                // PlaySoundMenuSelect();
                Personagem unid = cursor.GetComponent<ControleCursor>().ultimaUnidade;
                gerenciadorInput.cursorAtivo = 4;
                canvasHabilidades.GetComponent<GerenciadorTelaHab>().AbrirMenu(unid);
            }
            //itens
            else if(opcaoMenuBatalha == 2){
                // PlaySoundMenuSelect();
                Personagem unid = cursor.GetComponent<ControleCursor>().ultimaUnidade;
                gerenciadorInput.cursorAtivo = 2;
                canvasInventario.GetComponent<GerenciadorInventario>().AbrirMenu(unid);
            }
            //esperar
            else {
                // PlaySoundMenuSelect();
                SairMenuBatalha();
                cursor.GetComponent<ControleCursor>().Liberar();
                ProximoSeEmBatalha();
                //print(cursor.GetComponent<ControleCursor>().acaoDoCursor);
            }
            PlaySoundMenuSelect(somErro);
        } else if(entrada == Teclas.DPAD) {
            entrada = 0;
            PlaySoundMenuChange();
            opcaoMenuBatalha = opcaoMenuBatalha - (int) direcao.y;
            opcaoMenuBatalha = (menuBatalha.Length + opcaoMenuBatalha) % menuBatalha.Length;
            for (int i = 0; i < menuBatalha.Length; i++)
            {
                menuBatalha[i].text = textoMenuBatalha[i];
            }
            menuBatalha[opcaoMenuBatalha].text = "> " + menuBatalha[opcaoMenuBatalha].text + " <";
            cursor.GetComponent<ControleCursor>().LimparOverlays();
            if(opcaoMenuBatalha == 0) {
                cursor.GetComponent<ControleCursor>().MostrarOverlaysAtaque();
            }
        }
    }

    public void SalvarAtributos() {
        Personagem[] objHerois = new Personagem[]  {GameObject.Find("ZhengXiulan").GetComponent<Personagem>(), 
                                                    GameObject.Find("MiaoLin").GetComponent<Personagem>(), 
                                                    GameObject.Find("TaoJiang").GetComponent<Personagem>(), 
                                                    GameObject.Find("LiuJingsheng").GetComponent<Personagem>(), 
                                                    GameObject.Find("JiangXun").GetComponent<Personagem>(), 
                                                    GameObject.Find("GuanLong").GetComponent<Personagem>()};

        for (int i = 0; i < Defines.herois.Length; i++)
        {
            PlayerPrefs.SetInt("nivel_" + i, objHerois[i].nivel);
            PlayerPrefs.SetInt("exp_" + i, objHerois[i].exp);
        }
        PlayerPrefs.Save();
    }

    public void SalvarAtributos(bool zerar) {
        Personagem[] objHerois = new Personagem[]  {GameObject.Find("ZhengXiulan").GetComponent<Personagem>(), 
                                                    GameObject.Find("MiaoLin").GetComponent<Personagem>(), 
                                                    GameObject.Find("TaoJiang").GetComponent<Personagem>(), 
                                                    GameObject.Find("LiuJingsheng").GetComponent<Personagem>(), 
                                                    GameObject.Find("JiangXun").GetComponent<Personagem>(), 
                                                    GameObject.Find("GuanLong").GetComponent<Personagem>()};

        if(zerar) {
            for (int i = 0; i < Defines.herois.Length; i++)
            {
                PlayerPrefs.SetInt("nivel_" + i, 1);
                PlayerPrefs.SetInt("exp_" + i, 0);
            }
            PlayerPrefs.Save();
        }
    }

    public void CarregarAtributosSalvos() {
        Personagem[] objHerois = new Personagem[]  {GameObject.Find("ZhengXiulan").GetComponent<Personagem>(), 
                                                    GameObject.Find("MiaoLin").GetComponent<Personagem>(), 
                                                    GameObject.Find("TaoJiang").GetComponent<Personagem>(), 
                                                    GameObject.Find("LiuJingsheng").GetComponent<Personagem>(), 
                                                    GameObject.Find("JiangXun").GetComponent<Personagem>(), 
                                                    GameObject.Find("GuanLong").GetComponent<Personagem>()};

        for (int i = 0; i < Defines.herois.Length; i++)
        {
            objHerois[i].nivel = PlayerPrefs.GetInt("nivel_" + i);
            objHerois[i].exp = PlayerPrefs.GetInt("exp_" + i);
        }
    }

    public void AvancarCena() {
        if(estadoBatalha == 0) {
            gerenciadorInput.cursorAtivo = 6;
            dialogo.Executar(SceneManager.GetActiveScene().name + "_inic");
        } else if(estadoBatalha == 1) {
            gerenciadorInput.cursorAtivo = 0;
            ProximoSeEmBatalha();
        } else if(estadoBatalha == 2) {
            print("cena pos fase");
            gerenciadorInput.cursorAtivo = 6;
            dialogo.Executar(SceneManager.GetActiveScene().name + "_fim");
            //ações pós batalha
        } else {
            print("mudar de fase");
            if(temProxFase) {
                SalvarAtributos();
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            }
        }
    }

    public void ProximoSeEmBatalha() {
        if(estadoBatalha == 1) {
            if(mensagensPendentes.Count > 0) {
                gerenciadorInput.cursorAtivo = 6;
                dialogo.Executar(mensagensPendentes[0]);
                mensagensPendentes.RemoveAt(0);
            } else {
                Proximo();
            }
        }
    }
    
    public void Proximo() {
        personagens[0].PosTurno();
        if(personagens.Where((a) => a.time == 0).ToList().Count == 0) {
            print("Game over");
        } else if (personagens.Where((a) => a.time == 1).ToList().Count == 0) {
            print("Vencedor");
            estadoBatalha++;
            AvancarCena();
            return;
        }
        while(personagens[0].iniciativa < 1000) {
            foreach (var p in personagens)
            {
                p.iniciativa += p.Agilidade();
            }
            personagens.Sort( (a, b) => (TurnosAteAgir(a).CompareTo(TurnosAteAgir(b))));
        }
        personagens[0].PreTurno();
        if(personagens[0].pv > 0) {
            if (personagens[0].time == 1) {
                gerenciadorInput.cursorAtivo = 5;
                personagens[0].gameObject.GetComponent<Inimigo>().Iniciar();
            } else {
                gerenciadorInput.cursorAtivo = 0;
            }
            cursor.GetComponent<ControleCursor>().IrParaUnidade(personagens[0]);
            //_camera.GetComponent<ControladorCamera>().IrParaPosicao(personagens[0].transform);
            personagens[0].iniciativa -= 1000;
        }
    }

    public void AdicionarMsgNivel(Personagem unid, int niveis, int[] atributosAntigos) {
        List<Action<GerenciadorDialogo>> novaMsg = new List<Action<GerenciadorDialogo>>();
        string titulo = unid.nome + " ganhou " + niveis.ToString() + ((niveis > 1) ? " níveis!" : " nível!");
        string texto = String.Format("Pontos de vida máximos subiram de {0} para {1}! ", atributosAntigos[0], unid.MPV());
        texto += String.Format("Pontos de técnica máximos subiram de {0} para {1}! ", atributosAntigos[1], unid.MPT());
        texto += String.Format("Poder de ataque subiu de {0} para {1}! ", atributosAntigos[2], unid.Ataque());
        texto += String.Format("Defesa subiu de {0} para {1}! ", atributosAntigos[3], unid.Defesa());
        texto += String.Format("Agilidade subiu de {0} para {1}! ", atributosAntigos[4], unid.Agilidade());
        novaMsg.Add(gd => {gd.Dialogo(titulo, texto);});
        mensagensPendentes.Add(novaMsg);
    }

    //retorna positivo se é a unidade que deve agir agora
    public bool SeAtual(Personagem p) {
        return p == personagens[0];
    }
    public int TurnosAteAgir(Personagem p) {
        return Mathf.CeilToInt((1000f - p.iniciativa)/p.Agilidade());
    }

    public void EntrarMenuBatalha() {
        for (int i = 0; i < menuBatalha.Length; i++)
        {
            menuBatalha[i].text = textoMenuBatalha[i];
        }
        //se houver alvo próximo, definir opção como 0, se não houver, como 3
        opcaoMenuBatalha = 0;
        menuBatalha[opcaoMenuBatalha].text = "> " + menuBatalha[opcaoMenuBatalha].text + " <";

        if(opcaoMenuBatalha == 0) {
            srcMenu.PlayOneShot(clipMenuSelect);
            cursor.GetComponent<ControleCursor>().LimparOverlays();
            cursor.GetComponent<ControleCursor>().MostrarOverlaysAtaque();
        }
        AtualizarMenuBatalha();
        canvas.GetComponent<Canvas>().enabled = true;
    }

    //versão usada por inimigos e unidades não selecionadas
    public void MostrarMenuBatalhaInativo(Personagem unid) {
        for (int i = 0; i < menuBatalha.Length; i++)
        {
            menuBatalha[i].text = "";
        }
        AtualizarMenuBatalha(unid);
        canvas.GetComponent<Canvas>().enabled = true;
    }



    public void AtualizarMenuBatalha(Personagem unid) {
        labels[0].text = unid.nome + " Nv. " + unid.nivel + "." + string.Format("{0,2:D2}", unid.exp);
        labels[1].text = string.Format("PV: {0,3:D3}/{1,3:D3}", unid.pv, unid.MPV());
        labels[2].text = string.Format("PT: {0,3:D3}/{1,3:D3}", unid.pt, unid.MPT());
        string at, df, ag, mv;

        if(unid.Ataque() == unid.Ataque(true)) {
            at = string.Format("ATQ: {0,2:D2}", unid.Ataque().ToString());
        } else {
            at = unid.Ataque() > unid.Ataque(true) ? string.Format("ATQ: <color=#0000ffff>{0,2:D2}</color>", unid.Ataque().ToString()) :
                                                        string.Format("ATQ: <color=#ff0000ff>{0,2:D2}</color>", unid.Ataque().ToString());
        }
        if(unid.Defesa() == unid.Defesa(true)) {
            df = string.Format("DEF: {0,2:D2}", unid.Defesa().ToString());
        } else {
            df = unid.Defesa() > unid.Defesa(true) ? string.Format("DEF: <color=#0000ffff>{0,2:D2}</color>", unid.Defesa().ToString()) :
                                                        string.Format("DEF: <color=#ff0000ff>{0,2:D2}</color>", unid.Defesa().ToString());
        }
        if(unid.Agilidade() == unid.Agilidade(true)) {
            ag = string.Format("AGI: {0,2:D2}", unid.Agilidade().ToString());
        } else {
            ag = unid.Agilidade() > unid.Agilidade(true) ? string.Format("AGI: <color=#0000ffff>{0,2:D2}</color>", unid.Agilidade().ToString()) :
                                                        string.Format("AGI: <color=#ff0000ff>{0,2:D2}</color>", unid.Agilidade().ToString());
        }
        if(unid.Movimento() == unid.Movimento(true)) {
            mv = string.Format("MOV: {0,2:D2}", (unid.Movimento()/10).ToString());
        } else {
            mv = unid.Movimento() > unid.Movimento(true) ? string.Format("MOV: <color=#0000ffff>{0,2:D2}</color>", (unid.Movimento()/10).ToString()) :
                                                        string.Format("MOV: <color=#ff0000ff>{0,2:D2}</color>", (unid.Movimento()/10).ToString());
        }

        labels[3].text = at + " " + df + " " + ag + " " + mv;
        //labels[3].text = string.Format("DEF: {1,2:D2} AGI: {2,2:D2} MOV: {3,2:D2}", unid.Ataque(), unid.Defesa(), unid.Agilidade(), unid.Movimento()/10);
    }

    public void AtualizarMenuBatalha() {
        AtualizarMenuBatalha(cursor.GetComponent<ControleCursor>().ultimaUnidade);
    }

    public void SairMenuBatalha() {
        gerenciadorInput.cursorAtivo = 0;
        canvas.GetComponent<Canvas>().enabled = false;
    }

    public Personagem ObjetoNoTile(Vector3 alvo) {
        foreach (Personagem p in personagens)
        {
            if(p.transform.position == alvo) {
                return p;
            }
        }
        return null;
    }

    public void MostrarDadosDoAlvo(Personagem unid) {
        canvasAlvo.GetComponent<Canvas>().enabled = true;
        alvoLabels[0].text = unid.nome + " Nv. " + unid.nivel;
        alvoLabels[1].text = string.Format("PV: {0,3:D3}/{1,3:D3}", unid.pv, unid.MPV());
    }

    public void ReiniciarLabelsAlvo() {
        canvasAlvo.GetComponent<Canvas>().enabled = false;
        alvoLabels[0].text = "";
        alvoLabels[1].text = "";
    }

    public void PlaySoundMenuChange() {
        srcMenu.PlayOneShot(clipMenuChange);
    }

    public void PlaySoundMenuCancel() {
        srcMenu.PlayOneShot(clipMenuCancel);
    }

    public void PlaySoundMenuSelect(bool erro = false) {
        srcMenu.PlayOneShot(erro ? clipMenuErro : clipMenuSelect);
    }
}
