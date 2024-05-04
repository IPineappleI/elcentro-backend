namespace TangoSchool.ApplicationServices.Constants;

public static class EmailConstants
{
    public static class ResetPassword
    {
        public const string Subject = "Reset Your Password";

        public const string Body = """
                                   <!DOCTYPE html>
                                   <html lang="en">

                                   <head>
                                       <meta name="viewport" content="width=device-width, initial-scale=1">
                                   </head>

                                   <body style="width:100%">
                                       <div style="width:100% ">
                                           <div>Dear {0},</div>
                                           <div>We received a request to reset your password for {1} account.</div>
                                           <div>Password reset token: {2}</div>
                                           <div>If you did not request a password reset, please disregard this email. Your account will remain secure.</div>
                                           <div>Best Regards.</div>
                                       </div>
                                   </body>

                                   </html>
                                   """;
    }

    public static class SuccessRegistration
    {
        public const string Subject = "Welcome to our school";

        public const string Body = """
                                   <!DOCTYPE html>
                                   <html lang="en">
                                   
                                   <head>
                                       <meta name="viewport" content="width=device-width, initial-scale=1">
                                   </head>
                                   
                                   <body style="width:100%">
                                       <div style="width:100% ">
                                           <div>Dear {0},</div>
                                           <div>Your account is created.</div>
                                           <div>Your password: {1}</div>
                                           <div>Best Regards.</div>
                                       </div>
                                   </body>
                                   
                                   </html>
                                   """;
    }
}
