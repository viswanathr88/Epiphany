﻿
namespace Epiphany.ViewModel
{
    public interface IBookViewModel : IDataViewModel
    {
        int Id
        {
            get;
            set;
        }

        string Name
        {
            get;
            set;
        }
    }
}
