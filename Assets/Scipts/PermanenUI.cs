using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


public class PermanenUI : MonoBehaviour
{
    //play start
    public int cherries = 0;
    public int health = 5;
    public TextMeshProUGUI cherryText;
    public TextMeshProUGUI healthAmount;

    public static PermanenUI perm;

    private void Start()
    {
        DontDestroyOnLoad(gameObject);
        // set cherryCount 
        cherryText.text =cherries.ToString();
        //singleton
        if (!perm)
        {
            perm = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void Reset()
    {
        cherries = 0;
        cherryText.text = cherries.ToString();   
    }

}
