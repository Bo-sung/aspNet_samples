﻿namespace aspnetChat_server.DB
{
    // DBManager 클래스, 멀티스레드 환경에서 안전한 싱글톤 패턴으로 구현
    public class DBManager
    {
        private static DBManager _instance = null;
        private static readonly object _lock = new object();
        // DBMessage 객체
        private DBMessage _dbMessage = null;
        public DBMessage DBMessage { get => _dbMessage; }

        // 생성자
        private DBManager()
        {
            _dbMessage = new DBMessage();
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
    }
}
