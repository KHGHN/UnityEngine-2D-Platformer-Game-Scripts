using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HealthText : MonoBehaviour
{
    private Vector3 moveSpeed;

    private float timeToFade;

    private float timeElapsed;

    private RectTransform textTransform;

    private TextMeshProUGUI textMeshPro;

    private Color startColor;

    private void Awake()
    {
        textTransform = GetComponent<RectTransform>();
        textMeshPro = GetComponent<TextMeshProUGUI>();
        
    }

    private void Start()
    {
        moveSpeed = new Vector3(0, 75.0f, 0);
        timeToFade = 1.0f;
        timeElapsed = 0f;
        startColor = textMeshPro.color;
    }

    private void Update()
    {
        textTransform.position += moveSpeed * Time.deltaTime;

        timeElapsed += Time.deltaTime;

        if(timeElapsed < timeToFade)
        {
            float fadeAlpha = startColor.a * (1 - (timeElapsed / timeToFade));      // 시간이 지남에 따라 값이 0에 가까워짐
            textMeshPro.color = new Color(startColor.r, startColor.g, startColor.b, fadeAlpha);
        }
        else
        {
            Destroy(gameObject);
        }
    }

}
