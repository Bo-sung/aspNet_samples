
using aspnetChat_server.Protocols;

namespace aspnetChat_server
{
    public class Program
    {
        private static WebApplication m_app = null;

        /// <summary>
        /// 웹 응용 프로그램을 빌드합니다.
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        private static WebApplication BuildWebApp(string[] args)
        {
            // 웹 응용 프로그램을 빌드합니다.
            var builder = WebApplication.CreateBuilder(args);

            // 컨테이너에 서비스를 추가합니다.
            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            // API 문서 생성기 추가
            builder.Services.AddEndpointsApiExplorer();
            // Swagger UI 추가
            // Swagger UI : Restful API를 테스트하고 문서화하는 오픈소스 프레임워크
            builder.Services.AddSwaggerGen();
            // SignalR 추가
            builder.Services.AddSignalR();

            return builder.Build();
        }

        public static void Main(string[] args)
        {
            // 빌드 시작
            m_app = BuildWebApp(args);
            if (m_app == null)
            {
                return;
            }

            // HTTP 요청 파이프라인 구성
            if (m_app.Environment.IsDevelopment())
            {
                m_app.UseSwagger();
                m_app.UseSwaggerUI();
                m_app.UseDeveloperExceptionPage();
            }

            // HTTP 요청 파이프라인 구성
            m_app.UseHttpsRedirection();

            // 인증 및 권한 부여
            m_app.UseAuthorization();

            // 컨트롤러 매핑
            m_app.MapControllers();

            // SignalR 라우팅  
            m_app.UseRouting();

            // SignalR 엔드포인트 매핑
            m_app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHub<ChatHub>(ChatHub.URL_HEADER);
            });

            // 웹 응용 프로그램 실행
            m_app.Run();
        }
    }
}
