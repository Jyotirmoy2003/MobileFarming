using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MissionWaypoint : MonoBehaviour
{
    // Indicator icon
    public Image img;
    // The target (location, enemy, etc..)
    public Transform target;
   
    // To adjust the position of the icon
    public Vector3 offset;
    private Action updateDel;

    private void Update()=>updateDel?.Invoke();

    void UpdatePositiond()
    {
        // Giving limits to the icon so it sticks on the screen
        // Below calculations witht the assumption that the icon anchor point is in the middle
        // Minimum X position: half of the icon width
        float minX = img.GetPixelAdjustedRect().width / 2;
        // Maximum X position: screen width - half of the icon width
        float maxX = Screen.width - minX;

        // Minimum Y position: half of the height
        float minY = img.GetPixelAdjustedRect().height / 2;
        // Maximum Y position: screen height - half of the icon height
        float maxY = Screen.height - minY;

        // Temporary variable to store the converted position from 3D world point to 2D screen point
        Vector2 pos = Camera.main.WorldToScreenPoint(target.position + offset);

        // Check if the target is behind us, to only show the icon once the target is in front
        if(Vector3.Dot((target.position - transform.position), transform.forward) < 0)
        {
            // Check if the target is on the left side of the screen
            if(pos.x < Screen.width / 2)
            {
                // Place it on the right (Since it's behind the player, it's the opposite)
                pos.x = maxX;
            }
            else
            {
                // Place it on the left side
                pos.x = minX;
            }
        }

        // Limit the X and Y positions
        pos.x = Mathf.Clamp(pos.x, minX, maxX);
        pos.y = Mathf.Clamp(pos.y, minY, maxY);

        // Update the marker's position
        img.transform.position = pos;
        // Change the meter text to the distance with the meter unit 'm'
        //meter.text = ((int)Vector3.Distance(target.position, transform.position)).ToString() + "m";
    }

    void UpdatePosition()
    {
        // Get the icon's size for boundary constraints
        float minX = img.GetPixelAdjustedRect().width / 2;
        float maxX = Screen.width - minX;

        float minY = img.GetPixelAdjustedRect().height / 2;
        float maxY = Screen.height - minY;

        // Convert target's world position to screen position
        Vector2 screenPos = Camera.main.WorldToScreenPoint(target.position + offset);

        // Check if the target is off-screen
        bool isTargetOffScreen = screenPos.x < 0 || screenPos.x > Screen.width || screenPos.y < 0 || screenPos.y > Screen.height;

        if (isTargetOffScreen)
        {
            // Constrain the icon within screen boundaries
            screenPos.x = Mathf.Clamp(screenPos.x, minX, maxX);
            screenPos.y = Mathf.Clamp(screenPos.y, minY, maxY);

            // Calculate direction from the player to the target
            Vector3 toTarget = (target.position - transform.position).normalized;

            // Calculate the angle to the target and adjust rotation
            float angle = Mathf.Atan2(toTarget.y, toTarget.x) * Mathf.Rad2Deg;
            img.transform.rotation = Quaternion.Euler(0, 0, angle + 90); // Reversed adjustment for correct direction
        }
        else
        {
            // If the target is on-screen, set the icon position to the target's screen position
            img.transform.position = screenPos;
            img.transform.rotation = Quaternion.identity; // Reset rotation when the target is on-screen
        }

        // Update the icon's position on the screen
        img.transform.position = screenPos;
    }
    public void InitiateWaypoint(Transform target)
    {
        this.target = target;
        img.gameObject.SetActive(true);
        updateDel += UpdatePosition;
    }

    public void StopWaypoint()
    {
        updateDel -= UpdatePosition;
        img.gameObject.SetActive(false);
    }
}
