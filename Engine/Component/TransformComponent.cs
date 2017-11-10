﻿using System;
using System.Collections.Generic;
using OpenTK;

namespace Engine.Component {
	public class TransformComponent : Component {
		public Matrix4d WorldMatrix = Matrix4d.Identity;
		public Matrix4d LocalMatrix = Matrix4d.Identity;

		public Vector3d Position;
		public Quaterniond Orientation;
		public Vector3d Scale;

		public Vector3d WorldPosition => WorldMatrix.ExtractTranslation();
		public Quaterniond WorldOrientation => WorldMatrix.ExtractRotation();

		public TransformComponent(Vector3d position, Quaterniond orientation, Vector3d scale, GameObject owner) : base(owner) {
			Position = position;
			Orientation = orientation;
			Scale = scale;
			
			Children = new List<TransformComponent>();
		}
		
		public TransformComponent(GameObject owner) : this(Vector3d.Zero, owner) { }

		public TransformComponent(Vector3d position, GameObject owner) : this(position, Quaterniond.Identity,
			Vector3d.One, owner) { }

		public TransformComponent(Quaterniond orientation, GameObject owner) : this(Vector3d.Zero, orientation,
			Vector3d.One, owner) { }

		public TransformComponent(TransformComponent other) : this(other.Position, other.Orientation, other.Scale,
			other.GameObject) { }

		public TransformComponent(Matrix4d transformMatrix, GameObject owner ) : this(
			transformMatrix.ExtractTranslation(),
			transformMatrix.ExtractRotation(), transformMatrix.ExtractScale(), owner) { }

		private TransformComponent _parent;
		public List<TransformComponent> Children;


		public TransformComponent Parent {
			get => _parent;
			set {
				var oldParent = Parent;
				if (oldParent == value) return;

				oldParent?.Children.Remove(this);
				value?.Children.Add(this);

				_parent = value;
			}
		}

		public void UpdateLocalMatrix() {
			LocalMatrix = Matrix4d.Scale(Scale) * Matrix4d.CreateFromQuaternion(Orientation) *
			              Matrix4d.CreateTranslation(Position);
		}

		public void UpdateWorldMatrix() {
			UpdateLocalMatrix();
			if (Parent == null) {
				WorldMatrix = LocalMatrix;
			}
			else {
				Parent.UpdateWorldMatrix();
				WorldMatrix = Parent.WorldMatrix.Inverted().ClearScale() * LocalMatrix;
			}
		}

		public override void Update() {
			UpdateWorldMatrix();
		}
	}
}