﻿using System;
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
    }
}
