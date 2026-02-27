using Cysharp.Threading.Tasks;
using UnityEngine.Localization.Settings;

public class LocalizationService
{
    public async UniTask<string> Get(string key, LocalizationTable table)
    {
        return await LocalizationSettings.StringDatabase.GetLocalizedStringAsync($"{table}", key);
    }
}

public enum LocalizationTable
{
    Resources
}