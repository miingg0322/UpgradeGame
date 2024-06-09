using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;
    public static AudioManager Instance
    {
        get { return instance; }
        set { instance = value; }
    }

    [Header("#BGM")]
    public AudioClip bgmClip;
    public float bgmVolume;
    AudioSource bgmPlayer;
    AudioHighPassFilter bgmEffect;

    [Header("#GAME SFX")]
    public AudioClip[] gameSfxClips;
    public float gameSfxVolume;
    public int gameChannels;
    AudioSource[] gameSfxPlayers;
    int gameChannelIndex;

    [Header("#SKILL SFX")]
    public AudioClip[] skillSfxClips;
    public float skillSfxVolume;
    public int skillChannels;
    AudioSource[] skillSfxPlayers;
    int skillChannelIndex;

    [Header("#UI SFX")]
    public AudioClip[] uiSfxClips;
    public float uiSfxVolume;
    public int uiChannels;
    AudioSource[] uiSfxPlayers;
    int uiChannelIndex;

    public enum GameSfx { getItem, hitEnemy, playerAttack }
    public enum SkillSfx { victory , lackTicket , die }
    public enum UISfx { uiList, dungeonList, characterInfo }
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        Init();
    }

    void Init()
    {
        bgmVolume = PlayerPrefs.GetInt("bgmVolumeValue", 70) / 100f;
        gameSfxVolume = PlayerPrefs.GetInt("gameVolumeValue", 70) / 100f;
        skillSfxVolume = PlayerPrefs.GetInt("skillVolumeValue", 70) / 100f;
        uiSfxVolume = PlayerPrefs.GetInt("uiVolumeValue", 70) / 100f;

        GameObject bgmObject = new GameObject("BgmPlayer");
        bgmObject.transform.parent = transform;
        bgmPlayer = bgmObject.AddComponent<AudioSource>();
        bgmPlayer.playOnAwake = false;
        bgmPlayer.loop = true;
        bgmPlayer.volume = bgmVolume;
        bgmPlayer.clip = bgmClip;
        bgmEffect = Camera.main.GetComponent<AudioHighPassFilter>();

        // Game Sfx 초기화
        GameObject gameSfxObject = new GameObject("GameSfxPlayer");
        gameSfxObject.transform.parent = transform;
        gameSfxPlayers = new AudioSource[gameChannels];

        for (int i = 0; i < gameChannels; i++)
        {
            gameSfxPlayers[i] = gameSfxObject.AddComponent<AudioSource>();
            gameSfxPlayers[i].playOnAwake = false;
            gameSfxPlayers[i].bypassListenerEffects = true;
            gameSfxPlayers[i].volume = gameSfxVolume;
        }

        // Skill Sfx 초기화
        GameObject skillSfxObject = new GameObject("SkillSfxPlayer");
        skillSfxObject.transform.parent = transform;
        skillSfxPlayers = new AudioSource[skillChannels];

        for (int i = 0; i < skillChannels; i++)
        {
            skillSfxPlayers[i] = skillSfxObject.AddComponent<AudioSource>();
            skillSfxPlayers[i].playOnAwake = false;
            skillSfxPlayers[i].bypassListenerEffects = true;
            skillSfxPlayers[i].volume = skillSfxVolume;
        }

        // UI Sfx 초기화
        GameObject UiSfxObject = new GameObject("UiSfxPlayer");
        UiSfxObject.transform.parent = transform;
        uiSfxPlayers = new AudioSource[uiChannels];

        for (int i = 0; i < gameChannels; i++)
        {
            uiSfxPlayers[i] = UiSfxObject.AddComponent<AudioSource>();
            uiSfxPlayers[i].playOnAwake = false;
            uiSfxPlayers[i].bypassListenerEffects = true;
            uiSfxPlayers[i].volume = uiSfxVolume;
        }
    }
    public void PlayBgm(bool isPlay)
    {
        if (isPlay)
        {
            bgmPlayer.Play();
        }
        else
        {
            bgmPlayer.Stop();
        }
    }

    public void EffectBgm(bool isPlay)
    {
        bgmEffect.enabled = isPlay;
    }
    public void PlayGameSfx(GameSfx sfx)
    {
        for (int i = 0; i < gameChannels; i++)
        {
            int loopIndex = (i + gameChannelIndex) % gameChannels;

            if (gameSfxPlayers[loopIndex].isPlaying)
                continue;

            int ranIndex = 0;

            gameChannelIndex = loopIndex;
            gameSfxPlayers[loopIndex].clip = gameSfxClips[(int)sfx + ranIndex];
            gameSfxPlayers[loopIndex].Play();
            break;
        }
    }
    public void PlaySkillSfx(SkillSfx sfx)
    {
        for (int i = 0; i < skillChannels; i++)
        {
            int loopIndex = (i + skillChannelIndex) % skillChannels;

            if (skillSfxPlayers[loopIndex].isPlaying)
                continue;

            int ranIndex = 0;

            skillChannelIndex = loopIndex;
            skillSfxPlayers[loopIndex].clip = skillSfxClips[(int)sfx + ranIndex];
            skillSfxPlayers[loopIndex].Play();
            break;
        }
    }
    public void PlayUISfx(UISfx sfx)
    {
        for (int i = 0; i < uiChannels; i++)
        {
            int loopIndex = (i + uiChannelIndex) % uiChannels;

            if (uiSfxPlayers[loopIndex].isPlaying)
                continue;

            int ranIndex = 0;

            uiChannelIndex = loopIndex;
            uiSfxPlayers[loopIndex].clip = uiSfxClips[(int)sfx + ranIndex];
            uiSfxPlayers[loopIndex].Play();
            break;
        }
    }

    public void BGMVolumeSetting(float volume)
    {
        bgmPlayer.volume = volume;
    }
    public void GameVolumeSetting(float volume)
    {
        for(int index = 0; index < gameChannels; index++)
        {
            gameSfxPlayers[index].volume = volume;
        }       
    }
    public void SkillVolumeSetting(float volume)
    {
        for (int index = 0; index < skillChannels; index++)
        {
            skillSfxPlayers[index].volume = volume;
        }
    }
    public void UIVolumeSetting(float volume)
    {
        for (int index = 0; index < uiChannels; index++)
        {
            uiSfxPlayers[index].volume = volume;
        }
    }
}
