﻿using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Xunit;


namespace Acr.EfExtensions.Tests
{

    public class DataExtensionTests
    {

        [Fact]
        public async Task PropertyTests()
        {
            var dbName = $"{nameof(this.PropertyTests)}.db";

            await this.Execute(dbName, async db =>
            {
                db.SqliteEntities.Add(new SqliteEntity
                {
                    NullId = 1,
                    SetId = 1,
                    Date = DateTime.Now,
                    Offset = DateTimeOffset.UtcNow
                });

                db.SqliteEntities.Add(new SqliteEntity
                {
                    SetId = 2,
                    Date = DateTime.Now,
                    Offset = DateTimeOffset.UtcNow
                });
                db.SaveChanges();
            });

            using (var db = new SqliteDbContext(dbName))
            {
                db.SqliteEntities.First(x => x.NullId == 1).Should().NotBeNull("NullId should be set but wasn't");

                var objs = await db.ReflectionExecuteToList<SqliteDto>("SELECT * FROM SqliteEntities ORDER BY Id");
                var e = objs.First();
                e.NullId.Should().Be(1);
                e.SetId.Should().Be(1);

                e = objs.Last();
                e.NullId.Should().BeNull();
                e.SetId.Should().Be(2);

                e.Date.Date.Should().Be(DateTime.Now.Date);
                e.Offset.Date.Should().Be(DateTimeOffset.UtcNow.Date);
            }
        }


        [Fact]
        public async Task ParameterizedSql()
        {
            await this.Execute("parameterized_sql.db", async db =>
            {
                using (var reader = await db.ExecuteReader("SELECT * FROM Items WHERE Id = @Id", CancellationToken.None, ("Id", 1)))
                {
                    reader.Read();
                }
            });
        }


        [Fact]
        public async Task ReflectionExecuteToList()
        {
            await this.Execute("executetolist.db", async db =>
            {
                // TODO: different property types
                // TODO: nullable property types
                // TODO: missed properties
                // TODO: partial data sets
                await db.ReflectionExecuteToList<TestItem>("SELECT * FROM Items");
            });
        }


        async Task Execute(string dbName, Func<SqliteDbContext, Task> func)
        {
            using (var db = new SqliteDbContext(dbName))
            {
                db.Database.EnsureDeleted();
                db.Database.EnsureCreated();
                await func(db);
            }
        }
    }
}
