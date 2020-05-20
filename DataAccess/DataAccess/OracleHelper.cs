using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.ServiceModel;
using System.Text;
using System.Text.RegularExpressions;
using Common;
using AppEntities;
using Oracle.DataAccess.Client;
using Oracle.DataAccess.Types;

namespace DataAccess
{
    public static class OracleHelper
    {
        public const int ORACLE_WRONG_PARAMETERS = 6550;
        public const int ORACLE_USER_HANDLED_EXCEPTION_CODE = 20000;
        private static Dictionary<string, OracleParameter[]> m_CachedParameters;

        static OracleHelper()
        {
            m_CachedParameters = new Dictionary<string, OracleParameter[]>();
        }

        public static DataContainer FillDataTable(string connectionString, string commandText, params ParamInfo[] values)
        {
            using (var conn = new OracleConnection(connectionString))
            {
                try
                {
                    conn.Open();
                }
                catch (Exception ex)
                {
                    
                    throw ErrorUtils.CreateErrorWithSubMessage(ERR_SQL.ERR_SQL_OPEN_CONNECTION_FAIL, ex.Message);
                }

                try
                {
                    DataTable dt;
                    using (var comm = new OracleCommand(commandText, conn))
                    {
                        var adap = new OracleDataAdapter(comm);
                        var ds = new DataSet();
                        comm.CommandType = CommandType.StoredProcedure;
                        AssignParameters(comm, values);

                        adap.Fill(ds);
                        
                        if (ds.Tables.Count > 0)
                            dt = ds.Tables[0];
                        else
                            dt = null;
                    }
                    return new DataContainer{ DataTable = dt};
                }
                catch (OracleException ex)
                {
                    throw ThrowOracleUserException(ex, commandText);
                }
                catch (FaultException)
                {
                    throw;
                }
                catch (Exception ex)
                {
                    throw ErrorUtils.CreateErrorWithSubMessage(ERR_SQL.ERR_SQL_EXECUTE_COMMAND_FAIL, ex.Message);
                }
                finally
                {
                    conn.Close();
                }
            }
        }

        private static void AssignParameters(OracleCommand comm, params ParamInfo[] values)
        {
            try
            {
                DiscoveryParameters(comm);
                foreach (OracleParameter param in comm.Parameters)
                {
                    if (param.OracleDbType == OracleDbType.RefCursor)
                    {
                        param.Direction = ParameterDirection.Output;
                    }
                    else if (param.Direction == ParameterDirection.Input ||
                             param.Direction == ParameterDirection.InputOutput)
                    {
                        foreach (var parameterInfo in values)
                        {
                            if (param.ParameterName == parameterInfo.ParameterName.ToUpper())
                            {

                                if (parameterInfo.ParameterValue == null ||
                                    (parameterInfo.ParameterValue is string && (string)parameterInfo.ParameterValue == string.Empty))
                                {
                                    param.Value = DBNull.Value;
                                }
                                else if (param.OracleDbType == OracleDbType.NClob)
                                {
                                    var lob = new OracleClob(comm.Connection);
                                    var buffer = Encoding.Unicode.GetBytes(parameterInfo.ParameterValue.ToString());
                                    lob.Write(buffer, 0, buffer.Length);

                                    param.Value = lob;
                                }
                                else
                                {
                                    switch (param.OracleDbType)
                                    {
                                        case OracleDbType.Date:
                                            param.Value = Convert.ToDateTime(parameterInfo.ParameterValue);
                                            break;
                                        case OracleDbType.Byte:
                                        case OracleDbType.Int16:
                                        case OracleDbType.Int32:
                                        case OracleDbType.Int64:
                                        case OracleDbType.Single:
                                        case OracleDbType.Double:
                                        case OracleDbType.Decimal:
                                            param.Value = Convert.ToDecimal(parameterInfo.ParameterValue);
                                            break;
                                        default:
                                            param.Value = parameterInfo.ParameterValue;
                                            break;
                                    }
                                }

                            }
                        }
                    }
                }
            }
            catch(IndexOutOfRangeException)
            {
                if (m_CachedParameters.ContainsKey(comm.CommandText))
                    m_CachedParameters.Remove(comm.CommandText);
                throw;
            }
            catch (Exception ex)
            {
            }
        }
        

