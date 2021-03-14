using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.IO;
using System.Collections.Generic;

public class FileManager : MonoBehaviour
{
    private int nowPage = 0;	// 今のページ
	private string[] filename;  // ファイルたち

	public string path = @"H:\共有\エロ漫画\##未使用##\(同人CG集) [Master-random] トロピカルワイフ～出張先の南国で人妻エッチ!～\";    // パス

	public List<GameObject>	imageList;		// 画像リスト
	public GameObject		imagePlefab;    // イメージ作成用のプレハブ

	public GameObject mainObj;

	public Vector2 pos;						// 画像の位置
    void Start()
    {
		//try
		//{
			filename = Directory.GetFiles(path, "*", SearchOption.AllDirectories);
			//foreach (string f in filename)
			//{
			//	Debug.Log(f);
			//}
		//}
		//catch (Exception e)
		//{
		//	Console.WriteLine(e.Message);
		//}

		// 自分のデータ取得
		imageList = new List<GameObject>();

		pos = mainObj.transform.position;
	}

	void Update()
    {
		if (nowPage < filename.Length)
		{
			// オブジェクト作成
			GameObject obj = GameObject.Instantiate(imagePlefab, pos, Quaternion.identity);

			Debug.Log(pos);
			Debug.Log(obj.transform.position);
			Debug.Log(mainObj.transform.position);

			// 画像作成
			Image image = obj.GetComponent<Image>();
			Texture2D texture2D = GetTexture(filename[nowPage]);
			image.sprite = Sprite.Create(texture2D, new Rect(0, 0, texture2D.width, texture2D.height), Vector2.zero);

			// 画像オブジェのサイズ設定
			RectTransform rectTransform = obj.GetComponent<RectTransform>();
			rectTransform.sizeDelta = new Vector2(texture2D.width, texture2D.height);

			// 自分の子供に設定
			obj.transform.parent = mainObj.transform;

			// リストに追加
			imageList.Add(obj);

			// 次の準備
			pos.y -= texture2D.height - 10;

			nowPage++;
		}
	}

	public byte[] readPngFile(string path)
	{
		using (FileStream fileStream = new FileStream(path, FileMode.Open, FileAccess.Read))
		{
			BinaryReader bin = new BinaryReader(fileStream);
			byte[] values = bin.ReadBytes((int)bin.BaseStream.Length);
			bin.Close();
			return values;
		}
	}

	public Texture2D readByBinary(byte[] bytes)
	{
		Texture2D texture = new Texture2D(1, 1);
		texture.LoadImage(bytes);
		return texture;
	}

	public Texture2D GetTexture(string path)
	{
		return readByBinary(readPngFile(path));
	}
}
