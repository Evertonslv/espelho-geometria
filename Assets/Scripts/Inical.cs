using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Inical : MonoBehaviour
{

    void Start() {
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
        scroll.SetActive(false);
        Texture2D textura = camera.getTextura();
        camera.OnWebCamTextureToMatHelperDisposed();
             
        Texture2D containerImgs = ScreenCapture.CaptureScreenshotAsTexture();
        ScreenCapture.CaptureScreenshot("teste.png", 2);
       // containerImgs.width = containerImgs.width - 80;

        if (containerImgs != null && rec.verificaImagem(textura, containerImgs))
            SceneManager.LoadScene("Tela Accept");
        else
            SceneManager.LoadScene("Tela Error");
    }
}
