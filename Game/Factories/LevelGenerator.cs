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
        
        public static void GenerateLevel1() {
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
            var ship = new SpaceShip {
                TransformComponent = {
                    Scale = new Vector3d(0.02f),
                    Position = startingPoint,
                    Orientation = startOrientation
                }
            };
            GameObject.Instantiate(ship);

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
        

        public static void GenerateLevel1b() {
            //sun
            var sun = PlanetFactory.GeneratePlanet(PlanetFactory.PlanetTexture.SUN, new Vector3d(5000,0,0), new Vector3d(2000.0),
                new Vector3d(0,0.5,0));
            GameObject.Instantiate(sun);
            
            Light.SetSpotLight(sun.TransformComponent.Position,
                //           r      g      b      a
                new Vector4(.010f, .010f, .010f, 0f),
                new Vector4(.950f, .950f, .950f, 0f),
                new Vector4(.950f, .950f, .950f, 0f));
            
            //the player
            GamePlayEngine.spaceship.TransformComponent.Position = Vector3d.Zero;
            GamePlayEngine.spaceship.TransformComponent.Orientation = Quaterniond.Identity;
            GameObject.Instantiate(GamePlayEngine.spaceship);
            
            GameObject.Instantiate(new FinishMarker() {
                TransformComponent = {
                    Position = new Vector3d(2000,0,0),
                    Scale = new Vector3d(200)
                }
            });
        }
        
        public static void GenerateLevel2() {
            //sun
            var sun = PlanetFactory.GeneratePlanet(PlanetFactory.PlanetTexture.SUN, new Vector3d(5000,0,0), new Vector3d(2000.0),
                new Vector3d(0,0.5,0));
            GameObject.Instantiate(sun);
            
            Light.SetSpotLight(sun.TransformComponent.Position,
                //           r      g      b      a
                new Vector4(.010f, .010f, .010f, 0f),
                new Vector4(.950f, .950f, .950f, 0f),
                new Vector4(.950f, .950f, .950f, 0f));
            
            //the player
            GameObject.Instantiate(new SpaceShip {
                TransformComponent = {
                    Scale = new Vector3d(0.02f),
                    Position = new Vector3d(0,0,500),
                    Orientation = Quaterniond.Identity
                }
            });
            
            //Finish
            GameObject.Instantiate(new FinishMarker() {
                TransformComponent = {
                    Position = new Vector3d(2000,0,0),
                    Scale = new Vector3d(200)
                }
            });
        }
        
        public static void GenerateLevel3() {
            //the player
            GameObject.Instantiate(new SpaceShip {
                TransformComponent = {
                    Scale = new Vector3d(0.02f),
                    Position = new Vector3d(0,0,500),
                    Orientation = Quaterniond.Identity
                }
            });
        }
        
        public static void GenerateLevel4() {
            
        }
        
        public static void GenerateLevel5() {
            
        }
        
        public static void GenerateLevel6() {
            
        }
    }
}