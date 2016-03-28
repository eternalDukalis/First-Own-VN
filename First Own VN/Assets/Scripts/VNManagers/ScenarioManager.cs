using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class ScenarioManager : MonoBehaviour {

    public string StartText; //Стартовый текст со сценарием
    string[] Instructions; //Команды для выполнения
    int CurrentInstruction //Текущая команда (привязана в State.CurrentInstruction) 
    {
        get
        {
            return State.CurrentState.CurrentInstruction;
        }
        set
        {
            State.CurrentState.CurrentInstruction = value;
        }
    }
    static bool CanDoNext = true; //Сверяемая булева переменная для приостановки/возобновления основной сценарной корутины 
    static public bool PlayingMode = true; //Находимся в режиме проигрывания
    int finalCode = 13; //Код конечного символа
    TextManager TManager; //Компонент для управления текстовой формой
    BackgroundManager BManager; //Компонент для управления задним фоном
    SelectionManager SManager; //Компонент для управления выбором
    EffectsManager EManager; //Компонент для управления эффектами
    ScreensManager ScrManager; //Компонент для управления вставками
    CharacterManager CManager; //Компонент для управления спрайтами персонажей
    AudioManager AManager; //Компонент для управления звуком
    AchManager AchivManager; //Компонент для показа ачивок
    MiniGamesManager MGManager; //Компонент для мини-игр
    string ScenarioPath = "Scenario/"; //Директория, где лежит сценарий

    static Coroutine CoroutineManager = null; //Переменная для управления основной сценарной корутиной
    static IEnumerator staticCoroutine; //Статическая ссылка на основную сценарную корутину
	void Start () 
    {
        TManager = GameObject.Find("TEXTMANAGER").GetComponent<TextManager>(); //Находим компонент на сцене
        BManager = GameObject.Find("BACKGROUND").GetComponent<BackgroundManager>(); //Находим компонент на сцене
        SManager = GameObject.Find("CommonObject").GetComponent<SelectionManager>(); //Находим компонент на сцене
        EManager = GameObject.Find("CommonObject").GetComponent<EffectsManager>(); //Находим компонент на сцене
        ScrManager = GameObject.Find("CommonObject").GetComponent<ScreensManager>(); //Находим компонент на сцене
        CManager = GameObject.Find("CommonObject").GetComponent<CharacterManager>(); //Находим компонент на сцене
        AchivManager = GameObject.Find("CommonObject").GetComponent<AchManager>(); //Находим компонент на сцене
        AManager = GameObject.Find("Audio").GetComponent<AudioManager>(); //Находим компонент на сцене
        MGManager = GameObject.Find("CommonObject").GetComponent<MiniGamesManager>(); //Находим компонент на сцене
        TextAsset newtext = Resources.Load<TextAsset>(ScenarioPath + State.CurrentState.CurrentSource); //Загружаем файл
        ReadInstructions(newtext); //Загружаем инструкции
        staticCoroutine = MainScenarioCoroutine(); //Привязываем корутину к переменной
        CoroutineManager = StartCoroutine(staticCoroutine); //Старт основной сценарной корутины
        PlayingMode = true; //Обнуляем переменную
        CanDoNext = true; //Обнуляем переменную
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
        BManager.ChangeBackground(State.CurrentState.Background, 0); //Ставим стандартный фон
        yield return StartCoroutine(WaitNext()); //Ждём, пока можно будет начать
        while (CurrentInstruction < Instructions.Length) //Пока не кончились команды
        {
            Skip.AddPassed(State.CurrentState.CurrentSource, State.CurrentState.CurrentInstruction); //Добавляем строку в списки
            CurrentInstruction++; //Увеличиваем счётчик инструкций
            if (CurrentInstruction >= Instructions.Length) //Если инструкции закончились
                break; //То выход из цикла
            string[] operation = Instructions[CurrentInstruction].Split('|'); //Разделяем команду по символу "|"
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
                    case "hidetf": //Если нужно скрыть текстовую форму
                        TManager.TakeOffTextForm(); //Вызываем нужный метод в менеджере текста
                        yield return StartCoroutine(WaitNext()); //Ждём, пока можно будет продолжать
                        break;
                    case "showtf": //Если нужно открыть текстовую форму
                        TManager.ShowTextForm(); //Вызываем нужный метод в менеджере текста
                        yield return StartCoroutine(WaitNext()); //Ждём, пока можно будет продолжать
                        break;
                    case "switchtextform": //Если нужно сменить режим текста
                        TManager.TakeOffTextForm(); //Скрываем старую форму
                        yield return StartCoroutine(WaitNext()); //Ждём, пока можно будет продолжать
                        TManager.SwitchTextMode(); //Сменяем режим текста
                        TManager.ShowTextForm(); //Показываем новую форму
                        yield return StartCoroutine(WaitNext()); //Ждём, пока можно будет продолжать
                        break;
                    case "clearform": //Если нужно очистить полную текстовую форму
                        TManager.ClearText(); //Очищаем форму
                        break;
                    case "changeclothes": //Если нужно сменить одежду персонажа
                        CManager.ChangeClothes(operation[1], operation[2]); //Меняем одежду
                        break;
                    case "setactor": //Если нужно вставить персонажа
                        if (CManager.HasActor(operation[1])) //Если уже есть этот персонаж
                            CManager.ForceDeleteActor(operation[1]); //Продолжаем
                        switch (operation.Length) //В зависимости от количества параметров
                        {
                            case 3: //Если 2 параметра
                                CManager.SetActor(operation[1], "center", operation[2]); //То показываем персонажа в центре
                                break;
                            case 4: //Если 3 параметра
                                CManager.SetActor(operation[1], operation[2], operation[3]); //То показываем персонажа в нужном месте
                                break;
                            case 5: //Если 4 параметра
                                CManager.SetActor(operation[1], operation[2], operation[3], operation[4]); //То персонаж выезжает с одной из сторон в нужно место
                                break;
                        }
                        break;
                    case "delactor": //Если нужно удалить персонажа
                        switch (operation.Length) //в зависимости от количества параметров
                        {
                            case 2: //Если 1 параметр
                                CManager.DeleteActor(operation[1]); //Удаляем персонажа
                                break;
                            case 3: //Если 2 параметра
                                CManager.DeleteActor(operation[1], operation[2]); //Удаляем персонажа в нужную сторону
                                break;
                        }
                        break;
                    case "highlight": //Если нужно выделить персонажа
                        CManager.Highlight(operation[1]); //Выделяем персонажа
                        yield return StartCoroutine(WaitNext()); //Ждём
                        break;
                    case "unhighlight": //Если нужно снять выделение персонажа
                        CManager.Unhighlight(operation[1]); //Снимаем выделение персонажа
                        yield return StartCoroutine(WaitNext()); //Ждём
                        break;
                    case "attribute": //Если нужно установить атрибут
                        CManager.SetAttribute(operation[1], operation[2]); //Устанавливаем атрибут
                        break;
                    case "delattribute": //Если нужно убрать атрибут
                        CManager.DeleteAttribute(operation[1], operation[2]); //Удаляем атрибут
                        break;
                    case "moveactor": //Если нужно переместить персонажа
                        CManager.MoveActor(operation[1], operation[2]); //Перемещаем персонажа
                        break;
                    case "changeemo": //Если нужно сменить эмоцию персонажа
                        CManager.ChangeEmotion(operation[1], operation[2]); //Меняет эмоцию
                        break;
                    case "music": //Если нужно включить музыку
                        AManager.Play("Music", operation[1]); //Включаем музыку
                        State.CurrentState.Music = operation[1]; //Обновляем состояние
                        break;
                    case "musicoff": //Если нужно выключить музыку
                        AManager.Stop("Music"); //Выключаем музыку
                        State.CurrentState.Music = ""; //Обновляем состояние
                        break;
                    case "env": //Если нужно включить звуки окружения
                        AManager.Play("Environment", operation[1]); //Включаем звуки окружения
                        State.CurrentState.Environment = operation[1]; //Обновляем состояние
                        break;
                    case "envoff": //Если нужно выключить звуки окружения
                        AManager.Stop("Environment"); //Выключаем звуки окружения
                        State.CurrentState.Environment = ""; //Обновляем состояние
                        break;
                    case "sound": //Если нужно включить звуковые эффекты
                        AManager.Play("Sound", operation[1]); //Включаем звуковые эффекты
                        State.CurrentState.Sound = operation[1]; //Обновляем состояние
                        State.CurrentState.SoundLoop = false; //Обновляем состояние
                        break;
                    case "soundoff": //Если нужно выключить звуковые эффекты
                        AManager.Stop("Sound"); //Выключаем звуковые эффекты
                        State.CurrentState.Sound = ""; //Обновляем состояние
                        break;
                    case "soundloop": //Если нужно зациклить проигрывание звуковых эффектов
                        AManager.SetLoop("Sound", true); //Зацикливаем
                        State.CurrentState.SoundLoop = true; //Обновляем состояние
                        break;
                    case "showscr": //Если нужно показать одноцветный экран
                        EManager.PlainScreenOn(operation[1]); //Вызываем функцию показывания экрана
                        yield return StartCoroutine(WaitNext()); //Ждём, пока можно будет продолжать
                        break;
                    case "hidescr": //Если нужно убрать одноцветный экран
                        EManager.PlainScreenOff(); //Вызываем функцию скрытия экрана
                        yield return StartCoroutine(WaitNext()); //Ждём, пока можно будет продолжать
                        break;
                    case "jolt": //Если требуется тряска
                        EManager.Jolt(); //Тряска
                        break;
                    case "newday": //Если вставка "новый день"
                        ScrManager.NewDay(operation[1]); //Запускаем соответствующую вставку
                        yield return StartCoroutine(WaitNext()); //Ждём продолжения
                        break;
                    case "opening": //Если замена опенинга
                        ScrManager.Opening(); //Запускаем соответствующую вставку
                        yield return StartCoroutine(WaitNext()); //Ждём продолжения
                        break;
                    case "episode": //Если стартовая заставка эпизода
                        ScrManager.NewEpisode(int.Parse(operation[1])); //Запускаем соответствующую вставку
                        yield return StartCoroutine(WaitNext()); //Ждём продолжения
                        break;
                    case "end": //Если конечная заставка
                        ScrManager.End(operation[1], operation[2]); //Запускаем соответствующую вставку
                        yield return StartCoroutine(WaitNext()); //Ждём продолжения
                        break;
                    case "epilogue": //Если заставка эпилога
                        ScrManager.Inscription(operation[1], operation[2]); //Запускаем соответствующую вставка
                        yield return StartCoroutine(WaitNext()); //Ждём продолжения
                        break;
                    case "inscription": //Если надпись
                        ScrManager.Inscription(operation[1]); //Запускаем соответствующую вставку
                        yield return StartCoroutine(WaitNext()); //Ждём продолжения
                        break;
                    case "goto": //Если нужно перейти на другой источник инструкций
                        ChangeSource(operation[1]); //Пользуем соответствующим методом
                        break;
                    case "incvar": //Если нужно увеличить значение переменной
                        State.CurrentState.IncreaseVariable(operation[1]); //Увеличиваем значение переменной
                        break;
                    case "ifgoto": //Если нужно сделать условный переход
                        if (State.CurrentState.VariableGetsValue(operation[1], int.Parse(operation[2]))) //Если переменная достигла значения
                            ChangeSource(operation[3]); //Переходим на другой файл
                        break;
                    case "select": //Если нужно выбрать из нескольких вариантов
                        string[] texts = new string[(operation.Length - 1) / 2]; //Инициализируем массив вариантов выбора
                        string[] targets = new string[(operation.Length - 1) / 2]; //Инициализируем массив возможных переходов
                        for (int i = 1; i < operation.Length; i += 2)
                            texts[(i - 1) / 2] = operation[i]; //Заполняем массив вариантов выбора
                        for (int i = 2; i < operation.Length; i += 2)
                            targets[i / 2 - 1] = operation[i]; //Заполняем массив возможных переходов
                        SManager.SetSelection(texts, targets); //Выводм выборы
                        yield return StartCoroutine(WaitNext()); //Ждём, пока можно будет продолжать
                        ChangeSource(SelectionManager.NextTargert); //Осуществляем переход
                        break;
                    case "pushcg": //Если нужно добавить фон в галерею
                        CGGallery.Push(operation[1]); //Пушим
                        break;
                    case "ach": //Если ачивка
                        AchivManager.Push(operation[1], operation[2]); //Пытаемся запушить
                        break;
                    case "minigame": //Если мини-игра
                        MGManager.PlayGame(int.Parse(operation[1])); //Запускаем нужную
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

    void ChangeSource(string name) //Функция перехода на другой источник инструкций
    {
        Skip.AddPassed(State.CurrentState.CurrentSource, State.CurrentState.CurrentInstruction); //Добавляем строку в списки
        State.CurrentState.CurrentSource = name; //Записываем состояние
        TextAsset newtext = Resources.Load<TextAsset>(ScenarioPath + name); //Загружаем файл
        CurrentInstruction = -1; //Обнуляем счётчик инструкций
        ReadInstructions(newtext); //Считываем инструкции из файла
        Resources.UnloadUnusedAssets(); //Выгружаем неиспользуемые ресурсы
    }

    IEnumerator WaitNext() //Корутина ожидания возобновления основной сценарной корутины
    {
        while ((!CanDoNext) || (!PlayingMode)) //Пока сверяемая булева переменная равна false
            yield return null; //Ждём
    }

    static public void GoToMainMenu() //Функция перехода к главному меню
    {
        SceneManager.LoadScene("menu");
    }

    static public void LockCoroutine() //Функция приостановки основной сценарной корутины
    {
        CanDoNext = false; //Сверяемая булева переменная равна false
    }

    static public void UnlockCoroutine() //Функция возобновлени основной сценарной корутины
    {
        CanDoNext = true; //Сверяемая булева переменная равна true
    }

    static public bool GetCanDoNext() //Функция, возвращающая значение сверяемой булевой переменной
    {
        return CanDoNext; //Возвращаем результат
    }

    static public void StopScenario() //Функция для приостановки проигрывания сценария
    {
        if (CoroutineManager != null) 
            GameObject.FindObjectOfType<ScenarioManager>().StopCoroutine(CoroutineManager); //Останавливаем корутину
    }

    static public void StartScenario() //Функция для проигрывания сценария
    {
        CoroutineManager = GameObject.FindObjectOfType<ScenarioManager>().StartCoroutine(staticCoroutine); //Стартуем корутину
    }

    string DeleteSpacesAtTheEnd(string source) //Функция удаления конечного символа в конце строки
    {
        string res = source; //Создаём промежуточную переменную
        while (((int)res[res.Length - 1] == finalCode) || (res[res.Length - 1] == ' ')) //До тех пор, пока в конце есть пробелы
        {
            res = res.Remove(res.Length - 1); //Удаляем последний символ
        }
        return res; //Возвращаем результат
    }

    public virtual void Exit()
    {
        GoToMainMenu();
    }
}
