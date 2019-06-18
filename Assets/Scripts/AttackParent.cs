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
        lSrc = Instantiate(l.GetComponent<AudioSource>());
    }

    public void SetRightAnimator(Animator anim) {
        r = Instantiate(anim);
        // r.runtimeAnimatorController.animationClips[0].wrapMode = WrapMode.Once;
        RImage = Instantiate(anim.GetComponent<Image>());
        RImage.transform.SetParent(canvas.transform, false);
        if (RImage.rectTransform.localPosition.x < 0) {
            RImage.rectTransform.localPosition = new Vector3(-RImage.rectTransform.localPosition.x, RImage.rectTransform.localPosition.y, RImage.rectTransform.localPosition.z);
            RImage.rectTransform.localRotation = new Quaternion(RImage.rectTransform.localRotation.x, 180, RImage.rectTransform.localRotation.z, RImage.rectTransform.localRotation.w); 
        }
        rSrc = Instantiate(r.GetComponent<AudioSource>());
    }

    public void Abrir() {
        canvas.GetComponent<Canvas>().enabled = true;
    }

    public void Fechar() {
        canvas.GetComponent<Canvas>().enabled = false;
        Destroy(l.gameObject);
        Destroy(l);
        Destroy(r.gameObject);
        Destroy(r);
        Destroy(RImage.gameObject);
        Destroy(RImage);
        Destroy(lImage.gameObject);
        Destroy(lImage);
        Destroy(rSrc.gameObject);
        Destroy(rSrc);
        Destroy(lSrc.gameObject);
        Destroy(lSrc);
    }

    public void PlayLeft(string dano, bool isDano, float xOffset = 1f, float yOffset = 0f) {
        if (l) {
            l.SetBool("atacando", true);
            lSrc.Play();
            StartCoroutine(EndAnim(l));
        }
        ControladorDano.criaTexto(dano, RImage.transform, xOffset, yOffset, isDano);
    }

    public void PlayRight(string dano, float xOffset = -1f, float yOffset = 0f) {
        if (r) {
            r.SetBool("atacando", true);
            rSrc.Play();
            StartCoroutine(EndAnim(r));
        }
        ControladorDano.criaTexto(dano, lImage.transform, xOffset, yOffset, true);
    }

    IEnumerator EndAnim(Animator anim) {
        yield return new WaitForSeconds(0.75f);
        // anim.SetBool("atacando", false);
    }

}
