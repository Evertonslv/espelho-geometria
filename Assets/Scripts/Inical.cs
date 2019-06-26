using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using OpenCVForUnity.UnityUtils;
using System;
using System.Collections.Generic;
using UnityEngine.UI;

public class Inical : MonoBehaviour
{

    private Reconhecimento rec = null;
    void Start() {
        rec = new Reconhecimento();
    }

    // Update is called once per frame
    void Update() {}

    public void iniciarBtn()
    {
        SceneManager.LoadScene("Tela2");
    }

    public void analizarImagemBtn(camera camera)
    {
        Texture2D fotoCamera = camera.getTextura();
        //Texture2D imagemScreen = Resources.Load("ScreenCapture") as Texture2D;

        //if (imagemScreen == null)
        //    return;

        //imagemScreen = copiar(imagemScreen);
        Texture2D imagemScreen = UIArrastar.teste;

        // Habilita a possibilidade de usar a imagem
        //SetTextureImporterFormat(imagemScreen, true);
        //imagemScreen  = duplicateTexture(imagemScreen)
        var bytes = imagemScreen.EncodeToJPG();
        File.WriteAllBytes("imagem_tratamento.png", bytes);

        //spriteAguarde.enabled = true;
        var verificaImagem = false;
        try
        {
            verificaImagem = rec.verificaImagem(fotoCamera, imagemScreen); //rec.verificaImagemContorno(fotoCamera) && 
        }
        catch
        { 
        }

        if (verificaImagem)
            SceneManager.LoadScene("Tela Accept");
         else
             SceneManager.LoadScene("Tela Error");
    }

    public Texture2D copiar(Texture2D texture)
    {
        // Cria um RenderTexture temporário do mesmo tamanho que a textura 
        RenderTexture tmp = RenderTexture.GetTemporary(
                    texture.width,
                    texture.height,
                    0,
                    RenderTextureFormat.Default,
                    RenderTextureReadWrite.Linear);

        Graphics.Blit(texture, tmp);
        RenderTexture previous = RenderTexture.active;
        Texture2D myTexture2D = new Texture2D(texture.width, texture.height);
        myTexture2D.ReadPixels(new Rect(0, 0, tmp.width, tmp.height), 0, 0);
        myTexture2D.Apply();
        RenderTexture.active = previous;
        RenderTexture.ReleaseTemporary(tmp);
        return myTexture2D;
    }
}
