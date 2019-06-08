using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControladorCamera : MonoBehaviour
{
    public GameObject cursor;
    public int tamanhoGrid;

    private Vector3 offset;
    private Vector3 novaPosicao;
    private bool mover = false;
    // Start is called before the first frame update
    void Start()
    {
        // offset = transform.position - cursor.GetComponent<ControleCursor>().novaPosicao;
        transform.position = new Vector3(transform.position.x, transform.position.y, -10);
        novaPosicao = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void LateUpdate() {
        mover = false;
        ControleCursor contCur = cursor.GetComponent<ControleCursor>();
        Vector3 posCur = cursor.transform.position;
        if( DeltaX() >= 3f && DeltaY() >= 1.5f) {
            novaPosicao.x = Mathf.Round(posCur.x);
            novaPosicao.y = Mathf.Round(posCur.y);
            mover = true;
        } else if( DeltaY() >= 1.5f) {
            novaPosicao.y = Mathf.Round(posCur.y);
            mover = true;
        } else if( DeltaX() >= 3f) {
            novaPosicao.x = Mathf.Round(posCur.x);
            mover = true;
        }

        if(mover) {
            transform.position = Vector3.MoveTowards(transform.position, novaPosicao, 10f * Time.deltaTime);
        }
    }

    public float DeltaX() {
        return Mathf.Abs(cursor.transform.position.x - transform.position.x);
    }

    public float DeltaY() {
        return Mathf.Abs(cursor.transform.position.y - transform.position.y);
    }
}
