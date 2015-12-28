using UnityEngine;
using System.Collections;

public class ScenarioManager : MonoBehaviour {

    public TextAsset StartText; //Стартовый текст со сценарием
    string[] Instructions; //Команды для выполнения
    int CurrentInstruction = 0; //Текущая команда
    static bool CanDoNext = true; //Сверяемая булева переменная для приостановки/возобновления основной сценарной корутины 
    int finalCode = 13; //Код конечного символа
    TextManager TManager; //Компонент для управления текстовой формой
    BackgroundManager BManager; //Компонент для управления задним фоном
	void Start () 
    {
        TManager = GameObject.Find("TEXTMANAGER").GetComponent<TextManager>(); //Находим компонент на сцене
        BManager = GameObject.Find("BACKGROUND").GetComponent<BackgroundManager>(); //Находим компонент на сцене
        ReadInstructions(StartText); //Считываем команды из файла
        StartCoroutine(MainScenarioCoroutine()); //Старт основной сценарной корутины
	}
	
	void Update () 
    {
	
	}

    void ReadInstructions(TextAsset ta) //Считывание команд из файла
    {
        Instructions = ta.text.Split('\n'); //Создаём массив команд, разделённых по символу переноса строки
        for (int i = 0; i < Instructions.Length; i++) //Для всех строк команд
            Instructions[i] = DeleteSpacesAtTheEnd(Instructions[i]); //Удаляем конечный символ в конце строки
    }

    IEnumerator MainScenarioCoroutine() //Основная сценарная корутина
    {
        while (CurrentInstruction < Instructions.Length) //Пока не кончились команды
        {
            string[] operation = Instructions[CurrentInstruction].Split('|'); //Разделяем команду по символу "|"
            CurrentInstruction++; //Увеличиваем счётчик инструкций
            if (operation.Length == 1) //Если получилась одна часть команды
            {
                TManager.PushText(operation[0]); //Сменяем текст в форме
                yield return StartCoroutine(WaitNext()); //Ждём, пока можно будет продолжать
            }
            else //Иначе
            {
                switch (operation[0]) //Смотрим на первый элемент массива (в нём указана команда)
                {
                    case "add": //Если это добавление текста
                        TManager.AddText(operation[operation.Length - 1]); //Добавляем указанный текст
                        yield return StartCoroutine(WaitNext()); //Ждём, пока можно будет продолжать
                        break;
                    case "cback": //Если это смена фона
                        if (operation.Length > 2) //Если присутствуют дополнительные параметры
                            BManager.ChangeBackground(operation[1], float.Parse(operation[2])); //То передаём эти параметры в метод
                        else //Иначе
                            BManager.ChangeBackground(operation[1]); //Сменяем фон со стандартными параметрами
                        yield return StartCoroutine(WaitNext()); //Ждём, пока можно будет продолжать
                        break;
                    default: //Если команда не распознана, то первый элемент расценивается, как обозначение автора текста. Тогда
                        TManager.PushText(operation[1], operation[0]); //Сменяем текст в форме с автором
                        yield return StartCoroutine(WaitNext()); //Ждём, пока можно будет продолжать
                        break;
                }
            }
            yield return null; //Смена кадра
        }
    }

    IEnumerator WaitNext() //Корутина ожидания возобновления основной сценарной корутины
    {
        while (!CanDoNext) //Пока сверяемая булева переменная равна false
            yield return null; //Ждём
    }

    static public void LockCoroutine() //Функция приостановки основной сценарной корутины
    {
        CanDoNext = false; //Сверяемая булева переменная равна false
    }

    static public void UnlockCoroutine() //Функция возобновлени основной сценарной корутины
    {
        CanDoNext = true; //Сверяемая булева переменная равна true
    }

    string DeleteSpacesAtTheEnd(string source) //Функция удаления конечного символа в конце строки
    {
        string res = source; //Создаём промежуточную переменную
        while ((int)res[res.Length - 1] == finalCode) //До тех пор, пока в конце есть пробелы
        {
            res = res.Remove(res.Length - 1); //Удаляем последний символ
        }
        return res; //Возвращаем результат
    }
}
