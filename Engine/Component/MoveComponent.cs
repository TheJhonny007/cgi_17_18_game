﻿using Engine.Render;
using OpenTK;

namespace Engine {
	public class MoveComponent : Component.Component {
		public Vector3d LinearVelocity = Vector3d.Zero;
		public Vector3d AngularVelocity = Vector3d.Zero;

		public MoveComponent(GameObject gameObject) : base(gameObject) { }

		public override void Update() {
			ApplyLinearVelocity();
			ApplyAngularVelocity();
		}

		private void ApplyLinearVelocity() => GameObject.TransformComponent.Position += LinearVelocity * Time.DeltaTimeUpdate;

		private void ApplyAngularVelocity() {
			var angularChange = AngularVelocity * Time.DeltaTimeUpdate;
			var rotationAxis = angularChange == Vector3d.Zero ? Vector3d.One : angularChange.Normalized();
			var rotationAngle = angularChange.Length;
			GameObject.TransformComponent.Orientation = Quaterniond.FromAxisAngle(rotationAxis, rotationAngle) *
			                                            GameObject.TransformComponent.Orientation;
		}
	}
}