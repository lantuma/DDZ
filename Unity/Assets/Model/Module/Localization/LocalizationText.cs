using UnityEngine;
using UnityEngine.UI;

namespace ETModel
{
    public class LocalizationText: MonoBehaviour
    {
        [SerializeField,Header("LanguageKey")]
        public string LanguageKey = "";

        private void Awake()
        {
            LocalizationManager.onLanguageChanged += onLanguageChanged;
        }

        private void OnDestroy()
        {
            LocalizationManager.onLanguageChanged -= onLanguageChanged;
        }

        private void Start()
        {
            GetComponent<Text>().text = LocalizationManager.instance.GetValue(LanguageKey,LocalizationManager.languageType);
        }

        void onLanguageChanged(GameLanguageType languageType)
        {
            Text text = this.gameObject.GetComponent<Text>();

            if (text != null)
            {
                string value = LocalizationManager.instance.GetValue(LanguageKey,languageType);

                text.text = value;
            }
        }
    }
}
