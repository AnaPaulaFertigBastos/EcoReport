using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using EcoReport.Models;

namespace EcoReport.Data
{
    public class EcoReportContext : DbContext
    {
        public EcoReportContext(DbContextOptions<EcoReportContext> options) : base(options)
        {
        }

    }
}
