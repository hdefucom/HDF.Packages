using NUnit.Framework;
using System;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;

namespace HDF.Common.Test.NUnit
{
    public class BaseDBHelperTest
    {


        [Test]
        public void DBHelperTest()
        {
            var helper = new SqlServerDBHelper("Data Source=.;Initial Catalog=HDFTest;Integrated Security=True");

            var deltablesql = @"
if exists(select * from sysObjects where name='test' and xtype='U' )
begin
 drop table test;
end

CREATE TABLE [dbo].[test](
	[id] [int] NULL
) ON [PRIMARY]

select '1' 
";

            var res = helper.ExecuteScalar(deltablesql);

            Assert.NotNull(res);

            Assert.AreEqual(1, helper.ExecuteNonQuery("insert into test values('1')"));

            var param = helper.CreateParameter("@id", "1");
            using var reader = helper.ExecuteReader("select * from test where '1'=@id", CommandType.Text, param);
            Assert.True(reader.Read());
            Assert.AreEqual("1", reader["id"].ToString());

            param = helper.CreateParameter("@id", "1");
            var dt = helper.ExecuteAdapter("select * from test where '1'=@id", CommandType.Text, param);
            Assert.AreEqual("1", dt.Rows[0][0].ToString());

            param = helper.CreateParameter("@id", "1");
            Assert.AreEqual(1, helper.ExecuteNonQueryInTran("delete from test where '1'=@id", i => i > 0, CommandType.Text, param));
            Assert.AreEqual(1, helper.ExecuteNonQueryInTran("insert into test values('2')", i => false));

            param = helper.CreateParameter("@id", "1", DbType.String);
            Assert.True(helper.ExecuteNonQuery("delete from test where '1'=@id", CommandType.Text, param) > 0);


        }


        [Test]
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
