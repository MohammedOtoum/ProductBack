namespace ProductTask.Dto
{
    public class UserLogin
    {
        public string Email { get; set; }
        public string Password { get; set; }

        public UserLogin()
        {
            if (Email == null)
            {
                Email = " ";
            }
            if (Password == null)
            {
                Password = " ";
            }
        }
    }
}
