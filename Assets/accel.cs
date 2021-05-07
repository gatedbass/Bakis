using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class accel : MonoBehaviour
{
    public Oscillator m_Synth;
	public Text xt;
	public Text yt;
	public Text	zt;
	
	private Gyroscope m_Gyro;
	
	//private bool backswing = false;
	
	// Start is called before the first frame update
    void Start()
    {
        m_Gyro = Input.gyro;
		m_Gyro.enabled = true;
    }

    // Update is called once per frame
    void Update()
    {
		
		//xt.text = Input.gyro.rotationRate.x.ToString();
		//yt.text = Input.gyro.rotationRate.y.ToString();
	    //zt.text = Input.gyro.rotationRate.z.ToString();
		
        xt.text = Input.acceleration.x.ToString();
		yt.text = Input.acceleration.y.ToString();
		zt.text = Input.acceleration.z.ToString();
		
		/*
		if(Input.acceleration.x > 0.5){
			m_Synth.down();
		}*/
		
    }
}
