using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class MainMenu : MonoBehaviour
{

    private bool loading;
    public TextMeshProUGUI text;
    public Animator anim;
    private int cooldown = 10;
    private int nDots;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (loading) {
            if (--cooldown == 0) {
                cooldown = 10;
                if (++nDots == 3) nDots = 0;
                text.text = "Carregando" + new string('.', nDots);
            }
        }
    }

    public void Jogar() {
        loading = true;
        anim.SetBool("loading", true);
        Destroy(transform.GetChild(0).gameObject);
        Invoke("enableText", .30f);
        StartCoroutine(LoadScene());
    }

    public void Sair() {
        Application.Quit();
    }

    IEnumerator LoadScene() {
        yield return new WaitForSeconds(2f);
        AsyncOperation async = SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex + 1);

        while (!async.isDone) {
            yield return null;
        }
    }

    private void enableText() {
        text.gameObject.SetActive(true);
    }
}
