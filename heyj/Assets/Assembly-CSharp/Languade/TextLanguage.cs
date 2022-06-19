using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextLanguage : MonoBehaviour
{
    public string language;
    Text text;

    public string textRu;
    public string textEng;
    public string textZh;

    void Start()
    {
        text = GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        language = PlayerPrefs.GetString("Language");

        if(language == "" || language == "Eng")
        {
            text.text = textEng;
        }
        else if(language == "Ru")
        {
            text.text = textRu;
        }
        else if (language == "China")
        {
            text.text = textZh;
        }
    }
}
