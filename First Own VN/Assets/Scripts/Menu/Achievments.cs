using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Achievments : MonoBehaviour {

    static Dictionary<string, string> AchList; //Словарь с достижениями
    static string Key = "Achievments"; //Ключ для сохранения 
    static string[] CheckingAch = { "Женоненавистник" }; //Достижения, требующие проверки
	void Start ()
    {
        Load(); //Загружаем данные
	}
	
	void Update ()
    {
	
	}

    static public bool Push(string title, string description) //Пуш достижения
    {
        if (!CheckConditions(title)) //Если не прошло проверку
            return false; //То нет
        if (AchList.ContainsKey(title)) //Если уже есть такое достижение
            return false; //То нет
        AchList.Add(title, description); //Добавляем в словарь
        Save(); //Сохраняем
        return true; //Да
    }

    static public string GetDescription(string title) //Получение описания
    {
        if ((AchList == null) || (!AchList.ContainsKey(title))) //Если нет словаря или нет такого ключа
            return ""; //То возрващаем пустую строку
        return AchList[title]; //Возвращаем описание
    }

    static public string GetData() //Функция получения данных о достижениях
    {
        return PlayerPrefs.GetString(Key); //Возвращаем результат
    }

    static public void SetData(string data) //Функция установки данных о достижениях
    {
        PlayerPrefs.SetString(Key, data);//Записываем данные
        Load(); //Загружаем данные
    }

    static bool CheckConditions(string title) //Проверка достижения
    {
        if (title == CheckingAch[0]) //Если достижение "Женоненавистник"
        {
            if (State.CurrentState.VariableValue("sonya") > 0) //Если значение переменно sonya больше нуля
                return false; //То нет
        }
        return true; //Да
    }

    static void Save() //Сохранение данных
    {
        string res = ""; //Текущя строка
        foreach (KeyValuePair<string, string> x in AchList) //Для всех записей в словаре
        {
            res += x.Key + "|" + x.Value + "\n"; //Добавляем в строку
        }
        PlayerPrefs.SetString(Key, res); //Сохраняем по ключу
    }

    static void Load() //Загрузка данных
    {
        AchList = new Dictionary<string, string>(); //Инициализируем словарь
        if (!PlayerPrefs.HasKey(Key)) //Если нет записей по ключу
            return; //То выход
        string[] sep = { "\n" }; //Разделитель
        string[] data = PlayerPrefs.GetString(Key).Split(sep, System.StringSplitOptions.RemoveEmptyEntries); //Получаем данные по ключу
        foreach (string x in data) //Для каждой строки
        {
            string[] pair = x.Split('|'); //Разделяем навзание и описание
            AchList.Add(pair[0], pair[1]); //Добавляем в словарь
        }
    }
}
