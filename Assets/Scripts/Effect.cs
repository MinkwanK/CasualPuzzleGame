using System.Collections;
using System.Collections.Generic;
using UnityEngine;



//사운드 효과를 관리하는 스크립트
public class Effect : MonoBehaviour
{
    public AudioClip MoveSoundClip;
    public AudioClip DestroySoundClip;
    private AudioSource effectSource;

    public GameObject explosion;


    // Start is called before the first frame update
    void Start()
    {
        effectSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void DestroySound()
    {
        effectSource.PlayOneShot(DestroySoundClip);
    }
    public void MoveSound()
    {
        effectSource.clip = MoveSoundClip;
        effectSource.Play(); 
    }

    public void Explosion(Vector3 position,  Quaternion rot)
    {
       GameObject temp = GameObject.Instantiate(explosion, position, rot);
        StartCoroutine(DestroyExposion(temp));
        
    }

    IEnumerator DestroyExposion(GameObject temp)
    {
        yield return new WaitForSeconds(0.3f);

        GameObject.Destroy(temp);
    }


   
}
