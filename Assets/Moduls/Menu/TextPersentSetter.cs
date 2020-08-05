using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(TextMeshProUGUI))]
public class TextPersentSetter : MonoBehaviour
{
    private TextMeshProUGUI _text;
    [SerializeField] private Slider _slider;
    private void Start()
    {
        _text = GetComponent<TextMeshProUGUI>();
        _slider.onValueChanged.AddListener(delegate(float value) { SetText(value);  });
        SetText(_slider.value);
    }

    private void SetText(Single value)
    {
        if(_text !=null)
            _text.text = $"{(int)(value * 100)}%";
    }
}
