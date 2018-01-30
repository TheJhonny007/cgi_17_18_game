﻿using System;
using System.Collections.Generic;
using System.Linq;
using Engine;
using Engine.Component;
using Engine.GUI;
using Engine.Material;
using Game.GameObjects;
using Game.GamePlay;
using OpenTK;

namespace Game.Utils {
    public static class LevelGenerator {
        private static readonly Vector3d startingPoint = new Vector3d(0.0, 2000.0, -5500.0);
        private static readonly Quaterniond startOrientation = Quaterniond.FromAxisAngle(Vector3d.UnitY, -1.5);
        
        public static void GenerateLevel0() {
            //sun
            var sun = PlanetFactory.GeneratePlanet(PlanetFactory.PlanetTexture.SUN, new Vector3d(0, 0, 0), new Vector3d(2000.0),
                new Vector3d(0,0.5,0));
            sun.RenderComponent.Material = MaterialManager.GetMaterial(Material.SUN_LAVAFLOW);
            //HUD.AddHudObjectMarker(HUD.CreateHudObjectMarker(sun));
            GameObject.Instantiate(sun);
            //HUD.AddHudObjectMarker(HUD.CreateHudObjectMarker(sun));
            //sun.activateMarker();
            
            Light.SetSpotLight(sun.TransformComponent.Position,
                //           r      g      b      a
                new Vector4(.010f, .010f, .010f, 0f),
                new Vector4(.950f, .950f, .950f, 0f),
                new Vector4(.950f, .950f, .950f, 0f));

			//planets

			List<Planet> planets = new List<Planet>();
			for (var i = 1; i < 3; i++) {
				var planet = PlanetFactory.GeneratePlanetWithAsteroidBeld((PlanetFactory.PlanetTexture) i,
					AsteroidFactory.AsteroidType.STANDARD, 30, new Vector3d(10000.0 * i, 0, 0),
					new Vector3d(1000.0), new Vector3d(0, 0.5, 0), sun);

				GameObject.Instantiate(planet);
				planets.Add(planet);
			}

			planets.First().activateMarker();
			//planets.Last().activateMarker();


            //the player
            GamePlayEngine.ResetSpaceShip();
            GamePlayEngine.spaceship.TransformComponent.Position = startingPoint;
            GamePlayEngine.spaceship.TransformComponent.Orientation = startOrientation;
            GameObject.Instantiate(GamePlayEngine.spaceship);
            
            //finish
            GameObject.Instantiate(new FinishMarker {
                TransformComponent = {
                    Position = startingPoint + new Vector3d(500,-500,500),
                    Scale = new Vector3d(200)
                }
            });

			//black hole
			var blackHole = new BlackHole {
				TransformComponent = {
					Scale = new Vector3d(100),
					Position = startingPoint + new Vector3d(1000.0, 0.0, 0.0),
					Orientation = Quaterniond.Identity
				}
			};

			GameObject.Instantiate(blackHole);

			//metal chunks
			MetalChunkFactory.GenerateLine(startingPoint + new Vector3d(500, 0, 1000), startingPoint + new Vector3d(500, 800, 1000), MetalType.Copper, 10);

			MetalChunkFactory.GenerateRing(startingPoint + new Vector3d(200, 0, 0), new Vector3d(0, 0, 45), MetalType.Gold, 10, 100);

			//MetalChunkFactory.GenerateEye(startingPoint + new Vector3d(0, 200, 0),new Vector3d(45,90,0),MetalType.Silver, 16, 100.0, MetalType.Gold);
		}


