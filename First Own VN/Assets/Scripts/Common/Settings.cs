using UnityEngine;
using System.Collections;

public class Settings : MonoBehaviour {

    static public float SymbolInterval = 0.02f; //Интервал между появлением букв
    static public float SkipInterval = 0.05f; //Интервал перед пропуском
    static public float AutoBaseTime = 1f; //Базовое время автопоспроизведения
    static public float AutoSymbolTime = 0.04f; //Время автовоспроизведения для одного символа
    static public float MusicVolume = 0.7f; //Громкость музыки
    static public float EnvironmentVolume = 0.8f; //Громкость звуков окружения
    static public float SoundVolume = 0.9f; //Громкость звуковых эффектов
    static public bool FullScreen = true; //Полноэкранный режим
    static public bool SkipPassedOnly = true; //Пропускать только прочитанный текст
    static public bool StopSkipAfterChoice = true; //Останавливать пропуск после выбора
    static public float TextSpeed
    {
        get
        {
            return _textSpeed;
        }
        set
        {
            _textSpeed = value;
            SymbolInterval = (1 - _textSpeed) / TextSpeedDivider;
        }
    } //Скорость текста
    static public float AutoSpeed
    {
        get
        {
            return _autoSpeed;
        }
        set
        {
            _autoSpeed = value;
            AutoBaseTime = (1 + AutoTimeMultiplier - AutoTimeMultiplier * _autoSpeed) * AutoBaseTimeMin;
            AutoSymbolTime = (1 + AutoTimeMultiplier - AutoTimeMultiplier * _autoSpeed) * AutoSymbolTimeMin;
        }
    } //Скорость автовоспроизведения
    static public bool HasStarted
    {
        get
        {
            return _hasStarted;
        }
        set
        {
            _hasStarted = value;
            SaveSettings();
        }
    } //Была ли начата игра
    static bool _hasStarted = false; 
    static float _textSpeed = 0.8f; //Скорость текста
    static float _autoSpeed = 0.4f; //Скорость автовоспроизведения
    static float TextSpeedDivider = 10; //Делитель скорость текста
    static float AutoBaseTimeMin = 0.25f; //Максимум базового времени автовоспроизведения
    static float AutoSymbolTimeMin = 0.01f; //Максимум времени автовоспроизведения для одного символа
    static float AutoTimeMultiplier = 5; //Множитель для времени автовоспроизведения
    static int StandartWidth; //Ширина экрана
    static int StandartHeight; //Высота экрана
    static int WindowedWidth = 800; //Ширина экрана в оконном режиме
    static int WindowedHeight = 600; //Высота экрана в оконном режиме
    static string SettingsKey = "Settings"; //Ключ для сохранения настроек
    static bool Loaded = false; //Были ли загружены настройки
	void Start () 
    {
        StandartWidth = Display.main.systemWidth; //Ширина экрана
        StandartHeight = Display.main.systemHeight; //Высота экрана
        LoadSettings(); //Загружаем настройки
        ApplyVideoMode();
        foreach (AfterStart x in FindObjectsOfType<AfterStart>())
            x.SetActive(HasStarted);
	}
	
	void Update () 
    {
        if (Input.GetKeyDown(KeyCode.F)) //Если нажата клавиша F
        {
            SwitchScreenMode(); //Сменяем режим экрана
        }
	}

    static public void SwitchScreenMode() //Функция смены режима экрана
    {
        FullScreen = !FullScreen; //Сменяем режим
        ApplyVideoMode(); //Применяем видеорежим

    }

    static void ApplyVideoMode() //Функция применения видеорежима
    {
        if (FullScreen) //Если полноэкранный режим
            Screen.SetResolution(StandartWidth, StandartHeight, FullScreen); //Применяем его
        else //Иначе
            Screen.SetResolution(WindowedWidth, WindowedHeight, FullScreen); //Применяем оконный режим
    }

    static public float GetField(string field) //Функция получения значения настройки
    {
        switch (field) //В зависимости от запрашиваемого поля
        {
            case "Music":
                return MusicVolume;
            case "Environment":
                return EnvironmentVolume;
            case "Sound":
                return SoundVolume;
            case "TextSpeed":
                return TextSpeed;
            case "AutoSpeed":
                return AutoSpeed;
        } //Возвращаем нужное значение
        return 0;
    }

