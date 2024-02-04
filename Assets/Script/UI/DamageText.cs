using System;
using TMPro;
using UnityEngine;

public class DamageText : MonoBehaviour
{
    public event Action OnDamageText;
    public TextMeshPro textMeshPro;

    public float moveSpeed = 2f;
    public float fadeSpeed = 0.5f;
    public float lifeTime = 0.3f;

    private float flowTime;

    private void Start()
    {
        textMeshPro = GetComponent<TextMeshPro>();
    }

    private void OnEnable()
    {
        flowTime = 0f;
        textMeshPro.color = Color.white;
    }

    private void Update()
    {
        transform.position += Vector3.up * moveSpeed * Time.deltaTime;
        transform.forward = Camera.main.transform.forward;

        flowTime += Time.deltaTime;
        if (flowTime >= lifeTime)
        {
            var currentColor = textMeshPro.color;
            currentColor.a -= fadeSpeed * Time.deltaTime;

            textMeshPro.color = currentColor;

            if (currentColor.a <= 0)
            {
                OnDamageText?.Invoke();
                OnDamageText = null;
            }
        }
    }

    public void SetText(string text)
    {
        textMeshPro.text = text;
    }
}
