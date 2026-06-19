using UnityEngine;
using UnityEngine.SceneManagement;
public class Main_Menu_Script : MonoBehaviour
{
    public void OnStartClick()
    {
        SceneManager.LoadScene("Level 1");
    }
    public void OnOptionsClick()
    {
        SceneManager.LoadScene("Options_Scene");
    }

    public void OnCreditsClick()
    {
        SceneManager.LoadScene("Credits_Scene");
    }
        public void OnExitClick()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
        Application.Quit();
    }
    public void OnBackClick()
    {
        SceneManager.LoadScene("Main_Menu Scene");
    }
}
