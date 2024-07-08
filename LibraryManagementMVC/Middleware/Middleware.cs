using LibraryManagementMVC.Middleware.APIModels;
using LibraryManagementMVC.Models;
using loginmodel;
using loginresponsemodel;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ActionConstraints;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using registerresponsemodel;
using rolesmodel;
using System.Net;
using System.Net.Http.Headers;
using System.Reflection;
using System.Text;
using usersresponsemodel;

namespace LibraryManagementMVC.Middleware
{
    public class Middleware
    {
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private HttpClient _httpClient;
        private readonly string baseUrl;

        public Middleware(IHttpContextAccessor httpContextAccessor,IConfiguration configuration,HttpClient httpClient)
        {
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
            _httpClient = httpClient;
            baseUrl = _configuration.GetValue<string>("BaseUrl")!;
        }

        public async Task<LoginResponseModel?> Login(LoginModel loginModel)
        {
            string url = baseUrl + "login";
            HttpResponseMessage response = await _httpClient.PostAsJsonAsync(url, loginModel);
            if (response.IsSuccessStatusCode) {
                string responseBody = await response.Content.ReadAsStringAsync();
                LoginResponseModel model = LoginResponseModel.FromJson(responseBody);

                _httpContextAccessor.HttpContext.Session.SetString("AccessToken", model.AccessToken);
                _httpContextAccessor.HttpContext.Session.SetString("RefreshToken", model.RefreshToken);

                return model;
            } else
            {
                return null;
            }
        }

        private void AddTokenToHeader()
        {
            var token = _httpContextAccessor.HttpContext.Session.GetString("AccessToken");
            if (_httpClient.DefaultRequestHeaders.Contains("Authorization"))
            {
                _httpClient.DefaultRequestHeaders.Remove("Authorization");
            }
            _httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
        }

        private async Task RefreshToken()
        {
            string refreshToken = _httpContextAccessor.HttpContext.Session.GetString("RefreshToken");
            string url = baseUrl + "refresh";
            HttpResponseMessage response = await _httpClient.PostAsJsonAsync(url, refreshToken);
            if (response.IsSuccessStatusCode) { 
                string responseBody = await response.Content.ReadAsStringAsync();
                LoginResponseModel model = LoginResponseModel.FromJson(responseBody);
                _httpContextAccessor.HttpContext.Session.SetString("AccessToken", model.AccessToken);
                _httpContextAccessor.HttpContext.Session.SetString("RefreshToken", model.RefreshToken);
            }
        }

        public async Task<string?> GetUserRole(string mail)
        {
            string url = baseUrl + "api/Role/GetUsersRole?mail="+mail;
            AddTokenToHeader();
            HttpResponseMessage response = await _httpClient.GetAsync(url);
            if (response.IsSuccessStatusCode) {
                string responseBody = await response.Content.ReadAsStringAsync();

                return JsonConvert.DeserializeObject<string[]>(responseBody)[0];
            } else if(response.StatusCode == HttpStatusCode.Unauthorized)
            {
                await RefreshToken();
                return await GetUserRole(mail);
            }
            else
            {
                return null;
            }
        }

        public async Task<LoginModel?> Register(LoginModel registermodel)
        {
            string url = baseUrl + "register";
            var body = registermodel.ToJson();
            AddTokenToHeader();

            HttpResponseMessage response = await _httpClient.PostAsJsonAsync(url, registermodel);

            if (response.IsSuccessStatusCode)
            {
                return registermodel;
            }
            else if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                await RefreshToken();
                return await Register(registermodel);
            }
            else
            {
                return null;
            }
        }

