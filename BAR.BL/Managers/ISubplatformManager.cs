using BAR.BL.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAR.BL.Managers
{
	public interface ISubplatformManager
	{
		//Subplatsorms
		SubPlatform GetSubPlatform(string subplatformName);
		IEnumerable<SubPlatform> GetSubplatforms();

		SubPlatform CreateSubplatform(string name);

		//Questions
	}
}
