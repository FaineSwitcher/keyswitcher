using System;
using System.Text;
using System.Text.RegularExpressions;

public class Auri {
	public string raw;
	public string DIGX;
	public Auri() {
		this.raw = "[]";
		this.DIGX = "^";
	}
	public Auri(string RAW, string DIGGX = "") {
		this.raw = RAW;
		this.DIGX = "^";
		if (!String.IsNullOrEmpty(DIGGX)) DIGX = DIGGX;
	}
	int[] indMat(char o, char c, string d, int x) {
		bool closed = false;
		int z = 0, lo = -1, lc = -1, mx = 0;
		for (int i = 0; i != d.Length; i++) {
			var v = d[i];
			if (v == o) { z++; lo = i; }
			if (v == c) { z--; lc = i; closed = true; } else { closed = false; }
			if (z > mx) mx = z;
			if (mx == x && closed) break;
		}
		return new int[]{lo, lc};
	}	
	public string this[params string[] s] {
		get {
			return Get(s);
		}
	}
	public string Get(params string[] block) {
		return __GET(block);
	}
	void setmax(ref int max, int test) {
		if (max<test) max = test;
	}
	string __GET(string[] block, string RAW = "") {
		string rawr = raw;
		if (RAW != "") rawr = RAW;
		if (string.IsNullOrEmpty(rawr)) return rawr;
		for (int p = 0; p != block.Length; p++) {
			var blox = block[p];
			Console.WriteLine("Block Move: "+blox);
			bool indblock = false;
			var ind = -1;
			if (blox.StartsWith(DIGX,StringComparison.InvariantCulture)) {
				var ccc = Regex.Matches(blox, @"\^").Count;
				if (ccc > 1) {
					var ali = Regex.Split(blox, "\\^");
					for (int j = 1; j <= ccc; j++) {
						if (ali[j] == "") continue;
						rawr = __GET(new []{"^"+ali[j]}, rawr);
						Console.WriteLine("Multi-Blox #" + j + " ~ " + ali[j]);
					}
					continue;
				}
				indblock = true;
				Int32.TryParse(blox.Replace(DIGX, ""), out ind);
				if (ind == -1) {
					throw new Exception("Wrong index: " + blox);
				}
			}
			var f = indMat('{', '}', rawr, 1);
			bool noblock = false;
			if (!rawr.Contains("{") && !rawr.Contains("}")) noblock = true;
			var en = f[1];
//				if (indblock)
				en = rawr.Length-1;
			if (f[1] == -1 && !noblock) {
				throw new Exception("Bad array, block ending not found: }");
			}
			var bln = new StringBuilder();
			int max_v = 0;
			if (f[0] != -1 || noblock) {
				bool inb = false, inq = false, inqq = false, nextv = false, inbs = false, inbc = false, inbo = false, vals = false;
				int z = 0, q = 0, qx = 0, zx = 0, v = 0, vx = 0, vl = -1, sq = -1;
				for (int i = 0; i <= en; i++) {
					var k = rawr[i];
					if (!inq) {
						if (k == '\n' || k == '\r' || k == ' ' || k == '\t') {
//							Console.WriteLine("You lose: <"+k+">");
							continue;
						}
					}
					if (k == '"') { if (inq) { inq = false; inqq = true; } else { inqq = false; inq = true;}}
					if (k == '{' && /*!inbs &&*/ !inq) { inb = true; z++; }
					if (k == '}' && /*!inbs &&*/ !inq) { inb = false; z--; }
					if (k == '[' && /*!inb &&*/ !inq) { inbs = true; q++; inbo = true; /*if (indblock) continue;*/ } else { inbo = false; }
					if (k == ']' && /*!inb &&*/ !inq) { if (q == 1) { v=0; } inbs = false; q--; inbc = true; } else { inbc = false; }
					if (k == ',' && z==0&&q==1 && !inq) { vals = true; v++; setmax(ref max_v, v); } else vals = false;
					if (z > zx) zx = z;
					if (v > vx) vx = v;
					if (q > qx) qx = q;
					if (indblock) {
						if (i == 0 && z != 0) {
							throw new Exception("Can't get value, not array. Use block name instead.");
						}
						if (vl == ind && vals && q == 1) {
							rawr = bln.ToString();
							break;
						}
						if (q >= 1)
							bln.Append(k);
						if (q == 1 && inbo) bln.Clear();
						if (vals)
							bln.Clear();
					} else {
						if (i == 0 && q != 0) {
							throw new Exception("Can't get value, not block. Use index instead. or "+DIGX+" shortcut.");
						}
						if (z == 1 && !nextv) {
							if (inqq) {
								inq = false;
								var l = rawr[i+1];
								if (l == ':') {
									bln = new StringBuilder(bln.ToString().Substring(1, bln.Length-1));
									Console.WriteLine("blockname: " +bln);
									if (bln.ToString() == blox) {
										nextv = true;
										bln.Clear();
										i++;
										sq = q;
										continue;
									}
								}
							}
						}
						if (nextv) {
							if ((k == ',' || i == f[1]-1)&& !inq && z == 1 && q == sq) {
								if (!inq && k != ',')
								bln.Append(k);
								break;
							}
							bln.Append(k);
						} else {
							if (inq) bln.Append(k); else bln.Clear();
						}
					}
					vl = v;
				}
			} else {
				throw new Exception("There are no block in that array.");
			}
			if (indblock && max_v < ind) throw new Exception("Out of index, max: " +max_v+", requested: "+ind);
			rawr = bln.ToString();
		}
		return rawr;
	}
}