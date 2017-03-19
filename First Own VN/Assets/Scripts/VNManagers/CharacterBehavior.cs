using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class CharacterBehavior : MonoBehaviour {

    public enum Position { Left, Center, Right } //Перечислимый тип позиции

    static public string SpritesPath = "Graphics/Sprites/"; //Путь к спрайтами персонажей
    static float PlacingTime = 0.5f; //Время заполнения спрайта
    static float SpriteWidth = 0.5f; //Стандартная ширина спрайта
    static float MovingStep = 0.015f; //Длина шага при перемещении спрайта
    static RectTransform GraphicsTransform; //Родительский объект для элементов графики
    static Dictionary<Position, Vector2> XHighlightVectors; //Векторы выделения по х
    static Vector2 YHighlightVector; //Вектор выделения по y
    static int ScalingStepsNum = 40; //Количество шагов при выделении

    public Image BodySprite; //Тело персонажа
    Image ParentImage; //Родительский объект персонажа

    public string Name = ""; //Имя персонажа
    string CurrentEmotion = ""; //Текущая эмоция персонажа
    string CurrentClothes = ""; //Текущая одежда персонажа
    SortedList<string, int> CurrentAttributes; //Текущие атрибуты персонажа
    Position SpritePosition = Position.Center; //Текущая позиция персонажа
    bool Highlighted = false; //Выделен ли персонаж
    IEnumerator movcor;
    IEnumerator placecor;

    float CurrentPosition; //Позиция спрайта
	void Start () 
    {
        
	}
	
	void Update () 
    {
        
	}

    public void SetOnScene(string name, Position position, string emotion, string clothes) //Функция появления персонажа без движения
    {
        Init(); //Производим инициализацию
        if (ParentImage == null) //Если компонент Image ещё не найден
            ParentImage = GetComponent<Image>(); //То находим его
        State.CurrentState.AddCharacter(name, emotion, clothes, new List<string>(CurrentAttributes.Keys), position, Highlighted); //Обновляем состояние
        Name = name; //Записываем имя
        SpritePosition = position; //Записываем позицию
        CurrentClothes = clothes; //Записываем одежду
        CurrentEmotion = emotion; //Записываем эмоцию
        //SetEmotion(); //Ставим эмоцию
        //SetClothes(); //Ставим одежду
        SetSprite();
        CurrentPosition = GetPosition(position, true); //Вычисляем позицию спрайта
        ApplyPosition(); //Применяем позицию спрайта
        if (placecor != null)
            StopCoroutine(placecor);
        placecor = placing(true);
        StartCoroutine(placecor); //Начинаем корутину помещения
    }

    public void SetOnScene(string name, Position from, Position to, string emotion, string clothes) //Функция появления персонажа с движением
    {
        Init(); //Производим инициализацию
        if (ParentImage == null)//Если компонент Image ещё не найден
            ParentImage = GetComponent<Image>(); //То находим его
        State.CurrentState.AddCharacter(name, emotion, clothes, new List<string>(CurrentAttributes.Keys), to, Highlighted); //Обновляем состояние
        Name = name; //Записываем имя
        SpritePosition = to; //Записываем позицию
        CurrentClothes = clothes; //Записываем одежду
        CurrentEmotion = emotion; //Записываем эмоцию
        //SetEmotion(); //Ставим эмоцию
        //SetClothes(); //Ставим одежду
        SetSprite();
        CurrentPosition = GetPosition(from, false); //Вычисляем позицию спрайта
        ParentImage.fillAmount = 1; //Применяем позицию спрайта
        if (movcor != null)
            StopCoroutine(movcor);
        movcor = moving(GetPosition(to, true), false);
        StartCoroutine(movcor); //Начинаем корутину перемещения
    }

    public void DeleteFromScene() //Удаление персонажа
    {
        State.CurrentState.DeleteCharacter(Name); //Обновляем состояние
        if (placecor != null)
            StopCoroutine(placecor);
        placecor = placing(false);
        StartCoroutine(placecor); //Начинаем корутину скрытия
    }

    public void DeleteFromScene(Position to) //Удаление персонажа
    {
        State.CurrentState.DeleteCharacter(Name); //Обновляем состояние
        if (movcor != null)
            StopCoroutine(movcor);
        movcor = moving(GetPosition(to, false), true);
        StartCoroutine(movcor); //Начинаем корутину перемещения
    }

    public void ChangeEmotion(string emotion) //Смена эмоции персонажа
    {
        CurrentEmotion = emotion; //Эмоция
        State.CurrentState.UpdateCharacterEmotion(Name, CurrentEmotion); //Обновляем состояние
        //SetEmotion(); //Ставим эмоцию
        //SetClothes(); //Ставим одежду
        SetSprite();
        Resources.UnloadUnusedAssets(); //Выгружаем неиспользуемые ресурсы
    }

    public void Highlight() //Выделение персонажа
    {
        Highlighted = true; //Персонаж выделен
        State.CurrentState.UpdateCharacterHighlighted(Name, Highlighted); //Обновляем состояние
        StartCoroutine(graphicsScaling(new Vector2(XHighlightVectors[SpritePosition].x, YHighlightVector.x), new Vector2(XHighlightVectors[SpritePosition].y, YHighlightVector.y))); //Запускаем скалирование объекта с графикой
    }

    public void Unhighlight() //Снятие выделения персонажа
    {
        Highlighted = false; //Персонаж не выделен
        State.CurrentState.UpdateCharacterHighlighted(Name, Highlighted); //Обновляем состояние
        StartCoroutine(graphicsScaling(new Vector2(0, 0), new Vector2(1, 1))); //Запуск скалирования объекта с графикой
    }

    public void SetAttribute(string attribute) //Установка атрибута
    {
        CurrentAttributes.Add(attribute, 1); //Записываем атрибут в список атрибутов
        State.CurrentState.UpdateCharacterAttributes(Name, new List<string>(CurrentAttributes.Keys)); //Обновляем состояние
        SetSprite();
    }

    public void RemoveAttribute(string attribute) //Удаление атрибута
    {
        CurrentAttributes.Remove(attribute); //Удаляем атрибут из списка атрибутов
        State.CurrentState.UpdateCharacterAttributes(Name, new List<string>(CurrentAttributes.Keys)); //Обновляем состояние
        SetSprite();
    }

    public void MoveActor(Position to) //Перемещение персонажа
    {
        SpritePosition = to; //Позиция персонажа
        State.CurrentState.UpdateCharacterPosition(Name, to); //Обновляем состояние
        if (movcor != null)
            StopCoroutine(movcor);
        movcor = moving(GetPosition(SpritePosition, true), false);
        StartCoroutine(movcor); //Начинаем перемещение персонажа
    }

    IEnumerator placing(bool inc) //Корутина помещения/скрытия
    {
        BodySprite.color = new Color(BodySprite.color.r, BodySprite.color.g, BodySprite.color.b, (!inc).GetHashCode());
        while (((BodySprite.color.a < 1) && (inc)) || ((BodySprite.color.a > 0) && (!inc))) //Пока полностью не покажем/скроем
        {
            if ((Skip.isSkipping) || (CharacterManager.isLoading)) //Если пропуск или идёт загрузка
            {
                yield return new WaitForSeconds(Settings.SkipInterval); //Новый кадр
                BodySprite.color = new Color(BodySprite.color.r, BodySprite.color.g, BodySprite.color.b, inc.GetHashCode()); //Заканчиваем действие
                break; //Прерываем цикл
            }
            BodySprite.color = new Color(BodySprite.color.r, BodySprite.color.g, BodySprite.color.b, BodySprite.color.a + (2 * inc.GetHashCode() - 1) * Time.deltaTime / PlacingTime); //Добавляем/убавляем заполнение
            yield return null; //Новый кадр
        }
        if (!inc) //Если это было скрытие
            Destroy(this.gameObject); //То удаляем объект
        Resources.UnloadUnusedAssets(); //Выгружаем неиспользуемые ресурсы
    }

    IEnumerator moving(float pos, bool withDeleting) //Корутина перемещения
    {
        bool right = pos > CurrentPosition; //Вправо ли нужно двигаться
        while (right == (pos > CurrentPosition)) //Пока необходимость двигаться в определённую сторону сохраняется
        {
            if ((Skip.isSkipping)) //Если пропуск
            {
                yield return new WaitForSeconds(Settings.SkipInterval); //Новый кадр
                CurrentPosition = pos; //Ставим конечную позицию
                ApplyPosition(); //применяем позицию
                break; //Прерываем цикл
            }
            CurrentPosition += (2 * right.GetHashCode() - 1) * MovingStep; //Двигаем спрайт на шаг
            ApplyPosition(); //Применяем позицию
            yield return null; //Новый кадр
        }
        if (withDeleting) //Если объект нужно удалить
            Destroy(this.gameObject); //То удаляем его
        Resources.UnloadUnusedAssets(); //Выгружаем неиспользуемые ресурсы
    }

    IEnumerator graphicsScaling(Vector2 targetMin, Vector2 targetMax) //Скалирования объекта с графикой
    {
        Vector2 beginMin = GraphicsTransform.anchorMin; //Запоминаем изначальный anchorMin
        Vector2 beginMax = GraphicsTransform.anchorMax; //Запоминаем изначальный anchorMax
        ScenarioManager.LockCoroutine(); //Приостанавливаем сценарий
        float val = 0;
        for (int i = 0; i < ScalingStepsNum; i++) //Выполняем определённое количество шагов
        {
            if ((Skip.isSkipping) || (CharacterManager.isLoading)) //Если пропуск или загрузка
            {
                yield return new WaitForSeconds(Settings.SkipInterval); //Новый кадр
                break; //Прерываение цикла
            }
            val += 1 / (float)ScalingStepsNum;
            GraphicsTransform.anchorMin = beginMin + (targetMin - beginMin) * Mathf.Sin(Mathf.PI * val / 2); //изменяеи anchorMin
            GraphicsTransform.anchorMax = beginMax + (targetMax - beginMax) * Mathf.Sin(Mathf.PI * val / 2); //Изменяем anchorMax
            yield return null; //Новый кадр
        }
        GraphicsTransform.anchorMin = targetMin; //Ставим конечный anchorMin
        GraphicsTransform.anchorMax = targetMax; //Ставим конечный anchorMax
        ScenarioManager.UnlockCoroutine(); //Возобновляем сценарий
    }

    float GetPosition(Position pos, bool inside) //Получение числа из типа позиции
    {
        switch (pos) //В зависимости от значения
        {
            case Position.Left: //Если значение Left
                return (2 * inside.GetHashCode() - 1) * 0.25f; //То выичляем значение внутри или снаруюи в левой стороне
            case Position.Right: //Если значени Right
                return 1 - (2*inside.GetHashCode() - 1) * 0.25f; //То вычисляем значение внутри или снаружи в правой стороне
        }
        return 0.5f; //возвращаем 0.5
    }

    void ApplyPosition() //Применение позиции
    {
        ParentImage.rectTransform.anchorMin = new Vector2(CurrentPosition - SpriteWidth / 2, ParentImage.rectTransform.anchorMin.y); //Расчитываем и применяем левую границу
        ParentImage.rectTransform.anchorMax = new Vector2(CurrentPosition + SpriteWidth / 2, ParentImage.rectTransform.anchorMax.y); //Рассчитываем и применяем правую границу
    }

    void SetSprite()
    {
        string path = SpritesPath + Name + "/" + CurrentClothes + "/" + CurrentEmotion;
        foreach (KeyValuePair<string, int> x in CurrentAttributes)
        {
            path += "_" + x.Key;
        }
        Texture2D body = Resources.Load<Texture2D>(path);
        if (body == null)
            Debug.Log(CurrentEmotion);
        BodySprite.sprite = Sprite.Create(body, new Rect(0, 0, body.width, body.height), new Vector2(0, 0));
    }

    /*void SetEmotion() //Установка эмоции
    {
        Texture2D body = Resources.Load<Texture2D>(SpritesPath + Name + BodiesPath + CurrentEmotion);//Загружаем тело
        BodySprite.sprite = Sprite.Create(body, new Rect(0, 0, body.width, body.height), new Vector2(0, 0)); //Применяем тело
    }

    void SetClothes() //Установка одежды
    {
        Texture2D sclothes = Resources.Load<Texture2D>(SpritesPath + Name + ClothesPath + CurrentClothes + "/" + CurrentEmotion[0]); //Загружаем одежду
        ClothesSprite.sprite = Sprite.Create(sclothes, new Rect(0, 0, sclothes.width, sclothes.height), new Vector2(0, 0)); //Применяем одежду
    }*/

    void Init() //Инициализация параметров
    {
        CurrentAttributes = new SortedList<string, int>(); //Инициализация списка названий атрибутов
        ParentImage = GetComponent<Image>(); //Поиск компонента Image
        GraphicsTransform = GameObject.Find("Graphics").GetComponent<RectTransform>(); //Находим компонент на сцене
        XHighlightVectors = new Dictionary<Position, Vector2>(); //Инициализируем словарь
        XHighlightVectors.Add(Position.Right, new Vector2(-1, 1)); //Добавляем вектор для правой части
        XHighlightVectors.Add(Position.Center, new Vector2(-0.5f, 1.5f)); //Добавляем вектор для центральной части
        XHighlightVectors.Add(Position.Left, new Vector2(0, 2)); //Добавляем вектор для левой части
        YHighlightVector = new Vector2(-0.5f, 1.5f); //Y вектор выделения
    }
}
