using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inimigo : MonoBehaviour
{

    public bool vezInimigo;
    private int targetIndex = -1;
    private GerenciadorScript gs;
    private Personagem personagem;
    private List<Personagem> inimigosAcessiveis;
    private float lastTime;
    private bool mostrandoOverlay;
    // Start is called before the first frame update
    void Start()
    {
        gs = GameObject.Find("Gerenciador").GetComponent<GerenciadorScript>();
        personagem = gameObject.GetComponent<Personagem>();
    }

    // Update is called once per frame
    void Update()
    {
        if (vezInimigo) {
            atacaInimigo();
        }
    }

    public void atacaInimigo() {
        if (targetIndex == -1) {
            inimigosAcessiveis = personagem.GetInimigosAcessiveis();
            targetIndex = Random.Range(0, inimigosAcessiveis.Count);
            Vector3 novaPosicao = inimigosAcessiveis[targetIndex].transform.position;
            gs.cursor.GetComponent<ControleCursor>().novaPosicao = novaPosicao;
            lastTime = Time.time;
        }
        if (gs.cursor.GetComponent<ControleCursor>().transform.position == gs.cursor.GetComponent<ControleCursor>().novaPosicao) {
            if (!mostrandoOverlay) {
                gs.cursor.GetComponent<ControleCursor>().MostrarOverlaysAtaque();
                mostrandoOverlay = true;
            }
            if (transicaoCompletou()) {
                gameObject.GetComponent<Personagem>().Atacar(inimigosAcessiveis[targetIndex]);
                vezInimigo = false;
                targetIndex = -1;
                gs.cursor.GetComponent<ControleCursor>().LimparOverlays();
                gs.ReiniciarLabelsAlvo();
                gs.Proximo();
            } 
        }
    }

    private bool transicaoCompletou() {
        return Time.time - lastTime >= 2f;
    }
}
