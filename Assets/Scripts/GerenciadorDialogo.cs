using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GerenciadorDialogo : MonoBehaviour
{
    public Text caixaTexto;
    string mensagem = "Mussum Ipsum, cacilds vidis litro abertis. Quem manda na minha terra sou euzis! Quem num gosta di mim que vai caçá sua turmis! Diuretics paradis num copo é motivis de denguis. Tá deprimidis, eu conheço uma cachacis que pode alegrar sua vidis.";
    // Start is called before the first frame update
    public float progresso;
    void Start()
    {
        caixaTexto = GameObject.Find("DialogText").GetComponent<Text>();
        progresso = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        progresso += 0.5f;
        if(progresso <= mensagem.Length) {
            caixaTexto.text = mensagem.Substring(0, Mathf.FloorToInt(progresso));
        }
        //TODO: fazer andar mais rápido com action?
    }
}
