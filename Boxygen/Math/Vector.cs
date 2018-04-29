using System;
using System.Drawing;
using System.Security.Cryptography;
using Boxygen.Drawing.Primitives;
using Newtonsoft.Json;

namespace Boxygen.Math {
	public struct Vec2 {
		public static Vec2 Zero => new Vec2();
		public static Vec2 UnitX => new Vec2(1, 0);
		public static Vec2 UnitY => new Vec2(0, 1);

		public double X;
		public double Y;

		[JsonIgnore] public double Length => System.Math.Sqrt(X * X + Y * Y);
		[JsonIgnore] public Vec2 Normal => this == Zero ? Zero : this / Length;

		public Vec2(double v = 0) : this(v, v) { }
		public Vec2(double x, double y) {
			X = x;
			Y = y;
		}

		public double this[int i] {
			get {
				switch(i) {
					case 0: return X;
					case 1: return Y;
					default: throw new IndexOutOfRangeException();
				}
			}
			set {
				switch(i) {
					case 0: X = value; break;
					case 1: Y = value; break;
					default: throw new IndexOutOfRangeException();
				}
			}
		}

		public override bool Equals(object o) => o is Vec2 v && this == v;

		public override int GetHashCode() {
			unchecked {
				return (X.GetHashCode() * 397) ^ Y.GetHashCode();
			}
		}

		public override string ToString() => $"({X}, {Y})";

		public static bool operator ==(Vec2 l, Vec2 r) => System.Math.Abs(l.X - r.X) < 0.0001 && System.Math.Abs(l.Y - r.Y) < 0.0001;
		public static bool operator !=(Vec2 l, Vec2 r) => !(l == r);
		public static Vec2 operator +(Vec2 l, Vec2 r) => new Vec2(l.X + r.X, l.Y + r.Y);
		public static Vec2 operator -(Vec2 l, Vec2 r) => new Vec2(l.X - r.X, l.Y - r.Y);
		public static Vec2 operator *(Vec2 l, Vec2 r) => new Vec2(l.X * r.X, l.Y * r.Y);
		public static Vec2 operator /(Vec2 l, Vec2 r) => new Vec2(l.X / r.X, l.Y / r.Y);
		public static Vec2 operator *(Vec2 v, double s) => new Vec2(v.X * s, v.Y * s);
		public static Vec2 operator *(double s, Vec2 v) => new Vec2(v.X * s, v.Y * s);
		public static Vec2 operator /(Vec2 v, double s) => new Vec2(v.X / s, v.Y / s);
		public static Vec2 operator /(double s, Vec2 v) => new Vec2(v.X / s, v.Y / s);
		public static Vec2 operator +(Vec2 v) => v;
		public static Vec2 operator -(Vec2 v) => Zero - v;
		public static double operator |(Vec2 l, Vec2 r) => l.X * r.X + l.Y * r.Y;

		public static implicit operator PointF(Vec2 v) => new PointF((float)v.X, (float)v.Y);
	}

	public struct Vec3 {
		public static Vec3 Zero => new Vec3();
		public static Vec3 UnitX => new Vec3(1, 0, 0);
		public static Vec3 UnitY => new Vec3(0, 1, 0);
		public static Vec3 UnitZ => new Vec3(0, 0, 1);
		public static Vec3 Camera => new Vec3(-1, -1, -1);
		public static Vec3 Invalid => new Vec3(double.NaN);

		public double X, Y, Z;

		[JsonIgnore] public Vec2 XY => new Vec2(X, Y);
		[JsonIgnore] public Vec2 XZ => new Vec2(X, Z);
		[JsonIgnore] public Vec2 YX => new Vec2(Y, X);
		[JsonIgnore] public Vec2 YZ => new Vec2(Y, Z);
		[JsonIgnore] public Vec2 ZX => new Vec2(Z, X);
		[JsonIgnore] public Vec2 ZY => new Vec2(Z, Y);

		[JsonIgnore] public double Length => System.Math.Sqrt(X * X + Y * Y + Z * Z);
		[JsonIgnore] public Vec3 Normal => this == Zero ? Zero : this / Length;
		[JsonIgnore] public bool IsValid => !double.IsNaN(X) && !double.IsNaN(Y) && !double.IsNaN(Z);

		public Vec3(double v = 0) : this(v, v, v) { }
		public Vec3(Vec2 v, double z = 0) : this(v.X, v.Y, z) { }
		public Vec3(double x, double y, double z) {
			X = x;
			Y = y;
			Z = z;
		}