        private static void AssignParameters(OracleCommand comm, params object[] values)
        {
            try
            {
                DiscoveryParameters(comm);
                // assign value
                var index = 0;
                foreach (OracleParameter param in comm.Parameters)
                {
                    if (param.OracleDbType == OracleDbType.RefCursor)
                    {
                        param.Direction = ParameterDirection.Output;
                    }
                    else if (param.Direction == ParameterDirection.Input || param.Direction == ParameterDirection.InputOutput)
                    {
                        if (values[index] == null || (values[index] is string && (string)values[index] == string.Empty))
                        {
                            param.Value = DBNull.Value;
                        }
                        else if (param.OracleDbType == OracleDbType.NClob)
                        {
                            var lob = new OracleClob(comm.Connection);
                            var buffer = Encoding.Unicode.GetBytes(values[index].ToString());
                            lob.Write(buffer, 0, buffer.Length);

                            param.Value = lob;
                        }
                        else
                        {
                            switch (param.OracleDbType)
                            {
                                case OracleDbType.Date:
                                    param.Value = Convert.ToDateTime(values[index]);
                                    break;
                                case OracleDbType.Byte:
                                case OracleDbType.Int16:
                                case OracleDbType.Int32:
                                case OracleDbType.Int64:
                                case OracleDbType.Single:
                                case OracleDbType.Double:
                                case OracleDbType.Decimal:
                                    param.Value = Convert.ToDecimal(values[index]);
                                    break;
                                default:
                                    param.Value = values[index];
                                    break;
                            }
                        }
                        index++;
                    }
                }
            }
            catch(IndexOutOfRangeException)
            {
                if (m_CachedParameters.ContainsKey(comm.CommandText))
                    m_CachedParameters.Remove(comm.CommandText);
                throw;
            }
            catch (Exception ex)
            {
            }
        }

        public static List<T> ExecuteCommandText<T>(string connectionString, string commandText, params object[] values)
            where T : class, new()
        {
            using (var conn = new OracleConnection(connectionString))
            {
                try
                {
                    conn.Open();
                }
                catch (Exception ex)
                {
                    throw ErrorUtils.CreateErrorWithSubMessage(ERR_SQL.ERR_SQL_OPEN_CONNECTION_FAIL, ex.Message);
                }

                using (var comm = new OracleCommand(commandText, conn))
                {
                    try
                    {
                        comm.BindByName = true;
                        comm.CommandType = CommandType.Text;
                        for (int i = 0; i < values.Length; i++)
                        {
                            comm.Parameters.Add(":" + i, values[i]);
                        }

                        using (var dr = comm.ExecuteReader())
                        {
                            return dr.ToList<T>();
                        }
                    }
                    catch (OracleException ex)
                    {
                        
                        throw ThrowOracleUserException(ex, commandText);
                    }
                    catch (FaultException)
                    {
                        throw;
                    }
                    catch (Exception ex)
                    {
                        
                        throw ErrorUtils.CreateErrorWithSubMessage(ERR_SQL.ERR_SQL_EXECUTE_COMMAND_FAIL, ex.Message);
                    }
                    finally
                    {
                        conn.Close();
                    }
                }
            }
        }

