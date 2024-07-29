using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using TMPro;
using System.Collections;
public class PopupText : MonoBehaviour
{
    public TextMeshPro TextPrefab;
    private List<TextMeshPro> textMeshProList = new List<TextMeshPro>();
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        for (int i = 0; i < 10; i++)
        {
            textMeshProList.Add(Instantiate<TextMeshPro>(TextPrefab));
            textMeshProList[i].gameObject.SetActive(false);
        }
    }
    private void Start()
    {
        GlobalEvents.OnDamageTaken += ShowPopupText;
    }
    // Update is called once per frame
    void Update()
    {
        
    }
    public void ShowPopupText(int number, Vector3 position)
    {
        ShowPopupText(number.ToString(), position);
    }
    public void ShowPopupText(string text, Vector3 position)
    {
        var tmp = GetFirstAvailable();
        tmp.text = text;
        tmp.gameObject.transform.position = new Vector3(position.x, position.y, tmp.gameObject.transform.position.z);
        StartCoroutine(ShowText(tmp));
    }
    private TextMeshPro GetFirstAvailable()
    {
        foreach(var text in textMeshProList)
        {
            if (text.gameObject.activeInHierarchy)
                continue;
            return text;
        }
        return null;
    }
    IEnumerator ShowText(TextMeshPro text)
    {
        text.gameObject.SetActive(true);
        yield return new WaitForSeconds(2);
        text.gameObject.SetActive(false);
    }
}
