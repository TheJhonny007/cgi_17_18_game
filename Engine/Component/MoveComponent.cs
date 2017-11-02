﻿using OpenTK;
using OpenTK.Input;

namespace Engine {
	public class MoveComponent : Component {
		public Vector3 LinearVelocity = Vector3.Zero;
		public Vector3 AngularVelocity = Vector3.Zero;

		public MoveComponent(GameObject gameObject) : base(gameObject) { }

		public override void Update(double deltaTime) {
			ApplyLinearVelocity(deltaTime);
			ApplyAngularVelocity(deltaTime);
		}

		private void ApplyLinearVelocity(double deltaTime) {
			GameObject.TransformComponent.Position += LinearVelocity * (float) deltaTime;
		}

		private void ApplyAngularVelocity(double deltaTime) {
			var angularChange = AngularVelocity * (float) deltaTime;
			var rotationAxis = angularChange == Vector3.Zero ? Vector3.One : angularChange.Normalized();
			var rotationAngle = angularChange.Length;
			GameObject.TransformComponent.Orientation = Quaternion.FromAxisAngle(rotationAxis, rotationAngle) * GameObject.TransformComponent.Orientation;
		}
	}
}