using System.Drawing;

namespace Boxygen.Drawing {
	public interface IDrawable {
		void Draw(RenderContext ctx, Graphics g);
	}
}
