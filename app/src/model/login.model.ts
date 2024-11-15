export interface Login {
    isValid: true;
    message: string;
    email: string;                    // Email address of the student
    studentPassword: string;          // Password of the student (will be encrypted on the backend)
}
export interface UserMasterModel {
    firstName: string;
    lastName: string;
    userName: string;
    email: string;
    userPassword: string;
    dateOfBirth: string;
    gender: string;
    userTypeID: number;
    userMasterStatus: number;
  }
  