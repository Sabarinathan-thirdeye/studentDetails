export interface Login {
    isValid: true;
    message: string;
    email: string;                    // Email address of the student
    studentPassword: string;          // Password of the student (will be encrypted on the backend)
}