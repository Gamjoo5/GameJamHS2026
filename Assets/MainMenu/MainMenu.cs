using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public string spielSceneName = "Game";
    public GameObject optionsPanel;
    public GameObject menuButtons;
    public Slider lautstärkeSlider;

    private void Awake()
    {
        float savedVolume = PlayerPrefs.HasKey("Lautstaerke")
            ? PlayerPrefs.GetFloat("Lautstaerke", 1f)
            : 1f;
        if (savedVolume <= 0f) savedVolume = 1f;
        AudioListener.volume = savedVolume;

        if (lautstärkeSlider != null)
            lautstärkeSlider.SetValueWithoutNotify(savedVolume);

        if (optionsPanel != null)
            optionsPanel.SetActive(false);
    }

    public void SpielStarten()
    {
        SceneManager.LoadScene(spielSceneName);
    }

    public void SpielBeenden()
    {
        Application.Quit();
        Debug.Log("Spiel beendet!");
    }

    public void OptionsOeffnen()
    {
        optionsPanel.SetActive(true);
        if (menuButtons != null) menuButtons.SetActive(false);
    }

    public void OptionsSchliessen()
    {
        optionsPanel.SetActive(false);
        if (menuButtons != null) menuButtons.SetActive(true);
    }

    public void LautstärkeAendern(float value)
    {
        AudioListener.volume = value;
        PlayerPrefs.SetFloat("Lautstaerke", value);
    }
}
