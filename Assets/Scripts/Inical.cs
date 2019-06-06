using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;
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
        SpriteRenderer spriteAguarde = Component.FindObjectsOfType<SpriteRenderer>().ToList().Find( x=>x.name == "sprite_aguardando");
        
        // scroll.SetActive(false);
        // Texture2D textura = camera.getTextura();
        // camera.OnWebCamTextureToMatHelperDisposed();
             
        // Texture2D containerImgs = ScreenCapture.CaptureScreenshotAsTexture();
        // ScreenCapture.CaptureScreenshot("teste.png", 2);
        
        spriteAguarde.enabled = true;
    
        // var verificaImagem = new Reconhecimento().verificaImagem(textura, containerImgs);

        // if (containerImgs != null && verificaImagem)
        //     SceneManager.LoadScene("Tela Accept");
        // else
        //     SceneManager.LoadScene("Tela Error");
    }
}
