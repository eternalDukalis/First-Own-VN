using UnityEngine;
using System.Collections;

public class State {

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
    static State()
    {
        CurrentState = new State(); //Инициализация текущего состояния
    }
    public State()
    { 

    }
}
