using LibrarySystem.DAL.Models;
using LibrarySystem.DAL.Settings;
using Microsoft.Extensions.Options;
using System.Data.SqlClient;
using System.Data;

namespace LibrarySystem.BLL.Services.BookService;
public class BookService : IBookService
{

    private readonly ConnectionSetting _connectionString;

    public BookService(IOptions<ConnectionSetting> options)
    {
        _connectionString = options.Value;
    }


    public async Task InsertBookAsync(Book book)
    {
		using SqlCommand cmd = new SqlCommand("sp_insert_book", new SqlConnection(_connectionString.SqlConnectionString));
		cmd.CommandType = CommandType.StoredProcedure;

		cmd.Parameters.Add(new SqlParameter("@title", book.Title));
		cmd.Parameters.Add(new SqlParameter("@author", book.Author));
		cmd.Parameters.Add(new SqlParameter("@isbn", book.ISBN));
		cmd.Parameters.Add(new SqlParameter("@description", book.Description));
		cmd.Parameters.Add(new SqlParameter("@image", book.ImageURL));


		await cmd.Connection.OpenAsync();

		await cmd.ExecuteNonQueryAsync();

		cmd.Connection.Close();
	}


    public async Task UpdateBookAsync(Book book)
    {

		using SqlCommand cmd = new SqlCommand("sp_update_book", new SqlConnection(_connectionString.SqlConnectionString));

		cmd.CommandType = CommandType.StoredProcedure;

		cmd.Parameters.Add(new SqlParameter("@id", book.Id));
		cmd.Parameters.Add(new SqlParameter("@title", book.Title));
		cmd.Parameters.Add(new SqlParameter("@author", book.Author));
		cmd.Parameters.Add(new SqlParameter("@isbn", book.ISBN));
		cmd.Parameters.Add(new SqlParameter("@description", book.Description));
		cmd.Parameters.Add(new SqlParameter("@image", book.ImageURL));


		await cmd.Connection.OpenAsync();

		await cmd.ExecuteNonQueryAsync();

		cmd.Connection.Close();
	}


    public async Task DeleteBookAsync(int id)
    {

		using SqlCommand cmd = new SqlCommand("sp_delete_book", new SqlConnection(_connectionString.SqlConnectionString));

		cmd.CommandType = CommandType.StoredProcedure;


		cmd.Parameters.Add(new SqlParameter("@id", id));


		await cmd.Connection.OpenAsync();


		await cmd.ExecuteNonQueryAsync();


		cmd.Connection.Close();
	}


    public async Task<Book> SelectBookAsync(int id)
    {

        Book selectedBook = null;


        using (SqlCommand cmd = new SqlCommand("sp_select_book", new SqlConnection(_connectionString.SqlConnectionString)))
        {

            cmd.CommandType = CommandType.StoredProcedure;


            cmd.Parameters.Add(new SqlParameter("@id", id));


            await cmd.Connection.OpenAsync();


            using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
            {

                if (await reader.ReadAsync())
                {

                    int bookId = reader.GetInt32(0);
                    string title = reader.GetString(1);
                    string author = reader.GetString(2);
                    string isdn = reader.GetString(3);
                    string description = reader.GetString(4);
                    string imageUrl = reader.GetString(5);


                    selectedBook = new Book
                    {
                        Id = bookId,
                        Title = title,
                        Author = author,
						ISBN = isdn,
                        Description = description,
                        ImageURL = imageUrl
                        
                    };

                }
            }


            cmd.Connection.Close();
        }

        return selectedBook;
    }


    public async Task<IEnumerable<Book>> SelectAllBooksAsync()
    {

        List<Book> books = new List<Book>();


        using (SqlCommand cmd = new SqlCommand("sp_select_all_books", new SqlConnection(_connectionString.SqlConnectionString)))
        {

            cmd.CommandType = CommandType.StoredProcedure;


            await cmd.Connection.OpenAsync();

            using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
            {


                while (await reader.ReadAsync())
                {

                    int bookId = reader.GetInt32(0);
                    string title = reader.GetString(1);
                    string author = reader.GetString(2);
                    string isbn = reader.GetString(3);
                    string description = reader.GetString(4);
					string imageUrl = reader.GetString(5);

					Book book = new Book
                    {
                        Id = bookId,
                        Title = title,
                        Author = author,
						ISBN = isbn,
                        Description = description,
                        ImageURL= imageUrl
                    };


                    books.Add(book);
                }

            }

            cmd.Connection.Close();
        }

        return books;
    }



    public List<Book> SearchByTitle(string title)
    {

        List<Book> books = new List<Book>();


        using (SqlConnection connection = new SqlConnection(_connectionString.SqlConnectionString))
        {

            using (SqlCommand command = new SqlCommand("sp_search_by_title", connection))
            {

                command.CommandType = CommandType.StoredProcedure;


                command.Parameters.AddWithValue("@title", title);


                connection.Open();


                using (SqlDataReader reader = command.ExecuteReader())
                {

                    while (reader.Read())
                    {

                        Book book = new Book();
                        book.Id = reader.GetInt32(0);
                        book.Title = reader.GetString(1);
                        book.Author = reader.GetString(2);
                        book.ISBN = reader.GetString(3);
                        book.Description = reader.GetString(4);
                        book.ImageURL = reader.GetString(5);

                        books.Add(book);
                    }
                }


                connection.Close();
            }
        }


        return books;
    }


    public List<Book> SearchByAuthor(string author)
    {

        List<Book> books = new List<Book>();


        using (SqlConnection connection = new SqlConnection(_connectionString.SqlConnectionString))
        {

            using (SqlCommand command = new SqlCommand("sp_search_by_author", connection))
            {

                command.CommandType = CommandType.StoredProcedure;


                command.Parameters.AddWithValue("@author", author);


                connection.Open();


                using (SqlDataReader reader = command.ExecuteReader())
                {

                    while (reader.Read())
                    {

                        Book book = new Book();
                        book.Id = reader.GetInt32(0);
                        book.Title = reader.GetString(1);
                        book.Author = reader.GetString(2);
                        book.ISBN = reader.GetString(3);
                        book.Description = reader.GetString(4);
                        book.ImageURL = reader.GetString(5);


                        books.Add(book);
                    }
                }


                connection.Close();
            }
        }


        return books;
    }


    public List<Book> SearchByIsbn(string isdn)
    {

        List<Book> books = new List<Book>();


        using (SqlConnection connection = new SqlConnection(_connectionString.SqlConnectionString))
        {

            using (SqlCommand command = new SqlCommand("sp_search_by_isbn", connection))
            {

                command.CommandType = CommandType.StoredProcedure;


                command.Parameters.AddWithValue("@isbn", isdn);

                connection.Open();


                using (SqlDataReader reader = command.ExecuteReader())
                {

                    if (reader.Read())
                    {
                        Book book = new Book();
                        book.Id = reader.GetInt32(0);
                        book.Title = reader.GetString(1);
                        book.Author = reader.GetString(2);
                        book.ISBN = reader.GetString(3);
                        book.Description = reader.GetString(4);
                        book.ImageURL = reader.GetString(5);


                        books.Add(book);
                    }
                }


                connection.Close();
            }
        }


        return books;
    }


}
