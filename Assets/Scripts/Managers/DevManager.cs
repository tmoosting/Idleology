using UnityEngine;
using UnityEngine.UI;

namespace Managers
{
    public class DevManager : MonoBehaviour
    {
        // Handle dev settings and Developer Mode (for speedslider, more start money,..)
        
        [Header("Assigns")] 
        public Slider speedSlider;
   
   
        [Header("Settings")] 
        public bool enableDevMode;
  
   
        public void ChangeGameSpeed (float speed)
        {
            Time.timeScale = speed;
        }

        private void Start()
        {
            if (enableDevMode == true)
            {
                speedSlider.gameObject.SetActive(true);

            }
            else
            {
                speedSlider.gameObject.SetActive(false);
            }
        }
    }
}
