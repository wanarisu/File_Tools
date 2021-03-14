using System.Collections;
using System.Net;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;

public class API_test : MonoBehaviour
{
    //接続するURL
    private const string URL = @"https://192.168.0.219/test_img.png";
    //private const string URL = @"https://192.168.0.219/Tools/index.php/api/test/test";
    //private const string URL = @"http://cdn-www.dailypuppy.com/media/dogs/anonymous/coffee_poodle01.jpg_w450.jpg";
    public Text text;

    //ゲームオブジェクトUI > ButtonのInspector > On Click()から呼び出すメソッド
    public void OnClick()
    {
        //コルーチンを呼び出す
        StartCoroutine("GET", URL+ "?0=100");
        StartCoroutine("POST", URL);
    }

    //コルーチン
    IEnumerator GET(string url)
    {
        //URLをGETで用意
        UnityWebRequest webRequest = UnityWebRequest.Get(url);
        webRequest.certificateHandler = new AcceptAllCertificatesSignedWithASpecificKeyPublicKey();//追加
        //URLに接続して結果が戻ってくるまで待機
        yield return webRequest.SendWebRequest();

        //エラーが出ていないかチェック
        if (webRequest.isNetworkError)
        {
            text.text = webRequest.error;
            //通信失敗
            Debug.Log(webRequest.error);
        }
        else
        {
            text.text = webRequest.downloadHandler.text;

            //通信成功
            Debug.Log(webRequest.downloadHandler.text);
        }
    }

    //コルーチン
    IEnumerator POST(string url)
    {
        //POSTする情報
        WWWForm form = new WWWForm();
        form.AddField("zipcode", 1000001);

        //URLをPOSTで用意
        UnityWebRequest webRequest = UnityWebRequest.Post(url, form);
        //UnityWebRequestにバッファをセット
        webRequest.downloadHandler = new DownloadHandlerBuffer();
        //URLに接続して結果が戻ってくるまで待機
        yield return webRequest.SendWebRequest();

        //エラーが出ていないかチェック
        if (webRequest.isNetworkError)
        {
            text.text += webRequest.error;
            //通信失敗
            Debug.Log(webRequest.error);
        }
        else
        {
            text.text += webRequest.downloadHandler.text;

            //通信成功
            Debug.Log(webRequest.downloadHandler.text);
        }
    }
}



class AcceptAllCertificatesSignedWithASpecificKeyPublicKey : CertificateHandler
{
    // Encoded RSAPublicKey
    private static string PUB_KEY = "30818902818100C4A06B7B52F8D17DC1CCB47362" +
        "C64AB799AAE19E245A7559E9CEEC7D8AA4DF07CB0B21FDFD763C63A313A668FE9D764E" +
        "D913C51A676788DB62AF624F422C2F112C1316922AA5D37823CD9F43D1FC54513D14B2" +
        "9E36991F08A042C42EAAEEE5FE8E2CB10167174A359CEBF6FACC2C9CA933AD403137EE" +
        "2C3F4CBED9460129C72B0203010001";
    protected override bool ValidateCertificate(byte[] certificateData)
    {
        X509Certificate2 certificate = new X509Certificate2(certificateData);
        string pk = certificate.GetPublicKeyString();
        if (pk.Equals(PUB_KEY))//公開鍵があっているか検証
            return true;
        // Bad dog
        return false;
    }
}