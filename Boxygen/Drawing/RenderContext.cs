using System.Collections.Generic;
using System.Drawing;
using Boxygen.Drawing.Primitives;
using Boxygen.Math;

namespace Boxygen.Drawing {
	public class RenderContext {

		public Color WorldColor = Color.Black;

		// lighting
		public Vec3 LightNormal;
		public double AmbientIntensity;
		public double DiffuseIntensity;

		public List<Primitive> Polygons;
	}
}
