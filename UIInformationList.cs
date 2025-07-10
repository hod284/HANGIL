using BNG;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using VSTS;

public class UIInformationList : MonoBehaviour
{
    private ItemInformationsGroup _ItemDictionary= new ItemInformationsGroup();
    private List<VSTS_GroupInfo> _ItemDictionary_Value = new List<VSTS_GroupInfo>();
    [SerializeField] private UIItemInformationDirector _Director; 
    [SerializeField] private UnityEngine.UI.Button _ItemLabel;
    [SerializeField] private Transform _Parent;
    [SerializeField] private GameObject _View;
    [SerializeField] private GameObject _Pannel;
    [SerializeField] private bool _IsVR=false;
    private List<GameObject> _Showingob = new List<GameObject>();
    float _LastPress = 0f;
    float _Delay = 0.2f;
    public List<VSTS_GroupInfo> GetItemDictionary_Value { get => _ItemDictionary_Value; }
    public List<GameObject> GetShowingob { get => _Showingob; }
    public UIItemInformationDirector GetDirector { get => _Director; }
    private void Start()
    {
        if (InputManager.Instance.InputType == E_INPUT_DEVICE.VR &&!_IsVR)
            gameObject.SetActive(false);
        if (InputManager.Instance.InputType == E_INPUT_DEVICE.PC && _IsVR)
            gameObject.SetActive(false);
        if (_Director == null)
            _Director = GameObject.FindObjectOfType<UIItemInformationDirector>();
        _ItemDictionary = _Director.GetItemInformationsGroup;
        _ItemDictionary_Value = _ItemDictionary.Values.ToList();
        _ItemDictionary_Value.RemoveAll(x => x.displaymeshes_OB.Length ==0);
        var list = _ItemDictionary_Value;
        for (int i = 0; i < list.Count; i++)
        { 
            var button = Instantiate(_ItemLabel, _Parent);
            button.gameObject.SetActive(true);
            var text =   button.GetComponentsInChildren<Text>();
            text[0].text = list[i].NameDesc;
            text[1].text = list[i].EquipDesc;
            button.gameObject.name = list[i ].idx.ToString();
        }
        var Player = GameObject.FindObjectOfType<CharacterController>();
    }
    public void Run()
    {
        _View.gameObject.SetActive(!_View.gameObject.activeSelf);
        if (InputManager.Instance.InputType == E_INPUT_DEVICE.VR)
        {
            if (_View.gameObject.activeSelf)
            {
                _Director.Laser(false);
                _Director.OffBoxColider();
            }
            else
            {
                _Director.Laser(true);
                _Director.OnBoxColider();
            }
        }
        if (_Pannel.gameObject.activeSelf)
            _Pannel.gameObject.SetActive(false);
        if (InputManager.Instance.InputType == E_INPUT_DEVICE.PC)
        {
            InputManager.Instance.CursorActivate(_View.gameObject.activeSelf, 2);
            if (_View.gameObject.activeSelf)
                _Director.OffBoxColider();
            else
                _Director.OnBoxColider();
        }
    }
    private void LateUpdate ()
    {
        if (InputManager.Instance.InputType == E_INPUT_DEVICE.VR && InputBridge.Instance.BButtonDown|| InputManager.Instance.InputType == E_INPUT_DEVICE.PC && Input.GetKeyDown(KeyCode.Tab))
        {
            if (Time.time - _LastPress > _Delay)
            {
                _LastPress = Time.time;

                Debug.Log("한 번만 눌림");
                if(!_Director.GetinformationUIShowing)
                Run();
            }
        }
        
    }
}
