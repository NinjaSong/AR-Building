using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SessionDemographicQuestion : MonoBehaviour
{
    public RectTransform rootRect;
    public Text promptText;
    public Dropdown responseDropdown;
    public LayoutElement responseDropdownLayout;
    public HorizontalLayoutGroup layoutGroup;
    public InputField specifyField;
    public Color colorNormal;
    public Color colorFaded;

    List<string> options;

    private bool chosenOnce;

    private float specify;
    private float specifySpeed = 0.15f;
    private float specifyCutoff = 0.99f;

    public void Initialize(Transform parent, DemographicQuestion question)
    {
        transform.SetParent(parent);
        transform.localScale = Vector3.one;
        transform.SetAsLastSibling();
        
        promptText.text = "<color=#00000088>#" + transform.GetSiblingIndex() + ":</color>\t" + question.prompt;

        options = question.responses.ToList();
        options.Add("Other, please specify");
        options.Add("Prefer not to say");

        responseDropdown.ClearOptions();
        responseDropdown.AddOptions(options);
        responseDropdown.captionText.text = "Select one...";
        chosenOnce = false;
    }

    public void ResetQuestion()
    {
        responseDropdown.value = options.Count + 1;
        responseDropdown.captionText.text = "Select one...";
        chosenOnce = false;
    }

    private void Update()
    {
        responseDropdown.captionText.color = responseDropdown.value != -1 ? colorNormal : colorFaded;

        float specifyTarg = NeedsToSpecify() ? 1 : 0;
        if (specifyTarg == 0)
            specifyField.text = "";

        specifyField.text = specifyField.text.Replace("|", "").TrimStart();

        specify = Mathf.Lerp(specify, specifyTarg, specifySpeed);
        specifyField.interactable = specify > specifyCutoff;

        float t = Util.Smootherstep(specify);
        responseDropdownLayout.preferredWidth = (1 - t) * rootRect.sizeDelta.x;
        layoutGroup.spacing = t * 7;
    }

    public void SpecifyFieldComplete(string input)
    {
        specifyField.text = specifyField.text.Trim();
    }

    public void OnValueChanged(int value)
    {
        chosenOnce = true;
    }

    public bool IsSatisfied()
    {
        if (NeedsToSpecify())
            return !string.IsNullOrEmpty(specifyField.text);
        return chosenOnce;
    }

    private bool NeedsToSpecify()
    {
        return (responseDropdown.value == options.Count - 2);
    }

    public override string ToString()
    {
        string response = options[responseDropdown.value];
        if (NeedsToSpecify())
            response = specifyField.text;
        return response;
    }
}