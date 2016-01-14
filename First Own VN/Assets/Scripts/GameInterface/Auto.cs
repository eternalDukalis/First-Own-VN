using UnityEngine;
using System.Collections;

public class Auto : MonoBehaviour {

    public float BaseTime; //Базовое время
    public float SymbolTime; //Время для одного символа
    float MainTime; //Текущее оставшееся время
    static public bool isAuto; //Включён ли автоматический режим
	void Start () 
    {
        isAuto = false; //По умолчанию отключён
	}
	
	void Update () 
    {
        if (Input.GetKeyDown(KeyCode.A)) //Если нажата кнопка
        {
            isAuto = !isAuto; //То переключаем режим
        }
	}

    public void RefreshTime(int symbols) //Функция обновления оставшегося времени
    {
        MainTime = BaseTime + SymbolTime * symbols; //Рассчитываем время по формуле
    }

    public bool Continue() //Функция, возращающая, можно ли переходить дальше
    {
        if (!isAuto) //Если автоматический режим выключен 
            return false; //То возвращаем false
        if (MainTime <= 0) //Если время вышло
        {
            return true; //То возвращаем true
        }
        MainTime -= Time.deltaTime; //Вычитаем из текущего времени прошедшее с прошлой итерации время
        return false; //Возвращаем false
    }

    public virtual void Switch() //Функция переключения режима
    {
        isAuto = !isAuto; //Переключаем режим
    }
}