using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

[RequireComponent(typeof(CanvasGroup))]
public class ModalWindowManager : MonoBehaviour
{
    [Header("Настройки анимации")]
    public float duration = 0.5f;
    public AnimationCurve curve = AnimationCurve.EaseInOut(0, 0, 1, 1);
    public Vector2 startOffset = new Vector2(0, -100);

    [Header("Ссылки для блокировки")]
    public MonoBehaviour cameraScript; // Перетащите сюда скрипт управления камерой

    private CanvasGroup canvasGroup;
    private RectTransform rectTransform;
    private Vector2 originalPosition;
    private Coroutine currentCoroutine;

    // Статическая переменная, чтобы другие скрипты знали о состоянии окна
    public static bool IsWindowOpen = false;

    void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        rectTransform = GetComponent<RectTransform>();
        originalPosition = rectTransform.anchoredPosition;

        // Инициализация (окно скрыто)
        canvasGroup.alpha = 0;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
        IsWindowOpen = false;
    }

    public void Show()
    {
        IsWindowOpen = true;
        if (cameraScript != null) cameraScript.enabled = false;
        
        // Показываем курсор
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        Transition(1f, true);
    }

    public void Hide()
    {
        IsWindowOpen = false;
        if (cameraScript != null) cameraScript.enabled = true;

        // Если в игре курсор должен быть скрыт (от 1-го лица), раскомментируйте:
        // Cursor.visible = false;
        // Cursor.lockState = CursorLockMode.Locked;

        Transition(0f, false);
    }

    private void Transition(float targetAlpha, bool isVisible)
    {
        if (currentCoroutine != null) StopCoroutine(currentCoroutine);
        currentCoroutine = StartCoroutine(AnimateWindow(targetAlpha, isVisible));
    }

    private IEnumerator AnimateWindow(float targetAlpha, bool isVisible)
    {
        float elapsed = 0;
        float startAlpha = canvasGroup.alpha;
        
        Vector2 startPos = isVisible ? originalPosition + startOffset : rectTransform.anchoredPosition;
        Vector2 endPos = isVisible ? originalPosition : originalPosition + startOffset;

        // Блокируем клики сразу при открытии, снимаем только после закрытия
        canvasGroup.blocksRaycasts = true; 

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;
            float curveValue = curve.Evaluate(t);

            canvasGroup.alpha = Mathf.Lerp(startAlpha, targetAlpha, curveValue);
            rectTransform.anchoredPosition = Vector2.Lerp(startPos, endPos, curveValue);
            yield return null;
        }

        canvasGroup.alpha = targetAlpha;
        rectTransform.anchoredPosition = isVisible ? originalPosition : endPos;
        
        // Интерактиваность (кнопки) включается только когда анимация закончена
        canvasGroup.interactable = isVisible;
        canvasGroup.blocksRaycasts = isVisible;
    }
}