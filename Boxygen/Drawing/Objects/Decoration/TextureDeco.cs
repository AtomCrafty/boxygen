using System;
using System.Drawing;
using Boxygen.Drawing.Materials;
using Boxygen.Drawing.Primitives;
using Boxygen.Math;

namespace Boxygen.Drawing.Objects.Decoration {
	public class TextureDeco : Deco {

		public string Name;
		public DecoAnchor Anchor;
		public Texture Texture;
		public Vec2 Position;
		public Vec2 Margin = new Vec2(5, 5);
		public Vec2 Size;

		public TextureDeco(Face face, Texture texture, Vec2 size, DecoAnchor anchor) : base(face) {
			Texture = texture;
			Anchor = anchor;
			Size = size;
		}

		public override void Gather(RenderList list) {

			// Fetch face vectors
			var faceO = Face.O.Pos;
			var faceA = Face.A.Pos - faceO;
			var faceB = Face.B.Pos - faceO;
			var normalA = faceA.Normal;
			var normalB = faceB.Normal;

			// Texture spans
			var spanA = normalA * Size.Y;
			var spanB = normalB * Size.X;

			// A and B displacements
			var dispA = faceA - normalA * Margin.Y * 2 - spanA;
			var dispB = faceB - normalB * Margin.X * 2 - spanB;

			var origin = faceO + normalA * Margin.Y + normalB * Margin.X +
			             (Anchor == DecoAnchor.Custom
				             ? normalA * Position.Y + normalB * Position.X
				             : dispA * Anchor.DispFactorA() + dispB * Anchor.DispFactorB());

			// flip texture vertically
			list.Add(new Tex(origin + spanA, origin, origin + spanA + spanB, Texture) { Name = Name });

			//list.Add(new Tex(origin, origin + spanA, origin + spanB, Texture));
			//list.Add(new Quad(origin, origin + spanA, origin + spanB) { Fill = Brushes.White, Name = Name });
		}
	}

	public static class AnchorExtension {
		public static double DispFactorA(this DecoAnchor anchor) {
			switch(anchor) {
				case DecoAnchor.TopLeft:
				case DecoAnchor.Top:
				case DecoAnchor.TopRight:
					return 1;
				case DecoAnchor.Left:
				case DecoAnchor.Center:
				case DecoAnchor.Right:
					return 0.5;
				case DecoAnchor.BottomLeft:
				case DecoAnchor.Bottom:
				case DecoAnchor.BottomRight:
					return 0;
				case DecoAnchor.Custom:
					return 0;
				default: throw new ArgumentOutOfRangeException(nameof(anchor), anchor, null);
			}
		}

		public static double DispFactorB(this DecoAnchor anchor) {
			switch(anchor) {
				case DecoAnchor.TopRight:
				case DecoAnchor.Right:
				case DecoAnchor.BottomRight:
					return 1;
				case DecoAnchor.Top:
				case DecoAnchor.Center:
				case DecoAnchor.Bottom:
					return 0.5;
				case DecoAnchor.TopLeft:
				case DecoAnchor.Left:
				case DecoAnchor.BottomLeft:
					return 0;
				case DecoAnchor.Custom:
					return 0;
				default: throw new ArgumentOutOfRangeException(nameof(anchor), anchor, null);
			}
		}
	}
}
