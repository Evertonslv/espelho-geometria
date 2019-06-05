using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Inical : MonoBehaviour
{

    GameObject scroll = null;

    void Start() {
        scroll = GameObject.Find("/Canvas/Scroll View");
    }

    // Update is called once per frame
    void Update() { }

    public void iniciarBtn()
    {
        SceneManager.LoadScene("Tela2");
    }

    public void analizarImagemBtn(camera camera)
    {
        scroll.SetActive(false);
        Texture2D textura = camera.getTextura();
        camera.OnWebCamTextureToMatHelperDisposed();
             
        Texture2D containerImgs = ScreenCapture.CaptureScreenshotAsTexture();
        ScreenCapture.CaptureScreenshot("teste.png", 2);

        if (containerImgs != null && new Reconhecimento().verificaImagem(textura, containerImgs))
            SceneManager.LoadScene("Tela Accept");
        else
            SceneManager.LoadScene("Tela Error");
    }
}
