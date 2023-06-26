using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


/*
노래 재생 스크립트

 */
public class MusicPlayer : MonoBehaviour
{

    public AudioClip niaMusic;
    public AudioClip Mokoko;
    public AudioClip LiebenHeim;
    public AudioClip buttonSound;
    AudioSource musicSource;


    //열거형 노래 재생 리스트
    public enum EnumMusicList
    {
        NiaViliage = 1,
        Mokoko,
        Liebenheim,
    }

    EnumMusicList enumMusicList;


    void Awake()
    {
   
        musicSource = GetComponent<AudioSource>();
        enumMusicList = EnumMusicList.NiaViliage;
        musicSource.clip = niaMusic;
        musicSource.loop = true;
    }


    void Start()
    {
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

    //씬 제목에 따라 노래 교체
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
