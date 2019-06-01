using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TempoFinalizacao : MonoBehaviour
{
    public float timeLeft = 5.0f; // Tempo de espera
    public Text startText; // Usar para adicionar algum texto na tela

    // Start is called before the first frame update
    void Start()
    {        
    }

    // Update is called once per frame
    void Update()
    {
        timeLeft -= Time.deltaTime;
        //startText.text = (timeLeft).ToString("0");
        if (timeLeft < 0)
        {
            SceneManager.LoadScene("Tela2");
        }
    }
}
