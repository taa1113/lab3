using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Threading.Tasks;
using WebApplication1.Services;
using WebApplication1.Services.Interfaces;
using WebApplication1.Infrastructure;
using WebApplication1.Models.Forms;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace WebApplication1
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        public void ConfigureServices(IServiceCollection services)
        {
            // ��������� ����������� ��� ������� � �� � �������������� EF
            string connection = Configuration.GetConnectionString("SqlConnection");
            services.AddDbContext<EventsContext>(options => options.UseSqlServer(connection));

            // ���������� ��������� ������
            services.AddDistributedMemoryCache();
            services.AddSession();

            // ��������� ����
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
                {
                    options.Cookie.Name = "MyCookie";
                    options.ExpireTimeSpan = TimeSpan.FromDays(30); // ���������� ���� �������� ���� �� ������ ����������
                });

            // ���������� �����������
            services.AddMemoryCache();

            // ��������� ������������ CachedService
            services.AddScoped<ICachedCPEsService, CachedCPEsService>();
            services.AddScoped<ICachedEmployeesService, CachedEmployeesService>();
            services.AddScoped<ICachedEnterprisesService, CachedEnterprisesService>();
            services.AddScoped<ICachedEventsService, CachedEventsService>();
            services.AddScoped<ICachedManagersService, CachedManagersService>();
            services.AddScoped<ICachedPlannedEventsService, CachedPlannedEventsService>();
            services.AddScoped<ICachedSourceOfFinancingsService, CachedSourceOfFinancingsService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            
            app.UseRouting();
            app.UseStaticFiles();
            app.UseCookiePolicy();
            app.UseSession();
            app.UseAuthentication();

            app.Map("/info", UserInfo);
            app.Map("/searchform1", SearchForm1);
            app.Map("/searchform2", SearchForm);
            app.Map("/tables", tables =>
            {
                tables.Map("/Employees", Employees);
                tables.Map("/Enterprises", Enterprises);
                tables.Map("/Events", Events);
                tables.Map("/SourcesOfFinancing", SourcesOfFinancing);
                tables.Map("/PlannedEvents", PlannedEvents);

                //������
                tables.Run(async context =>
                {
                    string HTMLString = "<html>" +
                    "<head>" +
                    "<title> ������������ ����������������� �����������</title>" +
                    "<meta charset='utf-8'/>" +
                    "</head>" +
                    "<body>" +
                    "<a href='/tables/Employees'>Employees</a> <br>" +
                    "<a href='/tables/Enterprises'>Enterprises</a> <br>" +
                    "<a href='/tables/Events'>Events</a> <br>" +
                    "<a href='/tables/SourcesOfFinancing'>SourcesOfFinancing</a> <br>" +
                    "<a href='/tables/PlannedEvents'>PlannedEvents</a> <br>" +
                    "<a href='/tables'>�������</a>" +
                    "<a href='/'>�������</a>" +
                    "</body>" +
                    "</html>";
                    await context.Response.WriteAsync(HTMLString);
                });
            });

            //��������� ��������
            app.Run(async context => {
                string HTMLString = "<html>" +
                "<head>" +
                "<title>������������ ����������������� �����������</title>" +
                "<meta charset='utf-8'/>" +
                "</head>" +
                "<body>" +
                "<h1>������� ��������</h1>" +
                "<a href='/info'>����������</a> <br>" +
                "<a href='/tables'>�������</a> <br>" +
                "<a href='/searchform1'>����� 1</a> <br>" +
                "<a href='/searchform2'>����� 2</a>" +
                "</body>" +
                "</html>";
                
                await context.Response.WriteAsync(HTMLString);
            });
        }

        private static void UserInfo(IApplicationBuilder app)
        {
            app.Run(async (context) =>
            {
                // ������������ ������ ��� ������ 
                string strResponse = "<HTML><HEAD><TITLE>����������</TITLE></HEAD>" +
                "<META http-equiv='Content-Type' content='text/html; charset=utf-8'/>" +
                "<BODY><H1>����������:</H1>";
                strResponse += "<BR> ������: " + context.Request.Host;
                strResponse += "<BR> ����: " + context.Request.PathBase;
                strResponse += "<BR> ��������: " + context.Request.Protocol;
                strResponse += "<BR><A href='/'>�������</A></BODY></HTML>";
                // ����� ������
                await context.Response.WriteAsync(strResponse);
            });
        }

        private static void Employees(IApplicationBuilder app)
        {
            app.Run(async context =>
            {
                //��������� � �������
                ICachedEmployeesService cachedEmployeesService = context.RequestServices.GetService<ICachedEmployeesService>();
                IEnumerable<Employee> employees = cachedEmployeesService.GetEmployees("Employees20");

                string HtmlString = "<HTML><HEAD><TITLE>����������</TITLE></HEAD>" +
                    "<META http-equiv='Content-Type' content='text/html; charset=utf-8'/>" +
                    "<BODY><H1>������ �����������</H1>" +
                    "<TABLE BORDER=1>";
                HtmlString += "<TR>";
                HtmlString += "<TH>Id</TH>";
                HtmlString += "<TH>���</TH>";
                HtmlString += "<TH>�������</TH>";
                HtmlString += "<TH>��������</TH>";
                HtmlString += "<TH>�������</TH>";
                HtmlString += "<TH>���������</TH>";
                HtmlString += "</TR>";
                foreach (var employee in employees)
                {
                    HtmlString += "<TR>";
                    HtmlString += "<TD>" + employee.Id + "</TD>";
                    HtmlString += "<TD>" + employee.Name + "</TD>";
                    HtmlString += "<TD>" + employee.Surname + "</TD>";
                    HtmlString += "<TD>" + employee.MiddleName + "</TD>";
                    HtmlString += "<TD>" + employee.Phone + "</TD>";
                    HtmlString += "<TD>" + employee.Position + "</TD>";
                    HtmlString += "</TR>";
                }
                HtmlString += "</TABLE>";
                HtmlString += "<BR><A href='/'>�������</A></BR>";
                HtmlString += "</BODY></HTML>";

                // ����� ������
                await context.Response.WriteAsync(HtmlString);
            });
        }

        private static void Enterprises(IApplicationBuilder app)
        {
            app.Run(async context =>
            {
                //��������� � ��������
                ICachedEmployeesService cachedEmployeesService = context.RequestServices.GetService<ICachedEmployeesService>();
                IEnumerable<Employee> employees = cachedEmployeesService.GetEmployees("Employees20");
                ICachedEnterprisesService cachedEnterprisesService = context.RequestServices.GetService<ICachedEnterprisesService>();
                IEnumerable<Enterprise> enterprises = cachedEnterprisesService.GetEnterprises("Enterprises20");

                string HtmlString = "<HTML><HEAD><TITLE>�����������</TITLE></HEAD>" +
                    "<META http-equiv='Content-Type' content='text/html; charset=utf-8'/>" +
                    "<BODY><H1>������ �����������</H1>" +
                    "<TABLE BORDER=1>";
                HtmlString += "<TR>";
                HtmlString += "<TH>Id</TH>";
                HtmlString += "<TH>��������</TH>";
                HtmlString += "<TH>����� �������������</TH>";
                HtmlString += "<TH>�����</TH>";
                HtmlString += "<TH>������� ���������</TH>";
                HtmlString += "<TH>��� ���������</TH>";
                HtmlString += "<TH>�������� ���������</TH>";
                HtmlString += "<TH>������� ���������</TH>";
                HtmlString += "<TH>������� ��. ����������</TH>";
                HtmlString += "<TH>��� ��. ����������</TH>";
                HtmlString += "<TH>�������� ��. ����������</TH>";
                HtmlString += "<TH>������� ��. ����������</TH>";
                HtmlString += "</TR>";
                foreach (var enterprise in enterprises)
                {
                    HtmlString += "<TR>";
                    HtmlString += "<TD>" + enterprise.Id + "</TD>";
                    HtmlString += "<TD>" + enterprise.Name + "</TD>";
                    HtmlString += "<TD>" + enterprise.OwnershipForm + "</TD>";
                    HtmlString += "<TD>" + enterprise.Adress + "</TD>";
                    HtmlString += "<TD>" + enterprise.Manager.Surname + "</TD>";
                    HtmlString += "<TD>" + enterprise.Manager.Name + "</TD>";
                    HtmlString += "<TD>" + enterprise.Manager.MiddleName + "</TD>";
                    HtmlString += "<TD>" + enterprise.Manager.Phone + "</TD>";
                    HtmlString += "<TD>" + enterprise.CPE.Surname + "</TD>";
                    HtmlString += "<TD>" + enterprise.CPE.Name + "</TD>";
                    HtmlString += "<TD>" + enterprise.CPE.MiddleName + "</TD>";
                    HtmlString += "<TD>" + enterprise.CPE.Phone + "</TD>";
                    HtmlString += "</TR>";
                }
                HtmlString += "</TABLE>";
                HtmlString += "<BR><A href='/'>�������</A></BR>";
                HtmlString += "</BODY></HTML>";

                // ����� ������
                await context.Response.WriteAsync(HtmlString);
            });
        }
        private static void Events(IApplicationBuilder app)
        {
            app.Run(async context =>
            {
                //��������� � �������
                ICachedEventsService cachedEventsService = context.RequestServices.GetService<ICachedEventsService>();
                IEnumerable<Event> events = cachedEventsService.GetEvents("Events20");

                string HtmlString = "<HTML><HEAD><TITLE>�����������</TITLE></HEAD>" +
                    "<META http-equiv='Content-Type' content='text/html; charset=utf-8'/>" +
                    "<BODY><H1>������ �����������</H1>" +
                    "<TABLE BORDER=1>";
                HtmlString += "<TR>";
                HtmlString += "<TH>Id</TH>";
                HtmlString += "<TH>��������</TH>";
                HtmlString += "<TH>������� ���������</TH>";
                HtmlString += "</TR>";
                foreach (var e in events)
                {
                    HtmlString += "<TR>";
                    HtmlString += "<TD>" + e.Id + "</TD>";
                    HtmlString += "<TD>" + e.Name + "</TD>";
                    HtmlString += "<TD>" + e.Unit + "</TD>";
                    HtmlString += "</TR>";
                }
                HtmlString += "</TABLE>";
                HtmlString += "<BR><A href='/'>�������</A></BR>";
                HtmlString += "</BODY></HTML>";

                // ����� ������
                await context.Response.WriteAsync(HtmlString);
            });
        }
        private static void SourcesOfFinancing(IApplicationBuilder app)
        {
            app.Run(async context =>
            {
                //��������� � �������
                ICachedSourceOfFinancingsService cachedFinancingsService = context.RequestServices.GetService<ICachedSourceOfFinancingsService>();
                IEnumerable<SourceOfFinancing> financings = cachedFinancingsService.GetSourceOfFinancings("SourceOfFinansings20");

                string HtmlString = "<HTML><HEAD><TITLE>��������� ��������������</TITLE></HEAD>" +
                    "<META http-equiv='Content-Type' content='text/html; charset=utf-8'/>" +
                    "<BODY><H1>������ ��������������</H1>" +
                    "<TABLE BORDER=1>";
                HtmlString += "<TR>";
                HtmlString += "<TH>Id</TH>";
                HtmlString += "<TH>�����������</TH>";
                HtmlString += "<TH>����������� �����������</TH>";
                HtmlString += "<TH>���� ������������ ����������</TH>";
                HtmlString += "<TH>��������������� ������</TH>";
                HtmlString += "<TH>��������� ������</TH>";
                HtmlString += "</TR>";
                foreach (var f in financings)
                {
                    HtmlString += "<TR>";
                    HtmlString += "<TD>" + f.Id + "</TD>";
                    HtmlString += "<TD>" + f.Enterprise + "</TD>";
                    HtmlString += "<TD>" + f.Organisation + "</TD>";
                    HtmlString += "<TD>" + f.Ministry + "</TD>";
                    HtmlString += "<TD>" + f.RepublicBudget + "</TD>";
                    HtmlString += "<TD>" + f.LocalBudget + "</TD>";
                    HtmlString += "</TR>";
                }
                HtmlString += "</TABLE>";
                HtmlString += "<BR><A href='/'>�������</A></BR>";
                HtmlString += "</BODY></HTML>";

                // ����� ������
                await context.Response.WriteAsync(HtmlString);
            });
        }
        private static void PlannedEvents(IApplicationBuilder app)
        {
            app.Run(async context =>
            {
                //��������� � ��������
                ICachedEmployeesService cachedEmployeesService = context.RequestServices.GetService<ICachedEmployeesService>();
                IEnumerable<Employee> employees = cachedEmployeesService.GetEmployees("Employees20");
                ICachedSourceOfFinancingsService cachedFinancingsService = context.RequestServices.GetService<ICachedSourceOfFinancingsService>();
                IEnumerable<SourceOfFinancing> financings = cachedFinancingsService.GetSourceOfFinancings("SourceOfFinansings20");
                ICachedEventsService cachedEventsService = context.RequestServices.GetService<ICachedEventsService>();
                IEnumerable<Event> events = cachedEventsService.GetEvents("Events20");
                ICachedPlannedEventsService cachedPlannedEventsService = context.RequestServices.GetService<ICachedPlannedEventsService>();
                IEnumerable<PlannedEvent> plannedEvents = cachedPlannedEventsService.GetPlannedEvents("PlannedEvents20");
                ICachedEnterprisesService cachedEnterprisesService = context.RequestServices.GetService<ICachedEnterprisesService>();
                IEnumerable<Enterprise> enterprises = cachedEnterprisesService.GetEnterprises("Enterprises20");

                string HtmlString = "<HTML><HEAD><TITLE>����������� �����������</TITLE></HEAD>" +
                    "<META http-equiv='Content-Type' content='text/html; charset=utf-8'/>" +
                    "<BODY><H1>������ ����������� �����������</H1>" +
                    "<TABLE BORDER=1>";
                HtmlString += "<TR>";
                HtmlString += "<TH>Id</TH>";
                HtmlString += "<TH>��������</TH>";
                HtmlString += "<TH>������� ���������</TH>";
                HtmlString += "<TH>�����������</TH>";
                HtmlString += "<TH>������</TH>";
                HtmlString += "<TH>�����</TH>";
                HtmlString += "<TH>�����</TH>";
                HtmlString += "<TH>�������</TH>";
                HtmlString += "<TH>������������� ������</TH>";
                HtmlString += "<TH>������� ��������������</TH>";
                HtmlString += "<TH>��� ��������������</TH>";
                HtmlString += "<TH>�������� ��������������</TH>";
                HtmlString += "<TH>������� ��������������</TH>";
                HtmlString += "<TH>�������� �����������</TH>";
                HtmlString += "<TH>�������� ����������� �����������</TH>";
                HtmlString += "<TH>���� ������������</TH>";
                HtmlString += "<TH>��������������� ������</TH>";
                HtmlString += "<TH>��������� ������</TH>";
                HtmlString += "</TR>";
                foreach (var ev in plannedEvents)
                {
                    HtmlString += "<TR>";
                    HtmlString += "<TD>" + ev.Id + "</TD>";
                    HtmlString += "<TD>" + (ev.Event != null ? ev.Event.Name : "N/A") + "</TD>";
                    HtmlString += "<TD>" + (ev.Event != null ? ev.Event.Unit : "N/A") + "</TD>";
                    HtmlString += "<TD>" + (ev.Enterprise != null ? ev.Enterprise.Name : "N/A") + "</TD>";
                    HtmlString += "<TD>" + ev.DateOfStart + "</TD>";
                    HtmlString += "<TD>" + ev.DateOfEnd + "</TD>";
                    HtmlString += "<TD>" + ev.Scope + "</TD>";
                    HtmlString += "<TD>" + ev.Expenses + "</TD>";
                    HtmlString += "<TD>" + ev.EconomicEffect + "</TD>";
                    HtmlString += "<TD>" + (ev.Responsible != null ? ev.Responsible.Surname : "N/A") + "</TD>";
                    HtmlString += "<TD>" + (ev.Responsible != null ? ev.Responsible.Name : "N/A") + "</TD>";
                    HtmlString += "<TD>" + (ev.Responsible != null ? ev.Responsible.MiddleName : "N/A") + "</TD>";
                    HtmlString += "<TD>" + (ev.Responsible != null ? ev.Responsible.Phone : "N/A") + "</TD>";
                    HtmlString += "<TD>" + (ev.Finance != null ? ev.Finance.Enterprise : "N/A") + "</TD>";
                    HtmlString += "<TD>" + (ev.Finance != null ? ev.Finance.Organisation : "N/A") + "</TD>";
                    HtmlString += "<TD>" + (ev.Finance != null ? ev.Finance.Ministry : "N/A") + "</TD>";
                    HtmlString += "<TD>" + (ev.Finance != null ? ev.Finance.RepublicBudget : "N/A") + "</TD>";
                    HtmlString += "<TD>" + (ev.Finance != null ? ev.Finance.LocalBudget : "N/A") + "</TD>";
                    HtmlString += "</TR>";
                }
                HtmlString += "</TABLE>";
                HtmlString += "<BR><A href='/'>�������</A></BR>";
                HtmlString += "</BODY></HTML>";

                // ����� ������
                await context.Response.WriteAsync(HtmlString);
            });
        }

        private static void SearchForm(IApplicationBuilder app)
        {
            app.Run(async context =>
            {
                //��������� ������ �� ������
                SearchForm2 form = context.Session.Get<SearchForm2>("form") ?? new SearchForm2();

                string HTMLString = "<html><head>" +
                "<meta charset='utf-8'></head>" +
                "<body><form action=" + "/searchform2" + " / >" +
                "������������ �����������:<br><input type = 'text' name = 'eventName' placeholder='������������ �����������' list='eventsList' value = " + form.EventName + ">" +
                "<br>����������� �����:<br><input type = 'string' name = 'minScope' placeholder='����������� �����' value = " + form.MinScope + " > <br>" +
                "������� ���������:<br>" +
                "<select name='unit' size='4' value=" + form.Unit + ">" +
                "<option value='proc'>����</option>" +
                "<option value='chel'>���</option>" +
                "</select>" +
                "<datalist id='eventsList'>" +
                "<option value='1'>" +
                "<option value='2'>" +
                "</datalist>" +
                "<br><br><input type = 'submit' value = 'Submit' >" +
                "</form>" +
                "<a href='/'>�������</a>" +
                "</body></html>";

                //���������� ������ � ������
                form.EventName = context.Request.Query["eventName"];
                form.MinScope = context.Request.Query["minScope"];
                form.Unit = context.Request.Query["unit"];
                context.Session.Set<SearchForm2>("form", form);

                await context.Response.WriteAsync(HTMLString);
            });
        }

        private static void SearchForm1(IApplicationBuilder app)
        {
            app.Run(async context =>
            {
                CookieOptions options = new CookieOptions();
                options.Expires = DateTime.Now.AddMinutes(5);

                string eventName = context.Request.Cookies["eventName"] ?? "";
                string minScope = context.Request.Cookies["minScope"] ?? "";
                string unit = context.Request.Cookies["unit"] ?? "";
                
                string HTMLString = "<html><head>" +
                "<meta charset='utf-8'></head>" +
                "<body><form action=" + "/searchform1" + " / >" +
                "������������ �����������:<br><input type = 'text' name = 'eventName' placeholder='������������ �����������' list='eventsList' value = " + eventName + ">" +
                "<br>����������� �����:<br><input type = 'string' name = 'minScope' placeholder='����������� �����' value = " + minScope + " > <br>" +
                "������� ���������:<br>" +
                "<select name='unit' size='4' value=" + unit + ">" +
                "<option value='proc'>����</option>" +
                "<option value='chel'>���</option>" +
                "</select>" +
                "<datalist id='eventsList'>" +
                "<option value='1'>" +
                "<option value='2'>" +
                "</datalist>" +
                "<br><br><input type = 'submit' value = 'Submit' >" +
                "</form>" +
                "<a href='/'>�������</a>" +
                "</body></html>";

                eventName = context.Request.Query["eventName"];
                minScope = context.Request.Query["minScope"];
                unit = context.Request.Query["unit"];
                if (eventName != null)
                {
                    context.Response.Cookies.Append("eventName", eventName, options);
                    context.Response.Cookies.Append("minScope", minScope, options);
                    context.Response.Cookies.Append("unit", unit, options);
                }

                await context.Response.WriteAsync(HTMLString);
            });
        }
    }
}
