using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Inical : MonoBehaviour
{
    GameObject fundo = null;

    void Start() {
        this.fundo = GameObject.Find("/Canvas/fundo_branco");
        fundo.SetActive(false);
    }

    // Update is called once per frame
    void Update() { }

    public void iniciarBtn()
    {
        SceneManager.LoadScene("Tela2");
    }

    public void analizarImagemBtn(camera camera)
    {
        Reconhecimento rec = new Reconhecimento();

        GameObject scroll = GameObject.Find("/Canvas/Scroll View");
        this.fundo.SetActive(true);
        scroll.SetActive(false);

        Texture2D containerImgs = ScreenCapture.CaptureScreenshotAsTexture();
        ScreenCapture.CaptureScreenshot("teste.png", 2);
       // containerImgs.width = containerImgs.width - 80;

        if (containerImgs != null && rec.verificaImagem(camera.getTextura(), containerImgs))
            SceneManager.LoadScene("Tela Accept");
        else
            SceneManager.LoadScene("Tela Error");
    }
}
