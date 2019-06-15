using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControladorDano : MonoBehaviour
{
    private static ControladorTxtDano popupText;
    private static GameObject canvas;

    public static void Init() {
        canvas = GameObject.Find("CanvasDano");
        if (!popupText) {
            popupText = Resources.Load<ControladorTxtDano>("Text/PopupTextParent");
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public static void criaTextoDano(string texto, Transform t) {
        ControladorTxtDano txtDano = Instantiate(popupText);
        Vector2 txtPosition = new Vector2(t.position.x, t.position.y);
        txtDano.transform.SetParent(canvas.transform, false);
        txtDano.transform.position = txtPosition;
        txtDano.setText(texto);
    }
}
