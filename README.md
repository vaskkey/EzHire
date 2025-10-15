# EzHire - Recruitment Management System

EzHire is a full-stack recruitment management system designed to streamline the hiring process. It provides tools for managing recruitment campaigns, job postings, candidate applications, and the complete recruitment pipeline from application to offer.

## 🏗️ Architecture Overview

EzHire follows a client-server architecture with a clear separation between frontend and backend:

```
┌─────────────────────────────────────────────────┐
│              Frontend (Next.js)                 │
│  ┌──────────────────────────────────────────┐  │
│  │  Pages & Components                      │  │
│  │  - Authentication UI                     │  │
│  │  - Campaign Management                   │  │
│  │  - Job Postings                          │  │
│  │  - Candidate Applications                │  │
│  └──────────────────────────────────────────┘  │
│               ↓ HTTP/REST                       │
└─────────────────────────────────────────────────┘
                    ↓
┌─────────────────────────────────────────────────┐
│         Backend (ASP.NET Core API)              │
│  ┌──────────────────────────────────────────┐  │
│  │  Controllers Layer                       │  │
│  │  - Auth, Campaigns, Postings, etc.      │  │
│  └──────────────────────────────────────────┘  │
│               ↓                                 │
│  ┌──────────────────────────────────────────┐  │
│  │  Services Layer (Business Logic)        │  │
│  └──────────────────────────────────────────┘  │
│               ↓                                 │
│  ┌──────────────────────────────────────────┐  │
│  │  Repositories Layer (Data Access)       │  │
│  └──────────────────────────────────────────┘  │
│               ↓                                 │
│  ┌──────────────────────────────────────────┐  │
│  │  Entity Framework Core + DbContext      │  │
│  └──────────────────────────────────────────┘  │
└─────────────────────────────────────────────────┘
                    ↓
┌─────────────────────────────────────────────────┐
│           PostgreSQL Database                   │
└─────────────────────────────────────────────────┘
```

## 📁 Project Structure

```
EzHire/
├── ezhire_api/                     # Backend API
│   ├── ezhire_api/                 # Main API project
│   │   ├── Controllers/            # API endpoints
│   │   ├── Services/               # Business logic layer
│   │   ├── Repositories/           # Data access layer
│   │   ├── Models/                 # Domain entities
│   │   ├── DTO/                    # Data Transfer Objects
│   │   ├── DAL/                    # Database context
│   │   ├── Middlewares/            # Custom middleware
│   │   ├── Validators/             # Input validation
│   │   ├── Exceptions/             # Custom exceptions
│   │   ├── Migrations/             # EF Core migrations
│   │   ├── Dockerfile              # Container configuration
│   │   └── Program.cs              # Application entry point
│   └── ezhire_api_unit_tests/      # Unit tests
├── ezhire_fe/                      # Frontend application
│   ├── app/                        # Next.js App Router pages
│   │   ├── auth/                   # Authentication pages
│   │   ├── campaigns/              # Campaign management
│   │   └── context/                # React context providers
│   ├── components/                 # Reusable React components
│   ├── api/                        # API client & TypeScript types
│   └── public/                     # Static assets
└── README.md                       # This file
```

## 🔧 Technology Stack

### Backend (ezhire_api)
- **Framework**: ASP.NET Core 9.0
- **Language**: C# (.NET 9)
- **Database**: PostgreSQL
- **ORM**: Entity Framework Core 9.0
- **Authentication**: ASP.NET Core Identity with cookie-based auth
- **API Documentation**: OpenAPI/Swagger
- **Containerization**: Docker

### Frontend (ezhire_fe)
- **Framework**: Next.js 15.3 (App Router)
- **Language**: TypeScript 5.8
- **UI Library**: React 19
- **Styling**: Tailwind CSS 4 + DaisyUI 5
- **HTTP Client**: openapi-fetch (type-safe API client)
- **Icons**: Lucide React
- **Package Manager**: Bun

## 🗄️ Database Schema

### Core Entities

#### Users & Authentication
- **User**: Base user entity (extends IdentityUser)
- **HiringManager**: User role for hiring managers
- **Recruiter**: User role for recruiters

#### Recruitment Process
- **RecruitmentCampaign**: Top-level entity for recruitment initiatives
- **JobPosting**: Published job openings linked to campaigns
- **Candidate**: Potential employees with their information
- **Experience**: Candidate work history
- **JobApplication**: Links candidates to job postings with status tracking

#### Recruitment Stages & Meetings
- **RecruitmentStage**: Configurable stages in the hiring process
- **RecruitmentStageMeeting**: Base meeting entity
- **TechnicalMeeting**: Technical interview details
- **TeamMeeting**: Team fit interview details
- **CultureMeeting**: Culture fit interview details

#### Offers
- **Offer**: Job offers extended to candidates

### Key Relationships
- Campaigns have multiple JobPostings
- JobPostings have multiple JobApplications
- Candidates have multiple Experiences and JobApplications
- JobApplications progress through RecruitmentStages
- RecruitmentStages have associated Meetings
- Successful applications result in Offers

## 🎯 Key Features

