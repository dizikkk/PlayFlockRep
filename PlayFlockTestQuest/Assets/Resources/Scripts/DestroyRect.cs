using UnityEngine;
using System.Collections;
using System;
using UnityEngine.EventSystems;

public class DestroyRect : MonoBehaviour
{
    private float doubleClickTimeLimit = 0.25f; //Промежуток времени между кликами

    private bool isDestroy;

    private Ray ray; //луч
    private RaycastHit rayHit; //столкновение луча

    private void OnMouseDown()
    {
        if (isDestroy) //если удалить прямоуг. разрешено
        {
            CreateRect._createRectInst.RectArray.Remove(gameObject); //удаляем этот объект из массива созданных объектов
            Destroy(gameObject); //удаляем этот объект 
        }
        else StartCoroutine(ClickEvent()); //запускаем корутину ClickEvent
    }

    /// <summary>
    /// Корутина, которая считывает сделан ли второй клик в промежутке 0.25f (float doubleClickTimeLimit), 
    /// </summary>
    private IEnumerator ClickEvent()
    {
        yield return new WaitForEndOfFrame(); //начало действия корутины спустя один фрейм

        float count = 0f; //значение времени от клика
        while (count < doubleClickTimeLimit) //пока значение времени от клика меньше, чем промежуток времени между кликами
        {
            isDestroy = true; //разрешить удаление
            count += Time.deltaTime; //увеличение времени от клика каждый фрейм
            yield return null;
        }
        isDestroy = false; //запретить удаление
    }
}