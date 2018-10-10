using System;
using System.Threading;
using System.Threading.Tasks;

namespace Assets.Scenes {
	internal class Repeater {
		public unsafe Repeater (SynchronizationContext context, bool* c, int FirstDelay, int TheRestDelay) {
			Condition = c;
			this.FirstDelay = FirstDelay;
			this.TheRestDelay = TheRestDelay;
			this.context = context;
		}

		private readonly SynchronizationContext context;
		public unsafe bool* Condition { get; set; }
		public int FirstDelay { get; set; }
		public int TheRestDelay { get; set; }

		public async void startWork () {
			await Task.Run(() => looping());
		}

		public event Action Act;

		private unsafe void looping () {
			context?.Send(onAct, null);
			Thread.Sleep(FirstDelay);
			while (*Condition) {
				context?.Send(onAct, null);
				Thread.Sleep(TheRestDelay);
			}
		}

		private void onAct (object b) {
			Act?.Invoke();
		}
	}
}
