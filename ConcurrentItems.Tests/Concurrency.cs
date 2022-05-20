using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ConcurrentItems.Tests
{
  [TestClass]
  public class Concurrency
  {
    private static object sync = new();

    [TestMethod]
    public void Actions()
    {
      var x = new List<int> { 1, 10, 5, 25 };
      var y = new ConcurrentItems<int> { 1, 10, 5, 25 };
      var generator = new Random();

      Assert.AreEqual(x.Count, y.Count);

      Parallel.For(0, 1000, i =>
      {
        try
        {
          var constant = generator.Next();

          if (i % 10 == 0)
          {
            //Clear(x, y);
          }

          //Addition(x, y, constant);
          //Addition(x, y, generator.Next());

          //Deletion(x, y, constant);
          //Deletion(x, y, generator.Next());

          //IndexDeletion(x, y);
          //IndexDeletion(x, y);

          //Indices(x, y, constant);
          //Indices(x, y, generator.Next());

          //Insertion(x, y, constant);
          //Insertion(x, y, generator.Next());
        }
        catch (Exception e)
        {

        }
      });
    }

    private void Addition(IList<int> x, IList<int> y, int input)
    {
      y.Add(input);

      lock (sync)
      {
        x.Add(input);
        Assert.AreEqual(string.Join(",", x), string.Join(",", y));
      }
    }

    private void Clear(IList<int> x, IList<int> y)
    {
      y.Clear();

      lock (sync)
      {
        x.Clear();
        Assert.AreEqual(string.Join(",", x), string.Join(",", y));
      }
    }

    private void Deletion(IList<int> x, IList<int> y, int input)
    {
      y.Remove(input);

      lock (sync)
      {
        x.Remove(input);
        Assert.AreEqual(string.Join(",", x), string.Join(",", y));
      }
    }

    private void IndexDeletion(IList<int> x, IList<int> y)
    {
      var exceptionX = 0.GetType();
      var exceptionY = 0.ToString().GetType();

      // Remove in the middle

      y.RemoveAt(y.Count / 2);

      lock (sync)
      {
        x.RemoveAt(x.Count / 2);
        Assert.AreEqual(string.Join(",", x), string.Join(",", y));
      }

      // Remove first

      y.RemoveAt(0);

      lock (sync)
      {
        x.RemoveAt(0);
        Assert.AreEqual(string.Join(",", x), string.Join(",", y));
      }

      // Remove last

      y.RemoveAt(y.Count - 1);

      lock (sync)
      {
        x.RemoveAt(x.Count - 1);
        Assert.AreEqual(string.Join(",", x), string.Join(",", y));
      }
    }

    private void Indices(IList<int> x, IList<int> y, int input)
    {
      y[0] = 100 + input;
      y[2] = 20 + input;

      lock (sync)
      {
        x[0] = 100 + input;
        x[2] = 20 + input;

        Assert.AreEqual(string.Join(",", x), string.Join(",", y));
      }

      // Update all indexes 

      y[0] = 100 + input;
      y[1] = 50 + input;
      y[2] = 20 + input;
      y[3] = 0 + input;

      lock (sync)
      {
        x[0] = 100 + input;
        x[1] = 50 + input;
        x[2] = 20 + input;
        x[3] = 0 + input;

        Assert.AreEqual(string.Join(",", x), string.Join(",", y));
      }
    }

    private void Insertion(IList<int> x, IList<int> y, int input)
    {
      // Insert in the middle

      y.Insert(2, 30 + input);

      lock (sync)
      {
        x.Insert(2, 30 + input);
        Assert.AreEqual(string.Join(",", x), string.Join(",", y));
      }

      // Insert first

      y.Insert(0, 50 + input);

      lock (sync)
      {
        x.Insert(0, 50 + input);
        Assert.AreEqual(string.Join(",", x), string.Join(",", y));
      }

      // Insert last

      y.Insert(y.Count, 90 + input);

      lock (sync)
      {
        x.Insert(x.Count, 90 + input);
        Assert.AreEqual(string.Join(",", x), string.Join(",", y));
      }
    }
  }
}
