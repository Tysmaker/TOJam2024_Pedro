using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TestButtonEventTrigger : MonoBehaviour
{
    private EventTrigger eventTrigger;

    private void Awake()
    {
        eventTrigger = GetComponent<EventTrigger>();
        if (eventTrigger == null)
        {
            eventTrigger = gameObject.AddComponent<EventTrigger>();
        }
        AddEventTrigger(EventTriggerType.PointerEnter, OnPointerEnter);
        AddEventTrigger(EventTriggerType.PointerExit, OnPointerExit);
        AddEventTrigger(EventTriggerType.PointerDown, OnPointerDown);
        AddEventTrigger(EventTriggerType.PointerUp, OnPointerUp);
    }

    private void AddEventTrigger(EventTriggerType triggerType, System.Action<BaseEventData> action)
    {
        EventTrigger.Entry entry = new EventTrigger.Entry();
        entry.eventID = triggerType;
        entry.callback.AddListener((data) => action(data));
        eventTrigger.triggers.Add(entry);
    }

    private void OnPointerEnter(BaseEventData data)
    {
        Debug.Log("Pointer Enter");
    }

    private void OnPointerExit(BaseEventData data)
    {
        Debug.Log("Pointer Exit");
    }

    private void OnPointerDown(BaseEventData data)
    {
        Debug.Log("Pointer Down");
    }

    private void OnPointerUp(BaseEventData data)
    {
        Debug.Log("Pointer Up");
    }
}
