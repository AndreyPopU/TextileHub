using UnityEngine;

public class buttonSelect : MonoBehaviour
{
    
    public GameObject tab;
    //private Button[] button = new Button;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Tab_1 (bool toggle_value){
        if (toggle_value)
            {
                print("DO THIS");
            }
        else{
            print("do something else");
        }
    }
}
