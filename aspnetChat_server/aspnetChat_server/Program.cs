
using aspnetChat_server.DB;
using aspnetChat_server.Protocols;
using System.Configuration;

namespace aspnetChat_server
{
    public class Program
    {
        private static WebApplication m_app = null;

        /// <summary>
        /// �� ���� ���α׷��� ����մϴ�.
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        private static WebApplication BuildWebApp(string[] args)
        {
            // �� ���� ���α׷��� ����մϴ�.
            var builder = WebApplication.CreateBuilder(args);

            // �����̳ʿ� ���񽺸� �߰��մϴ�.
            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            // API ���� ������ �߰�
            builder.Services.AddEndpointsApiExplorer();
            // Swagger UI �߰�
            // Swagger UI : Restful API�� �׽�Ʈ�ϰ� ����ȭ�ϴ� ���¼ҽ� �����ӿ�ũ
            builder.Services.AddSwaggerGen();
            // SignalR �߰�
            builder.Services.AddSignalR();

            InitDB(builder);

            return builder.Build();
        }

        private static void InitDB(WebApplicationBuilder builder)
        {
            // Ŀ�ؼ� ��Ʈ�� �ҷ�����
            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
            // MYSQL ���� �߰�
            DBManager.Instance.Init(builder);
        }

        public static void Main(string[] args)
        {
            // ��� ����
            m_app = BuildWebApp(args);
            if (m_app == null)
            {
                return;
            }

            // HTTP ��û ���������� ����
            if (m_app.Environment.IsDevelopment())
            {
                m_app.UseSwagger();
                m_app.UseSwaggerUI();
                m_app.UseDeveloperExceptionPage();
            }

            // HTTP ��û ���������� ����
            m_app.UseHttpsRedirection();

            // ���� �� ���� �ο�
            m_app.UseAuthorization();

            // ��Ʈ�ѷ� ����
            m_app.MapControllers();

            // SignalR �����  
            m_app.UseRouting();

            // SignalR ��������Ʈ ����
            m_app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHub<ChatHub>(ChatHub.URL_HEADER);
            });

            // �� ���� ���α׷� ����
            m_app.Run();
        }
    }
}
