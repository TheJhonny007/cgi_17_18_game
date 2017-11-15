﻿using Engine;
using Engine.Material;
using Engine.Model;
using Engine.Component;
using Game.Components;

namespace Game.GameObjects {
	public class Planet : GameObject {
		public readonly RenderComponent RenderComponent;
		public readonly MoveComponent MoveComponent;
		public readonly CollisionComponent CollisionComponent;

		public Planet(int textureId, GameObject referenceObject = null) {
			RenderComponent = new RenderComponent(
				ModelLoaderObject3D.Load("data/objects/Planet.obj", this),
				MaterialManager.GetMaterial(Material.AmbientDiffuseSpecular),
				textureId,
				this
			);
			if (referenceObject != null) {
				MoveComponent = new GravityMovement(this, 0.0);
			}
			else {
				MoveComponent = new MoveComponent(this);
			}
			
			CollisionComponent = new SphereCollider(this, RenderComponent.Model,
				
				collision => {
					System.Console.WriteLine(ToString()+" collided with "+collision.gameObject.ToString());
				});
			CollisionComponent.Register();
		}

		public override void Awake() {
			base.Awake();
			
			Radius = RenderComponent.Model.GetRadius();
			TransformComponent.UpdateWorldMatrix();
		}

		public override void Update() {
			MoveComponent.Update();
			base.Update();
			RenderComponent.Update();
		}

		public override void Draw() {
			base.Draw();
			RenderComponent.Draw();
		}
	}
}