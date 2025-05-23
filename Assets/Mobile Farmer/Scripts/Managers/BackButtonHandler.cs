using UnityEngine;

public class BackButtonHandler : MonoBehaviour
{
    [SerializeField] GameObject exitMenu; // Assign your exit confirmation UI in inspector

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (exitMenu.activeSelf)
            {
                // If already showing, maybe quit or hide
                Application.Quit(); // Or hide the menu if needed
            }
            else
            {
                // Show exit UI
                exitMenu.SetActive(true);
            }
        }
    }

    public void QuiteGame()
    {
        Application.Quit();
    }
}
