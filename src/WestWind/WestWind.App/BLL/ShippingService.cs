using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WestWind.App.DAL;
using WestWind.App.Models.CRUD;

namespace WestWind.App.BLL
{
    public class ShippingService
    {
        #region Constructor and Dependencies
        private readonly WestwindContext _context;

        internal ShippingService(WestwindContext context)
        {
            _context = context;
        }
        #endregion

        #region CRUD Shippers
        public List<Shipper> ListShippers()
        {
            var result = from company in _context.Shippers
                         select new Shipper
                         {
                             ID = company.ShipperId,
                             CompanyName = company.CompanyName,
                             Phone = company.Phone
                         };
            return result.ToList();
        }

        public void AddShipper(Shipper data)
        {
            // TODO: Make sure there are no duplicate shippers
            _context.Shippers.Add(new Entities.Shipper
            {
                CompanyName = data.CompanyName,
                Phone = data.Phone
            });

            _context.SaveChanges();
        }

        public void UpdateShipper(Shipper data)
        {
            throw new NotImplementedException($"TODO: {nameof(UpdateShipper)}() has not been implemented.");
        }

        public void DeleteShipper(int shipperId)
        {
            throw new NotImplementedException($"TODO: {nameof(DeleteShipper)}() has not been implemented.");
        }
        #endregion
    }
}
