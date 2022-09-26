using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TutorialScript : MonoBehaviour
{
    public int clicked = 1;
    public Image oldImage;
    public Sprite newImage1;
    public Sprite newImage2;
    public Sprite newImage3;
    public Sprite newImage4;
    public Sprite newImage5;
    public Sprite newImage6;

    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        if(clicked == 7)
        {
            if(Input.GetKey(KeyCode.Mouse0))
            {
                SceneManager.LoadScene(sceneName: "SampleScene");
            }
        }   
    }

    public void ImageChange()
    {
        if(clicked == 1)
        {
            oldImage.sprite = newImage1;
            clicked = 2;
        }
        else if(clicked == 2)
        {
            oldImage.sprite = newImage2;
            clicked = 3;
        }
        else if(clicked == 3)
        {
            oldImage.sprite = newImage3;
            clicked = 4;
        }
        else if(clicked == 4)
        {
            oldImage.sprite = newImage4;
            clicked = 5;
        }
        else if(clicked == 5)
        {
            oldImage.sprite = newImage5;
            clicked = 6;
        }
        else if(clicked == 6)
        {
            oldImage.sprite = newImage6;
            clicked = 7;
        }

    }
}
