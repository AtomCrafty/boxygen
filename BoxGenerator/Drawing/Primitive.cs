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
	}
}