        //############################################# LEVEL 1 #############################################
        public static void GenerateLevel1() {
            //sun
            GameObject.Instantiate(PlanetFactory.GeneratePlanet(PlanetFactory.PlanetTexture.SUN, new Vector3d(11000,0,0), new Vector3d(2000.0),
                new Vector3d(0,0.5,0)));
            
            Light.SetSpotLight(new Vector3d(-500,0,0), 
                //           r      g      b      a
                new Vector4(.010f, .010f, .010f, 0f),
                new Vector4(.950f, .950f, .950f, 0f),
                new Vector4(.950f, .950f, .950f, 0f));
            
            //the player
            GamePlayEngine.ResetSpaceShip();
            GameObject.Instantiate(GamePlayEngine.spaceship);

            MetalChunkFactory.GenerateOnPositions(new Dictionary<Vector3d, MetalType> {
                {new Vector3d(200,0,0),MetalType.Copper},
                {new Vector3d(350,0,50),MetalType.Copper},
                {new Vector3d(500,0,0),MetalType.Copper},
                {new Vector3d(650,0,-50),MetalType.Copper}
            });
            MetalChunkFactory.GenerateLine(new Vector3d(800, 0, 0), new Vector3d(1200, 0, 0), MetalType.Copper, 5);
            MetalChunkFactory.GenerateLine(new Vector3d(1400, 0, 100), new Vector3d(1800, 0, 100), MetalType.Copper, 5);
            MetalChunkFactory.GenerateLine(new Vector3d(2000, 0, -100), new Vector3d(2500, 0, -100), MetalType.Copper, 5);
            MetalChunkFactory.GenerateLine(new Vector3d(2700, 100, 0), new Vector3d(3500, 100, 0), MetalType.Copper, 5);
            MetalChunkFactory.GenerateSingle(new Vector3d(3600, 0, 0), MetalType.Copper);
            MetalChunkFactory.GenerateLine(new Vector3d(3700, -100, 0), new Vector3d(4500, -100, 0), MetalType.Copper, 5);
            MetalChunkFactory.GenerateLine(new Vector3d(4700, 0, 100), new Vector3d(5200, 0, -100), MetalType.Copper, 15);
            MetalChunkFactory.GenerateLine(new Vector3d(5400, 0, -100), new Vector3d(5900, 0, 100), MetalType.Copper, 15);
            MetalChunkFactory.GenerateLine(new Vector3d(6100, 100, 0), new Vector3d(6600, -100, 0), MetalType.Copper, 15);
            MetalChunkFactory.GenerateLine(new Vector3d(6800, -100, 0), new Vector3d(7300, 100, 0), MetalType.Copper, 15);

            //finish
            GameObject.Instantiate(new FinishMarker {
                TransformComponent = {
                    Position = new Vector3d(7500,0,0),
                    Scale = new Vector3d(200)
                }
            });
        }
        
        //############################################# LEVEL 2 #############################################
        public static void GenerateLevel2() {
            //sun
            GameObject.Instantiate(PlanetFactory.GeneratePlanet(PlanetFactory.PlanetTexture.SUN, new Vector3d(15000,0,0), new Vector3d(2000.0),
                new Vector3d(0,0.5,0)));
            
            Light.SetSpotLight(new Vector3d(-500,0,0),
                //           r      g      b      a
                new Vector4(.010f, .010f, .010f, 0f),
                new Vector4(.950f, .950f, .950f, 0f),
                new Vector4(.950f, .950f, .950f, 0f));
            
            //the player
            GamePlayEngine.ResetSpaceShip();
            GameObject.Instantiate(GamePlayEngine.spaceship);

            MetalChunkFactory.GenerateSingle(new Vector3d(200,0,0),MetalType.Copper );
            MetalChunkFactory.GenerateSingle(new Vector3d(350,0,50),MetalType.Copper );
            MetalChunkFactory.GenerateSingle(new Vector3d(500,0,0),MetalType.Copper );
            MetalChunkFactory.GenerateSingle(new Vector3d(650,0,-50),MetalType.Copper );
            
            MetalChunkFactory.GenerateLine(new Vector3d(800, 0, 0), new Vector3d(1200, 0, 0), MetalType.Copper, 5);
            
            MetalChunkFactory.GenerateRing(new Vector3d(1500, -200, 0), Vector3d.Zero, MetalType.Copper, 25, 200);
            
            MetalChunkFactory.GenerateLine(new Vector3d(1600, 0, 0), new Vector3d(2500, 0, 0), MetalType.Copper, 25);
            MetalChunkFactory.GenerateLine(new Vector3d(2700, 0, 0), new Vector3d(3500, 100, 0), MetalType.Copper, 20);
            MetalChunkFactory.GenerateLine(new Vector3d(3600, 100, 0), new Vector3d(4400, 0, 100), MetalType.Copper, 20);
            MetalChunkFactory.GenerateLine(new Vector3d(4500, 0, 100), new Vector3d(5300, -100, 0), MetalType.Copper, 20);
            MetalChunkFactory.GenerateLine(new Vector3d(5400, -100, 0), new Vector3d(6100, 0, -100), MetalType.Copper, 20);
            MetalChunkFactory.GenerateLine(new Vector3d(6200, 0, -100), new Vector3d(6600, 100, 0), MetalType.Copper, 10);
            MetalChunkFactory.GenerateLine(new Vector3d(6700, 100, 0), new Vector3d(7100, 100, 0), MetalType.Copper, 20);
            MetalChunkFactory.GenerateLine(new Vector3d(7200, 100, 0), new Vector3d(8000, 0, 0), MetalType.Copper, 20);
            MetalChunkFactory.GenerateLine(new Vector3d(8100, 0, 0), new Vector3d(8700, 0, 0), MetalType.Copper, 20);
            
            MetalChunkFactory.GenerateRing(new Vector3d(9000, 300, 300), new Vector3d(0,0,90), MetalType.Copper, 25, 300);
            
            MetalChunkFactory.GenerateLine(new Vector3d(9300, 0, 0), new Vector3d(9500, 0, 0), MetalType.Copper, 4);
            
            MetalChunkFactory.GenerateRing(new Vector3d(9700, 0, 0), new Vector3d(0,90,0), MetalType.Copper, 5, 18);
            MetalChunkFactory.GenerateRing(new Vector3d(9750, 0, 0), new Vector3d(0,90,0), MetalType.Copper, 5, 18);
            MetalChunkFactory.GenerateRing(new Vector3d(9800, 0, 0), new Vector3d(0,90,0), MetalType.Copper, 5, 18);
            MetalChunkFactory.GenerateRing(new Vector3d(9850, 0, 0), new Vector3d(0,90,0), MetalType.Copper, 5, 18);
            MetalChunkFactory.GenerateRing(new Vector3d(9900, 0, 0), new Vector3d(0,90,0), MetalType.Copper, 5, 18);

            //finish
            GameObject.Instantiate(new FinishMarker {
                TransformComponent = {
                    Position = new Vector3d(10500,0,0),
                    Scale = new Vector3d(200)
                }
            });
        }
        
