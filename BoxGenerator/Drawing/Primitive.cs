using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using Boxygen.Math;

namespace Boxygen.Drawing {
	public abstract class Primitive : IDrawable {

		public Vec3 Origin, SpanA, SpanB;
		public Vec3 Normal => (SpanA & SpanB).Normal;

		public string Name;

		public Brush Fill = new SolidBrush(Color.Transparent);
		public Pen Stroke = Pens.Aqua;

		public abstract Vec3 CenterOfMass { get; }
		public abstract double Area { get; }

		public abstract void Draw(Graphics g, Vec2 center);

		public virtual void Transform(Transform transform) {
			Origin = transform.TransformVector(Origin);
			SpanA = transform.TransformVector(SpanA);
			SpanB = transform.TransformVector(SpanB);
		}

		public void DrawNormals(Graphics g, Vec2 center, bool dashed) {
			var c = center;
			var s = CenterOfMass.Project();
			var n = (SpanA & SpanB).Project();
			var f = (Normal * System.Math.Sign(Normal.ViewDistance) * 100).Project();

			g.ResetTransform();
			g.DrawLine(new Pen(Color.LawnGreen) { DashStyle = dashed ? DashStyle.Dash : DashStyle.Solid }, (float)(c.X + s.X), (float)(c.Y + s.Y), (float)(c.X + s.X + n.X), (float)(c.Y + s.Y + n.Y));
			g.DrawLine(new Pen(Color.Yellow, 2) { DashStyle = dashed ? DashStyle.Dash : DashStyle.Solid }, (float)(c.X + s.X), (float)(c.Y + s.Y), (float)(c.X + s.X + f.X), (float)(c.Y + s.Y + f.Y));
		}

		public override string ToString() => Name ?? GetType().Name;

		public static Comparer<Primitive> OrderSelector => Comparer<Primitive>.Create(SelectOrder);

		public static int SelectOrder(Primitive p1, Primitive p2) {
			const bool verbose = false;

			if(!Intersect(p1, p2)) return 0;

			var n1 = p1.Normal * System.Math.Sign(p1.Normal.ViewDistance);
			var n2 = p2.Normal * System.Math.Sign(p2.Normal.ViewDistance);

			var o1 = p1.Origin - p2.CenterOfMass;
			var o2 = p2.Origin - p1.CenterOfMass;

			int state = 0;
			bool foundOrder = false;

			// all vertecies are projected onto the camera-facing normal
			// if the value is < 0, they are behind, if it is > 0, they are in front

			if((o2 | n1) <= 0.0001 &&
			   ((o2 + p2.SpanA) | n1) <= 0.0001 &&
			   ((o2 + p2.SpanB) | n1) <= 0.0001 &&
			   ((o2 + p2.SpanA + p2.SpanB) | n1) <= 0.0001) {
				// all vertecies of p2 are behind the p1 plane
				if(verbose) Console.WriteLine($"{p2} is behind {p1}");
				state += 1;
				foundOrder = true;
			}

			if((o2 | n1) >= -0.0001 &&
			   ((o2 + p2.SpanA) | n1) >= -0.0001 &&
			   ((o2 + p2.SpanB) | n1) >= -0.0001 &&
			   ((o2 + p2.SpanA + p2.SpanB) | n1) >= -0.0001) {
				// all vertecies of p2 are in front of the p1 plane
				if(verbose) Console.WriteLine($"{p1} is in front of {p2}");
				state -= 1;
				foundOrder = true;
			}

			if((o1 | n2) <= 0.0001 &&
			   ((o1 + p1.SpanA) | n2) <= 0.0001 &&
			   ((o1 + p1.SpanB) | n2) <= 0.0001 &&
			   ((o1 + p1.SpanA + p1.SpanB) | n2) <= 0.0001) {
				// all vertecies of p1 are behind the p2 plane
				if(verbose) Console.WriteLine($"{p1} is behind {p2}");
				state -= 1;
				foundOrder = true;
			}

			if((o1 | n2) >= -0.0001 &&
			   ((o1 + p1.SpanA) | n2) >= -0.0001 &&
			   ((o1 + p1.SpanB) | n2) >= -0.0001 &&
			   ((o1 + p1.SpanA + p1.SpanB) | n2) >= -0.0001) {
				// all vertecies of p1 are in front of the p2 plane
				if(verbose) Console.WriteLine($"{p1} is in front of {p2}");
				state += 1;
				foundOrder = true;
			}

			if(!foundOrder) Console.WriteLine($"Unable to determine order of {p1} and {p2}");
			return state;
		}

		public static bool Intersect(Primitive p1, Primitive p2) {
			// TODO Intersect
			return true;
		}
	}
}
