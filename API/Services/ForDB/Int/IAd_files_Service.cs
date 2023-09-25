using API.DAL.Entity.Models;

namespace API.Services.ForAPI.Int
{
	public interface IAd_files_Service
	{
		Task<Adfile> Getfile(string id);

		Task <List<Adfile>> Getfiles(List<string> id);

		Task<Adfile> AddFile(IFormFile file, string login);


	}
}