        public async Task<List<RolesModel>> GetRoles()
        {
            string url = baseUrl + "api/Role/Get";
            AddTokenToHeader();

            HttpResponseMessage response = await _httpClient.GetAsync(url);
            if (response.IsSuccessStatusCode) {
                string responseBody = await response.Content.ReadAsStringAsync();
                List<RolesModel> model = RolesModel.FromJson(responseBody);
                return model;
            }
            else if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                await RefreshToken();
                return await GetRoles();
            }
            else
            {
                return new List<RolesModel>();
            }
        }

        public async Task<bool> CreateUser(UserModel userModel)
        {
            string url = baseUrl + "api/User/Create";
            AddTokenToHeader();

            HttpResponseMessage response = await _httpClient.PostAsJsonAsync(url, userModel);
            if (response.IsSuccessStatusCode)
            {
                return true;
            }
            else if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                await RefreshToken();
                return await CreateUser(userModel);
            }
            else
            {
                return false;
            }
        }

        public async Task<List<UsersResponseModel>> GetUsers()
        {
            string url = baseUrl + "api/User/Get";
            AddTokenToHeader();

            HttpResponseMessage response = await _httpClient.GetAsync(url);
            if (response.IsSuccessStatusCode)
            {
                string responseBody = await response.Content.ReadAsStringAsync();
                List<UsersResponseModel> model = UsersResponseModel.FromJson(responseBody);
                return model;
            }
            else if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                await RefreshToken();
                return await GetUsers();
            }
            else
            {
                return new List<UsersResponseModel>();
            }
        }

        public async Task<UsersResponseModel?> GetUserByMail(string email)
        {
            string url = baseUrl + "api/User/GetByMail?mail=" + email;
            AddTokenToHeader();

            HttpResponseMessage response = await _httpClient.GetAsync(url);
            if (response.IsSuccessStatusCode)
            {
                string responseBody = await response.Content.ReadAsStringAsync();
                UsersResponseModel model = UsersResponseModel.FromJsonSolo(responseBody);
                return model;
            }
            else if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                await RefreshToken();
                return await GetUserByMail(email);
            }
            else
            {
                return null;
            }
        }

        public async Task<UsersResponseModel?> UpdateUser(string mail, string newEmail,string Role)
        {
            string url = baseUrl + $"api/User/UpdateUser?mail={mail}&newMail={newEmail}&role={Role}";
            AddTokenToHeader();

            HttpResponseMessage response = await _httpClient.GetAsync(url);
            if (response.IsSuccessStatusCode) {
                string responseBody = await response.Content.ReadAsStringAsync();
                UsersResponseModel model = UsersResponseModel.FromJsonSolo(responseBody);
                return model;
            }
            else if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                await RefreshToken();
                return await UpdateUser(mail,newEmail,Role);
            }
            else
            {
                return null;
            }
        }

        public async Task<UsersResponseModel?> ConfirmUser(string email)
        {
            string url = baseUrl + "api/User/ConfirmUser?mail=" + email;
            AddTokenToHeader();

            HttpResponseMessage response = await _httpClient.GetAsync(url);
            if (response.IsSuccessStatusCode)
            {
                string responseBody = await response.Content.ReadAsStringAsync();
                UsersResponseModel model = UsersResponseModel.FromJsonSolo(responseBody);
                return model;
            }
            else if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                await RefreshToken();
                return await ConfirmUser(email);
            }
            else
            {
                return null;
            }
        }

        public async Task<Book?> CreateBook(Book book)
        {
            string url = baseUrl + "api/Books/Create";
            AddTokenToHeader();

            HttpResponseMessage response = await _httpClient.PostAsJsonAsync(url,book);


            if (response.IsSuccessStatusCode)
            {
                string responseBody = await response.Content.ReadAsStringAsync();
                Book model = Book.FromJson(responseBody);
                return model;
            }
            else if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                await RefreshToken();
                return await CreateBook(book);
            }
            else
            {
                return null;
            }
        }

        public async Task<List<Book>> GetBooks()
        {
            string url = baseUrl + "api/Books/GetAll";
            AddTokenToHeader();
            HttpResponseMessage response = await _httpClient.GetAsync(url);
            if (response.IsSuccessStatusCode)
            {
                string responseBody = await response.Content.ReadAsStringAsync();
                List<Book> model = Book.FromJsonList(responseBody);
                return model;
            }
            else if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                await RefreshToken();
                return await GetBooks();
            }
            else
            {
                return new List<Book>();
            }
        }

        public async Task<bool> DeleteBook(int id)
        {
            string url = baseUrl + "api/Books/Delete?id="+id;
            AddTokenToHeader();

            HttpResponseMessage response = await _httpClient.DeleteAsync(url);
            if (response.IsSuccessStatusCode) {
                return true;
            }
            else if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                await RefreshToken();
                return await DeleteBook(id);
            }
            else
            {
                return false;
            }
        }

        public async Task<List<Book>> SearchBook(string text,string filter)
        {
            if(filter == "bookName")
            {
                string url = baseUrl + "api/Books/SearchByTitle?title=" + text;
                AddTokenToHeader();

                HttpResponseMessage response = await _httpClient.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    string responseBody = await response.Content.ReadAsStringAsync();
                    List<Book> model = Book.FromJsonList(responseBody);
                    return model;
                }
                else if (response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    await RefreshToken();
                    return await SearchBook(text,filter);
                }
                else
                {
                    return new List<Book>();
                }
            }
            else if (filter == "author")
            {
                string url = baseUrl + "api/Books/SearchByAuthor?author=" + text;
                AddTokenToHeader();

                HttpResponseMessage response = await _httpClient.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    string responseBody = await response.Content.ReadAsStringAsync();
                    List<Book> model = Book.FromJsonList(responseBody);
                    return model;
                }
                else if (response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    await RefreshToken();
                    return await SearchBook(text,filter);
                }
                else { return new List<Book>(); }
            }
            else
            {
                return new List<Book>();
            }
        }

        public async Task<Book?> GetBookById(int id)
        {
            string url = baseUrl + "api/Books/GetById?id=" + id;
            AddTokenToHeader();

            HttpResponseMessage response = await _httpClient.GetAsync(url);
            if (response.IsSuccessStatusCode)
            {
                string responseBody = await response.Content.ReadAsStringAsync();
                Book model = Book.FromJson(responseBody);
                return model;
            }
            else if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                await RefreshToken();
                return await GetBookById(id);
            }
            else
            {
                return null;
            }
        }

        public async Task<bool> RentBook(Rental rental)
        {
            string url = baseUrl + "api/Rental/Create";
            AddTokenToHeader();

            HttpResponseMessage response = await _httpClient.PostAsJsonAsync(url, rental);
            if (response.IsSuccessStatusCode) {
                return true;
            }
            else if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                await RefreshToken();
                return await RentBook(rental);
            }
            else
            {
                return false;
            }
        }

        public async Task<List<Rental>> GetUserRentals(string mail)
        {
            string url = baseUrl + "api/Rental/GetByUser?mail=" + mail;
            AddTokenToHeader();

            HttpResponseMessage response = await _httpClient.GetAsync(url);
            if (response.IsSuccessStatusCode) { 
                string responseBody = await response.Content.ReadAsStringAsync();
                List<Rental> model = Rental.FromJsonList(responseBody);
                return model;
            }
            else if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                await RefreshToken();
                return await GetUserRentals(mail);
            }
            else
            {
                return new List<Rental>();
            }
        }

        public async Task<bool> Return(int id,int bookid)
        {
            string url = baseUrl + $"api/Rental/Return?id={id}&bookid={bookid}";
            AddTokenToHeader();

            HttpResponseMessage response = await _httpClient.GetAsync(url);
            if (response.IsSuccessStatusCode)
            {
                return true;
            }
            else if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                await RefreshToken();
                return await Return(id,bookid);
            }
            else
            {
                return false;
            }
        }

        public async Task<List<Rental>> GetRentals()
        {
            string url = baseUrl + "api/Rental/GetRents";
            AddTokenToHeader();

            HttpResponseMessage response = await _httpClient.GetAsync(url);
            if (response.IsSuccessStatusCode) {
                string responseBody = await response.Content.ReadAsStringAsync();
                List<Rental> model = Rental.FromJsonList(responseBody);
                return model;
            }
            else if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                await RefreshToken();
                return await GetRentals();
            }
            else
            {
                return new List<Rental>(); 
            }
        }

        public async Task<bool> ChangePassword(string mail, string currentPassword, string newPassword)
        {
            string url = baseUrl + $"api/User/ChangePassword?mail={mail}&currentPassword={currentPassword}&newPassword={newPassword}";
            AddTokenToHeader();
            HttpResponseMessage response = await _httpClient.GetAsync(url);
            if (response.IsSuccessStatusCode)
            {
                return true;
            }
            else if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                await RefreshToken();
                return await ChangePassword(mail, currentPassword, newPassword);
            }
            else
            {
                return false;
            }
        }

        public async Task<bool> RejectUser(string mail)
        {
            string url = baseUrl + $"api/User/RejectUser?mail={mail}";
            AddTokenToHeader();
            HttpResponseMessage response = await _httpClient.GetAsync(url);
            if (response.IsSuccessStatusCode)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
