using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CharacterManager : MonoBehaviour {

    public GameObject CharacterObject; //Объект персонажа
    public GameObject ParentForActors; //Объект с персонажами
    List<GameObject> Actors; //Список с объектами персонажей на сцене
	void Start () 
    {
        Actors = new List<GameObject>(); //Инициализируем список
        State.CharacterInfo[] cinf = State.CurrentState.Chars.ToArray(); //Сохраняем данные
        State.CurrentState.Chars = new List<State.CharacterInfo>(); //Очищаем список
        foreach (State.CharacterInfo cur in cinf) //Для каждой записи информации о персонаже
        {
            SetActor(cur.Name, PositionToString(cur.SpritePosition), cur.CurrentEmotion); //Добавляем объект
            for (int i = 0; i < cur.CurrentAttributes.Count; i++)
                SetAttribute(cur.Name, cur.CurrentAttributes.ToArray()[i]); //Добавляем все атрибуты
            if (cur.Highlighted) //Если выделен
                Highlight(cur.Name); //То выделяем
        }
    }
	
	void Update () 
    {

    }

    public bool HasActor(string name)
    {
        if (Actors.Find(x => x.GetComponent<CharacterBehavior>().Name == name) == null)
            return false;
        return true;
    }

    public void SetActor(string name, string position, string emotion) //Функция появления персонажа без движения
    {
        if (!State.CurrentState.Clothes.ContainsKey(name)) //Если для персонажа нет одёжки
            Debug.LogError("No clothes for this character."); //То ошибка
        GameObject obj = Instantiate(CharacterObject); //Выводим объект на сцену
        obj.transform.SetParent(ParentForActors.transform, false); //Помещаем в родительский объект
        Actors.Add(obj); //Добавляем в список
        obj.GetComponent<CharacterBehavior>().SetOnScene(name, StringToPosition(position), emotion, State.CurrentState.Clothes[name]); //Запускаем функцию появления
    }

    public void SetActor(string name, string from, string to, string emotion) //Функция появления персонажа с движением
    {
        if (!State.CurrentState.Clothes.ContainsKey(name)) //Если для персонажа нет одёжки
            Debug.LogError("No clothes for this character."); //То ошибка
        GameObject obj = Instantiate(CharacterObject); //Выводим объект на сцену
        obj.transform.SetParent(ParentForActors.transform, false); //Помещаем в родительский объект
        Actors.Add(obj); //Добавляем в список
        obj.GetComponent<CharacterBehavior>().SetOnScene(name, StringToPosition(from), StringToPosition(to), emotion, State.CurrentState.Clothes[name]); //Запускаем функцию появления
    }

    public void DeleteActor(string name) //Функция удаление персонажа без движения
    {
        CharacterBehavior actor = Actors.Find(x => x.GetComponent<CharacterBehavior>().Name == name).GetComponent<CharacterBehavior>(); //Находим персонажа
        Actors.Remove(actor.gameObject); //Удаляем персонажа из списка
        actor.DeleteFromScene(); //Запускаем функцию удаления
    }

    public void DeleteActor(string name, string to) //Функция удаления персонажа с движением
    {
        CharacterBehavior actor = Actors.Find(x => x.GetComponent<CharacterBehavior>().Name == name).GetComponent<CharacterBehavior>(); //Находим персонажа
        Actors.Remove(actor.gameObject); //Удаляем персонажа из списка
        actor.DeleteFromScene(StringToPosition(to)); //Запускаем функцию удаления
    }

    public void ForceDeleteActor(string name)
    {
        CharacterBehavior actor = Actors.Find(x => x.GetComponent<CharacterBehavior>().Name == name).GetComponent<CharacterBehavior>(); //Находим персонажа
        Actors.Remove(actor.gameObject); //Удаляем персонажа из списка
        Destroy(actor.gameObject); //Удаляем объект со сцены
    }

    public void ChangeClothes(string name, string clothes) //Функция смены одежды
    {
        if (State.CurrentState.Clothes.ContainsKey(name)) //Если для персонажа уже есть одежда
        {
            State.CurrentState.Clothes.Remove(name); //То удаляем её
        }
        State.CurrentState.Clothes.Add(name, clothes); //Добавляем одежду для персонажа
    }

    public void Highlight(string name) //Функция выделения персонажа 
    {
        Actors.Find(x => x.GetComponent<CharacterBehavior>().Name == name).GetComponent<CharacterBehavior>().Highlight(); //Находим персонажа и запускаем выделение
    }

    public void Unhighlight(string name) //функция снятия выделения персонажа
    {
        Actors.Find(x => x.GetComponent<CharacterBehavior>().Name == name).GetComponent<CharacterBehavior>().Unhighlight(); //Находим персонажа и снимаем выделение
    }

    public void SetAttribute(string name, string attribute) //Функция установка атрибута
    {
        Actors.Find(x => x.GetComponent<CharacterBehavior>().Name == name).GetComponent<CharacterBehavior>().SetAttribute(attribute); //Устанавливаем атрибут
    }

    public void DeleteAttribute(string name, string attribute) //Функция удаления атрибута
    {
        Actors.Find(x => x.GetComponent<CharacterBehavior>().Name == name).GetComponent<CharacterBehavior>().RemoveAttribute(attribute); //Удаляем атрибут
    }

    public void MoveActor(string name, string position) //Функия перемещения персонажа
    {
        Actors.Find(x => x.GetComponent<CharacterBehavior>().Name == name).GetComponent<CharacterBehavior>().MoveActor(StringToPosition(position)); //Запускаем перемещение персонажа
    }

    public void ChangeEmotion(string name, string emotion) //Функция смены эмоции персонажа
    {
        Actors.Find(x => x.GetComponent<CharacterBehavior>().Name == name).GetComponent<CharacterBehavior>().ChangeEmotion(emotion); //Меняем эмоцию
    }

    CharacterBehavior.Position StringToPosition(string pos) //Функция перевода строки в перечислимый типа Position
    {
        switch (pos) //В зависимости от содержимого строки
        {
            case "left": //Если значение left
                return CharacterBehavior.Position.Left; //То возвращаем Left
            case "right": //Если значение right
                return CharacterBehavior.Position.Right; //То возвращаем Right
        }
        return CharacterBehavior.Position.Center; //Возвращаем Center
    }

    string PositionToString(CharacterBehavior.Position pos) //Функция перевода Position в строку
    {
        switch (pos)
        {
            case CharacterBehavior.Position.Right:
                return "right";
            case CharacterBehavior.Position.Left:
                return "left";
        }
        return "center";
    }
}
