using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StairObjects_Detail : MonoBehaviour
{
    [SerializeField] private List<FloorInformationstructList> _Stairgroup = new List<FloorInformationstructList>();

    public List<FloorInformationstructList> GetStairgroup { get => _Stairgroup; }

    private void Start()
    {
        for(int I =0; I< transform.childCount; I++)
           transform.GetChild(I).gameObject.SetActive(true);
    }
}
