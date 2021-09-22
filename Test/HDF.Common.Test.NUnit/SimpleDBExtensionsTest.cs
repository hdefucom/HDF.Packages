using NUnit.Framework;
using System;
using System.Data;
using System.Data.SqlClient;

namespace HDF.Common.Test.NUnit
{
    public class SimpleDBExtensionsTest
    {



        [Test]
        public void SimpleDBTest()
        {
#if NET40
            string version = "_NET40";
#elif NETCOREAPP2_1
            string version = "_NETCORE21";
#elif NETCOREAPP3_1
            string version = "_NETCORE31";
#elif NET5_0
            string version = "_NET5";
#endif
            if (SimpleDBExtensions.DbProviderFactory == null)
                Assert.Throws<NullReferenceException>(() => string.Empty.ExecuteNonQuery());

            SimpleDBExtensions.Init(SqlClientFactory.Instance, "Data Source=.;Initial Catalog=HDFTest;Integrated Security=True");


            var deltablesql = $@"
if exists(select * from sysObjects where name='simple_test{version}' and xtype='U' )
begin
 drop table simple_test{version};
end

CREATE TABLE [dbo].[simple_test{version}](
	[id] [int] NULL
) ON [PRIMARY]

select '1' 
";

            Assert.NotNull(deltablesql.ExecuteScalar());

            Assert.AreEqual(1, $"insert into simple_test{version} values('1')".ExecuteNonQuery());

            var param = "@id".CreateParameter("1");
            using var reader = $"select * from simple_test{version} where '1'=@id".ExecuteReader(CommandType.Text, param);
            Assert.True(reader.Read());
            Assert.AreEqual("1", reader["id"].ToString());

            param = "@id".CreateParameter("1");
            var dt = $"select * from simple_test{version} where '1'=@id".ExecuteAdapter(CommandType.Text, param);
            Assert.AreEqual("1", dt.Rows[0][0].ToString());

            param = "@id".CreateParameter("1");
            Assert.AreEqual(1, $"delete from simple_test{version} where '1'=@id".ExecuteNonQueryInTran(i => i > 0, CommandType.Text, param));
            Assert.AreEqual(1, $"insert into simple_test{version} values('2')".ExecuteNonQueryInTran(i => false));

            param = "@id".CreateParameter("1", DbType.String);
            Assert.True($"delete from simple_test{version} where '1'=@id".ExecuteNonQuery(CommandType.Text, param) > 0);

        }


        [Test]
        public void ErrorTest()
        {
            Assert.Throws<ArgumentNullException>(() => SimpleDBExtensions.Init(null, null));
            Assert.Throws<ArgumentNullException>(() => SimpleDBExtensions.Init(SqlClientFactory.Instance, null));

            SimpleDBExtensions.Init(SqlClientFactory.Instance, "Data Source=.;Initial Catalog=HDFTest;Integrated Security=True");



            Assert.Throws<ArgumentNullException>(() => string.Empty.ExecuteNonQuery());
            Assert.Throws<ArgumentNullException>(() => string.Empty.ExecuteReader());
            Assert.Throws<ArgumentNullException>(() => string.Empty.ExecuteNonQueryInTran(null));
            Assert.Throws<ArgumentNullException>(() => "xxxx".ExecuteNonQueryInTran(null));





            Assert.Throws<SqlException>(() => "xxxx".ExecuteNonQuery());
            Assert.Throws<SqlException>(() => "xxxx".ExecuteReader());
            Assert.Throws<SqlException>(() => "xxxx".ExecuteNonQueryInTran(i => false));


        }





    }
}
