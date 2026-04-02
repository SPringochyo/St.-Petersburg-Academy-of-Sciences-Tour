using UnityEngine;

public class Exhibit : MonoBehaviour 
{
    [Header("Данные экспоната")]
    public string exhibitName; // Название
    [TextArea(5, 10)]
    public string description; // Описание
}