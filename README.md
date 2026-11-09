# Custom Garment Design and Order Tracking

## Project Summary

Custom Garment Design and Order Tracking is a mobile application developed using C# and .NET MAUI. The project provides a user-friendly interface for account management and garment catalog operations.

The application allows users to register, log in, search for garments, add new garment records, edit existing records, delete garments, and view individual garment summaries.

## Technologies Used

- C#
- .NET MAUI
- XAML
- SQLite
- Visual Studio
- GitHub

## Features

### User Authentication
- User registration and login
- Password complexity validation
- Password hashing with PBKDF2 and unique salts
- Password change functionality
- Administrator and customer account recognition
- Basic in-memory session management
- Logout functionality

### Garment Management
- Add new garments
- View saved garments
- Edit garment information
- Delete garments with confirmation
- Search by garment name
- Wildcard search support
- SQLite data persistence
- Automatic initialization of starter garments when the catalog is empty

### Application Navigation
- Login and registration pages
- Garment catalog and search interface
- Individual garment summary page
- Change password page

## Security

The application uses salted PBKDF2 password hashing rather than storing plaintext passwords. Registration includes password complexity requirements, and login verifies user credentials before starting a session.

The application also includes administrator account recognition, logout functionality, and basic authentication checks for protected pages and garment modification actions.

This is an educational prototype. Administrator-only permissions and customer-specific garment ownership are not fully implemented.

## Improvements

The final development phase introduced a garment summary page connected to the catalog, improved the visual presentation of garment details, and strengthened session checks around garment modification operations.

The final interface maintains a consistent dark theme with improved spacing, typography, and navigation.

## Demonstration Video
https://youtu.be/e-k1_nD4sck

Video link: **To be added after recording**

## Developer

Developed as a Software Development capstone project.
