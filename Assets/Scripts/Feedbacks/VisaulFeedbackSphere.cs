using UnityEngine;

public class VisaulFeedbackSphere : MonoBehaviour
{

    private Renderer sphereRenderer;
    public VisualFeedback visualFeedback;
    public Feedbacks feedbacks;

    void Start()
    {
        sphereRenderer = GetComponent<Renderer>();
    }

    void Update()
    {
        sphereRenderer.material.color = Color.Lerp(Color.red, Color.white, feedbacks.feedbackIntensity);
    }
}
