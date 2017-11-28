using System;
using System.Linq;
using UnityEngine;

namespace PhaserArray.PaycheckPlugin.Helpers
{
	public class Vector3Helper
	{
		private static readonly string[] ParseStripSymbols = {"[", "]", "(", ")", "{", "}", "<", ">", " "};

		/// <summary>
		/// Attempts to parse a string input as a vector. Strips normal, square, curly and angle brackets as well as spaces.
		/// </summary>
		/// <param name="input"></param>
		/// <param name="delimiter"></param>
		/// <returns></returns>
		public static Vector3? Parse(string input, char delimiter=(char)44)
		{
			input = ParseStripSymbols.Aggregate(input, (current, symbol) => current.Replace(symbol, string.Empty));
			var splitInput = input.Split(Convert.ToChar(","));
			if (splitInput.Length != 3) return null;
			var cords = new float[3];
			for (var i = 0; i < 3; i++)
			{
				if (float.TryParse(splitInput[i], out var cord))
				{
					cords[i] = cord;
				}
				else
				{
					return null;
				}
			}
			return new Vector3(cords[0], cords[1], cords[2]);
		}

		/// <summary>
		/// Uses Mathf.Round to round the provided vector.
		/// </summary>
		/// <param name="vector"></param>
		/// <returns>Rounded Vector</returns>
		public static Vector3 Round(Vector3 vector)
		{
			return new Vector3(Mathf.Round(vector.x), Mathf.Round(vector.y), Mathf.Round(vector.z));
		}
	}
}
