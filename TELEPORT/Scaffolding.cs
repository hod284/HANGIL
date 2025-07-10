using BNG;
using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VSTS;
/// <summary>
/// 계단에 있는 컴포넌트 pointerevents클래스온 오프 하는 클래스
/// </summary>
public class Scaffolding : MonoBehaviour
{
    [SerializeField] private UIStairObjects _UIStairObjects;
    [SerializeField] private int _FloorIndex;
    private MovingFloor_Basic _MovingFloor_BasicObject;
    [SerializeField] private bool _InitePotal=true;
    /// <summary>
    ///  콜라이더 충돌 하면 사다리및계단 컴포넌트에 bngframework에 있는 pointerevents클래스 온 레이케스트 충돌 하며 텔레포트 할 위치 정보및 허공이나 딸에서 움직일지 말지 설정값 셋팅
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter(Collider other)
    {
      var stairgroup =  _UIStairObjects.GetStairgroup;
      var floorlist =  stairgroup.Find(x => x._Name == transform.parent.gameObject.name);
        var floor = floorlist.list[_FloorIndex];
        _UIStairObjects.GetFloorInformation.SetGravity(floor.Gravity);
        _UIStairObjects.GetFloorInformation.SetPosition(floor.Transforms.position);
        
            if (!_InitePotal)
            {
                if (_MovingFloor_BasicObject == null)
                    _MovingFloor_BasicObject = _UIStairObjects.GetStairObjectList.Find(x => x.gameObject.name == transform.parent.gameObject.name);
                if (_MovingFloor_BasicObject != null)
                    _MovingFloor_BasicObject.GetComponent<BoxCollider>().enabled = true;
            }
            else
                showing();
    }
    private void OnTriggerStay(Collider other)
    {
        if (!_InitePotal)
            _MovingFloor_BasicObject.GetComponent<BoxCollider>().enabled = true;
    }
    /// <summary>
    ///  콜라이더 구역에서 나가면 사다리나 계단 컴포넌트에 bngframework에 있는 pointerevents클래스  오프 
    /// </summary>
    private void OnTriggerExit(Collider other)
    {
        _UIStairObjects.Close();
        _UIStairObjects.GetIItemInformationDirector.OnBoxColider();
        if (!_InitePotal)
        {
            ObjectHighlighter.RemoveOutline(_MovingFloor_BasicObject.gameObject);

            if (_MovingFloor_BasicObject == null)
                _MovingFloor_BasicObject = _UIStairObjects.GetStairObjectList.Find(x => x.gameObject.name == transform.parent.gameObject.name);
            if (_MovingFloor_BasicObject != null)
                _MovingFloor_BasicObject.GetComponent<BoxCollider>().enabled = false;
        }
        else
        {
            UIManager.Instance.CloseUI(E_UI_TYPE.UIPopupCommonInTraining);
            _UIStairObjects.ScaffoldingOn();
        }
    }
    private void showing()
    {
        if (!_UIStairObjects.GetIItemInformationDirector.GetinformationUIShowing)
        {
            _UIStairObjects.GetIItemInformationDirector.OffBoxColider();
            _UIStairObjects.GetIItemInformationDirector.SetInformationUIShowing(false);
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
                                if (_InitePotal)
                                    _UIStairObjects.ScaffoldingOff();
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
                                if (_InitePotal)
                                    _UIStairObjects.ScaffoldingOff();
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

}
