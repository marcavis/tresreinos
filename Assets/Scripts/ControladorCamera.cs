using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControladorCamera : MonoBehaviour
{
    public GameObject cursor;
    public int tamanhoGrid;

    private Vector3 offset;
    private float z;
    private Vector3 novaPosicao;
    // Start is called before the first frame update
    void Start()
    {
        // offset = transform.position - cursor.GetComponent<ControleCursor>().novaPosicao;
        z = transform.position.z;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void LateUpdate() {
        ControleCursor contCur = cursor.GetComponent<ControleCursor>();
        Vector3 novaPosicaoCursor = contCur.novaPosicao;
        if (contCur.acaoDoCursor == ControleCursor.NADA) {
            novaPosicaoCursor = cursor.transform.position;
            novaPosicao.z = z;
            transform.position = novaPosicao;
        }
        offset = transform.position - novaPosicaoCursor;
        if (contCur.acaoDoCursor != ControleCursor.NADA) {
            if (transform.position != novaPosicao) {
                transform.position = Vector3.MoveTowards(transform.position, novaPosicao, 10f * Time.deltaTime);
            } else if (Mathf.Abs(offset.x) > tamanhoGrid) {
                novaPosicaoCursor.x -= novaPosicaoCursor.x < 0 ? -tamanhoGrid : tamanhoGrid;
                novaPosicao = novaPosicaoCursor;
                novaPosicao.z = z;
            } else if (Mathf.Abs(offset.y) > tamanhoGrid) {
                novaPosicaoCursor.y -= novaPosicaoCursor.y < 0 ? -tamanhoGrid : tamanhoGrid;;
                novaPosicao = novaPosicaoCursor;
                novaPosicao.z = z;
            }
        }
    }

    public void IrParaPosicao(Transform t) {
        transform.position = novaPosicao = new Vector3(t.position.x, t.position.y, z);
    }

}
