using UnityEngine;
using UnityEngine.SceneManagement;
using Sirenix.OdinInspector;

public class MainMenu : MonoBehaviour
{
    [SerializeField]
    private Object StartScene;

    [SerializeField]
    private GameObject CreditsView;

    private void Start()
    {
        var audioSet = GetComponent<AudioSetter>();
        audioSet.ChangeAudio();
    }

    public void StartGame()
    {
        SceneManager.LoadScene("Level 0");
    }

    public void ShowCredits()
    {
        CreditsView.SetActive(true);
    }

    public void HideCredits()
    {
        CreditsView.SetActive(false);
    }

    public void Quit()
    {
        Application.Quit();
    }
}
