using UnityEngine;
using UnityEngine.SceneManagement;
using OpenCVForUnity.CoreModule;
using OpenCVForUnity.ObjdetectModule;
using OpenCVForUnity.UnityUtils;
using OpenCVForUnity.UnityUtils.Helper;
using OpenCVRect = OpenCVForUnity.CoreModule.Rect;

[RequireComponent(typeof(WebCamTextureToMatHelper))]
public class camera : MonoBehaviour
{
    /// <summary>
    /// The gray mat.
    /// </summary>
    Mat grayMat;

    /// <summary>
    /// The texture.
    /// </summary>
    Texture2D texture;

    /// <summary>
    /// The QRCode detector.
    /// </summary>
    QRCodeDetector detector;

    /// <summary>
    /// The points.
    /// </summary>
    Mat points;

    /// <summary>
    /// The image size rect.
    /// </summary>
    OpenCVRect imageSizeRect;

    /// <summary>
    /// The webcam texture to mat helper.
    /// </summary>
    WebCamTextureToMatHelper webCamTextureToMatHelper;

    // Use this for initialization
    void Start()
    {
        webCamTextureToMatHelper = gameObject.GetComponent<WebCamTextureToMatHelper>();

        detector = new QRCodeDetector();

        #if UNITY_ANDROID && !UNITY_EDITOR
            // Avoids the front camera low light issue that occurs in only some Android devices (e.g. Google Pixel, Pixel2).
            webCamTextureToMatHelper.avoidAndroidFrontCameraLowLightIssue = true;
        #endif
        webCamTextureToMatHelper.Initialize();

    }

    /// <summary>
    /// Raises the web cam texture to mat helper initialized event.
    /// </summary>
    public void OnWebCamTextureToMatHelperInitialized()
    {

        Mat webCamTextureMat = webCamTextureToMatHelper.GetMat();

        texture = new Texture2D(webCamTextureMat.cols(), webCamTextureMat.rows(), TextureFormat.RGBA32, false);

        gameObject.GetComponent<Renderer>().material.mainTexture = texture;

        gameObject.transform.localScale = new Vector3(webCamTextureMat.cols(), webCamTextureMat.rows(), 1);
        Debug.Log("Screen.width " + Screen.width + " Screen.height " + Screen.height + " Screen.orientation " + Screen.orientation);

        float width = webCamTextureMat.width();
        float height = webCamTextureMat.height();

        float widthScale = (float)Screen.width / width;
        float heightScale = (float)Screen.height / height;
        if (widthScale < heightScale)
        {
            Camera.main.orthographicSize = (width * (float)Screen.height / (float)Screen.width) / 2;
        }
        else
        {
            Camera.main.orthographicSize = height / 2;
        }

        grayMat = new Mat(webCamTextureMat.rows(), webCamTextureMat.cols(), CvType.CV_8UC1);
        imageSizeRect = new OpenCVRect(0, 0, grayMat.width(), grayMat.height());

        points = new Mat();

        // if WebCamera is frontFaceing, flip Mat.
        if (webCamTextureToMatHelper.GetWebCamDevice().isFrontFacing)
        {
           webCamTextureToMatHelper.flipHorizontal = true;
        }
    }

    /// <summary>
    /// Raises the web cam texture to mat helper disposed event.
    /// </summary>
    public void OnWebCamTextureToMatHelperDisposed()
    {

        if (grayMat != null)
            grayMat.Dispose();

        if (texture != null)
        {
            Texture2D.Destroy(texture);
            texture = null;
        }

        if (points != null)
            points.Dispose();
    }

    /// <summary>
    /// Raises the web cam texture to mat helper error occurred event.
    /// </summary>
    /// <param name="errorCode">Error code.</param>
    public void OnWebCamTextureToMatHelperErrorOccurred(WebCamTextureToMatHelper.ErrorCode errorCode)
    {
        Debug.Log("OnWebCamTextureToMatHelperErrorOccurred " + errorCode);
    }

    // Update is called once per frame
    void Update()
    {
        if (webCamTextureToMatHelper.IsPlaying() && webCamTextureToMatHelper.DidUpdateThisFrame())
        {
            // Pega imagem
            Mat rgbaMat = webCamTextureToMatHelper.GetMat();

            Utils.fastMatToTexture2D(rgbaMat, texture);   
        }
    }

    /// <summary>
    /// Raises the destroy event.
    /// </summary>
    void OnDestroy()
    {
        webCamTextureToMatHelper.Dispose();

        if (detector != null)
            detector.Dispose();
    }

    public Texture2D getTextura()
    {
        return texture;
    }
}