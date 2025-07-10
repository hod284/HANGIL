using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using BNG;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using VSTS;
using System.Linq;

public class UIStairObjects : MonoBehaviour
{
    [SerializeField] private List<FloorInformationstructList> _Stairgroup = new List<FloorInformationstructList>();
    [SerializeField] private GameObject _Floorview_PC;
    [SerializeField] private GameObject _Floorview_VR;
    [SerializeField] private GameObject _ButtonPrefab_PC;
    [SerializeField] private GameObject _ButtonPrefab_VR;
    [SerializeField] private GameObject _ButtonParent_PC;
    [SerializeField] private GameObject _ButtonParent_VR;
    [SerializeField] private PointerEvents _CloseButton_vr;
    [SerializeField] private UnityEngine.UI.Button _CloseButton_pc;
    [SerializeField] private GameObject _Righthand;
    [SerializeField] private GameObject _MapParent;
    private GameObject _Floorview;
    private UIItemInformationDirector _Director;
    private FloorInformation _Button;
    private List<MovingFloor_Basic> _StairObjectList;
    private List< Scaffolding> _Scaffoldings;
    public FloorInformation GetFloorInformation { get => _Button; }
    public List<MovingFloor_Basic> GetStairObjectList { get => _StairObjectList; }
    public List<FloorInformationstructList> GetStairgroup { get => _Stairgroup; }
    public UIItemInformationDirector GetIItemInformationDirector { get => _Director; }
    public GameObject GetRighthand { get => _Righthand; }
    public GameObject GetFloorview { get => _Floorview; }

  

    private void Start()
    {
        _Director = GameObject.FindAnyObjectByType<UIItemInformationDirector>();
        _CloseButton_vr.OnPointerClickEvent = new PointerEventDataEvent();
        _CloseButton_vr.OnPointerClickEvent.AddListener((PointerEventData eventData) => { Close(); });
        _CloseButton_vr.OnPointerEnterEvent = new PointerEventDataEvent();
        _CloseButton_vr.OnPointerEnterEvent.AddListener(close_HOVER);
        _CloseButton_vr.OnPointerExitEvent = new PointerEventDataEvent();
        _CloseButton_vr.OnPointerExitEvent.AddListener(close_EXIT);
        _CloseButton_pc.onClick.AddListener(Close);
        _ButtonPrefab_PC.GetComponent<FloorInformation>().GetComponent<FloorInformation>().SetVRMovingFloor(this);
        _ButtonPrefab_VR.GetComponent<FloorInformation>().GetComponent<FloorInformation>().SetVRMovingFloor(this);
        if (InputManager.Instance.InputType == E_INPUT_DEVICE.PC)
        {
            _Floorview = _Floorview_PC;
            _Button = _ButtonPrefab_PC.GetComponent<FloorInformation>();
        }
        else
        {
            _Floorview = _Floorview_VR;
            _Button = _ButtonPrefab_VR.GetComponent<FloorInformation>();
        }
        _StairObjectList = _MapParent.transform.GetComponentsInChildren<MovingFloor_Basic>().ToList();
        _Scaffoldings  =  transform.GetComponentsInChildren<Scaffolding>().ToList();
    }
    private void close_HOVER(PointerEventData eventData)
    {
        _CloseButton_vr.GetComponent<UnityEngine.UI.Image>().color = Color.gray;
    }
    private void close_EXIT(PointerEventData eventData)
    {
        _CloseButton_vr.GetComponent<UnityEngine.UI.Image>().color = Color.white;
    }
    public void Close()
    {
        _Director.Laser(true);
        _Director.SetInformationUIShowing(false);
        _Floorview.gameObject.SetActive(false);

        if (InputManager.Instance.InputType == E_INPUT_DEVICE.PC)
            InputManager.Instance.CursorActivate(false, 2);
    }
    public void ScaffoldingOn()
    { 
       for(int i  =0; i< _Scaffoldings.Count;i++)
            _Scaffoldings[i].transform.GetComponent<BoxCollider>().enabled = true;
    }
    public void ScaffoldingOff()
    {
        for (int i = 0; i < _Scaffoldings.Count; i++)
            _Scaffoldings[i].transform.GetComponent<BoxCollider>().enabled =false;
    }
}

[Serializable]
public struct FloorInformationstructList
{
    public string _Name;
    public List<FloorInformationstruct> list;
}
