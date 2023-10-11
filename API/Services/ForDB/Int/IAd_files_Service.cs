using API.DAL.Entity.Models;
using System.Runtime.CompilerServices;

namespace API.Services.ForAPI.Int
{
	public interface IAd_files_Service
	{
		Task<Adfile> Getfile(string id);

		Task <List<Adfile>> Getfiles(List<string> id);

		Task<Adfile> AddFile(IFormFile file, string login);

		Task<bool> DeleteFile(string id, string login);


	}
}
