using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TextManager : MonoBehaviour {

    public Text TextComponent; //Компонент Text нижней текстовой формы
    public Image BottomMask; //Маска нижней формы
    public float FormTime; //Время скрытия/открытия формы
    Coroutine WorkingWithText; //Переменная для управления корутиной
    string ColorOpenTag = "<color=#00000000>"; //Открывающий тег прозрачного текста
    string ColorCloseTag = "</color>"; //Закрывающий тег прозрачного текста
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

    public void PushText(string s, string a) //Функция смены текста в форме с автором
    {
        if (TextModeBottom) //Если режим нижней формы
            pTextBottom(s, a); //То вызываем соответствующий метод
    }

    public void PushText(string s) //Функция смены текста в форме
    {
        if (TextModeBottom) //Если режим нижней формы
            pTextBottom(s); //То вызываем соответствующий метод
    }

    public void AddText(string s) //Функция добавления текста в форму
    {
        if (TextModeBottom) //Если режим нижней формы
            aTextBottom(s); //То вызываем соответствующий метод
    }

    public void TakeOffTextForm() //Функция скрытия текстовой формы
    {
        if (TextModeBottom) //Если режим нижней формы
            StartCoroutine(takeOffTextForm(BottomMask)); //То запускаем соответствующую корутину
    }
    public void ShowTextForm() //Функция открытия текстовой формы
    {
        if (TextModeBottom) //Если режим нижней формы
            StartCoroutine(showTextForm(BottomMask)); //То запускаем соответствующую корутину
    }

    IEnumerator addText() //Корутина, осуществляющая побуквенное появление текста
    {
        ScenarioManager.LockCoroutine(); //Приостанавливаем основную сценарную корутину
        float tm = 0; //Счётчик времени
        while (MainText != TextComponent.text) //Пока текст в компоненте не станет желаемым
        {
            if (ControlManager.Next()) //Если нажата клавиша продолжения
            {
                TextComponent.text = MainText; //То сразу показываем весь текст
                break; //Прерываем цикл
            }
            tm += Time.deltaTime; //Увеличиваем счётчик времени на количество прошедшего времени
            if (tm > Settings.SymbolInterval) //Если счётчик времени больше, чем фиксированное значение
            {
                tm = 0; //Обнуляем счётчик времени
                TextComponent.text = ShowSymbol(TextComponent.text); //Делаем следующий символ видимым
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

    void pTextBottom(string s, string a) //Функция смены текста в форме с автором в нижней форме
    {
        AManager.UpdateAuthor(a); //Обновляем автора
        MainText = "\t"; //Очистка текста
        AddText(s); //Добавление текста
    }
    void pTextBottom(string s) //Функция смены текста в форме в нижней форме
    {
        AManager.DeleteAuthor(); //Удаляем автора
        MainText = "\t"; //Очистка текста
        AddText(s); //Добавление текста
    }
    void aTextBottom(string s) //Функция добавления текста в нижнюю форму
    {
        TextComponent.text = MainText + " " + TransparentText(s); //Помещение прозрачного текста в компонент
        MainText += " " + s; //Добавление текста в специальную переменную
        WorkingWithText = StartCoroutine(addText()); //Старт корутины
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
