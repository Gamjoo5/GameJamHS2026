using UnityEngine;
using UnityEngine.SceneManagement;

public class VictoryUI : MonoBehaviour
{
    [SerializeField] private GameObject victoryPanel;
    public string startMenuSceneName = "StartMenu";

    private void Awake()
    {
        if (victoryPanel != null)
            victoryPanel.SetActive(false);
    }

    public void Zeigen()
    {
        if (victoryPanel != null)
            victoryPanel.SetActive(true);
        Time.timeScale = 0f;
    }

    public void ZurueckZumHauptmenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(startMenuSceneName);
    }
}
