using UnityEngine;
using OpenCVForUnity.CoreModule;
using OpenCVForUnity.ImgprocModule;
using OpenCVForUnity.Features2dModule;
using OpenCVForUnity.UnityUtils;
using System.Collections.Generic;
using System.IO;

public class Reconhecimento
{
    //public bool verificaImagem(Texture2D texture, Texture2D texture2)
    //{
    //    //Texture2D tempTexture = Resources.Load("circulo") as Texture2D;
    //    Mat imgMat = new Mat(texture.height, texture.width, CvType.CV_8UC4);
    //    Mat tempMat = new Mat(texture2.height, texture2.width, CvType.CV_8UC4);
    //    Utils.texture2DToMat(texture, imgMat);
    //    Utils.texture2DToMat(texture2, tempMat);

    //    Imgproc.threshold(imgMat, imgMat, 0.8, 1.0, Imgproc.THRESH_TOZERO);
    //    Imgproc.threshold(tempMat, tempMat, 0.8, 1.0, Imgproc.THRESH_TOZERO);

    //    //Create the result mat
    //    int result_cols = imgMat.cols() - tempMat.cols() + 1;
    //    int result_rows = imgMat.rows() - tempMat.rows() + 1;
    //    Mat result = new Mat(result_rows, result_cols, CvType.CV_32FC1);

    //    int match_method = Imgproc.TM_CCOEFF_NORMED;

    //    Imgproc.matchTemplate(imgMat, tempMat, result, match_method);
    //    Debug.Log(match_method);

    //    return match_method <= 1;
    //}

    //public bool verificaImagem(Texture2D textParam, Texture2D textParam2)
    //{
    //    var bytes = textParam.EncodeToJPG();
    //    File.WriteAllBytes("imagem1_tratamento.png", bytes);
    //    bytes = textParam2.EncodeToJPG();
    //    File.WriteAllBytes("imagem2_tratamento.png", bytes);

    //    //Texture2D imgTexture = Resources.Load("circulo") as Texture2D;
    //    Texture2D camFoto = textParam;
    //    Texture2D printTela = textParam2;

    //    // Escala de cinza. CV_8UC1
    //    Mat img1Mat = new Mat(camFoto.height, camFoto.width, CvType.CV_8UC1);
    //    Utils.texture2DToMat(camFoto, img1Mat);

    //    // Escala de cinza. CV_8UC1
    //    Mat img2Mat = new Mat(printTela.height, printTela.width, CvType.CV_8UC1);
    //    Utils.texture2DToMat(printTela, img2Mat);

    //    Imgproc.GaussianBlur(img1Mat, img1Mat, new Size(5, 5), 0);
    //    Texture2D tex3 = new Texture2D(img1Mat.cols(), img1Mat.rows(), TextureFormat.RGBA32, false);
    //    Utils.matToTexture2D(img1Mat, tex3);
    //    bytes = tex3.EncodeToJPG();
    //    File.WriteAllBytes("imagem1_tratamento_gaussian.png", bytes);
    //    Imgproc.threshold(img1Mat, img1Mat, 100, 255, Imgproc.THRESH_BINARY);
    //    tex3 = new Texture2D(img1Mat.cols(), img1Mat.rows(), TextureFormat.RGBA32, false);
    //    Utils.matToTexture2D(img1Mat, tex3);
    //    bytes = tex3.EncodeToJPG();
    //    File.WriteAllBytes("imagem1_tratamento_threshold.png", bytes);

    //    Imgproc.GaussianBlur(img2Mat, img2Mat, new Size(5, 5), 0);
    //    Texture2D tex4 = new Texture2D(img2Mat.cols(), img2Mat.rows(), TextureFormat.RGBA32, false);
    //    Utils.matToTexture2D(img2Mat, tex4);
    //    bytes = tex4.EncodeToJPG();
    //    File.WriteAllBytes("imagem2_tratamento_gaussian.png", bytes);
    //    Imgproc.threshold(img2Mat, img2Mat, 240, 255, Imgproc.THRESH_BINARY);
    //    tex4 = new Texture2D(img2Mat.cols(), img2Mat.rows(), TextureFormat.RGBA32, false);
    //    Utils.matToTexture2D(img2Mat, tex4);
    //    bytes = tex4.EncodeToJPG();
    //    File.WriteAllBytes("imagem2_tratamento_threshold.png", bytes);

    //    ORB detector = ORB.create();
    //    ORB extractor = ORB.create();

