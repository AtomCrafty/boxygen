using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using Boxygen.Drawing.Materials;
using Boxygen.Math;

namespace Boxygen.Drawing.Primitives {
	public abstract class Primitive : IDrawable {

		#region Fields and properties

		public Vec3 O, A, B;
		public Vec3 SpanA => A - O;
		public Vec3 SpanB => B - O;
		public Vec3 ScaledNormal => SpanA & SpanB;
		public Vec3 Normal => (SpanA & SpanB).Normal;

		public abstract Vec3 CenterOfMass { get; }
		public abstract Vec3[] Vertices { get; }
		public abstract double Area { get; }
		public PointF[] Points => Vertices.Select(v => (PointF)v.Project()).ToArray();

		public string Name;
		public Material Material = Material.Default;

		#endregion

		protected Primitive(Vec3 o, Vec3 a, Vec3 b) {
			O = o;
			A = a;
			B = b;
		}

		public virtual void Draw(RenderContext ctx, Graphics g) {
			Material.DrawPolygon(ctx, g, this);
		}

		public void DrawNormals(Graphics g, bool dashed) {
			var s = CenterOfMass.Project();
			var n = Normal.Project() * 100;
			var f = Normal.FlipToFront().Project() * 100;

			g.DrawLine(new Pen(Color.Yellow, 3f) { DashStyle = dashed ? DashStyle.Dash : DashStyle.Solid }, (float)s.X, (float)s.Y, (float)(s.X + f.X), (float)(s.Y + f.Y));
			g.DrawLine(new Pen(Color.LawnGreen, 0.5f) { DashStyle = dashed ? DashStyle.Dash : DashStyle.Solid }, (float)s.X, (float)s.Y, (float)(s.X + n.X), (float)(s.Y + n.Y));
		}

		public virtual void Transform(Transform transform) {
			O = transform.TransformVector(O);
			A = transform.TransformVector(A);
			B = transform.TransformVector(B);
		}

		public override string ToString() => Name ?? GetType().Name;

		public static Comparer<Primitive> OrderSelector => Comparer<Primitive>.Create(SelectOrder);

		public static bool Verbose = false;

		public static int SelectOrder(Primitive p1, Primitive p2) {

			var n1 = p1.Normal.FlipToFront();
			var n2 = p2.Normal.FlipToFront();
			var c1 = p1.CenterOfMass;
			var c2 = p2.CenterOfMass;

			int order = 0;
			bool foundOrder = false;

			// all vertecies of one primitive are projected onto the front-facing normal vector of the other one
			// if the value is < 0, they are behind, if it is > 0, they are in front

			if(p2.Vertices.All(v => (v - c1 | n1) < 0.0001)) {
				// all vertecies of p2 are behind the p1 plane
				if(Verbose) Console.WriteLine($"{p2} is behind {p1}");
				order++;
				foundOrder = true;
			}
			else
			if(p2.Vertices.All(v => (v - c1 | n1) > -0.0001)) {
				// all vertecies of p2 are in front of the p1 plane
				if(Verbose) Console.WriteLine($"{p1} is in front of {p2}");
				order--;
				foundOrder = true;
			}

			if(p1.Vertices.All(v => (v - c2 | n2) < 0.0001)) {
				// all vertecies of p1 are behind the p2 plane
				if(Verbose) Console.WriteLine($"{p1} is behind {p2}");
				order--;
				foundOrder = true;
			}
			else
			if(p1.Vertices.All(v => (v - c2 | n2) > -0.0001)) {
				// all vertecies of p1 are in front of the p2 plane
				if(Verbose) Console.WriteLine($"{p2} is in front of {p1}");
				order++;
				foundOrder = true;
			}

			if(!foundOrder) Console.WriteLine($"Unable to determine order of {p1} and {p2}. Are they intersecting?");
			return order;
		}
	}
}
