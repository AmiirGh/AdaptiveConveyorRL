using UnityEngine;
using UnityEngine.UI;

public class VisualFeedback : MonoBehaviour
{
    [SerializeField]
    private TCP tcp;
    private int visualFbIndex = 0;
    private GameObject currentProduct;
    private float distanceToConveyorEnd = 0;
    private Vector3 conveyorEndPosition = new Vector3(0, 0, 0);
    private float maxDistanceToconveyorEnd = 0;
    
    public Image circleImage;
    public ProductSpawner productSpawner;
    private int prevSecondaryTaskTrialNumber = -1;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (tcp.fbModality != visualFbIndex) 
        { 
            circleImage.enabled = true; 
            return; 
        }

        UpdateCircleColor();
        
        
    }
    void UpdateCircleColor()
    {
        if(IsNewTrialStarted()) 
            currentProduct = GameObject.FindGameObjectWithTag("product");

        distanceToConveyorEnd = currentProduct.transform.position.z - conveyorEndPosition.z;
        //feedbackIntensity = Mathf.InverseLerp(0, maxDistanceToconveyorEnd, distanceToConveyorEnd);

    }
    private bool IsNewTrialStarted()
    {
        
        if (productSpawner.secondaryTaskTrialNumber > prevSecondaryTaskTrialNumber)
        {
            prevSecondaryTaskTrialNumber = productSpawner.secondaryTaskTrialNumber;
            return true;
        }
        
        return false;
    }
}
