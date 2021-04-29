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
   
   public Text x1;
   public Text x2;
   public Text t;
   public Text status;
   public double frequency = 440.0;
   
   private double increment;
   private double phase;
   private double sampling_frequency = 48000.0;
   private int n = 0;
   private bool once = false;
   
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
   
   /*GameObject CreateText(Transform canvas_transform, float x, float y, string text_to_print, int font_size, Color text_color)
	{
		GameObject UItextGO = new GameObject("Text2");
		UItextGO.transform.SetParent(canvas_transform);

		RectTransform trans = UItextGO.AddComponent<RectTransform>();
		trans.anchoredPosition = new Vector2(x, y);

		Text text = UItextGO.AddComponent<Text>();
		text.text = text_to_print;
		text.fontSize = font_size;
		text.color = text_color;

		return UItextGO;
	}*/
   
   IEnumerator wait(){
	   gain = volume;
		if(once == false){
			changeNote(1);
			once = true;
		}
		
		//CreateText(GameObject.Find("Canvas"), 0f, 0f, "coroutine delayed", 14, new Color(1f,1f,1f,1f));
		yield return new WaitForSeconds(2f);
		x1.text = "coroutine delayed";
   }
   
 
   
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
   
   public void down(){
		gain = volume;
		frequency = frequencies[thisFreq];
		thisFreq += 1;
		thisFreq = thisFreq % nf.Count;
   }
   
   public void up(){
	   
	   gain = 0;
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
	  /*
	  if(Input.GetKeyDown(KeyCode.Space))
	   {
			gain = volume;
			frequency = nf[thisFreq];
			thisFreq += 1;
			thisFreq = thisFreq % nf.Count;
			n++;
			x1.text = n.ToString();
		}
	   if(Input.GetKeyUp(KeyCode.Space))
	   {
		   gain = 0;
	   }*/
	  
	   
	   
	   // Veiksmas vyksta kas tris sekundes
	   if(Time.time - gx > 3){
		   if(deltaonce == false){
			   acc1 = Input.acceleration.x;
			   x1.text = acc1.ToString();
			   tx = Time.time;
			   deltaonce = true;
			   Debug.Log("deltaonce =" + deltaonce);
			   status.text = "Measuring...";
		   }
		   
		   // Tikrinamas pokytis per dvi sekundes
		   if(Time.time - tx >= 2)
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
	   }
	   
	   
	   if(Time.time > nextNoteTime){
		   if(Input.acceleration.x > 0.5f)
		   {	
				gain = volume;
				if(once == false){
					changeNote(1);
					once = true;
					nextNoteTime = Time.time + noteCooldown;
				}
		
			}
	   }
		
	   if(Input.acceleration.x < 0.5f && Input.acceleration.x > -0.5f)
	   {
		   once = false;
		   gain = 0;
	   }
	   
	   if(Time.time > nextNoteTime){
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
	    }
   }
   
	  void OnAudioFilterRead(float[] data, int channels)
	  {	
	   increment = frequency * 2.0 * Mathf.PI / sampling_frequency;
	   
	   for (int i = 0; i < data.Length; i += channels)
	   {
		   phase += increment;
		   data[i] = (float)(gain * Mathf.Sin((float)phase));
		   
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
