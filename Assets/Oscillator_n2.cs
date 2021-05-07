using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using Random = UnityEngine.Random;

/*
    TODO:
    Sukurti kintamaji, kuris saugotu, koks akcelerometro pokytis ivyko per laiko tarpa,
	Jeigu akcelerometru pokytis nebuvo pakankamas, nekeisti natos
*/

public class Oscillator_n2 : MonoBehaviour
{
   
   public AccLowPass accfilter;
   
   public Text x1;
   public Text x2;
   public Text t;
   public Text status;
   public Text freqmonitor;
   public double frequency;
   
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
   private double tRand;
   private bool deltaonce = false;
   
   public float gain;
   public float volume;
   public bool enabled;
   public bool continuous;
   public bool randMelody;
   private bool stopnegate;

   public float noteCooldown = 1f;
   private float nextNoteTime = 0;
   
   private int[] randDirSet;
   private int randDir;
   private int randNote;
   
   private double[] tones; // all 12 tones of a piano
   public double[] frequencies;
   public List<float> nf;
   public int thisFreq;
   
   void Start()
   {
	  
	//Unity kintamuju inicializavimas vyksta cia
	
		randDirSet = new int[2];
		randDirSet[0] = -1;
		randDirSet[1] = 1;	
		randDir = -1;
		tRand = 0;
		randMelody = false;
		
		
		frequencies = new double[8];
		mode = 1;
		continuous = false;
		stopnegate = false;
		thisFreq = 0;
		
		
		// sita gali tekti perrasyt kaip key:value pair dictionary
		tones = new double[12];
		tones[0] =	16.351f; // C		
		tones[1] =	17.324f; // C#		
		tones[2] =	18.354f; // D		
		tones[3] =	19.445f; // D#		
		tones[4] =	20.601f; // E		
		tones[5] =	21.827f; // F
		tones[6] =	23.124f; // F#		
		tones[7] =	24.449f; // G		
		tones[8] =	25.959f ;// G#		
		tones[9] = 27.5f;   // A	
		tones[10] =	29.135f; // A#
		tones[11] = 30.868f; // B
		
		changeKey(7,4,"major");
		frequency = frequencies[thisFreq];
	   
	   // Creating new list for lower octave notes
	   nf = new List<float>(8);
	   // Reducing scale by 2 octaves
	   foreach(float i in frequencies){
		   nf.Add(i / 4);
	   }
   }
   
   
   // Pakeicia grojimo tonacija
   // (gamos nata, oktava, mazorine/minorine scale)
   public void changeKey(int note, double oct, string scale){
	   
	   int index = 0, j = 0;
	   int[] steps = new int[7];
	   
	   oct = Math.Pow(2, oct);
	  
	   if(scale == "major"){
		  // mazorines dermes schema
		  steps[0] = 2;
		  steps[1] = 2;
		  steps[2] = 1;
		  steps[3] = 2;
		  steps[4] = 2;
		  steps[5] = 2;
		  steps[6] = 1;
 		  //{2,2,1,2,2,2};
	   } else if (scale == "minor") {
		   // minorines dermes schema
		  steps[0] = 2;
		  steps[1] = 1;
		  steps[2] = 2;
		  steps[3] = 2;
		  steps[4] = 1;
		  steps[5] = 2;
		  steps[6] = 2;
		  //{2,1,2,2,1,2};
	   }
	   
	   // 12 nes einama per dvylika chromatines gamos natu
	   for(int i = 0; i < 12; i++){
		   if(note == i){
			 // kai atrandama nata, kurios gama norima sudaryti, jinai nustatoma kaip gamos pirmas laipsnis
			   frequencies[0] = tones[i] * oct;
			   Debug.Log("freq pries cikla: " +frequencies[0]);
			   index = i;
			   
			   for(int k = 1; k < 8; k++){
				  
				   index += steps[j];
				    if(index > 11){
					   index -= 12;
					   oct = oct * 2;
					 
				   }
					frequencies[k] = tones[index] * oct;
					Debug.Log("freq po ciklo: " +frequencies[k]);
					Debug.Log("index (tone): " + index);
					Debug.Log("j (step before)): " + j);
					j++;
			   }
			   
			   break;
		   }
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
			thisFreq = thisFreq % 8;
			n++;
			//x1.text = n.ToString();
		} else if (dir == -1){
			if(thisFreq == 0){	
				frequency = frequencies[thisFreq];
				// padaro indekso reseta
				thisFreq = thisFreq % 8;
				n++;
				//x1.text = n.ToString();;
			} else {
				thisFreq -= 1;
				frequency = frequencies[Mathf.Abs(thisFreq)];
				// padaro indekso reseta
				thisFreq = thisFreq % 8;
				n++;
				//x1.text = n.ToString();
			}
		}
   }
   
