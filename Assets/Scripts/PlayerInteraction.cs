using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    public float distance = 10f; 
    public UIManager ui;         
    public LayerMask interactionLayer; // Обязательно выбери слой экспонатов в инспекторе!

    void Update()
    {
        if (Time.timeScale == 0f) return;

        if (Input.GetMouseButtonDown(0)) 
        {
            // 1. Создаем луч из позиции мышки на экране
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            // 2. Пускаем луч (с использованием маски слоев, чтобы не кликать на пол)
            if (Physics.Raycast(ray, out hit, distance, interactionLayer))
            {
                // 3. Ищем скрипт Exhibit
                Exhibit ex = hit.collider.GetComponentInParent<Exhibit>();
                
                if (ex != null)
                {
                    ui.Open(ex.exhibitName, ex.description);
                }
            }
        }
    }
}