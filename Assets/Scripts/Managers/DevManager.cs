using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Managers
{
    public class DevManager : MonoBehaviour
    {
        // Handle dev settings and Developer Mode (for speedslider, more start money,..)
        
        [Header("Assigns")] 
        public Slider speedSlider;
        public TextMeshProUGUI speedSliderText;
   
   
        [Header("Settings")] 
        public bool enableDevMode;
        public bool enableSpeedSlider;
  
    

        private void Awake()
        { 
                speedSlider.gameObject.SetActive(enableSpeedSlider); 
        }

        private void Update()
        { 
            if (enableSpeedSlider == true)
                   SetTimerText();
        }

        public void ChangeGameSpeed (float speed)
        {
            Time.timeScale = speed;
        }
        
        void SetTimerText()
        {
            float minutes = Mathf.FloorToInt(GameStateManager.Instance.timePassed / 60);
            float seconds = Mathf.FloorToInt(GameStateManager.Instance.timePassed % 60);
            float milliseconds = 100;
            milliseconds = 1000 - ((Time.timeSinceLevelLoad * 1000f) % 1000);
            string miliString = milliseconds.ToString().Substring(0, 2);
            string minuteString = minutes.ToString();
            if (minutes < 10)
                minuteString = "0" + minuteString;
            string secondsString = seconds.ToString();
            if (seconds < 10)
                secondsString = "0" + secondsString;
            speedSliderText.text = "" + minuteString + ":" + secondsString + ":" + miliString;
        }
    }
}
