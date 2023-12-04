using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

public class PointCreator : MonoBehaviour
{
    public GameObject point;

    

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            var a = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3 whereGenerate = new Vector3(a.x, a.y, 0);
            Instantiate(point , whereGenerate,transform.rotation );
        }
    }

    

}
