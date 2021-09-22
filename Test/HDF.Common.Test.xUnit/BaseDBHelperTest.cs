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
    public class BaseDBHelperTest
    {


        [Fact]
        public void DBHelperTest()
        {
#if NETCOREAPP2_1
            string version = "_NETCORE21";
#elif NETCOREAPP3_1
            string version = "_NETCORE31";
#elif NET5_0
            string version = "_NET5";
#endif

            var helper = new SqlServerDBHelper("Data Source=.;Initial Catalog=HDFTest;Integrated Security=True");

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

            var res = helper.ExecuteScalar(deltablesql);

            Assert.NotNull(res);

            Assert.Equal(1, helper.ExecuteNonQuery($"insert into test{version} values('1')"));

            var param = helper.CreateParameter("@id", "1");
            using var reader = helper.ExecuteReader($"select * from test{version} where '1'=@id", CommandType.Text, param);
            Assert.True(reader.Read());
            Assert.Equal("1", reader["id"].ToString());

            param = helper.CreateParameter("@id", "1");
            var dt = helper.ExecuteAdapter($"select * from test{version} where '1'=@id", CommandType.Text, param);
            Assert.Equal("1", dt.Rows[0][0].ToString());

            param = helper.CreateParameter("@id", "1");
            Assert.Equal(1, helper.ExecuteNonQueryInTran($"delete from test{version} where '1'=@id", i => i > 0, CommandType.Text, param));
            Assert.Equal(1, helper.ExecuteNonQueryInTran($"insert into test{version} values('2')", i => false));

            param = helper.CreateParameter("@id", "1", DbType.String);
            Assert.True(helper.ExecuteNonQuery($"delete from test{version} where '1'=@id", CommandType.Text, param) > 0);


        }


        [Fact]
        public void ErrorTest()
        {
            Assert.Throws<NullReferenceException>(() => new SqlServerNullFactoryDBHelper(string.Empty));

            Assert.Throws<ArgumentNullException>(() => new SqlServerDBHelper(string.Empty));
            Assert.Throws<ArgumentNullException>(() => new SqlServerDBHelper("xxx").ExecuteReader(string.Empty));
            Assert.Throws<ArgumentNullException>(() => new SqlServerDBHelper("xxx").ExecuteNonQuery(string.Empty));
            Assert.Throws<ArgumentNullException>(() => new SqlServerDBHelper("xxx").ExecuteNonQueryInTran(string.Empty, null));
            Assert.Throws<ArgumentNullException>(() => new SqlServerDBHelper("xxx").ExecuteNonQueryInTran("xxxx", null));


            Assert.Throws<ArgumentException>(() => new SqlServerDBHelper("xxx").ExecuteReader("xxxx"));

            var helper = new SqlServerDBHelper("Data Source=.;Initial Catalog=HDFTest;Integrated Security=True");

            Assert.Throws<SqlException>(() => helper.ExecuteNonQuery("xxxx"));
            Assert.Throws<SqlException>(() => helper.ExecuteReader("xxxx"));
            Assert.Throws<SqlException>(() => helper.ExecuteNonQueryInTran("xxxx", i => false));


        }



        public class SqlServerDBHelper : BaseDBHelper
        {
            public SqlServerDBHelper(string connectionString) : base(connectionString)
            {
            }

            public override DbProviderFactory DbProviderFactory => SqlClientFactory.Instance;
        }



        public class SqlServerNullFactoryDBHelper : BaseDBHelper
        {
            public SqlServerNullFactoryDBHelper(string connectionString) : base(connectionString)
            {
            }

            public override DbProviderFactory DbProviderFactory => null;
        }



    }
}
