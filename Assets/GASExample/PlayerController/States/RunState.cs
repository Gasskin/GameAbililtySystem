using System;
using Animancer;
using UnityEngine;

public class RunState : PlayerState
{
    public MixerTransition2DAsset.UnShared transition;
    public float smoothSpeed = 0.2f;
    private Vector2 currentMoveInput;
    private Vector2 smoothVelocity;

    private void Update()
    {
        currentMoveInput = Vector2.SmoothDamp(currentMoveInput, controller.moveInput, ref smoothVelocity, smoothSpeed);
        currentMoveInput.x = Mathf.Clamp(currentMoveInput.x, -1, 1);
        currentMoveInput.y = Mathf.Clamp(currentMoveInput.y, -1, 1);
        transition.State.Parameter = currentMoveInput;
    }

    private void OnEnable()
    {
        controller.animancer.Play(transition);
    }

    private void OnDisable()
    {
        currentMoveInput = Vector2.zero;
        transition.State.Parameter = currentMoveInput;
    }
}