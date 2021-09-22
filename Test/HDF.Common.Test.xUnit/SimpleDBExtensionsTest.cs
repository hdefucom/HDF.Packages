using System;
using System.Collections;
using System.Data.Common;
using System.Linq;
using System.Data.SqlClient;
using Xunit;
using System.Data;
using System.Runtime.CompilerServices;

namespace HDF.Common.Test.xUnit
{
    public class SimpleDBExtensionsTest
    {



        [Fact]
        public void SimpleDBTest()
        {
#if NETCOREAPP2_1
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
if exists(select * from sysObjects where name='test{version}' and xtype='U' )
begin
 drop table test{version};
end

CREATE TABLE [dbo].[test{version}](
	[id] [int] NULL
) ON [PRIMARY]

select '1' 
";

            Assert.NotNull(deltablesql.ExecuteScalar());

            Assert.Equal(1, $"insert into test{version} values('1')".ExecuteNonQuery());

            var param = "@id".CreateParameter("1");
            using var reader = $"select * from test{version} where '1'=@id".ExecuteReader(CommandType.Text, param);
            Assert.True(reader.Read());
            Assert.Equal("1", reader["id"].ToString());

            param = "@id".CreateParameter("1");
            var dt = $"select * from test{version} where '1'=@id".ExecuteAdapter(CommandType.Text, param);
            Assert.Equal("1", dt.Rows[0][0].ToString());

            param = "@id".CreateParameter("1");
            Assert.Equal(1, $"delete from test{version} where '1'=@id".ExecuteNonQueryInTran(i => i > 0, CommandType.Text, param));
            Assert.Equal(1, $"insert into test{version} values('2')".ExecuteNonQueryInTran(i => false));

            param = "@id".CreateParameter("1", DbType.String);
            Assert.True($"delete from test{version} where '1'=@id".ExecuteNonQuery(CommandType.Text, param) > 0);


        }


        [Fact]
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
