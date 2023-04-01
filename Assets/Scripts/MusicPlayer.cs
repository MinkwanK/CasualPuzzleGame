using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MusicPlayer : MonoBehaviour
{

    public AudioClip musicClip;
    public AudioClip buttonSound;
    private AudioSource musicSource;


    void Awake()
    {
        // 오브젝트 유지
        DontDestroyOnLoad(this.gameObject);

        musicSource = GetComponent<AudioSource>();
        musicSource.clip = musicClip;
        musicSource.loop = true;
    }


    void Start()
    {

        if (GameObject.FindGameObjectsWithTag("MusicPlayer").Length > 1)
            Destroy(this.gameObject);
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
}