        //############################################# LEVEL 3 #############################################
        public static void GenerateLevel3() {
            //sun
            GameObject.Instantiate(PlanetFactory.GeneratePlanet(PlanetFactory.PlanetTexture.SUN, new Vector3d(11000,0,0), new Vector3d(2000.0),
                new Vector3d(0,0.5,0)));
            
            Light.SetSpotLight(new Vector3d(-500,0,0),
                //           r      g      b      a
                new Vector4(.010f, .010f, .010f, 0f),
                new Vector4(.950f, .950f, .950f, 0f),
                new Vector4(.950f, .950f, .950f, 0f));
            
            //the player
            GamePlayEngine.ResetSpaceShip();
            GameObject.Instantiate(GamePlayEngine.spaceship);
            
            
            //finish
            GameObject.Instantiate(new FinishMarker {
                TransformComponent = {
                    Position = new Vector3d(7500,0,0),
                    Scale = new Vector3d(200)
                }
            });
        }
        
        //############################################# LEVEL 4 #############################################
        public static void GenerateLevel4() {
            //sun
            GameObject.Instantiate(PlanetFactory.GeneratePlanet(PlanetFactory.PlanetTexture.SUN, new Vector3d(11000,0,0), new Vector3d(2000.0),
                new Vector3d(0,0.5,0)));
            
            Light.SetSpotLight(new Vector3d(-500,0,0),
                //           r      g      b      a
                new Vector4(.010f, .010f, .010f, 0f),
                new Vector4(.950f, .950f, .950f, 0f),
                new Vector4(.950f, .950f, .950f, 0f));
            
            //the player
            GamePlayEngine.ResetSpaceShip();
            GameObject.Instantiate(GamePlayEngine.spaceship);
            
            
            //finish
            GameObject.Instantiate(new FinishMarker {
                TransformComponent = {
                    Position = new Vector3d(7500,0,0),
                    Scale = new Vector3d(200)
                }
            });
        }
        
        //############################################# LEVEL 5 #############################################
        public static void GenerateLevel5() {
            //sun
            GameObject.Instantiate(PlanetFactory.GeneratePlanet(PlanetFactory.PlanetTexture.SUN, new Vector3d(11000,0,0), new Vector3d(2000.0),
                new Vector3d(0,0.5,0)));
            
            Light.SetSpotLight(new Vector3d(-500,0,0),
                //           r      g      b      a
                new Vector4(.010f, .010f, .010f, 0f),
                new Vector4(.950f, .950f, .950f, 0f),
                new Vector4(.950f, .950f, .950f, 0f));
            
            //the player
            GamePlayEngine.ResetSpaceShip();
            GameObject.Instantiate(GamePlayEngine.spaceship);
            
            
            //finish
            GameObject.Instantiate(new FinishMarker {
                TransformComponent = {
                    Position = new Vector3d(7500,0,0),
                    Scale = new Vector3d(200)
                }
            });
        }
        
        //############################################# LEVEL 6 #############################################
        public static void GenerateLevel6() {
            //sun
            GameObject.Instantiate(PlanetFactory.GeneratePlanet(PlanetFactory.PlanetTexture.SUN, new Vector3d(11000,0,0), new Vector3d(2000.0),
                new Vector3d(0,0.5,0)));
            
            Light.SetSpotLight(new Vector3d(-500,0,0),
                //           r      g      b      a
                new Vector4(.010f, .010f, .010f, 0f),
                new Vector4(.950f, .950f, .950f, 0f),
                new Vector4(.950f, .950f, .950f, 0f));
            
            //the player
            GamePlayEngine.ResetSpaceShip();
            GameObject.Instantiate(GamePlayEngine.spaceship);
            
            
            //finish
            GameObject.Instantiate(new FinishMarker {
                TransformComponent = {
                    Position = new Vector3d(7500,0,0),
                    Scale = new Vector3d(200)
                }
            });
        }
    }
}