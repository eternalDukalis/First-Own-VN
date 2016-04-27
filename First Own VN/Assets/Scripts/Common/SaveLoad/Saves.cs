using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class Saves : MonoBehaviour {

    static Dictionary<Vector3, System.DateTime> SaveFiles; //Словарь с ключами сохранений и временем сохранения
    static Dictionary<Vector3, State> AllSaves; //Словарь с сохранениями
    static string SaveFilesKey = "SaveFiles"; //Ключ для сохранения словаря сохранения
    static string[] PairSeparator = { "\n[prev]\n" }; //РАзделитель для пары текущее сохранение - предыдущее сохраненее
    static string[] SavesSeparator = { "\n[nextsave]\n" }; //РАзделитель для сохранений
    void Start () 
    {
        LoadFilesList(); //Загружаем ключи сохранений
        LoadSaves(); //Загружаем сохранения
	}
	
	void Update () 
    {
        
	}

    static public void Save(Vector3 position) //Функция сохранения в слот
    {
        if (SaveFiles.ContainsKey(position)) //Если в слоте уже есть сохранение
        {
            SaveFiles.Remove(position); //Удаляем ключ
            AllSaves.Remove(position); //Удаляем сохранение
        }
        SaveFiles.Add(position, System.DateTime.Now); //Добавляем ключ
        SaveFilesList(); //Сохраняем изменения
        AllSaves.Add(position, new State(State.CurrentState)); //Добавляем сохранение
        AllSaves[position].PreviousState = State.CurrentState.PreviousState; //Добавляем информацию о предыдущем состоянии
        PlayerPrefs.SetString(string.Format("{0} {1} {2}", position.x, position.y, position.z), AllSaves[position].ToString()); //Сохраняем сохранение
        PlayerPrefs.SetString(string.Format("{0}-{1}-{2}", position.x, position.y, position.z), State.CurrentState.PreviousState.ToString()); //Сохраняем предыдущее состояение сохранения
    }

    static public void Load(Vector3 position) //Функция загрузки из слота
    {
        State.CurrentState = new State(AllSaves[position].PreviousState); //Загружаем предыдущее состояние из сохранения
        SceneManager.LoadScene("game"); //Загружаем уровень
    }

    static public void Continue() //Функция продолжения
    {
        Vector3 lastVec = new Vector3(); //Текущий ключ
        System.DateTime lastTime = new System.DateTime(); //Время
        bool found = false; //Нашли ли
        foreach (KeyValuePair<Vector3, System.DateTime> x in SaveFiles) //Для всех записей в словаре ключей
        {
            if (x.Value > lastTime) //Если сохранение произошло позже
            {
                lastTime = x.Value; //Сохраняем время
                lastVec = x.Key; //Сохраняем ключ
                found = true; //Нашли
            }
        }
        if (found) //Если нашли
            Load(lastVec); //Загружаем
    }

    static public void Delete(Vector3 position) //Функция удаления из слота
    {
        SaveFiles.Remove(position); //Удаляем ключ
        SaveFilesList(); //Сохраняем изменения
        AllSaves.Remove(position); //Удаляем сохранения из словаря
        PlayerPrefs.DeleteKey(string.Format("{0} {1} {2}", position.x, position.y, position.z)); //Удаляем сохранение с диска
        PlayerPrefs.DeleteKey(string.Format("{0}-{1}-{2}", position.x, position.y, position.z)); //Удаляем предыдущее состояние сохранения с диска
    }

    static public System.DateTime GetTime(Vector3 position) //Функция получения времени сохранения
    {
        if (!SaveFiles.ContainsKey(position)) //Если ключа нет
            return System.DateTime.MinValue; //То возвращаем минимальное значение
        return SaveFiles[position]; //Возвращаем время
    }

    static public State GetData(Vector3 position) //Функция получения состояние из сохранения
    {
        if (!AllSaves.ContainsKey(position)) //Если ключа нет
            return null; //Возращаем null
        return AllSaves[position]; //возвращаем состояние
    }

    static public string GetSavesData() //Функция получения всех данных о сохранениях
    {
        if (AllSaves == null) //Если словарь не инициализирован
            return ""; //Возваращаем пустую строку
        string res = ""; //Строка с результатом
        foreach (KeyValuePair<Vector3, State> x in AllSaves) //Для каждой записи в словаре
        {
            res += string.Format("{0} {1} {2}", x.Key.x, x.Key.y, x.Key.z) + PairSeparator[0]; //Добавляем данные о ключе
            res += PlayerPrefs.GetString(string.Format("{0} {1} {2}", x.Key.x, x.Key.y, x.Key.z)) + PairSeparator[0]; //Добавляем данные о текущем состоянии
            res += PlayerPrefs.GetString(string.Format("{0}-{1}-{2}", x.Key.x, x.Key.y, x.Key.z)) + SavesSeparator[0]; //ДОбавляем данные о предыдущем состоянии
        }
        return res; //Возвращаем результат
    }

    static public string GetSavesListData() //Функция получения данных о списке сохранений
    {
        return PlayerPrefs.GetString(SaveFilesKey); //Возвращаем результат
    }

    static public void SetSavesData(string list, string saves) //Функция установки сохранений
    {
        PlayerPrefs.SetString(SaveFilesKey, list); //Записываем данные о ключах
        LoadFilesList(); //ЗАгружаем ключи
        string[] pairs = saves.Split(SavesSeparator, System.StringSplitOptions.RemoveEmptyEntries); //Определяем пары данных о сохранениях
        foreach (string x in pairs) //Для каждой такой пары
        {
            string[] savesdata = x.Split(PairSeparator, System.StringSplitOptions.RemoveEmptyEntries); //Разделяем данные
            string[] vecd = savesdata[0].Split(' '); //Находим ключ-вектор
            Vector3 vec = new Vector3(int.Parse(vecd[0]), int.Parse(vecd[1]), int.Parse(vecd[2])); //Создаём ключ-вектор
            PlayerPrefs.SetString(string.Format("{0} {1} {2}", vec.x, vec.y, vec.z), savesdata[1]); //Записываем данные о текущем состоянии
            PlayerPrefs.SetString(string.Format("{0}-{1}-{2}", vec.x, vec.y, vec.z), savesdata[2]); //Записываем данные о предыдущем состоянии
        }
        LoadSaves(); //Загружаем данные о сохранениях
    }

    static void SaveFilesList() //Функция сохранения словаря ключей
    {
        string val = ""; //Промежуточная строка
        foreach (KeyValuePair<Vector3, System.DateTime> x in SaveFiles) //Для всех записей в словаре
        {
            val += string.Format("{0} {1} {2}|{3}\n", x.Key.x, x.Key.y, x.Key.z, x.Value); //Добавляем данные в строку
        }
        PlayerPrefs.SetString(SaveFilesKey, val); //Сохраняем строку
    }

    static void LoadFilesList() //Функция загружки словаря ключей
    {
        SaveFiles = new Dictionary<Vector3, System.DateTime>(); //Инициализируем словарь
        if (!PlayerPrefs.HasKey(SaveFilesKey)) //Если ключа нет
            return; //Прерываем функцию
        string val = PlayerPrefs.GetString(SaveFilesKey); //Получаем строку
        char[] sep = {'\n'}; //Разделитель - знак перевода строки
        string[] strs = val.Split(sep, System.StringSplitOptions.RemoveEmptyEntries); //Разделяем на строки
        foreach (string x in strs) //Для каждой строки
        {
            string[] data = x.Split('|'); //Разделяем на данные о векторе и данные о времени
            string[] vec = data[0].Split(' '); //Разделяем числа в данных о векторе
            SaveFiles.Add(new Vector3(int.Parse(vec[0]), int.Parse(vec[1]), int.Parse(vec[2])), System.DateTime.Parse(data[1])); //Добавляем запись в словарь
        }
    }

    static void LoadSaves() //Загрузка сохранений
    {
        AllSaves = new Dictionary<Vector3, State>(); //Инициализируем словарь
        foreach (KeyValuePair<Vector3, System.DateTime> x in SaveFiles) //Для каждого ключа в словаре ключей
        {
            AllSaves.Add(x.Key, new State(PlayerPrefs.GetString(string.Format("{0} {1} {2}", x.Key.x, x.Key.y, x.Key.z)))); //Получаем запись по ключу
            AllSaves[x.Key].PreviousState = new State(PlayerPrefs.GetString(string.Format("{0}-{1}-{2}", x.Key.x, x.Key.y, x.Key.z))); //Получаем предыдущее состояние по ключу
        }
    }
}
