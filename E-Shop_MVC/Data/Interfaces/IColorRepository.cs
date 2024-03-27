using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;

namespace E_Shop_MVC.Models.Data.Interfaces
{
    public interface IColorRepository
    {
        IEnumerable<ProductColor> GetAll();
        ProductColor GetById(int id);
    }
}
