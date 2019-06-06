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

    private RectTransform lixeira;

    private RectTransform objComponente;

    private float widthObj;

    private float HeightObj;

    private void Start()
    {
        GameObject scroll = GameObject.Find("/Canvas/Scroll View/Viewport/imagens");
        scrollView = scroll.transform;

        var objLixeira = Component.FindObjectsOfType<Image>().ToList().Find( x=>x.name == "lixeira");
        lixeira = objLixeira.GetComponent<RectTransform>();

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
            objComponente = objetoArrasta.GetComponent<RectTransform>();
            objComponente.sizeDelta = GetSizeObjeto();
            objetoArrasta.position = GetPositionObjeto();
        }

        if (Input.GetMouseButtonUp(0))
        {
            if(objetoArrasta != null)
            {
                objetoArrastaImg.raycastTarget = true;
               
                if (scrollView != null) {                   
                    if (ObjetoSobreposLixeira()) Destroy(objetoArrasta.gameObject);
                }

                objetoArrasta = null;

            }

            isArrasta = false;
        }
    }

    private Vector2 GetSizeObjeto() {
        //int widthProporcional = Mathf.RoundToInt(widthObj+(Screen.width*0.30f));
        //int hightProporcional = Mathf.RoundToInt(HeightObj+(Screen.height*0.30f));

        int widthProporcional = Mathf.RoundToInt(widthObj+180);
        int hightProporcional = Mathf.RoundToInt(HeightObj+180);

        // se o objeto saiu do scroll
        if (Input.mousePosition.x < scrollView.position.x)
            return new Vector2(widthProporcional, hightProporcional);
        else if(MouseSobreposLixeira())
            return new Vector2(widthObj, HeightObj);

        return new Vector2(objComponente.rect.width, objComponente.rect.height);
    }   

    private Vector3 GetPositionObjeto() {
        // se o objeto entrou no scroll
        if(((Input.mousePosition.x + (objComponente.rect.width/2)) >= scrollView.position.x) && !ObjetoSobreposLixeira())
            return new Vector3((scrollView.position.x - (objComponente.rect.width/2)), Input.mousePosition.y, Input.mousePosition.z);
        else if(Input.mousePosition.x + (objComponente.rect.width/2) >= Screen.width)
            return new Vector3((Screen.width - (objComponente.rect.width/2)), Input.mousePosition.y, Input.mousePosition.z);
        else if((Input.mousePosition.x <= (objComponente.rect.width/2)) && (Input.mousePosition.x < scrollView.position.x))
            return new Vector3((objComponente.rect.width/2), Input.mousePosition.y, Input.mousePosition.z);
        else if(Input.mousePosition.y - (objComponente.rect.height/2) <= 0)
            return new Vector3(Input.mousePosition.x, (objComponente.rect.height/2), Input.mousePosition.z);
        else if(Input.mousePosition.y + (objComponente.rect.height/2) >= Screen.height)
            return new Vector3(Input.mousePosition.x, (Screen.height - (objComponente.rect.height/2)), Input.mousePosition.z);
        else
            return Input.mousePosition;
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

    private bool MouseSobreposLixeira() {
        return (Input.mousePosition.x > lixeira.position.x && 
                    Input.mousePosition.y < (lixeira.position.y + lixeira.rect.height));
    }
    
    private bool ObjetoSobreposLixeira() {
        return (Input.mousePosition.x + (objComponente.rect.width/2) > lixeira.position.x && 
                    Input.mousePosition.y < (lixeira.position.y + lixeira.rect.height));
    }
}
