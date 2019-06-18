using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ControladorTxtDano : MonoBehaviour
{

    public Animator anim;
    private Text txt;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnEnable() {
        AnimatorClipInfo[] info = anim.GetCurrentAnimatorClipInfo(0);
        Destroy(gameObject, info[0].clip.length);
        txt = anim.GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void setText(string text) {
        txt.text = text;
    }

    public void setColor(Color color) {
        txt.color = color;
    }
}
