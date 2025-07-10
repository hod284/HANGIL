using BNG;
using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using VSTS;

public class MovingFloor_Basic : MonoBehaviour
{
    private UIStairObjects _UIStairObjects;
    [SerializeField] private string _Name;
    private void Start()
    {
        _UIStairObjects = GameObject.FindObjectOfType<UIStairObjects>();
        var _Pointer = GetComponent<PointerEvents>();
        if(_Pointer == null)
            _Pointer = gameObject.AddComponent<PointerEvents>();
       _Pointer.OnPointerClickEvent = new PointerEventDataEvent();
       _Pointer.OnPointerClickEvent.AddListener(showing);
       _Pointer.OnPointerEnterEvent = new PointerEventDataEvent();
       _Pointer.OnPointerEnterEvent.AddListener( x=> { HOVER(); });
       _Pointer.OnPointerExitEvent = new PointerEventDataEvent();
        _Pointer.OnPointerExitEvent.AddListener(x => { EXIT(); });
        GetComponent<BoxCollider>().enabled = false;
    }

    private void showing(PointerEventData eventData)
    {

        if (!_UIStairObjects.GetIItemInformationDirector.GetinformationUIShowing)
        {
            _UIStairObjects.GetIItemInformationDirector.OffBoxColider();
            _UIStairObjects.GetIItemInformationDirector.SetInformationUIShowing(true);
            _UIStairObjects.GetIItemInformationDirector.Laser(false);
            _UIStairObjects.GetIItemInformationDirector.GetUicusor.SetActive(true);
            switch (InputManager.Instance.InputType)
            {
                case E_INPUT_DEVICE.VR:  
                    UIPopupCommon.OpenBasicConfirmPopup("안내", "이동하시겠습니까?",
                            () =>
                            { 
                                _UIStairObjects.GetFloorInformation.Moving().Forget();
                                _UIStairObjects.GetIItemInformationDirector.OnBoxColider();
                            },
                            isLobby: false,
                            confirmText: "예",
                            cancelText: "아니요",
                            onClickCancel: () => {
                         
                                _UIStairObjects.Close();
                                _UIStairObjects.GetIItemInformationDirector.OnBoxColider();
                            },
                            vrType: E_VRUI_TYPE.VRUI_Fixed,
                            useBackGround: false);
                    break;
                case E_INPUT_DEVICE.PC:
                    InputManager.Instance.CursorActivate(true, 2);
                    UIPopupCommon.OpenBasicConfirmPopup("안내", "이동하시겠습니까?",
                            () => {
                                _UIStairObjects.GetIItemInformationDirector.OnBoxColider();
                                _UIStairObjects.GetFloorInformation.Moving().Forget();
                                },
                            isLobby: false,
                            confirmText: "예",
                            cancelText: "아니요",
                         onClickCancel: () =>
                         {
                             _UIStairObjects.GetIItemInformationDirector.OnBoxColider();
                             _UIStairObjects.Close();
                             });
                 break;
            }
          
        }
    }
    private void HOVER()
    {
        ObjectHighlighter.SetOutline(gameObject, ObjectHighlighter.E_OUTLINE.RED);
    }
    private void EXIT()
    {
        ObjectHighlighter.RemoveOutline(gameObject);
    }
}
