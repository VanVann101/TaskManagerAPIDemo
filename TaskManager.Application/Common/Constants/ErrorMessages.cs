namespace TaskManager.Application.Common.Constants {
    public static class ErrorMessages {
        public const string NotFound = "Entity not found";

        //Users validation messages
        public const string FirstNameRequired = "First name is required.";
        public const string FirstNameMaxLength = "First name cannot be longer than 50 characters.";
        public const string LastNameRequired = "Last name is required.";
        public const string LastNameMaxLength = "Last name cannot be longer than 50 characters.";
        public const string EmailRequired = "Email is required.";
        public const string EmailInvalid = "Invalid email address.";
        public const string PasswordRequired = "Password is required.";
        public const string PasswordMinLength = "Password must be at least 8 characters long.";
        public const string PhoneInvalid = "Invalid phone number.";

        public const string InvalidAuthData = "The user with the specified email address and password was not found.";

        ///Task validation messages
        public const string TaskNameRequired = "The task name is required.";
        public const string TaskNameMaxLength = "The task name must not exceed 100 characters.";
        public const string TaskDescriptionMaxLength = "The description must not exceed 500 characters.";
    }
}
