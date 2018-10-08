using System;
using System.Threading;
using System.Threading.Tasks;

namespace Assets.Scenes {
	internal class Repeater {
		public unsafe Repeater (bool* c, int FirstDelay, int TheRestDelay) {
			Condition = c;
			this.FirstDelay = FirstDelay;
			this.TheRestDelay = TheRestDelay;
		}

		public unsafe bool* Condition { get; set; }
		public int FirstDelay { get; set; }
		public int TheRestDelay { get; set; }

		public async void startWork () {
			await Task.Run(() => looping());
		}

		public event Action Act;

		private unsafe void looping () {
			Act?.Invoke();
			Thread.Sleep(FirstDelay);
			while (*Condition) {
				Act?.Invoke();
				Thread.Sleep(TheRestDelay);
			}
		}
	}
}
