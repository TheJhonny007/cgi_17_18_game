﻿using Engine.Material;
using Engine.Model;
using Engine.Render;

namespace Engine.Component {
	public class RenderComponent : Component, IOctreeItem<RenderComponent> {
		private static int idCounter;
		private readonly int id = idCounter++;

		public readonly Model3D Model;
		public AxisAlignedBoundingBox AABB;
		public BaseMaterial Material;
		public MaterialSettings MaterialSettings;

		public RenderComponent(Model3D model, BaseMaterial material, MaterialSettings materialSettings, GameObject gameObject) : base(gameObject) {
			Model = model;
			AABB = Model.AABB * gameObject.TransformComponent.Scale;
			Material = material;

			MaterialSettings = materialSettings;
		}

		public override void Update() { }

		public AxisAlignedBoundingBox GetAABB() => AABB + GameObject.TransformComponent.WorldPosition;
		public Sphere GetBoundingSphere() => new Sphere(GameObject.TransformComponent.WorldPosition, GameObject.Radius);

		public override int GetHashCode() => id;
	}
}