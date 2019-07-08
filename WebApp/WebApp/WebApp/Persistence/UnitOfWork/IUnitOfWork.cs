using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApp.Persistence.Repository;

namespace WebApp.Persistence.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        int Complete();

        ITicketRepository Tickets { get; set; }
        IPricesRepository Prices { get; set; }
        ILineRepository Lines { get; set; }
        IDiscountRepository Discounts { get; set; }

        IScheduleRepository Schedules { get; set; }

        IUserRepository Users { get; set; }
        IPictureRepository Pictures { get; set; }

        IDepatureRepository Depatures { get; set; }

        IStationRepository Stations { get; set; }

        ILocationRepository Locations { get; set; }

        IPayPalInfoRepository PayPalInfos { get; set; }
    }
}
