using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace SimplCommerce.Core.Infrastructure.EntityFramework
{
    public class SqlRepository : ISqlRepository
    {
        private readonly DbContext dbContext;

        public SqlRepository(SimplDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public void RunCommand(string command)
        {
            dbContext.Database.ExecuteSqlCommand(command);
        }

        public void RunCommands(IEnumerable<string> commands)
        {
            int counter = 0;
            try
            {
                foreach (var command in commands)
                {
                    counter++;
                    if (!string.IsNullOrEmpty(command.ToString()))
                    {
                        dbContext.Database.ExecuteSqlCommand(command);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public IEnumerable<string> ParseCommand(IEnumerable<string> lines)
        {
            var sb = new StringBuilder();
            var commands = new List<string>();
            foreach (var line in lines)
            {
                if (string.Equals(line, "GO", StringComparison.OrdinalIgnoreCase))
                {
                    if (sb.Length > 0)
                    {
                        commands.Add(sb.ToString());
                        sb = new StringBuilder();
                    }
                }
                else
                {
                    if (!string.IsNullOrWhiteSpace(line))
                    {
                        sb.Append(line);
                    }
                }
            }

            return commands;
        }
    }
}