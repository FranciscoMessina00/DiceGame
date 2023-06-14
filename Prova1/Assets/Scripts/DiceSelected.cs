using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Color = UnityEngine.Color;

public class DiceSelected : MonoBehaviour
{
    private Camera _mainCamera;
    public GameObject diceObject;
    private new SpriteRenderer renderer;
    private Color colore;
    private PlayerInput input;
    public bool selected = false;
    private void Awake()
    {
        _mainCamera = Camera.main;
        input = new PlayerInput();
    }
    public void Start()
    {
        
        input.Gameplay.Click.started += ctx => StartTouch(ctx);
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
    private void StartTouch(InputAction.CallbackContext context)
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
        renderer = diceObject.GetComponent<SpriteRenderer>();
        Dice dado = diceObject.GetComponent<Dice>();
        colore = renderer.color;
        if (colore.a == 1f)
        {
            colore.a = .5f;
            dado.Deselected();
        } else
        {
            colore.a = 1f;
            dado.Selected();
        }
        diceObject.GetComponent<SpriteRenderer>().color = colore;
    }
}
