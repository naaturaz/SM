using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class GetTextURL : MonoBehaviour
{
    public Text Text;
    void Start()
    {
        StartCoroutine(GetText());
    }

    IEnumerator GetText()
    {
        UnityWebRequest www = UnityWebRequest.Get("https://www.sugarwinds.com/clientFiles/global.txt");
        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log(www.error);
        }
        else
        {
            // Show results as text
            Debug.Log(www.downloadHandler.text);
            Text.text = www.downloadHandler.text;

            // Or retrieve results as binary data
            byte[] results = www.downloadHandler.data;
        }
    }
}
