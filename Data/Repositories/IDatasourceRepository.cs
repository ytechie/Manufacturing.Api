using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Manufacturing.Api.Data.Model;
using Manufacturing.Framework.Dto;

namespace Manufacturing.Api.Data.Repositories
{
    public interface IDatasourceRepository
    {
        IEnumerable<DatasourceConfiguration> GetAll();
    }
}
