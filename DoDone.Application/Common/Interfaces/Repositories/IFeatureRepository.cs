using DoDone.Domain.Features;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoDone.Application.Common.Interfaces.Repositories
{
    public interface IFeatureRepository
    {
        Task UnassaignAllAsync(
            Guid? featureId = null,
            Guid? purId = null,
            Guid? projectId = null,
            Guid? userId = null,
            bool onlyNotCompleted = true,
            IDbConnection dbConnection = null,
            IDbTransaction dbTransaction = null);
    }

}
