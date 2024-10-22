using MySql.Data.MySqlClient;

namespace aspnetChat_server.DB
{
    // DBManager 클래스, 멀티스레드 환경에서 안전한 싱글톤 패턴으로 구현
    public class DBManager
    {
        // 싱글턴 에리어
        private static DBManager _instance = null;
        private static readonly object _lock = new object();

        // 초기화 여부
        private bool m_isInit = false;
        // DBMessage 객체
        private DBMessage _dbMessage = null;
        // DB private 
        private DB<ChatMessage_data> _dbChat;
        public DBMessage DBMessage { get => _dbMessage; }
        // DB public
        public DB<ChatMessage_data> DBChat { get => _dbChat; }

        // 생성자
        private DBManager()
        {
        }

        // 인스턴스 반환
        public static DBManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_lock)
                    {
                        if (_instance == null)
                        {
                            _instance = new DBManager();
                        }
                    }
                }
                return _instance;
            }
        }

        public void Init(WebApplicationBuilder builder)
        {
            string connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
            if (m_isInit)
                return;
            m_isInit = true;
            _dbMessage = new DBMessage();
            // 객체 초기화
            _dbChat = new DB<ChatMessage_data>(connectionString);

            // 서비스 등록
            builder.Services.Add(new ServiceDescriptor(_dbChat.GetType(), _dbChat));
        }
    }
}
