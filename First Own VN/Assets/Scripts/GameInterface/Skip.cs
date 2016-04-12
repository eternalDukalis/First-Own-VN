using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class Skip : MonoBehaviour {

    static public bool isSkipping //Переменная, хранящая, нужно ли пропускать сценарий
    {
        get
        {
            bool res = _isSkipping && (isPassed(State.CurrentState.CurrentSource, State.CurrentState.CurrentInstruction) || !Settings.SkipPassedOnly);
            return res;
        }
    }
    static bool _isSkipping;
    static Dictionary<string, List<int>> Passed; //Словарь со списками номеров пройденных инструкций
    static string PassedKey = "Passed"; //Ключ для хранения
	void Start () 
    {
        //PlayerPrefs.DeleteKey("Passed");
        LoadPassed(); //Загружаем списки
        _isSkipping = false; //По умолчанию false
	}
	
	void Update () 
    {
        if (!isPassed(State.CurrentState.CurrentSource, State.CurrentState.CurrentInstruction) && Settings.SkipPassedOnly) //Если текст не был пройден и проигрывать непройденный текст нельзя
        {
            _isSkipping = false; //Выключаем режим проигрывания
        }
        if ((Input.GetKey(KeyCode.LeftControl)) || (Input.GetKey(KeyCode.RightControl))) //Если зажат ctrl
        {
            _isSkipping = true; //То нужно пропускать
        }
        else //Иначе
        {
            if (Input.GetKeyDown(KeyCode.Tab)) //Если нажат Tab
            {
                _isSkipping = !_isSkipping; //Переключаем режим
            }
        }
        if ((Input.GetKeyUp(KeyCode.LeftControl)) || (Input.GetKeyUp(KeyCode.RightControl))) //Если отпускается ctrl
        {
            _isSkipping = false; //То не нужно пропускать
        }
	}

    static public string GetData() //Функция получения данных о пройденных учатсках
    {
        return PlayerPrefs.GetString(PassedKey); //Возвращаем результат
    }

    static public void SetData(string data) //Функция уустановки данных о пройденных участках
    {
        PlayerPrefs.SetString(PassedKey, data); //ЗАписываем данные
        LoadPassed(); //Загружаем данные
    }

    public virtual void Switch() //Функция переключения режима
    {
        _isSkipping = !_isSkipping; //Переключаем режим
    }

    static public void SetMode(bool val) //Функция изменения режима пропуска
    {
        _isSkipping = val; //Изменяем значение
    }

    static void SavePassed() //Функция сохранения списка пройденного текста
    {
        string result = ""; //Результат
        foreach (KeyValuePair<string, List<int>> x in Passed) //Для каждого источника 
        {
            result += x.Key + "|"; //Добавляем название источника
            foreach (int y in x.Value) //Для всех номеров в списке
            {
                result += y + " "; //Добавляем номер
            }
            result += "\n"; //Переходим на новую строку
        }
        PlayerPrefs.SetString(PassedKey, result); //Сохраняем строку
    }

    static void LoadPassed() //Функця загрузки списка пройденного текста
    {
        Passed = new Dictionary<string, List<int>>(); //Инициализируем словарь
        if (!PlayerPrefs.HasKey(PassedKey)) //Если нет сохранённых данных
            return; //То выход
        char[] sep1 = { '\n' };
        char[] sep2 = { ' ' };
        string[] passed = PlayerPrefs.GetString(PassedKey).Split(sep1, System.StringSplitOptions.RemoveEmptyEntries); //Загружаем и разделяем данные
        foreach (string x in passed) //Для каждой строки
        {
            string[] data = x.Split('|'); //Разделяем строку
            Passed.Add(data[0], new List<int>()); //Инициализируем список
            string[] nums = data[1].Split(sep2, System.StringSplitOptions.RemoveEmptyEntries); //Раздеояем и получаем номера
            foreach (string y in nums) //Для каждого номера
            {
                Passed[data[0]].Add(int.Parse(y)); //Добавляем номер в список
            }
        }
    }

    static public void AddPassed(string source, int instruction) //Функция добавления в список пройденного текста
    {
        if (!Passed.ContainsKey(source)) //Если для источника нет списка
            Passed.Add(source, new List<int>()); //То создаём его
        if (!Passed[source].Contains(instruction)) //Если в списке нет номера
        {
            Passed[source].Add(instruction); //То добавляем в него номер
            SavePassed(); //Сохраняем данные
        }
    }

    static bool isPassed(string source, int instruction) //Можно ли пропускать это
    {
        if (!Passed.ContainsKey(source)) //Если источника нет в словаре
            return false; //То нет
        if (!Passed[source].Contains(instruction)) //Если нет номера в списке
            return false; //То нет
        return true; //Да
    }
}
