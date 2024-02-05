export interface User {
    Username: string;
    EmailAddress: string;
    PasswordSalt: string;
    PasswordHash: string;
    Photo: string;
    Banner: string;
    Role: string;
}