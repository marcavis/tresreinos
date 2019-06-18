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

    public static void criaTexto(string texto, Transform t, float xOffset, float yOffset, bool isDano) {
        ControladorTxtDano txtDano = Instantiate(popupText);
        Vector2 txtPosition = new Vector2(t.position.x + xOffset, t.position.y + yOffset);
        txtDano.transform.SetParent(canvas.transform, false);
        txtDano.transform.position = txtPosition;
        if (!isDano) txtDano.setColor(Color.green);
        txtDano.setText(texto);
    }
}
