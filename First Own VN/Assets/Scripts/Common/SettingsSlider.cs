using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SettingsSlider : MonoBehaviour {

    public string FieldName; //Название поля
    Slider slider; //Компонент слайдер
	void Start ()
    {
        slider = GetComponent<Slider>(); //Находим компонент
        slider.value = Settings.GetField(FieldName); //Получаем текущее значение
	}
	
	void Update ()
    {
        if (slider.value != Settings.GetField(FieldName)) //Если значение не совпадает со значением в настройках
            slider.value = Settings.GetField(FieldName); //То получаем значение ещё раз
	}

    public virtual void SetValue() //Функция установки значения настройки
    {
        Settings.SetField(FieldName, slider.value); //Устанавливаем значение
    }
}
