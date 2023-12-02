using LibrarySystem.DAL.Models;
using LibrarySystem.DAL.Settings;
using Microsoft.Extensions.Options;
using System.Data.SqlClient;
using System.Data;
using System.Runtime.Intrinsics.Arm;

namespace LibrarySystem.BLL.Services.BorrowingService;
public class BorrowingService : IBorrowingService
{
	private readonly ConnectionSetting _connectionString;

	public BorrowingService(IOptions<ConnectionSetting> options)
	{
		_connectionString = options.Value;
	}

	public async Task DeleteBorrowingAsync(int userId,int bookId)
	{
        using (SqlCommand cmd = new SqlCommand("sp_delete_borrowing", new SqlConnection(_connectionString.SqlConnectionString)))
        {

            cmd.CommandType = CommandType.StoredProcedure;



            cmd.Parameters.Add(new SqlParameter("@user_id", userId));
            cmd.Parameters.Add(new SqlParameter("@book_id", bookId));


            await cmd.Connection.OpenAsync();


            await cmd.ExecuteNonQueryAsync();


            cmd.Connection.Close();
        }
    }

	public async Task InsertBorrowingAsync(Borrowing borrowing)
	{
		using (SqlCommand cmd = new SqlCommand("sp_insert_borrowing", new SqlConnection(_connectionString.SqlConnectionString)))
		{
			cmd.CommandType = CommandType.StoredProcedure;

			cmd.Parameters.Add(new SqlParameter("@user_id", borrowing.UserId));
			cmd.Parameters.Add(new SqlParameter("@book_id", borrowing.BookId));


			await cmd.Connection.OpenAsync();

			await cmd.ExecuteNonQueryAsync();

			cmd.Connection.Close();
		}
	}

	public async Task<IEnumerable<Borrowing>> SelectAllUserBorrowingsAsync(int userId)
	{

        List<Borrowing> borrowingBooks = new List<Borrowing>();


        using (SqlCommand cmd = new SqlCommand("sp_select_all_user_borrowings", new SqlConnection(_connectionString.SqlConnectionString)))
        {

            cmd.CommandType = CommandType.StoredProcedure;

			cmd.Parameters.Add(new SqlParameter("@userId", userId));

			await cmd.Connection.OpenAsync();

            using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
            {


                while (await reader.ReadAsync())
                {

                    int bookId = reader.GetInt32(1);


                    Borrowing borrowing = new Borrowing
                    {
                        BookId = bookId
                    };


                    borrowingBooks.Add(borrowing);
                }

            }

            cmd.Connection.Close();
        }

        return borrowingBooks;
    }

	public async Task<bool> IsAvailable(int id)
	{
		Borrowing borrowing = null;

		using (SqlCommand cmd = new SqlCommand("sp_select_borrowing", new SqlConnection(_connectionString.SqlConnectionString)))
		{

			cmd.CommandType = CommandType.StoredProcedure;


			cmd.Parameters.Add(new SqlParameter("@bookId", id));


			await cmd.Connection.OpenAsync();


			using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
			{
				do
				{
					if (reader.HasRows)
					{
						await reader.ReadAsync();

						if (reader.GetFieldType(0) == typeof(int))
						{
							int userId = reader.GetInt32(0);
							int bookId = reader.GetInt32(1);


							borrowing = new Borrowing
							{
								BookId = bookId,
								UserId = userId
							};
						}
						else if (reader.GetFieldType(0) == typeof(string))
						{
							string message = reader.GetString(0);
						}
					}
				}
				while (reader.NextResult());
			}


			cmd.Connection.Close();
		}

		if (borrowing is null)
		{
			return true;
		}

		return false;
		
	}

    public async Task<bool> IsBorrowedByMe(int user,int book)
    {
        Borrowing borrowing = null;

        using (SqlCommand cmd = new SqlCommand("sp_select_borrowing_by_me", new SqlConnection(_connectionString.SqlConnectionString)))
        {

            cmd.CommandType = CommandType.StoredProcedure;


            cmd.Parameters.Add(new SqlParameter("@user_id", user));
            cmd.Parameters.Add(new SqlParameter("@book_id", book));


            await cmd.Connection.OpenAsync();


            using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
            {
                do
                {
                    if (reader.HasRows)
                    {
                        await reader.ReadAsync();

                        if (reader.GetFieldType(0) == typeof(int))
                        {
                            int userId = reader.GetInt32(0);
                            int bookId = reader.GetInt32(1);


                            borrowing = new Borrowing
                            {
                                BookId = bookId,
                                UserId = userId
                            };
                        }
                        else if (reader.GetFieldType(0) == typeof(string))
                        {
                            string message = reader.GetString(0);
                        }
                    }
                }
                while (reader.NextResult());
            }


            cmd.Connection.Close();
        }

        if (borrowing is null)
        {
            return false;
        }

        return true;
    }
}
