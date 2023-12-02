using LibrarySystem.DAL.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibrarySystem.DAL.Settings;
using Microsoft.Extensions.Options;
using LibrarySystem.DAL.ViewModels;
using static System.Reflection.Metadata.BlobBuilder;
using System.Security.Cryptography;
using LibrarySystem.BLL.Services.EmailService;

namespace LibrarySystem.BLL.Services.UserService;
public class UserService : IUserService
{

	private readonly ConnectionSetting _connectionString;
	private readonly IEmailService _emailService;

	public UserService(IOptions<ConnectionSetting> options, IEmailService emailService)
	{
		_connectionString = options.Value;
		_emailService = emailService;
	}


	public async Task Register(UserViewModel user)
	{

		CreatePasswordHash(user.Password, out byte[] passwordHash, out byte[] passwordSalt);

		Random random = new Random();
		int randomNumber = random.Next(1000000);
		string token = randomNumber.ToString("D6");

		using (SqlCommand cmd = new SqlCommand("sp_insert_user", new SqlConnection(_connectionString.SqlConnectionString)))
		{
			cmd.CommandType = CommandType.StoredProcedure;

			cmd.Parameters.Add(new SqlParameter("@email", user.Email.ToLower()));
			cmd.Parameters.Add(new SqlParameter("@passwordSalt", passwordSalt));
			cmd.Parameters.Add(new SqlParameter("@passwordHash", passwordHash));
			cmd.Parameters.Add(new SqlParameter("@role", "User"));
			cmd.Parameters.Add(new SqlParameter("@token", token));


			await cmd.Connection.OpenAsync();

			await cmd.ExecuteNonQueryAsync();

			cmd.Connection.Close();
		}

		EmailViewModel request = new EmailViewModel
		{
			To = user.Email,
			Subject = "Confirm Your Email",
			Body = $"<h1>{token}<h1/>"
		};

		await _emailService.SendEmail(request);
	}

	public async Task ResendEmail(UserViewModel model)
	{
		User user = await SelectUserByEmail(model.Email);

		EmailViewModel request = new EmailViewModel
		{
			To = model.Email,
			Subject = "Confirm Your Email",
			Body = $"<h1>{user.Token}<h1/>"
		};

		await _emailService.SendEmail(request);
	}

	public async Task<bool> ConfirmUserAccount(UserViewModel model)
	{
		string token = string.Empty;
		bool isConfirmed = false;

		using (SqlConnection connection = new SqlConnection(_connectionString.SqlConnectionString))
		{
			await connection.OpenAsync();

			using (SqlCommand cmd = new SqlCommand("sp_select_user", connection))
			{
				cmd.CommandType = CommandType.StoredProcedure;
				cmd.Parameters.Add(new SqlParameter("@email", model.Email.ToLower()));

				using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
				{
					if (await reader.ReadAsync())
					{
						token = reader.GetString(5);
						isConfirmed = reader.GetBoolean(6);
					}
				}
			}

			if (isConfirmed)
			{
				return true;
			}

			bool isCorrect = string.Equals(model.Token, token);

			if (isCorrect)
			{
				using (SqlCommand confirmCmd = new SqlCommand("sp_confirm_user", connection))
				{
					confirmCmd.CommandType = CommandType.StoredProcedure;

					confirmCmd.Parameters.Add(new SqlParameter("@email", model.Email.ToLower()));

					await confirmCmd.ExecuteNonQueryAsync();
				}

				return true;
			}
			else
			{
				return false;
			}
		}
	}

	public async Task<User> Login(UserViewModel model)
	{
		User? user = null;


		using (SqlCommand cmd = new SqlCommand("sp_select_user", new SqlConnection(_connectionString.SqlConnectionString)))
		{

			cmd.CommandType = CommandType.StoredProcedure;


			cmd.Parameters.Add(new SqlParameter("@email", model.Email.ToLower()));

			await cmd.Connection.OpenAsync();

			using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
			{

				if (await reader.ReadAsync())
				{

					var firstFieldType = reader.GetFieldType(0);


					if (firstFieldType == typeof(int))
					{
						int userId = reader.GetInt32(0);
						string email = reader.GetString(1);
						string role = reader.GetString(2);
						byte[] passwordHash = (byte[])reader.GetValue(3);
						byte[] passwordSalt = (byte[])reader.GetValue(4);
						bool isConfirmed = reader.GetBoolean(6);


						if (!VerifyPasswordHash(model.Password, passwordHash, passwordSalt))
						{
							user = new User
							{
								IsWrongPassword = true
							};

							return user;
						}

						if (!isConfirmed)
						{

							user = new User
							{
								IsConfirmed = false
							};

							return user;
						}

						user = new User
						{
							Id = userId,
							Email = email,
							Role = role,
							IsConfirmed = isConfirmed
						};
					}

				}
			}

			cmd.Connection.Close();
		}

		return user;
	}

	public async Task<User> SelectUserByEmail(string email)
	{
		User user = null;

		using (SqlCommand cmd = new SqlCommand("sp_select_user", new SqlConnection(_connectionString.SqlConnectionString)))
		{

			cmd.CommandType = CommandType.StoredProcedure;


			cmd.Parameters.Add(new SqlParameter("@email", email));


			await cmd.Connection.OpenAsync();


			using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
			{

				if (await reader.ReadAsync())
				{

					int userId = reader.GetInt32(0);
					string token = reader.GetString(5);
					bool isConfirmed = reader.GetBoolean(6);

					user = new User
					{
						Id = userId,
						IsConfirmed = isConfirmed,
						Token = token
					};

				}
			}


			cmd.Connection.Close();
		}



		return user;
	}

	private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
	{
		using (var hmac = new HMACSHA512())
		{
			passwordSalt = hmac.Key;
			passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
		}
	}
	private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
	{
		using (var hmac = new HMACSHA512(passwordSalt))
		{
			var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
			return computedHash.SequenceEqual(passwordHash);
		}
	}


}
