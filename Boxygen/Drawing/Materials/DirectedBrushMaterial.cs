﻿using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using Boxygen.Drawing.Primitives;
using Boxygen.Math;

namespace Boxygen.Drawing.Materials {
	public class DirectedBrushMaterial : Material {

		public Color Color = Color.FromArgb(unchecked((int)0xffdda866));

		public override void DrawPolygon(RenderContext ctx, Graphics g, Primitive p) {
			var light = ctx.LightNormal;
			var normal = p.Normal.FlipToFront();

			// TODO faces facing away from the light source should be darker
			double diffuseFactor = System.Math.Max(normal | -light, 0);

			double ambient = ctx.AmbientIntensity;
			double intensity = ambient + (1 - ambient) * ctx.DiffuseIntensity * diffuseFactor;
			double intensity2 = ambient + (1 - ambient) * ctx.DiffuseIntensity * diffuseFactor / 4;

			var ambientFill = Color.FromArgb(
				(int)System.Math.Min(Color.R * intensity2, 255),
				(int)System.Math.Min(Color.G * intensity2, 255),
				(int)System.Math.Min(Color.B * intensity2, 255)
			);

			var diffuseFill = Color.FromArgb(
				(int)System.Math.Min(Color.R * intensity, 255),
				(int)System.Math.Min(Color.G * intensity, 255),
				(int)System.Math.Min(Color.B * intensity, 255)
			);

			var src = new Vec3(0, 0, 50);
			var map = src.Project();
			var pro = src.ProjectAlong(p, light);
			var res = pro.Project();

			//var col = Color.FromArgb(p.Name[5] * 6 % 256, p.Name[6] * 6 % 256, p.Name[7] * 6 % 256);
			//var tcol = Color.FromArgb((int)(col.ToArgb() & 0x8fffffff));

			g.FillPolygon(new SolidBrush(ambientFill), p.Points);

			ctx.LightNormal = new Vec3(-1, -0.5, -0.3).Normal;
			ctx.LightNormal = new Vec3(-1, -0.1, -0.7).Normal;

			var shadowMap = new Region();
			shadowMap.MakeEmpty();
			if(p.ReceivesShadows) {
				foreach(var polygon in ctx.Polygons.Where(pol => pol.CastsShadows)) {
					if(polygon == p || polygon is Stack stack && stack.RenderList.Contains(p)) continue;

					bool throwsShadow = false;
					var shadow = polygon.Vertices.Select(v => {
						double dot = normal | ctx.LightNormal;

						if(System.Math.Abs(dot) < 0.0001) {
							// line is parallel to plane
							throwsShadow = false;
							return PointF.Empty;
						}

						double d = ((p.O - v) | normal) / dot;

						if(d > 0.0001) throwsShadow = true;
						return (d * ctx.LightNormal + v).Project();

					}).ToArray();


					if(!throwsShadow) continue;

					var path = new GraphicsPath();
					path.AddPolygon(shadow);
					shadowMap.Union(path);

					//var pcol = Color.FromArgb(polygon.Name[5] * 6 % 256, polygon.Name[6] * 6 % 256, polygon.Name[7] * 6 % 256);
					//g.DrawLine(new Pen(pcol), p.CenterOfMass.Project(), polygon.CenterOfMass.Project());
					//g.DrawPolygon(Pens.Green, polygon.Points);
					//g.DrawPath(new Pen(pcol), path);
				}
			}
			var all = new Region();
			shadowMap.Complement(all);
			var area = new GraphicsPath();
			area.AddPolygon(p.Points);
			shadowMap.Intersect(area);

			//g.DrawLine(new Pen(col, 2), (float)map.X, (float)map.Y, (float)res.X, (float)res.Y);
			//g.FillRectangle(new SolidBrush(col), (float)res.X - 4, (float)res.Y - 4, 8, 8);
			g.FillRegion(new SolidBrush(diffuseFill), shadowMap);
		}
	}
}
