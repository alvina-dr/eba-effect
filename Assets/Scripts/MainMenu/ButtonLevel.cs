using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class ButtonLevel : MonoBehaviour
{
    public TextAsset levelAsset;
    public void LoadLevel()
    {
        DataHolder.instance.levelToLoad = levelAsset;
        FindObjectOfType<MainMenu>().fadeMaterial.DOFade(1, .3f).OnComplete(() =>
        {
            SceneManager.LoadScene("Game");
        });
    }

}
