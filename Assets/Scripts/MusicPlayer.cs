using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


/*
게임 배경음악 및 버튼 효과음을 재생해준다.

 */
public class MusicPlayer : MonoBehaviour
{

    public AudioClip niaMusic;
    public AudioClip Mokoko;
    public AudioClip LiebenHeim;
    public AudioClip buttonSound;
    public AudioSource musicSource;


    //음악의 종류를 나타내는 열거형
    public enum EnumMusicList
    {
        NiaViliage = 1,
        Mokoko,
        Liebenheim,
    }

    EnumMusicList enumMusicList;


    void Awake()
    {
        // 오브젝트 유지
        DontDestroyOnLoad(this.gameObject);
        
        musicSource = GetComponent<AudioSource>();
        enumMusicList = EnumMusicList.NiaViliage;
        musicSource.clip = niaMusic;
        musicSource.loop = true;
    }


    void Start()
    {
        //씬에 따라 다른 음악 사용
        GameObject[] gameObjects = GameObject.FindGameObjectsWithTag("MusicPlayer");


        if (gameObjects.Length > 1)
        {
            Destroy(this.gameObject);
        }
        else
            musicSource.Play();

    }

    // Update is called once per frame
    void Update()
    {

    }


    public void PlayBtnSound()
    {
        musicSource.PlayOneShot(buttonSound);
    }

    public void UpdateMusic(EnumMusicList musicList)
    {
        switch(musicList)
        {
            case EnumMusicList.NiaViliage:
                if (musicSource.clip == niaMusic)
                    break;
                enumMusicList = EnumMusicList.NiaViliage;
                musicSource.clip = niaMusic;
                musicSource.Play();
                break;

            case EnumMusicList.Mokoko:
                if (musicSource.clip == Mokoko)
                    break;
                enumMusicList = EnumMusicList.Mokoko;
                musicSource.clip = Mokoko;
                musicSource.Play();
                break;

            case EnumMusicList.Liebenheim:
                if (musicSource.clip == LiebenHeim)
                    break;
                enumMusicList = EnumMusicList.Liebenheim;
                musicSource.clip = LiebenHeim;
                musicSource.Play();
                break;
        }
    }
}
