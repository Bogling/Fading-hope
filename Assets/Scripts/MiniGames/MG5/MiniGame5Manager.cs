using System.Collections.Generic;
using UnityEngine;

public class MiniGame5Manager : MonoBehaviour
{
    private List<MG5Card> cards = new List<MG5Card>(56);
    [SerializeField] private MG5Card cardHPrefab;
    [SerializeField] private MG5Card cardDPrefab;
    [SerializeField] private MG5Card cardCPrefab;
    [SerializeField] private MG5Card cardSPrefab;



    private static MiniGame5Manager instance;
    void Awake() {
        instance = this;

        
    }
}
