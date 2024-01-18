using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;
using System.Text.RegularExpressions;

namespace KillerDoors.Services.Localization
{
    [RequireComponent(typeof(TextMeshProUGUI))]
    [DisallowMultipleComponent]
    public class LocalizedText : MonoBehaviour
    {
        [SerializeField] private List<string> _subKeysAndConsts = new List<string>();
        [Header("Relative with CountableNounsKeys")]
        [SerializeField] private List<float> _numbers = new List<float>();
        [Header("Depends from Numbers")]
        [SerializeField] private List<string> _countableNounsKeys = new List<string>();
        private string _key;

        private const string NumberSubString = "/NUMBERSUBSTRING";
        private const string NewLine = "/NEWLINE";
        private const string SubKeyOrConst = "/SUBKEY";

        private TextMeshProUGUI _text;

        public event Action Localized;

        public bool IsInitialized =>
             _text && _localizationService != null;

        private ILocalizationService _localizationService;
        void Start()
        {
            if (!IsInitialized)
                Init();

            Localize();
        }
        public void Init()
        {
            _text = GetComponent<TextMeshProUGUI>();

            if (string.IsNullOrEmpty(_key))
                _key = _text.text;

            _localizationService = ServiceLocator.Container.Single<ILocalizationService>();

            if (IsInitialized)
                _localizationService.LanguageChanged += OnLanguageChange;
        }
        public void Localize(string newKey = null)
        {
            if (newKey != null)
                _key = newKey;

            if (!IsInitialized)
                Init();

            string value = _localizationService.GetTranslate(_key);

            InsertNewLines(ref value);

            InsertConstsOrTranslatesSubKeys(ref value);

            InsertNumbersAndNouns(ref value);

            CheckOnFinishLocalize(value);

            _text.text = value;

            Localized?.Invoke();
        }
        public void Localize(string newKey, List<string> subKeys)
        {
            _subKeysAndConsts = subKeys;
            Localize(newKey);
        }
        public void Localize(string newKey, List<float> numbers, List<string> countableNounsKeys)
        {
            _numbers = numbers;
            _countableNounsKeys = countableNounsKeys;
            Localize(newKey);
        }
        public void Localize(string newKey, List<string> subKeys, List<float> numbers, List<string> countableNounsKeys)
        {
            _subKeysAndConsts = subKeys;
            _numbers = numbers;
            _countableNounsKeys = countableNounsKeys;
            Localize(newKey);
        }
        private void OnDestroy()
        {
            if (_localizationService != null)
                _localizationService.LanguageChanged -= OnLanguageChange;
        }
        private void OnLanguageChange() =>
            Localize(_key);

        private void InsertNewLines(ref string value)
        {
            if (value.Contains(NewLine))
                value = value.Replace(NewLine, Environment.NewLine);
        }

        private void InsertNumbersAndNouns(ref string value)
        {
            if (_numbers.Count == 0) return;

            Regex regex = new Regex(Regex.Escape(NumberSubString));

            for (int indexNounKey = 0; indexNounKey < _numbers.Count; indexNounKey++)
            {
                string insertValue = _numbers[indexNounKey].ToString();

                TryAddCountableNoun(ref insertValue, indexNounKey);

                value = regex.Replace(value, insertValue, 1);
            }
        }

        private void TryAddCountableNoun(ref string insertValue, int indexNounKey)
        {
            if (indexNounKey < _countableNounsKeys.Count)
            {
                string[] wordsInQuantities1_2_5 = _localizationService.GetTranslate(_countableNounsKeys[indexNounKey]).Split("/");

                if (wordsInQuantities1_2_5.Length != 0)
                    insertValue += " " + ILocalizationService.GetCountableNoun(_numbers[indexNounKey], wordsInQuantities1_2_5);
            }
        }

        private void InsertConstsOrTranslatesSubKeys(ref string value)
        {
            if (_subKeysAndConsts.Count == 0) return;

            Regex regex = new Regex(Regex.Escape(SubKeyOrConst));

            foreach (string subKey in _subKeysAndConsts)
            {
                string insertValue = float.TryParse(subKey, out float _)
                    ? subKey
                    : _localizationService.GetTranslate(subKey);

                value = regex.Replace(value, insertValue, 1);
            }
        }
        private void CheckOnFinishLocalize(string value)
        {
#if UNITY_EDITOR
            if (value.Contains(NumberSubString))
                Debug.LogWarning($"Not enough Numbers and Nouns. Key: {_key}, Object: {gameObject.name}.");

            if (value.Contains(SubKeyOrConst))
                Debug.LogWarning($"Not enough SubKeys. Key: {_key}, Object: {gameObject.name}.");
#endif
        }
    }
}