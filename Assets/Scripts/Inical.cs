using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Inical : MonoBehaviour
{
    // Start is called before the first frame update
    void Start() { }

    // Update is called once per frame
    void Update() { }

    public void iniciarBtn()
    {
        SceneManager.LoadScene("Tela2");
    }

    public void analizarImagemBtn(camera camera, Texture2D containerImgs)
    {
        Reconhecimento rec = new Reconhecimento();

        if(rec.verificaImagem(camera.getTextura(), containerImgs))
            SceneManager.LoadScene("Tela Accept");
        else
            SceneManager.LoadScene("Tela Error");
    }
}
