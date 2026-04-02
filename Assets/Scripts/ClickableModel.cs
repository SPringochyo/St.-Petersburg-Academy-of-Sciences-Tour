using UnityEngine;
using UnityEngine.EventSystems;

public class ClickableModel : MonoBehaviour
{
    public ModalWindowManager windowManager;

    private void OnMouseDown()
    {
        // Проверка 1: Не открыто ли окно уже?
        // Проверка 2: Не нажал ли пользователь на UI (например, кнопку поверх модели)?
        if (!ModalWindowManager.IsWindowOpen && !EventSystem.current.IsPointerOverGameObject())
        {
            if (windowManager != null)
            {
                windowManager.Show();
            }
        }
    }
}