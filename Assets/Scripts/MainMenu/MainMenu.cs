using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MainMenu : MonoBehaviour
{
    public TextAsset[] levelAssetArray;
    public GameObject buttonLevelPrefab;
    public GameObject buttonLevelList;

    void Start()
    {
        levelAssetArray = Resources.LoadAll<TextAsset>("Levels");
        for (int i = 0; i < levelAssetArray.Length; i++)
        {
            GameObject _button = Instantiate(buttonLevelPrefab, buttonLevelList.transform);
            _button.GetComponentInChildren<TextMeshProUGUI>().text = levelAssetArray[i].name;
            _button.GetComponent<ButtonLevel>().levelAsset = levelAssetArray[i];
        }
    }

    void Update()
    {
        
    }
}
