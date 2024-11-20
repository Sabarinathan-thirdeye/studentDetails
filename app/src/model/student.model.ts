// Define the structure for the StudentDetail model
export interface StudentDetail {
  studentID: number;                // Unique identifier for the student
  firstName: string;                // First name of the student
  lastName: string ;                 // Last name of the student
  userName: string;                 // Username generated for the student
  dateOfBirth: string;              // Date of birth in ISO string format
  gender: string;                   // Gender of the student (e.g., "Male", "Female", "Other")
  email: string;                    // Email address of the student
  mobileNumber: number;             // Mobile number of the student
  createdOn?: string;               // Date when the student record was created (optional, in ISO string format)
  createdBy?: number;               // ID of the user who created the student record (optional)
  modifiedOn?: string;              // Date when the student record was last modified (optional, in ISO string format)
  modifiedBy?: number;              // ID of the user who last modified the student record (optional)
  studentstatus: number;            // Status of the student (e.g., 1 for active, 99 for inactive)
}


