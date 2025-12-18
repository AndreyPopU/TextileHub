using UnityEngine;

public class refImage : MonoBehaviour
{
    public GameObject [] references;
  
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        references [Random.Range(0, references.Length)].SetActive(true);    
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
