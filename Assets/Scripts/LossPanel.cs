using UnityEngine;
using UnityEngine.UI;

public class LossPanel : MonoBehaviour
{
    [SerializeField] private Text topScoreText;

    public void Setup(int score)
    {
        gameObject.SetActive(true);
        topScoreText.text = "Top score:\n" + score.ToString();
    }
}
