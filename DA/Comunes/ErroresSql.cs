using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace DA.Comunes
{
    internal static class ErroresSql
    {
        public static bool EsDuplicado(DbUpdateException ex) =>
            ex.InnerException is SqlException { Number: 2601 or 2627 };

        public static bool EsLlaveForanea(DbUpdateException ex) =>
            ex.InnerException is SqlException { Number: 547 };
    }
}
