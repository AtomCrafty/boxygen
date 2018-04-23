using System;
using System.Drawing;
using Boxygen.Drawing.Primitives;
using Boxygen.Math;

namespace Boxygen.Drawing.Objects.Deco {
	public class TextureDecoration : Decoration {

		public string Name;

		public Image Texture;
		public Anchor Anchor;
		public Vec2 Position;
		public Vec2 Margin = new Vec2(10, 10);
		public Vec2 Size;

		public TextureDecoration(Face face, Image texture, Vec2 size, Anchor anchor) : base(face) {
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

			var origin = faceO + normalA * Margin.Y + normalB * Margin.X;
			if(Anchor == Anchor.Custom) {
				origin += normalA * Position.Y + normalB * Position.X;
			}
			else {
				origin += dispA * Anchor.DispFactorA() + dispB * Anchor.DispFactorB();
			}

			//list.Add(new Tex(origin, origin + spanA, origin + spanB, Texture));
			list.Add(new Quad(origin, origin + spanA, origin + spanB) { Fill = Brushes.White, Name = Name });
		}
	}

	public static class AnchorExtension {
		public static double DispFactorA(this Anchor anchor) {
			switch(anchor) {
				case Anchor.TopLeft:
				case Anchor.Top:
				case Anchor.TopRight:
					return 1;
				case Anchor.Left:
				case Anchor.Center:
				case Anchor.Right:
					return 0.5;
				case Anchor.BottomLeft:
				case Anchor.Bottom:
				case Anchor.BottomRight:
					return 0;
				case Anchor.Custom:
					return 0;
				default: throw new ArgumentOutOfRangeException(nameof(anchor), anchor, null);
			}
		}

		public static double DispFactorB(this Anchor anchor) {
			switch(anchor) {
				case Anchor.TopRight:
				case Anchor.Right:
				case Anchor.BottomRight:
					return 1;
				case Anchor.Top:
				case Anchor.Center:
				case Anchor.Bottom:
					return 0.5;
				case Anchor.TopLeft:
				case Anchor.Left:
				case Anchor.BottomLeft:
					return 0;
				case Anchor.Custom:
					return 0;
				default: throw new ArgumentOutOfRangeException(nameof(anchor), anchor, null);
			}
		}
	}
}
