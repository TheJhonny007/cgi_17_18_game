﻿using System;
using System.Collections.Generic;
using Engine.Render;
using Game.GameObjects;
using OpenTK;

namespace Game.Utils {
	/// <summary>
	/// A Factory that can generate different configurations of metal chunks.
	/// </summary>
	public static class PointRingFactory {
		/// <summary>
		/// Generates a single metal chunk on a chosen location with specific type and scale.
		/// </summary>
		/// <param name="position"></param>
		/// <param name="type"></param>
		/// <param name="scale"></param>
		/// <returns></returns>
		public static PointRing GenerateSingle(Vector3d position, PointType type, double scale = 5.0) {
			var chunk = new PointRing(type) {
				TransformComponent = {
					Scale = new Vector3d(scale),
					Position = position,
					Orientation = Quaterniond.FromAxisAngle(Vector3d.UnitX, Math.PI / 2)
				},
				MoveComponent = {AngularVelocity = new Vector3d(0, 3.0, 0)}
			};

			chunk.Instantiate();
			return chunk;
		}

		/// <summary>
		/// Generates a line of MetalChunks from a start position to an end position.
		/// </summary>
		/// <param name="startPosition"></param>
		/// <param name="endPosition"></param>
		/// <param name="type"></param>
		/// <param name="count"></param>
		/// <param name="scale"></param>
		/// <returns>
		/// Returns a list of the generated metal chunks.
		/// The returned list is empty if count is 0 oder less.
		/// </returns>
		public static List<Ring> GenerateLine(Vector3d startPosition, Vector3d endPosition, PointType type, int count, double scale = 5.0) {
			var chunks = new List<Ring>();
			if (count < 1) return chunks;

			var positions = new List<Vector3d>();

			if (count == 1) {
				positions.Add(Math3D.InMiddleOf(startPosition, endPosition));
			}
			else {
				positions.Add(startPosition);

				if (count > 2) {
					var equiDistance = (endPosition - startPosition) / (count - 1);
					for (var i = 1; i < count - 1; i++) {
						positions.Add(startPosition + (equiDistance * i));
					}
				}

				positions.Add(endPosition);
			}

			foreach (var pos in positions) {
				chunks.Add(GenerateSingle(pos, type, scale));
			}

			return chunks;
		}

		/// <summary>
		/// Generates a ring of metal chunks.
		/// The angle is measured in degrees!
		/// </summary>
		/// <param name="center"></param>
		/// <param name="eulerAngle"></param>
		/// <param name="type"></param>
		/// <param name="count"></param>
		/// <param name="radius"></param>
		/// <param name="scale"></param>
		/// <returns>
		/// Returns a list of the generated metal chunks.
		/// The returned list is empty if count is 0 oder less.
		/// </returns>
		private static List<Ring> GenerateRing(Vector3d center, Vector3d eulerAngle, PointType type, int count, double radius, double scale = 5.0) {
			var chunks = new List<Ring>();
			if (count < 1) return chunks;

			for (var i = 0; i < count; i++) {
				var pos = new Vector3d(radius * Math.Cos(i * Math.PI * 2 / count), radius * Math.Sin(i * Math.PI * 2 / count), center.Z);
				var rotatedPos = Quaterniond.FromEulerAngles(eulerAngle.ToRadiansVector3D()).Rotate(pos) + center;
				chunks.Add(GenerateSingle(rotatedPos, type, scale));
			}

			return chunks;
		}


		/// <summary>
		/// Generates a ring of metal chunks with a chunk in its center.
		/// The angle is measured in degrees!
		/// </summary>
		/// <param name="center"></param>
		/// <param name="eulerAngle"></param>
		/// <param name="ringType"></param>
		/// <param name="ringCount"></param>
		/// <param name="radius"></param>
		/// <param name="eyeType"></param>
		/// <param name="ringScale"></param>
		/// <param name="eyeScale"></param>
		/// <returns>
		/// Returns a list of the generated metal chunks.
		/// The returned list is empty if count is 0 oder less.
		/// </returns>
		public static List<Ring> GenerateEye(Vector3d center, Vector3d eulerAngle, PointType ringType, int ringCount, double radius, PointType eyeType, double ringScale = 5.0, double eyeScale = 5.0) {
			var chunks = new List<Ring>();

			chunks.AddRange(GenerateRing(center, eulerAngle, ringType, ringCount, radius, ringScale));
			chunks.Add(GenerateSingle(center, eyeType, eyeScale));

			return chunks;
		}

		/// <summary>
		/// Generates metal chunks based on the position-MetalType pairs of the dictionary.
		/// </summary>
		/// <param name="positionsAndTypes"></param>
		/// <param name="scale"></param>
		/// <returns>
		/// Returns a list of the generated metal chunks.
		/// The returned list is empty if count is 0 oder less.
		/// </returns>
		public static List<Ring> GenerateOnPositions(Dictionary<Vector3d, PointType> positionsAndTypes, double scale = 5.0) {
			var chunks = new List<Ring>();
			if (positionsAndTypes.Count < 1) return chunks;

			foreach (var positionAndType in positionsAndTypes) {
				chunks.Add(GenerateSingle(positionAndType.Key, positionAndType.Value, scale));
			}

			return chunks;
		}
	}
}