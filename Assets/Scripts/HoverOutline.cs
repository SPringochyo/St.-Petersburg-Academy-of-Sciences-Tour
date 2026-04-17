using UnityEngine;

public class HoverOutline : MonoBehaviour {
    [Header("Settings")]
    public LayerMask exhibitLayer; // Create a "Exhibit" layer and assign here
    public float raycastLength = 50f;
    
    private GameObject currentHovered;
    private Camera mainCam;

    void Start() {
        mainCam = Camera.main;
    }

    void Update() {
        Ray ray = mainCam.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, raycastLength, exhibitLayer)) {
            GameObject target = hit.transform.root.gameObject; // Assumes outline is child of root
            if (target != currentHovered) {
                SetOutline(currentHovered, false);
                SetOutline(target, true);
                currentHovered = target;
            }
        } else {
            if (currentHovered != null) {
                SetOutline(currentHovered, false);
                currentHovered = null;
            }
        }
    }

    void SetOutline(GameObject obj, bool active) {
        if (obj == null) return;
        Transform outline = obj.transform.Find("OutlineMesh");
        if (outline != null) outline.gameObject.SetActive(active);
    }
}