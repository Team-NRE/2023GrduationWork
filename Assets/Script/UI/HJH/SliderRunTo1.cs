using UnityEngine;
using UnityEngine.UI;

public class SliderRunTo1 : MonoBehaviour
{
	public GameObject loadingText, finishText;
 
     public bool b=true;
	 public Slider slider;
	 public float speed=0.5f;

	float time =0f;
	
	void Start()
	{
		slider = GetComponent<Slider>();
	}
  
    void Update()
    {
		if(b)
		{
			time+=Time.deltaTime*speed;
			slider.value = time;
			
			if(time>1)
			{
				b = false;
				time=0;
			}

			if (loadingText != null) loadingText.SetActive(b);
			if (finishText != null) finishText.SetActive(!b);
    	}
	}
}
