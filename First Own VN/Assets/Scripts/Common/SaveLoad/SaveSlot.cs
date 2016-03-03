using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SaveSlot : MonoBehaviour {

    public Vector3 SaveSlotPosition; //Позиция слота
    public bool isLoading; //Загрузка ли
    public System.DateTime SaveTime; //Время сохранения
    public State SaveData; //Данные сохранения
    static public SaveSlot SelectedData; //Компонент выделенного слота
    public Image PreviewImage; //Превью сохранения
    public Image PlainScreenImage; //Одноцветный экран превью
    public Image[] Actors; //Спрайты персонажей превью
	void Start () 
    {
        GettingData(); //Получаем данные
	}
	
	void Update () 
    {
        if (SaveTime != Saves.GetTime(SaveSlotPosition)) //Если время сохранения изменилось
        {
            GettingData(); //получаем данные
        }
	}

    void GettingData() //Функция получения данных
    {
        SaveTime = Saves.GetTime(SaveSlotPosition); //Получаем время
        SaveData = Saves.GetData(SaveSlotPosition); //Получаем состояние
        if ((isLoading) && (SaveData == null)) //Если это экран загружки и состояние пустое
            GetComponent<Toggle>().interactable = false; //То с компонентом нельзя взаимодействовать
        else //Иначе
            GetComponent<Toggle>().interactable = true; //С компонентом можно взаимодействовать
        MakePreview(); //Делаем превью
    }

    public virtual void ChangingValue() //Функция изменения значения
    {
        SelectedData = this; //Перемещаем ссылку выделенного компонента на этот компонент
    }

    void MakePreview() //Функция установки превью
    {
        if (SaveData == null) //Если состояние пустое
        {
            PreviewImage.gameObject.SetActive(false); //То скрываем объект
            return; //Выход
        }
        PreviewImage.gameObject.SetActive(true); //Открываем объект
        Texture2D back = Resources.Load<Texture2D>(BackgroundManager.BackPath + SaveData.Background); //Загружаем фон
        PreviewImage.sprite = Sprite.Create(back, new Rect(0, 0, back.width, back.height), new Vector2(0, 0)); //Вставляем фон
        for (int i = 0; i < Actors.Length; i++) //Для всех позиций спрайтов персонажей
        {
            State.CharacterInfo cInfo = SaveData.Chars.Find(x => x.SpritePosition.GetHashCode() == i); //Находим персонажа, который находится на этой позиции
            if (cInfo == null) //Если такого персонажа нет
            {
                Actors[i].gameObject.SetActive(false); //То делаем спрайт персонажа неактивным
                continue; //Переходим на следующую итерацию
            }
            Actors[i].gameObject.SetActive(true); //Делаем спрайт персонажа активным
            Texture2D body = Resources.Load<Texture2D>(CharacterBehavior.SpritesPath + cInfo.Name + CharacterBehavior.BodiesPath + cInfo.CurrentEmotion); //Загружаем тело персонажа
            Actors[i].sprite = Sprite.Create(body, new Rect(0, 0, body.width, body.height), new Vector2(0, 0)); //Вставляем тело персонажа
            Image clothesImage = Actors[i].transform.GetChild(0).GetComponent<Image>(); //Находим объект одежды персонажа
            Texture2D clothes = Resources.Load<Texture2D>(CharacterBehavior.SpritesPath + cInfo.Name + CharacterBehavior.ClothesPath + cInfo.CurrentClothes + "/" + cInfo.CurrentEmotion[0]); //Загружаем одежду персонажа
            clothesImage.sprite = Sprite.Create(clothes, new Rect(0, 0, clothes.width, clothes.height), new Vector2(0, 0)); //Вставляем одежду персонажа
            int childCount = clothesImage.transform.childCount; //Определяем количество детей объект одежды персонажа
            for (int j = 0; j < childCount; j++) //Для всех детей объекта одежды персонажа
            {
                GameObject obj = clothesImage.transform.GetChild(0).gameObject; //Находим объект ребёнка
                obj.transform.SetParent(Actors[i].transform); //Переносим его на другой объект
                Destroy(obj); //Удаляем объект
            }
            foreach (string x in cInfo.CurrentAttributes) //Для каждого атрибута в списке атрибутов персонажа
            {
                GameObject obj = Instantiate(clothesImage.gameObject); //Создаём объект
                obj.transform.SetParent(clothesImage.transform, false); //В качестве родителя будет объект одежды персонажа
                obj.transform.SetAsLastSibling(); //Помещаем сверху
                Texture2D attr = Resources.Load<Texture2D>(CharacterBehavior.SpritesPath + cInfo.Name + CharacterBehavior.AttributesPath + x); //Загружаем атрибут
                obj.GetComponent<Image>().sprite = Sprite.Create(attr, new Rect(0, 0, attr.width, attr.height), new Vector2(0, 0)); //Вставляем атрибут
            }
        }
        if (SaveData.PlainScreenOn) //Если есть одноцветный экран
        {
            PlainScreenImage.gameObject.SetActive(true); //Делаем объект одноцветного экрана активным
            PlainScreenImage.color = EffectsManager.StringToColor(SaveData.PlainScreenColor); //Устанавливаем цвет
        }
        else //Иначе
            PlainScreenImage.gameObject.SetActive(false); //Делаем объект неактивным
    }
}
