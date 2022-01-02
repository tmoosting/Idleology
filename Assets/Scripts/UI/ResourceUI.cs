using Managers;
using ScriptableObjects;
using TMPro;
using UnityEngine;

namespace UI
{
    public class ResourceUI : MonoBehaviour
    {
        [Header("Assigns")] 
        [SerializeField] TextMeshProUGUI creditText;
        [SerializeField] TextMeshProUGUI creditIncomeText;
        [SerializeField] TextMeshProUGUI happinessText;

        public void UpdateTexts()
        { 
            creditText.text = GameStateManager.Instance.GetComponent<ResourceManager>().GetResource(Resource.Type.Credit)._amount.ToString();
            creditIncomeText.text = GameStateManager.Instance.GetComponent<ResourceManager>().GetResource(Resource.Type.Credit)._amount.ToString();
            happinessText.text = GameStateManager.Instance.GetComponent<ResourceManager>().GetResource(Resource.Type.Happiness)._amount.ToString();
        }
    }
}
