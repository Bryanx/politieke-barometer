using BAR.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAR.BL.Managers
{
	public class UnitOfWorkManager
	{
		/// <summary>
		/// Member om DAL.UnitOfWork in bij te houden
		/// die op zich dan weer de te gebruiken instantie van onze
		/// Context klasse in bijhoudt
		/// </summary>
		private UnitOfWork uof;

		internal UnitOfWork UnitOfWork
		{
			get
			{   
				//Om via buitenaf te verzekeren dat er géén onnodige nieuwe
				//instanaties van UnitOfWork geïnstantieerd worden...
				if (uof == null) uof = new UnitOfWork();
				return uof;
			}
		}

		/// <summary>
		/// Commits changes to the database when
		/// when unit of work is used.
		/// </summary>
		public void Save()
		{
			UnitOfWork.CommitChanges();
		}
	}
}
