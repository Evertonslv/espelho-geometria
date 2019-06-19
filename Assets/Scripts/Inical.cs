using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using OpenCVForUnity.UnityUtils;
using System;
using System.Collections.Generic;
using UnityEditor;

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
            //SceneManager.LoadScene("Tela Error");

        // SpriteRenderer spriteAguarde = Component.FindObjectsOfType<SpriteRenderer>().ToList().Find( x=>x.name == "sprite_aguardando");
        Texture2D containerImgs = ScreenCapture.CaptureScreenshotAsTexture(2);
        Texture2D fotoCamera = camera.getTextura();
        Texture2D imagemScreen = Resources.Load("ScreenCapture") as Texture2D;

        if (imagemScreen == null)
            return;

        // Habilita a possibilidade de usar a imagem
        //SetTextureImporterFormat(imagemScreen, true);
        imagemScreen  = duplicateTexture(imagemScreen);

        imagemScreen.Apply();

        var bytes = imagemScreen.EncodeToJPG();
        File.WriteAllBytes("imagem1_tratamento.png", bytes);

        //spriteAguarde.enabled = true;

       var verificaImagem = rec.verificaImagem(fotoCamera, imagemScreen); //rec.verificaImagemContorno(fotoCamera) && 

        if (verificaImagem)
            SceneManager.LoadScene("Tela Accept");
         else
             SceneManager.LoadScene("Tela Error");

    }

    Texture2D duplicateTexture(Texture2D source)
    {
        RenderTexture renderTex = RenderTexture.GetTemporary(
                    source.width,
                    source.height,
                    0,
                    RenderTextureFormat.Default,
                    RenderTextureReadWrite.Linear);

        Graphics.Blit(source, renderTex);
        RenderTexture previous = RenderTexture.active;
        RenderTexture.active = renderTex;
        Texture2D readableText = new Texture2D(source.width, source.height);
        readableText.ReadPixels(new Rect(0, 0, renderTex.width, renderTex.height), 0, 0);
        readableText.Apply();
        RenderTexture.active = previous;
        RenderTexture.ReleaseTemporary(renderTex);
        return readableText;
    }

    //public static void SetTextureImporterFormat(Texture2D texture, bool isReadable)
    //{
    //    if (null == texture) return;

    //    string assetPath = AssetDatabase.GetAssetPath(texture);
    //    var tImporter = AssetImporter.GetAtPath(assetPath) as TextureImporter;
    //    if (tImporter != null)
    //    {
    //        tImporter.isReadable = isReadable;
    //        AssetDatabase.ImportAsset(assetPath);
    //        AssetDatabase.Refresh();
    //    }

    //}

}
