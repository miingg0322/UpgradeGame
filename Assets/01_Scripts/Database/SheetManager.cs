using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public enum UserSchema
{
    ID = 1, PW, Nickname, Value
}
public class SheetResponse
{
    string order;
    public bool result;
    public string msg;
    public string[] data;
}
public class UserData
{
    public string id;
    public string nickname;
    public UserData(string[] data)
    {
        id = data[(int)UserSchema.ID - 1];
        nickname = data[(int)UserSchema.Nickname - 1];
    }
}

public class SheetManager : MonoBehaviour
{
    private static SheetManager instance;
    public static SheetManager Instance
    {
        get { return instance; }
        private set { }
    }
    readonly string url = "https://script.google.com/macros/s/AKfycbyvUwKgYuYj0Bs1gAoJoz97LhOf0I5vq6mgAj5s56ogpWi1vemgDjPCQkeQelY-NWMz/exec";
    private SheetResponse response;
    public UserData user;

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

    IEnumerator Post(WWWForm form)
    {
        using (UnityWebRequest www = UnityWebRequest.Post(url, form))
        {
            yield return www.SendWebRequest();

            if (www.isDone)
            {
                string result = www.downloadHandler.text;
                response = JsonUtility.FromJson<SheetResponse>(result);
                if (response.data != null)
                {
                    Debug.Log($"{response.result}, {response.data.Length}");
                }
                Debug.Log(result);
            }

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
        StartCoroutine(LoginCoroutine(id, password));
    }

    IEnumerator LoginCoroutine(string id, string password)
    {
        WWWForm form = new WWWForm();
        form.AddField("order", "login");
        form.AddField("id", id);
        form.AddField("pw", password);
        yield return StartCoroutine(Post(form));

        // 로그인 성공 여부
        if (response.result)
        {
            user = new UserData(response.data);
            Debug.Log($"{user.id}, {user.nickname} Login");
        }
        else
        {
            //GameManager.Instance.ActiveLoginFail();
        }

    }

    public void SetValue(int col, string value)
    {
        WWWForm form = new WWWForm();
        form.AddField("order", "setValue");
        form.AddField("col", col);
        form.AddField("value", value);

        StartCoroutine(Post(form));
    }

    public void SetValueTest()
    {
        SetValue((int)UserSchema.Value, "SetValue Test");
    }

    public void GetValue(int col, string value)
    {
        WWWForm form = new WWWForm();
        form.AddField("order", "getValue");
        form.AddField("col", value);
        StartCoroutine(Post(form));
    }

    public void GetUserData()
    {
        WWWForm form = new WWWForm();
        form.AddField("order", "getUserData");
        StartCoroutine(Post(form));
    }
}
