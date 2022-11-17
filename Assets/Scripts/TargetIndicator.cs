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
    public TargetData.TargetSide targetSide;
    TargetCtrl currentTarget;

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
            image.enabled = false;
            currentTarget = null;
        }
        else
        {
            image.enabled = true;
            currentTarget = null;
            for (int i = 0; i < GP.targetPool.transform.childCount; i++)
            {
                if (GP.targetPool.transform.GetChild(i).GetComponent<TargetCtrl>().targetData.targetSide == targetSide)
                {
                    currentTarget = GP.targetPool.transform.GetChild(i).GetComponent<TargetCtrl>();
                    transform.parent.transform.position = GP.targetPool.transform.GetChild(i).position + new Vector3(0, 0.38f, -0.2f);
                    break;
                }
            }
            if (currentTarget == null) image.enabled = false;
        }
    }
}
