using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Collider2D))]
public class Trigger : MonoBehaviour
{
    [SerializeField] private UnityEvent _onTriggerEnter;
    [SerializeField] private UnityEvent _onTriggerExit;
    [SerializeField] private string[] _tags;

    private Collider2D _trigger;

    private void OnEnable()
    {
        _trigger = GetComponent<Collider2D>();
        _trigger.isTrigger = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (HasTag(collision))
        {
            _onTriggerEnter?.Invoke();
        }
    }
    
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (HasTag(collision))
        {
            _onTriggerExit?.Invoke();
        }
    }

    private bool HasTag(Collider2D collision)
    {
        foreach (var tag in _tags)
        {
            bool hasTag = collision.CompareTag(tag);
            if (hasTag) return true;
        }
        return false;
    }

}
