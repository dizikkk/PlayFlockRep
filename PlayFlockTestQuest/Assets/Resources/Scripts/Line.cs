using UnityEngine;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine.EventSystems;

public class Line : MonoBehaviour
{
    private GameObject _fistRect; //первый прямоуг. в связи
    private GameObject _secondRect; //второй прямоуг. в связи

    private bool isConnectLineSet; //перменная, которая сообщаем была ли связь установлена

    [SerializeField] private LineRenderer lineRenderer; //компонент LineRenderer

    private List<Vector2> points = new List<Vector2>(); //массив созданных точек линии

    private int pointsCount = 0; //количество созданных точек

    public int PointsCount { get => pointsCount; set => pointsCount = value; }
    public List<Vector2> Points { get => points; set => points = value; }
    public GameObject FistRect { get => _fistRect; set => _fistRect = value; }
    public GameObject SecondRect { get => _secondRect; set => _secondRect = value; }
    public bool IsConnectLineSet { get => isConnectLineSet; set => isConnectLineSet = value; }

    private void Update()
    {
        if (isConnectLineSet) //если связь установлена
        {
            if (_fistRect == null || _secondRect == null) //если первый или второй прямоуг. в связи был удален
                Destroy(gameObject); //удаляем линию связи

            ChangeConnectLinePos(_fistRect.transform.position, _secondRect.transform.position); //изменяем месторасположение точек линии в зависимости от месторасположения прямоугольников
        }

    }

    /// <summary>
    /// Добавляем точки в массив 
    /// </summary>
    public void AddPoint(Vector2 newPoint)
    {
        points.Add(newPoint); //добавляем точку в массив, указываем ее месторасположение
        pointsCount++; //прибавляем 1 к общему количеству точек

        lineRenderer.positionCount = pointsCount; //указываем сколько точек создать в LineRenderer
        lineRenderer.SetPosition(pointsCount - 1, newPoint); //указываем месторасположение точки в LineRenderer
    }

    /// <summary>
    /// Следование точки за мышкой
    /// </summary>
    public void FollowToMouse(Vector3 curMousePos)
    {
        if (pointsCount == 2) //если созданы 2 точки
            lineRenderer.SetPosition(1, new Vector3(curMousePos.x, curMousePos.y, 1f)); //вторая точка следует за указателем мыши
    }

    /// <summary>
    /// Указываем что связь была установлена
    /// </summary>
    public void SetConnectLine(Vector3 rectPos)
    {
        lineRenderer.SetPosition(1, new Vector3(rectPos.x, rectPos.y, 1f)); //указываем месторасположение второй точки в координатах второго прямоугольника
    }

    /// <summary>
    /// Следование точек за прямоугольниками
    /// </summary>
    public void ChangeConnectLinePos(Vector2 firstRect, Vector2 secondRect)
    {
        lineRenderer.SetPosition(0, new Vector3(firstRect.x, firstRect.y, 1f)); //первая точка двигается вместе с первым прямоугольником
        lineRenderer.SetPosition(1, new Vector3(secondRect.x, secondRect.y, 1f)); //вторая точка двигается вместе со вторым прямоугольником
    }
}
