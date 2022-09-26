using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

class StoryBlock
{
    public string story;
    public string nama;

    public StoryBlock(string story, string nama)
    {
        this.story = story;
        this.nama = nama;
    }
}

public class StoryScene : MonoBehaviour
{
    public Text mainText;
    public Text name;
    public int clicked = 1;

    StoryBlock currentBlock;

    StoryBlock block1 = new StoryBlock("Sok banget sih ni orang.", "Player");
    StoryBlock block2 = new StoryBlock("Kamu bilang aku sok emangnya kamu bisa apa? Jangan-jangan kamu iri makanya gitu.Sini aku ajarin biar kamu makin pintar.", "NPC");
    StoryBlock block3 = new StoryBlock("Paling kamu cuma bisa ngomong doang.", "Player");
    StoryBlock block4 = new StoryBlock("Ayo duel quiz! Biar ku buktikan kehebatan ku.", "NPC");
    StoryBlock block5 = new StoryBlock("Siapa takut.", "Player");

    // Start is called before the first frame update
    void Start()
    {
        //DisplayBlock(block1);
    }

    void DisplayBlock(StoryBlock block)
    {
        mainText.text = block.story;
        name.text = block.nama;

        currentBlock = block;
    }

    public void ChangeText()
    {    
        if(clicked == 1)
        {
            DisplayBlock(block1);
            clicked = 2;
        }
        else if(clicked == 2)
        {
            DisplayBlock(block2);
            clicked = 3;
        }
        else if(clicked == 3)
        {
            DisplayBlock(block3);
            clicked = 4;
        }
        else if(clicked == 4)
        {
            DisplayBlock(block4);
            clicked = 5;
        }
        else if(clicked == 5)
        {
            DisplayBlock(block5);
            clicked = 6;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(clicked == 6)
        {
            if(Input.GetKey(KeyCode.Mouse0))
            {
                SceneManager.LoadScene(sceneName: "TutorialScene");
            }   
        }
    }
}
