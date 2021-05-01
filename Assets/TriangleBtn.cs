using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriangleBtn : MonoBehaviour
{
    
	public Oscillator_n osc;
	
	// Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
	
	public void onClick(){
		osc.changeMode(3);
	}
}
