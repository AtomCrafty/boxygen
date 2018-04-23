using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using Boxygen.Math;

namespace Boxygen.Drawing {
	public abstract class Primitive : IDrawable {

		public Vec3 O, A, B;
		public Vec3 SpanA => A - O;
		public Vec3 SpanB => B - O;
		public Vec3 Normal => (SpanA & SpanB).Normal;

		public List<Primitive> FrontLayers = new List<Primitive>();
		public List<Primitive> BackLayers = new List<Primitive>();

		protected Primitive(Vec3 o, Vec3 a, Vec3 b) {
			O = o;
			A = a;
			B = b;
		}

		public string Name;

		public Brush Fill = new SolidBrush(Color.Transparent);
		public Pen Stroke = Pens.Transparent;

		public abstract Vec3 CenterOfMass { get; }
		public abstract Vec3[] Vertices { get; }
		public abstract double Area { get; }

		public abstract void Draw(Graphics g);

		public void DrawNormals(Graphics g, bool dashed) {
			var s = CenterOfMass.Project();
			var n = (SpanA & SpanB).Project();
			var f = (Normal * System.Math.Sign(Normal.ViewDistance) * 100).Project();

			g.DrawLine(new Pen(Color.LawnGreen) { DashStyle = dashed ? DashStyle.Dash : DashStyle.Solid }, (float)s.X, (float)s.Y, (float)(s.X + n.X), (float)(s.Y + n.Y));
			g.DrawLine(new Pen(Color.Yellow, 2) { DashStyle = dashed ? DashStyle.Dash : DashStyle.Solid }, (float)s.X, (float)s.Y, (float)(s.X + f.X), (float)(s.Y + f.Y));
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
			var n1 = p1.Normal * System.Math.Sign(p1.Normal.ViewDistance);
			var n2 = p2.Normal * System.Math.Sign(p2.Normal.ViewDistance);

			var c1 = p1.CenterOfMass;
			var c2 = p2.CenterOfMass;

			if(p1.BackLayers.Contains(p2) || p2.FrontLayers.Contains(p1)) return 1;
			if(p2.BackLayers.Contains(p1) || p1.FrontLayers.Contains(p2)) return -1;

			int order = 0;
			bool foundOrder = false;

			// all vertecies are projected onto the camera-facing normal vector
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

			if(!foundOrder) Console.WriteLine($"Unable to determine order of {p1} and {p2}");
			return order;
		}
	}
}
