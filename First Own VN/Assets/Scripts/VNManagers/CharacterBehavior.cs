using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class CharacterBehavior : MonoBehaviour {

    public enum Position { Left, Center, Right } //Перечислимый тип позиции

    static string SpritesPath = "Graphics/Sprites/"; //Путь к спрайтами персонажей
    static string BodiesPath = "/Bodies/"; //Путь к телам персонажа
    static string ClothesPath = "/Clothes/"; //Путь к одежде персонажа
    static string AttributesPath = "/Attributes/"; //Путь к атрибутам персонажа
    static int FillOrigin = 1; //Стандартный метод заполнения спрайта
    static float PlacingTime = 1; //Время заполнения спрайта
    static float SpriteWidth = 0.5f; //Стандартная ширина спрайта
    static float MovingStep = 0.015f; //Длина шага при перемещении спрайта
    static RectTransform GraphicsTransform; //Родительский объект для элементов графики
    static Dictionary<Position, Vector2> XHighlightVectors; //Векторы выделения по х
    static Vector2 YHighlightVector; //Вектор выделения по y
    static int ScalingStepsNum = 20; //Количество шагов при выделении

    public Image BodySprite; //Тело персонажа
    public Image ClothesSprite; //Одежда персонажа
    Dictionary<string, GameObject> AttributeSprite; //Список атрибутов персонажа
    Image ParentImage; //Родительский объект персонажа

    public string Name = ""; //Имя персонажа
    string CurrentEmotion = ""; //Текущая эмоция персонажа
    string CurrentClothes = ""; //Текущая одежда персонажа
    List<string> CurrentAttributes; //Текущие атрибуты персонажа
    Position SpritePosition = Position.Center; //Текущая позиция персонажа
    bool Highlighted = false; //Выделен ли персонаж

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
        State.CurrentState.AddCharacter(name, emotion, clothes, CurrentAttributes, position, Highlighted); //Обновляем состояние
        Name = name; //Записываем имя
        SpritePosition = position; //Записываем позицию
        CurrentClothes = clothes; //Записываем одежду
        CurrentEmotion = emotion; //Записываем эмоцию
        SetEmotion(); //Ставим эмоцию
        SetClothes(); //Ставим одежду
        CurrentPosition = GetPosition(position, true); //Вычисляем позицию спрайта
        ApplyPosition(); //Применяем позицию спрайта
        StartCoroutine(placing(true)); //Начинаем корутину помещения
    }

    public void SetOnScene(string name, Position from, Position to, string emotion, string clothes) //Функция появления персонажа с движением
    {
        Init(); //Производим инициализацию
        if (ParentImage == null)//Если компонент Image ещё не найден
            ParentImage = GetComponent<Image>(); //То находим его
        State.CurrentState.AddCharacter(name, emotion, clothes, CurrentAttributes, to, Highlighted); //Обновляем состояние
        Name = name; //Записываем имя
        SpritePosition = to; //Записываем позицию
        CurrentClothes = clothes; //Записываем одежду
        CurrentEmotion = emotion; //Записываем эмоцию
        SetEmotion(); //Ставим эмоцию
        SetClothes(); //Ставим одежду
        CurrentPosition = GetPosition(from, false); //Вычисляем позицию спрайта
        ParentImage.fillAmount = 1; //Применяем позицию спрайта
        StartCoroutine(moving(GetPosition(to, true), false)); //Начинаем корутину перемещения
    }

    public void DeleteFromScene() //Удаление персонажа
    {
        State.CurrentState.DeleteCharacter(Name); //Обновляем состояние
        StartCoroutine(placing(false)); //Начинаем корутину скрытия
    }

    public void DeleteFromScene(Position to) //Удаление персонажа
    {
        State.CurrentState.DeleteCharacter(Name); //Обновляем состояние
        StartCoroutine(moving(GetPosition(to, false), true)); //Начинаем корутину перемещения
    }

    public void ChangeEmotion(string emotion) //Смена эмоции персонажа
    {
        CurrentEmotion = emotion; //Эмоция
        State.CurrentState.UpdateCharacterEmotion(Name, CurrentEmotion); //Обновляем состояние
        SetEmotion(); //Загружаем тело
        SetClothes(); //Загружаем одежду
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
        CurrentAttributes.Add(attribute); //Записываем атрибут в список атрибутов
        State.CurrentState.UpdateCharacterAttributes(Name, CurrentAttributes); //Обновляем состояние

        GameObject attr = Instantiate(ClothesSprite.gameObject); //Создаём объект
        attr.transform.SetParent(ParentImage.transform, false); //Помещяем его в родительский
        attr.transform.SetAsLastSibling(); //Выводим его на передний план
        Texture2D textattr = Resources.Load<Texture2D>(SpritesPath + Name + AttributesPath + attribute); //Загружаем текстуру атрибута
        attr.GetComponent<Image>().sprite = Sprite.Create(textattr, new Rect(0, 0, textattr.width, textattr.height), new Vector2(0, 0)); //Вставляем спрайт
        AttributeSprite.Add(attribute, attr); //Добавляем объект в словарь атрибутов
    }

    public void RemoveAttribute(string attribute) //Удаление атрибута
    {
        CurrentAttributes.Remove(attribute); //Удаляем атрибут из списка атрибутов
        State.CurrentState.UpdateCharacterAttributes(Name, CurrentAttributes); //Обновляем состояние

        GameObject attr = AttributeSprite[attribute]; //Получаем объект с атрибутом
        AttributeSprite.Remove(attribute); //Удаляем объект из словаря атрибутов
        Destroy(attr); //Удаляем объект
        Resources.UnloadUnusedAssets(); //Выгружаем неиспользуемые ресурсы
    }

    public void MoveActor(Position to) //Перемещение персонажа
    {
        SpritePosition = to; //Позиция персонажа
        State.CurrentState.UpdateCharacterPosition(Name, to); //Обновляем состояние
        StartCoroutine(moving(GetPosition(SpritePosition, true), false)); //Начинаем перемещение персонажа
    }

    IEnumerator placing(bool inc) //Корутина помещения/скрытия
    {
        ParentImage.fillOrigin = (FillOrigin == inc.GetHashCode()).GetHashCode(); //Определяем метод заполнения
        while (((ParentImage.fillAmount < 1) && (inc)) || ((ParentImage.fillAmount > 0) && (!inc))) //Пока полностью не покажем/скроем
        {
            if ((Skip.isSkipping)) //Если пропуск
            {
                yield return new WaitForSeconds(Settings.SkipInterval); //Новый кадр
                ParentImage.fillAmount = inc.GetHashCode(); //Заканчиваем действие
                break; //Прерываем цикл
            }
            ParentImage.fillAmount += (2 * inc.GetHashCode() - 1) * Time.deltaTime / PlacingTime; //Добавляем/убавляем заполнение
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
        for (int i = 0; i < ScalingStepsNum; i++) //Выполняем определённое количество шагов
        {
            if (Skip.isSkipping) //Если пропуск
            {
                yield return new WaitForSeconds(Settings.SkipInterval); //Новый кадр
                break; //Прерываение цикла
            }
            GraphicsTransform.anchorMin += (targetMin - beginMin) / ScalingStepsNum; //изменяеи anchorMin
            GraphicsTransform.anchorMax += (targetMax - beginMax) / ScalingStepsNum; //Изменяем anchorMax
            yield return null; //Новый кадр
        }
        GraphicsTransform.anchorMin = targetMin; //Ставим конечный anchorMin
        GraphicsTransform.anchorMax = targetMax; //Ставим конечный anchorMax
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

    void SetEmotion() //Установка эмоции
    {
        Texture2D body = Resources.Load<Texture2D>(SpritesPath + Name + BodiesPath + CurrentEmotion);//Загружаем тело
        BodySprite.sprite = Sprite.Create(body, new Rect(0, 0, body.width, body.height), new Vector2(0, 0)); //Применяем тело
    }

    void SetClothes() //Установка одежды
    {
        Texture2D sclothes = Resources.Load<Texture2D>(SpritesPath + Name + ClothesPath + CurrentClothes + "/" + CurrentEmotion[0]); //Загружаем одежду
        ClothesSprite.sprite = Sprite.Create(sclothes, new Rect(0, 0, sclothes.width, sclothes.height), new Vector2(0, 0)); //Применяем одежду
    }

    void Init() //Инициализация параметров
    {
        AttributeSprite = new Dictionary<string, GameObject>(); //Инициализация списка атрибутов
        CurrentAttributes = new List<string>(); //Инициализация списка названий атрибутов
        ParentImage = GetComponent<Image>(); //Поиск компонента Image
        GraphicsTransform = GameObject.Find("Graphics").GetComponent<RectTransform>(); //Находим компонент на сцене
        XHighlightVectors = new Dictionary<Position, Vector2>(); //Инициализируем словарь
        XHighlightVectors.Add(Position.Right, new Vector2(-1, 1)); //Добавляем вектор для правой части
        XHighlightVectors.Add(Position.Center, new Vector2(-0.5f, 1.5f)); //Добавляем вектор для центральной части
        XHighlightVectors.Add(Position.Left, new Vector2(0, 2)); //Добавляем вектор для левой части
        YHighlightVector = new Vector2(-0.5f, 1.5f); //Y вектор выделения
    }
}
