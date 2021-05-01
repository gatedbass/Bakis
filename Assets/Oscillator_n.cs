using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;

/*
    TODO:
    Sukurti kintamaji, kuris saugotu, koks akcelerometro pokytis ivyko per laiko tarpa,
	Jeigu akcelerometru pokytis nebuvo pakankamas, nekeisti natos
*/

public class Oscillator_n : MonoBehaviour
{
   
   public AccLowPass accfilter;
   
   public Text x1;
   public Text x2;
   public Text t;
   public Text status;
   public Text freqmonitor;
   public double frequency = 440.0;
   
   private double increment;
   private double phase;
   private double sampling_frequency = 48000.0;
   private int n = 0;
   private bool once = false;
   private float endFreq;
   private int mode; // Waveform mode
   
   private double gx = 0;
   private double acc1;
   private double acc2;
   private double tx;
   private double dX;
   private bool deltaonce = false;
   
   public float gain;
   public float volume = 0.1f;
   public float noteCooldown = 1f;
   private float nextNoteTime = 0;
   
   
   
   public float[] frequencies;
   public List<float> nf;
   public int thisFreq;
   
   
   void Start()
   {
	     
	   
	   
	   // A Major scale
	   
	   frequencies = new float[8];
	   frequencies[0] = 440;
	   frequencies[1] = 494;
	   frequencies[2] = 554;
	   frequencies[3] = 587;
	   frequencies[4] = 659;
	   frequencies[5] = 740;
	   frequencies[6] = 831;
	   frequencies[7] = 880;
	   
	   
	   // Creating new list for lower octave notes
	   nf = new List<float>(8);
	   // Reducing scale by 2 octaves
	   foreach(float i in frequencies){
		   nf.Add(i / 4);
	   }
   }
   
   public void changeMode(int arg){
	   mode = arg;
   }
   
   public void changeNote(int dir){
			
		if(dir == 1){
			frequency = frequencies[thisFreq];
			thisFreq += 1;
			// padaro indekso reseta
			thisFreq = thisFreq % nf.Count;
			n++;
			//x1.text = n.ToString();
		} else if (dir == -1){
			if(thisFreq == 0){	
				frequency = frequencies[thisFreq];
				// padaro indekso reseta
				thisFreq = thisFreq % nf.Count;
				n++;
				//x1.text = n.ToString();;
			} else {
				thisFreq -= 1;
				frequency = frequencies[Mathf.Abs(thisFreq)];
				// padaro indekso reseta
				thisFreq = thisFreq % nf.Count;
				n++;
				//x1.text = n.ToString();
			}
		}
   }
   
   void Update(){
	   
	   // Input.acceleration.x but filtered and absolute value
	   endFreq = Mathf.Abs(accfilter.filterAccelValue(true)[0]) * 400;
	  
	  
	   // Veiksmas vyksta kas tris sekundes
	   /*
		  Dabar mano kodas nuskaito acc1, po dvieju sekundziu nuskaito acc2 ir skaiciuoja skirtuma tarp ju, tai nebutinai atspindi visa akcelerometro pokyti kuris ivyko per ta laiko tarpa, nes acc1 == acc2 imanoma
		  
		  sprendimas: dX += sumuoti rodiklio reiksme kiekviena kadra? (kaip nustatyti krypti, ar tai svarbu?)
		  
		  */
		  
		  /*
		   if(deltaonce == false){
			   acc1 = Input.acceleration.x;
			   x1.text = acc1.ToString();
			   tx = Time.time;
			   deltaonce = true;
			   Debug.Log("deltaonce =" + deltaonce);
			   status.text = "Measuring...";
		   }
		   
		   // Tikrinamas pokytis 
		   if(Time.time - tx >= 0.1)
		   {
			   acc2 = Input.acceleration.x;
			   x2.text = acc2.ToString();
			   dX = (acc2 - acc1) * 10000;
			   tx = Time.time;
			   t.text = dX.ToString();
			   deltaonce = false;
			   Debug.Log("deltaonce =" + deltaonce);
			   status.text = "Measured!";
			   gx = Time.time;
		   }
	   
	   
	   if(dX >= 100 && dX != 0 && Time.time == gx){
		   if(Input.acceleration.x > 0.5f)
		   {	
				gain = volume;
				if(once == false){
					changeNote(1);
					once = true;r interpolation
					nextNoteTime = Time.time + noteCooldown;
				}
		
			}
	   }
		
		   if(Input.acceleration.x < 0.5f && Input.acceleration.x > -0.5f)
		   {
			   once = false;
			   gain = 0;
		   }
	   
		   if(dX >= 100 && dX != 0 && Time.time == gx){
				if(Input.acceleration.x < -0.5f)
			   {
					gain = volume;
					if(once == false)
					{
						changeNote(-1);
						once = true;
						nextNoteTime = Time.time + noteCooldown;
					}	
				}
		   } */
	   }
   
	  void OnAudioFilterRead(float[] data, int channels)
	  {	
	   //Vietoj vaiksciojimo per masyva darysiu tolydu
	   //increment = frequency * 2.0 * Mathf.PI / sampling_frequency;
	   
	   
	   gain = volume;
	   
	   
	   increment = endFreq  * 2.0 * Mathf.PI / sampling_frequency;
	   freqmonitor.text = endFreq.ToString();
	   
	   for (int i = 0; i < data.Length; i += channels)
	   {
		   phase += increment;
		  
		   if(mode == 1){
			   // Sine wave
			   data[i] = (float)(gain * Mathf.Sin((float)phase)); 
			}
			
		   if(mode == 2){
			   // Sqaure wave
			   if(gain * Mathf.Sin((float)phase) >=  0 * gain){
				   data[i] = (float)gain * 0.6f;
			   } else {
				   data[i] = (-(float)gain) * 0.6f;
			   }
		   }
		   
		   if(mode == 3){
			   // Triangle wave
			   data[i] = (float) (gain * (double)Mathf.PingPong ((float) phase, 1.0f));
		   }
		   
		   if(channels == 2)
		   {
			   data[i + 1] = data[i];
		   }
		   
		   if (phase > (Mathf.PI * 2))
		   {
			  phase =- 2 * Mathf.PI;
		   }
	   }
	  } 
   
   
}
