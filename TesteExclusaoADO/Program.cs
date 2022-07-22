using System;
using System.Data;
using System.Data.SqlClient;

namespace TesteExclusaoADO
{
    class Program
    {
        static void Main(string[] args)
        {
            TesteBanco();
        }

        static void TesteBanco()
        {
            string connStr = "Integrated Security=SSPI;Persist Security Info=False;Initial Catalog=clientes;Data Source=SeuServerSqlDB";

            SqlConnection connection = new SqlConnection(connStr);
            SqlTransaction transaction = null;

            try
            {
                connection.Open();
                transaction = connection.BeginTransaction();

                int clienteId = 1;
                int enderecoId = 1;

                string sqlCliente = GetSql(TSql.EXCLUSAO_CLIENTE);
                string sqlEndereco = GetSql(TSql.EXCLUSAO_ENDERECO);

                SqlCommand cmdEndereco = new SqlCommand(sqlEndereco, connection, transaction);
                cmdEndereco.Parameters.Add("@id", SqlDbType.Int).Value = enderecoId;
                cmdEndereco.ExecuteNonQuery();

                SqlCommand cmdCliente = new SqlCommand(sqlCliente, connection, transaction);
                cmdCliente.Parameters.Add("@id", SqlDbType.Int).Value = clienteId;
                cmdCliente.ExecuteNonQuery();

                transaction.Commit();
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                throw ex;
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                    connection.Close();
            }
        }

        private static string GetSql(TSql tipoQuery)
        {
            string sql = "";

            switch (tipoQuery)
            {
                case TSql.EXCLUSAO_CLIENTE:
                    sql = "delete from cliente where clienteId = @id";
                    break;

                case TSql.EXCLUSAO_ENDERECO:
                    sql = "delete from endereco where enderecoId = @id";
                    break;
            }

            return sql;
        }

    }

    public enum TSql
    {
        EXCLUSAO_CLIENTE,
        EXCLUSAO_ENDERECO
    }

}
