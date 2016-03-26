using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MusicGallery : MonoBehaviour {

    static List<string> MusicList; //Список музыки
    static string Key = "MusicGallery"; //Ключ для сохранения
	void Start ()
    {
        Load(); //Загружаем данные
    }
	
	void Update ()
    {
	
	}

    static public void Push(string title) //Функция пуша музыки
    {
        if (!MusicList.Contains(title)) //Если такой музыки ещё нет
        {
            MusicList.Add(title); //Добавляем в список
            Save(); //Сохраняем
        }
    }

    static public bool Exists(string title) //функия проверки музыки
    {
        if (MusicList == null)
            return false;
        if (MusicList.Contains(title)) //Если музыка есть в списке
            return true; //То правда
        return false; //Иначе ложь
    }

    static void Save() //Функция сохранения данных
    {
        string res = ""; //Текущая строка
        foreach (string x in MusicList) //Для всех записей в списке
        {
            res += x + "\n"; //Добавляем информацию о картинке
        }
        PlayerPrefs.SetString(Key, res); //Записываем по ключу
    }

    static void Load() //Функиця загрузки данных
    {
        MusicList = new List<string>(); //Инициализируем список
        if (!PlayerPrefs.HasKey(Key)) //Если нет записей по ключу
            return; //То выход
        string[] sep = { "\n" }; //Разделитель
        string[] data = PlayerPrefs.GetString(Key).Split(sep, System.StringSplitOptions.RemoveEmptyEntries); //Получаем данные по ключу
        foreach (string x in data) //Для каждой строки
        {
            MusicList.Add(x); //Добавляем данные в список
        }
    }
}