        public static List<T> ExecuteCommandTextGeneric<T>(string connectionString, string commandText, params object[] values)
        {
            using (var conn = new OracleConnection(connectionString))
            {
                try
                {
                    conn.Open();
                }
                catch (Exception ex)
                {
                    
                    throw ErrorUtils.CreateErrorWithSubMessage(ERR_SQL.ERR_SQL_OPEN_CONNECTION_FAIL, ex.Message);
                }

                using (var comm = new OracleCommand(commandText, conn))
                {
                    try
                    {
                        comm.BindByName = true;
                        comm.CommandType = CommandType.Text;
                        for (int i = 0; i < values.Length; i++)
                        {
                            comm.Parameters.Add(":" + i, values[i]);
                        }

                        using (var dr = comm.ExecuteReader())
                        {
                            var list = new List<T>();

                            if (comm.Parameters.Contains("RETURN_VALUE"))
                            {
                                var value = comm.Parameters["RETURN_VALUE"].Value;
                                var valueType = value.GetType();

                                if (valueType == typeof(OracleDecimal))
                                {
                                    value = ((OracleDecimal)value).Value;
                                }
                                else if (valueType == typeof(OracleDate))
                                {
                                    value = ((OracleDecimal)value).Value;
                                }
                                else if (valueType == typeof(OracleString))
                                {
                                    value = ((OracleString)value).Value;
                                }

                                list.Add((T)Convert.ChangeType(value, typeof(T)));
                                return list;
                            }

                            while (dr.Read())
                            {
                                list.Add((T)Convert.ChangeType(dr.GetValue(0), typeof(T)));
                            }
                            return list;
                        }
                    }
                    catch (OracleException ex)
                    {
                        
                        throw ThrowOracleUserException(ex, commandText);
                    }
                    catch (FaultException)
                    {
                        throw;
                    }
                    catch (Exception ex)
                    {
                        
                        throw ErrorUtils.CreateErrorWithSubMessage(ERR_SQL.ERR_SQL_EXECUTE_COMMAND_FAIL, ex.Message);
                    }
                    finally
                    {
                        conn.Close();
                    }
                }
            }
        }

        public static void ExecuteCommandText(string connectionString, string commandText, params object[] values)
        {
            using (var conn = new OracleConnection(connectionString))
            {
                try
                {
                    conn.Open();
                }
                catch (Exception ex)
                {
                    
                    throw ErrorUtils.CreateErrorWithSubMessage(ERR_SQL.ERR_SQL_OPEN_CONNECTION_FAIL, ex.Message);
                }

                using (var comm = new OracleCommand(commandText, conn))
                {
                    try
                    {
                        comm.CommandType = CommandType.Text;
                        for (int i = 0; i < values.Length; i++)
                        {
                            comm.Parameters.Add(":" + i, values[i]);
                        }

                        comm.ExecuteNonQuery();
                    }
                    catch (OracleException ex)
                    {
                        
                        throw ThrowOracleUserException(ex, commandText);
                    }
                    catch (FaultException)
                    {
                        throw;
                    }
                    catch (Exception ex)
                    {
                        
                        throw ErrorUtils.CreateErrorWithSubMessage(ERR_SQL.ERR_SQL_EXECUTE_COMMAND_FAIL, ex.Message);
                    }
                    finally
                    {
                        conn.Close();
                    }
                }
            }
        }
        
        public static void ExecuteStoreProcedure(string connectionString, string commandText, params object[] values)
        {
            using (var conn = new OracleConnection(connectionString))
            {
                try
                {
                    conn.Open();
                }
                catch (Exception ex)
                {
                    
                    throw ErrorUtils.CreateErrorWithSubMessage(ERR_SQL.ERR_SQL_OPEN_CONNECTION_FAIL, ex.Message);
                }

                try
                {
                    var comm = new OracleCommand(commandText, conn) {CommandType = CommandType.StoredProcedure};
                    AssignParameters(comm, values);

                    comm.ExecuteNonQuery();
                }
                catch (OracleException ex)
                {
                    throw ThrowOracleUserException(ex, commandText);
                }
                catch (FaultException)
                {
                    throw;
                }
                catch (Exception ex)
                {
                    
                    throw ErrorUtils.CreateErrorWithSubMessage(ERR_SQL.ERR_SQL_EXECUTE_COMMAND_FAIL, ex.Message);
                }
                finally
                {
                    conn.Close();
                }
            }
        }

