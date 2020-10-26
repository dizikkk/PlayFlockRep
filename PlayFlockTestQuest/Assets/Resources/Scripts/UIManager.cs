using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager _UIManagerInst;

    [SerializeField] private GameObject panel; //Панель с подсказками

    public GameObject Panel { get => panel; set => panel = value; }

    private void Awake()
    {
        _UIManagerInst = this;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// Кнопка закрытия панели
    /// </summary>
    public void ClosePanelBtnClick()
    {
        panel.SetActive(false); //скрыть панель
    }

    /// <summary>
    /// Кнопка открытия панели
    /// </summary>
    public void OpenPanelBtnClick()
    {
        panel.SetActive(true); //открываем панель
    }

    public void CloseApp()
    {
        Application.Quit(); // выход из игры
    }
}
