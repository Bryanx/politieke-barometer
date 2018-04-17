using BAR.BL.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAR.DAL
{
	public interface ISubplatformRepository
	{
		//Read
		SubPlatform ReadSubPlatform(string subplatformName);
		IEnumerable<SubPlatform> ReadSubPlatform();

		//Create
		//...

		//Update
		//...

		//Delete
		//...
	}
}
