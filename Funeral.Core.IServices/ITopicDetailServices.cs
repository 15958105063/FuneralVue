﻿using Funeral.Core.IServices.BASE;
using Funeral.Core.Model.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Funeral.Core.IServices
{
    public interface ITopicDetailServices : IBaseServices<TopicDetail>
    {
        Task<List<TopicDetail>> GetTopicDetails();
    }
}
