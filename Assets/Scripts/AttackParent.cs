using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AttackParent : MonoBehaviour
{
    private Animator l, r;
    private Image lImage, RImage;
    private GameObject canvas;
    private AudioSource lSrc, rSrc;

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
        // l.runtimeAnimatorController.animationClips[0].wrapMode = WrapMode.Once;
        lImage = Instantiate(anim.GetComponent<Image>());
        lImage.transform.SetParent(canvas.transform, false);
        lSrc = l.GetComponent<AudioSource>();
    }

    public void SetRightAnimator(Animator anim) {
        r = Instantiate(anim);
        // r.runtimeAnimatorController.animationClips[0].wrapMode = WrapMode.Once;
        RImage = Instantiate(anim.GetComponent<Image>());
        RImage.transform.SetParent(canvas.transform, false);
        rSrc = r.GetComponent<AudioSource>();
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
            l.SetBool("atacando", true);
            lSrc.Play();
            StartCoroutine(EndAnim(l));
        }
        ControladorDano.criaTextoDano(dano, RImage.transform, 1f);
    }

    public void PlayRight(string dano) {
        if (r) {
            r.SetBool("atacando", true);
            rSrc.Play();
            StartCoroutine(EndAnim(r));
        }
        ControladorDano.criaTextoDano(dano, lImage.transform, -1f);
    }

    IEnumerator EndAnim(Animator anim) {
        yield return new WaitForSeconds(0.75f);
        // anim.SetBool("atacando", false);
    }

}
