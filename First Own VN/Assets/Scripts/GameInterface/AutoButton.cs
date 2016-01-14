﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class AutoButton : MonoBehaviour {

    public Color ImageOn; //Цвет для включения
    Color ImageOff; //Цвет для выключения
    Button button; //Компонент button 
    bool isOn; //Включён ли
	void Start () 
    {
        isOn = Auto.isAuto; //Получаем значение из скрипта Skip 
        button = GetComponent<Button>(); //Ищем компонент
        ImageOff = button.image.color; //сохраняем стандартный цвет
	}
	
	void Update () 
    {
        if (isOn != Auto.isAuto) //Если нужно сменить режим
        {
            isOn = Auto.isAuto; //меняем значение переменной
            SetIndicator(); //изменяем цвет кнопки
        }
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
