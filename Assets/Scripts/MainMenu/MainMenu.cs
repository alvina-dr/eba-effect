using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MainMenu : MonoBehaviour
{
    public TextAsset[] levelAssetArray;
    public GameObject buttonLevelPrefab;
    public GameObject buttonLevelList;
    public AudioClip menuMusic;

    void Start()
    {
        levelAssetArray = Resources.LoadAll<TextAsset>("Levels");
        for (int i = 0; i < levelAssetArray.Length; i++)
        {

            GameObject _button = Instantiate(buttonLevelPrefab, buttonLevelList.transform);
            float Yposition = i * 110 * -1;
            _button.transform.localPosition = new Vector3(_button.transform.localPosition.x, Yposition, _button.transform.localPosition.z);
            _button.GetComponentInChildren<TextMeshProUGUI>().text = levelAssetArray[i].name;
            _button.GetComponent<TargetButton>().levelAsset = levelAssetArray[i];
        }
        AudioEngine.instance.PlayMusic(menuMusic, true);
    }

    void Update()
    {
        
    }
}
