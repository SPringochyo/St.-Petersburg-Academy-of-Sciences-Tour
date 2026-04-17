using UnityEngine;
using Yarn.Unity;

public class TestDialogTrigger : MonoBehaviour
{
    public DialogueRunner dialogueRunner;
    public string dialogueNode = "SampleSceneScript";

    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Player")) {
            dialogueRunner.StartDialogue(dialogueNode);
            
            // Optional: disable player movement/camera while dialog plays
        }
    }
}
