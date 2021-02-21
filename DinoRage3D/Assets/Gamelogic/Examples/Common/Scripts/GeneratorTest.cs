using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace Gamelogic.Examples
{
	public class GeneratorTest : GLMonoBehaviour
	{
		private class RandomIntGenerator : IGenerator<int>
		{
			private int max;
			public RandomIntGenerator(int max)
			{
				this.max = max;
			}

			public int Next()
			{
				return GLRandom.Range(max);
			}

			object IGenerator.Next()
			{
				return Next();
			}
		}

		public List<Color> colors;
		public Image buffer0;
		public Image buffer1;
		public Image buffer2;

		public Image current;
			
		private Buffer<int> colorIndexGenerator;

		public void Start()
		{
			colorIndexGenerator = new Buffer<int>(new RandomIntGenerator(colors.Count), 3);
			Next();
		}

		public void Next()
		{
			var nextItem = colorIndexGenerator.Next();
			var comingUp = colorIndexGenerator.PeekAll().ToList();

			current.color = colors[nextItem];
			buffer0.color = colors[comingUp[0]];
			buffer1.color = colors[comingUp[1]];
			buffer2.color = colors[comingUp[2]];
		}
	}
}