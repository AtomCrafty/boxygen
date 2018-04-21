using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Boxygen.Math {
	public class OrderingGraph<T> where T : class {

		public List<Node> Nodes = new List<Node>();

		#region Node and edge management

		public bool HasEdge(Node src, Node dst) {
			return src.Outgoing.Contains(dst) && dst.Incoming.Contains(src);
		}

		public void AddEdge(Node src, Node dst) {
			if(HasEdge(src, dst)) return;
			src.Outgoing.Add(dst);
			dst.Incoming.Add(src);
		}

		public void RemoveEdge(Node src, Node dst) {
			if(!src.Outgoing.Remove(dst)) throw new InvalidOperationException();
			if(!dst.Incoming.Remove(src)) throw new InvalidOperationException();
		}

		public void AddNodeBetween(Node parent, Node child, Node insert) {
			RemoveEdge(parent, child);
			AddEdge(parent, insert);
			AddEdge(insert, child);
		}

		public void RemoveNodeAndReconnect(Node node) {
			foreach(var parent in node.Incoming) {
				foreach(var child in node.Outgoing) {
					AddEdge(parent, child);
				}
			}

			node.Incoming.ForEach(p => RemoveEdge(p, node));
			node.Outgoing.ForEach(c => RemoveEdge(node, c));
		}

		public void EncloseParents(Node of, Node with) {
			of.Incoming.ForEach(p => {
				RemoveEdge(p, of);
				AddEdge(p, with);
			});
			AddEdge(with, of);
		}

		public void EncloseChildren(Node of, Node with) {
			of.Outgoing.ForEach(c => {
				RemoveEdge(of, c);
				AddEdge(with, c);
			});
			AddEdge(of, with);
		}

		public void AddNode(Node node) {
			Nodes.Add(node);
		}

		public Node AddNode(T tag) {
			var node = new Node(tag);
			Nodes.Add(node);
			return node;
		}

		#endregion

		public static List<T> Sort(List<T> list, Comparer<T> comp) {
			var graph = new OrderingGraph<T>();

			// top node; frontmost layer; drawn last
			var front = new Node(null);
			// bottom node; backmost layer; drawn first
			var back = new Node(null);

			graph.AddNode(front);
			graph.AddNode(back);
			graph.AddEdge(front, back);

			// build depencency graph
			foreach(var tag in list) {
				var node = graph.AddNode(tag);

				// node must be a descendant of these
				var upperBounds = graph.Nodes.Where(other => GetOrder(node, other) > 0).ToList();
				// node must be an ancestor of these
				var lowerBounds = graph.Nodes.Where(other => GetOrder(node, other) < 0).ToList();

				Console.WriteLine("Processing: " + tag);
				Console.WriteLine("Upper bounds: " + string.Join(", ", upperBounds));
				Console.WriteLine("Lower bounds: " + string.Join(", ", lowerBounds));

				upperBounds.ForEach(bound => graph.AddEdge(bound, node));
				lowerBounds.ForEach(bound => graph.AddEdge(node, bound));
			}

			Debug.Assert(front.GetDescendants().Distinct().Count() == list.Count + 1);
			Debug.Assert(back.GetAncestors().Distinct().Count() == list.Count + 1);

			// topologically sort the graph
			var l = new List<T>();
			var s = new Queue<Node>();
			s.Enqueue(back);
			while(s.Any()) {
				var n = s.Dequeue();
				// don't add "back" dummy tag
				if(n.Tag != null) l.Add(n.Tag);
				foreach(var m in n.Incoming.ToList()) {
					graph.RemoveEdge(m, n);
					if(!m.Outgoing.Any()) {
						s.Enqueue(m);
					}
				}
			}

			return l;

			// < 0: "node" is an ancestor of "other"
			// = 0: no correlation
			// > 0: "node" is a descendant of "other"
			int GetOrder(Node node, Node other) {
				if(node == other) return 0;
				if(other == front) return 1;
				if(other == back) return -1;
				return comp.Compare(other.Tag, node.Tag);
			}
		}

		public class Node {
			internal List<Node> Incoming = new List<Node>();
			internal List<Node> Outgoing = new List<Node>();
			public T Tag;

			public Node(T tag) {
				Tag = tag;
			}

			public List<Node> GetAncestors() {
				var list = new List<Node>();
				AddAncestors(list, false);
				return list;
			}

			public void AddAncestors(List<Node> list, bool includeSelf) {
				if(includeSelf) list.Add(this);
				Incoming.ForEach(p => p.AddAncestors(list, true));
			}

			public List<Node> GetDescendants() {
				var list = new List<Node>();
				AddDescendants(list, false);
				return list;
			}

			public void AddDescendants(List<Node> list, bool includeSelf) {
				if(includeSelf) list.Add(this);
				Outgoing.ForEach(p => p.AddDescendants(list, true));
			}

			public override string ToString() => Tag?.ToString() ?? "null";
		}
	}
}