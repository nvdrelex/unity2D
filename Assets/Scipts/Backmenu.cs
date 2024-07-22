using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class Backmenu : MonoBehaviour
{
    public void BackMenu()
    {
        SceneManager.LoadScene(0); // Thay "GameScene" bằng tên Scene của game bạn.
    }
}
