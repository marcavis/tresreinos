using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;

public class GerenciadorScript : MonoBehaviour
{
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


        labels = new Text[4];
        labels[0] = GameObject.Find("NomeLabel").GetComponent<Text>();
        labels[1] = GameObject.Find("PVLabel").GetComponent<Text>();
        labels[2] = GameObject.Find("PTLabel").GetComponent<Text>();
        labels[3] = GameObject.Find("StatusLabel").GetComponent<Text>();

        alvoLabels = new Text[2];
        alvoLabels[0] = GameObject.Find("AlvoLabel").GetComponent<Text>();
        alvoLabels[1] = GameObject.Find("AlvoPVLabel").GetComponent<Text>();
        
        AvancarCena();
        
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

    public void AvancarCena() {
        if(estadoBatalha == 0) {
            gerenciadorInput.cursorAtivo = 6;
            dialogo.Executar(SceneManager.GetActiveScene().name + "_inic");
        } else if(estadoBatalha == 1) {
            gerenciadorInput.cursorAtivo = 0;
            Proximo();
        } else if(estadoBatalha == 2) {
            print("cena pos fase");
            gerenciadorInput.cursorAtivo = 6;
            dialogo.Executar(SceneManager.GetActiveScene().name + "_fim");
            //ações pós batalha
        } else {
            print("mudar de fase");
            //mudar de fase?
        }
    }

    public void ProximoSeEmBatalha() {
        if(estadoBatalha == 1) {
            Proximo();
        }
    }
    public void Proximo() {
        
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
        labels[0].text = unid.nome + " Nv. " + unid.nivel;
        labels[1].text = string.Format("PV: {0,3:D3}/{1,3:D3}", unid.pv, unid.MPV());
        labels[2].text = string.Format("PT: {0,3:D3}/{1,3:D3}", unid.pt, unid.MPT());
        labels[3].text = string.Format("ATQ: {0,2:D2} DEF: {1,2:D2} AGI: {2,2:D2} MOV: {3,2:D2}", unid.Ataque(), unid.Defesa(), unid.Agilidade(), unid.Movimento()/10);
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
