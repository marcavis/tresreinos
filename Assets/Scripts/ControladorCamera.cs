using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControladorCamera : MonoBehaviour
{
    public GameObject cursor;
    public int tamanhoGrid;

    private Vector3 offset;
    private Vector3 novaPosicao;
    private bool travaX, travaY;
    // Start is called before the first frame update
    void Start()
    {
        // offset = transform.position - cursor.GetComponent<ControleCursor>().novaPosicao;
        transform.position = new Vector3(transform.position.x, transform.position.y, -10);
        novaPosicao = transform.position;
        travaX = false;
        travaY = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void LateUpdate() {
        ControleCursor contCur = cursor.GetComponent<ControleCursor>();
        Vector3 posCur = cursor.transform.position;
        if( DeltaX() >= 3f && DeltaY() >= 1.5f) {
            novaPosicao.x = posCur.x;
            novaPosicao.y = posCur.y;
            //travaY = true;
            transform.position = Vector3.MoveTowards(transform.position, novaPosicao, 12f * Time.deltaTime);
        } else if( DeltaY() >= 1.5f) {
            novaPosicao.y = posCur.y;
            //travaX = true;
            transform.position = Vector3.MoveTowards(transform.position, novaPosicao, 12f * Time.deltaTime);
        } else if( DeltaX() >= 3f) {
            novaPosicao.x = posCur.x;
            transform.position = Vector3.MoveTowards(transform.position, novaPosicao, 12f * Time.deltaTime);
        }
    }

    public float DeltaX() {
        return Mathf.Abs(cursor.transform.position.x - transform.position.x);
    }

    public float DeltaY() {
        return Mathf.Abs(cursor.transform.position.y - transform.position.y);
    }
}
