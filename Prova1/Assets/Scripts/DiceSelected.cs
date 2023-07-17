using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Color = UnityEngine.Color;

public class DiceSelected : MonoBehaviour
{
    private Camera _mainCamera;
    public GameObject diceObject;
    private SpriteRenderer m_renderer;
    private Color colore;
    private PlayerInput input;
    public GameObject[] dadiArray;
    [SerializeField] private GameObject lanciaBotton;
    UnityEngine.UI.Button bottoneLanciare;
    private void Awake()
    {
        _mainCamera = Camera.main;
        input = new PlayerInput();
        bottoneLanciare = lanciaBotton.GetComponent<UnityEngine.UI.Button>();
    }
    public void Start()
    {
        _mainCamera = Camera.main;
        // input.Gameplay.Click.started += ctx => StartTouch(ctx);
    }
    //public void OnClick(InputAction.CallbackContext context)
    //{
    //    if (!context.started) return;
    //    var rayHit = Physics2D.GetRayIntersection(_mainCamera.ScreenPointToRay(Mouse.current.position.ReadValue()));
    //    if (!rayHit.collider) return;
    //    diceObject = rayHit.collider.gameObject;
    //    SwitchState();
    //    //Dice dice = diceObject.GetComponent<Dice>();
    //    //print(dice.face);
    //}
    private void OnEnable()
    {
        input.Enable();
    }
    private void OnDisable()
    {
        input.Disable();
    }
    public void OnClick(InputAction.CallbackContext context)
    {
        if (!context.started) return;
        Vector3 v3 = input.Gameplay.ClickPosition.ReadValue<Vector2>();
        var rayHit = Physics2D.GetRayIntersection(_mainCamera.ScreenPointToRay(v3));
        if (!rayHit.collider) return;
        diceObject = rayHit.collider.gameObject;
        SwitchState();
    }
    private void SwitchState()
    {
        m_renderer = diceObject.GetComponent<SpriteRenderer>();
        Dice dado = diceObject.GetComponent<Dice>();
        colore = m_renderer.color;
        if (colore.a == 1f)
        {
            colore.a = .5f;
            dado.IsSelected(false);
        } else
        {
            colore.a = 1f;
            dado.IsSelected(true);
        }
        diceObject.GetComponent<SpriteRenderer>().color = colore;
        ControlloBottoneLancia();
    }
    public void ControlloBottoneLancia()
    {
        bool abilitaLancia = false;
        foreach (GameObject dadoObj in dadiArray)
        {
            Dice dado = dadoObj.GetComponent<Dice>();
            if (dado.selected) { abilitaLancia = true; }
        }    
        bottoneLanciare.interactable = abilitaLancia;
    }
}
