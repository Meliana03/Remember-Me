using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundEfect : MonoBehaviour
{
    public AudioSource mySound;
    public AudioClip hoverSound;
    public AudioClip clickSound;

    public void HoverSound()
    {
        mySound.PlayOneShot(hoverSound);
    }

    public void ClickSound()
    {
        mySound.PlayOneShot(clickSound);
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
