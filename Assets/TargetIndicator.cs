using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class TargetIndicator : MonoBehaviour
{
    bool animationCanLoop = true;
    Image image;
    GPCtrl GP;

    void Start()
    {
        GP = GPCtrl.instance;
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

    public void MoveToFirstTarget()
    {
        if (GP.targetPool.transform.childCount == 0)
        {
            Debug.Log("no children so no indicator");
            image.enabled = false;
        }
        else
        {
            image.enabled = true;
            transform.parent.transform.position = GP.targetPool.transform.GetChild(0).position + new Vector3(0, 0.38f, -0.2f); ;
        }
    }
}
