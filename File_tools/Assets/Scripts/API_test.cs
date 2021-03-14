using System.Collections;
using System.Net;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using System.Collections.Generic;

public class API_test : MonoBehaviour
{
	//接続するURL
	//private const string URL = @"https://192.168.0.219/test_img.png";
	private const string URL = @"http://192.168.0.219:8080/Tools/api/test/post_test";
	//private const string URL = @"http://cdn-www.dailypuppy.com/media/dogs/anonymous/coffee_poodle01.jpg_w450.jpg";
	public Text text;

    //ゲームオブジェクトUI > ButtonのInspector > On Click()から呼び出すメソッド
    public void OnClick()
    {
		//コルーチンを呼び出す
		StartCoroutine("POST", URL);
		StartCoroutine("GET", "http://192.168.0.219:8080/Tools/api/test/get_test?0=100");
    }

    private IEnumerator GET(string url)
    {
        using (var webRequest = UnityWebRequest.Get(url))
        {
            webRequest.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
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
    }

    //コルーチン
    IEnumerator POST(string url)
    {
        WWWForm form = new WWWForm();
        form.AddField("zipcode", 1000001);

        UnityWebRequest webRequest = UnityWebRequest.Post(url, form);

        //UnityWebRequestにバッファをセット
        webRequest.downloadHandler = new DownloadHandlerBuffer();
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
}
