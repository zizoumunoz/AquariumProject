using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SettingsManager : MonoBehaviour
{
    public SliderValue animationDurationValue;
    public SliderValue algaeSizeValue;
    public TMP_InputField pathInput;
    private void OnEnable()
    {
        animationDurationValue.Set(Settings.Instance.animationDuration, "s");
        algaeSizeValue.Set(Settings.Instance.algaeSize, "");
        pathInput.text = Settings.Instance.folder;
#if UNITY_WEBGL && !UNITY_EDITOR
        pathInput.interactable = false;
#endif
    }
    public void ChangedAnimationDuration()
    {
        if (animationDurationValue.slider.value != Settings.Instance.animationDuration)
        {
            Settings.Instance.animationDuration = animationDurationValue.slider.value;
            animationDurationValue.Set(Settings.Instance.animationDuration, "s");
            return;
        }
        else if (animationDurationValue.inputText.text.Trim().Length < 1)
        {
            return;
        }

        string newValue = animationDurationValue.inputText.text.Trim();
        if (newValue[^1] == 's' || newValue[^1] == 'S')
        {
            newValue = newValue[..^1].Trim();
        }

        float newValuefloat = animationDurationValue.slider.value;
        try
        {
            newValuefloat = float.Parse(newValue);
        }
        catch (Exception)
        {
        }

        newValuefloat = Mathf.Clamp(newValuefloat, animationDurationValue.slider.minValue, animationDurationValue.slider.maxValue);

        Settings.Instance.animationDuration = newValuefloat;
        animationDurationValue.Set(Settings.Instance.animationDuration, "s");
        SaveAndLoad.UpdateSettings();
    }
    public void ChangedAlgaeSize()
    {
        if (algaeSizeValue.slider.value != Settings.Instance.algaeSize)
        {
            Settings.Instance.algaeSize = (int)algaeSizeValue.slider.value;
            algaeSizeValue.Set(Settings.Instance.algaeSize, "");
            return;
        }
        else if (algaeSizeValue.inputText.text.Trim().Length < 1)
        {
            return;
        }

        string newValue = algaeSizeValue.inputText.text.Trim();
        if (newValue[^1] == 's' || newValue[^1] == 'S')
        {
            newValue = newValue[..^1].Trim();
        }

        float newValuefloat = algaeSizeValue.slider.value;
        try
        {
            newValuefloat = float.Parse(newValue);
        }
        catch (Exception)
        {
        }

        newValuefloat = Mathf.Clamp(newValuefloat, algaeSizeValue.slider.minValue, algaeSizeValue.slider.maxValue);

        Settings.Instance.algaeSize = Mathf.FloorToInt(newValuefloat);
        algaeSizeValue.Set(Settings.Instance.algaeSize, "s");
        SaveAndLoad.UpdateSettings();
    }

    public void ChangedPath()
    {
        string newPath = pathInput.text;
        if (SaveAndLoad.TryToSave(newPath))
        {
            SaveAndLoad.Replace(newPath);
        }
        else
        {
            Debug.Log("Unable to change path to target path");
        }
        SaveAndLoad.UpdateSettings();
    }
}

[Serializable]
public class SliderValue
{
    public TMP_InputField inputText;
    public TextMeshProUGUI placeholderText;
    public Slider slider;
    public void Set(float value, string additional)
    {
        slider.value = value;
        placeholderText.text = value.ToString("N1") + additional;
        if (inputText.text != "")
        {
            inputText.text = value.ToString("N1") + additional;
        }
        else
        {
            inputText.text = "";
        }
    }
}
