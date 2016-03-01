using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class State {

    public class CharacterInfo //Информация о персонаже
    {
        public string Name; //Имя
        public string CurrentEmotion; //Текущая эмоция
        public string CurrentClothes; //Текущая одежда
        public List<string> CurrentAttributes; //Текущий набор аттрибутов
        public CharacterBehavior.Position SpritePosition; //Текущая позиция
        public bool Highlighted; //Выделен ли
        public CharacterInfo()
        { }
        public CharacterInfo(CharacterInfo cInfo) //Конструктор копирования
        {
            Name = cInfo.Name;
            CurrentEmotion = cInfo.CurrentEmotion;
            CurrentClothes = cInfo.CurrentClothes;
            CurrentAttributes = new List<string>(cInfo.CurrentAttributes);
            SpritePosition = cInfo.SpritePosition;
            Highlighted = cInfo.Highlighted;
        }
        public CharacterInfo(string cInfo) //Конструктор с загрузкой из строки
        {
            Name = State.GetField(cInfo, "name:", "emotion");
            CurrentEmotion = State.GetField(cInfo, "emotion:", "clothes");
            CurrentClothes = State.GetField(cInfo, "clothes:", "position");
            SpritePosition = (CharacterBehavior.Position)int.Parse(State.GetField(cInfo, "position:", "highlighted"));
            Highlighted = int.Parse(State.GetField(cInfo, "highlighted:", "attributes")).Equals(1);
            CurrentAttributes = new List<string>();
            if (cInfo.EndsWith("attributes"))
                return;
            string attr = cInfo.Substring(cInfo.IndexOf("attributes") + ("attributes\n").Length);
            string[] attributes = attr.Split('\n');
            for (int i = 0; i < attributes.Length; i++)
                CurrentAttributes.Add(attributes[i]);
        }
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
        public override string ToString() //Строковое представление
        {
            string result = string.Format("name:{0}\nemotion:{1}\nclothes:{2}\nposition:{3}\nhighlighted:{4}\nattributes", Name, CurrentEmotion, CurrentClothes, SpritePosition.GetHashCode(), Highlighted.GetHashCode()); //Основная информация о персонаже
            for (int i = 0; i < CurrentAttributes.Count; i++)
                result += "\n" + CurrentAttributes.ToArray()[i]; //Добавляем все текущие атрибуты
            return result; //Возвращаем результат
        }
    }

    static public State CurrentState; //Текущее состояние
    static string NewGame = "source:scene11\nstring:0\ntext:\nauthor:\nbackground:potato_field\ntextform:1\nbottomform:1\nplainscreen:False\nplainscreencolor:\nmusic:\nsound:\nsoundloop:0\nenvironment:\n\nvariables\n\nclothes\n\nactors";
    public string MainText = "\n"; //Текущий текст
    public string Author; //Текущий автор
    public string Background; //Текущий фон
    public bool TextFormIsOn; //Включена ли текстовая форма
    public bool TextFormIsBottom; //Какая из текстовых форм включена
    public string CurrentSource; //Текущий файл с инструкциями
    public int CurrentInstruction
    {
        get
        {
            return _currentInstruction;
        }
        set
        {
            Changes(); //При изменении переменной будет вызываться эта функция
            _currentInstruction = value;
        }
    }//Текущий номер инструкции
    public bool PlainScreenOn = false; //Включён ли одноцветный экран
    public string PlainScreenColor = ""; //Цвет одноцветного экрана
    public List<CharacterInfo> Chars; //Информация об актёрах на сцене
    public Dictionary<string, string> Clothes; //Одёжки персонажей
    public string Music = ""; //Текущая музыка
    public string Environment = ""; //Текущие звуки окружения
    public string Sound = ""; //Текущие звуковые эффекты
    public bool SoundLoop = false; //Зациклены ли звуковые эффекты
    Dictionary<string, int> Variables; //Словарь переменных
    int _currentInstruction; //Текущий номер инструкции
    public State PreviousState; //Предыдущее состояние
    static State()
    {
        //string stateInfo = "source:scene14\nstring:8\ntext:	 Интересно, получилось ли.\nauthor:\nbackground:belov_day\ntextform:1\nbottomform:1\nplainscreen:False\nplainscreencolor:\nmusic:\nsound:\nsoundloop:0\nenvironment:\n\nvariables\nsonya:2\n\nclothes\nKsenia:Work\n\nactors\n======\nname:Ksenia\nemotion:1normal\nclothes:Work\nposition:1\nhighlighted:1\nattributes\ncatears";
        //State.CurrentState = new State(stateInfo);
        CurrentState = new State(NewGame); //Инициализация текущего состояния
    }
    public State()
    {
        Chars = new List<CharacterInfo>(); //Инициализация списка актёров
        Clothes = new Dictionary<string, string>(); //Инициализация словара одёжок персонажей
        Variables = new Dictionary<string, int>(); //Инициализация словаря переменных
    }

    public State(State state) //Конструктор копирования
    {
        MainText = state.MainText;
        Author = state.Author;
        Background = state.Background;
        TextFormIsOn = state.TextFormIsOn;
        TextFormIsBottom = state.TextFormIsBottom;
        CurrentSource = state.CurrentSource;
        _currentInstruction = state._currentInstruction;
        PlainScreenOn = state.PlainScreenOn;
        PlainScreenColor = state.PlainScreenColor;
        Clothes = new Dictionary<string, string>(state.Clothes);
        Music = state.Music;
        Environment = state.Environment;
        Sound = state.Sound;
        SoundLoop = state.SoundLoop;
        Variables = new Dictionary<string, int>(state.Variables);
        Chars = new List<CharacterInfo>();
        for (int i = 0; i < state.Chars.Count; i++)
            Chars.Add(new CharacterInfo(state.Chars.ToArray()[i]));
    }

    public State(string state) //Конструктор с загрузкой из строки
    {
        CurrentSource = GetField(state, "source:", "string");
        _currentInstruction = int.Parse(GetField(state, "string:", "text")) - 1;
        MainText = GetField(state, "text:", "author:");
        Author = GetField(state, "author:", "background");
        Background = GetField(state, "background:", "textform");
        TextFormIsOn = int.Parse(GetField(state, "textform:", "bottomform")).Equals(1);
        TextFormIsBottom = int.Parse(GetField(state, "bottomform:", "plainscreen")).Equals(1);
        PlainScreenOn = bool.Parse(GetField(state, "plainscreen:", "plainscreencolor"));
        PlainScreenColor = GetField(state, "plainscreencolor:", "music");
        Music = GetField(state, "music:", "sound");
        Sound = GetField(state, "sound:", "soundloop");
        SoundLoop = int.Parse(GetField(state, "soundloop:", "environment")).Equals(1);
        Environment = GetField(state, "environment:");
        Variables = new Dictionary<string, int>();
        if (GetField(state, "variables", "\n\n") != "")
        {
            string[] vars = GetField(state, "variables\n", "\n\n").Split('\n');
            for (int i = 0; i < vars.Length; i++)
            {
                string[] pair = vars[i].Split(':');
                Variables.Add(pair[0], int.Parse(pair[1]));
            }
        }
        Clothes = new Dictionary<string,string>();
        if (GetField(state, "clothes", "\n\n") != "")
        {
            string[] clths = GetField(state, "clothes\n", "\n\n").Split('\n');
            for (int i = 0; i < clths.Length; i++)
            {
                if (clths[i] == "")
                    break;
                string[] pair = clths[i].Split(':');
                Clothes.Add(pair[0], pair[1]);
            }
        }
        Chars = new List<CharacterInfo>();
        string[] sep = { "\n======\n" };
        string[] actors = state.Substring(state.IndexOf("actors") + ("actors").Length).Split(sep, System.StringSplitOptions.RemoveEmptyEntries);
        for (int i = 0; i < actors.Length; i++)
            Chars.Add(new CharacterInfo(actors[i]));
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

    public void IncreaseVariable(string var) //Функция увеличения переменной
    {
        if (!Variables.ContainsKey(var)) //Если переменной нет в словаре
            Variables.Add(var, 0); //То она создаётся
        Variables[var]++; //Значение переменной увеличивается на 1
    }

    public bool VariableGetsValue(string var, int value) //Достигла ли переменная значения
    {
        if ((Variables.ContainsKey(var)) && (Variables[var] >= value)) //Если переменная есть в словаре и её значение больше либо равно запрашиваемого
            return true; //То истина
        return false; //Иначе ложь
    }

    public override string ToString()
    {
        string result = "";
        result += "source:" + CurrentSource + "\n";
        result += "string:" + CurrentInstruction + "\n";
        result += "text:" + MainText + "\n";
        result += "author:" + Author + "\n";
        result += "background:" + Background + "\n";
        result += "textform:" + TextFormIsOn.GetHashCode() + "\n";
        result += "bottomform:" + TextFormIsBottom.GetHashCode() + "\n";
        result += "plainscreen:" + PlainScreenOn.ToString() + "\n";
        result += "plainscreencolor:" + PlainScreenColor + "\n";
        result += "music:" + Music + "\n";
        result += "sound:" + Sound + "\n";
        result += "soundloop:" + SoundLoop.GetHashCode() + "\n";
        result += "environment:" + Environment + "\n";
        result += "\nvariables";
        foreach (KeyValuePair<string, int> x in Variables)
        {
            result += "\n" + x.Key + ":" + x.Value;
        }
        result += "\n\nclothes";
        foreach (KeyValuePair<string, string> x in Clothes)
        {
            result += "\n" + x.Key + ":" + x.Value;
        }
        result += "\n\nactors";
        foreach (CharacterInfo x in Chars)
        {
            result += "\n======\n" + x.ToString();
        }
        //Формируем строку по формату
        return result; //Возвращаем результат
    }

    void Changes() //При каждом изменении состояния
    {
        PreviousState = new State(this); //Сохраняем предыдущее состояние
        Debug.Log(PreviousState.ToString());
    }

    static public string GetField(string source, string field, string nextfield) //Получение значения поля из строки
    {
        string nsource = source.Substring(source.IndexOf(field) + field.Length); //Отбрасываем начало
        nsource = nsource.Remove(nsource.IndexOf(nextfield)); //Отбрасываем конец
        while (nsource.EndsWith("\n")) //Пока в конце есть знаки перевода строки
            nsource = nsource.Remove(nsource.Length - 1); //Удаляем последний символ
        return nsource; //Возвращаем значение
    }

    static public string GetField(string source, string field) //Получение значения поля из строки
    {
        return GetField(source, field, "\n"); //Возвращаем значение
    }
}
