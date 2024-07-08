using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System;

public class SheetManager : MonoBehaviour
{
    private static SheetManager instance;
    public static SheetManager Instance
    {
        get { return instance; }
        private set { }
    }
    readonly string url = "https://script.google.com/macros/s/AKfycbxQfKuzApgi5AW-eue4vcZHadYqhU6jmb12dTcjRuTZp6HWUEbxsClxALEkkSbplO5RJw/exec";
    private SheetResponse response;
    public UserData user;
    public List<CharacterData> characterDatas = new List<CharacterData>();
    public CharacterData playingCharacter;
    public ItemList itemList;
    public WeightedRandom wRandom;
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
        itemList = GetComponent<ItemList>();
        wRandom = GetComponent<WeightedRandom>();
        StartCoroutine(GetItemList());
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
        characterDatas.Clear();
        if (response.count > 0)
        {
            string[][] parsed = response.ParseData();
            //Debug.Log(parsed.Length);
            for (int i = 0; i < parsed.Length; i++)
            {
                int slotIndex = int.Parse(parsed[i][1]);
                int job = int.Parse(parsed[i][2]);
                slots[slotIndex] = job;
                int len = parsed[i].Length;
                string[] data = new string[len];
                for (int j = 0; j < len; j++)
                {
                    data[j] = parsed[i][j];
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
        characterDatas.Remove(characterDatas[slot]);
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

    private IEnumerator GetItemList()
    {
        WWWForm form = new WWWForm();
        form.AddField("order", "getItem");
        yield return StartCoroutine(Post(form));
        string[][] data = response.ParseData();
        itemList.SortItem(data);
        yield return StartCoroutine(GetItemConst());
    }

    private IEnumerator GetItemConst()
    {
        WWWForm form = new WWWForm();
        form.AddField("order", "getItemConst");
        yield return StartCoroutine(Post(form));
        itemList.maxType = int.Parse(response.data[0]);
        itemList.maxGrade = int.Parse(response.data[1]);
        Debug.Log($"Get Const::: {itemList.maxType}, {itemList.maxGrade}");
        itemList.InitLists();
        wRandom.SetRandom();
        wRandom.RandomPickTest(1000);
    }

}