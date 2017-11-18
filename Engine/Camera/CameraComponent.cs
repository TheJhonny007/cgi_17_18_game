﻿using OpenTK;

namespace Engine {
	public class CameraComponent : Component.Component {
		private enum PlaneEnum {
			NEAR_PLANE = 0,
			FAR_PLANE = 1,
			LEFT_PLANE = 2,
			RIGHT_PLANE = 3,
			TOP_PLANE = 4,
			BOTTOM_PLANE = 5
		}

		private struct Plane {
			public double D;
			public Vector3d Normal;
		}

		public Vector3d Position { get; private set; }
		public Matrix4d LookAtMatrix { get; private set; }
		private readonly Plane[] planes;

		public CameraComponent(GameObject gameObject) : base(gameObject) => planes = new Plane[6];

		protected void SetLookAt(Vector3d eye, Vector3d target, Vector3d up) {
			LookAtMatrix = Matrix4d.LookAt(eye, target, up);
			Position = eye;

			CreateViewFrustumPlanes(Matrix4d.Identity * LookAtMatrix   /*DisplayCamera.PerspectiveProjection*/);
		}

		private void CreateViewFrustumPlanes(Matrix4d mat) {
			/**
			// by Mohrmann
			// left
			var plane = new Plane {
				Normal = {
					X = mat.M14 + mat.M11,
					Y = mat.M24 + mat.M21,
					Z = mat.M34 + mat.M31
				},
				D = mat.M44 + mat.M41
			};
			planes[(int) PlaneEnum.LEFT_PLANE] = plane;

			// right
			plane = new Plane {
				Normal = {
					X = mat.M14 - mat.M11,
					Y = mat.M24 - mat.M21,
					Z = mat.M34 - mat.M31
				},
				D = mat.M44 - mat.M41
			};
			planes[(int) PlaneEnum.RIGHT_PLANE] = plane;

			// bottom
			plane = new Plane {
				Normal = {
					X = mat.M14 + mat.M12,
					Y = mat.M24 + mat.M22,
					Z = mat.M34 + mat.M32
				},
				D = mat.M44 + mat.M42
			};
			planes[(int) PlaneEnum.BOTTOM_PLANE] = plane;

			// top
			plane = new Plane {
				Normal = {
					X = mat.M14 - mat.M12,
					Y = mat.M24 - mat.M22,
					Z = mat.M34 - mat.M32
				},
				D = mat.M44 - mat.M42
			};
			planes[(int) PlaneEnum.TOP_PLANE] = plane;

			// near
			plane = new Plane {
				Normal = {
					X = mat.M14 + mat.M13,
					Y = mat.M24 + mat.M23,
					Z = mat.M34 + mat.M33
				},
				D = mat.M44 + mat.M43
			};
			planes[(int) PlaneEnum.NEAR_PLANE] = plane;

			// far
			plane = new Plane {
				Normal = {
					X = mat.M14 - mat.M13,
					Y = mat.M24 - mat.M23,
					Z = mat.M34 - mat.M33
				},
				D = mat.M44 - mat.M43
			};
			planes[(int) PlaneEnum.FAR_PLANE] = plane;
			**/
			
			
			// gamedevs.org
			// left
			var plane = new Plane {
				Normal = {
					X = mat.M41 + mat.M11,
					Y = mat.M42 + mat.M12,
					Z = mat.M43 + mat.M13
				},
				D = (mat.M44 + mat.M14)
			};
			planes[(int) PlaneEnum.LEFT_PLANE] = plane;

			// right
			plane = new Plane {
				Normal = {
					X = mat.M41 - mat.M11,
					Y = mat.M42 - mat.M12,
					Z = mat.M43 - mat.M13
				},
				D = (mat.M44 - mat.M14)
			};
			planes[(int) PlaneEnum.RIGHT_PLANE] = plane;

			// bottom
			plane = new Plane {
				Normal = {
					X = mat.M41 + mat.M21,
					Y = mat.M42 + mat.M22,
					Z = mat.M43 + mat.M23
				},
				D = (mat.M44 + mat.M24)
			};
			planes[(int) PlaneEnum.BOTTOM_PLANE] = plane;

			// top
			plane = new Plane {
				Normal = {
					X = mat.M41 - mat.M21,
					Y = mat.M42 - mat.M22,
					Z = mat.M43 - mat.M23
				},
				D = (mat.M44 - mat.M24)
			};
			planes[(int) PlaneEnum.TOP_PLANE] = plane;

			// near
			plane = new Plane {
				Normal = {
					X = mat.M41 + mat.M31,
					Y = mat.M42 + mat.M32,
					Z = mat.M43 + mat.M33
				},
				D = (mat.M44 + mat.M34)
			};
			planes[(int) PlaneEnum.NEAR_PLANE] = plane;

			// far
			plane = new Plane {
				Normal = {
					X = mat.M41 - mat.M31,
					Y = mat.M42 - mat.M32,
					Z = mat.M43 - mat.M33
				},
				D = (mat.M44 - mat.M34)
			};
			planes[(int) PlaneEnum.FAR_PLANE] = plane;
			
			
			
			// normalize
			for (var i = 0; i < 6; i++) {
				plane = planes[i];

				var length = plane.Normal.Length;
				plane.Normal.X = plane.Normal.X / length;
				plane.Normal.Y = plane.Normal.Y / length;
				plane.Normal.Z = plane.Normal.Z / length;
				plane.D = plane.D / length;

				planes[i] = plane;
			}
			
		}

		private double SignedDistanceToPoint(int planeID, Vector3d pt) => Vector3d.Dot(planes[planeID].Normal, pt) + planes[planeID].D;
		public bool IsSphereInFrustum(Vector3d center, double radius) {
			for (var i = 0; i < planes.Length; i++) {
				if (SignedDistanceToPoint(i, center) < -radius) {
					return false;
				}
			}
			return true;
		}

		public Intersect IsAABBInFrustum(AxisAlignedBoundingBox aabb) {
			//var result = Intersect.INSIDE;
			/**for (var i = 0; i < planes.Length; i++) {
				var plane = planes[i];
				
				var nx = plane.Normal.X > 0.0;
				var ny = plane.Normal.Y > 0.0;
				var nz = plane.Normal.Z > 0.0;

				var dot = (plane.Normal.X * aabb.GetMinOrMax(nx).X) +
				          (plane.Normal.Y * aabb.GetMinOrMax(ny).Y) +
				          (plane.Normal.Z * aabb.GetMinOrMax(nz).Z);

				if (dot < -plane.D) result = Intersect.OUTSIDE;

				var dot2 = (plane.Normal.X * aabb.GetMinOrMax(!nx).X) +
				           (plane.Normal.Y * aabb.GetMinOrMax(!ny).Y) +
				           (plane.Normal.Z * aabb.GetMinOrMax(!nz).Z);

				if (dot2 <= -plane.D) result = Intersect.OVERLAP;
			}**/
			// return result

			var result = Intersect.INSIDE;
			for (var i = 0; i < planes.Length; i++) {
				
				if (SignedDistanceToPoint(i, aabb.getVertexP(planes[i].Normal)) > 0) {
					result = Intersect.OUTSIDE;
				}
				if (SignedDistanceToPoint(i, aabb.getVertexN(planes[i].Normal)) > 0) {
					result = Intersect.OVERLAP;
				}
				
			}
			return result;
		}

		public override void Update() { }
	}
}