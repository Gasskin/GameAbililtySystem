﻿using UnityEngine;

namespace GASExample
{
    public class PlayerInput
    {
        public bool Attack { get; private set; } = false;
        public bool Skill1 { get; private set; } = false;
        public bool SKill2 { get; private set; } = false;
        public bool Skill3 { get; private set; } = false;
        public bool Skill4 { get; private set; } = false;
        
        public bool Roll { get; private set; } = false;
        
        public Vector2 MoveInput { get; private set; } = Vector2.zero;
        
        public void ResetInput()
        {
            Attack = false;
            Skill1 = false;
            SKill2 = false;
            Skill3 = false;
            Skill4 = false;
            Roll = false;

            MoveInput = Vector2.zero;
        }
        
        public void GetInput()
        {
            Attack = Input.GetMouseButtonUp(0);
            Skill1 = Input.GetKeyDown(KeyCode.Alpha1);
            SKill2 = Input.GetKeyDown(KeyCode.Alpha2);
            Skill3 = Input.GetKeyDown(KeyCode.Alpha3);
            Skill4 = Input.GetKeyDown(KeyCode.Alpha4);
            Roll = Input.GetKeyDown(KeyCode.LeftShift);
            MoveInput = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        }
    }
}