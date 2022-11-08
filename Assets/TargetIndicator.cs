using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class TargetIndicator : MonoBehaviour
{
    bool animationCanLoop = true;
    Image image;

    void Start()
    {
        image = GetComponent<Image>();
    }

    void Update()
    {
        if (animationCanLoop)
        {
            animationCanLoop = false;
            transform.DOScale(0.085f, 0.2f).OnComplete(() =>
            {
                transform.DOScale(0.08f, 0.2f).OnComplete(() => {
                    animationCanLoop = true;
                });
            });
        }
    }
}
