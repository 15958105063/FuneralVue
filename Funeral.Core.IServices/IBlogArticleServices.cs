using Funeral.Core.IServices.BASE;
using Funeral.Core.Model.Models;
using Funeral.Core.Model.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Funeral.Core.IServices
{
    public interface IBlogArticleServices :IBaseServices<BlogArticle>
    {
        Task<List<BlogArticle>> GetBlogs();
        Task<BlogViewModels> GetBlogDetails(int id);

    }

}
