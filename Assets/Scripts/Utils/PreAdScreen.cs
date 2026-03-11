using System;
using System.Collections;
using TMPro;
using UnityEngine;
using Utils;

public class PreAdScreen : UIPanel
{
    [SerializeField] private TMP_Text _timer;
    [SerializeField] private int _adDelaySec;
    [SerializeField] private PreAdClicker _clicker;

    [SerializeField] private bool _shouldHideOthers;
             
    public static PreAdScreen Instance;

    private bool _isRu;

    private void Awake()
    {
        _isRu = LocalizationService.IsRu;
        if (Instance != null)
            Destroy(gameObject);

        Instance = this;         
    }

    public IEnumerator AdTimer(Action adCallback)
    {
        AnimatedShow();
        _clicker.StartField();

        for (var i = _adDelaySec; i > 0; i--)
        {
            _timer.text = LocalizationService.GetLocalizedStringAsync("Ad in", LocalizationTable.UI) + " " + i;
            yield return new WaitForSecondsRealtime(1);
        }

        AnimatedHide();

        adCallback.Invoke();
    }

    private void AnimatedShow()
    {
        Show();
        gameObject.SetActive(true);
        gameObject.transform.SetAsLastSibling();
    }

    private void AnimatedHide()
    {
        Hide();
    }

    public void StopField()
    {
        _clicker.StopField();
    }
}
