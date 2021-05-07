// To use this example, attach this script to an empty GameObject.
// Create three buttons (Create>UI>Button). Next, select your
// empty GameObject in the Hierarchy and click and drag each of your
// Buttons from the Hierarchy to the Your First Button, Your Second Button
// and Your Third Button fields in the Inspector.
// Click each Button in Play Mode to output their message to the console.
// Note that click means press down and then release.

using UnityEngine;
using UnityEngine.UI;

public class BtnManager : MonoBehaviour
{
    //Make sure to attach these Buttons in the Inspector
    public Button m_YourFirstButton, m_YourSecondButton, m_YourThirdButton, m_YourFourthButton,m_YourFifthButton,
	       m_but6, m_but7, m_but8;
	// Tureti du atskirus skriptus osciliatoriam gal but neoptimalu
	public Oscillator_n osc;
	public Oscillator_n2 osc2;
	public Oscillator_n3 osc3;
	
	bool osc_t, con_t, fxman1_t, fxman2_t, randm_t;

    void Start()
    {
        //Calls the TaskOnClick/TaskWithParameters/ButtonClicked method when you click the Button
        m_YourFirstButton.onClick.AddListener(delegate {TaskOnClick(1);});
        m_YourSecondButton.onClick.AddListener(delegate {TaskOnClick(2);});
        m_YourThirdButton.onClick.AddListener(delegate {TaskOnClick(3);});
        m_YourFourthButton.onClick.AddListener(delegate {ActivateOscs();});
        m_YourFifthButton.onClick.AddListener(delegate {ActivateCont();});
        m_but6.onClick.AddListener(delegate {ActivateFXmani1();});
        m_but7.onClick.AddListener(delegate {ActivateFXmani2();});
        m_but8.onClick.AddListener(delegate {ActivateRandMelody();});
		
		osc_t = false;
		con_t = false;
		fxman1_t = false;
		fxman2_t = false;
		randm_t = false;
    }
	
	private void TaskOnClick(int arg){
		osc.changeMode(arg);
		osc2.changeMode(arg);
		osc3.changeMode(arg);
		Debug.Log("Changing mode to " + arg);
	}
	
	private void ActivateOscs(){
		
		if(!osc_t){
			osc.enabled = true;
			osc2.enabled = true;
			osc3.enabled = true;
			osc_t = true;
		} else {
			osc.enabled = false;
			osc2.enabled = false;
			osc3.enabled = false;
			osc_t = false;
		}
	}
	
	private void ActivateCont(){
		if(!con_t){
			osc.continuous = true;
			osc2.continuous = true;
			osc3.continuous = true;
			con_t = true;
		} else {
			osc.continuous = false;
			osc2.continuous = false;
			osc3.continuous = false;
			con_t = false;
		}
	}
	
	private void ActivateFXmani1(){
		if(!fxman1_t){
			osc.manifx1 = true;
			fxman1_t = true;
		} else {
			osc.manifx1 = false;
			fxman1_t = false;
		}
	}
	
	private void ActivateFXmani2(){
		if(!fxman2_t){
			osc.manifx2 = true;
			fxman2_t = true;
		} else {
			osc.manifx2 = false;
			fxman2_t = false;
		}
	}
	
	private void ActivateRandMelody(){
		if(!randm_t){
			osc2.randMelody = true;
			Debug.Log("osc2 randmelody bool: " + osc2.randMelody );
			randm_t = true;
		} else {
			osc2.randMelody = false;
			Debug.Log("osc2 randmelody bool: " + osc2.randMelody );
			randm_t = false;
		}
	}
	
	

   
}