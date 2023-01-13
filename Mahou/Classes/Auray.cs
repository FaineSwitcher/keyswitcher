using System;
using System.Text;
using System.Diagnostics;
/// <summary> Multidimensional various types array.</summary>
public class Auray {
	public string raw;
	public int deep;
	public int len;
	public Auray() {
		this.raw = "[]";
		var size = __SIZE(raw);
		this.deep = size.deep;
		this.len = size.len;
	}
	public Auray(string RAW) {
		this.raw = RAW;
		this.raw = this.raw.Replace("\n","");
		this.raw = this.raw.Replace("\r","");
		var size = __SIZE(raw);
		this.deep = size.deep;
		this.len = size.len;
	}
	public string this[params int[] i] {
		get {
			return Get(i);
		}
		set {
			int v;
			if (Int32.TryParse(value, out v))
				Set(value, i);
			else Set("\""+value+"\"", i);
		}
 	}
	public string Get(params int[] ind_path) {
		return __GET(ind_path);
	}
	public void Set(string value, params int[] ind_path) {
		__SET(ind_path, value);
	}
	#region Parse, etc.
	void __SET(int[] index_path, string VALUE) {
		var l = index_path.Length;
		var indpath = __STR_INDEX_PATH(index_path);
//		Debug.WriteLine(">Index Path => " + indpath);
		var pre = new StringBuilder();
		var val = raw;
		var post = new StringBuilder();
		var nw = new StringBuilder();
		var c = 0;
		var incr = false;
		var _l = 0;
		for(int i = 0; i!= l; i++) {
			bool nuv = false, tsm = false;
			var minus = 0;
			if (val != null) {
				if (val.Length >= 2) {
					var siz = __SIZE(val);
					if (siz.len < index_path[i]) {
						tsm = true;
						minus = siz.len+1;
					}
					if (!nuv) {
						if (val[0] == '[' && val[val.Length-1] == ']') {
							var r = __PARSE(index_path[i], val);
							val = r[0];
							if (!incr) {
								pre.Append(r[1]).Append(",");
								post.Append(r[2]);
							}
						}
					} else {
						pre = new StringBuilder(raw.Substring(1, raw.Length-2));
						post = new StringBuilder("]");
						incr = true;
					}
				} else nuv = true;
			} else nuv = true;
			if (nuv || tsm) {
				if (!tsm) {
//					if (_l != 0)
//						nw += ",";
					nw.Append("["); c++;
				}
				var ll = index_path[i]-minus;
//				Debug.WriteLine("ADDING "+ll+" emptys.");
				for (int k = 0; ; k++) {
					if (k >= ll) break;
					nw.Append(",");
				}
			}
			_l = index_path[i];
//			else throw new Exception("Out of index at #"+i+" index path ["+index_path[i]+"] of "+indpath+", value: " + val + " is not array.");
		}
		nw.Append(VALUE);
		for (int i = 0; i != c; i++)
			nw.Append("]");
//		Debug.WriteLine(pre + "/" + val + "/" + post);
//		Debug.WriteLine(nw);
		raw = new StringBuilder("[").Append(pre.ToString()).Append(nw.ToString()).Append(post.ToString()).Append("]").ToString();
		var dl = __SIZE(raw);
		len = dl.len;
		deep = dl.deep;
//		Debug.WriteLine(raw);
//		Debug.WriteLine("len: " +dl.len + ", deep: " +dl.deep);
	}
	string __GET(int[] index_path) {
		var val = raw;
		var indpath = __STR_INDEX_PATH(index_path);
//		Debug.WriteLine("<Index Path => " + indpath);
		for(int i = 0; i!= index_path.Length; i++) {
//			Debug.WriteLine("Move #"+i+" => [" + index_path[i]+"]");
			var nuv = false;
			if (val != null) {
				var siz = __SIZE(val);
				if (siz.len < index_path[i]) nuv = true;
				if (val.Length >= 2) {
					if (val[0] == '[' && val[val.Length-1] == ']') {
						val = __PARSE(index_path[i], val)[0];
					}
				} else nuv = true;
			} else nuv = true;
			if (nuv) throw new Exception("Out of index at #"+i+" index path ["+index_path[i]+"] of "+indpath+", value: <" + val + "> from "+raw+" is not array.");
		}
		return val;
	}
	string[] __PARSE(int ind, string RAW) {
		var vi = new string[3];
		int d = 0, skip = 0;
		bool enq_s = false, enq_d = false, enq = false, ef = false; 
		for (int t = 1; t != RAW.Length-1; t++) {
			enq = enq_d || enq_s;
			char v = RAW[t], l = '\0';
			if (t-1 >= 0)
				l = RAW[t-1];
			if (l != '\\') {
				if (v == '\"' && !enq_s)
					enq_d = !enq_d;
				if (v == '\'' && !enq_d)
					enq_s = !enq_s;
			}
			if (!enq) {
				if (v == '[' )
					if (!ef) ef = true;
					else skip++;
				if (v == ']' && ef)
					if (skip > 0) skip--;
					else  ef = false;
				if (v == ',' && !enq && !ef)
					d++;
			}
			if ((v != ',' && 
			     ((v != ' ' && v != '\r' && v != '\n') && !enq))
			     || enq || ef) {
				if (d == ind) vi[0] += v;
			}
			if (d < ind)  vi[1] += v;
			if (d > ind)  vi[2] += v;
		}
		if (ef) {
			throw new Exception("Array [ and ] counts mismatch: " + RAW);
		}
		return vi;
	}
	string __STR_INDEX_PATH(int[] index_path, int l = -1) {
		if (l == -1) l = index_path.Length;
		var res = new StringBuilder();
		for(int i=0;i!=l;i++)
			res.Append("[").Append(index_path[i]).Append("]");
		return res.ToString();
	}
	SIZE __SIZE(string RAW) {
		int mx = 0, lvl = 0, tvls = 0;
		bool enq_s = false, enq_d = false;
		for (int t = 0; t != RAW.Length; t++) {
			var enq = enq_d || enq_s;
			char v = RAW[t], l = '\0';
			if (t-1 >= 0)
				l = RAW[t-1];
			if (l != '\\') {
				if (v == '"' && !enq_s) enq_d = !enq_d;
				if (v == '\'' && !enq_d) enq_s = !enq_s;
			}
			if (!enq) {
				if (v == '[') lvl++;
				if (v == ']') lvl--;
				if (mx < lvl)
					mx = lvl;
			}
			if (lvl == 0 && v == ',')
				tvls++;
		}
		if (lvl != 0) {
			throw new Exception("Array [ and ] counts mismatch: " + RAW);
		}
		return new SIZE(){len = tvls, deep = mx};
	}
	struct SIZE {
		public int deep;
		public int len;
	}
	#endregion
}