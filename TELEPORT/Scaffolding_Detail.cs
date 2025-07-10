using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VSTS;
using BNG;
using System.Linq;
public class Scaffolding_Detail : MonoBehaviour
{

    [SerializeField] private StairObjects_Detail _UIStairObjects;
    [SerializeField] private int _FloorIndex;
    private bool ThisGravity;
    private Vector3 ThisPosition;
    private  Vector3 _GravityValue = new Vector3(0, -7.81f, 0);
    private List<Collider> hits;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "PlayerController")
        {
            var stairgroup = _UIStairObjects.GetStairgroup;
            var floorlist = stairgroup.Find(x => x._Name == transform.parent.gameObject.name);
            var floor = floorlist.list[_FloorIndex];
            ThisGravity = floor.Gravity;
            ThisPosition = floor.Transforms.position;
            showing();
        }
    }
    private void OnTriggerStay(Collider other)
    {
        
    }
    /// <summary>
    ///  콜라이더 구역에서 나가면 사다리나 계단 컴포넌트에 bngframework에 있는 pointerevents클래스  오프 
    /// </summary>
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.name == "PlayerController")
        {
            UIManager.Instance.CloseUI(E_UI_TYPE.UIPopupCommonInTraining);
            if (InputManager.Instance.InputType == E_INPUT_DEVICE.PC)
                InputManager.Instance.CursorActivate(false, 2);
        }
    }
    private void showing()
    {
        switch (InputManager.Instance.InputType)
        {
            case E_INPUT_DEVICE.VR:
                UIPopupCommon.OpenBasicConfirmPopup("안내", "이동하시겠습니까?",
                        () =>
                        {
                            moving().Forget();
                        },
                        isLobby: false,
                        confirmText: "예",
                        cancelText: "아니요",
                        onClickCancel: () =>{},
                        vrType: E_VRUI_TYPE.VRUI_Fixed,
                        useBackGround: false);
                break;
            case E_INPUT_DEVICE.PC:
                InputManager.Instance.CursorActivate(true, 2);
                UIPopupCommon.OpenBasicConfirmPopup("안내", "이동하시겠습니까?",
                        () =>
                        {
                            moving().Forget();
                        },
                        isLobby: false,
                        confirmText: "예",
                        cancelText: "아니요",
                     onClickCancel: () =>
                     {
                         InputManager.Instance.CursorActivate(false, 2);
                     });
                break;
        }
    }
    private async UniTask moving()
    {
        if (InputManager.Instance.InputType == E_INPUT_DEVICE.PC)
            InputManager.Instance.CursorActivate(false, 2);
        var character = GameObject.FindObjectOfType<CharacterController>();
        character.enabled = false;
        character.transform.position =  ThisPosition;
        await UniTask.Yield(PlayerLoopTiming.LastPostLateUpdate);
        var GravityComponent = character.GetComponent<PlayerGravity>();
        character.enabled = ThisGravity;
        GravityComponent.Gravity = ThisGravity ? _GravityValue : Vector3.zero;
    }
}