   void Update(){
	   
	   	volume = enabled ? 0.1f : 0f;
	
	   if(Input.GetKeyDown(KeyCode.Space))
	   {
			gain = volume;
			frequency = frequencies[thisFreq];
			thisFreq += 1;
			thisFreq = thisFreq % 8;
			n++;
			//nt.text = n.ToString();
		}
	   if(Input.GetKeyUp(KeyCode.Space))
	   {
		   gain = 0;
	   }
	  
	   if(Input.GetKeyDown(KeyCode.RightArrow))
	   {
			gain = volume;
			changeNote(1);
		}
	   if(Input.GetKeyUp(KeyCode.RightArrow))
	   {
		   gain = 0;
	   }
	   
	    if(Input.GetKeyDown(KeyCode.LeftArrow))
	   {
			gain = volume;
			changeNote(-1);
		}
	   if(Input.GetKeyUp(KeyCode.LeftArrow))
	   {
		   gain = 0;
	   }
	  
	   
	   // Input.acceleration.y but filtered and absolute value
	   endFreq = accfilter.filterAccelValue(true)[1];;
	  
	  
	   // Veiksmas vyksta kas tris sekundes
	   /*
		  Dabar mano kodas nuskaito acc1, po dvieju sekundziu nuskaito acc2 ir skaiciuoja skirtuma tarp ju, tai nebutinai atspindi visa akcelerometro pokyti kuris ivyko per ta laiko tarpa, nes acc1 == acc2 imanoma
		  
		  sprendimas: dX += sumuoti rodiklio reiksme kiekviena kadra? (kaip nustatyti krypti, ar tai svarbu?)
		  
		  */
		  
		 /*
		   if(deltaonce == false){
			   acc1 = endFreq;
			   //x1.text = acc1.ToString();
			   tx = Time.time;
			   deltaonce = true;
			   //Debug.Log("deltaonce =" + deltaonce);
			   //status.text = "Measuring...";
		   }
		   
		   // Tikrinamas pokytis 
		   if(Time.time - tx >= 0.01)
		   {
			   acc2 = endFreq;
			   //x2.text = acc2.ToString();
			   dX = (acc2 - acc1) * 10000;
			   tx = Time.time;
			   //t.text = dX.ToString();
			   deltaonce = false;
			   //Debug.Log("deltaonce =" + deltaonce);
			   //status.text = "Measured!";
			   gx = Time.time;
		   }
	   
	   
	   if(dX >= 50 && dX != 0 && Time.time == gx){
		   if(endFreq > 0.5f)
		   {	
				if(once == false){
					changeNote(1);
					once = true;
					gain = volume;
					nextNoteTime = Time.time + noteCooldown;
				}
		
			}
	   }
		
		   if(endFreq < 0.5f && endFreq > -0.5f)
		   {
			   once = false;
		   }
	   
		   if(dX >= 50 && dX != 0 && Time.time == gx){
				if(endFreq < -0.5f)
			   {
					if(once == false)
					{
						changeNote(-1);
						once = true;
						nextNoteTime = Time.time + noteCooldown;
					}	
				}
		   }*/

   
		   //randDir = randDirSet[Random.Range(0, 2)];		   
		   
			
			
			if(randMelody){
				if(Time.time - tRand >= 1){
					//changeNote(randDir);
					randNote = Random.Range(0, 11);		   
					Debug.Log(randNote);
					frequency = frequencies[randNote];
					tRand = Time.time;
					Debug.Log("randomly changed note to " + randNote);
				}
			}
	   }
   
	  void OnAudioFilterRead(float[] data, int channels)
	  {	
	   //Vietoj vaiksciojimo per masyva darysiu tolydu
	   

	   increment = frequency * 2.0 * Mathf.PI / sampling_frequency;
	   
	   if(continuous){
		   gain = volume;
		   stopnegate = true;
	   } else {
		   if(stopnegate){
			   gain = 0;
			   stopnegate = false;
		   }
	   }
	   
	   
	   //increment = endFreq  * 2.0 * Mathf.PI / sampling_frequency;
	   
	   //freqmonitor.text = endFreq.ToString();
	   //freqmonitor.text = frequency.ToString();
	   
	   //Debug.Log("Synth 2 running");
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
