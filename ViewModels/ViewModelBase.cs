using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContentRate.ViewModels
{
    public class ViewModelBase : ReactiveObject
    {
        [Reactive]
        public bool IsBusy { get; protected set; }
        
    }
}
