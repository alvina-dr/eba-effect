using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataHolder : MonoBehaviour
{
    public static DataHolder instance = null;
    public TextAsset levelToLoad;
    public GameSettings GameSettings;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        GameSettings = Resources.Load<GameSettings>("GameSettings");
    }
}