    //    MatOfKeyPoint keypoints1 = new MatOfKeyPoint();
    //    Mat descriptors1 = new Mat();

    //    detector.detect(img1Mat, keypoints1);
    //    extractor.compute(img1Mat, keypoints1, descriptors1);

    //    MatOfKeyPoint keypoints2 = new MatOfKeyPoint();
    //    Mat descriptors2 = new Mat();

    //    detector.detect(img2Mat, keypoints2);
    //    extractor.compute(img2Mat, keypoints2, descriptors2);

    //    DescriptorMatcher matcher = DescriptorMatcher.create(DescriptorMatcher.BRUTEFORCE_HAMMINGLUT);
    //    MatOfDMatch matches = new MatOfDMatch();
    //    matcher.match(descriptors1, descriptors2, matches);

    //    List<MatOfDMatch> lista = new List<MatOfDMatch>();
    //    lista.Add(matches);

    //    matcher.knnMatch(descriptors1, descriptors2, lista, 2);

    //    long total = 0;

    //    foreach (MatOfDMatch item in lista)
    //    {
    //        if (item.toList()[0].distance < 0.9 * item.toList()[1].distance)
    //        {
    //            total++;
    //        }
    //    }

    //    long number_keypoints = 0;
    //    if (keypoints1.elemSize() <= keypoints2.elemSize())
    //        number_keypoints = keypoints1.elemSize();
    //    else
    //        number_keypoints = keypoints2.elemSize();

    //    Debug.Log(total / number_keypoints * 100);

    //    return (total / number_keypoints * 100) >= 270;
    //}
    public bool verificaImagem(Texture2D textParam, Texture2D textParam2)
    {
        var bytes = textParam.EncodeToJPG();
        File.WriteAllBytes("imagem1_tratamento.png", bytes);

        Texture2D camFoto = textParam;

        // Escala de cinza. CV_8UC1
        Mat img1Mat = new Mat(camFoto.height, camFoto.width, CvType.CV_8UC1);
        Utils.texture2DToMat(camFoto, img1Mat);

        Imgproc.GaussianBlur(img1Mat, img1Mat, new Size(5, 5), 0);
        Texture2D tex3 = new Texture2D(img1Mat.cols(), img1Mat.rows(), TextureFormat.RGBA32, false);
        Utils.matToTexture2D(img1Mat, tex3);
        bytes = tex3.EncodeToJPG();
        File.WriteAllBytes("imagem1_tratamento_gaussian.png", bytes);
        Imgproc.threshold(img1Mat, img1Mat, 100, 255, Imgproc.THRESH_BINARY);
        tex3 = new Texture2D(img1Mat.cols(), img1Mat.rows(), TextureFormat.RGBA32, false);
        Utils.matToTexture2D(img1Mat, tex3);
        bytes = tex3.EncodeToJPG();
        File.WriteAllBytes("imagem1_tratamento_threshold.png", bytes);

        List<MatOfPoint> srcContours = new List<MatOfPoint>();
        Mat srcHierarchy = new Mat();

        Imgproc.findContours(img1Mat, srcContours, srcHierarchy, Imgproc.RETR_TREE, Imgproc.CHAIN_APPROX_SIMPLE);

        int totalB = 0, totalQ = 0, totalR = 0, totalP;

        for (int i = 0; i < srcContours.Count; i++)
        {

            var peri = CvInvoke.ArcLength(contours[x], true);
            //    var epsilon = 0.01 * peri;
            //    var aprox = new VectorOfPoint();
            //    CvInvoke.ApproxPolyDP(contours[x], aprox, epsilon, true);

            double returnVal = Imgproc.matchShapes(srcContours[1], srcContours[i], Imgproc.CV_CONTOURS_MATCH_I1, 0);
            Debug.Log("returnVal " + i + " " + returnVal);

            Point point = new Point();
            float[] radius = new float[1];
            Imgproc.minEnclosingCircle(new MatOfPoint2f(srcContours[i].toArray()), point, radius);
            Debug.Log("point.ToString() " + point.ToString());
            Debug.Log("radius.ToString() " + radius[0]);

            Imgproc.circle(dstMat, point, 5, new Scalar(0, 0, 255), -1);
            Imgproc.putText(dstMat, " " + returnVal, point, Imgproc.FONT_HERSHEY_SIMPLEX, 0.4, new Scalar(0, 255, 0), 1, Imgproc.LINE_AA, false);
        }

        return false;
    }
}
