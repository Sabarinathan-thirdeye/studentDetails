// Define the structure for the StudentDetail model
export interface StudentDetail {
  studentID: number;                // Unique identifier for the student
  firstName: string;                // First name of the student
  lastName?: string;                // Last name of the student (optional)
  dateOfBirth?: string;             // Date of birth in string format (optional, will parse on display)
  gender?: string;                  // Gender of the student (optional)
  email: string;                    // Email address of the student
  mobileNumber?: string;            // Mobile number of the student (optional)
  createdOn?: string;               // Date when the student record was created (optional, in string format)
  createBy?: number;                // ID of the user who created the student record (optional)
  modifiedOn?: string;              // Date when the student record was last modified (optional, in string format)
  modifiedBy?: number;              // ID of the user who last modified the student record (optional)
  studentPassword: string;          // Password of the student (will be encrypted on the backend)
  confirmPassword: string;          // Confirmation of the password entered by the student
  studentstatus?: number;           // Status of the student (optional, e.g., active, inactive)
}
