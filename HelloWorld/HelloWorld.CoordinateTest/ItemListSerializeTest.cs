using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using System.Linq;

namespace HelloWorld.Test
{
  [TestClass]
  public class ItemListSerializeTest
  {

    public ItemListSerializeTest()
    {
      GameEnvironment.Initialize( @"C:\Data\Game" );
    }


    [TestMethod]
    public void SerializeTest()
    {

      var item1 = new Item( GameEnvironment.GetDataItem<ItemDescriptor>( Guid.Parse( "{5962E351-33CA-4968-90A3-8AF6C8847180}" ) ), 100 );
      var collection = new ItemCollection();

      collection.AddItems( item1 );

      var data = JsonConvert.DeserializeObject<ItemCollection>( JsonConvert.SerializeObject( collection ) );

      Assert.AreEqual( data.First(), item1 );
    }
  }
}