		public double this[int i] {
			get {
				switch(i) {
					case 0: return X;
					case 1: return Y;
					case 2: return Z;
					default: throw new IndexOutOfRangeException();
				}
			}
			set {
				switch(i) {
					case 0: X = value; break;
					case 1: Y = value; break;
					case 2: Z = value; break;
					default: throw new IndexOutOfRangeException();
				}
			}
		}

		public void Set(double v) => Set(v, v, v);
		public void Set(double x, double y, double z) {
			X = x;
			Y = y;
			Z = z;
		}

		public override bool Equals(object o) => o is Vec3 v && this == v;

		public override int GetHashCode() {
			unchecked {
				int hashCode = X.GetHashCode();
				hashCode = (hashCode * 397) ^ Y.GetHashCode();
				hashCode = (hashCode * 397) ^ Z.GetHashCode();
				return hashCode;
			}
		}

		public override string ToString() => $"({X}, {Y}, {Z})";

		public static bool operator ==(Vec3 l, Vec3 r) => System.Math.Abs(l.X - r.X) < 0.0001 && System.Math.Abs(l.Y - r.Y) < 0.0001 && System.Math.Abs(l.Z - r.Z) < 0.0001;
		public static bool operator !=(Vec3 l, Vec3 r) => !(l == r);
		public static Vec3 operator +(Vec3 l, Vec3 r) => new Vec3(l.X + r.X, l.Y + r.Y, l.Z + r.Z);
		public static Vec3 operator -(Vec3 l, Vec3 r) => new Vec3(l.X - r.X, l.Y - r.Y, l.Z - r.Z);
		public static Vec3 operator *(Vec3 l, Vec3 r) => new Vec3(l.X * r.X, l.Y * r.Y, l.Z * r.Z);
		public static Vec3 operator /(Vec3 l, Vec3 r) => new Vec3(l.X / r.X, l.Y / r.Y, l.Z / r.Z);
		public static Vec3 operator *(Vec3 v, double s) => new Vec3(v.X * s, v.Y * s, v.Z * s);
		public static Vec3 operator *(double s, Vec3 v) => new Vec3(v.X * s, v.Y * s, v.Z * s);
		public static Vec3 operator /(Vec3 v, double s) => new Vec3(v.X / s, v.Y / s, v.Z / s);
		public static Vec3 operator /(double s, Vec3 v) => new Vec3(v.X / s, v.Y / s, v.Z / s);
		public static Vec3 operator +(Vec3 v) => v;
		public static Vec3 operator -(Vec3 v) => Zero - v;
		public static Vec3 operator &(Vec3 l, Vec3 r) => new Vec3(l.Y * r.Z - l.Z * r.Y, l.Z * r.X - l.X * r.Z, l.X * r.Y - l.Y * r.X);
		public static double operator |(Vec3 l, Vec3 r) => l.X * r.X + l.Y * r.Y + l.Z * r.Z;

		#region Isometric projection

		private static readonly double TileHeight = 1;
		private static readonly double TileWidth = System.Math.Sin(60 * System.Math.PI / 180) * TileHeight * 2;

		private static readonly Vec2[] ProjectionMatrix = {
			new Vec2(TileWidth/2, TileHeight/2),
			new Vec2(-TileWidth/2, TileHeight/2),
			new Vec2(0, -TileHeight )
		};

		public Vec2 Project() => X * ProjectionMatrix[0] + Y * ProjectionMatrix[1] + Z * ProjectionMatrix[2];
		[JsonIgnore] public double ViewDistance => X + Y + Z; // -(this | Camera);
		public Vec3 FlipToFront() => ViewDistance < 0 ? -this : this;

		public Vec3 ProjectOnto(Primitive p) {
			var normal = p.Normal;
			var diff = p.O - this;
			double t = normal.X * diff.X + normal.Y * diff.Y + normal.Z * diff.Z;
			return this + normal * t;
		}

		public Vec3 ProjectAlong(Primitive target, Vec3 direction) {
			var normal = target.Normal;
			double dot = normal | direction;

			// line is parallel to plane
			if(System.Math.Abs(dot) < 0.0001) return new Vec3(double.NaN);

			// https://en.wikipedia.org/wiki/Line%E2%80%93plane_intersection#Algebraic_form
			return ((target.O - this) | normal) / dot * direction + this;
		}

		#endregion
	}
}
