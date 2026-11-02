using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace DA.Comunes
{
    public class ErroresSql
    {

        public static bool EsDuplicado(DbUpdateException ex) =>
            ex.InnerException is SqlException { Number: 2601 or 2627 };

        /// <summary>547: violación de llave foránea.</summary>
        public static bool EsLlaveForanea(DbUpdateException ex) =>
            ex.InnerException is SqlException { Number: 547 };

    }
}
