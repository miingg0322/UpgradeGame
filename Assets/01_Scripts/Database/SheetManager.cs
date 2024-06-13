using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.Linq;
using System;

public enum UserSchema
{
    ID = 1, PW, Nickname, Value
}
public enum CharSchema
{
    SLOT = 2, JOB, CLEAR, TUTORIAL = 6, SCORE, UPGRADE
}
public class SheetResponse
{
    string order;
    public bool result;
    public string msg;
    public string[] data;
    public int count;
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
public class CharacterData
{
    public int slot;
    public int job;
    public int clear;
    public string created;
    public bool tutorial;
    public int score;
    public int upgrade;

    public CharacterData(int slot, int job, int clear, string created, int tutorial, int score , int upgrade)
    {
        this.slot = slot;
        this.job = job;
        this.clear = clear;
        this.created = created;
        if (tutorial == 1)
            this.tutorial = true;
        else
            this.tutorial = false;
        this.score = score;
        this.upgrade = upgrade;
    }
    public CharacterData(string[] data)
    {
        slot = int.Parse(data[1]);
        job = int.Parse(data[2]);
        clear = int.Parse(data[3]);
        created = data[4];
        if (int.Parse(data[5]) == 1)
            tutorial = true;
        else
            tutorial = false;
        score = int.Parse(data[6]);
        upgrade = int.Parse(data[7]);
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
    readonly string url = "https://script.google.com/macros/s/AKfycby5BjbG1ikG64zJ2ENrCSL4V3YuQ7v5FR2zKvVBY91PZ3lzboTkP4aUFdQfOAZGkVd_0Q/exec";
    private SheetResponse response;
    public UserData user;
    public List<CharacterData> characterDatas = new List<CharacterData>();
    public CharacterData playingCharacter;
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
        Debug.Log("POST");
        using (UnityWebRequest www = UnityWebRequest.Post(url, form))
        {
            yield return www.SendWebRequest();

            if (www.isDone)
            {
                string result = www.downloadHandler.text;
                response = JsonUtility.FromJson<SheetResponse>(result);
                //if (response.data != null)
                //{
                //    Debug.Log($"{response.result}, {response.msg}");
                //    Debug.Log(response.data[0]);
                //    foreach (var data in response.data)
                //    {
                //        Debug.Log(data);
                //    }
                //}
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
            yield return StartCoroutine(GetUserCharacters());
            GameManager.Instance.loginUi.Login();
        }
        else
        {
            GameManager.Instance.loginUi.ActiveLoginFail();
        }

    }

    public void SetValue(int col, string value, int sheet = 0, int slot = -1)
    {
        WWWForm form = new WWWForm();
        form.AddField("order", "setValue");
        form.AddField("col", col);
        form.AddField("value", value);
        form.AddField("sheet", sheet);
        form.AddField("slot", slot);
        StartCoroutine(Post(form));
    }
    public void SetValue(int col, int value, int sheet = 0, int slot = -1)
    {
        WWWForm form = new WWWForm();
        form.AddField("order", "setValue");
        form.AddField("col", col);
        form.AddField("value", value);
        form.AddField("sheet", sheet);
        form.AddField("slot", slot);
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

    private IEnumerator GetUserCharacters()
    {
        WWWForm form = new WWWForm();
        form.AddField("order", "getCharacter");
        form.AddField("id", user.id);
        yield return StartCoroutine(Post(form));
        int[] slots = { -1, -1, -1};
        if(response.count > 0)
        {
            //Debug.Log(response.count);
            //Debug.Log(response.data.Length);
            int len = response.data.Length / response.count;
            string[,] splitData = new string[response.count,len];

            for (int i = 0; i < response.count; i++)
            {
                Debug.Log($"{len * i} ::: {len * (i + 1)}");
                for (int j = len * i; j < len * (i + 1); j++)
                {
                    //Debug.Log($"{i}, {j}");
                    splitData[i,j%len] = response.data[j];
                    //Debug.Log($"i:{i}, j:{j}, data: {splitData[i, j%len]}");
                }
            }
            //Debug.Log(splitData.Length);
            for (int i = 0; i < response.count; i++)
            {
                int slotIndex = int.Parse(splitData[i,1]);
                int job = int.Parse(splitData[i,2]);
                slots[slotIndex] = job;
                string[] data = new string[len];
                for (int j = 0; j < len; j++)
                {
                    data[j] = splitData[i, j];
                }
                CharacterData chData = new CharacterData(data);
                characterDatas.Add(chData);
            }
        }

        GameManager.Instance.SetUserSlots(slots);
    }

    public void CreateCharacter(int slot, int job)
    {
        WWWForm form = new WWWForm();
        form.AddField("order", "create");
        form.AddField("slot", slot);
        form.AddField("job", job);
        form.AddField("created", DateTime.Now.ToString());
        StartCoroutine(Post(form));
    }

    public void DeleteCharacter(int slot)
    {
        Debug.Log("Delete");
        WWWForm form = new WWWForm();
        form.AddField("order", "delete");
        form.AddField("slot", slot);
        StartCoroutine(Post(form));
    }

    public void TutorialClear()
    {
        characterDatas[GameManager.Instance.selectIndex].tutorial = true;
        SetValue((int)CharSchema.TUTORIAL, 1, 1, characterDatas[GameManager.Instance.selectIndex].slot);
    }
}