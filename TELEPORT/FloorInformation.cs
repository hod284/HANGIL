
using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using BNG;
using UnityEngine.EventSystems;
using VSTS;

public class FloorInformation : MonoBehaviour
{
    [SerializeField] private Vector3 _Position;
    [SerializeField] private bool _Gravity;
    [SerializeField] private GameObject _Player;
    [SerializeField] private Image _BlackScreen;
    [SerializeField] private GameObject _MovingFloorView;
     private UIStairObjects _MovingFloor;
    [SerializeField] private PointerEvents _Pointer;
    [SerializeField] private UnityEngine.UI.Button _Button;
    private Vector3 _GravityVolum = new Vector3(0, -5.5f, 0);
    public void SetPosition(Vector3 po) => _Position = po;
    public void SetGravity(bool gr) => _Gravity = gr;
    private float _LimitedAlpha = 1.0f;
    public void SetVRMovingFloor(UIStairObjects vRMovingFloor) => _MovingFloor = vRMovingFloor;

    private void Start()
    {
        if (_Button != null)
            _Button.onClick.AddListener(() => Moving().Forget());
        else
        {
           _Pointer.OnPointerClickEvent = new PointerEventDataEvent();
           _Pointer.OnPointerClickEvent.AddListener((PointerEventData eventData) => Moving().Forget());
           _Pointer.OnPointerEnterEvent = new PointerEventDataEvent();
           _Pointer.OnPointerEnterEvent.AddListener(HOVER);
           _Pointer.OnPointerExitEvent = new PointerEventDataEvent();
           _Pointer.OnPointerExitEvent.AddListener(EXIT);
        }
    }
    public async UniTask Moving()
    {
        float alpha = 0.0f;
        float time = 0.0f;
        _MovingFloorView.SetActive(false);
        while (time <= _LimitedAlpha+_LimitedAlpha)
        {
            time += 0.9f * Time.deltaTime;
            if (time <= _LimitedAlpha)
            {
                alpha += 0.9f * Time.deltaTime;
            }
            else
            {
                _Player.transform.position = _Position;
                _Player.GetComponent<PlayerGravity>().Gravity =  _Gravity ? _GravityVolum : Vector3.zero;
                _Player.GetComponent<CharacterController>().enabled = _Gravity ? true : false;
                alpha -= 0.9f * Time.deltaTime;
            }
            _BlackScreen.color = new Color(0, 0, 0, alpha);
            await UniTask.Yield();
        }
        _MovingFloor.GetIItemInformationDirector.SetInformationUIShowing(false);
        _MovingFloor.GetIItemInformationDirector.Laser(true);
        if (InputManager.Instance.InputType == E_INPUT_DEVICE.PC)
            InputManager.Instance.CursorActivate(false, 2);
        _MovingFloor.GetIItemInformationDirector.GetUicusor.SetActive(true);
        _MovingFloor.ScaffoldingOn();
    }
    private void HOVER(PointerEventData eventData)
    {
        GetComponent<Image>().color = Color.gray;
    }
    private void EXIT(PointerEventData eventData)
    {
        GetComponent<Image>().color= Color.white;
    }
}
