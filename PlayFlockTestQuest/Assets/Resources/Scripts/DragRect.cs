using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragRect : MonoBehaviour
{
    private Renderer _rendRect; //компонент Renderer прямоугольника

    private bool isDrag; //переменная для проверки можно ли перемещать прямоугольник
    private bool isMouseBtnDown; //если мышь нажата

    private Coroutine _dragEventCor; //экземпляр корутины

    private Vector3 startPos; //координаты начального расположения прямоугольника
    private Vector3 curMousePos; //координаты текущего положения мыши

    private Ray ray; //луч
    private RaycastHit rayHit; //столкновение луча

    void Start()
    {
        startPos = gameObject.transform.position; //координаты начального расположения прямоугольника
        _rendRect = GetComponent<Renderer>(); 
    }

    void Update()
    {
        curMousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition); //текущие координаты указателя мыши

        if (isDrag) //если можно перемещать прямоуг.
        {
            _rendRect.material.color = new Color(_rendRect.material.color.r, _rendRect.material.color.g, _rendRect.material.color.b, .5f); //ставим 50% прозрачность
            transform.position = new Vector3(curMousePos.x, curMousePos.y, 0f); //перемещаем прямоуг. за указателем мыши
        }
        else 
            _rendRect.material.color = new Color(_rendRect.material.color.r, _rendRect.material.color.g, _rendRect.material.color.b, 1f); // иначе возвращаем прозрачность на 100%
    }

    /// <summary>
    /// Обработчик нажатия мыши на объект
    /// </summary>
    private void OnMouseDown()
    {
        if (!EventSystem.current.IsPointerOverGameObject())
        {
            isMouseBtnDown = true; //мышь нажата на этот прямоуг.
            _dragEventCor = StartCoroutine(DragEvent()); //присваиваем корутину 
        }
    }

    /// <summary>
    /// Обработчик, если мышь была отжата 
    /// </summary>
    private void OnMouseUp()
    {
        isMouseBtnDown = false; //мышь не нажата
        isDrag = false; //запрещаем перемещение
        if (_dragEventCor != null) //если корутина не пустая
            StopCoroutine(_dragEventCor); //останавливаем корутину
        startPos = gameObject.transform.position; //присвоить начальное расположение прямоуг. там, куда мы его переместили
    }

    /// <summary>
    /// Корутина, которая отсчивает время удержания мыши на прямоуг.
    /// </summary>
    /// <returns></returns>
    IEnumerator DragEvent()
    {
        yield return new WaitForSeconds(0.5f); //ждем 0.5 секунды
        LineDraw._lineDrawInst.EndDraw(); //уничтожаем линию связи
        isMouseBtnDown = false; //мышь не нажата
        isDrag = true; //разрешаем перемещение
    }

    /// <summary>
    /// если мышь вышла за границы прямоугольника
    /// </summary>
    private void OnMouseExit()
    {
        if (isMouseBtnDown) //если мышь нажата
        {
            isDrag = false; //запрещаем перемещение
            if (_dragEventCor != null) //если корутина не пустая
                StopCoroutine(_dragEventCor); //останавливаем корутину
            isMouseBtnDown = false; //мышь не нажата
        }
    }

    /// <summary>
    /// Обработчик события при столкновении с триггером
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.name == "Cube(Clone)" || other.transform.name == "Border") //если наш прямоуг. коснулся другого прямоуг. или границы игрвого поля
        {
            isDrag = false; //запрещаем дальнейшее перемещение
            isMouseBtnDown = false; //мышь не нажата
            gameObject.transform.position = startPos; //возвращаем прямоуг. на исходное положение
        }
    }
}