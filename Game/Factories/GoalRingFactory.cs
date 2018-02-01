﻿using System;
using System.Collections.Generic;
using Engine;
using Game.GameObjects;
using Game.GamePlay;
using OpenTK;

namespace Game.Utils {
	public class GoalRingFactory {
		
		public static GoalRing GenerateSingle(Vector3d position, double scale = 5.0) {
			var chunk = new GoalRing() {
				TransformComponent = {
					Scale = new Vector3d(scale),
					Position = position,
					Orientation = Quaterniond.FromAxisAngle(Vector3d.UnitX, Math.PI/2)
                    
				},
				moveComponent = { AngularVelocity = new Vector3d(0,3.0,0) }
                
			};
            
       		GamePlayEngine.RegisterGoalRing(chunk);
			chunk.Instantiate();
			
			return chunk;
		}
		
		
		public static List<GameObject> GenerateGoalRingWithAsteroidRing(Vector3d center, double pointRingScale, Matrix4d rotationMat) {
			var objs = new List<GameObject>();

			var checkPoint = GenerateSingle(center, pointRingScale);
			objs.Add(checkPoint);
			var ring_radius = pointRingScale *2;
			var asteroid_radius = pointRingScale * 0.5;
			var asteroidRing =
				AsteroidFactory.GenerateAsteroidRingForCheckpoint(checkPoint, ring_radius, asteroid_radius, rotationMat);
			objs.AddRange(asteroidRing);
			
            
           

			return objs;
		}
		
		
	}
}