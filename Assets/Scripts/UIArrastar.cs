using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Linq;

public class UIArrastar : MonoBehaviour
{
    //Nome da tag criada
    public const string ARRASTAR_TAG = "UIArrastar";

    //Indica se esta sendo arrastado o objeto
    private bool isArrasta = false;

    //Posição original
    private Vector2 posicaoOriginal;

    //Objeto a movimentar
    private Transform objetoArrasta;

    //Imagem do objeto
    private Image objetoArrastaImg;

    //Objeto onde fica os objetos de imagem
    private Transform scrollView;

    private Transform containerDesenho;

    List<RaycastResult> listaObj = new List<RaycastResult>();

    private float widthObj;

    private float HeightObj;

    private void Start()
    {
        GameObject scroll = GameObject.Find("/Canvas/Scroll View/Viewport/imagens");
        scrollView = scroll.transform;

        scroll = null;

        GameObject obj = GameObject.Find("/Canvas/objetos_desenho");
        containerDesenho = obj.transform;
        obj = null;

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            objetoArrasta = GetTransformObj();

            if(objetoArrasta != null)
            {
                isArrasta = true;
                objetoArrasta.SetAsLastSibling();
                
                posicaoOriginal = objetoArrasta.position;
                objetoArrastaImg = objetoArrasta.GetComponent<Image>();

                if (objetoArrasta.parent.name == "imagens")
                {
                    Transform copia = Instantiate(objetoArrasta);
                    copia.SetParent(objetoArrasta.parent);
                    copia.position = posicaoOriginal;
                    copia.localScale = objetoArrasta.localScale;

                    widthObj = copia.GetComponent<RectTransform>().rect.width;
                    HeightObj = copia.GetComponent<RectTransform>().rect.height;
                }
                objetoArrasta.SetParent(containerDesenho);
                objetoArrastaImg.raycastTarget = false;
            }
        }

        if (isArrasta)
        {
            RectTransform obj = objetoArrasta.GetComponent<RectTransform>();

            int widthProporcional = Mathf.RoundToInt(widthObj+(Screen.width*0.30f));
            int hightProporcional = Mathf.RoundToInt(HeightObj+(Screen.height*0.30f));

            // se o objeto saiu do scroll
            if (Input.mousePosition.x < scrollView.position.x)
                obj.sizeDelta = new Vector2(widthProporcional, hightProporcional);
            else
                obj.sizeDelta = new Vector2(widthObj, HeightObj);

            // se o objeto entrou no scroll
            if(((Input.mousePosition.x + (obj.rect.width/2)) >= scrollView.position.x) 
                 && (Input.mousePosition.x < scrollView.position.x))
                objetoArrasta.position = new Vector3((scrollView.position.x - (obj.rect.width/2)), Input.mousePosition.y, Input.mousePosition.z);
            else if((Input.mousePosition.x <= (obj.rect.width/2))
                 && (Input.mousePosition.x < scrollView.position.x))
                objetoArrasta.position = new Vector3((obj.rect.width/2), Input.mousePosition.y, Input.mousePosition.z);
            else if(Input.mousePosition.y - (obj.rect.height/2) <= 0)
                objetoArrasta.position = new Vector3(Input.mousePosition.x, (obj.rect.height/2), Input.mousePosition.z);
            else if(Input.mousePosition.y + (obj.rect.height/2) >= Screen.height)
                objetoArrasta.position = new Vector3(Input.mousePosition.x, (Screen.height - (obj.rect.height/2)), Input.mousePosition.z);
            else
                objetoArrasta.position = Input.mousePosition;
           
            Debug.Log("arrasta:"+objetoArrasta.position.x);
            Debug.Log("scroll:"+scrollView.position.x);
            Debug.Log("width:"+obj.rect.width);
            Debug.Log("mouse:"+Input.mousePosition);
            Debug.Log("Screen Width: " + Screen.width);
            Debug.Log("Screen height: " + Screen.height);

        }

        if (Input.GetMouseButtonUp(0))
        {
            if(objetoArrasta != null)
            {
                objetoArrastaImg.raycastTarget = true;
               
                if (scrollView != null) {
                    if (Input.mousePosition.x >= scrollView.position.x)
                        Destroy(objetoArrasta.gameObject);
                }

                objetoArrasta = null;

            }

            isArrasta = false;
        }
    }

    private GameObject GetObjectSelecionado()
    {
        var ponteiro = new PointerEventData(EventSystem.current);
        ponteiro.position = Input.mousePosition;

        EventSystem.current.RaycastAll(ponteiro, listaObj);

        if (listaObj.Count <= 0) return null;
        return listaObj.First().gameObject;
    }

    private Transform GetTransformObj()
    {
        GameObject objClicado = GetObjectSelecionado();

        if (objClicado != null && objClicado.tag == ARRASTAR_TAG)
        {
            return objClicado.transform;
        }

        return null;
    }
}
