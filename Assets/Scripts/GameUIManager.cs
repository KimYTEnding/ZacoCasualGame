using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameUIManager : MonoBehaviour
{
    public static GameUIManager instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    public bool isGameOver = false;
    [SerializeField] private bool isStop = false;
    [SerializeField] private GameObject panel;
    [SerializeField] private TMP_Text title;
    [SerializeField] private TMP_Text score;

    private void Start()
    {
        isGameOver = false;
        isStop = false;
    }

    public void PauseGame()
    {
        if (!isGameOver)
        {
            if (!isStop)
            {
                Time.timeScale = 0f;
            }
            else
            {
                Time.timeScale = 1.0f;
            }
            isStop = !isStop;
            panel.SetActive(isStop);
        }
        else
        {
            Restart();
        }

    }

    public void SetScore(int score)
    {
        this.score.text = score.ToString();
    }

    public void ExecuteGameOver()
    {
        Invoke("GameOver", 1f);
    }

    private void GameOver()
    {
        Time.timeScale = 0f;
        isStop = true;
        title.text = "�й�";
        panel.SetActive(isStop);
    }

    public void OnPushESC()
    {
        if (!isGameOver)
        {
            title.text = "���� ����";
            PauseGame();
        }
    }

    public void Restart()
    {
        SceneManager.LoadScene("GameScene");
    }

    public void ToMain()
    {
        SceneManager.LoadScene("GameStartScene");
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
