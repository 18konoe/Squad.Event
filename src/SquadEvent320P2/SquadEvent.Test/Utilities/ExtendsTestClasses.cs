using System;
using System.Collections.Generic;
using SquadEvent.Shared.Utilities;

namespace SquadEvent.Test.Utilities
{
    public interface ITestElementClass : IValueEquatable<ITestElementClass>
    {
        DateTimeOffset Date { get; set; }
        TestEnum Status { get; set; }
    }
    public class TestElementClass : ITestElementClass
    {
        public TestElementClass(DateTimeOffset date, TestEnum status)
        {
            Date = date;
            Status = status;
        }
        public DateTimeOffset Date { get; set; }
        public TestEnum Status { get; set; }

        public bool ValueEquals(ITestElementClass other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Date.Equals(other.Date) && Status == other.Status;
        }
    }

    public interface ITestValueClass : IValueEquatable<ITestValueClass>
    {
        string Name { get; set; }
        IEnumerable<ITestElementClass> Elements { get; set; }
    }

    public class TestValueClass : ITestValueClass
    {
        public string Name { get; set; } = string.Empty;
        public IEnumerable<ITestElementClass> Elements { get; set; } = new List<ITestElementClass>();

        public bool ValueEquals(ITestValueClass other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            if (Name != other.Name) return false;
            if (!Elements.ValueEquals(other.Elements)) return false;
            return true;
        }
    }

    public interface ITestSurfaceClass : IValueEquatable<ITestSurfaceClass>
    {
        string Name { get; set; }
        DateTimeOffset? Date { get; set; }
        IDictionary<string, ITestValueClass> Values { get; set; }
    }

    public class TestSurfaceClass : ITestSurfaceClass
    {
        public string Name { get; set; } = string.Empty;
        public DateTimeOffset? Date { get; set; }
        public IDictionary<string, ITestValueClass> Values { get; set; } = new Dictionary<string, ITestValueClass>();

        public bool ValueEquals(ITestSurfaceClass other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            if (Name != other.Name) return false;
            if (!Nullable.Equals(Date, other.Date)) return false;
            if (!Values.ValueEquals(other.Values)) return false;
            return true;
        }
    }

    public enum TestEnum
    {
        Test1,
        Test2
    }
}