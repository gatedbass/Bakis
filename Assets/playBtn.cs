using System.Collections;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using UnityEngine;

public class playBtn : MonoBehaviour
{
   public Oscillator m_Synth;
	
	// Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
       if(Input.GetMouseButtonDown(0))
	   {
		   Debug.Log("down");
		   m_Synth.down();
	   } else if (Input.GetMouseButtonUp(0))
	   {
		   Debug.Log("up");
		   m_Synth.up();
	   }		   
    }
	

   
}
