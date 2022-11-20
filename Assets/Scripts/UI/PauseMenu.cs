using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class PauseMenu : MonoBehaviour
{

    public void ClosePauseMenu()
    {
        Time.timeScale = 1;
        transform.DOScale(0, .3f).OnComplete(() =>
        {
            gameObject.SetActive(false);
        });
    }
}
