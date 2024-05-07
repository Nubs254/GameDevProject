using UnityEngine;
using TMPro;

public class LevelSelector : MonoBehaviour
{
    public int level;
    public TextMeshProUGUI levelText;

    void Start()
    {
        if (levelText == null)
        {
            levelText = GetComponentInChildren<TextMeshProUGUI>();
        }

        if (levelText != null)
        {
            levelText.text = level.ToString();
        }
        else
        {
            Debug.LogError("No TextMeshProUGUI component found.");
        }
    }

    public void openScene()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Level " + level.ToString());
    }
}