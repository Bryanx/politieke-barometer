using BAR.BL.Domain;
using BAR.BL.Domain.Core;
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
		Customization ReadCustomization(string subplatformName);

		//Create
		int CreateSubplatform(SubPlatform subPlatform);

		//Update
		int UpdateSubplatform(SubPlatform subPlatform);
		int UpdateSubplatforms(IEnumerable<SubPlatform> subPlatforms);

		//Delete
		int DeleteSubplatform(SubPlatform subPlatform);
	}
}
