using System.Drawing;
using Boxygen.Math;

namespace Boxygen.Drawing {
	public interface IDrawable {
		void Draw(Graphics g, Vec2 center);
	}
}
