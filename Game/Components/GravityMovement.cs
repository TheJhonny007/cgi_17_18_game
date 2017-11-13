﻿﻿using System;
using Engine;
using Engine.Util;
using OpenTK;

namespace Game.Components {
    public class GravityMovement : MoveComponent {
        public double Distance = 2000;
        public double Speed = 1.0 / 180.0;
        public double _currentAngle;

        public GravityMovement(GameObject gameObject, double currentAngle) : base(gameObject) {
            LinearVelocity = Vector3d.Zero;
            _currentAngle = currentAngle;
        }

        public override void Update() {
            ComputeGravitationalMovement();
            base.Update();
        }

        private void ComputeGravitationalMovement() {
            GameObject.TransformComponent.Position.X = Math.Cos(_currentAngle) * Distance;
            GameObject.TransformComponent.Position.Z = Math.Sin(_currentAngle) * Distance;

            _currentAngle += Speed * Time.DeltaTimeUpdate;
    
            if(_currentAngle > 2 * Math.PI)
                _currentAngle -= 2 * Math.PI;
        }
    }
}