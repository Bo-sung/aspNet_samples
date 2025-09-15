using aspnetChat_server.DB;
using aspnetChat_server.Protocols;
using System.Configuration;

namespace aspnetChat_server
{
    public class Program
    {
        private static WebApplication m_app = null;

        /// <summary>
        /// 웹 어플리케이션을 빌드합니다.
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        private static WebApplication BuildWebApp(string[] args)
        {
            // 웹 어플리케이션을 빌드합니다.
            var builder = WebApplication.CreateBuilder(args);

            // 컨테이너에 서비스를 추가합니다.
            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            // API 탐색기 추가
            builder.Services.AddEndpointsApiExplorer();
            // Swagger UI 추가
            // Swagger UI : Restful API를 테스트하고 문서화하는 그래픽 사용자 인터페이스
            builder.Services.AddSwaggerGen();
            // SignalR 추가
            builder.Services.AddSignalR();

            InitDB(builder);

            return builder.Build();
        }

        private static void InitDB(WebApplicationBuilder builder)
        {
            // 커넥션 스트링을 가져옵니다.
            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
            // MYSQL 연결 추가
            DBManager.Instance.Init(builder);
        }

        public static void Main(string[] args)
        {
            // 앱 빌드
            m_app = BuildWebApp(args);
            if (m_app == null)
            {
                return;
            }

            // HTTP 요청 파이프라인을 구성합니다.
            if (m_app.Environment.IsDevelopment())
            {
                m_app.UseSwagger();
                m_app.UseSwaggerUI();
                m_app.UseDeveloperExceptionPage();
            }

            // HTTP 요청 파이프라인을 구성합니다.
            m_app.UseHttpsRedirection();

            // 인증 및 권한 부여
            m_app.UseAuthorization();

            // 컨트롤러를 매핑합니다.
            m_app.MapControllers();

            // SignalR 라우팅
            m_app.UseRouting();

            // SignalR 엔드포인트를 매핑합니다.
            m_app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHub<ChatHub>(ChatHub.URL_HEADER);
            });

            // 앱을 실행합니다.
            m_app.Run();
        }
    }
}