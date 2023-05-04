using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


/*
���� ������� �� ��ư ȿ������ ������ش�.

 */
public class MusicPlayer : MonoBehaviour
{

    public AudioClip niaMusic;
    public AudioClip Mokoko;
    public AudioClip LiebenHeim;
    public AudioClip buttonSound;
    public AudioSource musicSource;


    //������ ������ ��Ÿ���� ������
    public enum EnumMusicList
    {
        NiaViliage = 1,
        Mokoko,
        Liebenheim,
    }

    EnumMusicList enumMusicList;


    void Awake()
    {
        // ������Ʈ ����
        DontDestroyOnLoad(this.gameObject);
        
        musicSource = GetComponent<AudioSource>();
        enumMusicList = EnumMusicList.NiaViliage;
        musicSource.clip = niaMusic;
        musicSource.loop = true;
    }


    void Start()
    {
        //���� ���� �ٸ� ���� ���
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
