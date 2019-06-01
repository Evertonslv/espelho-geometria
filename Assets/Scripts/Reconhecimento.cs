#if !(UNITY_LUMIN && !UNITY_EDITOR)

using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using System.Collections;
using System.Collections.Generic;
using OpenCVForUnity.CoreModule;
using OpenCVForUnity.ObjdetectModule;
using OpenCVForUnity.ImgprocModule;
using OpenCVForUnity.UnityUtils;
using OpenCVForUnity.UnityUtils.Helper;
using OpenCVRect = OpenCVForUnity.CoreModule.Rect;
using OpenCVForUnity.Features2dModule;

[RequireComponent(typeof(WebCamTextureToMatHelper))]
public class Reconhecimento : MonoBehaviour
{
    /// <summary>
    /// The gray mat.
    /// </summary>
    Mat grayMat;

    Texture2D imgTexture;

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

        imgTexture = Resources.Load("triangulo") as Texture2D;

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
        Debug.Log("OnWebCamTextureToMatHelperInitialized");

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
        Debug.Log("OnWebCamTextureToMatHelperDisposed");

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

            Mat teste = new Mat();

            // Coloca para escala de sinza
            //Imgproc.cvtColor(rgbaMat, grayMat, Imgproc.COLOR_RGBA2GRAY);

            //bool result = detector.detect(grayMat, points);   

            // Pinta
            Utils.fastMatToTexture2D(rgbaMat, texture);   

            //Mat img1Mat = rgbaMat;
           // Utils.texture2DToMat(imgTexture, img1Mat);
            //Debug.Log("img1Mat.ToString() " + img1Mat.ToString());

            //Mat img2Mat = new Mat(imgTexture.height, imgTexture.width, CvType.CV_8UC3);
            //Utils.texture2DToMat(imgTexture, img2Mat);

            //float angle = UnityEngine.Random.Range(0, 360), scale = 1.0f;

            //Point center = new Point(img2Mat.cols() * 0.5f, img2Mat.rows() * 0.5f);

            //Mat affine_matrix = Imgproc.getRotationMatrix2D(center, angle, scale);

            //Imgproc.warpAffine(img1Mat, img2Mat, affine_matrix, img2Mat.size());
            
                //ORB detector = ORB.create();
                //ORB extractor = ORB.create();

                //MatOfKeyPoint keypoints1 = new MatOfKeyPoint();
                //Mat descriptors1 = new Mat();

                //detector.detect(img1Mat, keypoints1);
                //extractor.compute(img1Mat, keypoints1, descriptors1);

                //MatOfKeyPoint keypoints2 = new MatOfKeyPoint();
                //Mat descriptors2 = new Mat();

                //detector.detect(img2Mat, keypoints2);
                //extractor.compute(img2Mat, keypoints2, descriptors2);


                //DescriptorMatcher matcher = DescriptorMatcher.create(DescriptorMatcher.BRUTEFORCE_HAMMINGLUT);
                //MatOfDMatch matches = new MatOfDMatch();

                //matcher.match(descriptors1, descriptors2, matches);


                //Mat resultImg = new Mat();

                //Features2d.drawMatches(img1Mat, keypoints1, img2Mat, keypoints2, matches, resultImg);


                //Texture2D texture = new Texture2D(resultImg.cols(), resultImg.rows(), TextureFormat.RGBA32, false);

                //Utils.matToTexture2D(resultImg, texture);

              //  gameObject.GetComponent<Renderer>().material.mainTexture = texture;
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

    /// <summary>
    /// Raises the back button click event.
    /// </summary>
    public void OnBackButtonClick()
    {
        SceneManager.LoadScene("OpenCVForUnityExample");
    }

    /// <summary>
    /// Raises the play button click event.
    /// </summary>
    public void OnPlayButtonClick()
    {
        webCamTextureToMatHelper.Play();
    }

    /// <summary>
    /// Raises the pause button click event.
    /// </summary>
    public void OnPauseButtonClick()
    {
        webCamTextureToMatHelper.Pause();
    }

    /// <summary>
    /// Raises the stop button click event.
    /// </summary>
    public void OnStopButtonClick()
    {
        webCamTextureToMatHelper.Stop();
    }

    /// <summary>
    /// Raises the change camera button click event.
    /// </summary>
    public void OnChangeCameraButtonClick()
    {
        webCamTextureToMatHelper.requestedIsFrontFacing = !webCamTextureToMatHelper.IsFrontFacing();
    }
}

#endif