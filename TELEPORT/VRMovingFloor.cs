using BNG;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using VSTS;

public class VRMovingFloor : MonoBehaviour
{
    [SerializeField] private List<FloorInformationstruct> _Floors;
    
    private Vector3 _Gravity = new Vector3(0, -5.5f, 0);
    private Transform _MovingTransform;
  
    public Transform GetMovingTransform { get => _MovingTransform; }
    public void SetMovingTransform(Transform movingTransform) => _MovingTransform = movingTransform;
    public List<FloorInformationstruct> GetFloors { get => _Floors; }
    public void SetObjectPosition(Transform ObjectTransform, int index) =>ObjectTransform = _Floors[index].Transforms;
    public void SetObjectPosition(Transform ObjectTransform, Transform goal) => ObjectTransform = goal;
    public void SetObjectPosition(Transform ObjectTransform, Vector3 po ) => ObjectTransform.position = po;
    private void Awake()
    {
        if (GetComponent<Grabbable>())
        {
            var gr = GetComponent<Grabbable>();
            GameObject.Destroy(gr);
        }
      
    }
    public void SetPlayerPosition(GameObject player, int index)
    {
        player.transform.position = _Floors[index].Transforms.position;
        var gravity = player.GetComponent<PlayerGravity>();
        gravity.Gravity = _Floors[index].Gravity ? _Gravity : Vector3.zero;
        player.GetComponent<CharacterController>().enabled = _Floors[index].Gravity ? true : false;
    }
    public void SetPlayerPosition(GameObject player, string floorname)
    {
        FloorInformationstruct? fl = new FloorInformationstruct();
        fl = _Floors.Find(x => x.FloorName == floorname);
        if (fl.HasValue)
        {
            player.transform.position = fl.Value.Transforms.position;
            var gravity = player.GetComponent<PlayerGravity>();
            gravity.Gravity = fl.Value.Gravity ? _Gravity : Vector3.zero;
            player.GetComponent<CharacterController>().enabled = fl.Value.Gravity ? true : false;
        }
    }
}
[Serializable]
public struct FloorInformationstruct
{
    public bool Gravity;
    public Transform Transforms;
    public string FloorName;
}
