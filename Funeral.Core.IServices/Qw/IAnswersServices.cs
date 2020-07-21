using Funeral.Core.IServices.BASE;
using Funeral.Core.Model.Models;
using Funeral.Core.Model.ViewModels;
using System.Threading.Tasks;

namespace Funeral.Core.IServices
{
	/// <summary>
	/// IAnswersServices
	/// </summary>	
	public interface IAnswersServices : IBaseServices<Answers>
	{
		Task<bool> PostAnswersInfo(AnswersDto modeldto);
	}
}