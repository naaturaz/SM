using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

public class Test
{
    Naming n = new Naming(H.Male);

    [SetUp]
    public void Init()
    {
        
    }

    [Test]
    public void TestName()
    {
        Assert.AreEqual("nana", n.NewName());
    }

    [Test]
    public void TestName2()
    {
        for (int i = 0; i < 10; i++)
        {n.NewName();}
        Assert.AreEqual(10, Naming.PeoplesName);
    }

}
