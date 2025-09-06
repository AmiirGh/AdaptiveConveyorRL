using Oculus.Interaction;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    private float timer = 0;
    private Vector3 instantiatePos = new Vector3(0.5f,10.0299997f,-1.0f);
    public GameObject cubePrefab;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= 2.0f)
        {
            timer = 0;
            //Instantiate(cubePrefab, instantiatePos, Quaternion.identity);
        }
        
    }
}
