using System;
using System.Collections.Generic;

namespace ConcurrentItems.Tests
{
  [TestClass]
  public class Comparison
  {
    [TestMethod]
    public void Count()
    {
      var x = new List<int> { 1, 10, 5, 25 };
      var y = new V2<int> { 1, 10, 5, 25 };

      Assert.AreEqual(x.Count, y.Count);
    }

    [TestMethod]
    public void Creation()
    {
      var x = new List<int> { 1, 10, 5, 25 };
      var y = new V2<int> { 1, 10, 5, 25 };

      Assert.AreEqual(string.Join(",", x), string.Join(",", y));
    }

    [TestMethod]
    public void Addition()
    {
      var x = new List<int> { 1, 10, 5, 25 };
      var y = new V2<int> { 1, 10, 5, 25 };

      x.Add(100);
      y.Add(100);

      Assert.AreEqual(string.Join(",", x), string.Join(",", y));
    }

    [TestMethod]
    public void Clear()
    {
      var x = new List<int> { 1, 10, 5, 25 };
      var y = new V2<int> { 1, 10, 5, 25 };

      x.Clear();
      y.Clear();

      Assert.AreEqual(string.Join(",", x), string.Join(",", y));
    }

    [TestMethod]
    public void Deletion()
    {
      var x = new List<int> { 1, 10, 5, 25 };
      var y = new V2<int> { 1, 10, 5, 25 };

      // Remove non-existent

      x.Remove(100);
      y.Remove(100);

      Assert.AreEqual(string.Join(",", x), string.Join(",", y));

      // Remove in the middle

      x.Remove(5);
      y.Remove(5);

      Assert.AreEqual(string.Join(",", x), string.Join(",", y));

      // Remove first

      x.Remove(1);
      y.Remove(1);

      Assert.AreEqual(string.Join(",", x), string.Join(",", y));

      // Remove last

      x.Remove(25);
      y.Remove(25);

      Assert.AreEqual(string.Join(",", x), string.Join(",", y));
    }

    [TestMethod]
    public void IndexDeletion()
    {
      var x = new List<int> { 1, 10, 5, 25 };
      var y = new V2<int> { 1, 10, 5, 25 };

      var exceptionX = 0.GetType();
      var exceptionY = 0.ToString().GetType();

      // Out of range index 

      try { x.RemoveAt(-1); } catch (Exception e) { exceptionX = e.GetType(); }
      try { y.RemoveAt(-1); } catch (Exception e) { exceptionY = e.GetType(); }

      Assert.AreEqual(exceptionX, exceptionY);
      Assert.AreEqual(string.Join(",", x), string.Join(",", y));

      exceptionX = 0.GetType();
      exceptionY = 0.ToString().GetType();

      try { x.RemoveAt(x.Count); } catch (Exception e) { exceptionX = e.GetType(); }
      try { y.RemoveAt(y.Count); } catch (Exception e) { exceptionY = e.GetType(); }

      Assert.AreEqual(exceptionX, exceptionY);
      Assert.AreEqual(string.Join(",", x), string.Join(",", y));

      // Remove in the middle

      x.RemoveAt(2);
      y.RemoveAt(2);

      Assert.AreEqual(string.Join(",", x), string.Join(",", y));

      // Remove first

      x.RemoveAt(0);
      y.RemoveAt(0);

      Assert.AreEqual(string.Join(",", x), string.Join(",", y));

      // Remove last

      x.RemoveAt(x.Count - 1);
      y.RemoveAt(y.Count - 1);

      Assert.AreEqual(string.Join(",", x), string.Join(",", y));
    }

    [TestMethod]
    public void Indices()
    {
      var x = new List<int> { 1, 10, 5, 25 };
      var y = new V2<int> { 1, 10, 5, 25 };

      var exceptionX = 0.GetType();
      var exceptionY = 0.ToString().GetType();

      // Out of range index 

      try { x[-1] = 0; } catch (Exception e) { exceptionX = e.GetType(); }
      try { y[-1] = 0; } catch (Exception e) { exceptionY = e.GetType(); }

      Assert.AreEqual(exceptionX, exceptionY);
      Assert.AreEqual(string.Join(",", x), string.Join(",", y));

      exceptionX = 0.GetType();
      exceptionY = 0.ToString().GetType();

      try { x[x.Count] = 0; } catch (Exception e) { exceptionX = e.GetType(); }
      try { y[y.Count] = 0; } catch (Exception e) { exceptionY = e.GetType(); }

      Assert.AreEqual(exceptionX, exceptionY);
      Assert.AreEqual(string.Join(",", x), string.Join(",", y));

      // Update some of existing indexes

      x[0] = 100;
      x[2] = 20;

      y[0] = 100;
      y[2] = 20;

      Assert.AreEqual(string.Join(",", x), string.Join(",", y));

      // Update all indexes 

      x[0] = 100;
      x[1] = 50;
      x[2] = 20;
      x[3] = 0;

      y[0] = 100;
      y[1] = 50;
      y[2] = 20;
      y[3] = 0;

      Assert.AreEqual(string.Join(",", x), string.Join(",", y));
    }

    [TestMethod]
    public void Insertion()
    {
      var x = new List<int> { 1, 10, 5, 25 };
      var y = new V2<int> { 1, 10, 5, 25 };

      var exceptionX = 0.GetType();
      var exceptionY = 0.ToString().GetType();

      // Out of range index 

      try { x.Insert(-1, 0); } catch (Exception e) { exceptionX = e.GetType(); }
      try { y.Insert(-1, 0); } catch (Exception e) { exceptionY = e.GetType(); }

      Assert.AreEqual(exceptionX, exceptionY);
      Assert.AreEqual(string.Join(",", x), string.Join(",", y));

      exceptionX = 0.GetType();
      exceptionY = 0.ToString().GetType();

      try { x.Insert(x.Count + 1, 0); } catch (Exception e) { exceptionX = e.GetType(); }
      try { y.Insert(y.Count + 1, 0); } catch (Exception e) { exceptionY = e.GetType(); }

      Assert.AreEqual(exceptionX, exceptionY);
      Assert.AreEqual(string.Join(",", x), string.Join(",", y));

      // Insert in the middle

      x.Insert(2, 30);
      y.Insert(2, 30);

      Assert.AreEqual(string.Join(",", x), string.Join(",", y));

      // Insert first

      x.Insert(0, 50);
      y.Insert(0, 50);

      Assert.AreEqual(string.Join(",", x), string.Join(",", y));

      // Insert last

      x.Insert(x.Count, 90);
      y.Insert(y.Count, 90);

      Assert.AreEqual(string.Join(",", x), string.Join(",", y));
    }
  }
}