### Backend Architecture Patterns

#### Layered Architecture
The backend follows a clean, layered architecture:

1. **Controllers Layer**: Handles HTTP requests/responses
   - AuthController
   - RecruitmentCampaignsController
   - JobPostingsController
   - CandidatesController
   - JobApplicationsController
   - RecruitmentStagesController

2. **Services Layer**: Implements business logic
   - AuthService
   - RecruitmentCampaignsService
   - JobPostingsService
   - CandidatesService
   - JobApplicationsService
   - RecruitmentStagesService

3. **Repository Layer**: Abstracts data access
   - Follows Repository pattern for data operations
   - Uses Entity Framework Core for ORM

#### Cross-Cutting Concerns
- **Middleware**: Custom exception handling middleware
- **Validation**: FluentValidation for input validation
- **CORS**: Configured to allow frontend on localhost:3000
- **Timestamps**: Automatic `created_at` and `updated_at` tracking

### Frontend Architecture

#### Next.js App Router
- Server and client components for optimal performance
- File-based routing with dynamic routes
- React Server Components by default

#### Type Safety
- Full TypeScript implementation
- Auto-generated API types from OpenAPI spec
- Type-safe API client using openapi-fetch

#### Authentication
- Cookie-based session management
- Protected routes with Auth component wrapper
- Integration with backend Identity system

#### UI/UX
- Responsive design with Tailwind CSS
- Pre-built components from DaisyUI
- Theme support (light/dark mode)

## 🔐 Authentication & Authorization

### Backend
- **ASP.NET Core Identity** for user management
- **Cookie-based authentication** for stateful sessions
- Password requirements:
  - Minimum 8 characters
  - Requires digit, uppercase, and lowercase
  - Optional special characters
- Lockout policy: 5 failed attempts = 5-minute lockout
- Session expiry: 30 minutes with sliding expiration

### Frontend
- Session cookies with `credentials: 'include'`
- Protected routes via Auth wrapper component
- Automatic redirect to login for unauthenticated users

## 🚀 Getting Started

### Prerequisites
- .NET SDK 9.0+
- Node.js 20+ or Bun
- PostgreSQL 13+
- Docker (optional, for containerized deployment)

### Backend Setup

1. Navigate to the API directory:
```bash
cd ezhire_api
```

2. Configure the database connection in `appsettings.Development.json`:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Database=ezhire;Username=your_user;Password=your_password"
  }
}
```

3. Apply database migrations:
```bash
dotnet ef database update
```

4. Run the API:
```bash
dotnet run
```

The API will be available at `http://localhost:5011`

### Frontend Setup

1. Navigate to the frontend directory:
```bash
cd ezhire_fe
```

2. Install dependencies:
```bash
bun install
# or
npm install
```

3. Configure the API URL in `.env.local`:
```
NEXT_PUBLIC_API_URL=http://localhost:5011
```

4. Run the development server:
```bash
bun dev
# or
npm run dev
```

The frontend will be available at `http://localhost:3000`

## 🐳 Docker Deployment

The backend includes a multi-stage Dockerfile for production deployment:

```bash
cd ezhire_api/ezhire_api
docker build -t ezhire-api .
docker run -p 8080:8080 ezhire-api
```

## 📚 API Documentation

When running in development mode, the API exposes OpenAPI documentation:
- OpenAPI JSON: `http://localhost:5011/openapi/v1.json`

### Type Generation for Frontend

The frontend uses auto-generated TypeScript types from the OpenAPI spec:

```bash
cd ezhire_fe
bun schema
```

This generates `api/v1.d.ts` with all API types and endpoints.

## 🔄 Development Workflow

### Backend Development
1. Create/modify models in `Models/`
2. Create migrations: `dotnet ef migrations add MigrationName`
3. Update database: `dotnet ef database update`
4. Implement business logic in `Services/`
5. Expose endpoints via `Controllers/`
6. Run tests: `dotnet test`

### Frontend Development
1. Create/modify pages in `app/`
2. Build reusable components in `components/`
3. Use type-safe API client from `api/client.ts`
4. Update API types: `bun schema` (when backend changes)
5. Lint code: `bun lint`
6. Format code: `bun format`

## 🧪 Testing

### Backend Tests
Unit tests are located in `ezhire_api_unit_tests/`:
```bash
cd ezhire_api
dotnet test
```

## 🤝 Contributing

1. Follow the existing code structure and patterns
2. Maintain the layered architecture separation
3. Write unit tests for new features
4. Ensure TypeScript types are up to date
5. Follow C# and TypeScript best practices

## 📝 License

This project is part of a private repository. All rights reserved.

## 👥 User Roles

- **Hiring Manager**: Oversees recruitment campaigns and makes final hiring decisions
- **Recruiter**: Manages day-to-day recruitment activities, candidates, and interviews

## 🔗 Additional Resources

- [ASP.NET Core Documentation](https://docs.microsoft.com/aspnet/core)
- [Next.js Documentation](https://nextjs.org/docs)
- [Entity Framework Core](https://docs.microsoft.com/ef/core)
- [Tailwind CSS](https://tailwindcss.com/docs)
