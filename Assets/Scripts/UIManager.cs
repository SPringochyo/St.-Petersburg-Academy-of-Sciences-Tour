using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    public GameObject infoPanel;
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI descText;

    void Start()
    {
        infoPanel.SetActive(false);
        ApplyCursorSettings();
    }

    // LateUpdate работает ПОСЛЕ скриптов движения игрока
    void LateUpdate()
    {
        // Если курсор вдруг пропал или заблокировался - возвращаем его силой
        if (!Cursor.visible || Cursor.lockState != CursorLockMode.None)
        {
            ApplyCursorSettings();
        }
    }

    public void Open(string name, string desc)
    {
        nameText.text = name;
        descText.text = desc;
        infoPanel.SetActive(true);
        Time.timeScale = 0f;
        ApplyCursorSettings();
    }

    public void Close()
    {
        infoPanel.SetActive(false);
        Time.timeScale = 1f;
        ApplyCursorSettings();
    }

    private void ApplyCursorSettings()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }
}