using DG.Tweening;
using UnityEngine;

public class AnimationCtrl : MonoBehaviour
{
    [Header("ANIMATION SETTINGS")]
    [SerializeField] float animationTime = 1f;
    [Header("MOVE ANIMATION")]
    [SerializeField] bool moveForward;
    [SerializeField] float moveSpeed = 5f;
    [Header("FLOAT ANIMATION")]
    [SerializeField] bool floatInAir;
    [SerializeField] float floatAmplitude = 0.05f;
    bool animationCanLoop = true;
    Vector3 destination;

    private void Start()
    {
        destination = transform.position;
    }

    private void Update()
    {
        if (animationCanLoop)
        {
            animationCanLoop = false;
            if (floatInAir) destination += new Vector3(0, floatAmplitude, 0);
            if (moveForward) destination += transform.forward * moveSpeed;
            transform.DOMove(destination, animationTime).SetEase(Ease.Linear).OnComplete(() =>
            {
                if (floatInAir) destination -= new Vector3(0, floatAmplitude, 0);
                if (moveForward) destination += transform.forward * moveSpeed;
                transform.DOMove(destination, animationTime).SetEase(Ease.Linear).OnComplete(() => {
                    animationCanLoop = true;
                });
            });
        }
    }
}
