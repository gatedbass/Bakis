using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class MixLevels : MonoBehaviour
{
	
	public AudioMixer masterMixer;
	
	public void SetChorusRate(float rate){
		masterMixer.SetFloat("ChorusRate", rate);
	}
	
	public void SetLowPassFrq(float fr){
		masterMixer.SetFloat("LowPassFrq", fr);
	}

}
