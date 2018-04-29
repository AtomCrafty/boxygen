using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Boxygen.Math;
using Newtonsoft.Json;

namespace Boxygen.Drawing.Objects {
	public class Scene : Group {

		public Vec3 LightDirection;

		public void Save(string path) {
			//using(var writer = new StreamWriter(path)) {
			//new JsonSerializer {
			//Formatting = Formatting.Indented,
			//ReferenceLoopHandling = ReferenceLoopHandling.Serialize,
			//PreserveReferencesHandling = PreserveReferencesHandling.Objects,
			//TypeNameHandling = TypeNameHandling.Auto
			//}.Serialize(writer, this);
			//}

			//using(var fs = new FileStream(path, FileMode.Create, FileAccess.Write)) {
			//new BinaryFormatter().Serialize(fs, this);
			//}
		}
	}
}
