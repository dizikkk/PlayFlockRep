using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

public class CreateRect : MonoBehaviour
{
    /// <summary>
    /// Создание прямоугольника на игровом поле. Проверка на размещение прямоугольника в границах игрового поля с учетом нахождения в пределах размещения других прямоугольников,
    /// а также отступов от границ игрового поля
    /// </summary>

    public static CreateRect _createRectInst; //статичная переменная для синглтона

    [SerializeField] private GameObject prefabRect; //префаб прямоугольника, который будем создавать
    [SerializeField] private List<GameObject> rectArray; //массив созданных прямоугольников
    [SerializeField] private List<GameObject> bordersX; //массив границ расположенных по оси X
    [SerializeField] private List<GameObject> bordersY; //массив границ расположенных по оси Y

    private bool isCreateRect = true; //переменная для проверки можно ли создавать прямоугольник

    private Vector2 curMousePos; //текущие координаты мыши

    public List<GameObject> RectArray { get => rectArray; set => rectArray = value; }
    public bool IsCreateRect { get => isCreateRect; set => isCreateRect = value; }

    private Ray ray; //луч
    private RaycastHit rayHit; //столкновение луча

    private void Awake()
    {
        _createRectInst = this; // инициализация синглтона
    }

    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            isCreateRect = true;

            ray = Camera.main.ScreenPointToRay(Input.mousePosition); //точка, в которую мы пустили луч

            curMousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition); //текущие координаты мыши

            if (rectArray.Count > 0) //если массив созданных прямоугольников не пустой (на поле уже были созданы прямоугольники)
            {
                foreach (var _rect in rectArray.ToList()) //перебор каждого элемента массива
                {
                    Vector2 _rectPos = new Vector2(_rect.transform.position.x, _rect.transform.position.y); // координаты данного прямоугольника 

                    if (Mathf.Abs(curMousePos.x - _rectPos.x) < 2.1f && Mathf.Abs(curMousePos.y - _rectPos.y) < 1.1f) //если дистанция между данным прямоуг. меньше 2.1f по X и 1.1f по Y от места, в котором мы хотим создать прямоуг.
                        isCreateRect = false; //то нам нельзя создать прямоуг.
                }
            }

            CheckBorder(); //проверка на границы игрового поля

            if (Physics.Raycast(ray, out rayHit)) //если луч попал в объект
                if (rayHit.transform.name == "GameField" && !EventSystem.current.IsPointerOverGameObject()) //Если этот объект игровое поле и луч не попал в UI 
                    if (IsCreateRect) //если нам можно создать прямоуг.
                        CreateRectangle(); //создаем прямоуг.
        }
    }

    /// <summary>
    /// Создание прямоугольника
    /// </summary>
    private void CreateRectangle()
    {
        GameObject rect = Instantiate(prefabRect, new Vector2(curMousePos.x, curMousePos.y), Quaternion.identity); //создание прямоугольника в месте клика мышкой
        rectArray.Add(rect); //добавляем созданный прямоугольник в массив
    }

    /// <summary>
    /// Проверка на границы
    /// </summary>
    private void CheckBorder()
    {
        foreach (var _border in bordersX) //перебор каждого элемента границы по оси X
        {
            Vector2 _borderPos = _border.transform.position; //координаты границы
            if (Mathf.Abs(curMousePos.x - _borderPos.x) < 1.7f) //если дистанция между границой и точкой, в которой мы хотим создать прямоуг. меньше 1.7f по X
                isCreateRect = false; //то нам нельзя создать прямоуг.
        }

        foreach (var _border in bordersY) //перебор каждого элемента границы по оси Y
        {
            Vector2 _borderPos = _border.transform.position; //координаты границы
            if (Mathf.Abs(curMousePos.y - _borderPos.y) < 1.2f) //если дистанция между границой и точкой, в которой мы хотим создать прямоуг. меньше 1.2f по Y
                isCreateRect = false; //то нам нельзя создать прямоуг.
        }
    }
}