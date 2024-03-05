using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using TPI_ProgramacionIII.DBContexts;
using TPI_ProgramacionIII.Services.Implementations;
using TPI_ProgramacionIII.Services.Interfaces;


            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllers().AddNewtonsoftJson(options =>
                options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
            );


builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(setupAction =>

            {
                setupAction.AddSecurityDefinition("EcommerceRopaApiBearerAuth", new OpenApiSecurityScheme() //Esto va a permitir usar swagger con el token.
                {
                    Type = SecuritySchemeType.Http,
                    Scheme = "Bearer",
                    Description = "Acá pegar el token generado al loguearse."
                });

                setupAction.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                         {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "EcommerceRopaApiBearerAuth" } //Tiene que coincidir con el id seteado arriba en la definición
                            }, new List<string>() }
        });
    }); ;

            builder.Services.AddDbContext<ECommerceContext>(dbContextOptions => dbContextOptions.UseSqlite(
                builder.Configuration["DB:ConnectionString"]));

            #region Inyecccion de dependencias
            builder.Services.AddScoped<IProductService, ProductService>();
            builder.Services.AddScoped<IUserService, UserService>();
            builder.Services.AddScoped<IAdminService, AdminService>();
            builder.Services.AddScoped<IClientService, ClientService>();
            builder.Services.AddScoped<IOrderService, OrderService>();
            builder.Services.AddScoped<ILineOfOrderService, LineOfOrderService>();
            #endregion


            builder.Services.AddAuthentication("Bearer") //"Bearer" es el tipo de auntenticación que tenemos que elegir después en PostMan para pasarle el token
                .AddJwtBearer(options => //Acá definimos la configuración de la autenticación. le decimos qué cosas queremos comprobar. La fecha de expiración se valida por defecto.
                {
                    options.TokenValidationParameters = new()
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = builder.Configuration["Authentication:Issuer"],
                        ValidAudience = builder.Configuration["Authentication:Audience"],
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(builder.Configuration["Authentication:SecretForKey"]))
                    };
                }
            );


            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthentication();           

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
