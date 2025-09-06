using UnityEngine;
using UnityEngine.UI; // Required for using UI components like Image

public class ScreenHandler : MonoBehaviour
{
    // A public variable to hold the sprite you want to display.
    // Drag your sprite asset from the Project window into this field in the Inspector.
    public Sprite imageSprite;

    // A public variable to hold the Canvas.
    // Drag your Canvas object from the Hierarchy into this field.
    public Canvas canvas;

    void Start()
    {
        // Check if the required components are assigned
        if (imageSprite == null)
        {
            Debug.LogError("Image Sprite is not assigned!");
            return;
        }

        if (canvas == null)
        {
            Debug.LogError("Canvas is not assigned!");
            return;
        }

        // 1. Create a new GameObject to hold the Image component.
        GameObject newImage = new GameObject("MyDynamicImage");

        // 2. Add the Image component to the new GameObject.
        Image img = newImage.AddComponent<Image>();

        // 3. Assign the prepared sprite to the Image component.
        img.sprite = imageSprite;

        // 4. Position and size the image.
        // It's a good practice to set a RectTransform.
        RectTransform rt = newImage.GetComponent<RectTransform>();
        rt.sizeDelta = new Vector2(200, 200); // Set the width and height
        rt.anchoredPosition = new Vector2(0, 0); // Position at the center

        // 5. Parent the new image to the Canvas.
        newImage.transform.SetParent(canvas.transform, false);
        // The 'false' parameter prevents the world-space position from being reset.
    }
}