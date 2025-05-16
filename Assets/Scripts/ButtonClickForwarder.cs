using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.Interaction.Toolkit;

[RequireComponent(typeof(UnityEngine.XR.Interaction.Toolkit.Interactors.XRRayInteractor))]
public class ButtonClickForwarder : MonoBehaviour
{
    private UnityEngine.XR.Interaction.Toolkit.Interactors.XRRayInteractor _ray;
    
    void Awake()
    {
        _ray = GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactors.XRRayInteractor>();
    }

    void OnEnable()
    {
        _ray.selectEntered.AddListener(ForwardClick);
    }

    void OnDisable()
    {
        _ray.selectEntered.RemoveListener(ForwardClick);
    }

    private void ForwardClick(SelectEnterEventArgs args)
    {
        // see if what we hit has a Unity UI Button
        var go = args.interactableObject.transform.gameObject;
        var btn = go.GetComponent<Button>();
        if (btn != null && btn.interactable)
            btn.onClick.Invoke();
    }
}