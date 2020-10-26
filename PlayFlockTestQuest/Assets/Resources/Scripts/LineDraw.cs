using UnityEngine;
using UnityEngine.EventSystems;

public class LineDraw : MonoBehaviour
{
    public static LineDraw _lineDrawInst; //статичная переменная для синглтона

    [SerializeField] private GameObject linePrefab; //префаб для объекта с компонентом LineRenderer

    private Line currentLine; //текущий объект с LineRenderer

    private Vector3 curMousePos; //переменная для хранения текущей позиции мыши

    private Ray ray; //луч
    private RaycastHit rayHit; //столкновение луча

    private void Awake()
    {
        _lineDrawInst = this; //инициализация синглтона
    }

    void Update()
    {
        curMousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition); //текущая позиция мыши

        if (Input.GetMouseButtonDown(1)) //если была нажата правая кнопка мыши
        {
            ray = Camera.main.ScreenPointToRay(Input.mousePosition); //точка, в которую мы пустили луч
            if (Physics.Raycast(ray, out rayHit)) //если произошло столкновение луча с объектом
            {
                if (rayHit.transform.name == "Cube(Clone)" && currentLine == null && !EventSystem.current.IsPointerOverGameObject()) //если луч попал в прямоугольник и объект с LineRenderer не создан
                {
                    BeginDraw(); //начинаем рисовать линиию 
                }
                else if (rayHit.transform.name == "Cube(Clone)" && currentLine.PointsCount == 2 && !EventSystem.current.IsPointerOverGameObject()) //если луч попал в прямоугольник и LineRenderer имеет 2 созданные точки
                {
                    currentLine.SecondRect = rayHit.transform.gameObject; //указываем что второй прямоуг. для создания связи, является выбранный нами сейчас прямоуг.
                    SetConnectLine(); //устанавливаем связь между прямоугольниками
                }
                else
                    EndDraw(); //иначе вызываем функцию по удалению линни, т.к. связь не была установлена
            }
        }

        if (currentLine != null) //если объект с LineRenderer создан
            currentLine.FollowToMouse(curMousePos); //следование второй точки линии за мышкой
    }

    /// <summary>
    /// Начало рисования линии
    /// </summary>
    void BeginDraw()
    {
        currentLine = Instantiate(linePrefab, this.transform).GetComponent<Line>(); //создаем объект с компонентом LineRenderer
        currentLine.AddPoint(curMousePos); //создаем первую точку
        currentLine.AddPoint(curMousePos); //создаем вторую точку
        currentLine.FistRect = rayHit.transform.gameObject; //указываем что первый прямоуг. при создании связи является выбранный нами сейчас прямоуг.
    }

    /// <summary>
    /// Установка связи
    /// </summary>
    void SetConnectLine()
    {
        currentLine.IsConnectLineSet = true; //указываем что связь установлена
        currentLine.SetConnectLine(new Vector3(rayHit.transform.position.x, rayHit.transform.position.y, 1f)); //передаем в функцию объекта Line расположение прямоуг., с которым устанавливаем связь
        currentLine = null; //очищаем переменную, хранящую в себе объект с LineRenderer 
    }

    /// <summary>
    /// Удаляем линию, если связь не была ни с кем установлена
    /// </summary>
    public void EndDraw()
    {
        if (currentLine != null) //если линия для связи была создана
        {
            Destroy(currentLine.gameObject); //удаляем линию
        }
    }
}