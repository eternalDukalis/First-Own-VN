using UnityEngine;
using System.Collections;

public class MusicPlayer : MonoBehaviour {

    public GameObject MusicScreen; //Экран музыкальной галереи
    public AudioClip DefaultMusic; //Основная музыка главного меню
    MusicItem lastItem;
    AudioSource source; //Источник музыки
    string CurrentMusic = ""; //Текущая музыка
    const string MusicPath = "Music/"; //Путь к музыке
    bool inScreen; //Находимся на экране
	void Start ()
    {
        source = GetComponent<AudioSource>(); //Получаем источник
        source.volume = Settings.MusicVolume; //Ставим громкость
	}
	
	void Update ()
    {
        if (source.volume != Settings.MusicVolume) //Если громкость изменилась
            source.volume = Settings.MusicVolume; //Ставим громкость
        if (inScreen != MusicScreen.activeSelf) //Если вошли/вышли с экрана
        {
            inScreen = MusicScreen.activeSelf; //Меняем значение
            if (!inScreen) //Если ушли с экрана
            {
                source.clip = DefaultMusic; //Возвращаем дефолтную музыку
                source.Play(); //Проигрываем
                if (lastItem != null)
                    lastItem.Turn(false);
                CurrentMusic = "";
            }
        }
	}

    public virtual void Play(MusicItem item) //Проигрывание нужной музыки
    {
        if (CurrentMusic == item.Title) //Если это та же самая музыка
            return; //Выход
        lastItem = item;
        source.clip = Resources.Load<AudioClip>(AudioManager.AudioPath + MusicPath + item.Title); //Загружаем музыку
        source.Play(); //Проигрываем
        Resources.UnloadUnusedAssets(); //Выгружаем неиспользуемые ресурсы
        CurrentMusic = item.Title; //Меняем текущую музыку
    }
}