        public static List<T> ExecuteStoreProcedureGeneric<T>(string connectionString, string commandText, params object[] values)
        {
            using (var conn = new OracleConnection(connectionString))
            {
                try
                {
                    conn.Open();
                }
                catch (Exception ex)
                {
                    
                    throw ErrorUtils.CreateErrorWithSubMessage(ERR_SQL.ERR_SQL_OPEN_CONNECTION_FAIL, ex.Message);
                }

                try
                {
                    var comm = new OracleCommand(commandText, conn) {CommandType = CommandType.StoredProcedure};
                    AssignParameters(comm, values);

                    using (var dr = comm.ExecuteReader())
                    {
                        var list = new List<T>();

                        if (comm.Parameters.Contains("RETURN_VALUE"))
                        {
                            var value = comm.Parameters["RETURN_VALUE"].Value;
                            var valueType = value.GetType();

                            if (valueType == typeof(OracleDecimal))
                            {
                                value = ((OracleDecimal) value).Value;
                            }
                            else if(valueType == typeof(OracleDate))
                            {
                                value = ((OracleDecimal)value).Value;
                            }
                            else if(valueType == typeof(OracleString))
                            {
                                value = ((OracleString)value).Value;
                            }

                            list.Add((T)Convert.ChangeType(value, typeof(T)));
                            return list;
                        }

                        while (dr.Read())
                        {
                            list.Add((T)Convert.ChangeType(dr.GetValue(0), typeof(T)));
                        }
                        return list;
                    }
                }
                catch (OracleException ex)
                {
                    
                    throw ThrowOracleUserException(ex, commandText);
                }
                catch (FaultException)
                {
                    throw;
                }
                catch (Exception ex)
                {
                    
                    throw ErrorUtils.CreateErrorWithSubMessage(ERR_SQL.ERR_SQL_EXECUTE_COMMAND_FAIL, ex.Message);
                }
                finally
                {
                    conn.Close();
                }
            }
        }

        public static List<T> ExecuteStoreProcedure<T>(string connectionString, string commandText, params object[] values)
            where T : class, new()
        {
            using (var conn = new OracleConnection(connectionString))
            {
                try
                {
                    conn.Open();
                }
                catch(Exception ex)
                {
                    
                    throw ErrorUtils.CreateErrorWithSubMessage(ERR_SQL.ERR_SQL_OPEN_CONNECTION_FAIL, ex.Message);
                }

                using (var comm = new OracleCommand(commandText, conn))
                {
                    try
                    {
                        comm.CommandType = CommandType.StoredProcedure;
                        AssignParameters(comm, values);

                        using (var dr = comm.ExecuteReader())
                        {
                            return dr.ToList<T>();
                        }
                    }
                    catch (OracleException ex)
                    {
                        
                        throw ThrowOracleUserException(ex, commandText);
                    }
                    catch (FaultException)
                    {
                        throw;
                    }
                    catch (Exception ex)
                    {
                        
                        throw ErrorUtils.CreateErrorWithSubMessage(ERR_SQL.ERR_SQL_EXECUTE_COMMAND_FAIL, ex.Message);
                    }
                    finally
                    {
                        conn.Close();
                    }
                }
            }
        }

        public static OracleDataReader[] ExecuteReader(string connectionString, string commandText, params object[] values)
        {
            using (var conn = new OracleConnection(connectionString))
            {
                try
                {
                    conn.Open();
                }
                catch (Exception ex)
                {
                    
                    throw ErrorUtils.CreateErrorWithSubMessage(ERR_SQL.ERR_SQL_OPEN_CONNECTION_FAIL, ex.Message);
                }

                try
                {
                    var comm = new OracleCommand(commandText, conn) { CommandType = CommandType.StoredProcedure };
                    AssignParameters(comm, values);

                    comm.ExecuteNonQuery();

                    var readers = (from OracleParameter param in comm.Parameters
                                   where param.OracleDbType == OracleDbType.RefCursor
                                   select
                                       (param.Value as OracleDataReader) ?? ((OracleRefCursor)param.Value).GetDataReader()).ToArray();

                    return readers;
                }
                catch (OracleException ex)
                {
                    
                    throw ThrowOracleUserException(ex, commandText);
                }
                catch (FaultException)
                {
                    throw;
                }
                catch (Exception ex)
                {
                    
                    throw ErrorUtils.CreateErrorWithSubMessage(ERR_SQL.ERR_SQL_EXECUTE_COMMAND_FAIL, ex.Message);
                }
                finally
                {
                    conn.Close();
                }
            }
        }

