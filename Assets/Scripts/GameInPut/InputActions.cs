namespace GameInput

{
    public partial class PlayerMovement
    {
        private static PlayerMovement _instance;

        public static PlayerMovement Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new PlayerMovement();
                    _instance.Enable();
                }

                return _instance;
            }
            private set => _instance = value;
        }
    }
}
