﻿using System;
using System.Data;
using System.Data.Common;

namespace HDF.Common;

/// <summary>
/// 基础ADO拓展，不同数据库继承重写<see cref="DbProviderFactory"/>
/// </summary>
public abstract class BaseDBHelper
{
    /// <summary>
    /// 初始化BaseDBHelper
    /// </summary>
    /// <param name="connectionString"></param>
    /// <exception cref="ArgumentNullException"/>
    public BaseDBHelper(string connectionString)
    {
        if (DbProviderFactory == null)
            throw new NullReferenceException(nameof(DbProviderFactory) + "不能为空");

        if (connectionString.IsNullOrWhiteSpace())
            throw new ArgumentNullException(nameof(connectionString));

        ConnectionString = connectionString;
    }

    /// <summary>
    /// 表示一组方法用于创建数据源类的提供程序的实现。
    /// </summary>
    public abstract DbProviderFactory DbProviderFactory { get; }

    /// <summary>
    /// 数据库连接字符串
    /// </summary>
    public string ConnectionString { get; }



    /// <summary>
    /// 创建Sql参数对象
    /// </summary>
    /// <param name="name"></param>
    /// <param name="value"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"/>
    /// <exception cref="NullReferenceException"/>
    public DbParameter CreateParameter(string name, string value)
    {
        DbParameter parameter = DbProviderFactory.CreateParameter()!;
        parameter.ParameterName = name;
        parameter.Value = value;
        return parameter;
    }

    /// <summary>
    /// 创建Sql参数对象
    /// </summary>
    /// <param name="name">参数名称</param>
    /// <param name="value">参数值</param>
    /// <param name="type">参数类型</param>
    /// <returns>sql参数对象</returns>
    /// <exception cref="ArgumentNullException"/>
    public DbParameter CreateParameter(string name, string value, DbType type)
    {
        var parameter = DbProviderFactory.CreateParameter()!;
        parameter.DbType = type;
        parameter.ParameterName = name;
        parameter.Value = value;
        return parameter;
    }


    #region Execute

    private TResult Execute<TResult>(Func<DbCommand, TResult> func, string sql, CommandType commandType, params DbParameter[] parameters)
    {
        if (sql.IsNullOrWhiteSpace())
            throw new ArgumentNullException(nameof(sql));

        using var conn = DbProviderFactory.CreateConnection()!;
        conn.ConnectionString = ConnectionString;
        if (conn.State != ConnectionState.Open) conn.Open();

        using DbCommand cmd = conn.CreateCommand();
        cmd.CommandText = sql;
        cmd.CommandType = commandType;
        if (!parameters.IsNullOrEmpty())
            cmd.Parameters.AddRange(parameters);
        try
        {
            return func.Invoke(cmd);
        }
        catch (Exception)
        {
            throw;
        }
    }

    /// <summary>
    /// 执行sql语句
    /// </summary>
    /// <param name="sql">执行的sql</param>
    /// <param name="commandType">sql类型</param>
    /// <param name="parameters">sql参数</param>
    /// <returns>受影响的行数</returns>
    /// <exception cref="ArgumentNullException"/>
    public int ExecuteNonQuery(string sql, CommandType commandType = CommandType.Text, params DbParameter[] parameters) => Execute(cmd => cmd.ExecuteNonQuery(), sql, commandType, parameters);

    /// <summary>
    /// 执行查询，返回结果集的第一行的第一列，其他将被忽略
    /// </summary>
    /// <param name="sql">执行的sql</param>
    /// <param name="commandType">sql类型</param>
    /// <param name="parameters">sql参数</param>
    /// <returns>结果集的第一行的第一列</returns>
    /// <exception cref="ArgumentNullException"/>
    public object? ExecuteScalar(string sql, CommandType commandType = CommandType.Text, params DbParameter[] parameters) => Execute(cmd => cmd.ExecuteScalar(), sql, commandType, parameters);

    /// <summary>
    /// 执行sql，返回一个<see cref="DbDataReader"/>对象
    /// </summary>
    /// <param name="sql">执行的sql</param>
    /// <param name="commandType">sql类型</param>
    /// <param name="parameters">sql参数</param>
    /// <returns>一个<see cref="DbDataReader"/>对象</returns>
    /// <exception cref="ArgumentNullException"/>
    public DbDataReader ExecuteReader(string sql, CommandType commandType = CommandType.Text, params DbParameter[] parameters)
    {
        if (sql.IsNullOrWhiteSpace())
            throw new ArgumentNullException(nameof(sql));

        var conn = DbProviderFactory.CreateConnection()!;
        conn.ConnectionString = ConnectionString;
        if (conn.State != ConnectionState.Open) conn.Open();

        DbCommand cmd = conn.CreateCommand();
        cmd.CommandText = sql;
        cmd.CommandType = commandType;
        if (!parameters.IsNullOrEmpty())
            cmd.Parameters.AddRange(parameters);
        try
        {
            return cmd.ExecuteReader(CommandBehavior.CloseConnection);
        }
        catch (Exception)
        {
            throw;
        }
    }

    /// <summary>
    /// 执行sql，返回一个<see cref="DataTable"/>对象
    /// </summary>
    /// <param name="sql">执行的sql</param>
    /// <param name="commandType">sql类型</param>
    /// <param name="parameters">sql参数</param>
    /// <returns>一个<see cref="DataTable"/>对象</returns>
    /// <exception cref="ArgumentNullException"/>
    /// <exception cref="NullReferenceException"/>
    public DataTable ExecuteAdapter(string sql, CommandType commandType = CommandType.Text, params DbParameter[] parameters) =>
        Execute(cmd =>
         {
             DataTable dt = new();
             var adapter = DbProviderFactory.CreateDataAdapter()!;
             adapter.SelectCommand = cmd;
             adapter.Fill(dt);
             return dt;
         }, sql, commandType, parameters);


    #endregion

    #region ExecuteInTransaction

    /// <summary>
    /// 在事务中执行sql并返回受影响行数
    /// </summary>
    /// <param name="sql">执行的sql或存储过程（多条sql使用分号间隔）</param>
    /// <param name="isRollback">一个返回值为bool的委托，用于确定事务是否回滚，返回true则回滚事务</param>
    /// <param name="commandType">sql类型</param>
    /// <param name="parameters">sql参数</param>
    /// <returns>受影响行数</returns>
    /// <exception cref="ArgumentNullException"/>
    /// <exception cref="NullReferenceException"/>
    public int ExecuteNonQueryInTran(string sql, Func<int, bool> isRollback, CommandType commandType = CommandType.Text, params DbParameter[] parameters)
    {
        if (sql.IsNullOrWhiteSpace())
            throw new ArgumentNullException(nameof(sql));

        if (isRollback == null)
            throw new ArgumentNullException(nameof(isRollback));

        using var conn = DbProviderFactory.CreateConnection()!;
        conn.ConnectionString = ConnectionString;

        if (conn.State != ConnectionState.Open) conn.Open();

        using DbCommand cmd = conn.CreateCommand();

        using DbTransaction transaction = conn.BeginTransaction();

        cmd.CommandText = sql;
        cmd.CommandType = commandType;
        cmd.Transaction = transaction;
        if (!parameters.IsNullOrEmpty())
            cmd.Parameters.AddRange(parameters);

        try
        {
            var i = cmd.ExecuteNonQuery();

            if (!isRollback.Invoke(i))
                transaction.Commit();
            else
                transaction.Rollback();
            return i;
        }
        catch (Exception)
        {
            transaction.Rollback();
            throw;
        }
    }

    #endregion
}