        public static OracleDataAdapter CreateDataAdapter(string connectionString, string commandText, params object[] values)
        {
            using (var conn = new OracleConnection(connectionString))
            {
                try
                {
                    conn.Open();
                }
                catch (Exception ex)
                {
                    
                    throw ErrorUtils.CreateErrorWithSubMessage(ERR_SQL.ERR_SQL_OPEN_CONNECTION_FAIL, ex.Message);
                }

                try
                {
                    using (var comm = new OracleCommand(commandText, conn))
                    {
                        var adap = new OracleDataAdapter(comm);
                        comm.CommandType = CommandType.StoredProcedure;
                        AssignParameters(comm, values);

                        return adap;
                    }
                }
                catch (OracleException ex)
                {
                    
                    throw ThrowOracleUserException(ex, commandText);
                }
                catch (FaultException)
                {
                    throw;
                }
                catch (Exception ex)
                {
                    
                    throw ErrorUtils.CreateErrorWithSubMessage(ERR_SQL.ERR_SQL_EXECUTE_COMMAND_FAIL, ex.Message);
                }
                finally
                {
                    conn.Close();
                }
            }
        }

        public static void FillDataTable(string connectionString, string commandText, out DataTable resultTable, params object[] values)
        {
            using (var conn = new OracleConnection(connectionString))
            {
                try
                {
                    conn.Open();
                }
                catch (Exception ex)
                {
                    
                    throw ErrorUtils.CreateErrorWithSubMessage(ERR_SQL.ERR_SQL_OPEN_CONNECTION_FAIL, ex.Message);
                }

                try
                {
                    using (var comm = new OracleCommand(commandText, conn))
                    {
                        var adap = new OracleDataAdapter(comm);
                        var ds = new DataSet();
                        comm.CommandType = CommandType.StoredProcedure;
                        AssignParameters(comm, values);

                        adap.Fill(ds);
                        if(ds.Tables.Count > 0)
                            resultTable = ds.Tables[0];
                        else
                            resultTable = null;
                    }
                }
                catch (OracleException ex)
                {
                    
                    throw ThrowOracleUserException(ex, commandText);
                }
                catch (FaultException)
                {
                    throw;
                }
                catch (Exception ex)
                {
                    
                    throw ErrorUtils.CreateErrorWithSubMessage(ERR_SQL.ERR_SQL_EXECUTE_COMMAND_FAIL, ex.Message);
                }
                finally
                {
                    conn.Close();
                }
            }
        }

        public static void ExecuteSql2Table(string connectionString, string commandText, out DataTable resultTable, params object[] values)
        {
            using (var conn = new OracleConnection(connectionString))
            {
                try
                {
                    conn.Open();
                }
                catch (Exception ex)
                {
                    
                    throw ErrorUtils.CreateErrorWithSubMessage(ERR_SQL.ERR_SQL_OPEN_CONNECTION_FAIL, ex.Message);
                }

                try
                {
                    using (var comm = new OracleCommand(commandText, conn))
                    {
                        var adap = new OracleDataAdapter(comm);
                        var ds = new DataSet();
                        comm.CommandType = CommandType.Text;
                        AssignParameters(comm, values);

                        adap.Fill(ds);
                        if (ds.Tables.Count > 0)
                            resultTable = ds.Tables[0];
                        else
                            resultTable = null;
                    }
                }
                catch (OracleException ex)
                {
                    
                    throw ThrowOracleUserException(ex, commandText);
                }
                catch (FaultException)
                {
                    throw;
                }
                catch (Exception ex)
                {
                    
                    throw ErrorUtils.CreateErrorWithSubMessage(ERR_SQL.ERR_SQL_EXECUTE_COMMAND_FAIL, ex.Message);
                }
                finally
                {
                    conn.Close();
                }
            }
        }

        public static void FillDataSet(string connectionString, string commandText, out DataSet ds, params object[] values)
        {
            using (var conn = new OracleConnection(connectionString))
            {
                try
                {
                    conn.Open();
                }
                catch (Exception ex)
                {
                    
                    throw ErrorUtils.CreateErrorWithSubMessage(ERR_SQL.ERR_SQL_OPEN_CONNECTION_FAIL, ex.Message);
                }

                try
                {
                    using (var comm = new OracleCommand(commandText, conn))
                    {
                        comm.CommandType = CommandType.StoredProcedure;
                        AssignParameters(comm, values);
                        ds = new DataSet();
                        var adap = new OracleDataAdapter(comm);
                        adap.Fill(ds);
                    }
                }
                catch (OracleException ex)
                {
                    
                    throw ThrowOracleUserException(ex, commandText);
                }
                catch (FaultException)
                {
                    throw;
                }
                catch (Exception ex)
                {
                    
                    throw ErrorUtils.CreateErrorWithSubMessage(ERR_SQL.ERR_SQL_EXECUTE_COMMAND_FAIL, ex.Message);
                }
                finally
                {
                    conn.Close();
                }
            }
        }

