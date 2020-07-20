using Funeral.Core.IServices.BASE;
using Funeral.Core.Model;
using Funeral.Core.Model.Models;
using Funeral.Core.Model.ViewModels;
using System.Threading.Tasks;

namespace Funeral.Core.IServices
{
	/// <summary>
	/// IQuestionsServices
	/// </summary>	
	public interface IQuestionsServices : IBaseServices<Questions>
	{

		Task<bool> PostQuestionInfo(QuestionsDto modeldto);
		Task<PageModel<QuestionsDto>> GetListByPage(int pageindex = 1, int pagesize = 50, string orderby = "", string key = "");
	}
}