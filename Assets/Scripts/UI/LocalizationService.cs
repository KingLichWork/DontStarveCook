using Cysharp.Threading.Tasks;
using Kimicu.YandexGames;
using UnityEngine;
using UnityEngine.Localization.Settings;

public class LocalizationService
{
    public static bool IsRu { get; private set; }

    public static async UniTask Initialize()
    {
        await LocalizationSettings.InitializationOperation;
#if UNITY_EDITOR
        IsRu = true;
#else
            IsRu = YandexGamesSdk.Language.Contains("ru");
#endif
        Debug.Log("IsRu: " + IsRu);
        LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[IsRu ? 1 : 0];
    }

    public static string GetLocalizedString(string key, LocalizationTable table, object args = null)
    {
        return LocalizationSettings.StringDatabase.GetLocalizedString(table.ToString(), key, arguments: args);
    }

    public static async UniTask<string> GetLocalizedStringAsync(string key, LocalizationTable table, object args = null)
    {
        return await LocalizationSettings.StringDatabase.GetLocalizedStringAsync(table.ToString(), key, arguments: args);
    }
}

public enum LocalizationTable
{
    Resources,
    UI
}