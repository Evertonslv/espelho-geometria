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
            SceneManager.LoadScene("Tela Error");

        // SpriteRenderer spriteAguarde = Component.FindObjectsOfType<SpriteRenderer>().ToList().Find( x=>x.name == "sprite_aguardando");
        //Texture2D containerImgs = ScreenCapture.CaptureScreenshotAsTexture(2);
        Texture2D fotoCamera = camera.getTextura();
        Texture2D imagemScreen = Resources.Load("ScreenCapture") as Texture2D;

        if (imagemScreen == null)
            return;
        
        // Habilita a possibilidade de usar a imagem
        SetTextureImporterFormat(imagemScreen, true);

        var bytes = imagemScreen.EncodeToJPG();
        File.WriteAllBytes("imagem1_tratamento.png", bytes);

        //spriteAguarde.enabled = true;

       var verificaImagem = rec.verificaImagemContorno(fotoCamera) && rec.verificaImagem(fotoCamera, imagemScreen);

        if (verificaImagem)
            SceneManager.LoadScene("Tela Accept");
         else
             SceneManager.LoadScene("Tela Error");

    }

    public static void SetTextureImporterFormat(Texture2D texture, bool isReadable)
    {
        if (null == texture) return;

        string assetPath = AssetDatabase.GetAssetPath(texture);
        var tImporter = AssetImporter.GetAtPath(assetPath) as TextureImporter;
        if (tImporter != null)
        {
            tImporter.isReadable = isReadable;
            AssetDatabase.ImportAsset(assetPath);
            AssetDatabase.Refresh();
        }
    }
}
