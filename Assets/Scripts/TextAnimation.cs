using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;

public class TextAnimation : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private float originalSize;
    private float biggerSize = 55f;
    private float targetSize;
    private float speed = 5f;
    private TMP_Text text;

    void Start()
    {
        text = GetComponent<TMP_Text>();
        originalSize = text.fontSize;
        targetSize = originalSize;
    }

    void Update()
    {
        text.fontSize = Mathf.Lerp(text.fontSize, targetSize, speed * Time.unscaledDeltaTime);
    }
    public void OnPointerEnter(PointerEventData eventData) 
    { 
        targetSize = biggerSize;
    }
    public void OnPointerExit(PointerEventData eventData) 
    {
        targetSize = originalSize;
    }
}
