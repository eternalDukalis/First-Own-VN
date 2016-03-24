using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AudioManager : MonoBehaviour {

    [System.Serializable]
    public class AudioStream //Структура звукового потока
    {
        public string Name; //Навзание потока
        public AudioSource Source; //Компонент AudioSource
        public float StandartVolume; //Стандартная громкость
        public bool StandartLoop; //Зациклено ли по умолчанию
    }
    public AudioStream[] AudioChannels; //Массив аудипотоков
    public float FadeTime = 1; //Время затухания звука
    string AudioPath = "Audio/"; //Папки с аудиофайлами
    bool fading = false;
	void Start () 
    {
        SetVolumes(); //Применяем громкости
        if (State.CurrentState.Music != "") //Если есть музыка
            Play("Music", State.CurrentState.Music); //То включаем музыку
        if (State.CurrentState.Environment != "") //Если есть звуки окружения
            Play("Environment", State.CurrentState.Environment); //То включаем звуки окружения
        /*if (State.CurrentState.Sound != "") //Если есть звуковые эффекты
            Play("Sound", State.CurrentState.Sound); //То включаем звуковые эффекты*/
        if (State.CurrentState.SoundLoop) //Если звуквоые эффекты нужно зациклить
            SetLoop("Sound", true); //То зацикливаем
	}
	
	void Update () 
    {
        if (VolumesMismatch()) //Если громкости не совпадают
            SetVolumes(); //Применяем громкости
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
        AudioSource oldsource = stream.Source; //Сохраняем старый источник
        stream.Source = Instantiate(oldsource); //Размещаем новый
        stream.Source.gameObject.name = stream.Name; //Меняем имя
        stream.Source.transform.SetParent(transform); //Помещаем в родительский объект
        stream.Source.Stop(); //Останавливаем
        Coroutine cor = StartCoroutine(fadeOutAudio(oldsource)); //Начинаем корутину затухания
    }

    public virtual void SetLoop(string channel, bool newLoop) //Функция установки зацикленности
    {
        AudioStream stream = GetStream(channel); //Находим нужный аудиопоток
        stream.Source.loop = newLoop; //Устанавливаем зацикленность
    }

    IEnumerator fadeOutAudio(AudioSource source) //Корутина затухания
    {
        fading = true;
        float curVolume = source.volume; //Текущая громкость
        while (source.volume > 0) //Пока громкость больше нуля
        {
            source.volume -= curVolume * Time.deltaTime / FadeTime; //Уменьшаем громкость
            yield return null; //Новый кадр
        }
        source.Pause(); //Ставим на паузу
        fading = false;
        Destroy(source.gameObject); //Удаляем объект
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

    bool VolumesMismatch() //Функция проверки несовпадения громкостей
    {
        if (fading)
            return false;
        for (int i = 0; i < AudioChannels.Length; i++) //Для всех аудиопотоков
        {
            switch (AudioChannels[i].Name) //В зависимости от имени
            {
                case "Music": //Если поток музыки
                    if (AudioChannels[i].Source.volume != Settings.MusicVolume) //Если громкости не совпадают
                        return true; //Возвращаем true
                    break;
                case "Environment": //Если поток звуков окружения
                    if (AudioChannels[i].Source.volume != Settings.EnvironmentVolume) //Если громкости не совпадают
                        return true; //Возвращаем true
                    break;
                case "Sound": //Если поток звуковых эффектов
                    if (AudioChannels[i].Source.volume != Settings.SoundVolume) //Если громкости не совпадают
                        return true; //Возвращаем true
                    break;
            }
        }
        return false; //Возваращем false
    }
}
