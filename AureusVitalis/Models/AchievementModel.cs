namespace AureusVitalis.Models
{
    /// Представляет достижение пользователя в приложении.
    public class AchievementModel
    {
        /// Заголовок достижения.
        public string Title { get; set; } = null!;
        
        /// Описание, за что даётся достижение.
        public string Description { get; set; } = null!;
        
        /// Путь к иконке (трофею) для данного достижения.
        public string Icon { get; set; } = null!;
        
        /// Ключ модуля/блока, по которому хранится прогресс в таблице UserProgress.ModuleKey.
        public string ModuleKey { get; set; } = null!;
        
        /// Флаг, разблокировано ли достижение (модуль пройден пользователем).
        public bool IsUnlocked { get; set; }
    }
}