using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using ToDo.Models;
using Microsoft.Data.Sqlite;
using ToDo.Models.ViewModels;

namespace ToDo.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        var todoListViewModel = getAllTodos();
        return View(todoListViewModel);
    }

    public JsonResult PopulateForm(int id) {
        var todo = getTodo(id);
        return Json(todo);
    }

    public IActionResult Privacy()
    {
        return View();
    }

    internal TodoViewModel getAllTodos(){

        List<TodoItem> todoList = new();

        using (SqliteConnection con = 
               new SqliteConnection("Data Source=db.sqlite")) {
            
            using (var tableCmd = con.CreateCommand()){
                con.Open();
                tableCmd.CommandText = "SELECT * FROM todo";

                using (var reader = tableCmd.ExecuteReader()){

                    if (reader.HasRows) {
                        while(reader.Read()) {
                            todoList.Add(
                                new TodoItem
                                {
                                    Id = reader.GetInt32(0),
                                    Name = reader.GetString(1)
                                });
                        }
                    }
                    else {
                        return new TodoViewModel
                        {
                            TodoList = todoList
                        };
                    }

                }
            }
        }
        
        return new TodoViewModel
        {
            TodoList = todoList
        };
    }

    
    internal TodoItem getTodo(int id) {

        TodoItem todo = new();

        using (var connection = 
               new SqliteConnection("Data Source=db.sqlite"))

        {
            using (var tableCmd = connection.CreateCommand()){

                    connection.Open();
                    tableCmd.CommandText =  $"SELECT * FROM todo WHERE Id = '{id}'";

                    using (var reader = tableCmd.ExecuteReader()){

                    if (reader.HasRows) {

                        reader.Read();
                        todo.Id = reader.GetInt32(0);
                        todo.Name = reader.GetString(1);
            
                    }
                    else {
                        return todo;
                    }

                }


                }
        }
        return todo;
    }


    internal string reverseString(string str) {  
        char[] charList = str.ToCharArray();  
        for (int i = 0, n = str.Length - 1; i < n; i++, n--) {  
            char temp = charList[i];  
            charList[i] = charList[n];  
            charList[n] = temp;  
        }  
        return new string(charList);  
    } 


    public string getRandom() {
        
        Random rnd = new Random();
        TodoViewModel table = getAllTodos();
        List<int> ids = table.getIds();
        int total = table.len();
        int index = rnd.Next(total);
        TodoItem randomTodo = getTodo(ids[index]);
        string output = reverseString(randomTodo.Name);
        return output;
    }
   
   
    public RedirectResult createTodo(TodoItem todo)
    {
        using (SqliteConnection con = 
               new SqliteConnection("Data Source=db.sqlite"))

        {
            using (var tableCmd = con.CreateCommand()){
                con.Open();
                tableCmd.CommandText = $"INSERT INTO todo (name) VALUES ('{todo.Name}')";
                try {
                    tableCmd.ExecuteNonQuery();
                }
                catch (Exception ex){
                    Console.WriteLine(ex.Message);
                }
            }
        }
        return Redirect("https://localhost:7096/");
    }
    

    public RedirectResult updateTodo(TodoItem todo)
    {
        using (SqliteConnection con = 
               new SqliteConnection("Data Source=db.sqlite"))

        {
            using (var tableCmd = con.CreateCommand()){
                con.Open();
                tableCmd.CommandText = $"UPDATE todo SET name = '{todo.Name}' WHERE Id = '{todo.Id}'";
                try {
                    tableCmd.ExecuteNonQuery();
                }
                catch (Exception ex){
                    Console.WriteLine(ex.Message);
                }
            }
        }
        return Redirect("https://localhost:7096/");
    }

    [HttpPost]
    public ActionResult deleteTodo(int id)
    {
        using (SqliteConnection con = 
               new SqliteConnection("Data Source=db.sqlite"))

        {
            using (var tableCmd = con.CreateCommand()){
                con.Open();
                tableCmd.CommandText = $"DELETE from todo WHERE Id = '{id}'";
                tableCmd.ExecuteNonQuery();
            }
        }
        return new EmptyResult();
    }
} 
