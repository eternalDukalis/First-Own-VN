using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CGGallery : MonoBehaviour {

    static List<string> CGList; //Список картинок
    static string Key = "CGGallery"; //Ключ для хранения
	void Start ()
    {
        Load(); //Загружаем данные
	}
	
	void Update ()
    {
	
	}

    static public void Push(string title) //Функция пуша картинки
    {
        if (!CGList.Contains(title)) //Если такой картинки ещё нет
        {
            CGList.Add(title); //Добавляем в список
            Save(); //Сохраняем
        }
    }

    static public bool Exists(string title) //функия проверки картинки
    {
        if (CGList.Contains(title)) //Если картинка есть в списке
            return true; //То правда
        return false; //Иначе ложь
    }

    static void Save() //Функция сохранения данных
    {
        string res = ""; //Текущая строка
        foreach (string x in CGList) //Для всех записей в списке
        {
            res += x + "\n"; //Добавляем информацию о картинке
        }
        PlayerPrefs.SetString(Key, res); //Записываем по ключу
    }

    static void Load() //Функиця загрузки данных
    {
        CGList = new List<string>(); //Инициализируем список
        if (!PlayerPrefs.HasKey(Key)) //Если нет записей по ключу
            return; //То выход
        string[] sep = { "\n" }; //Разделитель
        string[] data = PlayerPrefs.GetString(Key).Split(sep, System.StringSplitOptions.RemoveEmptyEntries); //Получаем данные по ключу
        foreach (string x in data) //Для каждой строки
        {
            CGList.Add(x); //Добавляем данные в список
        }
    }
}
