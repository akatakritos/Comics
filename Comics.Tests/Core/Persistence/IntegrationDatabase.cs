using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Comics.Core.Persistence;

namespace Comics.Tests.Core.Persistence
{
    class IntegrationDatabase
    {
        private IntegrationDatabase(string file)
        {
            ConnectionString = $"DataSource={file}";
            DatabaseFilePath = file;
            var t = typeof(System.Data.SqlServerCe.AddOption);
            Debug.WriteLine(t);
        }

        public string ConnectionString { get; }
        public string DatabaseFilePath { get; }

        public ComicsContext CreateContext()
        {
            return new ComicsContext(ConnectionString);
        }

        public static IntegrationDatabase CreateBlankDatabase()
        {
            var db = new IntegrationDatabase(Path.GetTempFileName());

            if (!File.Exists(db.DatabaseFilePath))
            {
                File.Create(db.DatabaseFilePath).Close();
            }

            return db;
        }
    }
}
