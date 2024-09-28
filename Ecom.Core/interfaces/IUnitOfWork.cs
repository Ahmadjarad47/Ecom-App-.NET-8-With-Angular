using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecom.Core.interfaces
{
    public interface IUnitOfWork
    {
        public ICategoryRepositry CategoryRepositry { get; }
        public IPhotoRepositry PhotoRepositry { get; }
        public IProductRepositry productRepositry { get; }
        public ICustomerBasketRepositry CustomerBasket { get; }
        public IAuth Auth { get; }
    }
}
