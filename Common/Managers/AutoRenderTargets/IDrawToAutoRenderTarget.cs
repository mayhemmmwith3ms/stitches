using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StitchesLib.Common.Managers.AutoRenderTargets
{
	public interface IDrawToRenderTarget
	{
		public void QueueDrawAction();
	}
}
