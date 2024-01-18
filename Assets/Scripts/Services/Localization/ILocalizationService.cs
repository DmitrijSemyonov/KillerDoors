using System;

namespace KillerDoors.Services.Localization
{
    public interface ILocalizationService : IService
    {
        bool IsInitialized { get; }

        event Action LanguageChanged;

        string GetTranslate(string key, int languageId = -1);
        void Init();
        void SetLanguage(int id);

        public static string GetCountableNoun(int quantity, params string[] wordsInQuantities1_2_5)
        {
            if (wordsInQuantities1_2_5.Length != 3) return wordsInQuantities1_2_5[0];

            string result;
            int num100 = quantity % 100;
            if (num100 > 4 && num100 < 21) result = wordsInQuantities1_2_5[2];
            else
            {
                int num10 = num100 % 10;
                if (num10 == 1) result = wordsInQuantities1_2_5[0];
                else if (num10 > 1 && num10 < 5) result = wordsInQuantities1_2_5[1];
                else result = wordsInQuantities1_2_5[2];
            }
            return result;
        }
        public static string GetCountableNoun(float quantity, params string[] wordsInQuantities1_2_5)
        {
            if (wordsInQuantities1_2_5.Length != 3) return wordsInQuantities1_2_5[0];

            string result;
            float num100 = quantity % 100;
            if (num100 > 4 && num100 < 21) result = wordsInQuantities1_2_5[2];
            else
            {
                float num10 = num100 % 10;
                if (num10 == 1) result = wordsInQuantities1_2_5[0];
                else if (num10 > 1 && num10 < 5) result = wordsInQuantities1_2_5[1];
                else result = wordsInQuantities1_2_5[2];
            }
            return result;
        }
    }
}