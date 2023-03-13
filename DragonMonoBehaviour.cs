using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class DragonMonoBehaviour : MonoBehaviour
{
    protected virtual void Start()
    {

    }
    protected virtual void Awake()
    {
        this.LoadComponents();
    }
    protected virtual void Reset()
    {
        this.LoadComponents();
        this.ResetValue();
    }

    protected virtual void LoadComponents()
    {

    }
    
    protected virtual void ResetValue()
    {

    }


}
