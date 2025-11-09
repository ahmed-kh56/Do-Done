using DoDone.Application.Queries.ManagerActions.GetEmployees;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoDone.Application.Common.Dtos.UserDtos
{
    public record GetEmployeesRequest(int? PageNum, int? PageSize)
    {
        public GetEmployeesQuery ToQuery()
        {
            return new GetEmployeesQuery(PageNum ?? 0, PageSize ?? 12);
        }
    }


}
