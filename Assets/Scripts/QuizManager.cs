using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class QuizManager : MonoBehaviour // Created by Bryan Castaneda
{
    [System.Serializable]
    public class QuestionAndAnswers
    {
        public string Question; // The question text
        public string[] Answers; // Array of possible answers
        public int CorrectAnswer; // Index of the correct answer
    }

    public List<QuestionAndAnswers> QnA; // List to hold all questions and their answers
    public GameObject[] options; // Buttons for answers
    public TMP_Text QuestionTxt; // TextMeshPro for the question text
    public TMP_Text scoreText; // Text to display score on TestFailed canvas
    public Color defaultButtonColor = new Color32(18, 15, 15, 255); // Custom default color for buttons
    public GameObject testPassedCanvas; // Reference to the TestPassed canvas
    public GameObject testFailedCanvas; // Reference to the TestFailed canvas

    private int currentQuestionIndex; // Current question index
    private int questionsAsked = 0; // Counter for number of questions asked
    private const int totalQuestionsToAsk = 5; // Total number of questions to ask
    private int correctAnswers = 0; // Counter for correct answers

    private void Start()
    {
        InitializeQuestions(); // Populate the QnA list with questions and answers
        ShuffleQuestions(); // Shuffle the QnA list for better randomness
        if (testPassedCanvas != null)
        {
            testPassedCanvas.SetActive(false); // Ensure TestPassed canvas is hidden at the start
        }
        if (testFailedCanvas != null)
        {
            testFailedCanvas.SetActive(false); // Ensure TestFailed canvas is hidden at the start
        }
        AskNextQuestion();
    }

    void InitializeQuestions() //25 questions total that will be chosen at random
    {
        QnA = new List<QuestionAndAnswers>
  {
      // Easy Questions
      new QuestionAndAnswers
      {
          Question = "What is the closest planet to the Sun?",
          Answers = new string[] { "Mercury", "Venus", "Earth", "Mars" },
          CorrectAnswer = 0
      },
      new QuestionAndAnswers
      {
          Question = "Which planet is known as the Red Planet?",
          Answers = new string[] { "Venus", "Earth", "Mars", "Jupiter" },
          CorrectAnswer = 2
      },
      new QuestionAndAnswers
      {
          Question = "What is the name of our galaxy?",
          Answers = new string[] { "Milky Way", "Andromeda", "Black Hole", "Big Dipper" },
          CorrectAnswer = 0
      },
      new QuestionAndAnswers
      {
          Question = "What is Earth's only natural satellite?",
          Answers = new string[] { "Moon", "Mars", "Sun", "Comet" },
          CorrectAnswer = 0
      },
      new QuestionAndAnswers
      {
          Question = "What is the largest planet in our solar system?",
          Answers = new string[] { "Earth", "Saturn", "Jupiter", "Neptune" },
          CorrectAnswer = 2
      },
      new QuestionAndAnswers
      {
          Question = "What star is at the center of our solar system?",
          Answers = new string[] { "Moon", "Sun", "Sirius", "Polaris" },
          CorrectAnswer = 1
      },
      new QuestionAndAnswers
      {
          Question = "How many planets are in our solar system?",
          Answers = new string[] { "7", "8", "9", "10" },
          CorrectAnswer = 1
      },
      new QuestionAndAnswers
      {
          Question = "What is the smallest planet in the solar system?",
          Answers = new string[] { "Venus", "Pluto", "Mercury", "Mars" },
          CorrectAnswer = 2
      },
      new QuestionAndAnswers
      {
          Question = "What is the name of the first human to land on the Moon?",
          Answers = new string[] { "Yuri Gagarin", "Neil Armstrong", "Buzz Aldrin", "Michael Collins" },
          CorrectAnswer = 1
      },
      new QuestionAndAnswers
      {
          Question = "What is the term for a rock that burns up in Earth's atmosphere?",
          Answers = new string[] { "Asteroid", "Meteor", "Comet", "Satellite" },
          CorrectAnswer = 1
      },

      // Medium Questions
      new QuestionAndAnswers
      {
          Question = "Which planet has the most moons?",
          Answers = new string[] { "Earth", "Saturn", "Jupiter", "Uranus" },
          CorrectAnswer = 1
      },
      new QuestionAndAnswers
      {
          Question = "Which planet is famous for its rings?",
          Answers = new string[] { "Mars", "Saturn", "Jupiter", "Venus" },
          CorrectAnswer = 1
      },
      new QuestionAndAnswers
      {
          Question = "What is the name of the largest volcano in the solar system?",
          Answers = new string[] { "Mount Everest", "Olympus Mons", "Vesuvius", "Mauna Loa" },
          CorrectAnswer = 1
      },
      new QuestionAndAnswers
      {
          Question = "Which planet is the hottest in the solar system?",
          Answers = new string[] { "Venus", "Mercury", "Mars", "Jupiter" },
          CorrectAnswer = 0
      },
      new QuestionAndAnswers
      {
          Question = "What force keeps planets in orbit around the Sun?",
          Answers = new string[] { "Magnetism", "Gravity", "Friction", "Energy" },
          CorrectAnswer = 1
      },
      new QuestionAndAnswers
      {
          Question = "What is a black hole?",
          Answers = new string[] { "A hole in space", "A collapsed star", "A cloud of gas", "A type of planet" },
          CorrectAnswer = 1
      },
      new QuestionAndAnswers
      {
          Question = "What is the term for a year on Earth?",
          Answers = new string[] { "300 days", "365 days", "400 days", "390 days" },
          CorrectAnswer = 1
      },
      new QuestionAndAnswers
      {
          Question = "Which planet has a storm called the Great Red Spot?",
          Answers = new string[] { "Mars", "Jupiter", "Saturn", "Neptune" },
          CorrectAnswer = 1
      },
      new QuestionAndAnswers
      {
          Question = "What is the name of the rover that recently explored Mars?",
          Answers = new string[] { "Spirit", "Curiosity", "Opportunity", "Perseverance" },
          CorrectAnswer = 3
      },
      new QuestionAndAnswers
      {
          Question = "What is the primary gas in the Sun?",
          Answers = new string[] { "Oxygen", "Helium", "Hydrogen", "Carbon Dioxide" },
          CorrectAnswer = 2
      },

      // Challenging (Medium)
      new QuestionAndAnswers
      {
          Question = "What are comets mostly made of?",
          Answers = new string[] { "Rock and Dust", "Ice and Dust", "Gas and Liquid", "Metal and Gas" },
          CorrectAnswer = 1
      },
      new QuestionAndAnswers
      {
          Question = "What is the name of the first satellite sent into space?",
          Answers = new string[] { "Voyager", "Sputnik", "Apollo", "Hubble" },
          CorrectAnswer = 1
      },
      new QuestionAndAnswers
      {
          Question = "Which planet rotates on its side?",
          Answers = new string[] { "Neptune", "Uranus", "Saturn", "Jupiter" },
          CorrectAnswer = 1
      },
      new QuestionAndAnswers
      {
          Question = "What is the Kuiper Belt?",
          Answers = new string[] {
              "A ring of asteroids between Mars and Jupiter",
              "A region of icy bodies beyond Neptune",
              "A ring around Saturn",
              "A comet's tail"
          },
          CorrectAnswer = 1
      },
      new QuestionAndAnswers
      {
          Question = "What is the name of the dwarf planet in our solar system?",
          Answers = new string[] { "Pluto", "Ceres", "Eris", "All of the above" },
          CorrectAnswer = 3
      }
  };
    }


    void SetAnswers()
    {
        for (int i = 0; i < options.Length; i++)
        {
            if (i < QnA[currentQuestionIndex].Answers.Length)
            {
                options[i].SetActive(true);
                options[i].transform.GetChild(0).GetComponent<TMP_Text>().text = QnA[currentQuestionIndex].Answers[i];

                // Reset button color to the custom default color
                options[i].GetComponent<Image>().color = defaultButtonColor;
            }
            else
            {
                options[i].SetActive(false);
            }
        }
    }

    void ShuffleQuestions()
    {
        for (int i = 0; i < QnA.Count; i++)
        {
            int randomIndex = Random.Range(0, QnA.Count);
            QuestionAndAnswers temp = QnA[i];
            QnA[i] = QnA[randomIndex];
            QnA[randomIndex] = temp;
        }
    }

    void AskNextQuestion()
    {
        if (questionsAsked < totalQuestionsToAsk && QnA.Count > 0)
        {
            currentQuestionIndex = Random.Range(0, QnA.Count);
            QuestionTxt.text = QnA[currentQuestionIndex].Question;
            SetAnswers();
            questionsAsked++;
        }
        else
        {
            EndQuiz();
        }
    }

    public void CheckAnswer(int selectedOption)
    {
        if (selectedOption == QnA[currentQuestionIndex].CorrectAnswer)
        {
            // Correct answer: turn button green
            options[selectedOption].GetComponent<Image>().color = Color.green;
            Debug.Log("Correct Answer!");
            correctAnswers++; // Increment correct answer count
        }
        else
        {
            // Incorrect answer: turn button red
            options[selectedOption].GetComponent<Image>().color = Color.red;
            Debug.Log("Wrong Answer!");
        }

        // Delay before moving to the next question
        Invoke(nameof(AskNextQuestion), 0.8f);

        QnA.RemoveAt(currentQuestionIndex); // Remove the current question to avoid repetition
    }

    void EndQuiz()
    {
        Debug.Log("Quiz Completed!");

        if (correctAnswers == totalQuestionsToAsk)
        {
            // Show TestPassed canvas if all answers are correct
            if (testPassedCanvas != null)
            {
                testPassedCanvas.SetActive(true);
                Debug.Log("Congratulations! You passed the test!");
            }
            else
            {
                Debug.LogError("TestPassed canvas is not assigned in the Inspector!");
            }
        }
        else
        {
            // Show TestFailed canvas if not all answers are correct
            if (testFailedCanvas != null)
            {
                testFailedCanvas.SetActive(true);
                if (scoreText != null)
                {
                    scoreText.text = "Score: " + correctAnswers + "/" + totalQuestionsToAsk;
                }
                else
                {
                    Debug.LogError("Score text is not assigned in the Inspector!");
                }
            }
            else
            {
                Debug.LogError("TestFailed canvas is not assigned in the Inspector!");
            }
        }

        //Method for button to resume game
       
    }
    //Method for button to resume game
    public void ResumeGame()
    {
        SceneManager.LoadSceneAsync(1);
    }
}
