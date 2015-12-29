﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TextManager : MonoBehaviour {

    public Text TextComponent; //Компонент Text нижней текстовой формы
    public Text FullTextComponent; //Компонент Text полной текстовой формы
    public Image BottomMask; //Маска нижней формы
    public Image FullMask; //Маска полной формы
    public float FormTime; //Время скрытия/открытия формы
    string ColorOpenTag = "<color=#00000000>"; //Открывающий тег прозрачного текста
    string ColorCloseTag = "</color>"; //Закрывающий тег прозрачного текста
    string BoldOpenTag = "<b>"; //Открывающий тег жирного текста
    string BoldCloseTag = "</b>"; //Закрывающий тег жирного текста
    AuthorManager AManager; //Компонень для управления формой автора
    Text TextBottom; //Текст нижней формы
    static string MainText = "\t"; //Статическая переменная, хранящая текущий текст
    static bool TextModeBottom = true; //Статическая переменная, хранящая режим текста
	void Start () 
    {
        AManager = GameObject.Find("MAINAUTHOR").GetComponent<AuthorManager>(); //Находим компонент на сцене
	}
	
	void Update () 
    {
	
	}

    public void SwitchTextMode()
    {
        TextModeBottom = !TextModeBottom;
    }

    public void PushText(string s, string a) //Функция смены текста в форме с автором
    {
        if (TextModeBottom) //Если режим нижней формы
            pTextBottom(s, a); //То вызываем соответствующий метод
        else //Если режим полной формы
            pTextFull(s, a); //То вызываем соответствующий метод
    }
    public void PushText(string s) //Функция смены текста в форме
    {
        if (TextModeBottom) //Если режим нижней формы
            pTextBottom(s); //То вызываем соответствующий метод
        else //Если режим полной формы
            pTextFull(s); //То вызываем соответствующий метод
    }
    public void AddText(string s) //Функция добавления текста в форму
    {
        if (TextModeBottom) //Если режим нижней формы
            aTextBottom(s); //То вызываем соответствующий метод
        else //Если режим полной формы
            aTextFull(s); //То вызываем соответствующий метод
    }
    public void ClearText() //Функция очистки полного текстового поля
    {
        MainText = "\t"; //Обнуляем текст
        FullTextComponent.text = "\t"; //Обнуляем текст
    }

    public void TakeOffTextForm() //Функция скрытия текстовой формы
    {
        if (TextModeBottom) //Если режим нижней формы
            StartCoroutine(takeOffTextForm(BottomMask)); //То запускаем соответствующую корутину
        else //Если режим полной формы
            StartCoroutine(takeOffTextForm(FullMask)); //То запускаем соответствующую корутину
    }
    public void ShowTextForm() //Функция открытия текстовой формы
    {
        if (TextModeBottom) //Если режим нижней формы
            StartCoroutine(showTextForm(BottomMask)); //То запускаем соответствующую корутину
        else //Если режим полной формы
            StartCoroutine(showTextForm(FullMask)); //То запускаем соответствующую корутину
    }

    IEnumerator addText(Text txt) //Корутина, осуществляющая побуквенное появление текста
    {
        ScenarioManager.LockCoroutine(); //Приостанавливаем основную сценарную корутину
        float tm = 0; //Счётчик времени
        while (MainText != txt.text) //Пока текст в компоненте не станет желаемым
        {
            if (ControlManager.Next()) //Если нажата клавиша продолжения
            {
                txt.text = MainText; //То сразу показываем весь текст
                break; //Прерываем цикл
            }
            tm += Time.deltaTime; //Увеличиваем счётчик времени на количество прошедшего времени
            if (tm > Settings.SymbolInterval) //Если счётчик времени больше, чем фиксированное значение
            {
                tm = 0; //Обнуляем счётчик времени
                txt.text = ShowSymbol(txt.text); //Делаем следующий символ видимым
            }
            yield return null; //Смена кадра
        }
        while (!ControlManager.Next()) //Ожидание нажатия клавиши продолжения
            yield return null; //Смена кадра
        ScenarioManager.UnlockCoroutine(); //Возобновляем основную сценарную корутину
    }

    IEnumerator takeOffTextForm(Image mask) //Корутина, убирающая текстовую форму
    {
        ScenarioManager.LockCoroutine(); //Приостанавливем основную сценарную корутину
        while (mask.fillAmount > 0) //Пока заполненность больше 0
        {
            mask.fillAmount -= Time.deltaTime / FormTime; //Уменьшаем заполненность
            yield return null; //Смена кадра
        }
        if (TextModeBottom) //Если режим нижней формы
        {
            AManager.DeleteAuthor(); //Удаляем автора
            TextComponent.text = "\t"; //Обнуляем текст
            MainText = "\t"; //Обнуляем текст
        }
        else
        {
            FullTextComponent.text = "\t"; //Обнуляем текст
            MainText = "\t"; //Обнуляем текст
        }
        ScenarioManager.UnlockCoroutine(); //Возобновляем основную сценарную корутину
    }
    IEnumerator showTextForm(Image mask) //Корутина, показывающая текстовую форму
    {
        ScenarioManager.LockCoroutine(); //Приостанавливем основную сценарную корутину
        while (mask.fillAmount < 1) //Пока заполненность больше 0
        {
            mask.fillAmount += Time.deltaTime / FormTime; //Уменьшаем заполненность
            yield return null; //Смена кадра
        }
        ScenarioManager.UnlockCoroutine(); //Возобновляем основную сценарную корутину
    }

    void pTextBottom(string s, string a) //Функция смены текста в нижней форме с автором
    {
        AManager.UpdateAuthor(a); //Обновляем автора
        MainText = "\t"; //Очистка текста
        aTextBottom(s); //Добавление текста
    }
    void pTextBottom(string s) //Функция смены текста в нижней форме
    {
        AManager.DeleteAuthor(); //Удаляем автора
        MainText = "\t"; //Очистка текста
        aTextBottom(s); //Добавление текста
    }
    void aTextBottom(string s) //Функция добавления текста в нижнюю форму
    {
        TextComponent.text = MainText + " " + TransparentText(s); //Помещение прозрачного текста в компонент
        MainText += " " + s; //Добавление текста в специальную переменную
        StartCoroutine(addText(TextComponent)); //Старт корутины
    }

    void pTextFull(string s, string a) //Функция смены текста в полной форме с автором
    {
        FullTextComponent.text += "\n\t" + BoldOpenTag + a + ":" + BoldCloseTag; //Добавляем автора
        MainText += "\n\t" + BoldOpenTag + a + ":" + BoldCloseTag; //Добавляем автора
        aTextFull(s); //Добавляем текст
    }
    void pTextFull(string s) //Функция смены текста в нижней форме
    {
        string cur = "\n\t" + s; //Делаем перенос
        aTextFull(cur); //Добавляем текст
    }
    void aTextFull(string s) //Функция добалвения текста в полную форму
    {
        FullTextComponent.text = FullTextComponent.text + " " + TransparentText(s); //Помещение прозрачного текста в компонент
        MainText += " " + s; //Добавление текста в специальную переменную
        StartCoroutine(addText(FullTextComponent)); //Старт корутины
    }

    string TransparentText(string s) //Получение прозрачного текста
    {
        return ColorOpenTag + s + ColorCloseTag; //Возвращаем строку, полученную по формуле <Открывающий тег>Строка</Закрывающий тег>
    }

    string ShowSymbol(string s) //Функция, которая делает следующий символ видимым
    {
        string cur = s; //Создаём промежуточную переменную
        int StartTag = cur.IndexOf(ColorOpenTag); //Находим индекс начала открывающего тега
        if (StartTag < 0) //Если не нашли
            return cur; //То просто возвращаем то, что есть
        if (StartTag + ColorOpenTag.Length + ColorCloseTag.Length == cur.Length) //Если внутри тег ничего нет
        {
            cur = cur.Remove(StartTag); //Удаляем тег
            return cur; //Возвращаем полученный результат
        }
        char ch = cur[StartTag + ColorOpenTag.Length]; //Получаем перемещаемый символ
        cur = cur.Remove(StartTag + ColorOpenTag.Length, 1); //Удаляем символ
        cur = cur.Insert(StartTag, ch.ToString()); //Вставляем его перед тегом
        return cur; //Возвращаем результат
    }
}
