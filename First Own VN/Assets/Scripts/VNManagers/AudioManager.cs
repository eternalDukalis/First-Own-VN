using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AudioManager : MonoBehaviour {

    [System.Serializable]
    public struct AudioStream //Структура звукового потока
    {
        public string Name; //Навзание потока
        public AudioSource Source; //Компонент AudioSource
        public float StandartVolume; //Стандартная громкость
        public bool StandartLoop; //Зациклено ли по умолчанию
    }
    public AudioStream[] AudioChannels; //Массив аудипотоков
    public float FadeTime = 1; //Время затухания звука
    string AudioPath = "Audio/"; //Папки с аудиофайлами
	void Start () 
    {
        SetVolumes(); //Применяем громкости
	}
	
	void Update () 
    {
	
	}

    public virtual void Play(string channel, string source) //Функция проигрывания
    {
        SetVolumes(); //Восстанавливаем громкости
        AudioStream stream = GetStream(channel); //Находим нужный аудиопоток
        stream.Source.loop = stream.StandartLoop; //Устанавливаем стандартную зацикленность
        stream.Source.clip = Resources.Load<AudioClip>(AudioPath + channel + "/" + source); //Загружаем аудиофайл
        stream.Source.Play(); //Проигрываем
        Resources.UnloadUnusedAssets(); //Освобождаем неиспользуемые ресурсы
    }

    public virtual void Stop(string channel) //Функция остановки
    {
        AudioStream stream = GetStream(channel); //Находим нужный аудиопоток
        StartCoroutine(fadeOutAudio(stream.Source)); //Начинаем корутину затухания
    }

    public virtual void SetLoop(string channel, bool newLoop) //Функция установки зацикленности
    {
        AudioStream stream = GetStream(channel); //Находим нужный аудиопоток
        stream.Source.loop = newLoop; //Устанавливаем зацикленность
    }

    IEnumerator fadeOutAudio(AudioSource source) //Корутина затухания
    {
        float curVolume = source.volume; //Текущая громкость
        while (source.volume > 0) //Пока громкость больше нуля
        {
            source.volume -= curVolume * Time.deltaTime / FadeTime; //Уменьшаем громкость
            yield return null; //Новый кадр
        }
        source.Pause(); //Ставим на паузу
    }

    AudioStream GetStream(string name) //Функция поиска нужного потока
    {
        for (int i = 0; i < AudioChannels.Length; i++) //Для всех аудиопотоков
            if (AudioChannels[i].Name == name) //Если находим нужное имя
            {
                return AudioChannels[i]; //То возвращаем поток
            }
        return new AudioStream(); //Возвращаем новый поток
    }

    void SetVolumes()  //Установка громкостей
    {
        for (int i = 0; i < AudioChannels.Length; i++) //Для всех аудиопотоков
        {
            switch (AudioChannels[i].Name) //В зависимости от имени
            {
                case "Music": //Если поток музыки
                    AudioChannels[i].Source.volume = AudioChannels[i].StandartVolume * Settings.MusicVolume; //ТО устанавливаем нужную громкость
                    break;
                case "Environment": //Если поток звуков окружения
                    AudioChannels[i].Source.volume = AudioChannels[i].StandartVolume * Settings.EnvironmentVolume; //То устанавливаем нужную громкость
                    break;
                case "Sound": //Если поток звуковых эффектов
                    AudioChannels[i].Source.volume = AudioChannels[i].StandartVolume * Settings.SoundVolume; //То устанавливаем нужную громкость
                    break;
            }
        }
    }
}
