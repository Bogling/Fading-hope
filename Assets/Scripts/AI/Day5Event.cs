using System.Collections;
using UnityEngine;

public abstract class Day5Event : MonoBehaviour
{
    [SerializeField] protected int eventWeight;
    [SerializeField] protected int screamerID;
    protected bool isActive;
    protected bool isResting;
    protected bool isEnraged;
    public int EventWeight { get { return eventWeight; } }
    public abstract void StartEvent();
    public abstract void EndEvent();

    public abstract IEnumerator EventRest();

    public abstract bool IsActive();
    public abstract bool IsResting();
    public abstract void Enrage();
    public int ScreemerID { get {return screamerID;}}
}
