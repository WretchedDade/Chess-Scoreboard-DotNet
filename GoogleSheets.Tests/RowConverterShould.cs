using System;
using System.Collections.Generic;
using System.Linq;
using GoogleSheets.Tests.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GoogleSheets.Tests
{
    [TestClass]
    public class RowConverterShould
    {
        [TestMethod]
        public void CreateObjectFromRow()
        {
            var row = new List<object> { 1, "Dade", "Cook" } as IList<object>;
            string[] columnNames = new string[] { "Id", "First Name", "Last Name" };


            Person person = RowConverter.FromRow<Person>(row, columnNames);

            Assert.IsNotNull(person);
            Assert.AreEqual(1, person.Id);
            Assert.AreEqual("Dade", person.FirstName);
            Assert.AreEqual("Cook", person.LastName);
        }

        [TestMethod]
        public void ThrowExceptionIfNumberOfColumnNamesIsDifferentThanColumnsInRow()
        {
            var row = new List<object> { 1, "Dade", "Cook" } as IList<object>;
            string[] columnNames = new string[] { "Id", "First Name", "Last Name", "Date Of Birth" };

            Assert.ThrowsException<InvalidOperationException>(() => RowConverter.FromRow<Person>(row, columnNames));
        }

        [TestMethod]
        public void CreateObjectsFromRows()
        {
            var rows = new List<IList<object>>
            {
                new List<object> { 1, "Dade", "Cook" },
                new List<object> { 2, "Carla", "Cook" },
                new List<object> { 3, "Lane", "Cook" },
                new List<object> { 4, "Mary", "Cook" },
                new List<object> { 5, "Lee", "Cook" }
            } as IList<IList<object>>;

            string[] columnNames = new string[] { "Id", "First Name", "Last Name" };

            var people = RowConverter.FromRows<Person>(rows, columnNames).ToList();

            Assert.IsNotNull(people);
            Assert.AreEqual(5, people.Count());
            CollectionAssert.AllItemsAreNotNull(people);
            CollectionAssert.AllItemsAreUnique(people);

            rows = new List<IList<object>>
            {
                new List<object> { "Id", "First Name", "Last Name" },
                new List<object> { 1, "Dade", "Cook" },
                new List<object> { 2, "Carla", "Cook" },
                new List<object> { 3, "Lane", "Cook" },
                new List<object> { 4, "Mary", "Cook" },
                new List<object> { 5, "Lee", "Cook" }
            } as IList<IList<object>>;

            people = RowConverter.FromRows<Person>(rows).ToList();
            Person person = people[2];

            Assert.IsNotNull(people);
            Assert.AreEqual(5, people.Count());

            Assert.AreEqual(3, person.Id);
            Assert.AreEqual("Lane", person.FirstName);
            Assert.AreEqual("Cook", person.LastName);

            CollectionAssert.AllItemsAreNotNull(people);
            CollectionAssert.AllItemsAreUnique(people);
        }

        [TestMethod]
        public void CreateRowFromObject()
        {
            var person = new Person { Id = 1, FirstName = "Dade", LastName = "Cook" };

            IList<object> row = RowConverter.ToRow(person);

            Assert.IsNotNull(row);

            object id = row[0];
            object fName = row[1];
            object lName = row[2];

            Assert.IsInstanceOfType(id, typeof(int));
            Assert.IsInstanceOfType(fName, typeof(string));
            Assert.IsInstanceOfType(lName, typeof(string));

            Assert.AreEqual(1, id);
            Assert.AreEqual("Dade", fName);
            Assert.AreEqual("Cook", lName);
        }

        [TestMethod]
        public void CreateRowsFromObjects()
        {
            var people = new List<Person>
            {
                new Person { Id = 1, FirstName = "Dade", LastName = "Cook" },
                new Person { Id = 2, FirstName = "Carla", LastName = "Cook" },
                new Person { Id = 3, FirstName = "Lane", LastName = "Cook" }
            };

            var rows = RowConverter.ToRows(people, true).ToList();

            IList<object> headers = rows[0];
            IList<object> row = rows[2];

            Assert.IsNotNull(rows);
            Assert.AreEqual(4, rows.Count());

            Assert.AreEqual("Id", headers[0]);
            Assert.AreEqual("First Name", headers[1]);
            Assert.AreEqual("Last Name", headers[2]);

            Assert.AreEqual(2, row[0]);
            Assert.AreEqual("Carla", row[1]);
            Assert.AreEqual("Cook", row[2]);

            CollectionAssert.AllItemsAreNotNull(rows);
            CollectionAssert.AllItemsAreUnique(rows);

            rows = RowConverter.ToRows(people, false).ToList();
            row = rows[2];

            Assert.IsNotNull(rows);
            Assert.AreEqual(3, rows.Count());

            Assert.AreEqual(3, row[0]);
            Assert.AreEqual("Lane", row[1]);
            Assert.AreEqual("Cook", row[2]);

            CollectionAssert.AllItemsAreNotNull(rows);
            CollectionAssert.AllItemsAreUnique(rows);
        }
    }
}
