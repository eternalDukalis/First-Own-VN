using UnityEngine;
using System.Collections;

public class Settings : MonoBehaviour {

    static public float SymbolInterval = 0.02f; //Интервал между появлением букв
    static public float SkipInterval = 0.05f; //Интервал перед пропуском
    static public float MusicVolume = 1; //Громкость музыки
    static public float EnvironmentVolume = 1; //Громкость звуков окружения
    static public float SoundVolume = 1; //Громкость звуковых эффектов
    static public bool FullScreen = true; //Полноэкранный режим
    static int StandartWidth; //Ширина экрана
    static int StandartHeight; //Высота экрана
    static int WindowedWidth = 800; //Ширина экрана в оконном режиме
    static int WindowedHeight = 600; //Высота экрана в оконном режиме
	void Start () 
    {
        StandartWidth = Display.main.systemWidth; //Ширина экрана
        StandartHeight = Display.main.systemHeight; //Высота экрана
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
        if (FullScreen) //Если полноэкранный режим
            Screen.SetResolution(StandartWidth, StandartHeight, FullScreen); //Применяем его
        else //Иначе
            Screen.SetResolution(WindowedWidth, WindowedHeight, FullScreen); //Применяем оконный режим
    }
}
