﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SkipButton : MonoBehaviour {

    public Color ImageOn; //Цвет для включения
    Color ImageOff; //Цвет для выключения
    Button button; //Компонент button 
    bool isOn; //Включён ли
	void Start () 
    {
        isOn = Skip.isSkipping; //Получаем значение из скрипта Skip 
        button = GetComponent<Button>(); //Ищем компонент
        ImageOff = button.image.color; //сохраняем стандартный цвет
        SetIndicator(); //Изменяем цвет кнопки
	}
	
	void Update () 
    {
        if (isOn != Skip.isSkipping) //Если нужно сменить режим
        {
            isOn = Skip.isSkipping; //меняем значение переменной
            SetIndicator(); //изменяем цвет кнопки
        }
        if (button.interactable != Skip.canSkip)
            button.interactable = Skip.canSkip;
        //button.colors.
	}

    void SetIndicator() //функция изменения цвета кнопки
    {
        if (isOn) //если включен
        {
            button.image.color = ImageOn; //то меняем на цвет включения
        }
        else //иначе
        {
            button.image.color = ImageOff; //меняем на цвет выключения
        }
    }
}
