using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Amazon.DynamoDBv2;
using Amazon.S3;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using RPMCRUDAPIs.Models;
using Stripe;

namespace RPMCRUDAPIs
{
    public class Startup
    {
        public const string AppS3BucketKey = "AppS3Bucket";
       
        public Startup(IConfiguration configuration)
        {
            //Configuration = configuration;
            Configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();
        }

        public IConfigurationRoot Configuration { get; set; }
        //public static IConfiguration Configuration { get; private set; }

        // This method gets called by the runtime. Use this method to add services to the container
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAuthentication("Bearer")
            .AddJwtBearer(options =>
            {
                options.Audience = "2gq0d7i7a6rul0q0ai7l7dsids";
                options.Authority = "https://cognito-idp.us-west-2.amazonaws.com/us-west-2_5A1B8tLc9";     //https://cognito-idp.us-east-1.amazonaws.com/<userpoolID>
            });
            services.AddMvc();
            services.AddDefaultAWSOptions(Configuration.GetAWSOptions());
            services.AddAWSService<IAmazonDynamoDB>();
            services.AddSingleton<IGetDataInterface, GetDataModel>();
            services.AddSingleton<ICreateTable, CreateTable>();
            services.AddSingleton<IPutItem, PutItem>();
            Environment.SetEnvironmentVariable("AWS_ACCESS_KEY_ID", Configuration["AWS:AccessKey"]);
            Environment.SetEnvironmentVariable("AWS_SECRET_ACCESS_KEY", Configuration["AWS:SecretKey"]);
            Environment.SetEnvironmentVariable("AWS_REGION", Configuration["AWS:Region"]);
            services.AddSession(options => {
                options.IdleTimeout = TimeSpan.FromMinutes(10);
            });
           
            services.AddCors(setupAction =>
            setupAction.AddPolicy("MyPolicy",
                    builder => builder
                    .AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials()));
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            // This is your real test secret API key.
            StripeConfiguration.ApiKey = "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";
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }
            app.UseCors("MyPolicy");
            app.UseHttpsRedirection();
            app.UseSession();
            app.UseAuthentication();
            app.UseMvc();
        }
    }
}
