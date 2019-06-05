using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using OpenCVForUnity.CoreModule;
using OpenCVForUnity.ImgprocModule;
using OpenCVForUnity.UnityUtils;

public class Reconhecimento : MonoBehaviour
{
    public bool verificaImagem(Texture2D texture, Texture2D texture2)
    {
        //Texture2D tempTexture = Resources.Load("circulo") as Texture2D;
        Mat imgMat = new Mat(texture.height, texture.width, CvType.CV_8UC4);
        Mat tempMat = new Mat(texture2.height, texture2.width, CvType.CV_8UC4);
        Utils.texture2DToMat(texture, imgMat);
        Utils.texture2DToMat(texture2, tempMat);

        Imgproc.threshold(imgMat, imgMat, 0.8, 1.0, Imgproc.THRESH_TOZERO);
        Imgproc.threshold(tempMat, tempMat, 0.8, 1.0, Imgproc.THRESH_TOZERO);

        //Create the result mat
        int result_cols = imgMat.cols() - tempMat.cols() + 1;
        int result_rows = imgMat.rows() - tempMat.rows() + 1;
        Mat result = new Mat(result_rows, result_cols, CvType.CV_32FC1);

        int match_method = Imgproc.TM_CCOEFF_NORMED;

        Imgproc.matchTemplate(imgMat, tempMat, result, match_method);
        Debug.Log(match_method);

        return match_method <= 1;
    }
}
