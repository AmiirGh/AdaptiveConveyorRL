using UnityEngine;

public class ProductSpawner : MonoBehaviour
{
    private float timer = 5;
    private Vector3 instantiatePos = new Vector3(0.5f, 10.0f, -1.0f);
    public GameObject cubePrefab;
    public int secondaryTaskTrialNumber = 0;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= 5.0f)
        {
            timer = 0;
            Instantiate(cubePrefab, instantiatePos, Quaternion.identity);
            secondaryTaskTrialNumber++;
        }

    }
}
