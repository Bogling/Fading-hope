using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    [SerializeField] private float StartSpeed;
    [SerializeField] private float TravelSpeed;
    [SerializeField] private float EndSpeed;
    [SerializeField] private bool isLooping;
    [SerializeField] private bool deactivateOnArrival;
    [SerializeField] private Transform StartPosition;
    [SerializeField] private Transform EndPosition;
}
