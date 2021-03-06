﻿namespace Boxygen.Math {
	public class Transform {
		public static Transform Identity => new Transform();

		public Matrix4 Matrix;

		public Transform() {
			Matrix = Matrix4.Identity;
		}

		public Transform(Matrix4 matrix) {
			Matrix = matrix;
		}

		public Transform(Transform copy) {
			Matrix = new Matrix4(copy.Matrix);
		}

		public void Apply(Transform other) {
			Apply(other.Matrix);
		}

		public void Apply(Matrix4 matrix) {
			Matrix *= matrix;
		}

		public void Translate(Vec3 offset) {
			Apply(new Matrix4(offset));
		}

		public void Rotate(Vec3 axis, double angle) {
			double n1 = axis.X, n2 = axis.Y, n3 = axis.Z;
			double cos = System.Math.Cos(angle * System.Math.PI / 180);
			double sin = System.Math.Sin(angle * System.Math.PI / 180);

			Apply(new Matrix4(new[] {
				n1*n1*(1-cos)+cos,    n1*n2*(1-cos)-n3*sin, n1*n3*(1-cos)+n2*sin,
				n2*n1*(1-cos)+n3*sin, n2*n2*(1-cos)+cos,    n2*n3*(1-cos)+n1*sin,
				n3*n1*(1-cos)+n2*sin, n3*n2*(1-cos)+n1*sin, n3*n3*(1-cos)+cos
			}, Vec3.Zero));
		}

		public void Scale(double v) => Scale(v, v, v);
		public void Scale(double x, double y, double z) {
			Apply(new Matrix4(new[] {
				x, 0, 0,
				0, y, 0,
				0, 0, z
			}, Vec3.Zero));
		}

		public Vec3 TransformVector(Vec3 vec) {
			return Matrix * vec;
		}

		public Transform Copy() {
			return new Transform(this);
		}
	}
}
