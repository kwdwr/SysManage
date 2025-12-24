using SyllabusManager.App.Data;
using SyllabusManager.App.Interfaces;
using SyllabusManager.App.Services;

namespace SyllabusManager.App.Factories
{
    // FACTORY PATTERN: Centralized Service Creation
    public static class ServiceFactory
    {
        private static IDataRepository _repository;
        private static INotificationService _notificationService;
        private static IAuthorizationService _authService;
        private static IVersionControlService _vcsService;
        private static SyllabusService _syllabusService;

        public static IDataRepository GetDataRepository()
        {
            if (_repository == null)
            {
                _repository = new JsonDataRepository();
            }
            return _repository;
        }

        public static INotificationService GetNotificationService()
        {
            if (_notificationService == null)
            {
                var repo = GetDataRepository();
                var service = new NotificationService(repo);
                
                // WIRING ADAPTER
                service.AddChannel(new EmailAdapter());
                
                _notificationService = service;
            }
            return _notificationService;
        }

        public static IAuthorizationService GetAuthorizationService()
        {
            if (_authService == null) _authService = new AuthorizationService();
            return _authService;
        }

        public static IVersionControlService GetVersionControlService()
        {
            if (_vcsService == null) _vcsService = new VersionControlService();
            return _vcsService;
        }

        public static SyllabusService GetSyllabusService()
        {
            if (_syllabusService == null)
            {
                _syllabusService = new SyllabusService(
                    GetDataRepository(),
                    GetAuthorizationService(),
                    GetVersionControlService(),
                    GetNotificationService()
                );
            }
            return _syllabusService;
        }
    }
}
