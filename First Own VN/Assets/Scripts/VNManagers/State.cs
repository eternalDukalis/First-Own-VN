using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class State {

    struct CharacterInfo //Информация о персонаже
    {
        public string Name; //Имя
        public string CurrentEmotion; //Текущая эмоция
        public string CurrentClothes; //Текущая одежда
        public List<string> CurrentAttributes; //Текущий набор аттрибутов
        public CharacterBehavior.Position SpritePosition; //Текущая позиция
        public bool Highlighted; //Выделен ли
        public void UpdateEmotion(string emotion) //Обновление эмоции
        {
            CurrentEmotion = emotion; //Записываем эмоцию
        }
        public void UpdateAttributes(List<string> attributes) //Обновление атрибутов
        {
            CurrentAttributes = attributes; //Записываем атрибуты
        }
        public void UpdatePosition(CharacterBehavior.Position position) //Обновление позиции
        {
            SpritePosition = position; //Записываем позицию
        }
        public void UpdateHighlighted(bool highlighted) //Обновление выделения
        {
            Highlighted = highlighted; //Записываем выделение
        }
    }

    static public State CurrentState; //Текущее состояние
    public string MainText = "\n"; //Текущий текст
    public string Author; //Текущий автор
    public string Background; //Текущий фон
    public bool TextFormIsOn; //Включена ли текстовая форма
    public bool TextFormIsBottom; //Какая из текстовых форм включена
    public string CurrentSource; //Текущий файл с инструкциями
    public int CurrentInstruction; //Текущий номер инструкции
    public bool PlainScreenOn = false; //Включён ли одноцветный экран
    public string PlainScreenColor = ""; //Цвет одноцветного экрана
    List<CharacterInfo> Chars; //Информация об актёрах на сцене
    public Dictionary<string, string> Clothes; //Одёжки персонажей
    public string Music = ""; //Текущая музыка
    public string Environment = ""; //Текущие звуки окружения
    public string Sound = ""; //Текущие звуковые эффекты
    public bool SoundLoop = false; //Зациклены ли звуковые эффекты
    static State()
    {
        CurrentState = new State(); //Инициализация текущего состояния
    }
    public State()
    {
        Chars = new List<CharacterInfo>(); //Инициализация списка актёров
        Clothes = new Dictionary<string, string>(); //Инициализация словара одёжок персонажей
    }

    public void AddCharacter(string name, string currentEmotion, string currentClothes, List<string> currentAttributes, CharacterBehavior.Position spritePosition, bool highlighted) //Функция добавления информации о персонаже
    {
        CharacterInfo ci = new CharacterInfo(); //Новый объект информации о персонаже
        ci.Name = name; //Вставляем имя
        ci.CurrentEmotion = currentEmotion; //вставляем эмоцию
        ci.CurrentClothes = currentClothes; //Вставляем одежду
        ci.CurrentAttributes = currentAttributes; //Вставляем аттрибуты
        ci.SpritePosition = spritePosition; //Вставляем позицию
        ci.Highlighted = highlighted; //Вставляем выделение
        Chars.Add(ci); //Добавляем информацию в список
    }

    public void UpdateCharacterEmotion(string name, string emotion) //Обновление эмоции
    {
        Chars.Find(x => x.Name == name).UpdateEmotion(emotion); //Обновляем эмоцию
    }

    public void UpdateCharacterAttributes(string name, List<string> attributes) //Обновление атрибутов
    {
        Chars.Find(x => x.Name == name).UpdateAttributes(attributes); //Обновляем атрибуты
    }

    public void UpdateCharacterPosition(string name, CharacterBehavior.Position position) //Обновление позиции
    {
        Chars.Find(x => x.Name == name).UpdatePosition(position); //Обновляем позицию
    }

    public void UpdateCharacterHighlighted(string name, bool highlighted) //Обновление выделение
    {
        Chars.Find(x => x.Name == name).UpdateHighlighted(highlighted); //Обновляем выделение
    }

    public void DeleteCharacter(string name) //Удаление информации о персонаже
    {
        Chars.RemoveAll(x => x.Name == name); //Удаляем информацию
    }
}
