using UnityEngine;
using UnityEngine.UI;


/// <summary>Updates the graphics of UI elements to smoothly handle VR.</summary>
public class UpdateUI : MonoBehaviour
{
    public Camera view;

    // rotate the buttons toward the camera as it moves
    private void Update()
    {
        Button[] buttons;

        buttons = this.GetComponentsInChildren<Button>();
        foreach (Button button in buttons)
            button.transform.rotation = this.view.transform.rotation;
    }
}
