using System;

namespace Boxygen.Math {
	public class Matrix4 {
		private static readonly double[] IdentityValues = { 1, 0, 0, 0, 0, 1, 0, 0, 0, 0, 1, 0, 0, 0, 0, 1 };
		public static Matrix4 Identity => new Matrix4();

		private readonly double[] _values;

		public Matrix4() {
			_values = (double[])IdentityValues.Clone();
		}

		public Matrix4(Matrix4 matrix) {
			_values = (double[])matrix._values.Clone();
		}

		public Matrix4(Vec3 offset) : this() {
			_values[12] = offset.X;
			_values[13] = offset.Y;
			_values[14] = offset.Z;
		}

		public Matrix4(double[] values, Vec3 offset) {
			if(values.Length != 9) throw new ArgumentException("Expected 9 values", nameof(values));
			_values = new[] {
				values[0], values[1], values[2], 0,
				values[3], values[4], values[5], 0,
				values[6], values[7], values[8], 0,
				offset.X,  offset.Y,  offset.Z,  1
			};
		}

		public double this[int x, int y] {
			get => _values[x + 4 * y];
			private set => _values[x + 4 * y] = value;
		}

		public static Matrix4 operator *(Matrix4 l, Matrix4 r) {
			var m = new Matrix4();
			for(int y = 0; y < 4; y++) {
				for(int x = 0; x < 4; x++) {
					double s = 0;
					for(int i = 0; i < 4; i++) {
						s += l[i, y] * r[x, i];
					}
					m[x, y] = s;
				}
			}
			return m;
		}

		public static Vec3 operator *(Matrix4 m, Vec3 v) {
			var r = new Vec3();
			for(int j = 0; j < 3; j++) {
				double s = 0;
				for(int i = 0; i < 3; i++) {
					s += m[i, j] * v[i];
				}
				r[j] = s;
			}

			r.X += m._values[12];
			r.Y += m._values[13];
			r.Z += m._values[14];
			return r;
		}
	}
}