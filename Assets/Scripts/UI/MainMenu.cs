using UnityEngine;
using UnityEngine.SceneManagement;
using Sirenix.OdinInspector;

public class MainMenu : MonoBehaviour
{
    [SerializeField]
    private Object StartScene;

    [SerializeField]
    private GameObject CreditsView;

    public void StartGame()
    {
        SceneManager.LoadScene(StartScene.name);
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
