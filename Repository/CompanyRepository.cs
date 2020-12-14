﻿using Dapper;
using DapperDemo.Data;
using DapperDemo.Models;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace DapperDemo.Repository
{
    public class CompanyRepository : ICompanyRepository
    {

        private IDbConnection db;
        public CompanyRepository(IConfiguration configuration)
        {
            this.db = new SqlConnection(configuration.GetConnectionString("DefaultConnection"));
        }
        public Company Add(Company company)
        {
            string sql = "INSERT INTO Companies (Name, Address, City, State, PostalCode) VALUES (@Name, @Address, @City, @State, @PostalCode);"
                + "SELECT CAST(SCOPE_IDENTITY() as int);";

            var id = db.Query<int>(sql, new 
            { 
                @Name = company.Name, 
                @Address = company.Address, 
                @City = company.City, 
                @State = company.State, 
                @PostalCode = company.PostalCode 
            }).Single();

            company.CompanyId = id;
            return company;
        }

        public Company Find(int id)
        {
            string sql = "SELECT * FROM Companies WHERE CompanyId = @CompanyId";
            return db.Query<Company>(sql, new { @CompanyId = id }).Single();
        }

        public List<Company> GetAll()
        {
            string sql = "SELECT * FROM Companies";
            return db.Query<Company>(sql).ToList();
        }

        public void Remove(int id)
        {
            string sql = "DELETE FROM Companies WHERE CompanyId = @Id";
            db.Execute(sql, new { @Id = id });
        }

        public Company Update(Company company)
        {
            string sql = "UPDATE Companies SET Name = @Name, Address = @Address, City = @City, State = @State, PostalCode = @PostalCode WHERE CompanyId = @CompanyId";
            db.Execute(sql, new { @CompanyId = company.CompanyId });
            return company;
        }
    }
}
