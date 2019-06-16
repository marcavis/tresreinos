using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AttackParent : MonoBehaviour
{
    private Animator l, r;
    private Image lImage, RImage;
    private GameObject canvas;

    public void Init() {
        if (!canvas) {
            canvas = GameObject.Find("CanvasBatalha");
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

    public void SetLeftAnimator(Animator anim) {
        l = Instantiate(anim);
        l.runtimeAnimatorController.animationClips[0].wrapMode = WrapMode.Once;
        lImage = Instantiate(anim.GetComponent<Image>());
        lImage.transform.SetParent(canvas.transform, false);
    }

    public void SetRightAnimator(Animator anim) {
        r = Instantiate(anim);
        r.runtimeAnimatorController.animationClips[0].wrapMode = WrapMode.Once;
        RImage = Instantiate(anim.GetComponent<Image>());
        RImage.transform.SetParent(canvas.transform, false);
    }

    public void Abrir() {
        canvas.GetComponent<Canvas>().enabled = true;
    }

    public void Fechar() {
        canvas.GetComponent<Canvas>().enabled = false;
        Destroy(l);
        Destroy(r);
        Destroy(RImage);
        Destroy(lImage);
    }

    public void PlayLeft(string dano) {
        if (l) {
            l.Play("Atk");
            ControladorDano.criaTextoDano(dano, RImage.transform, 1f);
        }
    }

    public void PlayRight(string dano) {
        if (r) {
            r.Play("Atk");
            ControladorDano.criaTextoDano(dano, lImage.transform, -1f);
        }
    }

}
