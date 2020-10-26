using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class SetColor : MonoBehaviour
{
    private Color newColor; //переменная для хранения цвета

    // Start is called before the first frame update
    void Start()
    {
        newColor = new Color(Random.value, Random.value, Random.value, 1f); //присваем переменной, хранящей в себе цвет рандомное значение
        transform.GetComponent<Renderer>().material.SetColor("_Color", newColor); //окрашиваем в этот цвет прямоугольник
    }

    // Update is called once per frame
    void Update()
    {
       
    }
}
