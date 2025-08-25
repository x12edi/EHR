# EHR System  

A modular **Electronic Health Record (EHR)** platform built with:  
- **Backend**: ASP.NET Core (.NET 8), Entity Framework Core, Unit of Work + Repository Pattern  
- **Frontend**: React + TypeScript + Ant Design + Vite  
- **Database**: SQL Server  
- **Authentication**: ASP.NET Identity + JWT  
- **Architecture**: Clean architecture with DTOs, Services, Controllers, and layered separation  

## Features (Work in Progress)  

**Patient Management**  
- Create, edit, delete patients  
- Upload patient photos  
- Manage demographics, identifiers, and addresses  

**Appointments**  
- CRUD operations with search, sort, and pagination  
- Appointment calendar view (FullCalendar integration in progress)  

**Providers (Users + Clinician Profiles)**  
- Unified API to manage both Identity users and clinician profiles  
- Transaction-safe create/update/delete  

**Upcoming**  
- Encounters  
- Departments & Locations  
- Billing & Diagnostics  

## Tech Stack  

**Backend**  
- ASP.NET Core Web API  
- Entity Framework Core  
- AutoMapper  
- Repository & Unit of Work Pattern  
- SQL Server  

**Frontend**  
- React (Vite + TypeScript)  
- Ant Design UI library  
- Axios for API calls  
- FullCalendar (for scheduling view)  


