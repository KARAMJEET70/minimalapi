using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using MiniDemo.Database;
using MiniDemo.Model;
using MiniDemo.Services;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(option =>
{
    option.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateActor = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey=new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))

    };
});
builder.Services.AddAuthorization();
var constring = builder.Configuration.GetConnectionString("AppDB");
builder.Services.AddTransient<DataSeeder>();
builder.Services.AddScoped<IEmployees, Employees>();
builder.Services.AddDbContext<EmployeesDbContext>(x=>x.UseSqlServer(constring));
var app = builder.Build();
app.UseAuthorization();
app.UseAuthentication();

app.MapGet("/", () => "Hello World!");
app.MapPost("/login",
(IEmployees user, tbllogin service) => Login(user, service))
    .Accepts<tbllogin>("application/json")
    .Produces<string>(); 
app.MapGet("/employee", (Func<Employee>)(() =>
     {
         return new Employee()
         {
             Name = "karmjeet",
             Citizenship = "india",
             Employeeid = "7056"

         };
     }));

app.MapGet("/employees",
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "karan")]
([FromServices] IEmployees db) =>
{
    return db.getEmp().ToList();
});


app.MapGet("/employee/{id}",
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "karan")]
([FromServices] IEmployees db, string id ) =>
{
    return db.EmpId(id);
});

app.MapPut("/employee/update",
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "karan")]
([FromServices] IEmployees db, Employee data) =>
{
    return db.PutEmp(data); 
});

app.MapPost("/employee/create",
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "karan")]
([FromServices] IEmployees db, Employee data) =>
{
    return db.AddEmp(data);
    
});

IResult Login(IEmployees db, tbllogin data)
{
    if (!string.IsNullOrEmpty(data.EmailId) && !string.IsNullOrEmpty(data.pass))
    {
        var res = db.login(data);
        if (res is null) return Results.NotFound("user not found");
        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier,res.EmailId),
            new Claim(ClaimTypes.Email,res.EmailId),
            new Claim(ClaimTypes.Role,"karan")

        };
        var token = new JwtSecurityToken
        (

            issuer: builder.Configuration["Jwt:issuer"],
            audience: builder.Configuration["Jwt:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddDays(1),
            notBefore: DateTime.UtcNow,
            signingCredentials: new SigningCredentials(
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:key"])), SecurityAlgorithms.HmacSha256)

        );
        var tokensting = new JwtSecurityTokenHandler().WriteToken(token);
        return Results.Ok(tokensting);
    }
    return Results.BadRequest("Invalid user credentials");

};

//app.MapGet("/employee/{id}", async (http) =>
//{
//    if(!http.Request.RouteValues.TryGetValue("id",out var id ))
//    {
//        http.Response.StatusCode = 400;
//        return;
//    } 
//    else
//    {
//        await  http.Response.WriteAsJsonAsync(new Employees().Getlist().FirstOrDefault(x => x.Employeeid == (string)id));
//    }
//});

app.Run();
