using API.DAL.Entity.Models;

namespace API.Services.ForAPI.Int
{
	public interface IAd_files_Service
	{
		Task<Adfile> getAdfile(string id);
	}
}
