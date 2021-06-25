using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using SplitWise_Business.Validations;
using SplitWise_Services.Calculation;
using SplitWise_Services.Expense;
using SplitWise_Services.Group;
using SplitWise_Services.Settlement;
using SplitWise_Services.User;
using SplitWise_Store.Expense;
using SplitWise_Store.GroupStore;
using SplitWise_Store.Settlements;
using SplitWise_Store.UserStore;
using Microsoft.AspNetCore.Mvc.NewtonsoftJson;
using System.Text.Json.Serialization;

namespace SplitWise_App
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //services.AddMvc()
            //    .AddJsonOptions(
            //        options =>
            //        {
            //            options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve;
            //            options.JsonSerializerOptions.MaxDepth = 10;
            //        }
            //    );
            //services.AddMvc().AddJsonOptions(options =>
            //{
            //    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve;
            //    options.JsonSerializerOptions.MaxDepth = 10;
            //});
            //services.AddMvc().AddJsonOptions(option => option.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve);
            //services.AddMvc().AddJsonOptions(options =>
            //{
            //    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve;
            //    options.JsonSerializerOptions.MaxDepth = 10;
            //});
            //services.AddControllers().AddJsonOptions(options =>
            //{
            //    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve;
            //    options.JsonSerializerOptions.MaxDepth = 10;
            //});
            // DB Store
            services.AddSingleton<IUserStore, UserStore>();
            services.AddSingleton<IGroupStore, GroupStore>();
            services.AddSingleton<IExpenseStore, ExpenseStore>();
            services.AddSingleton<ISettlementStore, SettlementStore>();

            // Services
            services.AddSingleton<IUserService, UserService>();
            services.AddSingleton<IGroupService, GroupService>();
            services.AddSingleton<IExpenseService, ExpenseService>();
            services.AddSingleton<ISettlementService, SettlementService>();
            services.AddSingleton<ICalculationService, CalculationService>();

            // Validations,
            services.AddSingleton<IUserValidationService, UserValidationService>();
            services.AddSingleton<IGroupValidationService, GroupValidationService>();
            services.AddSingleton<IExpenseValidationService, ExpenseValidationService>();
            services.AddSingleton<ISettlementValidationService, SettlementValidationService>();
            services.AddSingleton<IGroupSummaryValidationService, GroupSummaryValidationService>();

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "SplitWise_App", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "SplitWise_App v1"));
            }

            //app.UseHttpsRedirection();

            app.UseRouting();

            //app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
