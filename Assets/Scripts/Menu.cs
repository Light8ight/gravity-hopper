using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{
    [SerializeField] private Text highestScoreText;

    public void OnStartButton()
    {
        SceneManager.LoadScene("Game");
    }

    public void OnExitButton()
    {
        Application.Quit();
    }

    public void LateUpdate()
    {
        int highestScore = PlayerPrefs.GetInt("highestScore");
        highestScoreText.text = "The highest score:\n" + highestScore.ToString();
    }
}
