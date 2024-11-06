using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartUIManager : MonoBehaviour
{
    [SerializeField] private GameObject BackPanel;
    [SerializeField] private GameObject DescriptionPanel;
    [SerializeField] private Button button;
    [SerializeField] private bool activeSet = false;

    public void EnterGame()
    {
        SceneManager.LoadScene("GameScene");
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public void LearnControlButton()
    {
        activeSet = true;
        BackPanel.SetActive(activeSet);
        DescriptionPanel.SetActive(activeSet);
    }

    public void PanelClicked()
    {
        activeSet = false;
        BackPanel.SetActive(activeSet);
        DescriptionPanel.SetActive(activeSet);
    }
}
