using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonLevel : MonoBehaviour
{
    public TextAsset levelAsset;
    public void LoadLevel()
    {
        DataHolder.instance.levelToLoad = levelAsset;
        SceneManager.LoadScene("Game");
    }

}
