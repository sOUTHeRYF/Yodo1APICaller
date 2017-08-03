using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Yodo1ServiceModels.DataSource.OnlineConfig;
namespace Yodo1ServiceModels.DataSource
{
    public class Yodo1DataSource
    {
        public IYodo1DataSource OnlineConfig => new OnlineConfigDataSource();
    }
}