    static public bool GetField(string field, bool varIsBool) //Функция получения значения настройки
    {
        switch (field) //В зависимости от запрашиваемого поля
        {
            case "Fullscreen":
                return FullScreen;
            case "SkipPassedOnly":
                return SkipPassedOnly;
            case "StopSkipAfterChoice":
                return StopSkipAfterChoice;
        } //Возвращаем нужное значение
        return false;
    }

    static public void SetField(string field, float value) //Функция установки значения настройки
    {
        switch (field) //В зависимости от названия поля
        {
            case "Music":
                MusicVolume = value;
                break;
            case "Environment":
                EnvironmentVolume = value;
                break;
            case "Sound":
                SoundVolume = value;
                break;
            case "TextSpeed":
                TextSpeed = value;
                break;
            case "AutoSpeed":
                AutoSpeed = value;
                break;
        } //Устанавливаем значение в нужное поле
        if (Loaded) //Если настройки были загружены
            SaveSettings(); //Сохраняем настройки
    }

    static public void SetField(string field, bool value) //Функция установки значения настройки
    {
        switch (field) //В зависимости от названия поля
        {
            case "Fullscreen":
                FullScreen = value;
                ApplyVideoMode();
                break;
            case "SkipPassedOnly":
                SkipPassedOnly = value;
                break;
            case "StopSkipAfterChoice":
                StopSkipAfterChoice = value;
                break;
        } //Устанавливаем значение в нужное поле
        if (Loaded) //Если настройки были загружены
            SaveSettings(); //Сохраняем настройки
    }

    static public string GetData() //Функция получения данных о настройках
    {
        return PlayerPrefs.GetString(SettingsKey); //Возвращаем результат
    }

    static public void SetData(string data) //Функция установки настроек
    {
        PlayerPrefs.SetString(SettingsKey, data); //Записываем данные
        LoadSettings(); //Загружаем данные
    }

    static void SaveSettings() //Функция сохранения настроек
    {
        string result = "";
        result += "Music " + MusicVolume + "\n";
        result += "Environment " + EnvironmentVolume + "\n";
        result += "Sound " + SoundVolume + "\n";
        result += "TextSpeed " + TextSpeed + "\n";
        result += "AutoSpeed " + AutoSpeed + "\n";
        result += "Fullscreen " + FullScreen + "\n";
        result += "SkipPassedOnly " + SkipPassedOnly + "\n";
        result += "StopSkipAfterChoice " + StopSkipAfterChoice + "\n";
        result += "HasStarted " + HasStarted; //Формируем строку с настройками
        PlayerPrefs.SetString(SettingsKey, result); //Сохраняем строку
    }

    static void LoadSettings() //Функция загрузки настроек
    {
        Loaded = true; //Помечаем, что настройки были загружены
        if (!PlayerPrefs.HasKey(SettingsKey)) //Если нет сохранённых настроек
            return; //То выход
        string[] settings = PlayerPrefs.GetString(SettingsKey).Split('\n'); //Загружаем и получаем строки с настройками
        for (int i = 0; i < settings.Length; i++) //Для всех строк с настройками
        {
            string[] data = settings[i].Split(' '); //Разделяем на название и значение
            switch (data[0]) //В зависимости от названия
            {
                case "Music":
                    MusicVolume = float.Parse(data[1]);
                    break;
                case "Environment":
                    EnvironmentVolume = float.Parse(data[1]);
                    break;
                case "Sound":
                    SoundVolume = float.Parse(data[1]);
                    break;
                case "TextSpeed":
                    TextSpeed = float.Parse(data[1]);
                    break;
                case "AutoSpeed":
                    AutoSpeed = float.Parse(data[1]);
                    break;
                case "Fullscreen":
                    FullScreen = bool.Parse(data[1]);
                    break;
                case "SkipPassedOnly":
                    SkipPassedOnly = bool.Parse(data[1]);
                    break;
                case "StopSkipAfterChoice":
                    StopSkipAfterChoice = bool.Parse(data[1]);
                    break;
                case "HasStarted":
                    HasStarted = bool.Parse(data[1]);
                    break;
            } //Устанавливаем значение в нужное поле
        }
    }
}
