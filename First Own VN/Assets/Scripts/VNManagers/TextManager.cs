using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TextManager : MonoBehaviour {

    Text TextComponent; //Компонент Text этого объекта
    Coroutine WorkingWithText; //Переменная для управления корутиной
    string ColorOpenTag = "<color=#00000000>"; //Открывающий тег прозрачного текста
    string ColorCloseTag = "</color>"; //Закрывающий тег прозрачного текста
    static string MainText = "\t"; //Статическая переменная, хранящая текущий текст
	void Start () 
    {
        TextComponent = GetComponent<Text>(); //Получаем компонент
        PushText("Кусок говна пывавпвап ваыпвыапывапыва ывапывапывапывапыва ывапывапавыпывап ывапвыапыавпваып ыавпывпапавы вапывап");
	}
	
	void Update () 
    {
	
	}

    public void PushText(string s) //Функция смены текста в форме
    {
        MainText = "\t"; //Очистка текста
        AddText(s); //Добавление текста
    }

    public void AddText(string s) //Функция добавления текста в форму
    {
        TextComponent.text = MainText + TransparentText(s); //Помещение прозрачного текста в компонент
        MainText += s; //Добавление текста в специальную переменную
        WorkingWithText = StartCoroutine(addText()); //Старт корутины
    }

    IEnumerator addText() //Корутина, осуществляющая побуквенное появление текста
    {
        float tm = 0; //Счётчик времени
        while (MainText != TextComponent.text) //Пока текст в компоненте не станет желаемым
        {
            tm += Time.deltaTime; //Увеличиваем счётчик времени на количество прошедшего времени
            if (tm > Settings.SymbolInterval) //Если счётчик времени больше, чем фиксированное значение
            {
                tm = 0; //Обнуляем счётчик времени
                TextComponent.text = ShowSymbol(TextComponent.text); //Делаем следующий символ видимым
            }
            yield return null;
        }
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
