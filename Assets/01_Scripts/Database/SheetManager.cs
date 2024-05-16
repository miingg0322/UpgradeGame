using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class SheetManager : MonoBehaviour
{
    private static SheetManager instance;
    public static SheetManager Instance
    {
        get { return instance; }
        private set { }
    }
    readonly string url = "https://script.google.com/macros/s/AKfycby_XCc9SupwLXy3q66CJJBZNA-jN_grI3bXTgcsIq7ggvErq97F7FJVbilEtZZ6YGUmCg/exec";
    private void Awake()
    {
        if(Instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    void Start()
    {

    }

   
    IEnumerator GetTest()
    {
        UnityWebRequest www = UnityWebRequest.Get(url);
        yield return www.SendWebRequest();

        string data = www.downloadHandler.text;
        Debug.Log(data);
    }

    IEnumerator PostTest()
    {
        WWWForm form = new WWWForm();
        form.AddField("value", "°ª");
        form.AddField("value2", "°ª2");

        UnityWebRequest www = UnityWebRequest.Post(url, form);
        yield return www.SendWebRequest();

        string data = www.downloadHandler.text;
        Debug.Log(data);
    }

    IEnumerator Post(WWWForm form)
    {
        using (UnityWebRequest www = UnityWebRequest.Post(url, form))
        {
            yield return www.SendWebRequest();

            if (www.isDone)
                Debug.Log(www.downloadHandler.text);
            else
                Debug.Log("Error");
        }
    }

    public void Register(string id, string password, string nickName)
    {
        WWWForm form = new WWWForm();
        form.AddField("order", "register");
        form.AddField("id", id);
        form.AddField("pw", password);
        form.AddField("nickname", nickName);
        StartCoroutine(Post(form));
    }

    public void Login(string id, string password)
    {
        WWWForm form = new WWWForm();
        form.AddField("order", "login");
        form.AddField("id", id);
        form.AddField("pw", password);
        StartCoroutine(Post(form));
    }
}