        public static void DiscoveryParameters(OracleCommand comm)
        {
            try
            {
                // discovery parameter
                var cachedKey = comm.CommandText;
                if (m_CachedParameters.ContainsKey(cachedKey))
                {
                    var source = m_CachedParameters[cachedKey];
                    foreach (var param in source)
                    {
                        comm.Parameters.Add((OracleParameter)param.Clone());
                    }
                }
                else
                {
                    lock (m_CachedParameters)
                    {
                        comm.CommandText += "--" + (new Random().Next());
                        OracleCommandBuilder.DeriveParameters(comm);
                        comm.CommandText = cachedKey;
                        
                        var source = new OracleParameter[comm.Parameters.Count];
                        
                        for (var i = 0; i < comm.Parameters.Count; i++)
                        {
                            source[i] = (OracleParameter)comm.Parameters[i].Clone();
                        }

                        m_CachedParameters[cachedKey] = source;
                    }
                }
            }
            catch(Exception ex)
            {
                throw ErrorUtils.CreateErrorWithSubMessage(ERR_SQL.ERR_SQL_DISCOVERY_PARAMS_FAIL, ex.Message);
            }
        }

        public static Exception ThrowOracleUserException(OracleException ex, string commandText)
        {
            if (ex.Number == ORACLE_WRONG_PARAMETERS)
            {
                if (m_CachedParameters.ContainsKey(commandText))
                    m_CachedParameters.Remove(commandText);
            }

            if (ex.Number == ORACLE_USER_HANDLED_EXCEPTION_CODE)
            {
                var match = Regex.Match(ex.Message, "<ERROR ID=([0-9]+)>([^<]*)</ERROR>");
                if (match.Success)
                {
                    var errCode = int.Parse(match.Groups[1].Value);
                    var errMessage = match.Groups[2].Value;

                    if (!string.IsNullOrEmpty(errMessage))
                    {
                        return ErrorUtils.CreateErrorWithSubMessage(errCode, errMessage);
                    }
                    return ErrorUtils.CreateError(errCode);
                }
            }
            return ErrorUtils.CreateErrorWithSubMessage(ERR_SQL.ERR_SQL_EXECUTE_COMMAND_FAIL, ex.Message);
        }
    }

    public static class ComponentExtensions
    {
        private static readonly Dictionary<Type, object> CachedMapInfo = new Dictionary<Type, object>();
        public static List<T> ToList<T>(this OracleDataReader reader)
            where T : class, new()
        {
            var col = new List<T>();
            while (reader.Read())
            {
                var obj = new T();
                reader.MapObject(obj);
                col.Add(obj);
            }
            return col;
        }

        public static T ToObject<T>(this OracleDataReader reader)
            where T : class, new()
        {
            var obj = new T();
            reader.Read();
            MapObject(reader, obj);
            return obj;
        }

        private static void MapObject<T>(this OracleDataReader reader, T obj)
            where T : class, new()
        {
            var mapInfo = GetMapInfo<T>();

            for (var i = 0; i < reader.FieldCount; i++)
            {
                if (reader[i] != DBNull.Value && mapInfo.ContainsKey(reader.GetName(i)))
                {
                    var prop = mapInfo[reader.GetName(i)];
                    prop.SetValue(obj, Convert.ChangeType(reader[i], prop.PropertyType), null);
                }
            }
        }

        private static Dictionary<string, PropertyInfo> GetMapInfo<T>()
        {
            var type = typeof(T);
            if (CachedMapInfo.ContainsKey(type))
            {
                return (Dictionary<string, PropertyInfo>)CachedMapInfo[type];
            }

            var mapInfo = new Dictionary<string, PropertyInfo>();
            foreach (PropertyInfo prop in type.GetProperties())
            {
                var attributes = prop.GetCustomAttributes(typeof(ColumnAttribute), true);
                foreach (ColumnAttribute attr in attributes)
                {
                    mapInfo.Add(attr.Name, prop);
                }
            }

            CachedMapInfo.Add(type, mapInfo);
            return mapInfo;
        }
    }
}