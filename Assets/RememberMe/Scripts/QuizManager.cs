using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class QuizManager : MonoBehaviour
{
    public static QuizManager instance;
    [SerializeField]
    private GameObject popUp;
    [SerializeField]
    private QuizDataScriptable questionData;

//    [SerializeField]
//    private Image questionImage; //Buat gambar soal

    [SerializeField]
    private Text questionText;

    [SerializeField]
    private WordData[] answerWordArray;  //awserWordPrefab;
    [SerializeField]
    private WordData[] optionsWordArray;

    private char[] charArray = new char[12];  // 12 dri box answer
    private int currentAnswerIndex = 0;
    private bool correctAnswer = true;
    private List<int> selectedWordIndex;

    private int currentQuestionIndex = 0;   // index untuk memberitahu skrng di pertanyaan ke brp
    private GameStatus gameStatus = GameStatus.Playing;
    private string answerWord; // Jawaban dari soal yg ditampilkan
    private int score = 0;

    // Time
    float currentTime;
    public float timeRemaining = 300f;
    [SerializeField] Text countdownText;

    // Score / current correct answer
    [SerializeField] Text scoreText;
    float currentScore;
    float currentCorrectAnswer = 0f;

    //Pass
    public Text buttonText;
    float currentPass = 5f;
    public Button buttonPass;
    public int[] PassIndex = new int [5];
    public int countPassIndex = 0;
    public int countPassIndex2 = 0;

    //Clue
    public Text cluebuttonText;
    float currentClue = 5f;
    public Button canUseClue;
    public int[] clueIndex = new int [5];
    public int countIndex = 0;

    //PopUp
    private bool wrongAnswer = false;
    private float popUpCurrentTime = 0.0f;
    private float timeToWait = 3.0f;
 
    private void Awake()
    {
        if(instance == null) instance = this;
        else
            Destroy(this.gameObject);

        selectedWordIndex = new List<int>();

    }

    public void Start()
    {
        setQuestion();
        currentTime = timeRemaining;
        buttonPass.enabled = true;
        canUseClue.enabled = true;

        for(int i = 0; i < 5; i++)
        {
            PassIndex[i] = -999;
        }
        
    }

    public void Update()
    {
        currentTime -= 1 * Time.deltaTime;
        countdownText.text = currentTime.ToString("0");

        if(currentTime <= 0)
        {
            currentTime = 0;
            SceneManager.LoadScene(sceneName: "LoseScene");
        }

        if(currentCorrectAnswer == 15 && Time.timeScale == 1.0)
        {
            Time.timeScale = 0f;
            SceneManager.LoadScene(sceneName: "WinScene");
        }

        if(wrongAnswer == true)
        {
            popUpCurrentTime += Time.deltaTime;
            if(popUpCurrentTime >= timeToWait)
            {
                popUpCurrentTime = 0.0f;
                popUp.SetActive(false);
                wrongAnswer = false;
            }
            else
            {
                popUp.SetActive(true);
            }
        }
        else
        {
            popUp.SetActive(false);
        }
    }

    private void setQuestion()
    {
        currentAnswerIndex = 0;
        selectedWordIndex.Clear();

        //questionImage.sprite = questionData.questions[currentQuestionIndex].questionImage;
        questionText.text = questionData.questions[currentQuestionIndex].questionText;
        answerWord = questionData.questions[currentQuestionIndex].answer;

        ResetQuestion();

        for(int i=0; i < answerWord.Length; i++)
        {
            charArray[i] = char.ToUpper(answerWord[i]);
        }

        //Random Huruf sisa (yg gk dipakai)
        for(int i = answerWord.Length; i < optionsWordArray.Length; i++)
        {
            charArray[i] = (char)UnityEngine.Random.Range(65, 91);
        }

        //Mengacak huruf
        charArray = ShuffleList.ShuffleListItems<char>(charArray.ToList()).ToArray();

        for(int i=0; i < optionsWordArray.Length; i++)
        {
            optionsWordArray[i].SetChar(charArray[i]);
        }
       
        if(currentQuestionIndex  == 29 && currentCorrectAnswer != 15)
        {
            currentQuestionIndex = PassIndex[countPassIndex2];
            countPassIndex2++;
            setQuestion();
        }
        else
        {
            currentQuestionIndex++;
        }

        gameStatus = GameStatus.Playing;
    }

    public void SelectedOption(WordData WordData)
    {
        if(gameStatus == GameStatus.Next || currentAnswerIndex >= answerWord.Length) return;

        //Debug.Log(clueIndex.Length);
        for(int i = 0; i < clueIndex.Length; i ++)
        {
            //Misal index saat ini == clue index ( ada isinya ), index ++ (lewatin kolomnya)
            if (currentAnswerIndex == clueIndex[i]) 
            {
                currentAnswerIndex++;
            }
        }
        selectedWordIndex.Add(WordData.transform.GetSiblingIndex());

        answerWordArray[currentAnswerIndex].SetChar(WordData.charValue); //kolom jawaban (_) sesuai dgn currentAnswerIndex misal index 0 brti garis ke 1
        WordData.gameObject.SetActive(false); //menghilangkan huruf yg sudah di pilih
        currentAnswerIndex++;

        //Debug.Log(answerWord[0]);
        if(currentAnswerIndex >= answerWord.Length)
        {
            correctAnswer = true;

            for(int i = 0; i < answerWord.Length; i++)
            {
                if(char.ToUpper(answerWord[i]) != char.ToUpper(answerWordArray[i].charValue)) //jika jawaban di (_) gk sama sm jawaban di answer diinspector
                {
                    correctAnswer = false;
                    break;
                }
            }

            if(currentCorrectAnswer <= 15)
            {
                if(correctAnswer)
                {
                    gameStatus = GameStatus.Next; 
                    score += 1;
                    currentCorrectAnswer += 1;
                    scoreText.text = currentScore.ToString(score + " / 15");
                    if(currentQuestionIndex < questionData.questions.Count)
                    {
                        Invoke("setQuestion", 0.5f); //Panggil set Question waktunya 0.5
                    }
                 /*    else
                    {
                       // gameover.SetActive(true);
                    }
                    */
                }
                else if(!correctAnswer)
                {
                   // Debug.Log("False");
                    wrongAnswer = true;
                    currentTime = currentTime - 2;

                    if(popUpCurrentTime == timeToWait)
                    {
                       // popUp.SetActive(false);
                        wrongAnswer = false;
                    }
                
                }
            }
            
            else if(currentCorrectAnswer == 15)
            {
                Time.timeScale = 0f;
                SceneManager.LoadScene(sceneName: "WinScene");
            }
        }
    }

    //reset kolom jawaban dan kolom huruf ke awal (full semua)
    private void ResetQuestion()
    {
        for(int i = 0; i < answerWordArray.Length; i++)
        {
            answerWordArray[i].gameObject.SetActive(true);
            answerWordArray[i].SetChar('_');
        }
        //Debug.Log(answerWord.Length);
        for(int i = answerWord.Length; i < answerWordArray.Length; i++)
        {
            answerWordArray[i].gameObject.SetActive(false);
        }

        //Reset option answer / huruf
        for(int i = 0; i < optionsWordArray.Length; i++)
        {
            optionsWordArray[i].gameObject.SetActive(true);
        }

        for(int i = 0; i < 5; i++)
        {
            clueIndex[i] = -999;
        }

        countIndex = 0;
        popUpCurrentTime = 0.0f;
        wrongAnswer = false;
    }

    public void ResetLastWord()
    {
        if(selectedWordIndex.Count > 0)
        {
            int index = selectedWordIndex[selectedWordIndex.Count - 1];
            optionsWordArray[index].gameObject.SetActive(true);
            selectedWordIndex.RemoveAt(selectedWordIndex.Count - 1);

            currentAnswerIndex--;
            answerWordArray[currentAnswerIndex].SetChar('_');
        }

        if(wrongAnswer == true)
        {
            wrongAnswer = false;
        }
    }

    public void PassBtnNewText()
    {
        if(currentPass > 0)
        {
            currentPass = currentPass - 1;
            buttonText.text = "Pass x" + currentPass;
            
            PassIndex[countPassIndex] = currentQuestionIndex - 1;
            countPassIndex += 1;
            setQuestion();
        }
        else 
        {
            buttonPass.enabled = false;
        }
    }

    private void CheckIndexClue()
    {
        for(int i = 0; i < clueIndex.Length; i ++)
        {
            //Misal index saat ini == clue index ( ada isinya ), index ++ (lewatin kolomnya)
            if (currentAnswerIndex == clueIndex[i]) 
            {
                currentAnswerIndex++;
            }
        }
    }

    public void ClueBtnText()
    {
        if(currentClue > 0)
        {
            currentClue = currentClue - 1;
            cluebuttonText.text = "Clue x" + currentClue;

            for(int i = 0; i < answerWord.Length; i++)
            {
                if(char.ToUpper(answerWord[i]) == char.ToUpper(answerWord[currentAnswerIndex])) // mencari apakah ada huruf yg sama dgn index saat ini
                {
                    answerWordArray[i].SetChar(char.ToUpper(answerWord[i])); // tulis huruf yg ada di kolom jawaban
                    clueIndex[countIndex] = i;
                    countIndex += 1;
                
                  // Hapus opsi pilihan huruf yang sudah ditampilin di clue
                  for(int j = 0; j < charArray.Length; j++)
                    {
                        if(charArray[j] == char.ToUpper(answerWord[i]))
                        {
                            optionsWordArray[j].gameObject.SetActive(false);
                        }
                    }
                }
            }
           CheckIndexClue();

           // Kalau currentAnswerIndex == panjang jawaban - 1 (kalau clue di pakai di kolom paling terakhir) cek apakah jawaban benar atau gk
           if(currentAnswerIndex == answerWord.Length)
           {
                correctAnswer = true;
                for(int i = 0; i < answerWord.Length; i++)
                {
                    if(char.ToUpper(answerWord[i]) != char.ToUpper(answerWordArray[i].charValue)) //jika jawaban di (_) gk sama sm jawaban di answer diinspector
                    {
                        correctAnswer = false;
                        break;
                    }
                }

                if(correctAnswer)
                {
                    gameStatus = GameStatus.Next; 
                    score += 1;
                    currentCorrectAnswer += 1;
                    scoreText.text = currentScore.ToString(score + " / 15");
                    if(currentQuestionIndex < questionData.questions.Count)
                    {
                        Invoke("setQuestion", 0.5f); //Panggil set Question waktunya 0.5
                    }
                }
                else if(!correctAnswer)
                {
                    wrongAnswer = true;
                    currentTime = currentTime - 2;

                    if(popUpCurrentTime == timeToWait)
                    {
                        wrongAnswer = false;
                    } 
                }
            }
        }
        else 
        {
            canUseClue.enabled = false;
        }
    }
   
}

[System.Serializable]
public class QuestionData
{
    //public Sprite questionImage;
    public string questionText;
    public string answer;
}

public enum GameStatus
{
    Playing,
    Next
}
