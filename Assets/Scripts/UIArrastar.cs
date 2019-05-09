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
                }
                objetoArrasta.SetParent(containerDesenho);
                objetoArrastaImg.raycastTarget = false;
            }
        }

        if (isArrasta)
        {
            objetoArrasta.position = Input.mousePosition;
        }

        if (Input.GetMouseButtonUp(0))
        {
            if(objetoArrasta != null)
            {
                objetoArrastaImg.raycastTarget = true;
               
                if (scrollView != null) {
                    RectTransform obj1 = (RectTransform)objetoArrasta;

                    if ((obj1.rect.width + objetoArrasta.position.x) >= scrollView.position.x)
                    {

                        Destroy(objetoArrasta.gameObject);
                    }
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
