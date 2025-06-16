namespace ProductTask.Dto
{
    public class UserConfirm
    {
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }

        public UserConfirm()
        {
            if (PasswordHash == null)
            {
                PasswordHash = new byte[0];
            }
            if (PasswordSalt == null)
            {
                PasswordSalt = new byte[0];
            }
        }
    }
}
