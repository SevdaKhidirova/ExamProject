using examPro.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace examPro.ViewModels
{
    public class HomeViewModel : BaseViewModel
    {
        public List<Service> services { get; set; }
    }
}
