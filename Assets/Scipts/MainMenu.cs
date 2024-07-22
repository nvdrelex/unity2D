using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void PlayGame()
    {
        SceneManager.LoadScene(1); // Thay "GameScene" bằng tên Scene của game bạn.
    }

    public void QuitGame()
    {
        Debug.Log("QUIT");
        Application.Quit();
    }

    public void OptionsMenu()
    {
        // Mở menu tùy chọn (nếu có)
        Debug.Log("Options");
    }
}
